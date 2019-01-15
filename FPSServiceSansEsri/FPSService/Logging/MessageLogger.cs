using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;

namespace FPSService.Logging
{
    public class MessageLogger
    {
        private string LogPath = ""; //Location of log directory, loaded from web.config
        private string LogOnOff = ""; //enable/disable logging, loaded from web.config
        private string strLogFile;

        public MessageLogger(string name)
        {

            LogPath = ConfigurationManager.AppSettings["LogDir"].ToString();
            LogOnOff = ConfigurationManager.AppSettings["LogEnable"].ToString();

            if (name.Trim().Length < 1) name = "NoIPAddr";
            else
            {
                if (name.Contains(':'))
                {
                    name = name.Substring(0, name.IndexOf(':'));
                }
                name = name.Trim();
            }
            try
            {

                if (new FileInfo(name).Directory.Exists)
                {
                    strLogFile = name;
                    strLogFile = System.IO.Path.Combine(LogPath, name);
                }
                else
                {
                    strLogFile = System.IO.Path.Combine(LogPath, name);
                }
            }
            catch
            {
                strLogFile = System.IO.Path.Combine(LogPath, name);
            }
        }
        public void writeToLogFileToTruck(string logMessage)
        {
            if (LogOnOff.ToUpper() != "OFF")
            {
                writeToLogFile("To Truck: " + logMessage);
            }
        }
        public void writeToLogFileFromTruck(string logMessage)
        {
            if (LogOnOff.ToUpper() != "OFF")
            {
                writeToLogFile("From Truck: " + logMessage);
            }
        }

        public void writeToLogFile(string logMessage)
        {
            try
            {
                string strLogMessage = string.Empty;
                StreamWriter swLog;

                strLogMessage = string.Format("{0}: {1}", DateTime.Now, logMessage);

                if (!File.Exists(strLogFile))
                {
                    File.CreateText(strLogFile);
                    swLog = new StreamWriter(strLogFile);
                }
                else
                {
                    swLog = File.AppendText(strLogFile);
                }

                swLog.WriteLine(strLogMessage);
                swLog.WriteLine();

                swLog.Close();
            }
            catch (Exception x)
            {
                Console.WriteLine("Failed to log message: " + x.Message);
            }
        }

    }
}