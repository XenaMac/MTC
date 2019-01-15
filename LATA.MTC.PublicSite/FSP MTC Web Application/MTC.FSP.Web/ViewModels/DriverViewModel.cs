using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class DriverViewModel
    {
        public Guid DriverId { get; set; }

        public String FSPIDNumber { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String DriverLicenseNumber { get; set; }

        public String DriverFullName { get; set; }
        
    }
}