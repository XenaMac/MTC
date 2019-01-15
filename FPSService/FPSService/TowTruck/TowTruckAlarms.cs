using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TowTruckAlarms
    {
        public bool hasAlarms = false;
        public List<AlarmTimer> truckAlarms = new List<AlarmTimer>();

        public TowTruckAlarms()
        {
            AlarmTimer LongBreak = new AlarmTimer("LongBreak");
            AlarmTimer LongLunch = new AlarmTimer("LongLunch");
            AlarmTimer Stationary = new AlarmTimer("Stationary");
            AlarmTimer OffBeat = new AlarmTimer("OffBeat");
            AlarmTimer LateOnPatrol = new AlarmTimer("LateOnPatrol");
            AlarmTimer EarlyOutOfService = new AlarmTimer("EarlyOutOfService");
            AlarmTimer Speeding = new AlarmTimer("Speeding");
            AlarmTimer GPSIssue = new AlarmTimer("GPSIssue");
            AlarmTimer LongIncident = new AlarmTimer("LongIncident");
            AlarmTimer OvertimeActivity = new AlarmTimer("OvertimeActivity");
            truckAlarms.Add(LongBreak);
            truckAlarms.Add(LongLunch);
            truckAlarms.Add(Stationary);
            truckAlarms.Add(OffBeat);
            truckAlarms.Add(LateOnPatrol);
            truckAlarms.Add(EarlyOutOfService);
            truckAlarms.Add(Speeding);
            truckAlarms.Add(GPSIssue);
            truckAlarms.Add(LongIncident);
            truckAlarms.Add(OvertimeActivity);
        }

        public void checkAlarms()
        {
            hasAlarms = false;
            foreach (AlarmTimer a in truckAlarms)
            {
                if (a.hasAlarm == true)
                {
                    hasAlarms = true;
                }
            }
        }

        public void startAlarm(string _AlarmName, string truckNumber, string DriverName, string ContractorCompany, string Beat, double lat, double lon, Guid runID, 
            string location, double speed, int heading)
        {
            AlarmTimer a = truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == _AlarmName; });
            if (a != null)
            {
                a.StartAlarm(truckNumber, DriverName, ContractorCompany, Beat, _AlarmName, lat, lon, runID, location, speed, heading);
            }
        }

        public void stopAlarm(string _AlarmName, string truckNumber, string DriverName, string ContractorCompany, string Beat, double lat, double lon, Guid runID,
            string location, double speed, int heading, string CallSign, Guid ScheduleID, int ScheduleType)
        {
            AlarmTimer a = truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == _AlarmName; });
            if (a != null)
            {
                a.StopAlarm(truckNumber, DriverName, ContractorCompany, Beat, _AlarmName, lat, lon, runID, location, speed, heading, CallSign, ScheduleID, ScheduleType);
            }
        }

        public DateTime getAlarmStart(string _AlarmName)
        {
            DateTime alarmDT = DateTime.Parse("01/01/2001 00:00:00");
            AlarmTimer a = truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == _AlarmName; });
            if (a != null)
            {
                string dt = a.GetStartTime();
                DateTime dtParse = new DateTime();
                if (DateTime.TryParse(dt, out dtParse))
                {
                    alarmDT = dtParse;
                }
            }
            return alarmDT;
        }

        public DateTime getAlarmEnd(string _AlarmName)
        {
            DateTime alarmDT = DateTime.Parse("01/01/2001 00:00:00");
            AlarmTimer a = truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == _AlarmName; });
            if (a != null)
            {
                string dt = a.GetEndTime();
                DateTime dtParse = new DateTime();
                if (DateTime.TryParse(dt, out dtParse))
                {
                    alarmDT = dtParse;
                }
            }
            return alarmDT;
        }

        public int getAlarmDuration(string _AlarmName)
        {
            int alarmDuration = 0;
            AlarmTimer a = truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == _AlarmName; });
            if (a != null)
            {
                alarmDuration = a.currentMinutes;
            }
            return alarmDuration;
        }
    }
}