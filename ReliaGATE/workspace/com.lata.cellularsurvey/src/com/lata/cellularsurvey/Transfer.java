package com.lata.cellularsurvey;

public class Transfer {
  
  private String ftpServer = "";
  private String ftpServerUsername = "";
  private String ftpServerPassword = "";
  private String localFilePath = "";
  private String remoteFilePath = "";
  private boolean valid = false;
  
  public boolean isValid() {
    return valid;
  }

  public void setValid(boolean valid) {
    this.valid = valid;
  }
  
  public String getFtpServer() {
    return ftpServer;
  }
  
  public void setFtpServer(String ftpServer) {
    this.ftpServer = ftpServer;
  }
  
  public String getFtpServerUsername() {
    return ftpServerUsername;
  }
  
  public void setFtpServerUsername(String ftpServerUsername) {
    this.ftpServerUsername = ftpServerUsername;
  }
  
  public String getFtpServerPassword() {
    return ftpServerPassword;
  }
  
  public void setFtpServerPassword(String ftpServerPassword) {
    this.ftpServerPassword = ftpServerPassword;
  }
  
  public String getLocalFilePath() {
    return localFilePath;
  }
  
  public void setLocalFilePath(String localFilePath) {
    this.localFilePath = localFilePath;
  }
  
  public String getRemoteFilePath() {
    return remoteFilePath;
  }
  
  public void setRemoteFilePath(String remoteFilePath) {
    this.remoteFilePath = remoteFilePath;
  }

  public boolean isValidTransfer() {
    if (ftpServer.length() == 0 || ftpServerUsername.length() == 0 || ftpServerPassword.length() == 0
        || localFilePath.length() == 0 || remoteFilePath.length() == 0) {
      return false;
    }
    valid = true;
    return true;
  }
}
