package com.lata.fsp.trackerterminal;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Hashtable;

import com.esf.device.gps.service.IGPSService;
import com.lata.fsp.runstate.service.IRunStateListener;
import com.lata.fsp.runstate.service.IRunStateService;
import com.lata.metra.latamasterterminal.xml.GeneratorHandler;

public class GPSUpdater extends TrackerThread implements IRunStateListener {
	


	private boolean timeSet = false;
	private IGPSService iGPSService;
	private IRunStateService irunStateService;
	private SendSocket sendSocket;
	private DateFormat dateFormat = new SimpleDateFormat(GPS_DATE_FORMAT);
	private String logFile = GPS_UPDATER_LOGFILE;
	private String historyFile = GPS_HISTORY_LOGFILE;
	private String cellSurveyData = null;
	private Object gpsLock = new Object();
	private long gpsRateMillis = FIFTEEN_SECONDS;
	private long gpsUpdateTime = 0;
	private long cellSurveyUpdateTime = 0;
	private long badGpsTime = 0;
	private boolean logging = false;
	private int maxSpeed = 0;
	private float maxSpeedLat = 0;
	private float maxSpeedLon = 0;
	private int lastSpeed = 0;
	private float lastLat = 0;
	private float lastLon = 0;
	private int lastAlt = 0;
	private int lastHead = 0;

	public GPSUpdater(IGPSService iGPSService, IRunStateService irunStateService, SendSocket sendSocket) {
		this.iGPSService = iGPSService;
		this.irunStateService = irunStateService;
		this.irunStateService.addRunStateListener(this);
		String logValue = irunStateService.getState(LOG);
		setLogging(logValue);
		logWrite("GPSUpdater getState LOG " + LOG + " = " + logValue);
		this.sendSocket = sendSocket;
		setGpsRateInt(irunStateService.getState(GPS_RATE));
		logWrite("GPSUpdater getState GPS_RATE (millis) = " + gpsRateMillis);
	}
	
	private void setGpsRateInt(String inRateString) {
		try {
			gpsRateMillis = new Integer(inRateString).intValue() * 1000;
		} catch (NumberFormatException e) {
			gpsRateMillis = FIFTEEN_SECONDS;
		}
		logWrite("setGpsRateInt gpsRateMillis = " + gpsRateMillis);
	}

