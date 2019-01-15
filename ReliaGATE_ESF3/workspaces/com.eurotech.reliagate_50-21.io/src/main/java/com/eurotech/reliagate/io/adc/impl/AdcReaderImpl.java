package com.eurotech.reliagate.io.adc.impl;

import java.io.IOException;

import org.osgi.service.component.ComponentContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.eclipse.kura.KuraException;
import com.eurotech.reliagate.io.adc.Adc;
import com.eurotech.reliagate.io.adc.AdcReaderService;
import com.eurotech.reliagate.io.i2cbus.I2cBusService;
import com.eurotech.reliagate.io.i2cbus.impl.I2cBusImpl;

public class AdcReaderImpl implements AdcReaderService {

	private static final Logger s_logger = LoggerFactory.getLogger(AdcReaderImpl.class);
	//private static AdcReaderImpl s_adcReader = null;
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
	
	/*
	public static AdcReaderService getInstance () {	
		if (s_adcReader == null) {
			s_adcReader = new AdcReaderImpl();
			s_adcReader.m_i2cbus = I2cBusImpl.getInstance();
		}
		return s_adcReader;
	}
	*/
	
	@Override
	public float getVoltage(Adc adc) throws IOException, KuraException {
		
		short adcData = m_i2cbus.readWord(adc.getAddress(), (byte)0);
		
		int v = ((adcData&0x0f) << 4) | (adcData>>12) & 0x0f;
		float voltage = (float)((v * 3.29) / 17.77);
		return voltage;
	}
}
