package com.lata.metra.latamasterterminal.xml;

public class XmlWriterException extends Exception {
	static final long serialVersionUID = 1;
	String _error;
	
	/**
	 * Creates an exception object with no specified message.
	 */
	XmlWriterException()
	{
		super("Exception message is not specified.");
	}
	
	/**
	 * Creates an XMLCommandException object with a specified exception message that
	 * is delegated to the superclass of Exception.
	 * @param msg The specified exception message.
	 */
	XmlWriterException(String msg) {
		super(msg);
		_error = msg;
	}
	
	XmlWriterException(Exception e) {
		super(e);
	}
}
