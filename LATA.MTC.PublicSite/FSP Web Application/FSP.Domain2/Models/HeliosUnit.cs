using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class HeliosUnit
    {
        public System.Guid HeliosUnitID { get; set; }
        public string HeliosID { get; set; }
        public string IPAddress { get; set; }
        public string PhoneNumber { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
    }
}
