package com.lata.fsp.mtc;

public class Transfer {

	private String ftpServer = "38.124.164.213";
	private String ftpServerUsername = "LMT";
	private String ftpServerPassword = "L@T@LMT";
	private String localFilePath = "/lata/transfers/ftpFromServer1.bin";
	private String remoteFilePath = "/ftpFromServer1.bin";
	private boolean valid = true;

	public Transfer(String ftpServer, String ftpServerUsername,
			String ftpServerPassword, String localFilePath,
			String remoteFilePath) {
		super();
		this.ftpServer = ftpServer;
		this.ftpServerUsername = ftpServerUsername;
		this.ftpServerPassword = ftpServerPassword;
		this.localFilePath = localFilePath;
		this.remoteFilePath = remoteFilePath;
	}
	
	public boolean isValid() {
		return valid;
	}

	public void setValid(boolean valid) {
		this.valid = valid;
	}

	public String getFtpServer() {
		return ftpServer;
	}

	public void setFtpServer(String ftpServer) {
		this.ftpServer = ftpServer;
	}

	public String getFtpServerUsername() {
		return ftpServerUsername;
	}

	public void setFtpServerUsername(String ftpServerUsername) {
		this.ftpServerUsername = ftpServerUsername;
	}

	public String getFtpServerPassword() {
		return ftpServerPassword;
	}

	public void setFtpServerPassword(String ftpServerPassword) {
		this.ftpServerPassword = ftpServerPassword;
	}

	public String getLocalFilePath() {
		return localFilePath;
	}

	public void setLocalFilePath(String localFilePath) {
		this.localFilePath = localFilePath;
	}

	public String getRemoteFilePath() {
		return remoteFilePath;
	}

	public void setRemoteFilePath(String remoteFilePath) {
		this.remoteFilePath = remoteFilePath;
	}

	public boolean isValidTransfer() {
		if (ftpServer.length() == 0 || ftpServerUsername.length() == 0 || ftpServerPassword.length() == 0
				|| localFilePath.length() == 0 || remoteFilePath.length() == 0) {
			return false;
		}
		valid = true;
		return true;
	}
}

