package com.lata.fsp.mtc;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class IfConfiger implements CellularSurveyConstants {
	  
	  private static final String LOG_FILE = "ifConfig.log";
	  private static String IFCONFIG_CMD = "ifconfig";
	  private static final String IPV4_ADDRESS_START = "inet addr:";
	  private static final String IPV4_ADDRESS_END = " ";
	  private static final int STANDARD_OUT = 0;
	  private static final int ERROR_OUT = 1;
	  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
	  private String errorOut = "";
	  private String standardOut = "";
	  private boolean logging = false;

	  public String getInterfaceAddr(String netIntrfc, boolean logging) {

	    if (netIntrfc == null || netIntrfc.length() == 0) {
	      logWrite(" IfConfiger netIntrfc null or zero length ");
	      return "";
	    }
	    this.logging = logging;
	    String ipAddress = "";
	    try {
	      Runtime runtime = Runtime.getRuntime();
	      Process process = runtime.exec(IFCONFIG_CMD);
	      IfConfigStreamHandler errorHandler = new IfConfigStreamHandler(process.getErrorStream(), this, ERROR_OUT);
	      IfConfigStreamHandler outputHandler = new IfConfigStreamHandler(process.getInputStream(), this, STANDARD_OUT); 
	      errorHandler.start();
	      outputHandler.start();
	      logWrite(" IfConfiger process exitVal " + process.waitFor());
	      try {
	        errorHandler.join();
	        outputHandler.join();
	      } catch (InterruptedException except) {
	        logWrite(" IfConfiger join InterruptedException " + except.toString());
	      }
	      if (standardOut.length() > 0) {
	        logWrite(" IfConfiger results " + standardOut);
	        /*
	         * Find network interface
	         */
	        int index1 = standardOut.indexOf(netIntrfc);
	        if (index1 < 0) {
	          logWrite(" IfConfiger interface " + netIntrfc + " not found");
	          return "";
	        }
	        logWrite(" IfConfiger index1 " + index1);
	        /*
	         * Find IPv4 address start
	         */
	        int index2 = standardOut.indexOf(IPV4_ADDRESS_START, ++index1);
	        if (index2 < 0) {
	          logWrite(" IfConfiger IPv4 address start " + IPV4_ADDRESS_START + " not found");
	          return "";
	        }
	        logWrite(" IfConfiger index2 before " + index2);
	        index2 += IPV4_ADDRESS_START.length();
	        logWrite(" IfConfiger index2 after " + index2);
	        /*
	         * Find IPv4 address end
	         */
	        int index3 = standardOut.indexOf(IPV4_ADDRESS_END, index2);
	        if (index3 < 0) {
	          logWrite(" IfConfiger IPv4 address end " + IPV4_ADDRESS_END + " not found");
	          return "";
	        }
	        /*
	         * Extract IP address
	         */
	        ipAddress = standardOut.substring(index2, index3).trim();
	        if (ipAddress.length() == 0) {
	          logWrite(" IfConfiger blank IPv4 address");
	          return "";
	        }
	        logWrite(" IfConfiger IPv4 address = " + ipAddress);
	      } else {
	        logWrite(" IfConfiger no response to ifconfig");
	        return "";
	      }
	      errorOut = "";
	      standardOut = "";

	    } catch (Throwable thro) {
	      logWrite(" IfConfiger error " + thro.toString());
	    }
	    return ipAddress;
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

	public synchronized void setStreamOut(String streamOut, int type) {
	  switch (type) {
	  case STANDARD_OUT:
	    this.standardOut += streamOut;
	    break;
	  case ERROR_OUT:
	    this.errorOut += streamOut;
	    break;
	  default:
	    break;
	  }
	  logWrite(" IfConfiger setStreamOut type " + type + " " + streamOut);
	}

	}

