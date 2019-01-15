using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ContractorManagerViewModel
    {
        public Guid ContractorManagerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PhoneNumber { get; set; }
    }
}