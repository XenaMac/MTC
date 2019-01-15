package com.lata.fsp.mtc;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Map;
import java.util.StringTokenizer;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import org.eclipse.kura.configuration.ConfigurableComponent;
import org.eclipse.kura.net.modem.ModemMonitorListener;
import org.eclipse.kura.net.modem.ModemMonitorService;
import org.eclipse.kura.net.wifi.WifiClientMonitorListener;
import org.eclipse.kura.net.wifi.WifiClientMonitorService;
import org.eclipse.kura.position.NmeaPosition;
import org.eclipse.kura.position.PositionService;
import org.osgi.service.component.ComponentContext;
import org.osgi.service.component.ComponentException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.lata.fsp.runstate.IRunStateListener;
import com.lata.fsp.runstate.RunState;
import com.lata.fsp.trackerterminal.TrackerTerminal;

public class LataLogger implements ConfigurableComponent, ModemMonitorListener, WifiClientMonitorListener,
CellularSurveyConstants, IRunStateListener {	
	private static final Logger s_logger = LoggerFactory.getLogger(LataLogger.class);
	private static final String DELIMITER = "|";
	private final static int TASK_RATE = 5;
	private static final int DFLT_NORMAL_SHUTDOWN_DELAY = 2400; // in seconds
	private static final int MIN_MPH_MOVEMENT = 2;
	private ModemMonitorService m_modemMonitorService;
	private WifiClientMonitorService m_wifiClientMonitorService;
	private PositionService m_positionService;
	private ScheduledExecutorService    m_worker;
	private ScheduledFuture<?>          m_handle;
	private ScheduledFuture<?>  m_delayedShutdownTask = null;
	private IfConfiger ifConfig = new IfConfiger();
	private volatile boolean active = true;
	private SimpleDateFormat dateTimeFormat = new SimpleDateFormat(DATE_HOUR_MINUTES_FORMAT);
	private SimpleDateFormat dateFormat = new SimpleDateFormat(DATE_FORMAT);
	private String autoLogoffTime = "23:00";
	private boolean isLoggedOn = false;
	private boolean isSurveyRunning = false;
	private boolean isSurveyArgsModified = false;
	private boolean isTransferRateSurveyStopped = true;
	private boolean logDownloadRate = false;
	private boolean logPing = false;
	private boolean logUploadRate = false;
	private boolean debug = true;
	private UpdateIntervals updateIntervals= new UpdateIntervals();
	private Modem modem= new Modem();
	private Pinger pinger;
	private DataRecorder dataRecorder;
	private DataUsage dataUsage;
	private static FTPClient fTPUpClient;
	private static FTPClient fTPDownClient;
	private RunState runstate;
	private TrackerTerminal trackerTerminal;
	private long startSurveyTimestamp = MAX_TIMESTAMP;
	private long endSurveyTimestamp = 0;
	private long lastMovedTimestamp = 0;
	private long shutDownTimestamp = 0;
/*	private LedService m_reliagateLed;
	private IgnitionService m_ignitionSensor;*/

	public LataLogger() 
	{
		super();
		m_worker = Executors.newSingleThreadScheduledExecutor();
	}

	public void setModemMonitorService(ModemMonitorService modemMonitorService) {
		s_logger.info("setModemMonitorService()");
		m_modemMonitorService = modemMonitorService;
	}

	public void unsetModemMonitorService(ModemMonitorService modemMonitorService) {
		s_logger.info("unsetModemMonitorService()");
		m_modemMonitorService = null;
	}
	
	public void setWifiClientMonitorService(WifiClientMonitorService wifiClientMonitorService) {
		s_logger.info("setWifiClientMonitorService()");
		m_wifiClientMonitorService = wifiClientMonitorService;
	}

	public void unsetWifiClientMonitorService(WifiClientMonitorService wifiClientMonitorService) {
		s_logger.info("unsetWifiClientMonitorService()");
		m_wifiClientMonitorService = null;
	}
	
	public void setPositionService(PositionService positionService) {
		s_logger.info("setPositionService");
		m_positionService = positionService;
	}

	public void unsetPositionService(PositionService positionService) {
		s_logger.info("unsetPositionService");
		m_positionService = null;
	}
	
/*	public void setIgnitionService(IgnitionService ignitionService) {
		s_logger.info("setIgnitionService()");
		m_ignitionSensor = ignitionService;
	}

	public void unsetIgnitionService(IgnitionService ignitionService) {
		s_logger.info("unsetIgnitionService()");
		m_ignitionSensor = null;
	}

	public void setLedService(LedService ledService) {
		s_logger.info("setLedService()");
		m_reliagateLed = ledService;
	}

	public void unsetLedService(LedService ledService) {
		s_logger.info("unsetLedService()");
		m_reliagateLed = null;
	}*/
	
	private void startLataTrax() {
		runstate = new RunState(debug);
		runstate.start();
		runstate.addRunStateListener(this);
		s_logger.info("startLataTrax starting trackerTerminal");
		try {
			trackerTerminal = new TrackerTerminal();
			trackerTerminal.bind(m_positionService, runstate);
			trackerTerminal.start();
		} catch (Throwable ex) {
			s_logger.info("startLataTrax Exception " + ex.getLocalizedMessage());
		}
	}
	
	private void stopLataTrax() {
		runstate.removeRunStateListener(this);
		runstate.stopThread();
		s_logger.info("stopLataTrax stopping trackerTerminal");
		try {
			trackerTerminal.unbind();
			trackerTerminal.stopThread();
		} catch (Throwable ex) {
			s_logger.info("stopLataTrax Exception " + ex.getLocalizedMessage());
		}
	}
	
	protected void activate(ComponentContext componentContext, Map<String,Object> properties) 
	{
		s_logger.info("LataLogger: Activating ...");
		

/*		try {
			m_reliagateLed.setState(Led.BOTTOM, LedState.OFF);
			m_reliagateLed.setState(Led.TOP, LedState.OFF);
		} catch (KuraException e) {
			s_logger.info("activate exception " + e.getMessage());
		}*/
		
		active = true;
		startLataTrax();
		
		if (m_modemMonitorService != null) {
			s_logger.info("activate Registering with ModemMonitorService");
			m_modemMonitorService.registerListener(this);
		}
		
		if (m_wifiClientMonitorService != null) {
			s_logger.info("activate Registering with WifiClientMonitorService");
			m_wifiClientMonitorService.registerListener(this);
		}
		
		try  {			
			doUpdate(false);
		}
		catch (Exception e) {
			s_logger.error("Error during component activation", e);
			throw new ComponentException(e);
		}
		s_logger.info("LataLogger: Activating ... Done.");
	}
	
	protected void deactivate(ComponentContext componentContext) 
	{
		s_logger.info("LataLogger: Deactivating ...");

		active = false;
		stopLataTrax();
		stopSurvey();
		destroyConfigVars();
		if (m_modemMonitorService != null) {
			s_logger.info("deactivate() :: Unregistering with ModemMonitorService");
			m_modemMonitorService.unregisterListener(this);
		}
		
		if (m_wifiClientMonitorService != null) {
			s_logger.info("deactivate() :: Unregistering with WifiClientMonitorService");
			m_wifiClientMonitorService.unregisterListener(this);
		}
		
		// shutting down the worker and cleaning up the properties
		m_worker.shutdown();

		// stop delayed shutdown thread
		if ((m_delayedShutdownTask != null) && !m_delayedShutdownTask.isCancelled()) {
			s_logger.info("deactivate() :: Stopping delayedShutdownTask thread");
			boolean status = m_delayedShutdownTask.cancel(true);
			if (status) {
				s_logger.info("deactivate() :: delayedShutdownTask thread stopped");
			}
			m_delayedShutdownTask = null;
		}
		
		s_logger.info("LataLogger: Deactivating ... Done.");
	}	
	
	public void updated(Map<String,Object> properties)
	{
		s_logger.info("LataLogger: Updated ...");
		doUpdate(true);
		s_logger.info("LataLogger: Updated ... Done.");
	}

	private void doUpdate(boolean onUpdate) 
	{
		// cancel a current worker handle if one if active
		if (m_handle != null) {
			m_handle.cancel(true);
		}
		
		m_handle = m_worker.scheduleAtFixedRate(new Runnable() {		
			@Override
			public void run() {
				Thread.currentThread().setName(getClass().getSimpleName());
				lataLogger();
			}
		}, 0, TASK_RATE, TimeUnit.SECONDS);
	}
	
	private void lataLogger() 
	{					
		s_logger.info("lataLogger: started");
		if (active) {
/*			processIgnitionState();
			processLoggedOnState();*/
			processGpsState();
			if (!isSurveyRunning) {
				createConfigVars();
				loadConfigFile();
				startSurvey();
			}
			if (isSurveyArgsModified) {
				/*
				 * Stop previous survey so next call
				 * can start survey with new args
				 */
				stopTransferRateSurvey();
				isTransferRateSurveyStopped = true;
				isSurveyArgsModified = false;
			} else {
				long nowTimestamp = new Date().getTime();
				if (nowTimestamp >= startSurveyTimestamp && nowTimestamp <= endSurveyTimestamp) {
					if (isTransferRateSurveyStopped) {
						startTransferRateSurvey();
						isTransferRateSurveyStopped = false;
					}
				} else {
					if (!isTransferRateSurveyStopped) {
						stopTransferRateSurvey();
						isTransferRateSurveyStopped = true;
					}
				}
			}
		}
	}

/*	private void processIgnitionState() {
		boolean isIgnitionOn = false;
		try {
			isIgnitionOn = m_ignitionSensor.isIgnitionOn();
			isIgnitionOn = true;
			s_logger.info("processIgnitionState isIgnitionOn: " + isIgnitionOn);
			if (isIgnitionOn && !ifPastAutoLogoffTime()) {
				if (m_reliagateLed.getState(Led.TOP) == LedState.OFF) {
					m_reliagateLed.setState(Led.TOP, LedState.ON);
				}
				if ((m_delayedShutdownTask != null) && !m_delayedShutdownTask.isCancelled()) {
					shutDownTimestamp = 0;
					s_logger.info("processIgnitionState :: ignition ON :: Stopping delayedShutdownTask");
					boolean status = m_delayedShutdownTask.cancel(true);
					if (status) {
						s_logger.info("processIgnitionState :: ignition ON :: delayedShutdownTask stopped");
					}
					m_delayedShutdownTask = null;
				}
			} else {
				if (m_reliagateLed.getState(Led.TOP) == LedState.ON) {
					m_reliagateLed.setState(Led.TOP, LedState.OFF);
				}
				if (m_delayedShutdownTask == null) {
					int shutdownDelay = DFLT_NORMAL_SHUTDOWN_DELAY;
					String shutdownDelayStr = runstate.getState(XML_IGNITION_TIMEOUT);
					if (shutdownDelayStr != null) {
						try {
							shutdownDelay = new Integer(shutdownDelayStr).intValue();
						} catch (NumberFormatException except) {
							s_logger.info("processIgnitionState Exception: " + except.getMessage());
						}
					}
					shutDownTimestamp = System.currentTimeMillis();
					TimeUnit timeUnit = TimeUnit.SECONDS;
					s_logger.info("processIgnitionState Scheduling 'shutdown' thread in " + shutdownDelay + " " + timeUnit.toString());
					ScheduledThreadPoolExecutor executor = new ScheduledThreadPoolExecutor(1);
					m_delayedShutdownTask = executor.schedule(new Runnable() {
						@Override
						public void run() {
							doShutdown();
						}
					}, shutdownDelay, timeUnit);
				} else if (isLoggedOn) {
					shutDownTimestamp = 0;
					s_logger.info("processIgnitionState :: Driver still logged on :: Stopping delayedShutdownTask");
					boolean status = m_delayedShutdownTask.cancel(true);
					if (status) {
						s_logger.info("processIgnitionState :: Logged on with ignition OFF :: delayedShutdownTask stopped");
					}
					m_delayedShutdownTask = null;
				} else if (lastMovedTimestamp > shutDownTimestamp) {
						shutDownTimestamp = 0;
						s_logger.info("processIgnitionState :: Moved with ignition OFF :: Stopping delayedShutdownTask");
						boolean status = m_delayedShutdownTask.cancel(true);
						if (status) {
							s_logger.info("processIgnitionState :: Moved with ignition OFF :: delayedShutdownTask stopped");
						}
						m_delayedShutdownTask = null;
				}
			}
		} catch (Exception e) {
			s_logger.info("processIgnitionState Exception: " + e.getMessage());
		} 
		
	}*/

/*	private void doShutdown() {
		s_logger.info("!!! Shutting System Down !!!");
		try {
			Runtime rt = Runtime.getRuntime();
			rt.exec("poweroff");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}*/
	
/*	private void processLoggedOnState() {
		if (isLoggedOn) {
			s_logger.info("processLoggedOnState: driver is logged on");
			s_logger.info("processLoggedOnState: autoLogoffTime: " + autoLogoffTime);
			if (ifPastAutoLogoffTime()) {
				s_logger.info("processLoggedOnState: after auto log off time, logging off driver");
				runstate.setState(XML_LOGGED_ON, FALSE);
			}
		} else {
			s_logger.info("processLoggedOnState: driver is logged off");
		}
		
	}*/
	
/*	private boolean ifPastAutoLogoffTime() {
		boolean status = false;
		try {
			if (autoLogoffTime != null && autoLogoffTime.length() > 0) {
				Date now = new Date();
				String nowDateOnlyStr = dateFormat.format(now);
				Date autoLogoffDateTime = dateTimeFormat.parse(nowDateOnlyStr + "-" + autoLogoffTime);
				if (now.compareTo(autoLogoffDateTime) > 0) {
					s_logger.info("ifPastAutoLogoffTime: now: " + now);
					s_logger.info("ifPastAutoLogoffTime: autoLogoffDateTime: " + autoLogoffDateTime);
					status = true;
				}
			}
		} catch (ParseException e) {
			s_logger.info("ifPastAutoLogoffTime: Bad autoLogoffTime: " + autoLogoffTime);
		}
		return status;
	}*/
	
	private void processGpsState() {
		try {
			
			/*LedState ledState = m_reliagateLed.getState(Led.BOTTOM);*/
			if (m_positionService.isLocked()) {
				s_logger.info("setGpsLed GPS locked");
/*				if (ledState == LedState.OFF) {
					m_reliagateLed.setState(Led.BOTTOM, LedState.ON);
					s_logger.info("setGpsLed ON");
				}*/
				if (isMoving()) {
					lastMovedTimestamp = System.currentTimeMillis();
					s_logger.info("isMoving");
				}
			} else {
				s_logger.info("setGpsLed GPS not locked");
/*				if (ledState == LedState.ON) {
					m_reliagateLed.setState(Led.BOTTOM, LedState.OFF);
					s_logger.info("setGpsLed OFF");
				}*/
			}
		} catch (Exception e) {
			s_logger.info("processGpsState Exception: " + e.getMessage());
		}
	}
	
	private boolean isMoving() {
		NmeaPosition position = m_positionService.getNmeaPosition();
		int speed = (int) Math.round(position.getSpeedMph());
		if (speed > MIN_MPH_MOVEMENT) {
			return true;
		} else {
			return false;
		}
	}
	
	@Override
	public void loaded() {
		s_logger.debug("loaded");
		String surveySetting = runstate.getState(XML_SURVEY_STATE);
		if (surveySetting != null) {
			s_logger.debug("loaded loaded Survey run status: " + surveySetting);
			setSurvey(surveySetting);
		}
		autoLogoffTime = runstate.getState(XML_AUTO_LOG_OFF_TIME).trim();
		if (autoLogoffTime != null) {
			s_logger.debug("loaded loaded autoLogoffTime: " + autoLogoffTime);
		}
		String loggedOn = runstate.getState(XML_LOGGED_ON);
		if (loggedOn != null) {
			s_logger.debug("loaded loaded logged on state: " + loggedOn);
			if (loggedOn.equals(TRUE)) {
				loggedOn = FALSE;
				s_logger.debug("loaded setting logged on state to : " + loggedOn);
				runstate.setState(XML_LOGGED_ON, loggedOn);
			}
			setLoggedOnState(loggedOn);
		}
	}

	@Override
	public void newState(String key, String value) {
		if (key == null) {
			return;
		} else if (key.equalsIgnoreCase(XML_SURVEY_STATE)) {
			s_logger.info("newState Receiving new Survey run status: " + value);
			setSurvey(value);
		} else if (key.equalsIgnoreCase(XML_LOGGED_ON)) {
			s_logger.info("newState Receiving change of Logged On state: " + value);
			setLoggedOnState(value);
		}
	}
	
	private void setLoggedOnState(String value) {
		if (value.toLowerCase().equals("t")) {
			isLoggedOn = true;
		} else if (value.toLowerCase().equals("f")) {
			isLoggedOn = false;
		} 
	}
	
	@Override
	public void setCellularSignalLevel(int signalLevel) {
		if (signalLevel < 0) {
			s_logger.info("Cellular Signal Level is set to {} dBm " + signalLevel);
		} else {
			s_logger.info(
					"Cellular Signal Level reported {} -> undetectable. Setting it to 0 " + signalLevel);
		}
		if (dataRecorder != null) {
			dataRecorder.setCellularSignalLevel(signalLevel);
		}
	}

	@Override
	public void setWifiSignalLevel(int signalLevel) {
		s_logger.info("WiFi Signal level is set to {} dBm " + signalLevel);
	}
	
	private void stopSurvey() {
		s_logger.info("stopSurvey");
		dataUsage.stopThread();
		try {
			dataUsage.join();
		} catch (InterruptedException ex) {
			s_logger.info("stopSurvey: problem joining dataUsage thread " + ex.toString());
		}
		dataUsage = null;
		dataRecorder.stopThread();
		try {
			dataRecorder.join();
		} catch (InterruptedException ex) {
			s_logger.info("stopSurvey: problem joining dataRecorder thread " + ex.toString());
		}
		dataRecorder = null;
		stopTransferRateSurvey();
		isSurveyRunning = false;
	}
	
	private void stopTransferRateSurvey() {
		s_logger.info("stopTransferRateSurvey");	
		if (fTPDownClient != null) {
			fTPDownClient.stopThread();
			updateIntervals.setDownloadUpdateSeconds(-1);
			try {
				fTPDownClient.join();
			} catch (InterruptedException ex) {
				s_logger.info("stopTransferRateSurvey problem joining fTPDownClient thread " + ex.toString());
			}
			fTPDownClient = null;
		}
		if (fTPUpClient != null) {
			fTPUpClient.stopThread();
			updateIntervals.setUploadUpdateSeconds(-1);
			try {
				fTPUpClient.join();
			} catch (InterruptedException ex) {
				s_logger.info("stopTransferRateSurvey problem joining fTPUpClient thread " + ex.toString());
			}
			fTPUpClient = null;
		}
		if (pinger != null) {
			pinger.stopThread();
			updateIntervals.setPingUpdateSeconds(-1);
			try {
				pinger.join();
			} catch (InterruptedException ex) {
				s_logger.info("stopTransferRateSurvey problem joining pinger thread " + ex.toString());
			}
			pinger = null;
		}
	}
	
	private void loadConfigFile() {
		s_logger.debug("loadConfigFile");
	}

	private void createConfigVars() {
		s_logger.debug("createConfigVars");
	}

	private void destroyConfigVars() {
		s_logger.debug("destroyConfigVars");
	}
	
	private void startSurvey() {
		s_logger.info("startSurvey");
		String netIntrfcAddr = ifConfig.getInterfaceAddr(modem.getNetworkInterface(), debug);
		if (netIntrfcAddr != null && netIntrfcAddr.length() > 0) {
			s_logger.info("startSurvey: using " + netIntrfcAddr);
			dataRecorder = new DataRecorder(m_positionService, runstate, modem, updateIntervals, debug);
			if (active) {
				dataRecorder.start();
				isSurveyRunning = true;
			}
			dataUsage = new DataUsage(dataRecorder, runstate, updateIntervals, UPDATE_DEFAULT, debug);
			if (active) {
				dataUsage.start();
			}
		} else {
			s_logger.info("startSurvey: Network interface " + modem.getNetworkInterface() + " not found");
		}
	}
	
	private void startTransferRateSurvey() {
		s_logger.info("startTransferRateSurvey");
		String netIntrfcAddr = ifConfig.getInterfaceAddr(modem.getNetworkInterface(), debug);
		if (netIntrfcAddr != null && netIntrfcAddr.length() > 0) {
			s_logger.info("startTransferRateSurvey: using " + netIntrfcAddr);
			Transfer download = modem.getDownload();
			if (logDownloadRate && download != null && download.isValid()) {
				updateIntervals.setDownloadUpdateSeconds(UPDATE_DEFAULT);
				fTPDownClient = new FTPClient(dataRecorder, DOWN, netIntrfcAddr, LOG_DIRECTORY, download, updateIntervals, debug);
				fTPDownClient.start();
			} else {
				fTPDownClient = null;
			}
			Transfer upload = modem.getUpload();
			/*
			 * Create unique remote filename
			 */
			upload.setRemoteFilePath(netIntrfcAddr + ".bin");
			if (logUploadRate && upload != null && upload.isValid()) {
				updateIntervals.setUploadUpdateSeconds(UPDATE_DEFAULT);
				fTPUpClient = new FTPClient(dataRecorder, UP, netIntrfcAddr, LOG_DIRECTORY, upload, updateIntervals, debug);
				fTPUpClient.start();
			} else {
				fTPUpClient = null;
			}
			Ping ping = modem.getPing();
			if (logPing && ping != null && ping.isValid()) {
				updateIntervals.setPingUpdateSeconds(UPDATE_DEFAULT);
				pinger = new Pinger(dataRecorder, netIntrfcAddr, ping, updateIntervals.getPingUpdateSeconds(), debug);
				pinger.start();
			} else {
				pinger = null;
			}
		} else {
			s_logger.info("startTransferRateSurvey: Network interface " + modem.getNetworkInterface() + " not found");
		}
	}
	private void setSurvey(String value) {
		if (value.toLowerCase().equals("t")) {
			s_logger.info("setSurvey turning on survey unconditionally ");
			turnOnTransferRateSurvey();
		} else if (value.toLowerCase().equals("f")) {
			s_logger.info("setSurvey turning off survey unconditionally ");
			turnOffTransferRateSurvey();
		} else {
			/*
			 * Clear all survey args
			 */
			turnOffTransferRateSurvey();
			try {
				/*
				 * Set one or more survey args
				 */
				SimpleDateFormat simpleDateFormat = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
				StringTokenizer strToke = new StringTokenizer(value, DELIMITER);
				/*
				 * Get start timestamp
				 */
				String startSurveyTimestampStr = strToke.nextToken();
				s_logger.info("setSurvey startSurveyTimestampStr = " + startSurveyTimestampStr);
				startSurveyTimestamp  = simpleDateFormat.parse(startSurveyTimestampStr).getTime();
				/*
				 * Get duration and add to start for stop timestamp
				 */
				String durationStr = strToke.nextToken();
				s_logger.info("setSurvey durationStr = " + durationStr);
				int duration = new Integer(durationStr);
				endSurveyTimestamp = startSurveyTimestamp + (duration * 60000);
				/*
				 * Get transfer type(s) to log
				 */
				String transferTypes = strToke.nextToken().toLowerCase();
				s_logger.info("setSurvey transferTypes = " + transferTypes);
				for (int ii = 0; ii < transferTypes.length(); ii++) {
					switch (transferTypes.charAt(ii)) {
					case 'd':
						s_logger.info("setSurvey setting logDownloadRate = true ");
						logDownloadRate = true;
						break;
					case 'u':
						s_logger.info("setSurvey setting logUploadRate = true ");
						logUploadRate = true;
						break;
					case 'p':
						s_logger.info("setSurvey setting logPing = true ");
						logPing = true;
						break;
					}
				}
				long nowTimestamp = new Date().getTime();
				s_logger.info("setSurvey nowTimestamp " + nowTimestamp + " startSurveyTimestamp " +
						startSurveyTimestamp + " endSurveyTimestamp " + endSurveyTimestamp);
			} catch (Exception ex) {
				s_logger.info("setSurvey turning off survey due to exception " + ex.getMessage());
				turnOffTransferRateSurvey();
			}
			isSurveyArgsModified = true;
		}
	}
	private void turnOnTransferRateSurvey() {
		logDownloadRate = true;
		logUploadRate = true;
		logPing = true;
		startSurveyTimestamp = 0;
		endSurveyTimestamp = MAX_TIMESTAMP;
	}
	private void turnOffTransferRateSurvey() {
		logDownloadRate = false;
		logUploadRate = false;
		logPing = false;
		startSurveyTimestamp = MAX_TIMESTAMP;
		endSurveyTimestamp = 0;
	}

}
