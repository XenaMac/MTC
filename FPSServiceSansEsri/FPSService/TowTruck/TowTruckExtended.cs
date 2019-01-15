using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TowTruckExtended
    {
        public string ContractorName { get; set; }
        public Guid ContractorID { get; set; }
        public string TruckNumber { get; set; }
        public string FleetNumber { get; set; }
        public DateTime ProgramStartDate { get; set; }
        public string VehicleType { get; set; }
        public int VehicleYear { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string LicensePlate { get; set; }
        public DateTime RegistrationExpireDate { get; set; }
        public DateTime InsuranceExpireDate { get; set; }
        public DateTime LastCHPInspection { get; set; }
        public DateTime ProgramEndDate { get; set; }
        public int FAW { get; set; }
        public int RAW { get; set; }
        public int RAWR { get; set; }
        public int GVW { get; set; }
        public int GVWR { get; set; }
        public int Wheelbase { get; set; }
        public int Overhang { get; set; }
        public int MAXTW { get; set; }
        public DateTime MAXTWCALCDATE { get; set; }
        public string FuelType { get; set; }
        public Guid FleetVehicleID { get; set; }
        public bool isBackup { get; set; }
    }
}