using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Calendar
    {
        public System.Guid CalendarID { get; set; }
        public string CalendarName { get; set; }
        public System.DateTime CalendarDate { get; set; }
        public System.Guid BeatScheduleID { get; set; }
    }
}
