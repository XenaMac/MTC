package com.lata.fsp.runstate.bundle;

import org.eclipse.soda.sat.core.framework.BaseBundleActivator;
import org.eclipse.soda.sat.core.util.LogUtility;

import com.lata.fsp.runstate.RunState;
import com.lata.fsp.runstate.service.IRunStateService;

public class Activator extends BaseBundleActivator {
	
	private RunState runstate;
	
	protected void activate() {
		runstate = new RunState();
		runstate.start();
		addExportedService(IRunStateService.SERVICE_NAME, runstate, null);
		log("RunState is loaded and active...");

	}

	protected void deactivate(){
		runstate.stopThread();
		removeExportedService(IRunStateService.SERVICE_NAME);
	}
	
	public void log(String str){
		LogUtility.logInfo(RunState.class.getName()+": "+str);
	}
}

