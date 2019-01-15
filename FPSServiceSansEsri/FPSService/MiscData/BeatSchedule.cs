using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class BeatSchedule
    {
        public Guid BeatID { get; set; }
        //public Guid BeatScheduleID { get; set; }
        public Guid scheduleID { get; set; }
        public string Contractor { get; set; }
        public string BeatNumber { get; set; }
        public string ScheduleName { get; set; }
        public bool Weekday { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int ScheduleType { get; set; }
    }
}