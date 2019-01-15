package com.lata.cellularsurvey;

public class Ping {
  
  private String ipAddress = "";
  private int count = 0;
  private String countStr = "";
  private boolean valid = false;
  
  public boolean isValid() {
    return valid;
  }

  public void setValid(boolean valid) {
    this.valid = valid;
  }

  public String getCountStr() {
    return countStr;
  }

  public void setCountStr(String countStr) {
    this.countStr = countStr;
  }

  public String getIpAddress() {
    return ipAddress;
  }
  
  public void setIpAddress(String ipAddress) {
    this.ipAddress = ipAddress;
  }
  
  public int getCount() {
    return count;
  }
  
  public void setCount(int count) {
    this.count = count;
  }

  public boolean isValidPing() {
    try {
      count = new Integer(countStr).intValue();
    } catch (NumberFormatException except) {
      return false;
    }
    if (count == 0 || ipAddress.length() == 0) {
      return false;
    }
    valid = true;
    return true;
  }
}
