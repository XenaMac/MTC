using System;
using System.Reflection.Emit;

namespace MTC.FSP.Web.ViewModels
{
    public class DailyScheduleViewModel
    {
        public string ScheduleType { get; set; }
        public string BeatNumber { get; set; }
        public string ContractCompanyName { get; set; }
        public string ContactName { get; set; }
        public string ScheduleName { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        
        public bool? Sunday { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
    }
}