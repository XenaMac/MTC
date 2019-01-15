using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UIIncident
    {
        //Incident Type
        [Required]
        [DisplayName("Incident Type")]
        public String SelectedIncidentType { get; set; }

        //Freeway
        [Required]
        [DisplayName("Freeway")]
        public int SelectedFreeway { get; set; }


        //Beats (depending on freeway selection)
        [Required]
        [DisplayName("Beat")]
        public String SelectedBeat { get; set; }

        //Segment (depending on freeway selection)
        [Required]
        [DisplayName("Segment")]
        public String SelectedSegment { get; set; }

        [Required]
        [DisplayName("Vehicle Position")]
        public String SelectedVehicilePosition { get; set; }

        //Vehicle Description
        [StringLength(400)]
        [DisplayName("Vehicle Description")]
        public String VehicleDescription { get; set; }

        //Dispatch Comments
        [StringLength(400)]
        [DisplayName("Dispatch Comments")]
        public String DispatchComments { get; set; }

        public Guid IncidentID { get; set; }
        public String IncidentNumber { get; set; }
        public String BeatNumber { get; set; }
        public String TruckNumber { get; set; }
        public String DriverName { get; set; }
        public String Timestamp { get; set; }
        public String State { get; set; }
        public String DispatchNumber { get; set; }
        public String ContractorName { get; set; }

        /// <summary>
        /// An Incident is active if any of the assists is still active. 
        /// </summary>
        public bool IsIncidentComplete { get; set; }
        public bool IsAcked { get; set; }
    }
}