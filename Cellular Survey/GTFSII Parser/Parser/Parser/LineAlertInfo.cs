using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    public class LineAlertInfo
    {
        public String LineName { get; set; }
        public String LineAbbr { get; set; }
        public List<String> SubLineList { get; set; }
    }
}
