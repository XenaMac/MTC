using FSP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{
    [CustomAuthorization]
    public class SurveyController : Controller
    {
        //
        // GET: /Survey/

        public ActionResult Index()
        {
            return View();
        }

    }
}