	public void run() {
		try {
			setActive(true);
			Hashtable gpsHashtable;
			while (isActive()) {
				try {
					synchronized(gpsLock){
						gpsHashtable = getGPSData();
					}
					if (gpsHashtable != null && ifGpsUpdateTime()) {
						gpsHashtable.put("Id", Sequencer.getInstance().getSequence() + "");
						GeneratorHandler genHandler = new GeneratorHandler();
						String xml = genHandler.generate(gpsHashtable);
						sendSocket.newGPSMessage(xml);
						setGpsUpdateTime();
						clearMaxSpeed();
					}
					sleep(ONE_SECOND);
				} catch (InterruptedException interruptedException) {
					logWrite("!@# Exception in run while active " + interruptedException.getMessage());
				}
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in run " + throwable.getMessage());
			setActive(false);
		}
	}

	private boolean ifGpsUpdateTime() {
		boolean status = false;
		if (Math.abs(System.currentTimeMillis() - gpsUpdateTime) > gpsRateMillis) {
			status = true;
		}
		return status;
	}
	private boolean isPastBadGpsTime() {
		boolean status = false;
		if (Math.abs(System.currentTimeMillis() - badGpsTime) > FIVE_SECONDS) {
			status = true;
		}
		return status;
	}
	private boolean isCellSurveyDataExpired() {
		boolean status = false;
		if (Math.abs(System.currentTimeMillis() - cellSurveyUpdateTime) > TEN_SECONDS) {
			status = true;
		}
		return status;
	}
	private void setGpsUpdateTime() {
		gpsUpdateTime = System.currentTimeMillis();
	}

	private void setBadGpsTime() {
		badGpsTime = System.currentTimeMillis();
	}
	
	private void setCellSurveyUpdateTime() {
		cellSurveyUpdateTime = System.currentTimeMillis();
	}
	
	public Hashtable getGPSData() {
		Hashtable gpsData = null;
		try {
			if (isActive()) {
				gpsData = new Hashtable();
				if (iGPSService == null) {
					throw new Exception("Error, null _gpsservice in GPSUpdater");
				}
				gpsData.put("Command", "GPS");
				Date date = new Date();
				gpsData.put("Time", dateFormat.format(date));
				int gpsStatus = iGPSService.getStat();
				if (gpsStatus == 1) {
					gpsData.put("Status", "No Signal");
				}
				else if(gpsStatus == 2) {
					gpsData.put("Status", "Error");
				}
				else if(gpsStatus == 4) {
					gpsData.put("Status", "Valid");
					if (!timeSet) {
						if (iGPSService.setSystemTime()) {
							logWrite("getGPSData setting time from GPS");
							timeSet = true;
						}
					}
				}
				else {
					gpsData.put("Status", "Failure");
				}
				int speed = Math.round(iGPSService.getSpeedMPH());
				float lat = iGPSService.getLat();
				float lon = iGPSService.getLon();
				int dop = iGPSService.getPacc();
				int alt = Math.round(iGPSService.getAltitude());
				int head = iGPSService.getHeading();
				if ((dop < MAX_DOP) && (gpsStatus == 4)) {
					if (dop <= GOOD_DOP  && isPastBadGpsTime()) {
						checkMaxSpeed(speed, lat, lon);
						lastSpeed = speed;
						lastLat = lat;
						lastLon = lon;
						lastAlt = alt;
						lastHead = head;
					}
				} else {
					setBadGpsTime();
					lastSpeed = 0;
				}
				gpsData.put("DOP", dop + "");
				gpsData.put("Speed", lastSpeed + "");
				gpsData.put("Lat", lastLat + "");
				gpsData.put("Lon", lastLon + "");
				gpsData.put("Alt", lastAlt + "");
				gpsData.put("Head", lastHead + "");
				gpsData.put("MaxSpd", getMaxSpeed() + "");
				gpsData.put("MLat", getMaxSpeedLat() + "");
				gpsData.put("MLon", getMaxSpeedLon() + "");
				if (cellSurveyData != null && !isCellSurveyDataExpired()) {
					gpsData.put(XML_CELLULAR_SURVEY, cellSurveyData);
				} else {
					gpsData.put(XML_CELLULAR_SURVEY, "0");
				}
				historyWrite("," + dop + "," + lat + "," + lon + "," + speed + ","  
						+ head + "," + alt + "," + gpsStatus);

			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in getGPSData " + throwable.getMessage());
			gpsData = null;
		}
		return gpsData;
	}
	
	private float getMaxSpeedLon() {
		return maxSpeedLon;
	}

	private float getMaxSpeedLat() {
		return maxSpeedLat;
	}

	private int getMaxSpeed() {
		return maxSpeed;
	}

	private void checkMaxSpeed(int speed, float lat, float lon) {
		if (speed >= maxSpeed) {
			maxSpeed = speed;
			maxSpeedLat = lat;
			maxSpeedLon = lon;
		}
	}
	
	private void clearMaxSpeed() {
			maxSpeed = 0;
			maxSpeedLat = 0;
			maxSpeedLon = 0;
	}
	
	public void stopThread() {
		super.stopThread();
		logWrite("stopThread isActive = " + isActive());
		this.irunStateService.removeRunStateListener(this);
		this.irunStateService = null;
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

	public void historyWrite(String msg) {
		try {
			Date now = new Date();
			SimpleDateFormat nameFormat = new SimpleDateFormat(LOG_FILE_DATE_FORMAT);
			String logNameDate = nameFormat.format(now);
			SimpleDateFormat format = new SimpleDateFormat(LOG_DATE_FORMAT);
			String formatted = format.format(now);
			try {
				BufferedWriter writer = new BufferedWriter(new FileWriter(historyFile + logNameDate + LOG_FILE_EXTENSION, true));
				writer.write(formatted + msg + NEWLINE);
				writer.close();
			}
			catch (IOException iOException) {
				System.out.println("!@# Unable to write to gps history file " + historyFile + logNameDate + LOG_FILE_EXTENSION + iOException.getMessage() + NEWLINE);
			}
		} catch (Throwable throwable) {
			System.out.println("!@# Exception in historyWrite "+ throwable.getMessage() + NEWLINE);
		}
	}
	
	public void loaded() {	
	}

	public void newState(String key, String value) {
		if (key == null) {
			return;
		} else if (key.equalsIgnoreCase(XML_CELLULAR_SURVEY)) {
			logWrite("newState Receiving new Cell Survey reading: " + value);
			setCellularSurvey(value);
		} else if (key.equalsIgnoreCase(GPS_RATE)) {
			logWrite("newState Receiving new GPS Rate: " + value);
			setGpsRateInt(value);
		} else if (key.equalsIgnoreCase(LOG)) {
			logWrite("newState Receiving new LOG: " + value);
			setLogging(value);
		}
	}
	
	private void setLogging(String value) {
		if (value.equalsIgnoreCase(TRUE)) {
			logging = true;
		} else {
			logging = false;
		}
	}
	
	private void setCellularSurvey(String cellSurveyStr) {
		if (cellSurveyStr != null && cellSurveyStr.length() > 0) {
			cellSurveyData = cellSurveyStr;
			setCellSurveyUpdateTime();
		} else {
			cellSurveyData = null;
		}
		logWrite("setCellularSurvey cellSurveyData = " + cellSurveyStr);
	}
	
	public Hashtable getLatestLocation(){
		Hashtable gpsData;
		synchronized(gpsLock){
			gpsData = getGPSData();
		}
		return gpsData;
	}
}
