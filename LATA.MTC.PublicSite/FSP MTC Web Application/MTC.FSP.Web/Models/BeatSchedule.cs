//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTC.FSP.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BeatSchedule
    {
        public System.Guid BeatScheduleID { get; set; }
        public string ScheduleName { get; set; }
        public Nullable<bool> Sunday { get; set; }
        public Nullable<bool> Monday { get; set; }
        public Nullable<bool> Tuesday { get; set; }
        public Nullable<bool> Wednesday { get; set; }
        public Nullable<bool> Thursday { get; set; }
        public Nullable<bool> Friday { get; set; }
        public Nullable<bool> Saturday { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<int> NumberOfTrucks { get; set; }
    }
}
