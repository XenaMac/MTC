using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace MTC.FSP.Web.ViewModels
{
    public class DSR
    {
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }

        public string DriverLastName { get; set; }
        public string DriverFirstName { get; set; }
        public Guid? DriverID { get; set; }
        public string Beat { get; set; }
        public string DropSiteBeat { get; set; }
        public string Callsign { get; set; }
        public int fromTruck { get; set; }
        public double? StartODO { get; set; }
        public double? EndODO { get; set; }
        public DateTime OnPatroll { get; set; }
        public DateTime RollIn { get; set; }
        public string CHPLogNumber { get; set; }
        public string OTAuthorizationNumber { get; set; }
        public string DropSite { get; set; }
        public string IncidentSurveyNumber { get; set; }
        public string ContractCompany { get; set; }

        public List<MTCPreAssist> PreAssists { get; set; }
        public List<MTCAssist> Assists { get; set; }
        public List<MTCActionTaken> ActionsTaken { get; set; }
    }
}