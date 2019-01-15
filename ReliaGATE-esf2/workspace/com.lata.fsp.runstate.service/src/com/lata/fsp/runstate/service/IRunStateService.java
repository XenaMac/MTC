package com.lata.fsp.runstate.service;

import java.util.Enumeration;
import java.util.Hashtable;

public interface IRunStateService {
	
	public static final String SERVICE_NAME = IRunStateService.class.getName();

	public String getState(String key);
		
	public void setState(String key, String value);
	
	public void setStateValues(Hashtable newValues);
	
	public Hashtable getRunstateCopy();
	
	public int getConfigInt(String name);

	public String getConfigString(String name);

	public Hashtable getStateHashtable();

	public Enumeration keys();
	
	public void addRunStateListener(IRunStateListener listener);
	
	public void removeRunStateListener(IRunStateListener listener);
}

