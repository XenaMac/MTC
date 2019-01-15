using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FSP.Common
{
    public class FSPLogging
    {
        public static void WriteToLogFile(String _value)
        {
            String sLogFilePath = String.Empty;

            try
            {
                //check in web.config file is loggin is enabled

                sLogFilePath = System.Web.HttpContext.Current.Server.MapPath("Log.txt");
                StreamWriter sw = null;
                if (!File.Exists(sLogFilePath))
                    sw = File.CreateText(sLogFilePath);
                else
                    sw = File.AppendText(sLogFilePath);

                sw.WriteLine(DateTime.Now + " " + _value);
                sw.Close();
            }
            catch { }
        }
    }
}
