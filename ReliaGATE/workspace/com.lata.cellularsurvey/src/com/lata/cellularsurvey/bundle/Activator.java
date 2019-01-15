package com.lata.cellularsurvey.bundle;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import org.eclipse.soda.sat.core.framework.BaseBundleActivator;
import com.lata.cellularsurvey.CellularSurvey;
import com.lata.fsp.runstate.service.IRunStateService;
import com.esf.device.gps.service.IGPSService;


/**
 * @author Aarmijo
 *
 */
public class Activator extends BaseBundleActivator {

	private static final String LABEL = Activator.class.getName() + ": ";
	private static final String LOG_DIRECTORY = "/lata/logs/";
	private static final String LOG_FILE = "activator.log";
	private CellularSurvey cellularSurvey = null;
	private SimpleDateFormat dateTimeFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");


	/* (non-Javadoc)
	 * @see org.eclipse.soda.sat.core.framework.BaseBundleActivator#activate()
	 */
	protected void activate() {

		writeDebugLog(LABEL + "activate()");
		cellularSurvey = new CellularSurvey();
		cellularSurvey.bind(getIGPSService(), getIRunStateService());
		cellularSurvey.start();
	}

	/* (non-Javadoc)
	 * @see org.eclipse.soda.sat.core.framework.BaseBundleActivator#deactivate()
	 */
	protected void deactivate() {

		writeDebugLog(LABEL + "deactivate()");
		cellularSurvey.stopThread();
	}
	
	private IGPSService getIGPSService() {
		return (IGPSService) getImportedService(IGPSService.SERVICE_NAME);
	}
	
	private IRunStateService getIRunStateService() {
		return (IRunStateService) getImportedService(IRunStateService.SERVICE_NAME);
	}
	
	protected String[] getImportedServiceNames() {
		return new String[] { IGPSService.SERVICE_NAME, IRunStateService.SERVICE_NAME };
	}
	
	public void writeDebugLog(String msg) {
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

