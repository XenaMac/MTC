package com.lata.cellularsurvey;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Vector;

import com.esf.device.gps.service.IGPSService;
import com.lata.cellularsurvey.DataRecorder;
import com.lata.fsp.runstate.service.IRunStateService;

public class CellularSurvey extends Thread implements CellularSurveyConstants {

  private static final String DEBUG_TAG = "Debug";
  private static final String LOG_FILE = "cellularsurvey.log";
  private static FTPClient[] fTPUpClient;
  private static FTPClient[] fTPDownClient;
  private static Pinger[] pinger;
  private DataRecorder dataRecorder;
  private volatile boolean active = true;
  private int modemCount = 0;
  private File configFile = null;
  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private IGPSService iGPSService;
  private IRunStateService iRunStateService;
  private IfConfiger ifConfig;
  private boolean surveyRunning = false;
  private boolean debug = false;
  private UpdateIntervals updateIntervals;
  private Vector modems;

  public CellularSurvey() {
	    this.configFile = new File(MAIN_DIRECTORY + CONFIGURATION_FILE);
	    this.ifConfig = new IfConfiger();
}

/* (non-Javadoc)
   * @see java.lang.Thread#run()
   */
  public void run() {

    while (active) {
      while (!configFile.exists()) {
        if (surveyRunning) {
          writeDebugLog(" CellularSurvey stopping survey");
          stopSurvey();
          destroyConfigVars();
          surveyRunning = false;
        }
        try {
          Thread.sleep(ONE_SECOND);
        } catch (InterruptedException except) {

        }
      }
      if (!surveyRunning && configFile.exists()) {
        createConfigVars();
        loadConfigFile();
        startSurvey();
        surveyRunning = true;
      }
    }
    writeDebugLog(" CellularSurvey ending survey");
  }

  private void loadConfigFile() {
    String xml = new String();
    XmlParser xmlParser;
    try {
      BufferedReader input = new BufferedReader(new FileReader(configFile));
      try {
        String line = null;
        while ((line = input.readLine()) != null) {
          line = line.trim();
          xml += line;
          writeDebugLog(line);
          checkForDebugTag(line);
        }
        writeDebugLog(" CellularSurvey parsing config file");
        xmlParser = new XmlParser(updateIntervals, modems);
        xmlParser.parse(xml, debug);
      } finally {
        input.close();
        writeDebugLog(" CellularSurvey loading config file done");
        if (debug == true) {
          dumpConfig();
        }
      }
    } catch (IOException except) {
      writeErrorLog(" CellularSurvey error loading config file " + except.toString());
    }
  }

  private void checkForDebugTag(String line) {
    String xmlTagValue = "";
    String startXmlTag = DEBUG_TAG + ">";

    int indexStart = line.indexOf(startXmlTag);
    if (indexStart >= 0) {
      int intValueStart = indexStart + startXmlTag.length();
      int indexEnd = line.indexOf("</" + DEBUG_TAG);
      if (indexEnd >= 0) {
        try {
          xmlTagValue =  line.substring(intValueStart, indexEnd).trim();
          if (xmlTagValue != null && xmlTagValue.trim().length() > 0) {
            if (xmlTagValue.equalsIgnoreCase("ON") || 
                xmlTagValue.equalsIgnoreCase("TRUE") ||
                xmlTagValue.equalsIgnoreCase("T")) {
              debug = true;
            } else {
              debug = false;
            }
          }
        } catch (IndexOutOfBoundsException except) {
        }
      } else {
        /*
         * Malformed or no end tag or end tag not on same line
         */
      }
    } else {
      /*
       * Not Debug tag
       */
    }
  }

