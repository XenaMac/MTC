using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public class BeatFreeway
    {
        public Guid BeatID { get; set; }
        public string BeatNumber { get; set; }
        public string BeatDescription { get; set; }
        public bool Active { get; set; }
        public List<string> Freeways { get; set; }
    }
}