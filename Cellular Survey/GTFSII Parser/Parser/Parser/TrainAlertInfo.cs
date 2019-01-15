using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    public class TrainAlertInfo
    {
        public String TrainNum { get; set; }
        public String SchedOrigin { get; set; }
        public String SchedOriginTime { get; set; }
        public String SchedDestination { get; set; }
        public String SchedDestTime { get; set; }
    }
}
