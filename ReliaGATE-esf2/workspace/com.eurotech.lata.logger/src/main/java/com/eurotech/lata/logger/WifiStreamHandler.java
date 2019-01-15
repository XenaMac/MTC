package com.eurotech.lata.logger;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class WifiStreamHandler extends Thread {
	  private InputStream inputStream = null;
	  private WifiState wifiState = null;
	  private int type = 0;

	  WifiStreamHandler(InputStream inputStream, WifiState wifiState, int type)
	  {
	    this.inputStream = inputStream;
	    this.wifiState = wifiState;
	    this.type = type;
	  }

	  public void run()
	  {
	    try
	    {
	      InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
	      BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
	      String line = null;
	      while ((line = bufferedReader.readLine()) != null) {
	    	  wifiState.setStreamOut(line, type);
	      }
	    } catch (IOException ioe) {
	      ioe.printStackTrace();  
	    }
	  }
}
