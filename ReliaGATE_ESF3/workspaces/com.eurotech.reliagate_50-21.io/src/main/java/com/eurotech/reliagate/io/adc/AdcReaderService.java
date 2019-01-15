package com.eurotech.reliagate.io.adc;

import java.io.IOException;

import org.eclipse.kura.KuraException;

public interface AdcReaderService {

	public float getVoltage(Adc adc) throws IOException, KuraException;
}
