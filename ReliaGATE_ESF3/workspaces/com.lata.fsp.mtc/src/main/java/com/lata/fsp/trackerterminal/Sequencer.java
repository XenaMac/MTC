package com.lata.fsp.trackerterminal;


import java.text.SimpleDateFormat;
import java.util.Date;

public class Sequencer {

	private Object lock = new Object();
	private static Sequencer _instance = null;
	private int sequenceNumber = 0;
	
	private Sequencer() {
		Date now = new Date(System.currentTimeMillis());
		String HH = new SimpleDateFormat("HH").format(now);
		String mm = new SimpleDateFormat("mm").format(now);
		String ss = new SimpleDateFormat("ss").format(now);
		try{
			sequenceNumber = 3600*Integer.parseInt(HH)+60*Integer.parseInt(mm)+Integer.parseInt(ss);
		}catch(NumberFormatException nfx){
			sequenceNumber = (int)System.currentTimeMillis()%(24*3600);
		}
	}

	public synchronized static Sequencer getInstance() {
		if (_instance == null) {
			_instance = new Sequencer();
		}
		return _instance;
	}
	
	public int getSequence() {
		int sn = 0;
		synchronized(lock) {
			sn = sequenceNumber++;
			if(sequenceNumber>=24*3600)
				sequenceNumber = 0;
		}
		return sn;
	}
}

