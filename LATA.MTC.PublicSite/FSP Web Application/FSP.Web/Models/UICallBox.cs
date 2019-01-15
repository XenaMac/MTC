using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UICallBox
    {
        public Guid CallBoxId { get; set; }
        public String TelephoneNumber { get; set; }
        public int FreewayId { get; set; }
        public String LocationDescription { get; set; }
        public Double Lat { get; set; }
        public Double Lon { get; set; }
    }
}