using System.Web.Mvc;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers
{
    public class HomeController : MtcBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Temp()
        {
            return View();
        }

        public ActionResult GetCurrentUserId()
        {
            return Json(HttpContext.User.Identity.Name, JsonRequestBehavior.AllowGet);
        }
    }
}