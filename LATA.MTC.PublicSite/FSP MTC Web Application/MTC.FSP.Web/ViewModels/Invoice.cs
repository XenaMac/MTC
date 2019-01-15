using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class Invoice
    {
        public String InvoiceID { get; set; }
        public int Month { get; set; }
        public Guid BeatID { get; set; }
        public Guid ContractorID { get; set; }
        public string BeatNum { get; set; }
        public float FuelRate { get; set; }
        public float BasePay { get; set; }
        public float BaseRate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string  CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public List<InvoiceSummary> Summaries { get; set; }
        public List<InvoiceAdditions> Additions { get; set; }
        public List<InvoiceDeductions> Deductions { get; set; }
        public List<InvoiceAnamolies> Anamolies { get; set; }


        ///NEW //////////////////////////////////////////////////////////////////////////
        //TOTALS FOR WEEKDAY
        public int TotalWeekDaysInMonth { get; set; }

        //Totals For Saturday
        public int TotalSaturDaysInMonth { get; set; }

        //Totals For Sunday
        public int TotalSunDaysInMonth { get; set; }

        //Totals for Holidays
        public int TotalHolidaysInMonth { get; set; }

        //Totals for Custom Days
        public int TotalCustomDaysInMonth { get; set; }

        //Week Days in Month
        public List<DateTime> WeekDaysInMonth { get; set; }
        public List<DateTime> SaturDaysInMonth { get; set; }
        public List<DateTime> SunDaysInMonth { get; set; }
        public List<DateTime> HolidaysInMonth { get; set; }
        public List<DateTime> CustomDaysInMonth { get; set; }
    }
}