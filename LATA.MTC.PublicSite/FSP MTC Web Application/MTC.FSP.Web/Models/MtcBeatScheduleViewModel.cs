using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.Models
{
    public class MtcBeatScheduleViewModel
    {
        public Guid BeatBeatScheduleID { get; set; }
        public String Beat { get; set; }
        public Guid BeatID { get; set; }
        public List<BeatScheduleViewModel> Schedule { get; set; }
        public TimeSpan OnPatrol { get; set; }
        public List<String> ScheduleList { get; set; }
    }

    public class BeatScheduleViewModel
    {
        public Guid BeatScheduleId { get; set; }
        public String ScheduleName { get; set; }

        public int NumberOfTrucks { get; set; }

        public String StartTime { get; set; }
        public String EndTime { get; set; }

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public Guid BeatId { get; set; }
    }
}