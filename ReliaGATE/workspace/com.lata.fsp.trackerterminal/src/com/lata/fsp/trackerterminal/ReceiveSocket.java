package com.lata.fsp.trackerterminal;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

import com.lata.fsp.runstate.service.IRunStateListener;
import com.lata.fsp.runstate.service.IRunStateService;
import com.lata.metra.latamasterterminal.xml.CommandParserHandler;
import com.lata.metra.latamasterterminal.xml.GeneratorHandler;
import com.lata.metra.latamasterterminal.xml.OrderedParserHandler;
import com.lata.metra.latamasterterminal.xml.TagValuePair;

public class ReceiveSocket extends TrackerThread implements IRunStateListener {
	
	private TrackerTerminal trackerTerminal;
	private IRunStateService irunStateService;
	private DatagramSocket datagramSocket;
	private CommandParserHandler commandParser;
	private OrderedParserHandler orderedParser;
	private GeneratorHandler genHandler;
	private String serviceIP = SERVICE_IP;
	private int receivePort = RECEIVE_SOCKET_PORT;
	private boolean haveConnection = false;
	private String logFile = RECEIVE_SOCKET_LOGFILE;
	private boolean logging = false;
	
	public ReceiveSocket(TrackerTerminal trackerTerminal, IRunStateService irunStateService, String serviceIP, int receivePort) {
		logWrite("ReceiveSocket serverIP " + serviceIP + " receivePort " + receivePort);
		this.trackerTerminal = trackerTerminal;
		this.irunStateService = irunStateService;
		this.irunStateService.addRunStateListener(this);
		String logValue = irunStateService.getState(LOG);
		setLogging(logValue);
		logWrite("ReceiveSocket getState LOG " + LOG + " = " + logValue);
		this.serviceIP = serviceIP;
		this.receivePort = receivePort;
		commandParser = new CommandParserHandler();
		orderedParser = new OrderedParserHandler();
		genHandler = new GeneratorHandler();
	}
	
	public void run() {
		try {
			logWrite("run start");
			setActive(true);
			while (!haveConnection && isActive()) {
				getConnection();
			}
			if (isActive()) {
				while (haveConnection && isActive()) {
					try {
						byte[] data = new byte[2048];
						int bytes = 0;
						DatagramPacket datagramPacket = new DatagramPacket(data, data.length);
						logWrite("run ACTIVE: Listening with socket on port: " + datagramSocket.getLocalPort());
						datagramSocket.receive(datagramPacket);
						InetAddress inetAddress = datagramPacket.getAddress();
						if (!inetAddress.getHostAddress().equals(serviceIP)) {
							logWrite("run Received data from unknown IP (" + inetAddress.getHostAddress() + ")");
							continue;
						}
						String xml = new String(datagramPacket.getData(), 0, datagramPacket.getLength());
						bytes = datagramPacket.getLength();
						logWrite("read(" + bytes + "):");
						logWrite(xml);
						Hashtable request = commandParser.parse(xml);
						if(request.isEmpty()){
							logWrite("run Failed to parse message");
							continue;
						}
						processRequest(request, xml);
					} catch (Throwable throwable) {
						disconnect();
						haveConnection = false;
						if(!throwable.getMessage().startsWith("The operation timed out")) {
						logWrite("!@# SOCKET ERROR in run: " + throwable.getMessage());
						} else {
							logWrite("!@# Exception in run 1: " + throwable.getMessage());
						}
					}
				}
			}
			logWrite("run stop");
		} catch (Throwable throwable) {
			logWrite("!@# Exception in run 2: " + throwable.getMessage());
		}
		disconnect();
		setActive(false);
		logWrite("!@# Thread is dead");
	}
	
