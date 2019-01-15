package com.eurotech.reliagate.io.ignition;

import java.io.IOException;

import org.eclipse.kura.KuraException;

public interface IgnitionService {

	public boolean isIgnitionOn() throws IOException, KuraException;
}
