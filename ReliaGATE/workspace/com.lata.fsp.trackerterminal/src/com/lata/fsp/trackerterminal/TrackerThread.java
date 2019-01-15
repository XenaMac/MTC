package com.lata.fsp.trackerterminal;

public abstract class TrackerThread extends Thread implements TrackerTerminalConstants {
	
	private boolean active = true;

	public boolean isActive() {
		return this.active;
	}

	public void setActive(boolean active) {
		this.active = active;
	}
	
	public void stopThread() {
		this.active = false;
		this.interrupt();
	}

}
