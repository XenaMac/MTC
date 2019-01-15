package com.lata.kmlgenerator;

import java.io.File;
import java.io.FilenameFilter;

public class ExtensionFilter implements FilenameFilter {

	private String myExtension;

	public
	ExtensionFilter( String extension )
	{
		myExtension = extension;
	}

	public boolean
	accept( File dir, String name )
	{
		return( name.endsWith( myExtension ) );
	}

}
