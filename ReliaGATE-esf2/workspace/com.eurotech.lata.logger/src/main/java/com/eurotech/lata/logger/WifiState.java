package com.eurotech.lata.logger;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import com.lata.fsp.runstate.IRunStateService;

public class WifiState extends Thread implements CellularSurveyConstants {

	  private static final String LOG_FILE_DATE_FORMAT = "yyyyMMdd";
	  private static final String LOG_FILE = "wifistate.log";
	  private static final int STANDARD_OUT = 0;
	  private static final int ERROR_OUT = 1;
	  private static String WIFI_TAIL_CMD = "tail -n 1 ";
	  private volatile boolean active = true;
	  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
	  private IRunStateService iRunStateService = null;
	  private String errorOut = "";
	  private String standardOut = "";
	  private boolean logging = false;
	  private long wifiUpdateMillis = 0;
	  private String lastWifiState = "";
	  private String wifiClientValue = "0";
	  
	  public WifiState(IRunStateService iRunStateService, int wifiUpdateSeconds, boolean logging) {
		    this.iRunStateService = iRunStateService;
		    this.logging = logging;
		    this.wifiUpdateMillis = wifiUpdateSeconds * ONE_SECOND;
		  }
	  
	  public void run() {
		    logWrite(" WifiState starting");
		    while (active) {
		      try {
		        Runtime runtime = Runtime.getRuntime();
		        Date now = new Date();
				SimpleDateFormat nameFormat = new SimpleDateFormat(LOG_FILE_DATE_FORMAT);
				String logNameDate = nameFormat.format(now);
		        Process process = runtime.exec(WIFI_TAIL_CMD + LOG_DIRECTORY + WIFI_CLIENT_FILE_PREFIX +
		        		logNameDate + LOG_EXTENSION);
		        WifiStreamHandler errorHandler = new WifiStreamHandler(process.getErrorStream(), this, ERROR_OUT);
		        WifiStreamHandler outputHandler = new WifiStreamHandler(process.getInputStream(), this, STANDARD_OUT); 
		        errorHandler.start();
		        outputHandler.start();
		        logWrite(" WifiState process exitVal " + process.waitFor());
		        try {
		          errorHandler.join();
		          outputHandler.join();
		        } catch (InterruptedException except) {
		          logWrite(" WifiState join InterruptedException " + except.toString());
		        }
		        if (standardOut.length() > 0) {
		          logWrite(" WifiState results " + standardOut);
		          if (standardOut.trim().equals(lastWifiState)) {
		        	  if (wifiClientValue.equals("1")) {
		        		  wifiClientValue = "0";
		        		  iRunStateService.setState(XML_WIFI_STATE, wifiClientValue+"");
		        	  }
		          } else {
		        	  if (wifiClientValue.equals("0")) {
		        		  wifiClientValue = "1";
		        		  iRunStateService.setState(XML_WIFI_STATE, wifiClientValue+"");
		        	  }
		          }
		          lastWifiState = standardOut.trim();
		        }
		        if (errorOut.length() > 0) {
		        	logWrite(" WifiState error " + errorOut);
		        }
		        errorOut = "";
		        standardOut = "";
		        try {
		          Thread.sleep(wifiUpdateMillis);
		        } catch (InterruptedException except) {
		          logWrite(" WifiState sleep InterruptedException " + except.toString());
		        }
		      } catch (Throwable thro) {
		        logWrite(" WifiState error " + thro.toString());
		      }
		    }
		    logWrite(" WifiState ending");
		  }

		  public void logWrite(String msg) {
		    if (logging == true) {
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

		  public void stopThread() {
		    logWrite(" WifiState stopping thread");
		    active = false;
		  }

		  public synchronized void setStreamOut(String streamOut, int type) {
		    switch (type) {
		    case STANDARD_OUT:
		      this.standardOut = streamOut;
		      break;
		    case ERROR_OUT:
		      this.errorOut += streamOut;
		      break;
		    default:
		      break;
		    }
		    logWrite(" WifiState setStreamOut type " + type + " " + streamOut);
		  }
}
