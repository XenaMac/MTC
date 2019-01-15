using System.Web.Mvc;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Monitoring
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class TruckStatusController : Controller
    {
        // GET: TruckStatus
        public ActionResult Index()
        {
            return View();
        }       
    }
}