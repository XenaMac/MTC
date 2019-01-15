package com.eurotech.reliagate.io.leds;

public enum LedState {

	OFF(0), ON(1), BLINKING_SLOW(2), BLINKING(3), BLINKING_FAST(4);
	
	private int m_state;
	
	/*
	 * constructor
	 */
	private LedState (int status) {
		m_state = status;
	}
	
	/**
	 * Reports state of ReliaGATE LED
	 * 
	 * @return LED state as {@link int}
	 */
	public int getState() {
		return m_state;
	}
}
