package com.eurotech.reliagate.io.ignition.impl;

import java.io.IOException;

import org.osgi.service.component.ComponentContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.eclipse.kura.KuraException;
import org.eclipse.kura.KuraErrorCode;
import com.eurotech.reliagate.io.i2cbus.I2cBusService;
import com.eurotech.reliagate.io.ignition.IgnitionService;

public class IgnitionImpl implements IgnitionService {

	private static final Logger s_logger = LoggerFactory.getLogger(IgnitionImpl.class);
	private static final byte I2CBUS_ADDRESS = 0x4c;
	private static final byte I2CBUS_IGNITION_BIT = 0x10;
	
	//private static IgnitionImpl s_ignitionSensor = null;
	private I2cBusService m_i2cbus = null;
	
	// ----------------------------------------------------------------
	//
	// Dependencies
	//
	// ----------------------------------------------------------------
	public void setI2cBusService(I2cBusService i2cBusService) {
		s_logger.info("setI2cBusService()");
		m_i2cbus = i2cBusService;
	}

	public void unsetI2cBusService(I2cBusService i2cBusService) {
		s_logger.info("unsetI2cBusService()");
		m_i2cbus = null;
	}
	
	protected void activate(ComponentContext componentContext) {
		s_logger.info("<IAB> activate");
	}
	
	protected void deactivate(ComponentContext componentContext) {
		s_logger.info("<IAB> deactivate");
	}
	
	@Override
	public boolean isIgnitionOn() throws IOException, KuraException {
		
		byte data = m_i2cbus.readByte(I2CBUS_ADDRESS, (byte)0x00);
		int lastError = m_i2cbus.getLastError();
		if(lastError != 0) {
			m_i2cbus.clearLastError();
			throw new KuraException(KuraErrorCode.INTERNAL_ERROR, "Error reading byte from i2c bus. Error code is: " + m_i2cbus.getLastError());
		}
		
		return ((data&I2CBUS_IGNITION_BIT) != 0)? true : false;
	}	
}
