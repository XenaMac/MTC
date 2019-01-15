package com.lata.fsp.runstate;

import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Properties;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.FileInputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.Writer;
import java.io.OutputStreamWriter;
import java.util.Date;
import java.text.SimpleDateFormat;

import com.lata.fsp.runstate.service.IRunStateListener;
import com.lata.fsp.runstate.service.IRunStateService;

public class RunState extends Thread implements IRunStateService, RunStateConstants {

	private Object lock = new Object();
	private Hashtable state;
	private Hashtable influx;
	private Hashtable ftermConfigVars = new Hashtable();
	Hashtable resetRunstate;
	private String logFile = RUNSTATE_LOGFILE;
	private String propertiesFile;
	private volatile boolean active = true;
	private ConfigFile config;
	private boolean logging = false;
	private RunStateLoadedListener rsll = new RunStateLoadedListener();
	SimpleDateFormat dateFmt =
		new SimpleDateFormat(LOG_DATE_FORMAT);

	public RunState() {
		try {
			config = new ConfigFile(CONFIG_FILE);
			propertiesFile = PROPERTIES_FILE;
			logWrite("state serialize file: " + propertiesFile);
			load(propertiesFile);
			influx = new Hashtable();
		} catch (Throwable throwable) {
			logWrite("!@# Exception in RunState" + throwable.getMessage());
		}
	}

