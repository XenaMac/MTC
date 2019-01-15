using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceSummaryViewModel
    {

        public int BeatNumber { get; set; }

        public String InvoiceNumber { get; set; }

        public int TotalDays { get; set; }

        public int TotalShifts{ get; set; }

        public decimal ContractHours { get; set; }

        public decimal OnPatrolHours { get; set; }

        public decimal FuelRate { get; set; }

        public decimal PayRate { get; set; }

        public decimal BasePay { get; set; }

        public decimal TotalAdditions { get; set; }

        public decimal TotalDeductions { get; set; }

        public decimal TotalInvoice { get; set; }
    }
}