package com.lata.metra.latamasterterminal.xml;

import java.util.Enumeration;
import java.util.Hashtable;
import java.io.IOException;
import java.io.StringReader;

import org.xml.sax.Attributes;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;
import org.xml.sax.XMLReader;
import org.xml.sax.helpers.DefaultHandler;
import org.xml.sax.helpers.XMLReaderFactory;

public class CommandParserHandler extends DefaultHandler {
	
	private Hashtable _procdata = null;
	private StringBuffer _buffer = new StringBuffer();
	private boolean _haveFirstElement = true;
	
	public CommandParserHandler() { }
	
	public Hashtable parse(String packet) {
		_procdata = new Hashtable();
		_buffer = new StringBuffer();
		_haveFirstElement = true;

		try {
    		XMLReader xmlReader = XMLReaderFactory.createXMLReader();
    		xmlReader.setContentHandler(this);

    		StringReader stringReader = new StringReader(packet);
    		xmlReader.parse(new InputSource(stringReader));
		}
		catch (IOException ioe) {
			System.out.println("parse()->" + ioe.getMessage());
		}
		catch (SAXException saxe) {
			System.out.println("parse()->" + saxe.getMessage());
		}
		return _procdata;
	}
	
	public void dump(Hashtable table) {
		String output = "";
    	try {
	    	Enumeration e = table.keys();
	    	System.out.println("dump()->");
	    	String key = "";
	    	while (e.hasMoreElements()) {
	    		key = (String) e.nextElement();
	    		System.out.println("key: " + key);
	    		output += table.get(key);
	    		System.out.println("value: " + table.get(key));
	    	}
    	}
    	catch (Exception e) {
    		System.out.println("dump()->" + e.getMessage());
    	}
	}
	
	public void startDocument() { }

	public void endDocument() {	}

	public void startElement(String uri, String lname, String qname, Attributes attributes) {
		if (_haveFirstElement) {
			_procdata.put("Command", qname);
			_haveFirstElement = false;
		}
    	_buffer.setLength(0);
	}

    public void endElement(String uri, String lname, String qname) {
    	if (!_haveFirstElement) {
    		String command = (String) _procdata.get("Command");
    		if (!command.equals(qname)) {
    			_procdata.put(qname, _buffer.toString().trim());	
    		}
    	}
    	_buffer.setLength(0);
    }

    public void characters (char ch[], int start, int length) {
    	_buffer.append(ch, start, length);
	}
} 