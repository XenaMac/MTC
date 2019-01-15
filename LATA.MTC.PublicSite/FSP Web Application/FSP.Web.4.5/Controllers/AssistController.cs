using FSP.Domain.Model;
using FSP.Web.Filters;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Controllers
{
    [CustomAuthorization]
    public class AssistController : Controller
    {
        //
        // GET: /Assist/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAssists()
        {
            List<UIAssist> returnList = new List<UIAssist>();

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

            using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
            {
                var allAssists = from q in service.GetAllAssists()
                                 select new UIAssist
                                 {
                                     AssistNumber = q.AssistNumber,
                                     BeatNumber = q.BeatNumber,
                                     Color = q.Color,
                                     DispatchNumber = q.DispatchNumber,
                                     Driver = q.DriverName,
                                     DriverComments = q.Comments,
                                     DropZone = q.DropZone,
                                     Make = q.Make,
                                     PlateNumber = q.LicensePlate,
                                     ContractorName = q.ContractorName
                                 };

                foreach (var assist in allAssists)
                {
                    if (!String.IsNullOrEmpty(contractorName))
                    {
                        if (contractorName == assist.ContractorName)
                            addTruck = true;
                        else
                            addTruck = false;
                    }
                    else
                        addTruck = true;

                    if (addTruck)
                        returnList.Add(assist);
                }



            }

            return Json(returnList.OrderBy(p => p.BeatNumber), JsonRequestBehavior.AllowGet);
        }

    }
}