	public void run(){
		try {
			propogateValueToRunstate(SERVER_IP);
			propogateValueToRunstate(VERSION);
			propogateValueToRunstate(GPS_RATE);
			propogateValueToRunstate(SFTP_SERVER_IP);

			Hashtable changedValues = new Hashtable();
			while(active){
				synchronized(influx){
					synchronized (lock){
						Enumeration keys = influx.keys();
						while(keys.hasMoreElements()){
							String key = (String)keys.nextElement();
							String value = (String)influx.remove(key);
							if(value==null){
								logWrite("State value null, changing to empty string");
								value = "";
							}
							logWrite("attempting setState "+key+", "+value);
							key = put(key, value);
							if(key!=null)
								changedValues.put(key,value);
							//else the runstate did not contain key
						}
					}
				}
				if(!changedValues.isEmpty())
				{
					Enumeration keys = changedValues.keys();
					while(keys.hasMoreElements())
					{
						String key = (String)keys.nextElement();
						String value = (String)changedValues.remove(key);
						logWrite("Publishing state for key="+key+" value="+value);
						rsll.publishNewState(key, value);
					}
				}else{
					save(propertiesFile);
					synchronized(influx){
						try{
							influx.wait();
						}catch(InterruptedException x){}
					}
				}
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in run" + throwable.getMessage());
		}
	}

	private void propogateValueToRunstate(String key){
		try {
			String value =getConfigString(key);
			if(value !=null)
			{
				state.put(key, value);
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in propogateValueToRunstate" + throwable.getMessage());
		}
	}

	public void stopThread() {
		logWrite("stop() invoked...");
		active = false;
		this.interrupt();
	}

	public void dump() {
		try {
			synchronized (lock){
				Enumeration e = state.keys();
				System.out.println("dump(): displaying State->");
				String key = "";
				while (e.hasMoreElements()) {
					key = (String) e.nextElement();
					System.out.println("key: " + key);
					System.out.println("value: " + state.get(key));
				}
			}
		} catch (Exception e) {
			System.out.println("dump()->" + e.getMessage());
		}
	}

	private Hashtable load(String filename) {
		Properties props = new Properties ();
		try {
			logWrite("load(): reading state information from '" + filename
					+ "'.");
			if(new File(filename).exists()){
				FileInputStream fis = new FileInputStream(filename);
				props.load(fis);
				fis.close();
				logWrite("Loaded props from file: "+filename);
				return generateRunstate(props);
			}else{
				logWrite("Exception in load, File not found: "+filename);
				if(!filename.endsWith(".bckup")){
					return load(filename+".bckup");
				}
			}
		}catch (FileNotFoundException e){
			logWrite("Exception in load");
			logWrite(e.getMessage());
			if(!filename.endsWith(".bckup")){
				return load(filename+".bckup");
			}
		} catch (Throwable e) {
			logWrite("Exception in load");
			logWrite(e.getMessage());
			if(!filename.endsWith(".bckup")){
				return load(filename+".bckup");
			}
		}
		return generateRunstate(props);
	}

	private void save(String filename) {
		try {
			synchronized (lock){
				new File(filename).createNewFile();
				FileOutputStream fos = new FileOutputStream(filename);
				Object value = null, key = null;
				Properties props = new Properties();
				Enumeration keys = state.keys();
				while(keys.hasMoreElements()){
					key = (String)keys.nextElement();
					value =(String)state.get(key);
					if(key instanceof String && value instanceof String){
						props.setProperty((String)key, (String)value);
					}
				}
				props.store(fos, "LATA FTT RunState");
				fos.flush();
				fos.close();
				logWrite("saved properties in file "+filename);
			}
		} catch (FileNotFoundException e) {
			logWrite("Exception in save");
			e.printStackTrace();
		} catch (IOException e) {
			logWrite("Exception in save");
			e.printStackTrace();
		}
		if(!filename.endsWith(".bckup")){
			save(filename+".bckup");
		}
	}

	private Hashtable generateRunstate(Properties props) {
		logWrite("STATE VARIABLE: generateRunstate()...");
		synchronized (lock){
			if(props == null)
				props = new Properties();
			if(ftermConfigVars == null)
				ftermConfigVars = new Hashtable();

			state = getDefaultRunstate(); 

			boolean onTrip = false;
			if(props.containsKey("OnTrip"))
				onTrip = "true".equalsIgnoreCase(props.getProperty("OnTrip"));
			logWrite("STATE VARIABLE: checking for previous values...");
			Enumeration keys = state.keys();
			while(keys.hasMoreElements()){
				String key = (String)keys.nextElement();
				if(!onTrip	&&	("GPSEnable".equals(key) || "ModemEnable".equals(key))){
					logWrite("STATE VARIABLE: Not loading old value of "+key+" reverting to default of "+state.get(key));
				}else if(props.containsKey(key)){
					String value = props.getProperty(key);
					if("Elected".equals(key) && !"LST".equalsIgnoreCase(value)){
						value = "true";
					}
					if(value!=null){
						logWrite("STATE VARIABLE: Loaded varible "+key+" with previous value of "+value);
						state.put(key, value);
					}
				}else{
					logWrite("STATE VARIABLE: Missing variable "+key+" reverting to default of "+state.get(key));
				}
			}

			logWrite("STATE VARIABLE: loading mterm config vars...");
			loadMTermConfigStateVars();

			/*
			 * GPS send rates
			 */
		}
		logWrite("STATE VARIABLE: generateRunstate() completed.");
		return state;
	}

	private Hashtable getDefaultRunstate(){
		Hashtable defaultRunstate = new Hashtable();
		defaultRunstate.put(VERSION, OCTA_SERVICE_IP);
		defaultRunstate.put(CLIENT_PORT, DEFAULT_CLIENT_PORT);
		defaultRunstate.put(SERVER_PORT, DEFAULT_SERVER_PORT);
		defaultRunstate.put(SERVER_IP, OCTA_SERVICE_IP);
		defaultRunstate.put(CAR_ID, DEFAULT_CAR_ID);
		defaultRunstate.put(LOG, TRUE);
		defaultRunstate.put(SFTP_SERVER_IP, DEFAULT_SFTP_SERVER_IP);
		defaultRunstate.put(GPS_RATE, DEFAULT_GPS_RATE);
		defaultRunstate.put(XML_CELLULAR_SURVEY, "");
		return defaultRunstate;
	}

	public Object get(String key) {
		String value = "";
		if (key.equals("ServerPort") || key.equals("ClientPort")) {
			synchronized (lock){
				if(ftermConfigVars!=null)
				value = (String) ftermConfigVars.get(key);
			}
		}
		else {
			synchronized (lock){
				if(state!=null)
				value = (String) state.get(key);
			}
		}
		return value;
	}

	public Hashtable getStateHashtable() {
		synchronized (lock){
			return state;
		}
	}

	public Enumeration keys() {
		synchronized (lock){
			return state.keys();
		}
	}

	private void loadMTermConfigStateVars() {
		synchronized (lock){
			ftermConfigVars.put(SERVER_IP, getConfigString(SERVER_IP));
			ftermConfigVars.put(SFTP_SERVER_IP, getConfigString(SFTP_SERVER_IP));
			ftermConfigVars.put(VERSION, getConfigString(VERSION));
			ftermConfigVars.put(GPS_RATE, getConfigString(GPS_RATE));
			ftermConfigVars.put(SERVER_PORT, getConfigString(SERVER_PORT));
			ftermConfigVars.put(CLIENT_PORT, getConfigString(CLIENT_PORT));
		}
	}

	private void saveMTermConfigVar(String key, String value) {
		logWrite("saveMTermConfigVar: " + key + " = " + value);

		String contents = new String();
		String output = new String();
		Hashtable mTermConfig = new Hashtable();
		try {
			BufferedReader input = new BufferedReader(new FileReader(
					CONFIG_FILE));
			try {
				String line = null;
				while ((line = input.readLine()) != null) {
					contents += line.trim();
				}
				mTermConfig = new Hashtable();
				ConfigParserHandler cphandler = new ConfigParserHandler();
				mTermConfig = cphandler.parse(contents);
				logWrite("Setting "+key+" in mterm.config from RunState: "+value);
				mTermConfig.put(key, value);
			} finally {
				input.close();
			}

			output = "<Config>\n";
			Enumeration keys = mTermConfig.keys();
			while (keys.hasMoreElements()) {
				key = (String) keys.nextElement();
				String val = (String) mTermConfig.get(key);
				if(key.equals(SERVER_IP))
					logWrite("Saving ServerIP in RunState from mterm.config: "+val);
				output += "<" + key + ">" + val + "</" + key + ">\n";
			}
			output += "</Config>\n";

			Writer out = new OutputStreamWriter(new FileOutputStream(
					CONFIG_FILE));
			try {
				out.write(output);
			} finally {
				out.close();
			}
		} catch (IOException ex) {
			logWrite(ex.getMessage());
		}
	}

	private String put(String key, String value) {
		logWrite("put(): " + key + " = " + value);
		String stateKey = null;
		if(state.containsKey(key)){
			state.put(key, value);
			stateKey = key;
		}
		if (ftermConfigVars.containsKey(key)) {
			logWrite("Change to " + key + " is being serialized to mterm.config...");
			saveMTermConfigVar(key, value);
			ftermConfigVars.put(key, value);
			logWrite(key + " is serialized.");
			stateKey = key;
		}
		else
		{
			Enumeration keys = state.keys();
			while(keys.hasMoreElements()){
				stateKey = (String)keys.nextElement();
				if(stateKey.equalsIgnoreCase(key)){
					state.put(stateKey, value);
					break;
				}
			}
		}

		logWrite("STATE CHANGE: " + stateKey + " : " + value);
		return stateKey;
	}

	public String getConfigString(String name){
		synchronized (lock){
			return config.getConfigString(name);
		}
	}

	public int getConfigInt(String name){
		synchronized (lock){
			return config.getConfigInt(name);
		}
	}

	public void setState(String key, String value) {
		synchronized(influx){
			influx.put(key, value);
			influx.notify();
		}
		if (key.equalsIgnoreCase(LOG)) {
			logging = true;
		}
	}
	public void setStateValues(Hashtable newValues) {
		synchronized(influx){
			Enumeration keys = newValues.keys();
			while(keys.hasMoreElements())
			{
				Object key = keys.nextElement();
				influx.put(key, newValues.get(key));
			}
			influx.notify();
		}
	}
	
	public Hashtable getRunstateCopy(){
		Hashtable copy = new Hashtable();
		synchronized(lock){
			Enumeration keys = state.keys();
			while(keys.hasMoreElements())
			{
				Object key = keys.nextElement();
				copy.put(key, state.get(key));
			}			
		}
		return copy;
	}

	public String getState(String key) {
		return (String) get(key);
	}

	public void addRunStateListener(IRunStateListener listener){
		rsll.addListener(listener);
	}

	public void removeRunStateListener(IRunStateListener listener){
		rsll.removeListener(listener);
	}
	
	
	public void logWrite(String msg) {
		if (logging) {
			try {
				Date now = new Date();
				SimpleDateFormat nameFormat = new SimpleDateFormat(LOG_FILE_DATE_FORMAT);
				String logNameDate = nameFormat.format(now);
				SimpleDateFormat format = new SimpleDateFormat(LOG_DATE_FORMAT);
				String formatted = format.format(now);
				try {
					BufferedWriter writer = new BufferedWriter(new FileWriter(logFile + logNameDate + LOG_FILE_EXTENSION, true));
					writer.write(formatted + msg + NEWLINE);
					writer.close();
				}
				catch (IOException iOException) {
					System.out.println("!@# Unable to write to log file: " + logFile + logNameDate + LOG_FILE_EXTENSION + iOException.getMessage() + NEWLINE);
				}
			} catch (Throwable throwable) {
				System.out.println("!@# Exception in logWrite: "+ throwable.getMessage() + NEWLINE);
			}
		}
	}
}

