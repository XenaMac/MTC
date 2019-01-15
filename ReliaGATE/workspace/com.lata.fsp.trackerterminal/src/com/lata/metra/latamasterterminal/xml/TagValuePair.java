package com.lata.metra.latamasterterminal.xml;

public class TagValuePair {
	private String tag;
	private String value;
	/**
	 * @param tag
	 * @param value
	 */
	public TagValuePair(String tag, String value) {
		this.tag = tag;
		this.value = value;
	}
	/**
	 * @return the tag
	 */
	public synchronized String getTag() {
		return tag;
	}
	/**
	 * @param tag the tag to set
	 */
	public synchronized void setTag(String tag) {
		this.tag = tag;
	}
	/**
	 * @return the value
	 */
	public synchronized String getValue() {
		return value;
	}
	/**
	 * @param value the value to set
	 */
	public synchronized void setValue(String value) {
		this.value = value;
	}
}

