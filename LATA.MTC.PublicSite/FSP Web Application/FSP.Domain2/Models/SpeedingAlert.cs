using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class SpeedingAlert
    {
        public System.Guid AlarmID { get; set; }
        public System.Guid DriverID { get; set; }
        public string VehicleNumber { get; set; }
        public double LoggedSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public System.DateTime SpeedingTime { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
    }
}
