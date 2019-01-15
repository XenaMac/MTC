package com.lata.fsp.runstate;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.Hashtable;

public class ConfigFile implements RunStateConstants {
	private static Hashtable _config = null;
	private static boolean _haveConfig = false;
	public ConfigFile(String CONFIG_FILE){
		log("Base.readConfigFile(): reading " + CONFIG_FILE);
		_haveConfig = true;
		String contents = new String();
	    try {
	    	BufferedReader input =  new BufferedReader(new FileReader(CONFIG_FILE));
	    	try {
	    		String line = null; 
	    		while (( line = input.readLine()) != null){
	    			if(line.indexOf(SERVER_IP)>0)
	    				log("Base.readConfigFile(): read SERVER_IP in: "+line);
	    			contents += line.trim();
	    		}
	    		_config = new Hashtable();
	    		ConfigParserHandler cphandler = new ConfigParserHandler();
                _config = cphandler.parse(contents);
                
                //cphandler.dump(_config);
        	}
	    	finally {
	    		input.close();
	    	}
	    }
	    catch (IOException ex) {
	    	_haveConfig = false;
	    	log(ex.getMessage());
	    }
	}
	protected synchronized String getConfigString(String name) {
		if (_haveConfig) {
			if (_config.containsKey(name)) {
				log("configuration is loaded. returning "+(String) _config.get(name)+" for "+name);
				return (String) _config.get(name);
			}
			else {
				log("configuration parameter '" + name + "' not available.");
			}
		}
		else {
			log("configuration not loaded.");
		}
		return "";
	}
	
	protected synchronized int getConfigInt(String name) {
		if (_haveConfig) {
			if (_config.containsKey(name)) {
				try {
					int i = Integer.parseInt(getConfigString(name));
					return i;
				}
				catch (NumberFormatException e) {
					return -1;
				}
			}
			else {
				log("configuration parameter '" + name + "' not available.");
			}
		}
		else {
			log("configuration not loaded.");
		}
		return -1;
	}
	
	public void log(String str){
		System.out.println(ConfigFile.class.getName()+": "+str);
	}
}
