using MTC.FSP.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MTC.FSP.Web.Controllers.FAQ
{
    public class TrainingMediaController : Controller
    {
        // GET: TrainingMedia
        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public ActionResult Index()
        {
            return View();
        }
    }
}