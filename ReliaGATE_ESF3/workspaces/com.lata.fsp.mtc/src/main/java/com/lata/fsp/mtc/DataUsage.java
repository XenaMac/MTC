package com.lata.fsp.mtc;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashSet;
import java.util.StringTokenizer;
import java.util.concurrent.TimeUnit;

import com.lata.fsp.runstate.IRunStateService;

public class DataUsage extends Thread implements CellularSurveyConstants {

	private static final String LOG_FILE = "datausage.log";
	private static final int STANDARD_OUT = 0;
	private static final int ERROR_OUT = 1;
	private static String DATA_USAGE_CMD = "/etc/init.d/GetUsage";
	private static String IP_LIST_CMD = "/etc/init.d/GetIps";
	private static String IP_LIST_FILE = "iplist.log";
	private NumberFormat nbrFormat = new DecimalFormat("#0.0000000000");
	private NumberFormat intFormat = new DecimalFormat("#00");
	private volatile boolean active = true;
	private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_TIME_FORMAT);
	private SimpleDateFormat yyyyMMddFormat = new SimpleDateFormat("yyyyMMdd");
	private SimpleDateFormat yyyyMMFormat = new SimpleDateFormat("yyyyMM");
	private DataRecorder dataRecorder = null;
	private IRunStateService iRunStateService = null;
	private UpdateIntervals updateIntervals = null;
	private String errorOut = "";
	private String standardOut = "";
	private boolean logging = false;
	private long usageUpdateMillis = 0;
	private long rcvLast = 0;
	private long xmtLast = 0;
	private long rcvTotal = 0;
	private long xmtTotal = 0;
	private HashSet<String> ipsSent = new HashSet<String>();
	private ArrayList<String> ipQueue = new ArrayList<String>();


	public DataUsage(DataRecorder dataRecorder, IRunStateService iRunStateService, UpdateIntervals updateIntervals, int updateInterval, boolean logging) {
		this.dataRecorder = dataRecorder;
		this.iRunStateService = iRunStateService;
		this.updateIntervals = updateIntervals;
		this.usageUpdateMillis = updateInterval * ONE_SECOND; // 3 seconds default
		this.logging = logging;
	}

	public void run() {
		logWrite(" Data Usage task starting");
		Date now = new Date();
		if (isUsageResetDay(now)) {
			resetDataUsage(now);
		}
		int count = 0;
		while (active) {
			try {
				Runtime runtime = Runtime.getRuntime();
				logWrite(" Data usage command " + DATA_USAGE_CMD);
				Process process = runtime.exec(DATA_USAGE_CMD);
				DataUsageStreamHandler errorHandler = new DataUsageStreamHandler(process.getErrorStream(), this, ERROR_OUT);
				DataUsageStreamHandler outputHandler = new DataUsageStreamHandler(process.getInputStream(), this, STANDARD_OUT); 
				errorHandler.start();
				outputHandler.start();
				logWrite(" Data usage process exitVal " + process.waitFor());
				try {
					errorHandler.join();
					outputHandler.join();
				} catch (InterruptedException except) {
					logWrite(" Data usage join InterruptedException " + except.toString());
				}
				String rcvStr = USAGE_START_VALUE;
				String xmtStr = USAGE_START_VALUE;
				if (standardOut.length() > 0) {
					logWrite(" Data usage results " + standardOut);
					StringTokenizer stringTokenizer = new StringTokenizer(standardOut, "|");
					if (stringTokenizer.hasMoreTokens()) {
						rcvStr = stringTokenizer.nextToken();
					}
					logWrite(" Data usage rcvStr " + rcvStr);
					if (stringTokenizer.hasMoreTokens()) {
						xmtStr = stringTokenizer.nextToken();
					}
					logWrite(" Data usage xmtStr " + xmtStr);
				}
				if (errorOut.length() > 0) {
					logWrite(" Data usage Error: " + errorOut);
				}
				errorOut = "";
				standardOut = "";
				logWrite(" Data usage count: " + count);
				if (++count >= 10) {
					count = 0;
					logWrite(" List IP command " + IP_LIST_CMD);
					process = runtime.exec(IP_LIST_CMD);
					errorHandler = new DataUsageStreamHandler(process.getErrorStream(), this, ERROR_OUT);
					outputHandler = new DataUsageStreamHandler(process.getInputStream(), this, STANDARD_OUT); 
					errorHandler.start();
					outputHandler.start();
					logWrite(" List IP command process exitVal " + process.waitFor());
					try {
						errorHandler.join();
						outputHandler.join();
					} catch (InterruptedException except) {
						logWrite(" List IP join InterruptedException " + except.toString());
					}
					if (standardOut.length() > 0) {
						logWrite(" List IP results " + standardOut);
					}
					if (errorOut.length() > 0) {
						logWrite(" List IP Error: " + errorOut);
					}
					errorOut = "";
					standardOut = "";
				}
				if (dataRecorder != null) {
					try {
						long rcv = new Long(rcvStr).longValue();
						long rcvUsage = 0;
						if (rcv > rcvLast) {
							rcvUsage = rcv - rcvLast;
							rcvTotal += rcvUsage;
						}
						rcvLast = rcv;
						long xmt = new Long(xmtStr).longValue();
						long xmtUsage = 0;
						if (xmt > xmtLast) {
							xmtUsage = xmt - xmtLast;
							xmtTotal += xmtUsage;
						}
						xmtLast = xmt;
						updateDataUsage(((rcvUsage + xmtUsage) / 1073741824.0), false);
						/*
						 * Save to data recorder only if cellular survey not running
						 */
						if (updateIntervals.getDownloadUpdateSeconds() <= 0) {
							dataRecorder.saveBytesXfered(DOWN, rcvTotal);
						}
						if (updateIntervals.getUploadUpdateSeconds() <= 0) {
							dataRecorder.saveBytesXfered(UP, xmtTotal);
						}
						String ipAddress = getNextIp();
						if (ipAddress != null) {
							ipsSent.add(ipAddress);
							iRunStateService.setState(XML_IP_HISTORY, ipAddress);
						}
					} catch (Exception ex) {
						logWrite(" Data usage error writing to data recorder");
					}
				} else {
					logWrite(" Data usage error no data recorder");
				}
				try {
					Thread.sleep(usageUpdateMillis);
				} catch (InterruptedException except) {
					logWrite(" Data usage sleep InterruptedException " + except.toString());
				}
			} catch (Throwable thro) {
				logWrite(" Data usage error " + thro.toString());
			}
		}
		logWrite(" Data usage ending");
	}
	
	private String getNextIp() {
		String ipAddress = null;
		try {
			if (!ipQueue.isEmpty()) {
				ipAddress = ipQueue.remove(0);
			} else {
				String nextIp;
				BufferedReader buffRead = null;
				try {
					String filename = LOG_DIRECTORY + IP_LIST_FILE;
					buffRead = new BufferedReader(new FileReader(filename));
					while ((nextIp = buffRead.readLine()) != null) {
						if (!ipsSent.contains(nextIp)) {
							ipQueue.add(nextIp);
						}
					}
				} catch (Exception except) {
					if (buffRead != null) { 
						try {
							buffRead.close();
						} catch (IOException except2) {
							logWrite("getNextIp " + except2.toString());
						} 
					} 
				}
				if (buffRead != null) { 
					try {
						buffRead.close();
					} catch (IOException except2) {
						logWrite("getNextIp " + except2.toString());
					} 
				} 
			}
		} catch (Exception except) {
			logWrite("getNextIp Exception " + except);
		}
		logWrite("getNextIp ipAddress " + ipAddress);
		return ipAddress;
	}
	
	private void resetDataUsage(Date now) {
		logWrite(" resetDataUsage resetting data usage");
		updateDataUsage(0.0, true);
		iRunStateService.setState(XML_LAST_BILL_RESET, dateTimeFormat.format(now)); 
	}

	private synchronized void updateDataUsage(double deltaUsage, boolean reset) {
		try {
			String totalUsageStr = iRunStateService.getState(XML_DATA_USAGE);
			logWrite(" Old Data usage totalUsageStr " + totalUsageStr);
			double totalUsage = 0.0;
			if (!reset) {
				totalUsage = new Double(totalUsageStr).doubleValue() + deltaUsage;
			}
			totalUsageStr = nbrFormat.format(totalUsage);
			logWrite(" New Data usage totalUsageStr " + totalUsageStr);
			iRunStateService.setState(XML_DATA_USAGE, totalUsageStr);
		} catch (Exception except) {
			logWrite(" updateDataUsage exception " + except);
		}
	}
	
	private boolean isUsageResetDay(Date now) {
		/*
		 * Reset data usage if reset was not already done today
		 * and one of the following conditions exist: 
		 * 1) today is billing cycle day or
		 * 2) today is past this month's billing cycle day and
		 *    last reset was prior to this month's billing cycle day or
		 * 3) today is before this month's billing cycle day and 
		 *    last reset was prior to last month's billing cycle day
		 */
		boolean status = false;
		try {
			String lastBillResetStr = iRunStateService.getState(XML_LAST_BILL_RESET);
			logWrite(" isUsageResetDay lastBillResetStr " + lastBillResetStr);
			if (lastBillResetStr != null && lastBillResetStr.length() > 0) {
				try {
					dateTimeFormat.parse(lastBillResetStr);
					Date nowDateOnly = yyyyMMddFormat.parse(yyyyMMddFormat.format(now));
					Date lastBillResetDateOnly = yyyyMMddFormat.parse(yyyyMMddFormat.format(dateTimeFormat.parse(lastBillResetStr)));
					String billDayStr = intFormat.format(new Integer(iRunStateService.getState(XML_BILL_START_DAY)));
					logWrite(" isUsageResetDay billDay " + billDayStr);
					Date thisMonthsBillResetDateOnly = yyyyMMddFormat.parse(yyyyMMFormat.format(now) + billDayStr);
					Calendar calendar = Calendar.getInstance();
					calendar.setTime(now);
					calendar.add(Calendar.MONTH, -1);
					Date lastMonthsBillResetDateOnly = yyyyMMddFormat.parse(yyyyMMddFormat.format(calendar.getTime()));
					logWrite(" isUsageResetDay lastBillResetDateOnly " + lastBillResetDateOnly);
					logWrite(" isUsageResetDay lastMonthsBillResetDateOnly " + lastMonthsBillResetDateOnly);
					logWrite(" isUsageResetDay nowDateOnly " + nowDateOnly);
					logWrite(" isUsageResetDay thisMonthsBillResetDateOnly " + thisMonthsBillResetDateOnly);
					if ((nowDateOnly.compareTo(lastBillResetDateOnly) != 0) && // Reset data usage if reset was not already done today and
						((nowDateOnly.compareTo(thisMonthsBillResetDateOnly) == 0) || // today is billing cycle date or
						((nowDateOnly.compareTo(thisMonthsBillResetDateOnly) > 0) && // today is past this month's billing cycle day and
						(lastBillResetDateOnly.compareTo(thisMonthsBillResetDateOnly) < 0)) || // last reset was prior to this month's billing cycle day or
						((nowDateOnly.compareTo(thisMonthsBillResetDateOnly) < 0) && // today is before this month's billing cycle day and
						(lastBillResetDateOnly.compareTo(lastMonthsBillResetDateOnly) < 0)))) // last reset was prior to last month's billing cycle day
					{                  
						status = true; 
						logWrite(" isUsageResetDay resetting usage " + status);
					}
				} catch (ParseException pe) {
					lastBillResetStr = dateTimeFormat.format(now);
					logWrite(" isUsageResetDay lastBillResetStr2 " + lastBillResetStr);
					iRunStateService.setState(XML_LAST_BILL_RESET, lastBillResetStr); 
				}
			} else {
				lastBillResetStr = dateTimeFormat.format(now);
				logWrite(" isUsageResetDay lastBillResetStr3 " + lastBillResetStr);
				iRunStateService.setState(XML_LAST_BILL_RESET, lastBillResetStr); 
			}
		} catch (Exception ex) {
			logWrite(" isUsageResetDay Exception " + ex);
		}
		return status;
	}

	public void logWrite(String msg) {
		if (logging == true) {
			try {
				BufferedWriter writer =
						new BufferedWriter(new FileWriter(LOG_DIRECTORY + LOG_FILE, true));
				writer.write(dateTimeFormat.format(new Date()) + msg + "\n");
				writer.close();
			}
			catch (IOException ioe) {
				System.out.println("!@# Unable to write to log file: " + LOG_DIRECTORY + LOG_FILE + "\n" + ioe.getMessage());
			}
		}
	}

	public void stopThread() {
		logWrite(" Data usage stopping thread");
		active = false;
	}

	public synchronized void setStreamOut(String streamOut, int type) {
		switch (type) {
		case STANDARD_OUT:
			this.standardOut = streamOut;
			break;
		case ERROR_OUT:
			this.errorOut += streamOut;
			break;
		default:
			break;
		}
		logWrite(" Data usage setStreamOut type " + type + " " + streamOut);
	}
}

