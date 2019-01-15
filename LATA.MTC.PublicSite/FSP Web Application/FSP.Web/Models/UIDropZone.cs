using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UIDropZone
    {
        public Guid DropZoneID { get; set; }
        public String Location { get; set; }
        public String Comments { get; set; }
        public String DropZoneNumber { get; set; }
        public String DropZoneDescription { get; set; }
        public List<UIMapPolygonLinePoint> PolygonPoints { get; set; }
    }
}