using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TowTruckGPS
    {
        public string TruckID { get; set; }
        public string Direction { get; set; }
        public int Speed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string State { get; set; }
        public bool Alarm { get; set; }
        public string AlarmValue { get; set; }
        public double Heading { get; set; }
        public string BeatDescription { get; set; }
    }
}