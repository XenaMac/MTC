package com.eurotech.lata.logger;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Vector;

import org.eclipse.kura.position.NmeaPosition;
import org.eclipse.kura.position.PositionService;
import com.lata.fsp.runstate.IRunStateService;

public class DataRecorder extends Thread implements CellularSurveyConstants {

	private static final String DEBUG_FILE = "DataRecorder.log";
	private volatile boolean active = true;
	private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
	private NumberFormat nbrFormat = new DecimalFormat("#0.000");
	private NumberFormat gpsFormat = new DecimalFormat("#0.00000");
	private PositionService iGPSService = null;
	private IRunStateService iRunStateService = null;
	private boolean timeSet = false;
	private boolean debug = false;
	private long downFeed = 0;
	private long upFeed = 0;
	private String ping = "";
	private int modemCount = 0;
	private Modem modem;
	private String surveyFilepath;
	private UpdateIntervals updateIntervals;
	private long dataRecordMillis = 0;

	private int m_cellularSignalLevel;

	public DataRecorder(PositionService iGPSService, IRunStateService iRunStateService, Modem modem, UpdateIntervals updateIntervals, boolean debug) {
		this.iGPSService = iGPSService;
	    this.iRunStateService = iRunStateService;
		this.updateIntervals = updateIntervals;
		this.debug = debug;
		surveyFilepath = LOG_DIRECTORY + SURVEY_NAME 
				+ (new SimpleDateFormat(SURVEY_NAME_FORMAT).format(new Date())) + LOG_EXTENSION;
		if (modem != null) {
			this.modem = modem;
		} else {
			writeDebugLog(",!!! Null modems");
		}
	}

	public void run() {
		writeDebugLog(",!!! DataRecorder starting");
		dataRecordMillis = updateIntervals.getDataRecordSeconds() * ONE_SECOND;
		writeDataHeader();
		while (active) {
			writeData(getGPSData() + getFeedData());
			try {
				Thread.sleep(dataRecordMillis);
			} catch (InterruptedException except) {

			}
		}
		writeDebugLog(",!!! DataRecorder stopping");
	}

	private String getFeedData() {
		String feedDataStr = "";
		try {
			StringBuffer runstateSB = new StringBuffer();
			runstateSB.append(m_cellularSignalLevel + "|");
			feedDataStr = feedDataStr + "," + m_cellularSignalLevel;
			double bitsXfrd;
			int downloadUpdateSeconds = updateIntervals.getDownloadUpdateSeconds();
			String speedStr = "";
			if (downloadUpdateSeconds > 0) {
				bitsXfrd = downFeed * 8 / downloadUpdateSeconds;
				speedStr = nbrFormat.format(bitsXfrd / 1048576.0);
			} else {
				/*
				 * Data received usage
				 */
				speedStr = nbrFormat.format(downFeed / 1048576.0);
			}
			feedDataStr = feedDataStr + "," + speedStr;
			runstateSB.append(speedStr + "|");
			downFeed = 0;
			/*
			 * Bits transfered / # of seconds of data gathered
			 */
			int uploadUpdateSeconds = updateIntervals.getUploadUpdateSeconds();
			if (uploadUpdateSeconds > 0) {
				bitsXfrd = upFeed * 8 / uploadUpdateSeconds;
				speedStr = "";
				speedStr = nbrFormat.format(bitsXfrd / 1048576.0);
			} else {
				/*
				 * Data transmitted usage
				 */
				speedStr = nbrFormat.format(upFeed / 1048576.0);
			}
			feedDataStr = feedDataStr + "," + speedStr;
			runstateSB.append(speedStr + "|");
			upFeed = 0;
			if (updateIntervals.getPingUpdateSeconds() > 0) {
				if (ping == null) {
					ping = PING_MAX_VALUE;
				}
			} else {
				/*
				 * Total Data transmitted usage
				 */
				ping = nbrFormat.format(new Double(iRunStateService.getState(XML_DATA_USAGE)).doubleValue());
			}
			feedDataStr = feedDataStr + "," + ping;
			runstateSB.append(ping);
			ping = PING_MAX_VALUE;
			writeDebugLog("," + runstateSB.toString());
			iRunStateService.setState(XML_CELLULAR_SURVEY_VALUES, runstateSB.toString());
		}
		catch (Exception oe) {
			writeDebugLog(",!!! getFeedData exception " + oe);
		}
		return feedDataStr;
	}

	private String getGPSData() {
		String gpsData = "";
		try {

			if (iGPSService != null) {

				NmeaPosition position = iGPSService.getNmeaPosition();
				double pdop = position.getPDOP();
				double lat = position.getLatitude();
				double lon = position.getLongitude();
				if (!iGPSService.isLocked()) {
					pdop = BAD_PDOP;
					lat = addWander(lat);
					lon = addWander(lon);
				}
				gpsData = new String("," + pdop + ","
						+ gpsFormat.format(lat) + "," + gpsFormat.format(lon) + ","
						+ nbrFormat.format(position.getAltitude() * METERS_TO_FEET) + "," + nbrFormat.format(position.getSpeedMph()) + ","
						+ nbrFormat.format(position.getTrack()));
			} else {
				gpsData = ",!!! Error null iGPSService.";
			}
		}
		catch(Exception ex) {
			gpsData = ",!!! Error in getGPSData: " + ex.toString();
		}
		return gpsData;
	}
	private double addWander(double inValue) {
		double randomNumber = Math.random();
		return inValue + ((0.5 - (randomNumber < 0.01 ? 0.01 : randomNumber))/500.0);
	}
	public void writeData(String msg) {
		try {
			BufferedWriter writer =
					new BufferedWriter(new FileWriter(surveyFilepath, true));
			writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
			writer.close();
		}
		catch (IOException ioe) {
			System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + DEBUG_FILE + "\n" + ioe.getMessage());
		}
	}

	public void writeDebugLog (String msg) {
		if (debug == true) {
			try {
				BufferedWriter writer =
						new BufferedWriter(new FileWriter(LOG_DIRECTORY + DEBUG_FILE, true));
				writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
				writer.close();
			}
			catch (IOException ioe) {
				System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + DEBUG_FILE + "\n" + ioe.getMessage());
			}
		}
	}

	public void writeDataHeader() {
		try {
			BufferedWriter writer =
					new BufferedWriter(new FileWriter(surveyFilepath, true));
			String headerStr = "Date-Time,DOP,Latitude,Longitude,Altitude (ft),Speed (mph),Heading (deg)";
			headerStr = headerStr + "," + modem.getName() + " SS (dBm)," + modem.getName() + " Down (Mbs)," + modem.getName() + " Up (Mbs)," + modem.getName() + " Ping (Sec)";
			writer.write(headerStr + "\n");
			writer.close();
		}
		catch (IOException ioe) {
			System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + DEBUG_FILE + "\n" + ioe.getMessage());
		}
	}

	public void stopThread() {
		writeDebugLog(",!!! DataRecorder stopping thread");
		active = false;
	}

	public synchronized void saveBytesXfered(int direction, long bytes) {
		if (direction == DOWN) {
			downFeed = bytes;
		} else if (direction == UP) {
			upFeed = bytes;
		}
	}

	public synchronized void saveResponseTime(String response) {
		ping = response;
	}

	public void setCellularSignalLevel(int signalLevel) {
		m_cellularSignalLevel = signalLevel;
	}

}

