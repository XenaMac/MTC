package com.lata.fsp.mtc;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class IfConfigStreamHandler extends Thread {
	  private InputStream inputStream = null;
	  private IfConfiger ifconfig = null;
	  private int type = 0;
	  
	  IfConfigStreamHandler(InputStream inputStream, IfConfiger ifconfig, int type)
	  {
	    this.inputStream = inputStream;
	    this.ifconfig = ifconfig;
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
	        ifconfig.setStreamOut(line, type);
	      }
	    } catch (IOException ioe) {
	      ioe.printStackTrace();  
	    }
	  }
	}
