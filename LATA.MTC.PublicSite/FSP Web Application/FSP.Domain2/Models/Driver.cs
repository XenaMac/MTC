using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Driver
    {
        public Driver()
        {
            this.DriverDailySchedules = new List<DriverDailySchedule>();
            this.DriverInteractions = new List<DriverInteraction>();
        }

        public System.Guid DriverID { get; set; }
        public System.Guid ContractorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FSPIDNumber { get; set; }
        public System.DateTime ProgramStartDate { get; set; }
        public Nullable<System.DateTime> TrainingCompletionDate { get; set; }
        public System.DateTime DOB { get; set; }
        public System.DateTime LicenseExpirationDate { get; set; }
        public System.DateTime DL64ExpirationDate { get; set; }
        public System.DateTime MedicalCardExpirationDate { get; set; }
        public Nullable<System.DateTime> LastPullNoticeDate { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string UDF { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> ContractorEndDate { get; set; }
        public Nullable<System.DateTime> ProgramEndDate { get; set; }
        public Nullable<System.DateTime> ContractorStartDate { get; set; }
        public Nullable<System.Guid> BeatID { get; set; }
        public string Password { get; set; }
        public string DL64Number { get; set; }
        public string DriversLicenseNumber { get; set; }
        public Nullable<System.DateTime> AddedtoC3Database { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<DriverDailySchedule> DriverDailySchedules { get; set; }
        public virtual ICollection<DriverInteraction> DriverInteractions { get; set; }
    }
}
