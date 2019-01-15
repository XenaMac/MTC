package com.lata.kmlgenerator;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.StringTokenizer;
import java.util.Vector;

public class KMLGenerator {
	private static final int NUMBER_DATA_COLS = 5;
	private static final int NUMBER_NON_CELL_COLS = 6;
	private static final long SIXTY_SECONDS = 60000;
	private static final String BAD_DOP = "20";

	private static String KML_HEADER1 =  "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
		      + "<kml xmlns=\"http://www.opengis.net/kml/2.2\">"
		      + "<Document>"
		      + "<name>" ;

	private static String KML_HEADER2 = "</name>"
		      + "<open>1</open>"
		      + "<Style id=\"green\">"
		      + "<LineStyle>"
		      + "<color>7d00ff00</color>"
		      + "<width>4</width>"
		      + "</LineStyle>"
		      + "<PolyStyle>"
		      + "<color>7d00ff00</color>"
		      + "</PolyStyle>"
		      + "</Style>"
		      + "<Style id=\"yellow\">"
		      + "<LineStyle>"
		      + "<color>7d00ffff</color>"
		      + "<width>4</width>"
		      + "</LineStyle>"
		      + "<PolyStyle>"
		      + "<color>7d00ffff</color>"
		      + "</PolyStyle>"
		      + "</Style>"
		      + "<Style id=\"red\">"
		      + "<LineStyle>"
		      + "<color>7d0000ff</color>"
		      + "<width>4</width>"
		      + "</LineStyle>"
		      + "<PolyStyle>"
		      + "<color>7d0000ff</color>"
		      + "</PolyStyle>"
		      + "</Style>"
		      + "<Style id=\"blue\">"
		      + "<LineStyle>"
		      + "<color>7dff0000</color>"
		      + "<width>4</width>"
		      + "</LineStyle>"
		      + "<PolyStyle>"
		      + "<color>7dff0000</color>"
		      + "</PolyStyle>"
		      + "</Style>";
	
	private static String KML_FOLDER1 = "<Placemark><name>";
	
	private static String KML_FOLDER2 = "</name><visibility>1</visibility><styleUrl>#";
	
	private static String KML_FOLDER3 = "</styleUrl><LineString><extrude>1</extrude><tessellate>1</tessellate>"
	          + "<altitudeMode>relativeToGround</altitudeMode><coordinates>";
	
	private static String KML_FOLDER4 = "</coordinates></LineString></Placemark>";

	private String directoryName;
	private String modemName;
	private File csvDirectory;
	private String[] csvFileNames;
	private String[] kmlFolderName = new String[NUMBER_DATA_COLS];
	private StringBuffer[] kmlFolder = new StringBuffer[NUMBER_DATA_COLS];
	private double[] modemDataAverage = new double[NUMBER_DATA_COLS];
	private int dataPointCount = 0;
	private SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
	private NumberFormat gpsFormat = new DecimalFormat("#0.00000");

	public KMLGenerator(String folderName, String modemName) {
		this.directoryName = folderName.trim();
		this.modemName = modemName.trim();
	}
	
	public String validateFolder() {
		String status = null;
		try {
			if (directoryName == null || directoryName.length() == 0) {
				return "Enter location of Log files!";
			}
			csvDirectory = new File(directoryName);
			if (!csvDirectory.exists()) {
				return "Nonexistent folder! Please reenter.";
			}
			csvFileNames = csvDirectory.list(new ExtensionFilter(".log"));
			if (csvFileNames == null || csvFileNames.length == 0) {
				return "No Log files found in directory: " + csvDirectory ;
			}

		} catch (Exception ex) {
			return ex.getMessage();
		}
		return status;
	}
	public int getFileCount() {
		return csvFileNames.length;
	}
	
	public String processCSVFiles() {
		String batchStatus = "";
		String status = "";
		for (int ii = 0; ii < csvFileNames.length; ii++) {
			status = processCSV(csvFileNames[ii]);
			if (status.length() == 0) {
				status = generateKML(modemName + csvFileNames[ii].substring(0, csvFileNames[ii].lastIndexOf("log")) + "kml");
			}
			batchStatus += status;
		}
		return batchStatus;
	}
	
