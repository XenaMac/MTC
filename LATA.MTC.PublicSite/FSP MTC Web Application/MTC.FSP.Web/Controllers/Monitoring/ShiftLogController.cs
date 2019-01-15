using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers.Monitoring
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor,InVehicleContractor,DataConsultant")]
    public class ShiftLogController : Controller
    {
        public ActionResult GetAll(string truckNumber)
        {
            using (var service = new TowTruckServiceClient())
            {
                return Json(service.getTruckRunStatus(truckNumber), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ShiftLog
        public ActionResult Index(string truckNumber)
        {
            if (string.IsNullOrEmpty(truckNumber))
                truckNumber = "123";

            ViewBag.TruckNumber = truckNumber;
            return View();
        }
    }
}