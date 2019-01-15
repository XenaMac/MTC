using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class InvestigationViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Guid DriverId { get; set; }
        public String DriverName { get; set; }

        public Guid BeatId { get; set; }
        public String BeatNumber { get; set; }

        public Guid ContractorId { get; set; }
        public String ContractCompanyName { get; set; }

        public int ViolationTypeId { get; set; }
        public String ViolationTypeName { get; set; }
        
        public String Summary { get; set; }

        public int CHPOfficerId { get; set; }
        public String InvestigatingOfficer { get; set; }
    }
}