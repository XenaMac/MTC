using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class buBeat
    {
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
    }
}