  private void dumpConfig() {
    try {
      writeDebugLog("CellularSurvey logging " + debug);
      if (updateIntervals == null) {
        writeDebugLog("CellularSurvey dumpConfig updateIntervals == null");
      } else {
        writeDebugLog(" DataRecordSeconds " + updateIntervals.getDataRecordSeconds());
        writeDebugLog(" DownloadUpdateSeconds " + updateIntervals.getDownloadUpdateSeconds());
        writeDebugLog(" UploadUpdateSeconds " + updateIntervals.getUploadUpdateSeconds());
        writeDebugLog(" PingUpdateSeconds " + updateIntervals.getPingUpdateSeconds());
      }
      if (modems == null) {
        writeDebugLog(" CellularSurvey dumpConfig modems == null");
      } else {
        int size = modems.size();
        writeDebugLog(" modems count: " + size);
        for (int ii = 0; ii < size; ii++) {
          writeDebugLog(" modems [" + ii + "]");
          Modem modem = (Modem) modems.elementAt(ii);
          if (modem == null) {
            writeDebugLog(" CellularSurvey dumpConfig modem == null");
          } else {
            writeDebugLog(" ModemName " + modem.getName());
            writeDebugLog(" Interface " + modem.getNetworkInterface());
            Transfer download = modem.getDownload();
            if (download == null || !download.isValid()) {
              writeDebugLog(" CellularSurvey dumpConfig download == null or not valid");
            } else {
              writeDebugLog(" FTPServer " + download.getFtpServer());
              writeDebugLog(" FTPServerUsername " + download.getFtpServerUsername());
              writeDebugLog(" FTPServerPassword " + download.getFtpServerPassword());
              writeDebugLog(" LocalFilePath " + download.getLocalFilePath());
              writeDebugLog(" RemoteFilePath " + download.getRemoteFilePath());
            }
            Transfer upload = modem.getUpload();
            if (upload == null || !upload.isValid()) {
              writeDebugLog(" CellularSurvey dumpConfig upload == null or not valid");
            } else {
              writeDebugLog(" FTPServer " + upload.getFtpServer());
              writeDebugLog(" FTPServerUsername " + upload.getFtpServerUsername());
              writeDebugLog(" FTPServerPassword " + upload.getFtpServerPassword());
              writeDebugLog(" LocalFilePath " + upload.getLocalFilePath());
              writeDebugLog(" RemoteFilePath " + upload.getRemoteFilePath());
            }
            Ping ping = modem.getPing();
            if (ping == null || !ping.isValid()) {
              writeDebugLog(" CellularSurvey dumpConfig ping == null or not valid");
            } else {
              writeDebugLog(" IPAddress " + ping.getIpAddress());
              writeDebugLog(" Count " + ping.getCount());
            }
          }
        }
      }
    } catch (Throwable except) {
      writeDebugLog(" CellularSurvey dumpConfig exception " + except.toString());
    }
  }

  public void createConfigVars() {
    writeDebugLog(" CellularSurvey creating config vars");
    updateIntervals = new UpdateIntervals();
    modems = new Vector();
  }

  public void destroyConfigVars() {
    writeDebugLog(" CellularSurvey destroying config vars");
    updateIntervals = null;
    modems = null;
  }

