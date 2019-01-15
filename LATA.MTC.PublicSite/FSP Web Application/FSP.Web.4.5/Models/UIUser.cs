using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Models
{
    public class UIUser
    {
        public String Email { get; set; }
        public String RoleName { get; set; }
        public Guid? ContractorID { get; set; }
    }
}