using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Alarm
    {
        public System.Guid AlarmID { get; set; }
        public string AlarmType { get; set; }
        public System.DateTime AlarmTime { get; set; }
        public System.Guid DriverID { get; set; }
        public System.Guid VehicleID { get; set; }
    }
}
