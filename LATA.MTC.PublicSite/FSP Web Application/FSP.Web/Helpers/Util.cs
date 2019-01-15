using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace FSP.Web.Helpers
{
    public static class Util
    {
        #region Status Codes
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        public static void WriteToLog(String message)
        {
            if (ConfigurationManager.AppSettings["IsLoggingEnabled"] == "true")
            {
                //if logging is enabled
               
                try
                {
                    String sLogFilePath = ConfigurationManager.AppSettings["LogFileLocation"];
                    StreamWriter sw = null;
                    if (!File.Exists(sLogFilePath))
                        sw = File.CreateText(sLogFilePath);
                    else
                        sw = File.AppendText(sLogFilePath);

                    sw.WriteLine(DateTime.Now + " " + message);
                    sw.Close();
                }
                catch { }              
            }
            
            System.Diagnostics.Debug.WriteLine(message);

        }

        public static void WriteToEventLog(String message)
        {
            try
            {
                string sSource = "FSPWeb";
                string sLog = "Application";

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, message, EventLogEntryType.Error);
            }
            catch (Exception ex) 
            {
                WriteToLog(ex.Message);
            }

            System.Diagnostics.Debug.WriteLine(message);

        }
    }
}