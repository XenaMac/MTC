using FSP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{
    [CustomAuthorization(Roles = "Admin, CHP, Contractor")]
    public class CHPInformationController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }
    }
}
