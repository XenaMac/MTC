using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Report
    {
        public System.Guid ReportID { get; set; }
        public string ReportName { get; set; }
        public string ConnString { get; set; }
        public string SQL { get; set; }
        public string cmdType { get; set; }
        public string parameters { get; set; }
    }
}
