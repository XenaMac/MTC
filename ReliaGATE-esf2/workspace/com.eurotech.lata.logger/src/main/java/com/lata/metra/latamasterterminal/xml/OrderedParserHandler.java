package com.lata.metra.latamasterterminal.xml;

import java.util.Enumeration;
import java.util.Vector;
import java.io.IOException;
import java.io.StringReader;

import org.xml.sax.Attributes;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;
import org.xml.sax.XMLReader;
import org.xml.sax.helpers.DefaultHandler;
import org.xml.sax.helpers.XMLReaderFactory;

public class OrderedParserHandler extends DefaultHandler {
	
	private Vector _procdata = null;
	private StringBuffer _buffer = new StringBuffer();
	private boolean _haveFirstElement = true;
	
	public OrderedParserHandler() { }
	
	public Vector parse(String packet) {
		_procdata = new Vector();
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
	
	public void dump(Vector table) {
    	try {
	    	System.out.println("In OrderedParserHandler.dump()->");
	    	TagValuePair pair = null;
	        for (Enumeration e = table.elements(); e.hasMoreElements();) {
	    		pair = (TagValuePair) e.nextElement();
	    		System.out.println("element: " + pair.getTag());
	    		System.out.println("value: " + pair.getValue());
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
			_procdata.add(new TagValuePair("Command", qname));
			_haveFirstElement = false;
		}
    	_buffer.setLength(0);
	}

    public void endElement(String uri, String lname, String qname) {
    	if (!_haveFirstElement) {
    		if (!qname.equals("Command")) {
    			_procdata.add(new TagValuePair(qname, _buffer.toString().trim()));
    		}
    	}
    	_buffer.setLength(0);
    }

    public void characters (char ch[], int start, int length) {
    	_buffer.append(ch, start, length);
	}
} 
