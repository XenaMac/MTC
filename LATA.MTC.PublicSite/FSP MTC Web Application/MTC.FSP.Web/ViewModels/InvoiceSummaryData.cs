using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceSummaryData
    {
        //TOTALS FOR WEEKDAY
        public int TotalWeekDaysInMonth { get; set; }
        public int TotalWeekDaysWorkedInMonth { get; set; }
        public decimal TotalWeekdaysWorkedShifts { get; set; }
        public decimal TotalWeekDaysWorkedHours { get; set; }

        //Totals For Saturday
        public int TotalSaturDaysInMonth { get; set; }
        public int TotalSaturDaysWorkedInMonth { get; set; }
        public decimal TotalSaturDaysWorkedShifts { get; set; }
        public decimal TotalSaturDaysWorkedHours { get; set; }

        //Totals For Sunday
        public int TotalSunDaysInMonth { get; set; }
        public int TotalSunDaysWorkedInMonth { get; set; }
        public decimal TotalSunDaysWorkedShifts { get; set; }
        public decimal TotalSunDaysWorkedHours { get; set; }

        //Totals for Holidays
        public int TotalHolidaysInMonth { get; set; }
        public int TotalHolidaysWorkedInMonth { get; set; }
        public decimal TotalHolidaysWorkedShifts { get; set; }
        public decimal TotalHolidaysWorkedHours { get; set; }

        //Totals for Custom Days
        public int TotalCustomDaysInMonth { get; set; }
        public int TotalCustomDaysWorkedInMonth { get; set; }
        public decimal TotalCustomDaysWorkedShifts { get; set; }
        public decimal TotalCustomDaysWorkedHours { get; set; }

        //Week Days in Month
        public List<DateTime> WeekDaysInMonth { get; set; }
        public List<DateTime> WeekDaysWorkedInMonth { get; set; }
        public List<DateTime> SaturDaysInMonth { get; set; }
        public List<DateTime> SaturDaysWorkedInMonth { get; set; }
        public List<DateTime> SunDaysInMonth { get; set; }
        public List<DateTime> SunDaysWorkedInMonth { get; set; }
        public List<DateTime> HolidaysInMonth { get; set; }
        public List<DateTime> HolidaysWorkedInMonth { get; set; }
        public List<DateTime> CustomDaysInMonth { get; set; }
        public List<DateTime> CustomDaysWorkedInMonth { get; set; }

        //Shifts for the Month
        public decimal WeekDayShiftsInMonth { get; set; }
        public decimal WeekDayHoursInMonth { get; set; }
        public decimal SaturdayShiftsInMonth { get; set; }
        public decimal SaturdayHoursInMonth { get; set; }

        //Totals All
        public int TotalDays { get; set; }
        public decimal TotalShifts { get;set; }
        public decimal TotalHoursWorked { get; set; }
    }
}