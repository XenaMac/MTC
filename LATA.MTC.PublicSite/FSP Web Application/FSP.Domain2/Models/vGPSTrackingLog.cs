using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class vGPSTrackingLog
    {
        public System.Guid GPSID { get; set; }
        public string Direction { get; set; }
        public string VehicleStatus { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public System.DateTime timeStamp { get; set; }
        public System.Guid BeatSegmentID { get; set; }
        public string VehicleID { get; set; }
        public double Speed { get; set; }
        public bool Alarms { get; set; }
        public System.Guid DriverID { get; set; }
        public string VehicleNumber { get; set; }
        public System.Guid BeatID { get; set; }
        public string Position { get; set; }
        public bool SpeedingAlarm { get; set; }
        public Nullable<double> SpeedingValue { get; set; }
        public Nullable<System.DateTime> SpeedingTime { get; set; }
        public string SpeedingLocation { get; set; }
        public bool OutOfBoundsAlarm { get; set; }
        public string OutOfBoundsMessage { get; set; }
    }
}
