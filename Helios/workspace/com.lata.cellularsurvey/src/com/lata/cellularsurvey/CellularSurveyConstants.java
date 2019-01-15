/**
 * 
 */
package com.lata.cellularsurvey;

/**
 * @author Aarmijo
 *
 */
public interface CellularSurveyConstants {
    public static final int ONE_SECOND = 1000;
    public static final int FIVE_SECONDS = ONE_SECOND * 5;
    public static final int TEN_SECONDS = ONE_SECOND * 10;
    public static final int ONE_MINUTE = ONE_SECOND * 60;
    public static final int DOWN = 0;
    public static final int UP = 1;
    public static final String PING_MAX_VALUE = "0.000";
    public static final String FILE_TRANSFER_DIRECTORY = "/lata/transfers/";
    public static final String LOG_DIRECTORY = "/lata/logs/";
    public static final String LOG_EXTENSION = ".log";
    public static final String MAIN_DIRECTORY = "/lata/";
    public static final String CONFIGURATION_FILE = "config.xml";
    public static final String ERROR_FILE = "error.log";
    public static final String DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";
    public static final String SURVEY_NAME_FORMAT = "yyyyMMdd-HHmmss";
    public static final String SURVEY_NAME = "survey-";
    public static final String SURVEY_EXTENSION = ".csv";
    public static final String XML_CELLULAR_SURVEY = "Cell";
}
