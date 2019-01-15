using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class DashboardState
    {
        public string TruckNumber { get; set; }
        public string IPAddress { get; set; }
        public int GPSRate { get; set; }
        public string Log { get; set; }
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public string SFTPServerIP { get; set; }
    }
}