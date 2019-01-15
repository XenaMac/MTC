package com.lata.fsp.trackerterminal.bundle;

import org.eclipse.soda.sat.core.framework.BaseBundleActivator;

import com.esf.device.gps.service.IGPSService;
import com.lata.fsp.runstate.service.IRunStateService;
import com.lata.fsp.trackerterminal.TrackerTerminal;

public class Activator extends BaseBundleActivator {
	
	private TrackerTerminal trackerTerminal;
	
	protected void activate() {
		trackerTerminal = new TrackerTerminal();
		trackerTerminal.bind(getIGPSService(), getIRunStateService());
		trackerTerminal.start();
	}
	
	protected void deactivate() {
		trackerTerminal.unbind();
		trackerTerminal.stopThread();
	}
	
	private IGPSService getIGPSService() {
		return (IGPSService) getImportedService(IGPSService.SERVICE_NAME);
	}
	
	private IRunStateService getIRunStateService() {
		return (IRunStateService) getImportedService(IRunStateService.SERVICE_NAME);
	}
	
	protected String[] getImportedServiceNames() {
		return new String[] {
				IGPSService.SERVICE_NAME, IRunStateService.SERVICE_NAME
		};
	}

}
