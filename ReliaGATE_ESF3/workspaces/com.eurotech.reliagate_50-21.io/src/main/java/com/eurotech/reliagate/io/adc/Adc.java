package com.eurotech.reliagate.io.adc;

public enum Adc {

	One((byte)0xA2), Two((byte)0xA4);
	
	private byte m_address;
	
	private Adc(byte address) {
		m_address = address;
	}
	
	public byte getAddress() {
		return m_address;
	}
}
