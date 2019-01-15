package com.eurotech.lata.logger;

public class Modem {

	  private String name = "mod1";
	  private String networkInterface = "ppp0";
	  private Transfer download = 
			  new Transfer("38.124.164.213", 
			  "LMT", 
			  "L@T@LMT", 
			  "/lata/transfers/ftpFromServer1.bin",
			  "/ftpFromServer1.bin");
	  private Transfer upload = 			  
			  new Transfer("38.124.164.213", 
			  "LMT", 
			  "L@T@LMT", 
			  "/lata/transfers/ftpToServerX.bin",
			  "/ftpToServerX.bin");
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

