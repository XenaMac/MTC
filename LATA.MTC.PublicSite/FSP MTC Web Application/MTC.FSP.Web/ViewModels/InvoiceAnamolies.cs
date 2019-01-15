using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceAnamolies
    {
        public Guid ID { get; set; }
        public string BeatNumber { get; set; }
        public string date { get; set; }
        public string ScheduledName { get; set; }
        public string Description { get; set; }
    }
}