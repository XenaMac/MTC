package com.eurotech.lata.logger;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.StringTokenizer;

public class Pinger extends Thread implements CellularSurveyConstants {

  private static final String LOG_FILE = "ping.log";
  private static final String PING_RESULTS_START = "rtt min/avg/max/mdev = ";
  private static final int STANDARD_OUT = 0;
  private static final int ERROR_OUT = 1;
  private static String PING_CMD = "ping";
  private static String PING_COUNT_ARG = " -c ";
  private static String PING_INTERFACE_ARG = " -I ";
  private volatile boolean active = true;
  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private NumberFormat nbrFormat = new DecimalFormat("#0.000");
  private String netIntrfc = null;
  private Ping ping = null;
  private DataRecorder dataRecorder = null;
  private String errorOut = "";
  private String standardOut = "";
  private boolean logging = false;
  private long pingUpdateMillis = 0;

  public Pinger(DataRecorder dataRecorder, String netIntrfc, Ping ping, int pingUpdateSeconds, boolean logging) {
    this.dataRecorder = dataRecorder;
    this.netIntrfc = netIntrfc;
    this.ping = ping;
    this.logging = logging;
    this.pingUpdateMillis = pingUpdateSeconds * ONE_SECOND;
  }

  public void run() {
    logWrite(" Pinger starting");
    while (active) {
      try {
        Runtime runtime = Runtime.getRuntime();
        String pingNetIntrfc = "";
        if (netIntrfc != null && netIntrfc.length() > 0) {
          pingNetIntrfc = PING_INTERFACE_ARG + netIntrfc;
        }
        String fullPingCmd = PING_CMD + PING_COUNT_ARG + ping.getCount() + pingNetIntrfc + " " + ping.getIpAddress();
        logWrite(" Pinger command " + fullPingCmd);
        Process process = runtime.exec(fullPingCmd);
        StreamHandler errorHandler = new StreamHandler(process.getErrorStream(), this, ERROR_OUT);
        StreamHandler outputHandler = new StreamHandler(process.getInputStream(), this, STANDARD_OUT); 
        errorHandler.start();
        outputHandler.start();
        logWrite(" Pinger process exitVal " + process.waitFor());
        try {
          errorHandler.join();
          outputHandler.join();
        } catch (InterruptedException except) {
          logWrite(" Pinger join InterruptedException " + except.toString());
        }
        String pingAverageStr = PING_MAX_VALUE;
        if (standardOut.length() > 0) {
          logWrite(" Pinger results " + standardOut);
          int index = standardOut.indexOf(PING_RESULTS_START)
              + PING_RESULTS_START.length();
          logWrite(" Pinger index " + index);
          String minAvMax = standardOut.substring(index);
          logWrite(" Pinger minAvMax " + minAvMax);
          StringTokenizer stringTokenizer = new StringTokenizer(minAvMax, "/");
          String firstToken = "";
          if (stringTokenizer.hasMoreTokens()) {
            firstToken = stringTokenizer.nextToken();
          }
          logWrite(" Pinger firstToken " + firstToken);
          if (stringTokenizer.hasMoreTokens()) {
        	  pingAverageStr = stringTokenizer.nextToken();
          }
          logWrite(" Pinger average response time " + pingAverageStr);
        }
        errorOut = "";
        standardOut = "";
        if (dataRecorder != null) {
        	try {
        		double pingAverage = new Double(pingAverageStr).doubleValue();
        		pingAverageStr = nbrFormat.format(pingAverage/1000.0);
        		dataRecorder.saveResponseTime(pingAverageStr);
        	} catch (Exception ex) {
        		logWrite(" Pinger error writing to data recorder");
        	}
        } else {
          logWrite(" Pinger error no data recorder");
        }
        try {
          Thread.sleep(pingUpdateMillis);
        } catch (InterruptedException except) {
          logWrite(" Pinger sleep InterruptedException " + except.toString());
        }
      } catch (Throwable thro) {
        logWrite(" Pinger error " + thro.toString());
      }
    }
    logWrite(" Pinger ending");
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
    logWrite(" Pinger stopping thread");
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
    logWrite(" Pinger setStreamOut type " + type + " " + streamOut);
  }
}

