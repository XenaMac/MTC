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
    
    public partial class CHPInspection
    {
        public System.Guid InspectionID { get; set; }
        public System.Guid FleetVehicleID { get; set; }
        public System.DateTime InspectionDate { get; set; }
        public System.Guid InspectionTypeID { get; set; }
        public string InspectionNotes { get; set; }
        public System.Guid ContractorID { get; set; }
        public int CHPOfficerId { get; set; }
    
        public virtual CHPOfficer CHPOfficer { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual FleetVehicle FleetVehicle { get; set; }
        public virtual InspectionType InspectionType { get; set; }
    }
}