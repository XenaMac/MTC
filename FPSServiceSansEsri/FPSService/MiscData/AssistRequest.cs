using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class AssistRequest
    {
        public Guid AssistID { get; set; }
        public string AssistType { get; set; }
        public Guid IncidentID { get; set; }
        public DateTime DispatchTime { get; set; }
        public string ServiceType { get; set; }
        public string DropZone { get; set; }
        public string Make { get; set; }
        public string VehicleType { get; set; }
        public string VehiclePosition { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }
        public int StartOD { get; set; }
        public int EndOD { get; set; }
        public string TowLocation { get; set; }
        public int Tip { get; set; }
        public string TipDetail { get; set; }
        public string CustomerLastName { get; set; }
        public string Comments { get; set; }
        public bool IsMDC { get; set; }
        public DateTime x1097 { get; set; }
        public DateTime x1098 { get; set; }
        public string Contractor { get; set; }
        public string LogNumber { get; set; }
        public Guid BeatSegmentID { get; set; }
    }
}