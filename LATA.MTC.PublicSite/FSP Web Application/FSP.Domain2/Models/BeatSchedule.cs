using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class BeatSchedule
    {
        public System.Guid BeatScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public bool Weekday { get; set; }
        public System.TimeSpan Logon { get; set; }
        public System.TimeSpan RollOut { get; set; }
        public System.TimeSpan OnPatrol { get; set; }
        public System.TimeSpan RollIn { get; set; }
        public System.TimeSpan LogOff { get; set; }
    }
}
