using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.CHP
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class DriverInteractionsController : Controller
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();

        public ActionResult Create()
        {
            ViewBag.ContractorID = new SelectList(_db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName");
            ViewBag.DriverID = new SelectList(_db.Drivers.Select(p => new {p.DriverID, DriverName = p.LastName + ", " + p.FirstName}).OrderBy(p => p.DriverName), "DriverID", "DriverName");
            ViewBag.InteractionTypeID = new SelectList(_db.InteractionTypes.OrderBy(p => p.InteractionType1), "InteractionTypeID", "InteractionType1");
            ViewBag.VehicleNumber = new SelectList(_db.FleetVehicles.OrderBy(p => p.VehicleNumber), "VehicleNumber", "VehicleNumber");
            ViewBag.BeatNumber = new SelectList(_db.BeatDatas.ToList().Select(b => new
            {
                BeatNumber = b.BeatID
            }).ToList().OrderBy(p => p.BeatNumber), "BeatNumber", "BeatNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InteractionID,ContractorID,DriverID,InteractionTypeID,InteractionArea,InteractionDescription,InspectionPassFail,AccidentPreventable,FollowupRequired,FollowupDescription,FollowupDate,FollowupCompletionDate,FollowupComments,CloseDate,BadgeID,InteractionDate,VehicleNumber,BeatNumber")] DriverInteraction driverInteraction)
        {
            if (ModelState.IsValid)
            {
                driverInteraction.InteractionID = Guid.NewGuid();

                _db.DriverInteractions.Add(driverInteraction);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorID = new SelectList(_db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName");
            ViewBag.DriverID = new SelectList(_db.Drivers.Select(p => new {p.DriverID, DriverName = p.LastName + ", " + p.FirstName}).OrderBy(p => p.DriverName), "DriverID", "DriverName");
            ViewBag.InteractionTypeID = new SelectList(_db.InteractionTypes.OrderBy(p => p.InteractionType1), "InteractionTypeID", "InteractionType1");
            ViewBag.VehicleNumber = new SelectList(_db.FleetVehicles.OrderBy(p => p.VehicleNumber), "VehicleNumber", "VehicleNumber");
            ViewBag.BeatNumber = new SelectList(_db.BeatDatas.ToList().Select(b => new
            {
                BeatNumber = b.BeatID
            }).ToList().OrderBy(p => p.BeatNumber), "BeatNumber", "BeatNumber");
            return View(driverInteraction);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var driverInteraction = await _db.DriverInteractions.FindAsync(id);
            if (driverInteraction == null)
                return HttpNotFound();
            return View(driverInteraction);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var driverInteraction = await _db.DriverInteractions.FindAsync(id);
            if (driverInteraction != null) _db.DriverInteractions.Remove(driverInteraction);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var driverInteraction = await _db.DriverInteractions.FindAsync(id);
            if (driverInteraction == null)
                return HttpNotFound();
            ViewBag.ContractorID = new SelectList(_db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName", driverInteraction.ContractorID);
            ViewBag.DriverID = new SelectList(_db.Drivers.Select(p => new {p.DriverID, DriverName = p.LastName + ", " + p.FirstName}).OrderBy(p => p.DriverName), "DriverID", "DriverName",
                driverInteraction.DriverID);
            ViewBag.InteractionTypeID = new SelectList(_db.InteractionTypes, "InteractionTypeID", "InteractionType1", driverInteraction.InteractionTypeID);
            ViewBag.VehicleNumber = new SelectList(_db.FleetVehicles.OrderBy(p => p.VehicleNumber), "VehicleNumber", "VehicleNumber", driverInteraction.VehicleNumber);
            ViewBag.BeatNumber = new SelectList(_db.BeatDatas.ToList().Select(b => new
            {
                BeatNumber = b.BeatID
            }).ToList().OrderBy(p => p.BeatNumber), "BeatNumber", "BeatNumber", driverInteraction.BeatNumber);
            return View(driverInteraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InteractionID,ContractorID,DriverID,InteractionTypeID,InteractionArea,InteractionDescription,InspectionPassFail,AccidentPreventable,FollowupRequired,FollowupDescription,FollowupDate,FollowupCompletionDate,FollowupComments,CloseDate,BadgeID,InteractionDate,VehicleNumber,BeatNumber")] DriverInteraction driverInteraction)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(driverInteraction).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(_db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName", driverInteraction.ContractorID);
            ViewBag.DriverID = new SelectList(_db.Drivers.Select(p => new {p.DriverID, DriverName = p.LastName + ", " + p.FirstName}).OrderBy(p => p.DriverName), "DriverID", "DriverName",
                driverInteraction.DriverID);
            ViewBag.InteractionTypeID = new SelectList(_db.InteractionTypes, "InteractionTypeID", "InteractionType1", driverInteraction.InteractionTypeID);
            ViewBag.VehicleNumber = new SelectList(_db.FleetVehicles.OrderBy(p => p.VehicleNumber), "VehicleNumber", "VehicleNumber", driverInteraction.VehicleNumber);
            ViewBag.BeatNumber = new SelectList(_db.BeatDatas.ToList().Select(b => new
            {
                BeatNumber = b.BeatID
            }).ToList().OrderBy(p => p.BeatNumber), "BeatNumber", "BeatNumber", driverInteraction.BeatNumber);
            return View(driverInteraction);
        }

        #region Json

        public ActionResult GetInteractions()
        {
            var data = (from p in _db.DriverInteractions
                select new
                {
                    p.InteractionID,
                    Contractor = p.Contractor.ContractCompanyName,
                    Driver = p.Driver.LastName + ", " + p.Driver.FirstName,
                    InteractionType = p.InteractionType.InteractionType1,
                    p.InteractionArea,
                    p.InteractionDescription,
                    p.InspectionPassFail,
                    p.AccidentPreventable,
                    p.FollowupRequired,
                    p.FollowupDescription,
                    p.FollowupDate,
                    p.FollowupCompletionDate,
                    p.FollowupComments,
                    p.CloseDate,
                    p.BadgeID,
                    p.InteractionDate,
                    p.VehicleNumber,
                    p.BeatNumber
                }).ToList();


            return Json(data.OrderByDescending(p => p.Driver), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}