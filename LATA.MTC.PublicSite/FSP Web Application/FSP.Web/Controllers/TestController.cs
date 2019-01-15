using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Grid()
        {
            return View();
        }

        public ActionResult MapJunk()
        {
            return View();
        }

        public ActionResult AutoCompleting()
        {
            return View();
        }

    }
}
