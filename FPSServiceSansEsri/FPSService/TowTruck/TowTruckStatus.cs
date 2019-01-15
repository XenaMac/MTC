using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Types;
namespace FPSService.TowTruck
{
    public class TowTruckStatus
    {
        public bool SpeedingAlarm { get; set; }
        public string SpeedingValue { get; set; }
        public DateTime SpeedingTime { get; set; }
        public SqlGeography SpeedingLocation { get; set; }
        public bool OutOfBoundsAlarm { get; set; }
        public string OutOfBoundsMessage { get; set; }
        public DateTime OutOfBoundsTime { get; set; }
        public DateTime OutOfBoundsStartTime { get; set; }
        public DateTime OutOfBoundsTimeCleared { get; set; }
        public DateTime OutOfBoundsTimeExcused { get; set; }
        public string VehicleStatus { get; set; }
        public DateTime StatusStarted { get; set; }
        public bool LogOnAlarm { get; set; }
        public DateTime LogOnAlarmTime { get; set; }
        public DateTime LogOnAlarmCleared { get; set; }
        public DateTime LogOnAlarmExcused { get; set; }
        public string LogOnAlarmComments { get; set; }
        public bool RollOutAlarm { get; set; }
        public DateTime RollOutAlarmTime { get; set; }
        public DateTime RollOutAlarmCleared { get; set; }
        public DateTime RollOutAlarmExcused { get; set; }
        public string RollOutAlarmComments { get; set; }
        public bool OnPatrolAlarm { get; set; }
        public DateTime OnPatrolAlarmTime { get; set; }
        public DateTime OnPatrolAlarmCleared { get; set; }
        public DateTime OnPatrolAlarmExcused { get; set; }
        public string OnPatrolAlarmComments { get; set; }
        public bool RollInAlarm { get; set; }
        public DateTime RollInAlarmTime { get; set; }
        public DateTime RollInAlarmCleared { get; set; }
        public DateTime RollInAlarmExcused { get; set; }
        public string RollInAlarmComments { get; set; }
        public bool LogOffAlarm { get; set; }
        public DateTime LogOffAlarmTime { get; set; }
        public DateTime LogOffAlarmCleared { get; set; }
        public DateTime LogOffAlarmExcused { get; set; }
        public string LogOffAlarmComments { get; set; }
        public bool IncidentAlarm { get; set; }
        public DateTime IncidentAlarmTime { get; set; }
        public DateTime IncidentAlarmCleared { get; set; }
        public DateTime IncidentAlarmExcused { get; set; }
        public string IncidentAlarmComments { get; set; }
        public bool GPSIssueAlarm { get; set; } //handles NO GPS
        public DateTime GPSIssueAlarmStart { get; set; } //handles NO GPS
        public DateTime GPSIssueAlarmCleared { get; set; }
        public DateTime GPSIssueAlarmExcused { get; set; }
        public string GPSIssueAlarmComments { get; set; }
        public bool StationaryAlarm { get; set; } //handles no movement, speed = 0
        public DateTime StationaryAlarmStart { get; set; } //handles no movement, speed = 0
        public DateTime StationaryAlarmCleared { get; set; }
        public DateTime StationaryAlarmExcused { get; set; }
        public string StationaryAlarmComments { get; set; }
    }
}