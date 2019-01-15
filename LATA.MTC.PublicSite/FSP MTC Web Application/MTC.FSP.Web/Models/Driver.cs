//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTC.FSP.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Driver
    {
        public Driver()
        {
            this.DriverInteractions = new HashSet<DriverInteraction>();
            this.MTCPreAssists = new HashSet<MTCPreAssist>();
            this.Appeals = new HashSet<Appeal>();
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
        public virtual ICollection<DriverInteraction> DriverInteractions { get; set; }
        public virtual ICollection<MTCPreAssist> MTCPreAssists { get; set; }
        public virtual ICollection<Appeal> Appeals { get; set; }
    }
}
