using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class ShiftSchedule
    {
        public System.Guid BeatScheduleID { get; set; }
        public System.Guid BeatID { get; set; }
        public bool Active { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public virtual Beat Beat { get; set; }
    }
}
