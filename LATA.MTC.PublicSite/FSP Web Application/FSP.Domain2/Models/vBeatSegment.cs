using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class vBeatSegment
    {
        public System.Guid BeatSegmentID { get; set; }
        public string BeatSegmentDescription { get; set; }
        public string CHPDescription { get; set; }
        public string PIMSID { get; set; }
        public string BeatSegmentExtent { get; set; }
        public string BeatSegmentNumber { get; set; }
    }
}
