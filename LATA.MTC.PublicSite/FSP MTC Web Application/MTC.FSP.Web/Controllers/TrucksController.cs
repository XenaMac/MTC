using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Hubs;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers
{
    public class TrucksController : MtcBaseController
    {
        public ActionResult GetAll()
        {
            using (var service = new TowTruckServiceClient())
            {
                try
                {
                    var serviceTowTrucks = service.CurrentTrucks().ToList();

                    //check to see if current user is a contractor. Then filter to only "his/her" trucks"
                    if (!string.IsNullOrEmpty(UsersContractorCompanyName))
                        serviceTowTrucks =
                            serviceTowTrucks.Where(p => p.ContractorName == UsersContractorCompanyName).ToList();

                    return Json(serviceTowTrucks.OrderBy(p => p.BeatNumber).ThenBy(p => p.TruckNumber),
                        JsonRequestBehavior.AllowGet);
                }
                catch
                {
                }

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        public void SetSelectedTruck(string truckId)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MtcHub>();
            context.Clients.All.setSelectedTruck(truckId, HttpContext.User.Identity.Name);
        }
    }
}