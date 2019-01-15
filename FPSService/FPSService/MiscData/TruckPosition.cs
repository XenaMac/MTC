using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class TruckPosition //Used by tablet client to see where the truck currently is in space
    {
        public string TruckNumber { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Speed { get; set; }
        public int Direction { get; set; }
        public string TruckStatus { get; set; }
    }
}