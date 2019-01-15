package com.lata.fsp.runstate;

import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;
import com.lata.fsp.runstate.service.IRunStateListener;

public class RunStateLoadedListener {

	private List _listeners;
	private boolean loaded = true;
	
	public RunStateLoadedListener() {
		super();
		_listeners = new ArrayList();
	}
	
	public synchronized void addListener(IRunStateListener listener) {
		_listeners.add(listener);
		if(loaded){
			listener.loaded();
		}
	}

	public synchronized void loaded() {
		cleanListeners();
		for (Iterator i = _listeners.iterator(); i.hasNext();)
			((IRunStateListener) i.next()).loaded();
	}
	
	public synchronized void publishNewState(String key, String value){
		cleanListeners();
		for (Iterator i = _listeners.iterator(); i.hasNext();){
			try{
			((IRunStateListener) i.next()).newState(key, value);
			}catch(Throwable x){
				x.printStackTrace();
			}
		}
	}
	
	public synchronized void removeListener(IRunStateListener listener) {
		_listeners.remove(listener);
	}
	private synchronized void cleanListeners(){
		for (Iterator i = _listeners.iterator(); i.hasNext();){
			IRunStateListener listener =(IRunStateListener) i.next();
			if(listener==null)_listeners.remove(listener);
		}
	}
}
