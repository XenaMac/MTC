package com.lata.fsp.runstate;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;

public class RunStateLoadedListener {

	private List _listeners;
	private boolean loaded = true;
	
	public RunStateLoadedListener() {

		super();
		/*logWrite("RunStateLoadedListener");*/
		_listeners = new ArrayList();
	}
	
	public synchronized void addListener(IRunStateListener listener) {
		/*logWrite("addListener " + listener.toString());*/
		_listeners.add(listener);
		if(loaded){
			listener.loaded();
		}
	}

	public synchronized void loaded() {
		/*logWrite("loaded");*/
		cleanListeners();
		for (Iterator i = _listeners.iterator(); i.hasNext();) {
			((IRunStateListener) i.next()).loaded();
		}
	}
	
	public synchronized void publishNewState(String key, String value){
		/*logWrite("publishNewState");*/
		cleanListeners();
		for (Iterator i = _listeners.iterator(); i.hasNext();){
			try{
			((IRunStateListener) i.next()).newState(key, value);
			/*logWrite("publishNewState _listeners i " + i.toString());*/
			}catch(Throwable x){
				/*logWrite("publishNewState exception " + x.getMessage());*/
			}
		}
	}
	
	public synchronized void removeListener(IRunStateListener listener) {
		/*logWrite("removeListener " + listener.toString());*/
		_listeners.remove(listener);
	}
	private synchronized void cleanListeners(){
		/*logWrite("cleanListeners");*/
		for (Iterator i = _listeners.iterator(); i.hasNext();){
			IRunStateListener listener =(IRunStateListener) i.next();
			if(listener==null) {
				/*logWrite("cleanListeners listener == null");*/
				_listeners.remove(listener);
				
			}
		}
	}
/*	public void logWrite(String msg) {
		try {
			Date now = new Date();
			SimpleDateFormat nameFormat = new SimpleDateFormat("yyyyMMdd");
			String logNameDate = nameFormat.format(now);
			SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss:SSS:");
			String formatted = format.format(now);
			String logFile = "/lata/logs/runstateloaded";
			try {
				BufferedWriter writer = new BufferedWriter(new FileWriter(logFile + logNameDate + ".log", true));
				writer.write(formatted + msg + "\n");
				writer.close();
			}
			catch (IOException iOException) {
				System.out.println("!@# Unable to write to log file: " + logFile + logNameDate + ".log" + iOException.getMessage() + "\n");
			}
		} catch (Throwable throwable) {
			System.out.println("!@# Exception in logWrite: "+ throwable.getMessage() + "\n");
		}
	}*/
}
