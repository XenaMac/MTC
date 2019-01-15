using System;

namespace MTC.FSP.Web.ViewModels
{
    public class AlarmReportViewModel
    {
        public Guid AlertId { get; set; }
        public DateTime Date { get; set; }
        public String BeatNumber { get; set; }
        public String CallSign { get; set; }
        public String DriverId { get; set; }
        public String DriverName { get; set; }
        public String TruckNumber { get; set; }
        public ScheduleViewModel RegularShift { get; set; }
        public ScheduleViewModel HolidayShift { get; set; }
        public ScheduleViewModel CustomShift { get; set; }
        public DateTime AlarmTime { get; set; }
        public int? AlarmDuration { get; set; }
        public String AlarmLocation { get; set; }
        public String LatLon { get; set; }
        public String Heading { get; set; }
        public Double? Speed { get; set; }

        public Guid? ScheduleId { get; set; }

        public DateTime? x1097 { get; set; }

        public DateTime? x1098 { get; set; }

        public String x1097Location { get; set; }
    }

    public class ScheduleViewModel
    {
        public String ScheduleName { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
    }
}
