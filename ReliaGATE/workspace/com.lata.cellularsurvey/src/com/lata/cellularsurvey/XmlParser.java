package com.lata.cellularsurvey;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.io.StringReader;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Stack;
import java.util.Vector;

import org.xml.sax.*;  
import org.xml.sax.helpers.DefaultHandler;
import org.xml.sax.helpers.XMLReaderFactory;

public class XmlParser extends DefaultHandler implements CellularSurveyConstants {

  private static final String DEBUG_FILE = "XmlParser.log";
  SimpleDateFormat dateTimeFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss:");
  private Stack stack;
  private Locator locator;
  private UpdateIntervals updateIntervals;
  private Vector modems;
  private Modem modem;
  private Transfer download;
  private Transfer upload;
  private Ping ping;
  private int updateIntervalsCount = 0;
  private int modemsCount = 0;
  private boolean debug = false;

  public XmlParser(UpdateIntervals updateIntervals, Vector modems) {
    this.updateIntervals = updateIntervals;
    this.modems = modems;
  }

  public void parse(String configXml, boolean debug) {
    this.debug = debug;
    writeDebugLog("parse start");
    try {
      XMLReader xmlReader = XMLReaderFactory.createXMLReader();
      xmlReader.setContentHandler(this);

      StringReader stringReader = new StringReader(configXml);
      xmlReader.parse(new InputSource(stringReader));
    }
    catch (IOException ioe) {
      writeErrorLog("XmlParser parse " + ioe.getMessage());
    }
    catch (SAXException saxe) {
      writeErrorLog("XmlParser parse " + saxe.getMessage());
    }
  }

  private String getCurrentElementName() {
    return (String) stack.peek();
  }

  public void setDocumentLocator(Locator l) {
    locator = l;
  }

  public void startDocument() throws SAXException {
    writeDebugLog("startDocument");
    stack = new Stack();
  }

  public void startElement(String namespace, String local, String name,
      Attributes attrs) throws SAXException {
    writeDebugLog("startElement name " + name);
    stack.push(name);
    if ("UpdateIntervals".equals(name)) {
      if (updateIntervalsCount > 0) {
        throw new SAXParseException("Too many UpdateIntervals tags.", locator);
      } else {
        ++updateIntervalsCount;
      }
    } else if ("Modems".equals(name)) {
      if (modemsCount > 0) {
        throw new SAXParseException("Too many Modems tags.", locator);
      } else {
        ++modemsCount;
      }
    } else if ("Modem".equals(name)) {
      if (modems == null) {
        throw new SAXParseException("New Modem tag without enclosing Modems.", locator);
      }
      if (modem == null) {
        modem = new Modem();
      } else {
        throw new SAXParseException("New Modem tag before ending old.", locator);
      }
    } else if ("Download".equals(name)) {
      if (modem == null) {
        throw new SAXParseException("New Download tag without enclosing Modem.", locator);
      }
      if (upload != null) {
        throw new SAXParseException("New Download tag without closing Upload tag.", locator);
      }
      if (download == null) {
        download = modem.getDownload();
      } else {
        throw new SAXParseException("New Download tag before ending old Download or Upload.", locator);
      }
    } else if ("Upload".equals(name)) {
      if (modem == null) {
        throw new SAXParseException("New Upload tag without enclosing Modem.", locator);
      }
      if (download != null) {
        throw new SAXParseException("New Upload tag without closing Download tag.", locator);
      }
      if (upload == null) {
        upload = modem.getUpload();
      } else {
        throw new SAXParseException("New Upload tag before ending old Upload or Download.", locator);
      }
    } else if ("Ping".equals(name)) {
      if (modem == null) {
        throw new SAXParseException("New Ping tag without enclosing Modem.", locator);
      }
      if (ping == null) {
        ping = modem.getPing();
      } else {
        throw new SAXParseException("New Ping tag before ending old.", locator);
      }
    } else if (name.length() == 0)
      throw new SAXParseException("XML names not available", locator);
  }

