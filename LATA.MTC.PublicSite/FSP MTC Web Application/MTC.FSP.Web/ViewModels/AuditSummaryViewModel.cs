using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class AuditSummaryViewModel
    {
        public String ChangeContent { get; set; }

        public DateTime ChangeDate { get; set; }

        public String ChangedBy { get; set; }
    }
}