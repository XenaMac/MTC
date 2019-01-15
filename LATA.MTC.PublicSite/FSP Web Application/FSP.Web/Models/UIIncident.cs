using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;

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

      
    }
}