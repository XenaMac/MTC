using System.Web.Mvc;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class TablesController : Controller
    {
        // GET: Tables
        public ActionResult Index()
        {
            return View();
        }
    }
}