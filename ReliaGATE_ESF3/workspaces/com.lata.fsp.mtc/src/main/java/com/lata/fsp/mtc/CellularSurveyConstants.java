package com.lata.fsp.mtc;

public interface CellularSurveyConstants {
	public static final int ONE_SECOND = 1000;
	public static final int FIVE_SECONDS = ONE_SECOND * 5;
	public static final int TEN_SECONDS = ONE_SECOND * 10;
	public static final int ONE_MINUTE = ONE_SECOND * 60;
	public static final int DOWN = 0;
	public static final int UP = 1;
	public static final int DATA_RECORD_DEFAULT = 5;
	public static final int UPDATE_DEFAULT = 3;
	public static final long MAX_TIMESTAMP = Long.MAX_VALUE;
	public static final double METERS_TO_FEET = 3.280839895;
	public static final double BAD_PDOP = 20.0;
	public static final String PING_MAX_VALUE = "0.000";
	public static final String USAGE_START_VALUE = "0.000";
	public static final String FILE_TRANSFER_DIRECTORY = "/lata/transfers/";
	public static final String LOG_DIRECTORY = "/lata/logs/";
	public static final String LOG_EXTENSION = ".log";
	public static final String MAIN_DIRECTORY = "/lata/";
	public static final String ERROR_FILE = "error.log";
	public static final String DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";
	public static final String DATE_FORMAT = "yyyyMMdd";
	public static final String DATE_HOUR_MINUTES_FORMAT = "yyyyMMdd-HH:mm";
	public static final String SURVEY_NAME_FORMAT = "yyyyMMdd-HHmmss";
	public static final String SURVEY_NAME = "survey-";
	public static final String SURVEY_EXTENSION = ".csv";
	public static final String XML_CELLULAR_SURVEY_VALUES = "Cell";
	public static final String XML_DATA_USAGE = "DataUsed";
	public static final String XML_SURVEY_STATE = "Survey";
	public static final String XML_IGNITION_TIMEOUT = "IgnTimeoutSecs";
	public static final String XML_BILL_START_DAY = "BillStartDay";
	public static final String XML_LAST_BILL_RESET = "LastBillReset";
	public static final String XML_LOGGED_ON = "LoggedOn";
	public static final String XML_AUTO_LOG_OFF_TIME = "AutoLogOffTime";
	public static final String XML_IP_HISTORY = "IPHistory";
	public static final String FALSE = "F";
	public static final String TRUE = "T";
}
