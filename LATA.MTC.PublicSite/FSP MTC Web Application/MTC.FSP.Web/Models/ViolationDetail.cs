using System;

namespace MTC.FSP.Web.Models
{
    public class ViolationDetail
    {
        public Guid Id { get; set; }
        public Guid DriverId { get; set; }
        public Guid VehicleId { get; set; }
        public Guid BeatId { get; set; }
        public string Driver { get; set; }
        public string FleetVehicle { get; set; }
        public string Beat { get; set; }
        public string Callsign { get; set; }
        public string OffenseNumb { get; set; }
        public string Status { get; set; }
        public string Severity { get; set; }
        public string LengthOfViolation { get; set; }
        public string Deduction { get; set; }
        public string Notes { get; set; }
    }
}