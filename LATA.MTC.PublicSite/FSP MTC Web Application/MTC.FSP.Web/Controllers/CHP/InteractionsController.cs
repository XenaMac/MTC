using System.Web.Mvc;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.CHP
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class InteractionsController : Controller
    {
        // GET: Interactions
        public ActionResult Index()
        {
            return View();
        }
    }
}