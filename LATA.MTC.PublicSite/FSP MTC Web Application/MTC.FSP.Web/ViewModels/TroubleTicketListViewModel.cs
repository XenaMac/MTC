using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class TroubleTicketListViewModel
    {
        public int Id { get; set; }
        public TroubleTicketStatus TroubleTicketStatus { get; set; }

        public String TroubleTicketStatusName { get; set; }

        public TroubleTicketType TroubleTicketType { get; set; }

        public String TroubleTicketTypeName { get; set; }

        public String ContactName { get; set; }

        public String ContactPhone { get; set; }

        public Guid? AssociatedTowContractorId { get; set; }
        public Guid? AssociatedInVehicleContractorId { get; set; }
        public Guid? AssociatedInVehicleLATATraxContractorId { get; set; }

        public String TowContractCompanyName { get; set; }
        public String TowContactPhone { get; set; }
        public String TowContactName { get; set; }

        public String InVehicleContractCompanyName { get; set; }
        public String InVehicleContactPhone { get; set; }
        public String InVehicleContactName { get; set; }

        public String InVehicleLATATraxContractCompanyName { get; set; }
        public String InVehicleLATATraxContactPhone { get; set; }
        public String InVehicleLATATraxContactName { get; set; }

        public List<TroubleTicketProblemsViewModel> TroubleTicketProblems { get; set; }

        public List<TroubleTickeComponentIssuesViewModel> TroubleTicketComponentIssues { get; set; }

        public List<TroubleTickeLATATraxIssuesViewModel> TroubleTicketLATATraxIssues { get; set; }

        public List<TroubleTickeDriversViewModel> TroubleTicketDrivers { get; set; }

        public Guid VehicleId { get; set; }

        public String TruckNumber { get; set; }

        public DateTime? ProblemStartedOn { get; set; }

        public String ShiftsMissed { get; set; }

        public String ContractorNotes { get; set; }

        public String MTCNotes { get; set; }

        public String InVehicleContractorNotes { get; set; }

        public String LATANotes { get; set; }




        public DateTime? DateTruckOutOfService { get; set; }

        public DateTime? DateTruckBackInService { get; set; }

        public DateTime? FirstShiftTruckMissed { get; set; }

        public DateTime? LastShiftTruckMissed { get; set; }


        public String ReliaGateOEMSerialNumber { get; set; }


        public bool ReplacmentIsFixed { get; set; }

        public DateTime? ReplacmentDate { get; set; }

        public String ReplacementOEMSerialNumber { get; set; }

        public String ReplacementIPAddress { get; set; }

        public String ReplacementWiFiSSID { get; set; }

        public DateTime? TroubleShootingDate { get; set; }

        public DateTime? FixedDate { get; set; }

        public DateTime? RemovedAssetDate { get; set; }

        public DateTime? ShippedAssetDate { get; set; }

        public DateTime? ReceivedAssetDate { get; set; }

        public DateTime? InstalledAssetDate { get; set; }

        public String LATARMANumber { get; set; }

        public String MaintenanceNotes { get; set; }
        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public String CreatedBy { get; set; }

        public String ModifiedBy { get; set; }



    }

}