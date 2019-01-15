package com.lata.cellularsurvey;

public class Modem {

  private String name = "";
  private String networkInterface = "";
  private Transfer download = new Transfer();
  private Transfer upload = new Transfer();
  private Ping ping = new Ping();

  public String getName() {
    return name;
  }

  public void setName(String name) {
    this.name = name;
  }

  public String getNetworkInterface() {
    return networkInterface;
  }

  public void setNetworkInterface(String networkInterface) {
    this.networkInterface = networkInterface;
  }

  public Transfer getDownload() {
    return download;
  }

  public Transfer getUpload() {
    return upload;
  }

  public Ping getPing() {
    return ping;
  }

  public boolean isValidModem() {
    if (name.length() == 0 || networkInterface.length() == 0) {
      return false;
    }
    return true;
  }
}
