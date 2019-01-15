package com.lata.fsp.trackerterminal;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetSocketAddress;
import java.net.SocketTimeoutException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Hashtable;
import java.util.Vector;

import com.lata.fsp.runstate.IRunStateListener;
import com.lata.fsp.runstate.IRunStateService;
import com.lata.metra.latamasterterminal.xml.GeneratorHandler;

public class SendSocket extends TrackerThread implements IRunStateListener {
	
	private TrackerTerminal trackerTerminal;
	private IRunStateService irunStateService;
	private DatagramSocket datagramSocket;
	private InetSocketAddress inetSocketAddress;
	private String serviceIP = SERVICE_IP;
	private int sendPort = SEND_SOCKET_PORT;
	private String logFile = SEND_SOCKET_LOGFILE;
	private Vector msgQueue;
	private Object gpsMsglock = new Object();
	private Object msglock = new Object();
	private String gpsMsg;
	private boolean logging = false;
	private boolean sentRunstate = false;
	
	public SendSocket(TrackerTerminal trackerTerminal, IRunStateService irunStateService, String serviceIP, int sendPort) {
		logWrite("SendSocket serverIP " + serviceIP + " sendPort " + sendPort);
		this.trackerTerminal = trackerTerminal;
		this.irunStateService = irunStateService;
		this.irunStateService.addRunStateListener(this);
		String logValue = irunStateService.getState(LOG);
		setLogging(logValue);
		logWrite("SendSocket getState LOG " + LOG + " = " + logValue);
		this.serviceIP = serviceIP;
		this.sendPort = sendPort;
		this.msgQueue = new Vector();
	}
	
	public void run() {
		try {
			logWrite("run start");
			setActive(true);
			while (isActive()) {
				String xml = null;
				try {
					synchronized (gpsMsglock) {
						xml = gpsMsg;
						gpsMsg = null;
					}
					if (xml == null) {
						synchronized (msglock) {
							if (!msgQueue.isEmpty()) {
								xml = (String)msgQueue.firstElement();
								msgQueue.removeElement(xml);
							}
						}
					}
					if (xml != null) {
						if (!sendMessage(xml)) {
							logWrite("run unable to send message!!!");
						}
					}
					sleep(FIFTY_MILLIS);
				} catch (InterruptedException interruptedException) {
					logWrite("!@# Exception in run while active" + interruptedException.getMessage());
				}
			}
			logWrite("run stop");
		} catch (Throwable throwable) {
			logWrite("!@# Exception in run: " + throwable.getMessage());
		}
	}
	
	public void enqueueMessage(String xml){
		synchronized (msglock) {
			if (msgQueue.size() >= MAX_MSG_QUEUE_SIZE) {
				msgQueue.clear();
				logWrite("enqueueMessage clearing queue due to size >= " + MAX_MSG_QUEUE_SIZE);
			}
			msgQueue.add(xml);
			logWrite("enqueueMessage adding to queue " + xml);
		}
	}
	
	public void newGPSMessage(String xml) {
		synchronized(gpsMsglock){
			gpsMsg = xml;
		}
	}
	
	public boolean sendMessage(String xml){
		boolean sentStatus = false;
		try{
			if (isActive()) {
				if(datagramSocket == null || !haveConnection()) {
					logWrite ("sendMessage no connection, getting one");
					getConnection();	
				}
				if (datagramSocket != null && haveConnection()){
					logWrite ("sendMessage ACTIVE: connection is active...");

					if (xml != null) {
						byte[] data = xml.getBytes();
						try {
							DatagramPacket sendPacket = new DatagramPacket(data, data.length, inetSocketAddress);
							datagramSocket.send(sendPacket);
							sentStatus = true;
							logWrite("sendMessage xml: " + xml);
							logWrite("sendMessage sent length " + xml.length());
						} catch (SocketTimeoutException socketTimeoutException) {
							logWrite("sendMessage Socket Timeout: " + socketTimeoutException.getMessage());
							disconnect();
						}
						data = null;
					} else {
						logWrite(" sendMessage XML on _logQ.pull() was NULL!");
					}
				} else {
					logWrite("sendMessage Message lost via no connection: " + xml);
				}
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in sendMessage: " + throwable.getMessage());
			disconnect();
		}
		finally {
		}
		return sentStatus;
	}
	
	private boolean haveConnection() {
		boolean status = false;
		try {
			if(datagramSocket != null) {
				status = !datagramSocket.isClosed();
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in haveConnection: " + throwable.getMessage());
		}
		return status;
	}

	private void getConnection() {
		if (isActive()) {
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
				if (datagramSocket == null || inetSocketAddress == null) {
					logWrite("getConnection Reinitializing Socket for addr: " + serviceIP + " on port: "+ sendPort);
					datagramSocket = new DatagramSocket();
					inetSocketAddress = new InetSocketAddress(serviceIP, sendPort);
				}
				if (datagramSocket != null) {
					/*
					 * Send CarId to test connection
					 */
					String xml = trackerTerminal.generateStateMsg(XML_CARID, irunStateService.getState(XML_CARID));
					byte[] data = xml.getBytes();
					logWrite("getConnection sending a message using hostname:port " + inetSocketAddress.getHostName()+":"+inetSocketAddress.getPort());
					DatagramPacket sendPacket = new DatagramPacket(data, data.length, inetSocketAddress);
					datagramSocket.send(sendPacket);
					logWrite(new String(data).toString());
					logWrite("getConnection sent length " + xml.length());
					logWrite("getConnection sent xml " + xml);
					logWrite("getConnection(): Connection established!");
					/*
					 * Now queue sending run state -- only once until next boot
					 */
					if (!sentRunstate) {
						trackerTerminal.sendRunState();
						sentRunstate = true;
					}
				}
			}
			catch (SocketTimeoutException socketTimeoutException) {
				logWrite("getConnection Socket Timeout: " + socketTimeoutException.getMessage());
				disconnect();
			}
			catch (Throwable throwable) {
				String msg = throwable.getMessage();
				logWrite("!@# Exception in getConnection 2: " + msg);
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
	}
	
	private void disconnect() {
		try { 
			if (datagramSocket != null) { 
				datagramSocket.disconnect();
				datagramSocket.close(); 
				datagramSocket = null;
			}
			inetSocketAddress = null;
		} 
		catch(Throwable throwable) { 
			logWrite("!@# Exception in disconnect: " + throwable.getMessage());
		}
	}
	
	public void stopThread() {
		super.stopThread();
		logWrite("stopThread isActive = " + isActive());
		disconnect();
		this.irunStateService.removeRunStateListener(this);
		this.irunStateService = null;
	}
	
	private void setLogging(String value) {
		if (value.equalsIgnoreCase(TRUE)) {
			logging = true;
		} else {
			logging = false;
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

	public void loaded() {
	}

	public void newState(String key, String value) {
		if (key == null) {
			return;
		} else if (key.equalsIgnoreCase(LOG)) {
			logWrite("newState Receiving new LOG: " + value);
			setLogging(value);
		}
	}
}
