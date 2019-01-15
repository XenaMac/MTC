using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Web.Junk.Models;

namespace FSP.Web.Junk.Controllers
{
    public class UserController : Controller
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(dc.Users);
        }

        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(dc.Roles, "RoleID", "RoleName");
            return View();
        }
    }
}
