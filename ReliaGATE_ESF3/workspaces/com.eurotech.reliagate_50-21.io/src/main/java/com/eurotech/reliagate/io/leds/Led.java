package com.eurotech.reliagate.io.leds;

public enum Led {

	BOTTOM(0), TOP(1);
	private int m_led = 0;
	
	/*
	 * constructor
	 */
	private Led(int led) {
		m_led = led;
	}
	
	/**
	 * Reports value
	 * 
	 * @return value as {@link int}
	 */
	public int getValue() {
		return m_led;
	}
	
	/**
	 * Reports mask (for I2C write operation)
	 * 
	 * @return mask as {@link byte}
	 */
	public byte getMask() {
		
		byte ret = 0;
		switch (m_led) {
		case 0:
			ret = (byte)0x40; 
			break;
		case 1:
			ret = (byte)0x80;
			break;
		}
		
		return ret;
	}
}
