using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class Alarm
    {
        public AlarmTimer alarmTimer = new AlarmTimer();
        public string alarmName;
        public bool hasAlarm;
        private Guid alarmID;

        public Alarm(string _AlarmName)
        {
            alarmName = _AlarmName;
        }

        public void startTimer(string truckNumber, string driverName, string contractorCompany, string Beat, double lat, double lon, Guid runID)
        {
            Guid g = alarmTimer.StartTimer(truckNumber, driverName, contractorCompany, Beat, alarmName, lat, lon, runID);
            hasAlarm = true;
            if (g != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                alarmID = g;
            }
        }

        public void stopTimer(string truckNumber, string driverName, string contractorCompany, string Beat, double lat, double lon, Guid runID)
        {
            hasAlarm = false;
            alarmTimer.StopTimer(alarmID, truckNumber, driverName, contractorCompany, Beat, alarmName, lat ,lon, runID);
        }

        public void setAlarmStatus(bool _hasAlarm)
        {
            hasAlarm = _hasAlarm;
        }

        public int getAlarmTime()
        {
            return alarmTimer.statusMinutes;
        }

        public bool isRunning()
        {
            return alarmTimer.isRunning();
        }
    }
}