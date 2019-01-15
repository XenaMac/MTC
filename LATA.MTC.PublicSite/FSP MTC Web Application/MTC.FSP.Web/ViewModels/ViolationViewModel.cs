using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ViolationViewModel
    {
        public int Id { get; set; }

        public int ViolationTypeId { get; set; }

        public String ViolationTypeCode { get; set; }

        public String ViolationTypeName { get; set; }

        public String ViolationSeverity { get; set; }

        public String OffenseNumber { get; set; }

        public Guid ContractorId { get; set; }
        public String ContractCompanyName { get; set; }

        public DateTime DateTimeOfViolation { get; set; }

        public Guid? BeatId { get; set; }
        public String BeatNumber { get; set; }

        public Guid? DriverId { get; set; }
        public String DriverName { get; set; }
        public String DriverFspIdNumber { get; set; }

        public Guid? FleetVehicleId { get; set; }
        public String TruckNumber { get; set; }

        public String CallSign { get; set; }

        public int ViolationStatusTypeId { get; set; }
        public String ViolationStatusTypeName { get; set; }

        public String DeductionAmount { get; set; }

        public String Notes { get; set; }
        public String PenaltyForDriver { get; set; }

        public String AlarmName { get; set; }

        public DateTime? AlertTime { get; set; }

        public String LengthOfViolation { get; set; }

        public DateTime CreatedOn { get; set; }

        public String CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public String ModifiedBy { get; set; }
    }
}