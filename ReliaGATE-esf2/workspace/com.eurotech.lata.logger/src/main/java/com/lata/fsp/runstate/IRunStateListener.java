package com.lata.fsp.runstate;

public interface IRunStateListener {
	/*
	 * Used to inform listeners that the runstate has completed loading
	 */
	public void loaded();
	
	/*
	 * Used to inform listeners that a state has been updated
	 */
	public void newState(String key, String value);
}
