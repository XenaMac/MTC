using System;

namespace MTC.FSP.Web.Models
{
    public class SearchQuery
    {
        public int PageIndex { get; set; }
        public string SearchValue { get; set; }
        public string SearchColumn { get; set; }
    }

    public abstract class ReportQueryBase
    {
        public string Format { get; set; }
    }

    public class AssistsLoggedQuery : ReportQueryBase
    {
        public DateTime? DatePostedStart { get; set; }
        public DateTime? DatePostedEnd { get; set; }

        public string BeatNumber { get; set; }

        public string ContractCompanyName { get; set; }

        public Guid? DriverId { get; set; }

        public string CallSign { get; set; }
    }

    public class SurveyQuery : ReportQueryBase
    {
        public Guid? SurveyId { get; set; }

        public Guid? QuestionId { get; set; }
    }

    public class AlarmReportQuery : ReportQueryBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AlarmName { get; set; }

        public string BeatNumber { get; set; }

        public string DriverName { get; set; }

        public string CallSign { get; set; }

        public string TruckNumber { get; set; }
    }

    public class ViolationsSearchQuery
    {
        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid? BeatId { get; set; }

        public Guid? ContractorId { get; set; }

        public Guid? DriverId { get; set; }

        public Guid? VehicleId { get; set; }

        public int? ViolationTypeId { get; set; }

        public string CallSign { get; set; }

        public string AlarmName { get; set; }
    }
}