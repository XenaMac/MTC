package com.eurotech.reliagate.io.leds.impl;

import java.util.concurrent.ScheduledFuture;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

import org.osgi.service.component.ComponentContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.eclipse.kura.KuraException;
import org.eclipse.kura.KuraErrorCode;
import com.eurotech.reliagate.io.i2cbus.I2cBusService;
import com.eurotech.reliagate.io.i2cbus.impl.I2cBusImpl;
import com.eurotech.reliagate.io.leds.Led;
import com.eurotech.reliagate.io.leds.LedService;
import com.eurotech.reliagate.io.leds.LedState;

public class LedImpl implements LedService {

	private static final Logger s_logger = LoggerFactory.getLogger(LedImpl.class);
	private final static long THREAD_TERMINATION_TOUT = 1; // in seconds
	//private static LedImpl s_ReliagateLedControl = null;
	private static final byte I2C_ADDRESS = 0x4c;
	
	private I2cBusService m_i2cbus = null;
	private int m_blinkCnt = 0;
	private LedState [] m_ledState = {LedState.OFF, LedState.OFF};
	private ScheduledThreadPoolExecutor  m_executor;
	private ScheduledFuture<?>  m_ledBlinkerTask;
	
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
		s_logger.info("<IAB> [+] activate()");
		m_executor = new ScheduledThreadPoolExecutor(1);
		try {
			i2cInit();
		} catch (KuraException e) {
			e.printStackTrace();
		}
		s_logger.info("<IAB> [-] activate()");
	}
	
	protected void deactivate(ComponentContext componentContext) {
		s_logger.info("<IAB> deactivate()");
		if (m_executor != null) {
			s_logger.debug("Terminating LedBlinker Thread ...");
    		m_executor.shutdownNow();
    		try {
				m_executor.awaitTermination(THREAD_TERMINATION_TOUT, TimeUnit.SECONDS);
			} catch (InterruptedException e) {
				s_logger.warn("Interrupted", e);
			}
    		s_logger.info("LedBlinker Thread terminated? - {}", m_executor.isTerminated());
			m_executor = null;
		}
		
		m_i2cbus = null;	
	}
	
	/*
	private LedImpl() throws KuraException {
		
		m_executor = new ScheduledThreadPoolExecutor(1);
		i2cInit();
	}
	
	public static LedService getInstance () throws KuraException {
		if (s_ReliagateLedControl == null) {
			s_ReliagateLedControl = new LedImpl();
		}
		return s_ReliagateLedControl;
	}
	*/
	
	@Override
	public LedState getState(Led led) {
		
		return m_ledState[led.getValue()];
	}

	@Override
	public void setState(Led led, LedState state) throws KuraException {
		
		s_logger.debug("setState() :: setting " + led + " LED " + state);
		
		if ((m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING_SLOW) ||
				(m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING) ||
				(m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING_FAST) || 
				(m_ledState[Led.TOP.getValue()] == LedState.BLINKING_SLOW) || 
				(m_ledState[Led.TOP.getValue()] == LedState.BLINKING) ||
				(m_ledState[Led.TOP.getValue()] == LedState.BLINKING_FAST)) {
			// set state and start worker
			m_ledState[led.getValue()] = state;
			
			if ((m_ledBlinkerTask == null) || m_ledBlinkerTask.isDone() || m_ledBlinkerTask.isCancelled()) {
				s_logger.debug("Starting blinker thread ..");
				m_blinkCnt = 0;
				//m_ledBlinker.start();
				m_ledBlinkerTask = m_executor.scheduleAtFixedRate(new Runnable() {
		    		@Override
		    		public void run() {
		    			Thread.currentThread().setName("LedBlinker");
		    			ledBlinkerThread();
		    	}}, 0, 250, TimeUnit.MILLISECONDS);
			}
		} else {
			if ((state == LedState.OFF) || (state == LedState.ON)) {
				// stop worker
				if (m_ledBlinkerTask != null) {
					while(!(m_ledBlinkerTask.isDone() || m_ledBlinkerTask.isCancelled())) {
						sleep(200);
						s_logger.debug("Stopping blinker thread ...");
						m_ledBlinkerTask.cancel(true);
					}
				}
				m_blinkCnt = 0;
				
				byte mask = i2cGet();
				if (state == LedState.OFF) {
					mask &= ~led.getMask();
				} else if (state == LedState.ON) {
					mask |= led.getMask();
				}
				i2cSet(mask);
				m_ledState[led.getValue()] = state;
			} else {
				m_ledState[led.getValue()] = state;
				if ((m_ledBlinkerTask == null) || m_ledBlinkerTask.isDone() || m_ledBlinkerTask.isCancelled()) {
					s_logger.debug("Starting blinker thread ...");
					m_blinkCnt = 0;
					//m_ledBlinker.start();
					m_ledBlinkerTask = m_executor.scheduleAtFixedRate(new Runnable() {
			    		@Override
			    		public void run() {
			    			Thread.currentThread().setName("LedBlinker");
			    			ledBlinkerThread();
			    	}}, 0, 250, TimeUnit.MILLISECONDS);
				}
			}
		}
	}
	
	/*
	 * LED blinker thread
	 */
	private boolean ledBlinkerThread () {
		
		byte mask = getMask();
		try {
			byte byteRead = i2cGet();
			i2cSet((byte) (mask | (byteRead & 0x3f)));
		} catch (KuraException e) {
			e.printStackTrace();
		}
		
		m_blinkCnt = (m_blinkCnt < 7)? (m_blinkCnt+1) : 0;		
		sleep(250);
		return true;
	}
	
	
	/*
	 * Reports mask based on blink counter and states of ReliaGATE LEDs
	 */
	private byte getMask () {
		byte mask = 0;
		
		if (m_ledState[Led.BOTTOM.getValue()] == LedState.OFF) {
			mask &= ~Led.BOTTOM.getMask();
		}
		
		if (m_ledState[Led.TOP.getValue()] == LedState.OFF) {
			mask &= ~Led.TOP.getMask();
		}
		
		if (m_ledState[Led.BOTTOM.getValue()] == LedState.ON) {
			mask |= Led.BOTTOM.getMask();
		}
		
		if (m_ledState[Led.TOP.getValue()] == LedState.ON) {
			mask |= Led.TOP.getMask();
		}
		
		if (m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING_FAST) {
			switch (m_blinkCnt) {
			case 0:
			case 2:
			case 4:
			case 6:
				mask &= ~Led.BOTTOM.getMask();
				break;
			case 1:
			case 3:
			case 5:
			case 7:
				mask |= Led.BOTTOM.getMask();
				break;
			}
		}
		
		if (m_ledState[Led.TOP.getValue()] == LedState.BLINKING_FAST) {
			switch (m_blinkCnt) {
			case 0:
			case 2:
			case 4:
			case 6:
				mask &= ~Led.TOP.getMask();
				break;
			case 1:
			case 3:
			case 5:
			case 7:
				mask |= Led.TOP.getMask();
				break;
			}
		}
		
		if (m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING) {
			switch (m_blinkCnt) {
			case 0:
			case 1:
			case 4:
			case 5:
				mask &= ~Led.BOTTOM.getMask();
				break;
			case 2:
			case 3:
			case 6:
			case 7:
				mask |= Led.BOTTOM.getMask();
				break;
			}
		}
		
		if (m_ledState[Led.TOP.getValue()] == LedState.BLINKING) {
			switch (m_blinkCnt) {
			case 0:
			case 1:
			case 4:
			case 5:
				mask &= ~Led.TOP.getMask();
				break;
			case 2:
			case 3:
			case 6:
			case 7:
				mask |= Led.TOP.getMask();
				break;
			}
		}
		
		if (m_ledState[Led.BOTTOM.getValue()] == LedState.BLINKING_SLOW) {
			switch (m_blinkCnt) {
			case 0:
			case 1:
			case 2:
			case 3:
				mask &= ~Led.BOTTOM.getMask();
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				mask |= Led.BOTTOM.getMask();
				break;
			}
		}
		
		if (m_ledState[Led.TOP.getValue()] == LedState.BLINKING_SLOW) {
			switch (m_blinkCnt) {
			case 0:
			case 1:
			case 2:
			case 3:
				mask &= ~Led.TOP.getMask();
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				mask |= Led.TOP.getMask();
				break;
			}
		}
		
		return mask;
	}
	
	/*
	 * initialization on I2C bus
	 */
	private synchronized void i2cInit() throws KuraException {
		try {
			//m_i2cbus = I2cBusImpl.getInstance();
			m_i2cbus.writeByte(I2C_ADDRESS, (byte)0x03, (byte)0x10);
			if(m_i2cbus.getLastError() != 0) {
				throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Failed to initialize I2C - error code is: " + m_i2cbus.getLastError());
			}
			setState(Led.BOTTOM, LedState.OFF);
			setState(Led.TOP, LedState.OFF);
		} catch (Exception e) {
			e.printStackTrace();
			throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Failed to initialize I2C - ", e);
		}
	}
	
	/*
	 * Reports a byte read from I2C bus
	 */
	private synchronized byte i2cGet() throws KuraException {
		
		byte ret = 0;
		try {
			ret = m_i2cbus.readByte(I2C_ADDRESS, (byte)0x01);
			if(m_i2cbus.getLastError() != 0) {
				throw new KuraException(KuraErrorCode.INTERNAL_ERROR, "Error reading a byte from i2c bus. Error code is: " + m_i2cbus.getLastError());
			}
		} catch (Exception e) {
			throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Error reading a byte from i2c bus", e);
		}
		return ret;
	}
	
	/*
	 * Writes a byte to I2C bus
	 */
	private synchronized void i2cSet (byte mask) throws KuraException {
		try {
			m_i2cbus.writeByte(I2C_ADDRESS, (byte)0x01, mask);
			if(m_i2cbus.getLastError() != 0) {
				throw new KuraException(KuraErrorCode.INTERNAL_ERROR, "Failed to write a byte to i2c bus. Error code is: " + m_i2cbus.getLastError());
			}
		} catch (Exception e) {
			throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Failed to write a byte to I2C bus", e);
		}
	}
	
	private void sleep(long millis) {
		try {
			Thread.sleep(millis);
		} catch (InterruptedException e) {
			// ignore
		}
	}
}
