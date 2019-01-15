using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class VehicleViewModel
    {
        public Guid FleetVehicleId { get; set; }

        public String VehicleNumber { get; set; }

        public String VehicleMake { get; set; }

        public String VehicleModel { get; set; }
    }
}