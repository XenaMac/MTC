using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class ClientTruckStatus //return status of truck to client tablet
    {
        public string TruckNumber { get; set; }
        public string TruckStatus { get; set; }
        public double Speed { get; set; }
    }
}