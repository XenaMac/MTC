package com.lata.fsp.trackerterminal;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Enumeration;
import java.util.Hashtable;

import org.eclipse.kura.position.PositionService;

import com.lata.fsp.runstate.IRunStateListener;
import com.lata.fsp.runstate.IRunStateService;
import com.lata.metra.latamasterterminal.xml.GeneratorHandler;

public class TrackerTerminal extends TrackerThread implements IRunStateListener {

	private PositionService igpsService;
	private IRunStateService irunStateService;
	private boolean newServerIP = false;
	private String logFile = TRACKER_TERMINAL_LOGFILE;
	private GPSUpdater gpsUpdater;
	private SendSocket sendSocket;
	private ReceiveSocket receiveSocket;
	GeneratorHandler genHandler;
	private String serviceIP = SERVICE_IP;
	private int sendSocketPort = SEND_SOCKET_PORT;
	private int receivePort = RECEIVE_SOCKET_PORT;
	private int testGPSRate = 15;
	private boolean logging = true;

	public TrackerTerminal() {
		genHandler = new GeneratorHandler();
	}

	public void run() {
		try {
			logWrite("run start");
			setActive(true);
			while (isActive()) {
				try {
					if(newServerIP){
						newServerIP = false;
						stopThreads();
					}
					startThreads();
					/*count = testStateVars(count);*/
					sleep(ONE_SECOND);
				} catch (InterruptedException interruptedException) {
					logWrite("!@# Exception in while active" + interruptedException.getMessage());
				}
			}
			logWrite("run stop");
		} catch (Throwable throwable) {
			logWrite("!@# Exception in run" + throwable.getMessage());
			setActive(false);
		}
	}


	
	private void startThreads() {
		try {
			if (isActive()) {
				if (sendSocket == null) {
					logWrite("startThreads sendSocket is null");
					stopSecondaryThreads();
					logWrite("startThreads starting sendSocket");
					sendSocket = new SendSocket(this, irunStateService, serviceIP, sendSocketPort);
					sendSocket.start();
				} else if (!sendSocket.isActive()){
					logWrite("startThreads sendSocket is not active");
					stopSecondaryThreads();
					try {
						sendSocket.join();
					} catch (InterruptedException interruptedException) {
					}
					logWrite("startThreads starting sendSocket");
					sendSocket = new SendSocket(this, irunStateService, serviceIP, sendSocketPort);
					sendSocket.start();
				}
				startSecondaryThreads();
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in startThreads" + throwable.getMessage());
		}
	}

	private void startSecondaryThreads() {
		try {
			if (isActive()) {
				if (receiveSocket == null) {
					logWrite("startSecondaryThreads starting receiveSocket");
					receiveSocket = new ReceiveSocket(this, irunStateService, serviceIP, receivePort);
					receiveSocket.start();
				} else if (!receiveSocket.isActive()) {
					logWrite("startSecondaryThreads receiveSocket is not active");
					try {
						receiveSocket.stopThread();
						receiveSocket.join();
					} catch (InterruptedException interruptedException) {
					}
					logWrite("startSecondaryThreads starting receiveSocket");
					receiveSocket = new ReceiveSocket(this, irunStateService, serviceIP, receivePort);
					receiveSocket.start();
				}
				if (gpsUpdater == null) {
					logWrite("startSecondaryThreads starting gpsUpdater");
					gpsUpdater = new GPSUpdater(igpsService, irunStateService, sendSocket);
					gpsUpdater.start();
				} else if (!gpsUpdater.isActive()) {
					logWrite("startSecondaryThreads gpsUpdater is not active");
					try {
						gpsUpdater.stopThread();
						gpsUpdater.join();
					} catch (InterruptedException interruptedException) {
					}
					logWrite("startSecondaryThreads starting gpsUpdater");
					gpsUpdater = new GPSUpdater(igpsService, irunStateService, sendSocket);
					gpsUpdater.start();
				}
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in startSecondaryThreads" + throwable.getMessage());
		}
	}

	private void stopSecondaryThreads() {
		try {
			logWrite("stopSecondaryThreads");
			if (receiveSocket != null) {
				logWrite("stopSecondaryThreads receiveSocket active without sendSocket, killing");
				try {
					receiveSocket.stopThread();
					receiveSocket.join();
				} catch (InterruptedException interruptedException) {
				}
				logWrite("stopSecondaryThreads receiveSocket now null");
				receiveSocket = null;
			}
			if (gpsUpdater != null) {
				logWrite("stopSecondaryThreads gpsUpdater active without sendSocket, killing");
				try {
					gpsUpdater.stopThread();
					gpsUpdater.join();
				} catch (InterruptedException interruptedException) {
				}
				logWrite("stopSecondaryThreads gpsUpdater now null");
				gpsUpdater = null;
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in stopSecondaryThreads" + throwable.getMessage());
		}
	}
	
	public void stopThread() {
		try {
			super.stopThread();
			logWrite("stopThread isActive = " + isActive());
			try {
				sendSocket.stopThread();
				sendSocket.join();
			} catch (InterruptedException interruptedException) {
			}
			stopSecondaryThreads();
		} catch (Throwable throwable) {
			logWrite("!@# Exception in stopThread" + throwable.getMessage());
		}
	}
	
	private void stopThreads() {
		try {
			logWrite("stopThreads");
			try {
				sendSocket.stopThread();
				sendSocket.join();
			} catch (InterruptedException interruptedException) {
			}
			stopSecondaryThreads();
		} catch (Throwable throwable) {
			logWrite("!@# Exception in stopThreads" + throwable.getMessage());
		}
	}

	public void bind(PositionService igpsService, IRunStateService irunStateService) {
		try {
			this.igpsService = igpsService;
			this.irunStateService = irunStateService;
			this.irunStateService.addRunStateListener(this);
			logWrite("bind irunStateService: " + irunStateService);
			
			String logValue = irunStateService.getState(LOG);
			setLogging(logValue);
			logWrite("bind getState LOG " + LOG + " = " + logValue);
			
			serviceIP = irunStateService.getState(SERVER_IP);
			logWrite("bind getState SERVER_IP " + SERVER_IP + " = " + serviceIP);
			
			try {
				sendSocketPort = new Integer(irunStateService.getState(CLIENT_PORT)).intValue();
			} catch (NumberFormatException except) {
				logWrite("Bad CLIENT_PORT " + CLIENT_PORT + " using default = " + sendSocketPort);
			}
			logWrite("bind getState CLIENT_PORT " + CLIENT_PORT + " = " + sendSocketPort);
			
			try {
				receivePort = new Integer(irunStateService.getState(SERVER_PORT)).intValue();
			} catch (NumberFormatException except) {
				logWrite("Bad SERVER_PORT " + SERVER_PORT + " using default = " + receivePort);
			}
			logWrite("bind getState SERVER_PORT " + SERVER_PORT + " = " + receivePort);
			
		} catch (Throwable throwable) {
			logWrite("!@# Exception in bind" + throwable.getMessage());
		}
	}

	public void unbind() {
		try {
			this.igpsService = null;
			this.irunStateService.removeRunStateListener(this);
			this.irunStateService = null;
		} catch (Throwable throwable) {
			logWrite("!@# Exception in unbind" + throwable.getMessage());
		}
	}
	
	public void newState(String key, String value) {
		try {
			if (key == null) {
				return;
			} else if (key.equalsIgnoreCase(XML_IP_HISTORY)) {
				logWrite("Receiving new IP Address: " + value);
				enqueueMessage(generateIpHistoryMsg(value));
			} else if (key.equalsIgnoreCase(SERVER_IP)) {
				logWrite("Receiving new Server IP: " + value + "  reinitializing sockets");
				/*
				 * The trackerterminal thread should revive the sockets with the new IP
				 */
				newServerIP = true;
			} else if (key.equalsIgnoreCase(LOG)) {
				logWrite("Receiving new LOG: " + value);
				setLogging(value);
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in newState" + throwable.getMessage());
		}
	}

	private void setLogging(String value) {
		try {
			if (value.equalsIgnoreCase(TRUE)) {
				logging = true;
			} else {
				logging = false;
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in newState" + throwable.getMessage());
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

	public void sendGPS(String Id){
		try {
			if(gpsUpdater!=null){
				Hashtable gpsData = gpsUpdater.getLatestLocation();
				if(!gpsData.containsKey("Command")){
					logWrite("sendGPS gpsData had lost its command.");
					gpsData.put("Command", "GPS");
				}
				gpsData.put("Id",Id);
				GeneratorHandler gh = new GeneratorHandler();
				sendSocket.enqueueMessage(gh.generate(gpsData));
			}else{
				logWrite("sendGPS: Not sending GPS because gpsUpdater was null");
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in sendGPS" + throwable.getMessage());
		}
	}
	
	public void enqueueMessage(String xml){
		try {
			sendSocket.enqueueMessage(xml);
			logWrite("enqueueMessage calling sendSocket enqueueMessage" + xml);
		} catch (Throwable throwable) {
			logWrite("!@# Exception in enqueueMessage" + throwable.getMessage());
		}
	}
	
	public void sendRunState() {
		try {
			logWrite("sendRunState");
			sendGPS( Sequencer.getInstance().getSequence() + "");
			Hashtable state = irunStateService.getStateHashtable();
			Enumeration enum1 = state.keys();
			while (enum1.hasMoreElements()) {
				String key = (String) enum1.nextElement();
				if (key.equalsIgnoreCase(XML_IP_HISTORY)) continue;
				String value = (String) state.get(key);
				Hashtable var = new Hashtable();
				var.put(XML_COMMAND, XML_STATE);
				var.put(XML_ID, Sequencer.getInstance().getSequence() + "");
				var.put(key, value);
				enqueueMessage(genHandler.generate(var));
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in sendRunState" + throwable.getMessage());
		}
	}
	
	public String generateStateMsg(String name, String value) {
		String response = "";
		try {
			if (name == null || value == null ) {
				logWrite("ERROR generateStateMsg name " + name + " value " + value);
			} else {
				Hashtable toxml = new Hashtable();
				toxml.put(XML_COMMAND, XML_STATE);
				toxml.put(XML_ID, Sequencer.getInstance().getSequence() + "");
				toxml.put(name, value);
				response = genHandler.generate(toxml);
			}
		} 
		catch(Throwable throwable) { 
			logWrite("!@# Exception in generateStateMsg " + throwable.getMessage());
		}
		return response;
	}
	
	public String generateIpHistoryMsg(String value) {
		String response = "";
		try {
			if (value == null ) {
				logWrite("ERROR generateIpHistoryMsg null IP Address");
			} else {
				Hashtable toxml = new Hashtable();
				toxml.put(XML_COMMAND, XML_IP_HISTORY);
				toxml.put(XML_ID, Sequencer.getInstance().getSequence() + "");
				toxml.put(XML_IP, value);
				response = genHandler.generate(toxml);
			}
		} 
		catch(Throwable throwable) { 
			logWrite("!@# Exception in generateIpHistoryMsg " + throwable.getMessage());
		}
		return response;
	}
	
	public void reboot() {
		logWrite("reboot REBOOTING SYSTEM!!!");
		try {
			stopThread();
			Runtime rt = Runtime.getRuntime();
			rt.exec("/sbin/reboot");
		} catch (Throwable throwable) {
			logWrite("!@# Exception in reboot unable to reboot " + throwable.getMessage());
		}
	}
	
	public void loaded() {
		logWrite("loaded");
		String value = irunStateService.getState(LOG);
		if (value != null) {
			logWrite("Receiving new LOG: " + value);
			setLogging(value);
		}
	}
	
	private int testStateVars(int count) {
		try {
			if (++count > 60 && count < 119) {
				if (testGPSRate == 15) {
					logWrite("testStateVars setting GPS_RATE to 1 count = " + count);
					irunStateService.setState(GPS_RATE, "30");
					testGPSRate = 30;
				}
			} else if (count == 119) {
				/*logWrite("testStateVars testing GetGPS message = " + count);
				sendGPS("9999");*/
				logWrite("testStateVars testing reboot = " + count);
				reboot();
			} else if (count > 119) {
				if (testGPSRate == 1) {
					logWrite("testStateVars setting GPS_RATE to 15 count = " + count);
					irunStateService.setState(GPS_RATE, "15");
					testGPSRate = 15;
				}
				count = 0;
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in enqueueMessage" + throwable.getMessage());
		}
		return count;
	}
}
