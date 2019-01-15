package com.lata.fsp.mtc;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class DataUsageStreamHandler extends Thread {
  private InputStream inputStream = null;
  private DataUsage dataUsage = null;
  private int type = 0;

  DataUsageStreamHandler(InputStream inputStream, DataUsage dataUsage, int type)
  {
    this.inputStream = inputStream;
    this.dataUsage = dataUsage;
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
    	  dataUsage.setStreamOut(line, type);
      }
    } catch (IOException ioe) {
      ioe.printStackTrace();  
    }
  }

}

