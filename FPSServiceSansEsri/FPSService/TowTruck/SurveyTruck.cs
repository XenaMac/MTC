using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class SurveyTruck
    {
        public string TruckNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double SignalStrength { get; set; }
        public double PDOP { get; set; }
        public double Ping { get; set; }
        public double Up { get; set; }
        public double Down { get; set; }
    }
}