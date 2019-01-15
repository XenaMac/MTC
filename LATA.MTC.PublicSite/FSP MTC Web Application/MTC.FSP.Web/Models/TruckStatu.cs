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
    
    public partial class TruckStatu
    {
        public System.Guid StatusID { get; set; }
        public string DriverName { get; set; }
        public string ContractorCompany { get; set; }
        public string Beat { get; set; }
        public string StatusName { get; set; }
        public System.DateTime StatusStart { get; set; }
        public Nullable<System.DateTime> StatusEnd { get; set; }
        public Nullable<int> StatusMins { get; set; }
        public string TruckNumber { get; set; }
        public Nullable<double> lat { get; set; }
        public Nullable<double> lon { get; set; }
        public Nullable<System.Guid> runID { get; set; }
        public string location { get; set; }
        public Nullable<double> Speed { get; set; }
        public string Heading { get; set; }
        public Nullable<System.Guid> ScheduleID { get; set; }
    }
}
