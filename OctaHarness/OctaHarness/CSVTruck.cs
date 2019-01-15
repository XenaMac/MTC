using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OctaHarness
{
    public class CSVTruck
    {
        public string TruckNumber { get; set; }
        public int Direction { get; set; }
        public string VehicleStatus { get; set; }
        public int Speed { get; set; }
        public int MaxSpeed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
