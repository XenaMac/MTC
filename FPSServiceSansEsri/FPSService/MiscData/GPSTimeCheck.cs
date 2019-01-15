using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class GPSTimeCheck
    {
        public string TruckNumber { get; set; }
        public string IPAddress { get; set; }
        public DateTime GPSTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime SpeedingTime { get; set; }
    }
}