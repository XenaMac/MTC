using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers
{
    public class AssistEntryController : Controller
    {
        // GET: AssistEntry
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Assists()
        {
            return View();
        }

        public ActionResult GetAllAssists()
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                //var data = from a in db.Assists
                //           join d in db.Drivers on a.DriverID equals d.DriverID
                //           join c in db.Contractors on a.ContractorID equals c.ContractorID
                //           join f in db.FleetVehicles on a.FleetVehicleID equals f.FleetVehicleID
                //           join t in db.TowLocations on a.TowLocationID equals t.TowLocationID
                //           select new
                //           {
                //               AssistId = a.AssistID,
                //               AssistStartTime = a.OnSiteTime,
                //               AssistEndTime = a.x1098,
                //               Lat = a.Lat,
                //               Lon = a.Lon,
                //               DriverId = a.DriverID,
                //               DriverName = d.FirstName + " " + d.LastName,
                //               ContractorId = a.ContractorID,
                //               ContractorName = c.ContractCompanyName,
                //               FSPTruckNumber = f.FleetNumber,
                //               StartOD = a.StartOD,
                //               TowLocation = t.TowLocation1,
                //               Make = a.Make,
                //               LicensePlate = a.LicensePlate,
                //               State = a.State,
                //               Comments = a.Comments
                //           };

                var data = new List<String>();
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}