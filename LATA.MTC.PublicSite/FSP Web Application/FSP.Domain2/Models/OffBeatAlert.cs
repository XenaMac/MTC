using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class OffBeatAlert
    {
        public System.Guid AlertID { get; set; }
        public System.Guid DriverID { get; set; }
        public string VehicleNumber { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public System.DateTime OffBeatTime { get; set; }
    }
}
