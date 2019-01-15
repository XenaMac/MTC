using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class buBeatSegment
    {
        public System.Guid BeatSegmentID { get; set; }
        public string CHPDescription { get; set; }
        public string PIMSID { get; set; }
        public System.Data.Entity.Spatial.DbGeography BeatSegmentExtent { get; set; }
        public string BeatSegmentNumber { get; set; }
        public string BeatSegmentDescription { get; set; }
        public string CHPDescription2 { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
        public bool Active { get; set; }
    }
}
