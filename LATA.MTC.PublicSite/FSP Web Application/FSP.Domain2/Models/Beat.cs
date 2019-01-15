using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Beat
    {
        public Beat()
        {
            this.BeatAlarms = new List<BeatAlarm>();
            this.BeatBeatSegments = new List<BeatBeatSegment>();
            this.BeatsFreeways = new List<BeatsFreeway>();
            this.Contracts = new List<Contract>();
            this.DriverDailySchedules = new List<DriverDailySchedule>();
            this.FleetVehiclesBeats = new List<FleetVehiclesBeat>();
            this.ShiftSchedules = new List<ShiftSchedule>();
        }

        public System.Guid BeatID { get; set; }
        public bool Active { get; set; }
        public System.Data.Entity.Spatial.DbGeography BeatExtent { get; set; }
        public int FreewayID { get; set; }
        public string BeatDescription { get; set; }
        public string BeatNumber { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public Nullable<bool> IsTemporary { get; set; }
        public string BeatColor { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public virtual ICollection<BeatAlarm> BeatAlarms { get; set; }
        public virtual ICollection<BeatBeatSegment> BeatBeatSegments { get; set; }
        public virtual ICollection<BeatsFreeway> BeatsFreeways { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<DriverDailySchedule> DriverDailySchedules { get; set; }
        public virtual ICollection<FleetVehiclesBeat> FleetVehiclesBeats { get; set; }
        public virtual ICollection<ShiftSchedule> ShiftSchedules { get; set; }
    }
}