  public void characters(char buf[], int off, int len) throws SAXException {
    String top = getCurrentElementName();
    String value = new String(buf, off, len);
    writeDebugLog("characters: top " + top + " value " + value);
    if ("DataRecordSeconds".equals(top)) {
      if (updateIntervals != null) {
        updateIntervals.setDataRecordSecondsStr(updateIntervals.getDataRecordSecondsStr() + value);
      } else {
        throw new SAXParseException("UpdateIntervals tag missing.", locator);
      }
    } else if ("DownloadUpdateSeconds".equals(top)) {
      if (updateIntervals != null) {
        updateIntervals.setDownloadUpdateSecondsStr(updateIntervals.getDownloadUpdateSecondsStr() + value);
      } else {
        throw new SAXParseException("UpdateIntervals tag missing.", locator);
      }
    } else if ("UploadUpdateSeconds".equals(top)) {
      if (updateIntervals != null) {
        updateIntervals.setUploadUpdateSecondsStr(updateIntervals.getUploadUpdateSecondsStr() + value);
      } else {
        throw new SAXParseException("UpdateIntervals tag missing.", locator);
      }
    } else if ("PingUpdateSeconds".equals(top)) {
      if (updateIntervals != null) {
        updateIntervals.setPingUpdateSecondsStr(updateIntervals.getPingUpdateSecondsStr() + value);
      } else {
        throw new SAXParseException("UpdateIntervals tag missing.", locator);
      }
    } else if ("ModemName".equals(top)) {
      if (modem != null) {
        modem.setName(modem.getName() + value);
      } else {
        throw new SAXParseException("Modem tag missing.", locator);
      }
    } else if ("Interface".equals(top)) {
      if (modem != null) {
        modem.setNetworkInterface(modem.getNetworkInterface() + value);
      } else {
        throw new SAXParseException("Interface tag missing.", locator);
      }
    } else if ("FTPServer".equals(top)) {
      if (download != null) {
        download.setFtpServer(download.getFtpServer() + value);
      } else if (upload != null){
        upload.setFtpServer(upload.getFtpServer() + value);
      } else {
        throw new SAXParseException("Download or Upload tag missing.", locator);
      }
    } else if ("FTPServerUsername".equals(top)) {
      if (download != null) {
        download.setFtpServerUsername(download.getFtpServerUsername() + value);
      } else if (upload != null){
        upload.setFtpServerUsername(upload.getFtpServerUsername() + value);
      } else {
        throw new SAXParseException("Download or Upload tag missing.", locator);
      }
    } else if ("FTPServerPassword".equals(top)) {
      if (download != null) {
        download.setFtpServerPassword(download.getFtpServerPassword() + value);
      } else if (upload != null){
        upload.setFtpServerPassword(upload.getFtpServerPassword() + value);
      } else {
        throw new SAXParseException("Download or Upload tag missing.", locator);
      }
    } else if ("LocalFilePath".equals(top)) {
      if (download != null) {
        download.setLocalFilePath(download.getLocalFilePath() + value);
      } else if (upload != null){
        upload.setLocalFilePath(upload.getLocalFilePath() + value);
      } else {
        throw new SAXParseException("Download or Upload tag missing.", locator);
      }
    } else if ("RemoteFilePath".equals(top)) {
      if (download != null) {
        download.setRemoteFilePath(download.getRemoteFilePath() + value);
      } else if (upload != null){
        upload.setRemoteFilePath(upload.getRemoteFilePath() + value);
      } else {
        throw new SAXParseException("Download or Upload tag missing.", locator);
      }
    } else if ("IPAddress".equals(top)) {
      if (ping != null) {
        ping.setIpAddress(ping.getIpAddress() + value);
      } else {
        throw new SAXParseException("Ping tag missing.", locator);
      }
    } else if ("Count".equals(top)) {
      if (ping != null) {
        ping.setCountStr(ping.getCountStr() + value);
      } else {
        throw new SAXParseException("Ping tag missing.", locator);
      }
    }
  }

  public void endElement(String namespace, String local, String name)
      throws SAXException {
    if ("UpdateIntervals".equals(name)) {
      /*
       * Validate UpdateIntervals
       */
      if (updateIntervals == null || !updateIntervals.isValidUpdateIntervals()) {
        throw new SAXParseException("Bad updateIntervals value or not initialized", locator);
      }
    } else if ("Modem".equals(name)) {
      /*
       * Add modem to modems vector
       */
      if (modems == null || modem == null || !modem.isValidModem()) {
        throw new SAXParseException("Bad modem value(s) or modems or modem not initialized", locator);
      }
      writeDebugLog("endElement: Add modem to modems vector here");
      modems.add(modem);
      modem = null;
    } else if ("Download".equals(name)) {
      /*
       * Verify Download
       */
      if (modem == null || download == null || !download.isValidTransfer()) {
        throw new SAXParseException("Bad Download value or modem or download not initialized", locator);
      }
      writeDebugLog("endElement: Download good");
      download = null;
    } else if ("Upload".equals(name)) {
      /*
       * Verify Upload
       */
      if (modem == null || upload == null || !upload.isValidTransfer()) {
        throw new SAXParseException("Bad Upload value or modem or upload not initialized", locator);
      }
      writeDebugLog("endElement: Upload good");
      upload = null;
    } else if ("Ping".equals(name)) {
      /*
       * Verify Ping
       */
      if (modem == null || ping == null || !ping.isValidPing()) {
        throw new SAXParseException("Bad Ping value or modem or ping not initialized", locator);
      }
      writeDebugLog("endElement: Ping good");
      ping = null;
    }
  }

  public void writeDebugLog(String msg) {
    if (debug == true) {
      try {
        BufferedWriter writer =
            new BufferedWriter(new FileWriter(LOG_DIRECTORY + DEBUG_FILE, true));
        writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
        writer.close();
      }
      catch (IOException ioe) {
        System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + DEBUG_FILE + "\n" + ioe.getMessage());
      }
    }
  }
  
  public void writeErrorLog(String msg) {
    try {
      BufferedWriter writer =
          new BufferedWriter(new FileWriter(MAIN_DIRECTORY + ERROR_FILE, true));
      writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
      writer.close();
      writeDebugLog(msg);
    }
    catch (IOException ioe) {
      System.out.println("!@# Unable to write to log file: " + MAIN_DIRECTORY + ERROR_FILE + "\n" + ioe.getMessage());
    }
}
}
