using System.Web.Mvc;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Data
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor,InVehicleContractor")]
    public class DatabaseController : Controller
    {
        // GET: Database
        public ActionResult Index()
        {
            return View();
        }
    }
}