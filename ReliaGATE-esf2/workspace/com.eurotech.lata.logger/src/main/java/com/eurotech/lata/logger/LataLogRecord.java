package com.eurotech.lata.logger;

public class LataLogRecord {
	
	private static final String s_separator = ",";

	private int m_cellularRssi;
	private int m_wifiRssi;
	private int m_fixQuality;
	private double m_latitude;
	private double m_longitude;
	private double m_speedMPH;
	private String m_timestamp;
	private double m_download;
	private double m_upload;
	private double m_latency;
	
	public LataLogRecord(int cellRssi, double download, double upload, double latency, int wifiRssi, int fixQuality,
			double latitude, double longitude, double speed, String timestamp) {
		m_cellularRssi = cellRssi;
		m_wifiRssi = wifiRssi;
		m_fixQuality = fixQuality;
		m_latitude = latitude;
		m_longitude = longitude;
		m_speedMPH = speed;
		m_timestamp = timestamp;
	}
	
	public int getCellularRssi() {
		return m_cellularRssi;
	}

	public int getWifiRssi() {
		return m_wifiRssi;
	}

	public int getFixQuality() {
		return m_fixQuality;
	}

	public double getLatitude() {
		return m_latitude;
	}

	public double getLongitude() {
		return m_longitude;
	}

	public double getSpeedMPH() {
		return m_speedMPH;
	}
	
	public String getTimestamp() {
		return m_timestamp;
	}

	public String toString () {
		StringBuffer sb = new StringBuffer();
		sb.append(m_cellularRssi);
		sb.append(s_separator);
		sb.append(m_download);
		sb.append(s_separator);
		sb.append(m_upload);
		sb.append(s_separator);
		sb.append(m_latency);
		sb.append(s_separator);
		sb.append(m_wifiRssi);
		sb.append(s_separator);
		sb.append(m_fixQuality);
		sb.append(s_separator);
		sb.append(m_latitude);
		sb.append(s_separator);
		sb.append(m_longitude);
		sb.append(s_separator);
		sb.append(m_speedMPH);
		sb.append(s_separator);
		sb.append(m_timestamp);
		return sb.toString();
	}
}
