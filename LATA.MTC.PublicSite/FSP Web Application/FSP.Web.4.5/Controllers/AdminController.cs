using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Metadata.Edm;
using FSP.Domain.Model;
using FSP.Web.Models;
using FSP.Web.Filters;

namespace FSP.Web.Controllers
{
    [CustomAuthorization]
    public class AdminController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //[CustomAuthorization(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorization(Roles = "Admin, Dispatcher")]
        public ActionResult IncidentCreated()
        {
            return View();
        }

        [CustomAuthorization(Roles = "Admin, Dispatcher")]
        public ActionResult CreateIncident()
        {
            UIIncident model = this.CreateUIIncidentModel();
            return View(model);
        }

        [CustomAuthorization(Roles = "Admin, Dispatcher")]
        public ActionResult Dispatch()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorization(Roles = "Admin, Dispatcher")]
        public ActionResult CreateIncident(UIIncident incident)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("IncidentCreated");
            }

            UIIncident model = this.CreateUIIncidentModel();
            return View();
        }

        #region helpers

        private UIIncident CreateUIIncidentModel()
        {
            
            //MVC likes ViewBags better for reposts. I tried to have the list part of the model but no good.
            ViewBag.IncidentTypes = db.IncidentTypes.OrderBy(p => p.IncidentType1).ToList();
            ViewBag.Freeways = db.Freeways.OrderBy(p => p.FreewayName).ToList();
            ViewBag.VehiclePositions = db.VehiclePositions.OrderBy(p => p.VehiclePosition1).ToList();

            UIIncident model = new UIIncident();

            return model;
        }
        public ActionResult GetBeatsByFreewayId(int freewayId)
        {
            var beats = from q in db.vBeats
                        where q.FreewayID == freewayId
                        orderby q.BeatNumber
                        select new
                        {
                            BeatID = q.BeatID,
                            BeatNumber = q.BeatNumber
                        };

            return Json(beats, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetSegmentsByBeatId(Guid beatId)
        {
            var beats = from q in db.BeatBeatSegments
                        join s in db.vBeatSegments on q.BeatSegmentID equals s.BeatSegmentID
                        orderby s.BeatSegmentNumber
                        where q.BeatID == beatId
                        select new
                        {
                            BeatSegmentID = s.BeatSegmentID,
                            BeatSegmentNumber = s.BeatSegmentNumber
                        };

            return Json(beats, JsonRequestBehavior.AllowGet);

        }



        #endregion

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
