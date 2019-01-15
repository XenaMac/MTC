using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TowTruckDashboardState
    {
        public string TruckNumber { get; set; }
        public string IPAddress { get; set; }
        public int Direction { get; set; }
        public double Speed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string VehicleState { get; set; }
        public bool Alarms { get; set; }
        public bool SpeedingAlarm { get; set; }
        public string SpeedingValue { get; set; }
        public DateTime SpeedingTime { get; set; }
        public bool OutOfBoundsAlarm { get; set; }
        public string OutOfBoundsMessage { get; set; }
        public DateTime OutOfBoundsTime { get; set; }
        public int Heading { get; set; }
        public DateTime LastMessage { get; set; }
        public string ContractorName { get; set; }
        public string BeatNumber { get; set; }
        public int GPSRate { get; set; }
        public string Log { get; set; }
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public string SFTPServerIP { get; set; }
        public string GPSStatus { get; set; }
        public string GPSDOP { get; set; }
    }
}