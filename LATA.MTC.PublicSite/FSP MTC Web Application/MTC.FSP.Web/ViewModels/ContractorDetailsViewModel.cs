using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ContractorDetailsViewModel
    {
        public Guid ContractorId { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Comments { get; set; }
        public String ContactFirstName { get; set; }
        public String ContactLastName { get; set; }
        public String ContractCompanyName { get; set; }
        public String Email { get; set; }
        public String OfficeTelephone { get; set; }
        public String State { get; set; }
        public String Zip { get; set; }
        public String MCPNumber { get; set; }
        public DateTime MCPExpiration { get; set; }

        public int? ContractorTypeId { get; set; }

        public List<VehicleViewModel> FleetVehicles { get; set; }
        public List<DriverViewModel> Drivers { get; set; }
        public List<CHPInspectionViewModel> CHPInspections { get; set; }
        public List<ContractorManagerViewModel> ContractorManagers { get; set; }
    }


}