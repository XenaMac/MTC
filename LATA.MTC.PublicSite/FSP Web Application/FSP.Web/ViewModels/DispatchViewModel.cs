using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FSP.Web.ViewModels
{
    public class DispatchViewModel
    {
        public List<Dispatch> Dispatchs { get; set; }
    }

    public class Dispatch
    {
        [DisplayName("Beat #")]
        public String BeatNumber { get; set; }

        [DisplayName("Truck #")]
        public String TruckNumber { get; set; }

        [DisplayName("Contractor Name")]
        public String ContractorName { get; set; }

        [DisplayName("Driver Name")]
        public String DriverName { get; set; }

        [DisplayName("Service Date")]
        public String ServiceDate { get; set; }

        [DisplayName("Beat Start Time")]
        public String BeatStartTime { get; set; }

        [DisplayName("Status")]
        public String Status { get; set; }

        [DisplayName("Last Update Time")]
        public String LastUpdateTime { get; set; }

        [DisplayName("Alarms")]
        public String Alarms { get; set; }

        [DisplayName("Freeway")]
        public String Freeway { get; set; }

        [DisplayName("Direction")]
        public String Direction { get; set; }

        [DisplayName("Last Location(Segment)")]
        public String LastLocation { get; set; }

        [DisplayName("Assit Description")]
        public String AssistDescription { get; set; }

        [DisplayName("Incident Code")]
        public String IncidentCode { get; set; }

        [DisplayName("Service Code")]
        public String ServiceCode { get; set; }

        [DisplayName("Vehicle Description")]
        public String VehicleDescription { get; set; }

        [DisplayName("Vehicle License Plate Number")]
        public String VehicleLicensePlateNumber { get; set; }

        [DisplayName("Tow Location")]
        public String TowLocation { get; set; }

        //internal
        public Boolean IsVisible { get; set; }

    }
  
}