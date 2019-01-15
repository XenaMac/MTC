namespace MTC.FSP.Web.Models
{
    public class DriverAlertComment
    {
        public string BeatNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverFullName { get; set; }
        public string Datestamp { get; set; }
        public string Comment { get; set; }
        public string ExceptionType { get; set; }
    }
}