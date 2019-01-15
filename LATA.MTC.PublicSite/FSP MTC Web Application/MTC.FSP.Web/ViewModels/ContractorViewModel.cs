using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class ContractorViewModel
    {
        public Guid ContractorId { get; set; }

        public String ContractorCompanyName { get; set; }

        public String Email { get; set; }

        public String ContactName { get; set; }

        public String Phone { get; set; }
    }
}