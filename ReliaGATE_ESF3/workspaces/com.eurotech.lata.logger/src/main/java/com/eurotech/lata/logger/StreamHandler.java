package com.eurotech.lata.logger;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class StreamHandler extends Thread {
  private InputStream inputStream = null;
  private Pinger pinger = null;
  private int type = 0;

  StreamHandler(InputStream inputStream, Pinger pinger, int type)
  {
    this.inputStream = inputStream;
    this.pinger = pinger;
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
        pinger.setStreamOut(line, type);
      }
    } catch (IOException ioe) {
      ioe.printStackTrace();  
    }
  }

}

