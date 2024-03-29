package com.eurotech.lata.logger;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

import net.sf.jftp.config.Settings;
import net.sf.jftp.net.BasicConnection;
import net.sf.jftp.net.ConnectionHandler;
import net.sf.jftp.net.ConnectionListener;
import net.sf.jftp.net.FtpConnection;
import net.sf.jftp.net.FtpConstants;
import net.sf.jftp.system.logging.Log;
import net.sf.jftp.system.logging.Logger;

public class FtpUpload implements ConnectionListener, Logger, FtpConstants, CellularSurveyConstants {

  private static final String LOG_FILE = "upload.log";
  private static final int ONE_SECOND = 1000;
  private static final int TIMEOUT = 15 * ONE_SECOND;
  private SimpleDateFormat dateFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private NumberFormat nbrFormat = new DecimalFormat("#0.0");
  private boolean connected = false;
  private String logDir = null;
  private long prevTime = 0;
  private long prevBytes = 0;
  private ConnectionHandler handler = new ConnectionHandler();
  private FtpConnection ftpConnection = null;
  private FTPClient fTPClient = null;
  private boolean logging = false;
  private int uploadUpdateSeconds = 0;

  public FtpUpload(FTPClient fTPClient, String netIntrfc, String host, String localDir, String logDir,
      String localFilename, String remoteFilename, String username, String password, int uploadUpdateSeconds, boolean logging)
  {
    this.fTPClient = fTPClient;
    this.logDir = logDir;
    this.logging = logging;
    this.uploadUpdateSeconds = uploadUpdateSeconds;
    logWrite(" FtpUpload started with host: " + host + " dir: " + localDir + " localFilename: " + localFilename 
    		+ " remoteFilename: " + remoteFilename + " username: " + username + " password: " + password);
    Settings.bufferSize = 16384;

    Log.setLogger(this);
    ftpConnection = new FtpConnection(host, netIntrfc);
    ftpConnection.addConnectionListener(this);
    ftpConnection.setConnectionHandler(handler);
    ftpConnection.setLocalPath(localDir);
    logWrite(" local directory " + ftpConnection.getLocalPath());
    if (ftpConnection.login(username, password) == LOGIN_OK) {

      while(!connected)
      {
        try { 
          Thread.sleep(10); 
        }
        catch(Exception ex) { 
          logWrite(ex.getMessage());
        }
      }
      logWrite(" remote directory " + ftpConnection.getPWD());
      logWrite(" Upload started");
      prevTime = System.currentTimeMillis();
      ftpConnection.handleUpload(localFilename, remoteFilename);
      while(connected)
      {
        try { 
          Thread.sleep(100); 
        }
        catch(Exception ex) { 
          logWrite(ex.getMessage());

        }
        long currTime = System.currentTimeMillis();
        /*
         * Abort transfer if no activity
         */
        if (currTime >= (prevTime + TIMEOUT)) {
          ftpConnection.abortTransfer();
          connected = false;
        }
      }
      /*con.upload(file);*/
      logWrite(" Upload finished");
    } else {
      logWrite(" Unable to log in");
    }
  }

  public void updateRemoteDirectory(BasicConnection con) {
    // TODO Auto-generated method stub

  }

  public void updateProgress(String file, String type, long bytes) {
    long currTime = System.currentTimeMillis();
    /*
     * Log data every five seconds
     */
    if (currTime >= (prevTime + FIVE_SECONDS)) {
      if (fTPClient.isAbort()) {
        abort();
        return;
      }
      if (bytes >= prevBytes) {
        long bytesXfrd = bytes - prevBytes;
        fTPClient.saveBytesXfered(bytesXfrd, UP);
        double bitsXfrd = bytesXfrd * 8 / uploadUpdateSeconds;
        String speedStr = "";
        if (bitsXfrd < 1048576) {
          speedStr = nbrFormat.format(bitsXfrd / 1024) + " Kbps";
        } else {
          speedStr = nbrFormat.format(bitsXfrd / 1048576) + " Mbps";
        }
        logWrite("," + bytes + "," + bytesXfrd + "," + speedStr);
        prevTime = currTime;
        prevBytes = bytes;
      } else {
        logWrite(" Unexpected bytes " + bytes);
      }
    }
  }

  public void connectionInitialized(BasicConnection con) {
    connected = true;
    logWrite(" Connection initialized");
  }

  public void connectionFailed(BasicConnection con, String why) {
    connected = false;
    logWrite(" Connection failed: " + why);
  }

  public void actionFinished(BasicConnection con) {
    connected = false;
    logWrite(" Connection closed");
  }

  public void abort() {
    logWrite(" Upload aborted");
    connected = false;
    ftpConnection.abortTransfer();
  }

  public void logWrite(String msg) {
    if (logging == true) {
      try {
        BufferedWriter writer =
            new BufferedWriter(new FileWriter(logDir + LOG_FILE, true));
        writer.write(dateFormat.format(new Date()) + msg + "\n");
        writer.close();
      }
      catch (IOException ioe) {
        System.out.println("!@# Unable to write to log file: " + logDir + LOG_FILE + "\n" + ioe.getMessage());
      }
    }
  }

  public void debug(String msg) {
    // TODO Auto-generated method stub

  }

  public void debugRaw(String msg) {
    // TODO Auto-generated method stub

  }

  public void debug(String msg, Throwable throwable) {
    // TODO Auto-generated method stub

  }

  public void warn(String msg) {
    // TODO Auto-generated method stub

  }

  public void warn(String msg, Throwable throwable) {
    // TODO Auto-generated method stub

  }

  public void error(String msg) {
    // TODO Auto-generated method stub

  }

  public void error(String msg, Throwable throwable) {
    // TODO Auto-generated method stub

  }

  public void info(String msg) {
    // TODO Auto-generated method stub

  }

  public void info(String msg, Throwable throwable) {
    // TODO Auto-generated method stub

  }

  public void fatal(String msg) {
    // TODO Auto-generated method stub

  }

  public void fatal(String msg, Throwable throwable) {
    // TODO Auto-generated method stub

  }

}

