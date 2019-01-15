using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ScheduleDateViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Abbreviation { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EndDate { get; set; }
        public List<ScheduleDateScheduleTimesViewModel> Times { get; set; }

    }

    public class ScheduleDateScheduleTimesViewModel
    {
        public String ScheduleName { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
    }
}