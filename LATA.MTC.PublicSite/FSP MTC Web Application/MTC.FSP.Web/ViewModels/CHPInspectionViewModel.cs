using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class CHPInspectionViewModel
    {
        public Guid InspectionId { get; set; }
        public DateTime InspectionDate { get; set; }
        public String InspectionNotes { get; set; }
    }
}