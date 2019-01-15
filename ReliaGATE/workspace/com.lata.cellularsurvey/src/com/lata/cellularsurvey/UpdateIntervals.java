package com.lata.cellularsurvey;

public class UpdateIntervals {
  public static final int DATA_RECORD_DEFAULT = 10;
  public static final int UPDATE_DEFAULT = 5;
  private int dataRecordSeconds = DATA_RECORD_DEFAULT;
  private int downloadUpdateSeconds = UPDATE_DEFAULT;
  private int uploadUpdateSeconds = UPDATE_DEFAULT;
  private int pingUpdateSeconds = UPDATE_DEFAULT;
  private String dataRecordSecondsStr = "";
  private String downloadUpdateSecondsStr = "";
  private String uploadUpdateSecondsStr = "";
  private String pingUpdateSecondsStr ="";
  
  public String getDataRecordSecondsStr() {
    return dataRecordSecondsStr;
  }
  
  public void setDataRecordSecondsStr(String dataRecordSecondsStr) {
    this.dataRecordSecondsStr = dataRecordSecondsStr;
  }
  
  public String getDownloadUpdateSecondsStr() {
    return downloadUpdateSecondsStr;
  }
  
  public void setDownloadUpdateSecondsStr(String downloadUpdateSecondsStr) {
    this.downloadUpdateSecondsStr = downloadUpdateSecondsStr;
  }
  
  public String getUploadUpdateSecondsStr() {
    return uploadUpdateSecondsStr;
  }
  
  public void setUploadUpdateSecondsStr(String uploadUpdateSecondsStr) {
    this.uploadUpdateSecondsStr = uploadUpdateSecondsStr;
  }
  
  public String getPingUpdateSecondsStr() {
    return pingUpdateSecondsStr;
  }
  
  public void setPingUpdateSecondsStr(String pingUpdateSecondsStr) {
    this.pingUpdateSecondsStr = pingUpdateSecondsStr;
  }

  public int getDataRecordSeconds() {
    return dataRecordSeconds;
  }
  
  public void setDataRecordSeconds(int dataRecordSeconds) {
    this.dataRecordSeconds = dataRecordSeconds;
  }
  
  public int getDownloadUpdateSeconds() {
    return downloadUpdateSeconds;
  }
  
  public void setDownloadUpdateSeconds(int downloadUpdateSeconds) {
    this.downloadUpdateSeconds = downloadUpdateSeconds;
  }
  
  public int getUploadUpdateSeconds() {
    return uploadUpdateSeconds;
  }
  
  public void setUploadUpdateSeconds(int uploadUpdateSeconds) {
    this.uploadUpdateSeconds = uploadUpdateSeconds;
  }
  
  public int getPingUpdateSeconds() {
    return pingUpdateSeconds;
  }
  
  public void setPingUpdateSeconds(int pingUpdateSeconds) {
    this.pingUpdateSeconds = pingUpdateSeconds;
  }

  public boolean isValidUpdateIntervals() {
    
    try {
      dataRecordSeconds = new Integer(dataRecordSecondsStr).intValue();
    } catch (NumberFormatException except) {
      dataRecordSeconds = DATA_RECORD_DEFAULT;
    }
    
    if (dataRecordSeconds < 2) {
      dataRecordSeconds = DATA_RECORD_DEFAULT;
    }
    
    try {
      downloadUpdateSeconds = new Integer(downloadUpdateSecondsStr).intValue();
    } catch (NumberFormatException except) {
      downloadUpdateSeconds = dataRecordSeconds/2;
    }
    
    if (downloadUpdateSeconds < 1) {
      downloadUpdateSeconds = UPDATE_DEFAULT;
    }
    
    try {
      uploadUpdateSeconds = new Integer(uploadUpdateSecondsStr).intValue();
    } catch (NumberFormatException except) {
      uploadUpdateSeconds = dataRecordSeconds/2;
    }
    
    if (uploadUpdateSeconds < 1) {
      uploadUpdateSeconds = UPDATE_DEFAULT;
    }
    
    try {
      pingUpdateSeconds = new Integer(pingUpdateSecondsStr).intValue();
    } catch (NumberFormatException except) {
      pingUpdateSeconds = dataRecordSeconds/2;
    }
    
    if (pingUpdateSeconds < 1) {
      pingUpdateSeconds = UPDATE_DEFAULT;
    }
    return true;
  }
}
