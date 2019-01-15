using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class FleetVehicle
    {
        public FleetVehicle()
        {
            this.CHPInspections = new List<CHPInspection>();
            this.FleetVehiclesBeats = new List<FleetVehiclesBeat>();
        }

        public System.Guid FleetVehicleID { get; set; }
        public System.Guid ContractorID { get; set; }
        public System.DateTime ProgramStartDate { get; set; }
        public string FleetNumber { get; set; }
        public string VehicleType { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public System.DateTime RegistrationExpireDate { get; set; }
        public System.DateTime InsuranceExpireDate { get; set; }
        public System.DateTime LastCHPInspection { get; set; }
        public string Comments { get; set; }
        public System.DateTime ProgramEndDate { get; set; }
        public int FAW { get; set; }
        public int RAW { get; set; }
        public int RAWR { get; set; }
        public int GVW { get; set; }
        public int GVWR { get; set; }
        public int Wheelbase { get; set; }
        public int Overhang { get; set; }
        public int MAXTW { get; set; }
        public System.DateTime MAXTWCALCDATE { get; set; }
        public string FuelType { get; set; }
        public string VehicleNumber { get; set; }
        public string IPAddress { get; set; }
        public string AgreementNumber { get; set; }
        public virtual ICollection<CHPInspection> CHPInspections { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<FleetVehiclesBeat> FleetVehiclesBeats { get; set; }
    }
}