	private void processRequest(Hashtable request, String xml) {
		try {
			String command = (String) request.get(XML_COMMAND);
			logWrite("processRequest received command " + command);
			
			if (command.equals(CMD_ACK)) {
				logWrite("processRequest processed " + CMD_ACK);
				return;
			}
			if (command.equals(CMD_GET_GPS)) {
				String id = (String) request.get(XML_ID);
				/*trackerTerminal.enqueueMessage(generateAck(id));*/
				trackerTerminal.sendGPS(id);
				logWrite("processRequest processed " + CMD_GET_GPS);
				return;
			}
			if (command.equals(CMD_GET_STATE)) {
			/*if (command.equals(METRA_CMD_GET_STATE)) {*/
				/*trackerTerminal.enqueueMessage(generateAck((String) request.get(XML_ID)));*/
				String name = (String) request.get(XML_NAME);
				trackerTerminal.sendRunState();
				logWrite("processRequest processed " + CMD_GET_STATE + " " + name);
				return;
			}
			if (command.equals(CMD_GET_VAR)) {
				/*trackerTerminal.enqueueMessage(generateAck((String) request.get(XML_ID)));*/
				String name = (String) request.get(XML_NAME);
				trackerTerminal.enqueueMessage(trackerTerminal.generateStateMsg(name, irunStateService.getState(name)));
				logWrite("processRequest processed " + CMD_GET_VAR + " " + name);
				return;
			}
			if (command.equals(CMD_REBOOT)) {
				/*trackerTerminal.enqueueMessage(generateAck((String) request.get(XML_ID)));*/
				logWrite("processRequest processed " + CMD_REBOOT);
				trackerTerminal.reboot();
				return;
			}
			if (command.equals(CMD_SET_VAR)) {
				/*trackerTerminal.enqueueMessage(generateAck((String) request.get(XML_ID)));*/
				Vector vRequest = orderedParser.parse(xml);
				for (Enumeration enum = vRequest.elements(); enum.hasMoreElements(); ) {
					TagValuePair pair = (TagValuePair) enum.nextElement();
					String tag = pair.getTag();
					if (!tag.equals(XML_COMMAND) && !tag.equals(XML_ID) && !tag.equals(CMD_SET_VAR)) {
						String value = pair.getValue();
						irunStateService.setState(tag, value);
						logWrite("processRequest processed " + CMD_SET_VAR + " Name " + tag + " Value " + value);
					}
				}
				return;
			}
		}
		catch(Throwable throwable) { 
			logWrite("!@# Exception in processRequest: " + throwable.getMessage());
		}
	}

	private String generateAck(String id) {
		String response = "";
		try {
			Hashtable ack = new Hashtable();
			ack.put(XML_COMMAND, CMD_ACK);
			ack.put(XML_ID, id);
			response = genHandler.generate(ack);
		} 
		catch(Throwable throwable) { 
			logWrite("!@# Exception in generateAck " + throwable.getMessage());
		}
		return response;
	}
	
	private void disconnect() {
		try { 
			logWrite("disconnect closing Socket");
			if (datagramSocket != null) { 
				datagramSocket.disconnect();
				datagramSocket.close(); 
				datagramSocket = null;
			}
		} 
		catch(Throwable throwable) { 
			logWrite("!@# Exception in disconnect: " + throwable.getMessage());
		}
	}
	
	private void getConnection() {
		if (datagramSocket != null) {
			try {
				datagramSocket.close();
				datagramSocket = null;
			}
			catch (Throwable throwable) {
				logWrite("!@# Exception in getConnection 1: " + throwable.getMessage());
				datagramSocket = null;
			}
		}
		try {
			if (datagramSocket == null) {
				logWrite("getConnection Opening Socket on port: "+ receivePort);
				datagramSocket = new DatagramSocket(receivePort);
				datagramSocket.setSoTimeout(THIRTY_SECONDS);
			}
			if (datagramSocket != null) {
				haveConnection = true;
				logWrite("getConnection(): Connection established!");
			}
		}
		catch (Throwable throwable) {
			String msg = throwable.getMessage();
			logWrite("!@# Exception in getConnection: " + msg);
			disconnect();
			if (msg.indexOf("refused") != -1 || msg.indexOf("unreachable") != -1) {
				try {
					Thread.sleep(TEN_SECONDS);
				}
				catch (InterruptedException interruptedException) {
					
				}
			}
		}
	}
	
	public void stopThread() {
		try {
			super.stopThread();
			logWrite("stopThread isActive = " + isActive());
			disconnect();
			this.irunStateService.removeRunStateListener(this);
			this.irunStateService = null;
		}
		catch(Throwable throwable) { 
			logWrite("!@# Exception in stopThread: " + throwable.getMessage());
		}
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
	
	private void setLogging(String value) {
		try {
			if (value.equalsIgnoreCase(TRUE)) {
				logging = true;
			} else {
				logging = false;
			}
		}
		catch(Throwable throwable) { 
			logWrite("!@# Exception in setLogging: " + throwable.getMessage());
		}
	}
	
	public void newState(String key, String value) {
		try {
			if (key == null) {
				return;
			} else if (key.equalsIgnoreCase(LOG)) {
				logWrite("newState Receiving new LOG: " + value);
				setLogging(value);
			}
		}
		catch(Throwable throwable) { 
			logWrite("!@# Exception in newState: " + throwable.getMessage());
		}
	}

	public void loaded() {
	}
}
