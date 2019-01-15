package com.lata.fsp.trackerterminal;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Hashtable;

import org.eclipse.kura.position.NmeaPosition;
import org.eclipse.kura.position.PositionService;
import com.lata.fsp.runstate.IRunStateListener;
import com.lata.fsp.runstate.IRunStateService;
import com.lata.metra.latamasterterminal.xml.GeneratorHandler;

public class GPSUpdater extends TrackerThread implements IRunStateListener {
	
	public static final double METERS_TO_FEET = 3.280839895;
	private boolean timeSet = false;
	private PositionService iGPSService;
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
	private long lastWatchdog = System.currentTimeMillis();
	private boolean logging = false;
	private int maxSpeed = 0;
	private float maxSpeedLat = 0;
	private float maxSpeedLon = 0;
	private int lastSpeed = 0;
	private float lastLat = 0;
	private float lastLon = 0;
	private int lastAlt = 0;
	private int lastHead = 0;
	private int[] speedList;
	private float runningTotal = 0;
	private int speedCount = SPEED_SAMPLE;
	private int prevSpeed = 0;

	public GPSUpdater(PositionService iGPSService, IRunStateService irunStateService, SendSocket sendSocket) {
		this.iGPSService = iGPSService;
		this.irunStateService = irunStateService;
		this.irunStateService.addRunStateListener(this);
		String logValue = irunStateService.getState(LOG);
		setLogging(logValue);
		logWrite("GPSUpdater getState LOG " + LOG + " = " + logValue);
		this.sendSocket = sendSocket;
		setGpsRateInt(irunStateService.getState(GPS_RATE));
		logWrite("GPSUpdater getState GPS_RATE (millis) = " + gpsRateMillis);
		initMovingAverage();
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
					long now = System.currentTimeMillis();
					if (Math.abs(now - lastWatchdog) > FIFTEEN_SECONDS) {
						logWrite("GPS is alive (enabled=true)...");
						watchDogWrite("GPS is alive (enabled=true)...");
						lastWatchdog = now;
					}
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
	
	public static final double D2R = Math.PI/180.0;
	public static final double R2D = 180.0/Math.PI;
	private int getHeading(double lat1, double long1, double lat2, double long2) {
	    double diff_lat, diff_long;
	    double degree;

	    try {
			diff_long = (((long2*1000000)-(long1*1000000))/1000000) * D2R;
			diff_lat = (((lat2*1000000)-(lat1*1000000))/1000000) * D2R;     

			degree = R2D * (Math.atan2(Math.sin(diff_long) * Math.cos(D2R*lat2),
					Math.cos(D2R*lat1) * Math.sin(D2R*lat2) - Math.sin(D2R*lat1) *
					Math.cos(D2R*lat2) * Math.cos(diff_long)));

			if (degree >= 0) {
			    return (int) Math.round(degree);
			} else {
			    return (int) Math.round(360+degree);
			}
		} catch (Throwable throwable) {
			logWrite("!@# Exception in getHeading " + throwable.getMessage());
			return 0;
		}                                                                 
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
				NmeaPosition position = iGPSService.getNmeaPosition();
				int speed = getMovingAverage((int) Math.round(position.getSpeedMph()));
				float lat = (float) position.getLatitude();
				float lon = (float) position.getLongitude();
				float dop = (float) position.getPDOP();
				if (iGPSService.isLocked()) {
					gpsData.put("Status", "Valid");
				} else {
					gpsData.put("Status", "No GPS");
					dop = MAX_DOP;
				}
				int alt = (int) Math.round(position.getAltitude() * METERS_TO_FEET);
				int headNmea = (int) Math.round(position.getTrack());
				int head = lastHead;
				if ((dop < MAX_DOP)) {
					if (dop <= GOOD_DOP  && isPastBadGpsTime()) {
						head = getHeading(lastLat, lastLon, lat, lon);
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
					initMovingAverage();
					prevSpeed = 0;
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
						+ prevSpeed + "," + head + "," + headNmea + "," + alt);

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
	
	private void initMovingAverage() {
		speedList = new int[speedCount];
        for (int ii = 0; ii < speedCount; ii++) {
			speedList[ii] = 0;
		}
        runningTotal = 0;
	}
	
	private int getMovingAverage(int newSpeed) {
		try {
			if ((newSpeed - prevSpeed) <= getMaxAcceleration(newSpeed)) {
				runningTotal -= speedList[0];
				for (int ii = 0; ii < (speedCount - 1); ii++) {
					speedList[ii] = speedList[ii + 1];
				}
				speedList[(speedCount - 1)] = newSpeed;
				runningTotal += newSpeed;

			}
			int averageSpeed = Math.round(runningTotal / speedCount);
			if (averageSpeed > 120) {
				logWrite("Alert! Possible calculation error: " + averageSpeed);
			}
			prevSpeed = newSpeed;
			return averageSpeed;
		} catch (Throwable throwable) {
			logWrite("!@# Exception in getMovingAverage " + throwable.getMessage());
			return 0;
		}
	}
	
	private int getMaxAcceleration(int speed) {
		int maxAcceleration = 10;
		if (speed > 60) {
			maxAcceleration = 2;
		} else if (speed > 45) {
			maxAcceleration = 3;
		} else if (speed > 40) {
			maxAcceleration = 4;
		} else if (speed > 35) {
			maxAcceleration = 5;
		} else if (speed > 25) {
			maxAcceleration = 6;
		} else if (speed > 20) {
			maxAcceleration = 8;
		}
		return maxAcceleration;
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
		logWrite("loaded");
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

	public void watchDogWrite(String msg) {
		Date now = new Date();
		SimpleDateFormat format = new SimpleDateFormat(LOG_DATE_FORMAT);
		try {
			String formatted = format.format(now);
			BufferedWriter writer =
				new BufferedWriter(new FileWriter(WATCH_DOG_FILE, false));
			writer.write(formatted + msg + "\n");
			writer.close();
		}
		catch (IOException ioe) {
			logWrite("!@# Unable to write to log file: " + WATCH_DOG_FILE + "\n" + ioe.getMessage());
		}
	}
}