  public void startSurvey() {
    if (modems != null) {
      modemCount = modems.size();
      dataRecorder = new DataRecorder(iGPSService, iRunStateService, modems, updateIntervals, debug);
      if (modemCount > 0) {
        fTPUpClient = new FTPClient[modemCount];
        fTPDownClient = new FTPClient[modemCount];
        pinger = new Pinger[modemCount];
        for (int ii = 0; ii < modemCount; ii++) {
          Modem modem = (Modem) modems.elementAt(ii);
          if (modem != null) {
        	String netIntrfc = "";
            while (active) {
              netIntrfc = ifConfig.getInterfaceAddr(modem.getNetworkInterface(), debug);
              if (netIntrfc != null && netIntrfc.length() > 0) {
            	  break;
              } else {
                try {
            	  writeErrorLog(" CellularSurvey: Network interface " + modem.getNetworkInterface() + " not found");
				  sleep(FIVE_SECONDS);
			    } catch (InterruptedException e) {
			    }
              }
            }
            if (active) {
              Transfer download = modem.getDownload();
              if (download != null && download.isValid()) {
                fTPDownClient[ii] = new FTPClient(dataRecorder, ii, DOWN, netIntrfc, LOG_DIRECTORY, download, updateIntervals, debug);
                fTPDownClient[ii].start();
              } else {
                fTPDownClient[ii] = null;
              }
              Transfer upload = modem.getUpload();
              if (upload != null && upload.isValid()) {
                fTPUpClient[ii] = new FTPClient(dataRecorder, ii, UP, netIntrfc, LOG_DIRECTORY, upload, updateIntervals, debug);
                fTPUpClient[ii].start();
              } else {
                fTPUpClient[ii] = null;
              }
              Ping ping = modem.getPing();
              if (ping != null && ping.isValid()) {
                pinger[ii] = new Pinger(dataRecorder, ii, netIntrfc, ping, updateIntervals.getPingUpdateSeconds(), debug);
                pinger[ii].start();
              } else {
                pinger[ii] = null;
              }
            }
          }
        }
      }
      if (active) {
        dataRecorder.start();
        surveyRunning = true;
      }
    } else {
      writeErrorLog("CellularSurvey: null modems!");
    }
  }

  public void stopSurvey() {
    dataRecorder.stopThread();
    try {
      dataRecorder.join();
    } catch (InterruptedException ex) {
      writeDebugLog("  CellularSurvey problem joining dataRecorder thread " + ex.toString());
    }
    dataRecorder = null;
    int modemCount = modems.size();
    for (int ii = 0; ii < modemCount; ii++) {
      if (fTPDownClient[ii] != null) {
        fTPDownClient[ii].stopThread();
        try {
          fTPDownClient[ii].join();
        } catch (InterruptedException ex) {
          writeDebugLog(" CellularSurvey problem joining fTPDownClient " + ii + " thread " + ex.toString());
        }
        fTPDownClient[ii] = null;
      }
      if (fTPUpClient[ii] != null) {
        fTPUpClient[ii].stopThread();
        try {
          fTPUpClient[ii].join();
        } catch (InterruptedException ex) {
          writeDebugLog(" CellularSurvey problem joining fTPUpClient " + ii + " thread " + ex.toString());
        }
        fTPUpClient[ii] = null;
      }
      if (pinger[ii] != null) {
        pinger[ii].stopThread();
        try {
          pinger[ii].join();
        } catch (InterruptedException ex) {
          writeDebugLog(" CellularSurvey problem joining pinger " + ii + " thread " + ex.toString());
        }
        pinger[ii] = null;
      }
    }
  }

  public void stopThread() {
    writeDebugLog(" CellularSurvey stopping threads");
    active = false;
    stopSurvey();
  }

  public void writeDebugLog(String msg) {
    if (debug == true) {
      try {
        BufferedWriter writer =
            new BufferedWriter(new FileWriter(LOG_DIRECTORY + LOG_FILE, true));
        writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
        writer.close();
      }
      catch (IOException ioe) {
        System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + LOG_FILE + "\n" + ioe.getMessage());
      }
    }
  }
  
  public void writeErrorLog(String msg) {
      try {
        BufferedWriter writer =
            new BufferedWriter(new FileWriter(MAIN_DIRECTORY + ERROR_FILE, true));
        writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
        writer.close();
        writeDebugLog(msg);
      }
      catch (IOException ioe) {
        System.out.println("!@# Unable to write to log file: " + MAIN_DIRECTORY + ERROR_FILE + "\n" + ioe.getMessage());
      }
  }

  public void bind(IGPSService iGPSService, IRunStateService iRunStateService) {
	  this.iGPSService = iGPSService;
	  this.iRunStateService = iRunStateService;
  }
	public void unbind() {
		try {
			this.iGPSService = null;
			this.iRunStateService = null;
		} catch (Throwable throwable) {
		    writeDebugLog("!@# Exception in unbind" + throwable.getMessage());
		}
	}
}
