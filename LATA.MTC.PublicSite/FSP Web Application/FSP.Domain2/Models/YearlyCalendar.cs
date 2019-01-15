using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class YearlyCalendar
    {
        public System.Guid DateID { get; set; }
        public string dayName { get; set; }
        public System.DateTime Date { get; set; }
    }
}
