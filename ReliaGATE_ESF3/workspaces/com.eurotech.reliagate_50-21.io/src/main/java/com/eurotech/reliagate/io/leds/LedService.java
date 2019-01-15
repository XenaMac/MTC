package com.eurotech.reliagate.io.leds;

import org.eclipse.kura.KuraException;

public interface LedService {
	
	/**
	 * Reports state of specified ReliaGATE LED
	 * 
	 * @param led - LED as {@link Led}
	 * @return LED state as {@link LedState}
	 * @throws ReliagateLedException
	 */
	public LedState getState(Led led) /*throws KuraException*/;
	
	/**
	 * Sets state of specified ReliaGATE LED
	 * 
	 * @param led - LED as {@link Led}
	 * @param state - LED state as {@link LedState}
	 * @throws ReliagateLedException
	 */
	public void setState (Led led, LedState state) throws KuraException;	
}
