using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UIIncidentDispatch
    {
        [DisplayName("Direction")]
        public String DirectionName { get; set; }

        [DisplayName("Freeway")]
        public String FreewayId { get; set; }

        [DisplayName("Location")]
        public String LocationName { get; set; }
        
        public List<String> SelectedBeatSegments { get; set; }
        
        public String Description { get; set; }
        
        public List<String> SelectedTrucks { get; set; }
    }
}