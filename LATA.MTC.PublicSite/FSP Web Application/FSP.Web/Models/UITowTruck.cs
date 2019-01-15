using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UITowTruck
    {

        public String TruckNumber { get; set; }
        public String VehicleStateIconUrl { get; set; }
        public String VehicleState { get; set; }
        public Double Heading { get; set; }
        public String BeatNumber { get; set; }
        public String BeatSegmentNumber { get; set; }
        public int ContractorId { get; set; }
        public Double Lat { get; set; }
        public Double Lon { get; set; }
        public Double Speed { get; set; }
        public int LastUpdate { get; set; }
        public String LastMessage { get; set; }
        public String DriverName { get; set; }
        public String ContractorName { get; set; }
        public String Location { get; set; }
        //internal prop
        public Boolean Old { get; set; }      

    }
}