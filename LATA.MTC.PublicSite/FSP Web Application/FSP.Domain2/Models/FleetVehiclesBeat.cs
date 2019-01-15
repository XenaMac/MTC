using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class FleetVehiclesBeat
    {
        public System.Guid FleetVehicleBeat { get; set; }
        public System.Guid FleetVehicleID { get; set; }
        public System.Guid BeatID { get; set; }
        public virtual Beat Beat { get; set; }
        public virtual FleetVehicle FleetVehicle { get; set; }
    }
}
