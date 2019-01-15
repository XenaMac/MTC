using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceServiceData
    {
        //Custom
        public int CustomDays { get; set; }
        public int CustomShifts { get; set; }
        public decimal CustomContractHours { get; set; }
        public decimal CustomOnPatrolHours { get; set; }

        //Holidays
        public int HolDays { get; set; }
        public int HolShifts { get; set; }
        public decimal HolContractHours { get; set; }
        public decimal HolOnPatrolHours { get; set; }

        //Sundays
        public int SunDays { get; set; }
        public int SunShifts { get; set; }
        public decimal SunContractHours { get; set; }
        public decimal SunOnPatrolHours { get; set; }

        //Saturdays
        public int SatDays { get; set; }
        public int SatShifts { get; set; }
        public decimal SatContractHours { get; set; }
        public decimal SatOnPatrolHours { get; set; }

        //Saturdays
        public int WeekDays { get; set; }
        public int WeekShifts { get; set; }
        public decimal WeekContractHours { get; set; }
        public decimal WeekOnPatrolHours { get; set; }

        //Totals
        public int TotalDays { get; set; }
        public int TotalShifts { get; set; }
        public decimal TotalContractHours { get; set; }
        public decimal TotalOnPatrolHours { get; set; }
    }
}