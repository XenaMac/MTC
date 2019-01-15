using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class BeatCallSignsViewModel
    {
        public Guid BeatId { get; set; }
        public String BeatNumber { get; set; }
        public String OnCallAreas { get; set; }
        public String Freq { get; set; }
        public String  CHPArea { get; set; }

        public IEnumerable<CallSignViewModel> CallSigns { get; set; }
    }

    public class CallSignViewModel
    {
        public String CallSign { get; set; }
    }
}