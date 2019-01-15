package com.eurotech.reliagate.io.i2cbus;

import java.io.IOException;

import org.eclipse.kura.KuraException;

public interface I2cBusService {
	
	/**
	 * Writes byte to I2C bus 
	 * 
	 * @param slaveAddress 	- the slave address to write to
	 * @param command 		- the command to write
	 * @param data			- the data to write
	 * @throws IOException  - if the write can not succeed
	 * @throws KuraException - if open() fails
	 */
	public void writeByte(byte slaveAddress, byte command, byte data) throws Exception;
	
	/**
	 * Writes word to I2C bus 
	 * 
	 * @param slaveAddress 	- the slave address to write to
	 * @param command 		- the command to write
	 * @param data			- the data to write
	 * @throws IOException  - if the write can not succeed
	 * @throws KuraException - if open() fails
	 */
	public void writeWord(byte slaveAddress, byte command, short data) throws Exception; 
	
	
	/**
	 * writes block of data to the I2Cbus
	 * 
	 * @param slaveAddress	- the slave address to write to
	 * @param command		- the command to write
	 * @param data			- the data to write
	 * @throws IOException	- if the write can not succeed
	 * @throws KuraException - if open() fails
	 */
	public void writeBlock(byte slaveAddress, byte command, byte[] data) throws Exception;

	
	/**
	 * reads a byte from the I2CBus
	 * 
	 * @param slaveAddress	- the slave address to read from
	 * @param command		- the command to send
	 * @return				- the byte read from the I2CBus
	 * @throws IOException	- if the read fails
	 * @throws KuraException - if open() fails
	 */
	public byte readByte(byte slaveAddress, byte command) throws IOException, KuraException;
	
	
	/**
	 * reads a 16 bit word from the I2CBus
	 * 
	 * @param slaveAddress	the slave address to read from
	 * @param command		the command to send
	 * @return				the word read from the I2CBus
	 * @throws Exception	if the read fails
	 */
	public short readWord(byte slaveAddress, byte command) throws IOException, KuraException;
	
	/**
	 * reads a block of bytes from the I2CBus
	 * 
	 * @param slaveAddress	the slave address to read from
	 * @param command		the command to send
	 * @return				an array of bytes read from the I2CBus
	 * @throws IOException	- if the read fails
	 * @throws KuraException - if open() fails
	 */
	public byte[] readBlock(byte slaveAddress, byte command) throws IOException, KuraException;

	/**
	 * gets the last error provided by the I2CBus native library
	 * 
	 * @return				an int representing the last error provided by the
	 * 						I2CBus native library
	 * @throws KuraException	if there is an error reading the last error
	 */
	public int getLastError() throws IOException, KuraException;
	
	/**
	 * clears last error provided by the I2CBus native library
	 * 
	 * @throws IOException
	 * @throws KuraException
	 */
	public void clearLastError() throws IOException, KuraException;

}
