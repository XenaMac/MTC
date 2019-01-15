package com.eurotech.reliagate.io.i2cbus.impl;

import java.io.IOException;

import org.osgi.service.component.ComponentContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.eclipse.kura.KuraException;
import org.eclipse.kura.KuraErrorCode;
import com.eurotech.reliagate.io.i2cbus.I2cBusService;

public class I2cBusImpl implements I2cBusService {
	
	private static final String LABEL = I2cBusImpl.class.getName() + ": ";
	private static final Logger s_logger = LoggerFactory.getLogger(I2cBusImpl.class);
	//private static I2cBusImpl s_i2cBus = null; 
	
	static {
		try {
			System.loadLibrary("i2cbus");
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	private native int I2CBusOpen();
	private native boolean I2CBusClose(int handle);
	private native boolean I2CBusWriteByte(int handle, byte slaveAddress, byte command, byte data);
	private native boolean I2CBusWriteWord(int handle, byte slaveAddress, byte command, short data);
	private native boolean I2CBusWriteBlock(int handle, byte slaveAddress, byte command, byte[] data);
	private native byte I2CBusReadByte(int handle, byte slaveAddress, byte command);
	private native short I2CBusReadWord(int handle, byte slaveAddress, byte command);
	private native byte[] I2CBusReadBlock(int handle, byte slaveAddress, byte command);
	private native int I2CBusGetLastError(int handle);
	private native int I2CBusClearLastError(int handle);
	
	/*
	public static I2cBusImpl getInstance() {
		if (s_i2cBus == null) {
			s_i2cBus = new I2cBusImpl();
		}
		return s_i2cBus;
	}
	*/
	
	protected void activate(ComponentContext componentContext) {
		s_logger.info("<IAB> activate");
	}
	
	protected void deactivate(ComponentContext componentContext) {
		s_logger.info("<IAB> deactivate");
	}
	
	@Override
	public synchronized void writeByte(byte slaveAddress, byte command,
			byte data) throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "writeByte() :: unable to open I2CBus");
		}
		if (!I2CBusWriteByte(handle, slaveAddress, command, data)) {
			close(handle);
			throw new IOException(LABEL + "writeByte() failed on I2CBus");
		}
		close(handle);
	}
	
	@Override
	public synchronized void writeWord(byte slaveAddress, byte command,
			short data) throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "writeWord() :: unable to open I2CBus");
		}
		if (!I2CBusWriteWord(handle, slaveAddress, command, data)) {
			close(handle);
			throw new IOException(LABEL + "writeWord() failed on I2CBus");
		}	
		close(handle);
	}
	
	@Override
	public synchronized void writeBlock(byte slaveAddress, byte command,
			byte[] data) throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "writeBlock() :: unable to open I2CBus");
		}
		if (!I2CBusWriteBlock(handle, slaveAddress, command, data)) {
			close(handle);
			throw new IOException(LABEL + "writeBlock() failed on I2CBus");
		}
		close(handle);
	}
	
	@Override
	public synchronized byte readByte(byte slaveAddress, byte command)
			throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "unable to open I2CBus");
		}
		byte result = I2CBusReadByte(handle, slaveAddress, command);
		close(handle);
		return result;
	}
	
	@Override
	public synchronized short readWord(byte slaveAddress, byte command)
			throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "unable to open I2CBus");
		}
		short result = I2CBusReadWord(handle, slaveAddress, command);
		close(handle);
		return result;
	}
	
	@Override
	public synchronized byte[] readBlock(byte slaveAddress, byte command)
			throws IOException, KuraException {
		
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "unable to open I2CBus");
		}
		byte[] result = I2CBusReadBlock(handle, slaveAddress, command);
		close(handle);
		return result;
	}
	
	@Override
	public int getLastError() throws IOException, KuraException {
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "unable to open I2CBus");
		}
		int lastError = I2CBusGetLastError(handle);
		close(handle);
		return lastError;
	}
	
	@Override
	public void clearLastError() throws IOException, KuraException {
		int handle = open();
		if(handle < 0) {
			throw new IOException(LABEL + "unable to open I2CBus");
		}
		I2CBusClearLastError(handle);
		close(handle);
	}
	
	/*
	 * Opens I2C device
	 */
	private int open() throws KuraException {
		int handle = I2CBusOpen();
		if (handle < 0) {
			throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Failed to open I2C bus");
		}
		return handle;
	}

	/*
	 * Closes I2C device
	 */
	private void close(int handle) throws KuraException {
		if (!I2CBusClose(handle)) {
			throw new KuraException (KuraErrorCode.INTERNAL_ERROR, "Failed to close I2C bus");
		}
	}
}
