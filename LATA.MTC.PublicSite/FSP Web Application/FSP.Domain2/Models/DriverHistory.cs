using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class DriverHistory
    {
        public System.Guid DriverID { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public System.Guid ContractorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FSPIDNumber { get; set; }
        public System.DateTime ProgramStartDate { get; set; }
        public Nullable<System.DateTime> TrainingCompletionDate { get; set; }
        public System.DateTime DOB { get; set; }
        public string LicenseNumber { get; set; }
        public System.DateTime LicenseExpirationDate { get; set; }
        public System.DateTime DL64ExpirationDate { get; set; }
        public System.DateTime MedicalCardExpirationDate { get; set; }
        public Nullable<System.DateTime> LassPullNoticeDate { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string UDF { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> ContractorEndDate { get; set; }
        public Nullable<System.DateTime> ProgramEndDate { get; set; }
        public System.Guid ScheduleID { get; set; }
        public Nullable<System.DateTime> DriverStartDate { get; set; }
    }
}
