package com.lata.fsp.runstate;

public interface RunStateConstants {
	
	public static final String LOG_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss:SSS:";
	public static final String LOG_FILE_DATE_FORMAT = "yyyyMMdd";
	public static final String CONFIG_FILE = "/lata/config/fterm.config";
	public static final String PROPERTIES_FILE = "/lata/config/fterm.props";
	public static final String LOG_FILE_DIRECTORY = "/lata/logs/";
	public static final String RUNSTATE_LOGFILE = LOG_FILE_DIRECTORY + "runstate";
	public static final String LOG_FILE_EXTENSION = ".log";
	public static final String OCTA_SERVICE_IP = "38.124.164.211";
	public static final String CAR_ID = "TruckID";
	public static final String CLIENT_PORT = "ClientPort";
	public static final String DEFAULT_CAR_ID = "0000";
	public static final String DEFAULT_CLIENT_PORT = "9017";
	public static final String DEFAULT_GPS_RATE = "5";
	public static final String DEFAULT_SERVER_PORT = "9018";
	public static final String DEFAULT_SFTP_SERVER_IP = OCTA_SERVICE_IP;
	public static final String DEFAULT_VERSION = "0.0.0";
	public static final String GPS_RATE = "GpsRate";
	public static final String LOG = "Log";
	public static final String SERVER_IP = "ServerIP";
	public static final String SERVER_PORT = "ServerPort";
	public static final String SFTP_SERVER_IP = "SFTPServerIP";
	public static final String VERSION = "Version";
	public static final String FALSE = "F";
	public static final String TRUE = "T";
	public static final String NEWLINE = "\n";
	public static final String XML_COMMAND = "Command";
	public static final String XML_ID = "Id";
	public static final String XML_NAME = "Name";
	public static final String XML_STATE = "State";
	public static final String XML_CARID = "TruckID";
	public static final String XML_CELLULAR_SURVEY = "Cell";
	public static final String XML_DATA_USAGE = "DataUsed";
	public static final String XML_SURVEY_STATE = "Survey";
	public static final String XML_IGNITION_TIMEOUT = "IgnTimeoutSecs";
	public static final String XML_BILL_START_DAY = "BillStartDay";
	public static final String XML_LAST_BILL_RESET = "LastBillReset";
	public static final String XML_IP_HISTORY = "IPHistory";
	public static final String XML_LOGGED_ON = "LoggedOn";
	public static final String XML_AUTO_LOG_OFF_TIME = "AutoLogOffTime";
	public static final String XML_WIFI_STATE = "W";
	public static final String DEFAULT_TIMEOUT_SECS = "2400";
}

