package com.lata.metra.latamasterterminal.xml;

import java.io.StringWriter;
import java.io.Writer;
import java.util.Hashtable;
import java.util.ArrayList;
import java.util.Enumeration;

public class GeneratorHandler {

	public GeneratorHandler() {
		//super(_runState);
		}
	
	public synchronized String generate(ArrayList data) {
		String xml = null;
		
		Writer writer = new StringWriter();
		XmlWriter xwriter = new XmlWriter(writer);
		
		try {
			Hashtable temp = (Hashtable) data.get(0);
			String command = (String) temp.get("Command");
			xwriter.writeEntity(command);
			for (int i=1; i<data.size(); i++) {
				temp = (Hashtable) data.get(i);
				Enumeration e = temp.keys();
				while (e. hasMoreElements()) {
					String key = (String) e.nextElement();
					xwriter.writeEntity(key);
					xwriter.writeText((String)temp.get(key));
					xwriter.endEntity();
				}	
			}
			xwriter.endEntity();
			xml = writer.toString();
		}
		catch (XmlWriterException xwe) {
		//	log("getConnection()->XmlWriter failed.");
		}
		return xml;
	}
	
	public synchronized String generate(Hashtable data) {
		String xml = null;
		
		Writer writer = new StringWriter();
		XmlWriter xwriter = new XmlWriter(writer);
		
		try {
			String command = (String) data.get("Command");
			xwriter.writeEntity(command);
			data.remove("Command");
			
			String id = (String) data.get("Id");
			if(id!=null){
				xwriter.writeEntity("Id");
				xwriter.writeText(id);
				data.remove("Id");
				xwriter.endEntity();
			}
			Enumeration e = data.keys();
			while (e. hasMoreElements()) {
				String key = (String) e.nextElement();
				//if("LastAudioPlayed".equals(key))continue;
				xwriter.writeEntity(key);
				xwriter.writeText((String)data.get(key));
				xwriter.endEntity();
			}
			xwriter.endEntity();
			xml = writer.toString();
		}
		catch (XmlWriterException xwe) {
		//	log("getConnection()->XmlWriter failed.");
		}
		return xml;
	}
}