	private String processCSV(String fileName) {
		String status = "";
		File csvFile;
		BufferedReader input = null;
		int lineNumber = 0;
		try {
			csvFile = new File(directoryName + "\\" + fileName);
			if (!csvFile.exists()) {
				return fileName + ": was deleted?\n" ;
			}
			if (!csvFile.canRead()) {
				return fileName + ": can't read.\n" ;
			}
			input = new BufferedReader(new FileReader(csvFile));
			String line = null;
			/*
			 * Get header line
			 */
			if ((line = input.readLine()) == null) {
				input.close();
				return fileName + ": is empty.\n" ;
			}
			++lineNumber;
			/*
			 * Use standard header
			 */
			line = "Date-Time,DOP,Latitude,Longitude,Altitude (ft),Speed (mph),Heading (deg),mod1 SS (dBm),"
					+ "mod1 Down (Mbs),mod1 Up (Mbs),mod1 Ping (Sec)";
			StringTokenizer strToke = new StringTokenizer(line, ",");
			if (!strToke.nextToken().trim().equals("Date-Time")) {
				input.close();
				return fileName + ": has bad header.\n" ;
			}
			/*
			 * Get folder names
			 */
			try {
				for (int ii = 0; ii < NUMBER_NON_CELL_COLS; ii++) {
					strToke.nextToken();
				}
				/*boolean foundVZW = false;*/
				for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
					if (ii == 0) {
						kmlFolderName[ii] = "mod1 PDOP (dop)";
					} else {
						kmlFolderName[ii] = strToke.nextToken().trim();
					}
				}
			} catch (Exception ex) {
				input.close();
				return fileName + ": has incomplete header.\n" ;
			}
			
			/*
			 * Initialize
			 */
			boolean isNoGps = false;
			String dateTime;
			String pdop = "", pdopOld = "";
			String lat = "", lon = "", latOld = "", lonOld = "";
			String altitude = "", altitudeOld = "";
			String[] modemData = new String[NUMBER_DATA_COLS];
			String[] modemDataHeight = new String[NUMBER_DATA_COLS];
			String[] modemDataOld = new String[NUMBER_DATA_COLS];
			String[] modemDataHeightOld = new String[NUMBER_DATA_COLS];
			String[] kmlFolderColor = new String[NUMBER_DATA_COLS];
			Double[] modemDataValueOld = new Double[NUMBER_DATA_COLS];
			
			for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
				kmlFolder[ii] = new StringBuffer("");
				modemDataAverage[ii] = 0.0;
			    dataPointCount = 0;
			}
			long oldTimestamp = 0;
			long newTimestamp = 0;
			boolean isToBeMapped = true;
			boolean isAlreadyStopped = false;
			boolean noSignalStrength = true;
			/*
			 * Read data lines
			 */
			lineloop: while ((line = input.readLine()) != null) {
				++lineNumber;
				line = line.trim();
				try {
					if (!line.startsWith("20") || line.endsWith(",")) {
						/*
						 * Skip bad line
						 */
						continue lineloop;
					}
					strToke = new StringTokenizer(line, ",");
					dateTime = strToke.nextToken();
					if (dateTime.equals("2014/08/18 14:33:43")) {
						System.out.println(dateTime);
					}
					Date date = dateFormat.parse(dateTime);
					newTimestamp = date.getTime();
					pdop = strToke.nextToken();
					if (pdop.startsWith("!!! No GPS")) {
						/*
						 * Skip line
						 */
						continue lineloop;
					}
					if ((newTimestamp - oldTimestamp) < SIXTY_SECONDS ) { 
						isToBeMapped = true;
					} else {
						isToBeMapped = false;
					}
					/*
					 * Get GPS lat, lon
					 */
					lat = strToke.nextToken();
					if (lat.equals("0.00000")) {
						/*
						 * No GPS skip this line
						 */
						continue lineloop;
					}
					lon = strToke.nextToken();
					altitude = strToke.nextToken();
					double speed  = new Double(strToke.nextToken()).doubleValue();
					if (speed < 1.0) {
						if (isAlreadyStopped) {
							/*
							 * Don't plot if not moving
							 */
							continue lineloop;
						} else {
							isAlreadyStopped = true;
						}
					} else {
						isAlreadyStopped = false;
					}
					/*
					 * Don't bother with heading
					 */
					strToke.nextToken();
					/*
					 * Check for no GPS
					 */
					if (altitude.equals(altitudeOld) && pdop.equals(pdopOld) && 
							lat.equals(latOld) && lon.equals(lonOld)) {
						isNoGps = true;
					} else {
						isNoGps = false;
					}
					/*
					 * Get modem data values
					 */
					dataloop: for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
						if (ii == 0) {
							if (isNoGps) {
								pdop = BAD_DOP;
								lat = addNoise(lat);
								lon = addNoise(lon);
							}
							modemData[ii] = pdop;
						} else {
							modemData[ii] = strToke.nextToken();
						}
						double dataValue  = new Double(modemData[ii]).doubleValue();
						double colorDataValue = dataValue;
						if (dataPointCount >= 2) {
							colorDataValue = (colorDataValue + modemDataValueOld[ii])/2.0;
						}
						modemDataAverage[ii] += dataValue;
						switch (ii) {
						case 0:
							modemDataHeight[ii] = getPdopHeight(dataValue);
							kmlFolderColor[ii] = getPdopColor(colorDataValue);
							break;
						case 1:
							if (dataValue > -1.0) {
								/*
								 * No signal strength
								 */
								if (noSignalStrength) {
									dataPointCount = 0;
								}
								continue lineloop;
							} else {
								noSignalStrength = false;
							}
							modemDataHeight[ii] = getSignalStrengthHeight(dataValue);
							kmlFolderColor[ii] = getSignalStrengthColor(colorDataValue);
							break;
						case 2:
							modemDataHeight[ii] = getTransferRateHeight(dataValue);
							kmlFolderColor[ii] = getTransferRateDownColor(colorDataValue);
							break;
						case 3:
							modemDataHeight[ii] = getTransferRateHeight(dataValue);
							kmlFolderColor[ii] = getTransferRateUpColor(colorDataValue);
							break;
						default:
							modemDataHeight[ii] = getResponseRateHeight(dataValue);
							kmlFolderColor[ii] = getResponseRateColor(colorDataValue);
							break;
						}
						modemDataValueOld[ii] = dataValue;
					}
					if (++dataPointCount > 2 && isToBeMapped) {
						/*
						 * Output KML
						 */
						for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
							String units = "";
							switch (ii) {
							case 0:
								units = " pdop ";
								break;
							case 1:
								units = " dBm ";
								break;
							case 2:
							case 3:
								units = " Mbs ";
								break;
							default:
								units = " Sec ";
								break;
							}
							kmlFolder[ii].append(KML_FOLDER1 + dateTime + " "  + modemDataOld[ii] + " to " + modemData[ii] 
									+ units + KML_FOLDER2 + kmlFolderColor[ii]  + KML_FOLDER3 + lonOld + "," + latOld + "," 
									+ modemDataHeightOld[ii] + " " + lon + "," + lat + "," + modemDataHeight[ii] + KML_FOLDER4);
						}

					}
					/*
					 * Save old values
					 */
					oldTimestamp = newTimestamp;
					if (isNoGps == false) {
						latOld = lat;
						lonOld = lon;
						pdopOld = pdop;
						altitudeOld = altitude;
					}
					for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
						modemDataOld[ii] = modemData[ii];
						modemDataHeightOld[ii] = modemDataHeight[ii];
					}

				} catch (Throwable ex) {

				}
			}
			input.close();
		} catch (Exception ex) {
			if (input != null) {
				try {
					input.close();
				} catch (IOException iex) {
				}
			}
			return ex.getMessage();
		}
		return status;
	}

	private String addNoise(String value) {
		double valueDouble  = new Double(value).doubleValue();
		double randomNumber = Math.random();
		value = gpsFormat.format(new Double(valueDouble + ((0.5 - (randomNumber < 0.01 ? 0.01 : randomNumber))/500.0)));
		return value;
	}
	private String getPdopHeight(double value) {
		String heightStr = new Integer((int)Math.floor(10.0 * (10 - (value > 10 ? 10 : value)))).toString();
		return heightStr;
	}
	private String getPdopColor(double value) {
		String heightStr = "red";
		if (value > 5.0 && value < 10.0) {
			heightStr = "yellow";
		} else if (value <= 5.0) {
			heightStr = "green";
		}
		return heightStr;
	}
	private String getSignalStrengthHeight(double value) {
		String heightStr = new Integer((int)Math.floor(3.0 * (113 + (value < -113 ? -113 : value)))).toString();
		return heightStr;
	}
	private String getSignalStrengthColor(double value) {
		String heightStr = "red";
		if (value > -100.0 && value < -90) {
			heightStr = "yellow";
		} else if (value >= -90) {
			heightStr = "green";
		}
		return heightStr;
	}
	private String getTransferRateHeight(double value) {
		String heightStr = new Integer((int)Math.floor(value * 100)).toString();
		return heightStr;
	}
	private String getTransferRateDownColor(double value) {
		String heightStr = "red";
		if (value > 0.5 && value < 1.0) {
			heightStr = "yellow";
		} else if (value >= 1.0) {
			heightStr = "green";
		}
		return heightStr;
	}
	private String getTransferRateUpColor(double value) {
		String heightStr = "red";
		if (value > 0.25 && value < 0.75) {
			heightStr = "yellow";
		} else if (value >= 0.75) {
			heightStr = "green";
		}
		return heightStr;
	}
	private String getResponseRateHeight(double value) {
		String heightStr = new Integer((int)Math.floor(100.0 * (1.5 - (value > 1.5 ? 1.5 : value)))).toString();
		return heightStr;
	}
	private String getResponseRateColor(double value) {
		String heightStr = "red";
		if (value > 0.75 && value < 1.5) {
			heightStr = "yellow";
		} else if (value <= 0.75) {
			heightStr = "green";
		}
		return heightStr;
	}
	private String generateKML(String fileName) {
		try {
			BufferedWriter writer =
					new BufferedWriter(new FileWriter(directoryName + "\\" + fileName, false));
			writer.write(KML_HEADER1);
			writer.write(fileName);
			writer.write(KML_HEADER2);
			/*
			 * Output data in folders
			 */
			for (int ii = 0; ii < NUMBER_DATA_COLS; ii++) {
				String units = "";
				switch (ii) {
				case 0:
					units = " PDOP average";
					break;
				case 1:
					units = " dBm average";
					break;
				case 2:
				case 3:
					units = " Mbs average";
					break;
				default:
					units = " Sec average";
					break;
				}
				String folderInfo = "<Folder><name>" + modemName + kmlFolderName[ii].substring(4, kmlFolderName[ii].lastIndexOf("("))
						+ ": " +  doubleFormat(modemDataAverage[ii]/dataPointCount) + units + "</name><open>0</open>";
				writer.write(folderInfo);
				writer.write(kmlFolder[ii].toString());
				writer.write("</Folder>");
			}
			writer.write("</Document></kml>");
			writer.close();
		}
		catch (Throwable ex) {
			return fileName + ": not generated -- " + ex.getMessage() + "\n";
		}
		return fileName + ": generated \n";
	}
	
	private String doubleFormat(double inValue) {
		StringWriter sw = new StringWriter();
		PrintWriter pw = new PrintWriter(sw);
		pw.printf("%.3f", inValue);
		return sw.toString();
	}

	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
