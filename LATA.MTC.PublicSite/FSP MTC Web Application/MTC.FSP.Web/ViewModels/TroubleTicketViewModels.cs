using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class TroubleTicketProblemsViewModel
    {
        public int TroubleTicketProblemId { get; set; }

        public String Problem { get; set; }

        public int TroubleTicketId { get; set; }
    }

    public class TroubleTickeComponentIssuesViewModel
    {
        public int TroubleTicketComponentIssueId { get; set; }

        public String Issue { get; set; }

        public int TroubleTicketId { get; set; }
    }

    public class TroubleTickeLATATraxIssuesViewModel
    {
        public int TroubleTicketLATATraxIssueId { get; set; }

        public String Issue { get; set; }

        public int TroubleTicketId { get; set; }
    }

    public class TroubleTickeDriversViewModel
    {
        public Guid DriverId { get; set; }

        public String DriverFullName { get; set; }

        public int TroubleTicketId { get; set; }
    }
}