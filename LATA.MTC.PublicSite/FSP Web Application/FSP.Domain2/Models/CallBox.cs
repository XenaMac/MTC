using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class CallBox
    {
        public System.Guid CallBoxID { get; set; }
        public string TelephoneNumber { get; set; }
        public string Location { get; set; }
        public int FreewayID { get; set; }
        public string SiteType { get; set; }
        public string Comments { get; set; }
        public System.Data.Entity.Spatial.DbGeography Position { get; set; }
        public string SignNumber { get; set; }
    }
}
