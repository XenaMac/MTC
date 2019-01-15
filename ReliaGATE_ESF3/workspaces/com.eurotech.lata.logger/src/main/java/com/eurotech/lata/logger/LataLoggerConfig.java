package com.eurotech.lata.logger;

import java.util.Map;

public class LataLoggerConfig {
	
	private static final String KEY_LOGGING_ENABLED = "logging.enabled";
	private static final String KEY_LOGGING_RATE = "logging.rate";
	private static final String KEY_LOG_FILE = "log.file";
	private static final String KEY_MAX_LOG_RECORDS = "max.log.records";
	
	private static final boolean DFLT_LOGGING_ENABLED = true;
	private static final int DFLT_LOGGING_RATE = 2;
	private static final String DFLT_LOG_FILE = "/tmp/lata.log";
	private static final int DFLT_MAX_LOG_RECORDS = 250;
	
	private Map<String, Object> m_properties;
	
	public LataLoggerConfig(Map<String, Object> properties)  {
		m_properties = properties;
	}
	
	public boolean isLoggingEnabled() {
		boolean ret = DFLT_LOGGING_ENABLED;
		Boolean bool = (Boolean)m_properties.get(KEY_LOGGING_ENABLED);
    	if (bool != null) {
    		ret = bool;
    	}
    	return ret;
	}
	
	public int getLogRate() {
		int ret = DFLT_LOGGING_RATE;
		Integer intgr = (Integer)m_properties.get(KEY_LOGGING_RATE);
    	if (intgr != null) {
    		ret = intgr;
    	}
		return ret;
	}
	
	public String getLogFileName() {
		String ret = DFLT_LOG_FILE;
		String str = (String)m_properties.get(KEY_LOG_FILE);
		if (str != null) {
			ret = str;
		}
		return ret;
	}
	
	public int getMaxLogRecords() {
		int ret = DFLT_MAX_LOG_RECORDS;
		Integer intgr = (Integer)m_properties.get(KEY_MAX_LOG_RECORDS);
    	if (intgr != null) {
    		ret = intgr;
    	}
		return ret;
	}
}
