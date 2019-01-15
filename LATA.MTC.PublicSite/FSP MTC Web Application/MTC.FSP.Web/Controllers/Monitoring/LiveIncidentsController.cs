using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers.Monitoring
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor")]
    public class LiveIncidentsController : MtcBaseController
    {
        [HttpGet]
        public ActionResult GetIncidents()
        {
            using (var service = new TowTruckServiceClient())
            {
                var allIncidents = (from q in service.GetAllIncidents()
                                    where !string.IsNullOrEmpty(q.IncidentType)
                                    select new
                                    {
                                        q.IncidentID,
                                        q.CallSign,
                                        IncidentNumber = q.IncidentID,
                                        BeatNumber = q.Beat,
                                        q.TruckNumber,
                                        DriverName = q.Driver,
                                        DispatchComments = q.DispatchSummaryMessage,
                                        Timestamp = q.Time.ToShortTimeString(),
                                        q.State,
                                        q.DispatchNumber,
                                        q.ContractorName,
                                        q.IsIncidentComplete,
                                        IsAcked = q.isAcked,
                                        q.IncidentType,
                                        q.Location,
                                        q.UserPosted
                                    }).ToList();


                if (!string.IsNullOrEmpty(UsersContractorCompanyName))
                    allIncidents = allIncidents.Where(p => p.ContractorName == UsersContractorCompanyName).ToList();

                return Json(allIncidents.OrderBy(p => p.BeatNumber), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}