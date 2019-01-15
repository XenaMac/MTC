using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{

    //anybody can see the home page
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

    }
}
