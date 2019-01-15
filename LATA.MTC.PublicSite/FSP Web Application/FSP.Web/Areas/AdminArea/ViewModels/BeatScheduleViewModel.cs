using FSP.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Areas.AdminArea.ViewModels
{
    public class BeatBeatScheduleViewModel
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
        public String ScheduleName { get; set; }
        public String Start { get; set; }
        public String End { get; set; }

    }
}