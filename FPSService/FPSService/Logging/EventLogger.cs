using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace FPSService.Logging
{
    public class EventLogger
    {
        private void CheckLogStatus()
        {
            try
            {
                if (!EventLog.SourceExists("MTCService"))
                {
                    EventLog.CreateEventSource("MTCService", "MTCServiceLog");
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void LogEvent(string EventData, bool error)
        {
            try
            {
                CheckLogStatus();
                EventLog eventLog1 = new EventLog();
                eventLog1.Source = "MTCService";
                eventLog1.Log = "MTCServiceLog";
                if (error == true)
                {
                    eventLog1.WriteEntry(EventData, EventLogEntryType.Error);
                }
                else
                {
                    eventLog1.WriteEntry(EventData);
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }
    }
}