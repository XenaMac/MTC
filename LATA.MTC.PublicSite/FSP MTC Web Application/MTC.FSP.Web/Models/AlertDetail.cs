using System;

namespace MTC.FSP.Web.Models
{
    public class AlertDetail
    {
        public Guid AlertId { get; set; }
        public int BeatNumber { get; set; }        
        public string ContractCompanyName { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public DateTime AlarmTime { get; set; }
        public int? AlarmDuration { get; set; }
        public string AlarmType { get; set; }
        public string Comments { get; set; }
        public DateTime? ExcuseTime { get; set; }
        public string ExcusedBy { get; set; }
        public bool IsExcused { get; set; }

        public string CallSign { get; set; }
    }
}