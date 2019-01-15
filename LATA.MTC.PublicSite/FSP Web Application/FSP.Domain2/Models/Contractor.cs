using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Contractor
    {
        public Contractor()
        {
            this.CHPInspections = new List<CHPInspection>();
            this.ContractorManagers = new List<ContractorManager>();
            this.Contracts = new List<Contract>();
            this.DriverInteractions = new List<DriverInteraction>();
            this.Drivers = new List<Driver>();
            this.FleetVehicles = new List<FleetVehicle>();
            this.InsuranceCarriers = new List<InsuranceCarrier>();
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
        public virtual ICollection<CHPInspection> CHPInspections { get; set; }
        public virtual ICollection<ContractorManager> ContractorManagers { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<DriverInteraction> DriverInteractions { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<FleetVehicle> FleetVehicles { get; set; }
        public virtual ICollection<InsuranceCarrier> InsuranceCarriers { get; set; }
    }
}
