using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.TowTruck
{
    public class StatusTimer
    {
        public DateTime statusStart { get; set; }
        public DateTime statusEnd { get; set; }
        public int statusMinutes { get; set; }
        public int currentMinutes { get; set; }
        private Timer tmrStatus;
        private bool tmrRunning = false;
        SQL.SQLCode sql;
        public string statusName;
        public string statusValue;

        public StatusTimer(string _statusName)
        {
            statusName = _statusName;
            tmrStatus = new Timer();
            tmrStatus.Interval = 1000;
            tmrStatus.Elapsed += tmrStatus_Elapsed;
            statusStart = Convert.ToDateTime("01/01/2001 00:00:00");
            statusEnd = Convert.ToDateTime("01/01/2001 00:00:00");
        }

        private void tmrStatus_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentMinutes += 1;
        }

        public bool isRunning()
        {
            return tmrRunning;
        }


        public Guid StartTimer(string statusName, string truckNumber, string driverName, string contractorCompany, string Beat, double lat, double lon,
            Guid runID, string location, double speed, int heading, Guid ScheduleID)
        {
            Guid g = new Guid();

            if (tmrRunning == false)
            {
                statusStart = DateTime.Now;
                statusEnd = Convert.ToDateTime("01/01/2001 00:00:00");
                currentMinutes = 0;
                g = Guid.NewGuid();
                tmrStatus.Start();
                sql = new SQL.SQLCode();
                sql.logStatus(g, statusName, truckNumber, driverName, contractorCompany, Beat, statusStart, statusEnd, lat, lon, runID, location, speed, heading, ScheduleID);
                tmrRunning = true;
                
            }

            return g;
        }

        public string GetStartTime()
        {
            if (statusStart != Convert.ToDateTime("01/01/2001 00:00:00"))
            {
                return statusStart.ToString("HH:mm:ss");
            }
            else
            {
                return "Not Started";
            }
        }

        public string GetEndTime()
        {
            if (statusEnd != Convert.ToDateTime("01/01/2001 00:00:00"))
            {
                return statusEnd.ToString("HH:mm:ss");
            }
            else
            {
                return "Not Ended";
            }
        }

        public void StopTimer(Guid alarmID, string statusName, string truckNumber, string driverName, string contractorCompany, string Beat, 
            double lat, double lon, Guid runID, string location, double speed, int heading, Guid ScheduleID)
        {
            if (tmrRunning == true)
            {
                statusEnd = DateTime.Now;
                tmrStatus.Stop();
                TimeSpan ts = statusEnd - statusStart;
                statusMinutes += Convert.ToInt32(ts.TotalMinutes);
                tmrRunning = false;
                sql = new SQL.SQLCode();
                sql.logStatus(alarmID, statusName, truckNumber, driverName, contractorCompany, Beat, statusStart, statusEnd, lat, lon, runID, location, speed, heading, ScheduleID);
            }
        }
    }
}