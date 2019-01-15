using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class TroubleTicketViewModel : TroubleTicket
    {
        public List<int> TroubleTicketProblemIds { get; set; }

        public List<int> TroubleTicketComponentIssueIds { get; set; }

        public List<int> TroubleTicketLATATraxIssueIds { get; set; }

        public List<Guid> TroubleTicketDriverIds { get; set; }
    }
}