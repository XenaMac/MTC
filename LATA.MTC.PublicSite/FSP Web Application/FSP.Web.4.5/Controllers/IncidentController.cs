using FSP.Domain.Model;
using FSP.Web.Filters;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{
    [CustomAuthorization]
    public class IncidentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetIncidents()
        {
            List<UIIncident> returnList = new List<UIIncident>();
            String contractorName = String.Empty;
            Boolean addTruck = true;

            using (FSPDataContext dc = new FSPDataContext())
            {
                var user = dc.Users.Where(p => p.Email == User.Identity.Name).FirstOrDefault();

                if (user.Role.RoleName == "Contractor")
                    contractorName = dc.Contractors.Where(p => p.ContractorID == user.ContractorID).FirstOrDefault().ContractCompanyName;
                else
                    contractorName = String.Empty;
            }

            using (TowTruckServiceClient service = new TowTruckServiceClient())
            {
                try
                {
                    var allIncidents = from q in service.getIncidentData()
                                       select new UIIncident
                                       {
                                           IncidentID = q.IncidentID,
                                           IncidentNumber = q.IncidentNumber,
                                           BeatNumber = q.BeatNumber,
                                           TruckNumber = q.TruckNumber,
                                           DriverName = q.DriverName,
                                           DispatchComments = q.DispatchComments,
                                           Timestamp = q.Timestamp.ToString(),
                                           State = q.State,
                                           DispatchNumber = q.IncidentNumber,
                                           ContractorName = q.ContractorName,
                                           IsIncidentComplete = q.IsIncidentComplete,
                                           IsAcked = q.IsAcked
                                       };


                    foreach (var incident in allIncidents)
                    {
                        if (!String.IsNullOrEmpty(contractorName))
                        {
                            if (contractorName == incident.ContractorName)
                                addTruck = true;
                            else
                                addTruck = false;
                        }
                        else
                            addTruck = true;

                        if (addTruck)
                            returnList.Add(incident);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }


            }

            return Json(returnList.OrderBy(p => p.BeatNumber), JsonRequestBehavior.AllowGet);
        }

    }
}
