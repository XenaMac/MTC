using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UIMapPolygonLine
    {
        public String Number { get; set; }
        public String Description { get; set; }
        public List<UIMapPolygonLinePoint> Points { get; set; }
        public String Color { get; set; }
    }
}