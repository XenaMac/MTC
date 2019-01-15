using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;

namespace OctaHarness
{
    class TowTruck
    {
        public int MessageID { get; set; }
        public string IPAddress { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleStatus { get; set; }
        public string Direction { get; set; }
        public DateTime LastUpdate { get; set; }
        public string BeatSegmetID { get; set; }
        public int Speed { get; set; }
        public bool Alarms { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Guid DriverID { get; set; }
        public Guid BeatID { get; set; }
    }
}
