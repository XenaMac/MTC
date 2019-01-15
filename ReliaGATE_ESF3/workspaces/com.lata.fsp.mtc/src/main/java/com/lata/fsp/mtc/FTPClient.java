package com.lata.fsp.mtc;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

public final class FTPClient extends Thread implements CellularSurveyConstants
{
  private static final String LOG_FILE = "ftpclient.log";
  private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
  private volatile boolean active = true;
  private boolean abort = false;
  private String netIntrfc = null;
  private String xferDir = null;
  private String logDir = null;
  private String localFilename = null;
  private String remoteFilename = null;
  private String server = null;
  private String username = null;
  private String password = null;
  private int direction = DOWN;
  private FtpDownload ftpDownload = null;
  private FtpUpload ftpUpload = null;
  private DataRecorder dataRecorder = null;
  private UpdateIntervals updateIntervals;
  private boolean logging = false;

  public FTPClient(DataRecorder dataRecorder, int direction, 
      String netIntrfc, String logDir, Transfer transfer, UpdateIntervals updateIntervals, boolean logging) {
    super();
    this.dataRecorder = dataRecorder;
    this.direction = direction;
    this.netIntrfc = netIntrfc;
    File tmpFile = new File(transfer.getLocalFilePath());
    this.localFilename = tmpFile.getName();
    this.remoteFilename = transfer.getRemoteFilePath();
    this.xferDir = tmpFile.getParent();
    this.logDir = logDir;
    this.server = transfer.getFtpServer();
    this.username = transfer.getFtpServerUsername();
    this.password = transfer.getFtpServerPassword();
    this.logging = logging;
    this.updateIntervals = updateIntervals;
  }

  private void doFTP()
  {
    /*
     * Error check parameters
     */
    if (xferDir != null && xferDir.length() > 0 &&
        localFilename != null && localFilename.length() > 0 &&
        server != null && server.length() > 0 &&
        username != null && username.length() > 0 &&
        password != null && password.length() > 0 ) {

      File file = new File(xferDir + File.separator + localFilename);
      if (direction == DOWN) {
        /*
         * Delete existing local file that will be downloaded
         */
        if (file.exists()) {
          file.delete();
        }
        logWrite(" Download " + netIntrfc + " " + "-" + direction + " started");
        ftpDownload = new FtpDownload(this, netIntrfc, server, xferDir, logDir, localFilename, username, password, updateIntervals.getDownloadUpdateSeconds(), logging);
        logWrite(" Download " + netIntrfc + "-" + direction + " done");

      } else if (direction == UP) {
        /*
         * Make sure file to upload exists
         */
        if (file.canRead()) {
          logWrite(" Upload " + netIntrfc + " " + "-" + direction + " started");
          ftpUpload = new FtpUpload(this, netIntrfc, server, xferDir, logDir, localFilename, remoteFilename, username, password, updateIntervals.getUploadUpdateSeconds(), logging);
          logWrite(" Upload " + netIntrfc + "-" + direction + " done");
        } else {
          logWrite(" File "+ localFilename + " in dir " + xferDir + " not found or can't read");
        }
      } else {
        logWrite(" Bad direction");
      }
    } else {
      logWrite(" Missing argument(s)");
    }
    logWrite(" doFTP " + "-" + direction + " ended");
  } 

  public void logWrite(String msg) {
    if (logging == true) {
      try {
        BufferedWriter writer =
            new BufferedWriter(new FileWriter(logDir + LOG_FILE, true));
        writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
        writer.close();
      }
      catch (IOException ioe) {
        System.out.println("!@# Unable to write to log file: " + logDir + LOG_FILE + "\n" + ioe.getMessage());
      }
    }
  }

  /* (non-Javadoc)
   * @see java.lang.Thread#run()
   */
  public void run() {

    while (active) {
      logWrite(" FTPClient " + "-" + direction + " test started");
      /*
       * doFTP blocks until FTP done
       */
      doFTP();
      logWrite(" FTPClient " + "-" + direction + " test done ");
      try {
        Thread.sleep(ONE_SECOND);
      } catch (InterruptedException except) {

      }
    }
  }

  /**
   * 
   */
  public synchronized void stopThread() {
    logWrite(" FTPClient " + "-" + direction + " stopping thread and transfer");
    active = false;
    abort = true;
  }

  public synchronized boolean isAbort() {
    return abort;
  }

  public synchronized void setAbort(boolean abort) {
    this.abort = abort;
  }

  public synchronized void saveBytesXfered(long bytes, int dir) {
    if (dataRecorder != null) {
      try {
        logWrite(" FTPClient " + "-" + direction + " writing to data recorder " + bytes);
        dataRecorder.saveBytesXfered(dir, bytes);
      } catch (Exception ex) {
        logWrite(" FTPClient " + "-" + direction + " error writing to data recorder");
      }
    } else {
      logWrite(" FTPClient " + "-" + direction + " error no data recorder");
    }
  }
}


