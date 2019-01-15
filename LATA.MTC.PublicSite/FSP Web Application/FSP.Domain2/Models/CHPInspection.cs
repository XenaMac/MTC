using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class CHPInspection
    {
        public System.Guid InspectionID { get; set; }
        public System.Guid FleetVehicleID { get; set; }
        public string BadgeID { get; set; }
        public System.DateTime InspectionDate { get; set; }
        public System.Guid InspectionTypeID { get; set; }
        public string InspectionNotes { get; set; }
        public System.Guid ContractorID { get; set; }
        public virtual CHPOfficer CHPOfficer { get; set; }
        public virtual CHPOfficer CHPOfficer1 { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual FleetVehicle FleetVehicle { get; set; }
        public virtual InspectionType InspectionType { get; set; }
    }
}
