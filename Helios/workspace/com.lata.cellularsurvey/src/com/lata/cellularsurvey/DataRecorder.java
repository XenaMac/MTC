package com.lata.cellularsurvey;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Vector;

import com.esf.device.gps.service.IGPSService;
import com.lata.fsp.runstate.service.IRunStateService;

public class DataRecorder extends Thread implements CellularSurveyConstants {

  private static final String DEBUG_FILE = "DataRecorder.log";
  private volatile boolean active = true;
  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private NumberFormat nbrFormat = new DecimalFormat("#0.000");
  private IGPSService iGPSService = null;
  private IRunStateService iRunStateService = null;
  private boolean timeSet = false;
  private boolean debug = false;
  private long[] downFeeds = null;
  private long[] upFeeds = null;
  private String[] pings = null;
  private int modemCount = 0;
  private Vector modems;
  private String surveyFilepath;
  private UpdateIntervals updateIntervals;
  private long dataRecordMillis = 0;
  private int downloadUpdateSeconds = 0;
  private int uploadUpdateSeconds = 0;

  public DataRecorder(IGPSService iGPSService, IRunStateService iRunStateService, Vector modems, UpdateIntervals updateIntervals, boolean debug) {
    this.iGPSService = iGPSService;
    this.iRunStateService = iRunStateService;
    this.updateIntervals = updateIntervals;
    this.debug = debug;
    surveyFilepath = LOG_DIRECTORY + SURVEY_NAME 
        + (new SimpleDateFormat(SURVEY_NAME_FORMAT).format(new Date())) + LOG_EXTENSION;
    if (modems != null) {
      this.modems = modems;
      this.modemCount = modems.size();
    } else {
      writeDebugLog(",!!! Null modems");
    }
  }

  public void run() {
    writeDebugLog(",!!! DataRecorder starting");
    downFeeds = new long[modemCount];
    upFeeds = new long[modemCount];
    pings = new String[modemCount];
    dataRecordMillis = updateIntervals.getDataRecordSeconds() * ONE_SECOND;
    downloadUpdateSeconds = updateIntervals.getDownloadUpdateSeconds();
    uploadUpdateSeconds = updateIntervals.getUploadUpdateSeconds();
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
	  StringBuffer runstateSB = new StringBuffer();
	  for (int ii = 0; ii < modemCount; ii++) {
		  //TODO Get signal strength in dBm
		  runstateSB.append("-140" + "|");
		  feedDataStr = feedDataStr + "," + "-140";
		  double bitsXfrd = downFeeds[ii] * 8 / downloadUpdateSeconds;
		  String speedStr = "";
		  speedStr = nbrFormat.format(bitsXfrd / 1048576.0);
		  feedDataStr = feedDataStr + "," + speedStr;
		  runstateSB.append(speedStr + "|");
		  downFeeds[ii] = 0;
		  /*
		   * Bits transfered / # of seconds of data gathered
		   */
		  bitsXfrd = upFeeds[ii] * 8 / uploadUpdateSeconds;
		  speedStr = "";
		  speedStr = nbrFormat.format(bitsXfrd / 1048576.0);
		  feedDataStr = feedDataStr + "," + speedStr;
		  runstateSB.append(speedStr + "|");
		  upFeeds[ii] = 0;
		  if (pings[ii] == null) {
			  pings[ii] = PING_MAX_VALUE;
		  }
		  feedDataStr = feedDataStr + "," + pings[ii];
		  runstateSB.append(pings[ii]);
		  pings[ii] = PING_MAX_VALUE;
	  }
	  writeDebugLog(runstateSB.toString());
	  iRunStateService.setState(XML_CELLULAR_SURVEY, runstateSB.toString());
	  return feedDataStr;
  }

  private String getGPSData() {
    String gpsData = "";
    try {
      int gpsStatus = iGPSService.getStat();
      if (iGPSService == null) {
        gpsData = ",!!! Error null iGPSService.";
      } else  if(gpsStatus == 1) {
        gpsData = ",!!! No GPS data available";
      }
      else if(gpsStatus == 2) {
        gpsData = ",!!! Error in GPS response";
      }
      else if(gpsStatus != 4) {
        gpsData = ",!!! GPS status malfunction";
      } else {
        if (!timeSet) {
          if (iGPSService.setSystemTime()) {
            writeDebugLog(",!!! DataRecorder setting time from GPS");
            timeSet = true;
          }
        }
        gpsData = new String("," + iGPSService.getPacc() + ","
            + iGPSService.getLat() + "," + iGPSService.getLon() + ","
            + iGPSService.getAltitude() + "," + iGPSService.getSpeedMPH() + ","
            + iGPSService.getHeading());
      } 
    }
    catch(Exception ex) {
      gpsData = ",!!! Error in getGPSData: " + ex.toString();
    }
    return gpsData;
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
      for (int ii = 0; ii < modemCount; ii++) {
        Modem modem = (Modem) modems.elementAt(ii);
        headerStr = headerStr + "," + modem.getName() + " Down (Mbs)," 
        + modem.getName() + " Up (Mbs)," + modem.getName() + " Ping (Sec)";
      }
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

  public synchronized void saveBytesXfered(int index, int direction, long bytes) {
    if (index < modemCount) {
      if (direction == DOWN) {
        downFeeds[index] = bytes;
      } else if (direction == UP) {
        upFeeds[index] = bytes;
      }
    } else {
      writeDebugLog(",!!! Bad feed index");
    }
  }
  
  public synchronized void saveResponseTime(int index, String response) {
    if (index < modemCount) {
      pings[index] = response;
    } else {
      writeDebugLog(",!!! Bad feed index");
    }
  }

}
