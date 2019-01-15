package com.lata.cellularsurvey;

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

public class FtpDownload implements ConnectionListener, Logger, FtpConstants, CellularSurveyConstants {

  private static final String LOG_FILE = "download.log";
  private static final int ONE_SECOND = 1000;
  private static final int TIMEOUT = 15 * ONE_SECOND;
  private SimpleDateFormat dateFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private NumberFormat nbrFormat = new DecimalFormat("#0.0");
  private boolean connected = false;
  private String logDir = null;
  private long prevTime = 0;
  private long prevBytes = 0;
  private int clientIndex = 0;
  private ConnectionHandler handler = new ConnectionHandler();
  private FtpConnection ftpConnection = null;
  private FTPClient fTPClient = null;
  private boolean logging = false;
  private int downloadUpdateSeconds = 0;

  public FtpDownload(FTPClient fTPClient, int clientIndex, String netIntrfc, String host, String localDir, String logDir,
      String file, String username, String password, int downloadUpdateSeconds, boolean logging)
  {
    this.fTPClient = fTPClient;
    this.clientIndex = clientIndex;
    this.logDir = logDir;
    this.logging = logging;
    this.downloadUpdateSeconds = downloadUpdateSeconds;
    logWrite(" FtpDownload " + clientIndex + " started with host: " + host + " dir: " + localDir + " file: " + file 
        + " username: " + username + " password: " + password);
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
      logWrite(" Download started");
      prevTime = System.currentTimeMillis();
      ftpConnection.handleDownload(file);
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
      /*con.download(file);*/
      logWrite(" Download finished");
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
        fTPClient.saveBytesXfered(bytesXfrd, DOWN);
        double bitsXfrd = bytesXfrd * 8 / downloadUpdateSeconds;
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

  private void abort() {
    logWrite(" Download aborted");
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
