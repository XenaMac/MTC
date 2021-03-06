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
    
    public partial class Contractor
    {
        public Contractor()
        {
            this.ContractorManagers = new HashSet<ContractorManager>();
            this.Drivers = new HashSet<Driver>();
            this.FleetVehicles = new HashSet<FleetVehicle>();
            this.DriverInteractions = new HashSet<DriverInteraction>();
            this.CHPInspections = new HashSet<CHPInspection>();
            this.Contracts = new HashSet<Contract>();
            this.Appeals = new HashSet<Appeal>();
        }
    
        public System.Guid ContractorID { get; set; }
        public string Address { get; set; }
        public string OfficeTelephone { get; set; }
        public string MCPNumber { get; set; }
        public System.DateTime MCPExpiration { get; set; }
        public string Comments { get; set; }
        public string ContractCompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public Nullable<int> ContractorTypeId { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
    
        public virtual ICollection<ContractorManager> ContractorManagers { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<FleetVehicle> FleetVehicles { get; set; }
        public virtual ContractorType ContractorType { get; set; }
        public virtual ICollection<DriverInteraction> DriverInteractions { get; set; }
        public virtual ICollection<CHPInspection> CHPInspections { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Appeal> Appeals { get; set; }
    }
}
