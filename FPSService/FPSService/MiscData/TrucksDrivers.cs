using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class TrucksDrivers
    {
        public Guid TruckID { get; set; }
        public string TruckNumber { get; set; }
        public Guid DriverID { get; set; }
        public string DriverName { get; set; }
        public string ContractorName { get; set; }
    }
}