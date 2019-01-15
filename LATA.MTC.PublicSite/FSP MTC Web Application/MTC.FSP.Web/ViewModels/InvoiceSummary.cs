using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceSummary
    {
        public string Row { get; set; }
        public float Days { get; set; }
        public float Shifts { get; set; }
        public float ContractHours { get; set; }
        public float OnPatrolHours { get; set; }
    }
}