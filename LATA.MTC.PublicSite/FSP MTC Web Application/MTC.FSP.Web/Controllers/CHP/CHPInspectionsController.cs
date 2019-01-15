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
    public class CHPInspectionsController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.CHPOfficerId = new SelectList(db.CHPOfficers.Select(p => new { Id = p.Id, OfficerName = p.OfficerLastName + ", " + p.OfficerFirstName }), "Id", "OfficerName");
            ViewBag.ContractorID = new SelectList(db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName");
            ViewBag.FleetVehicleID = new SelectList(db.FleetVehicles.OrderBy(p => p.FleetNumber), "FleetVehicleID", "FleetNumber");
            ViewBag.InspectionTypeID = new SelectList(db.InspectionTypes.OrderBy(p => p.InspectionType1), "InspectionTypeID", "InspectionType1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InspectionID,FleetVehicleID,CHPOfficerId,InspectionDate,InspectionTypeID,InspectionNotes,ContractorID")] CHPInspection cHPInspection)
        {
            if (ModelState.IsValid)
            {
                cHPInspection.InspectionID = Guid.NewGuid();
                db.CHPInspections.Add(cHPInspection);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CHPOfficerId = new SelectList(db.CHPOfficers.Select(p => new { Id = p.Id, OfficerName = p.OfficerLastName + ", " + p.OfficerFirstName }), "Id", "OfficerName");
            ViewBag.ContractorID = new SelectList(db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName");
            ViewBag.FleetVehicleID = new SelectList(db.FleetVehicles.OrderBy(p => p.FleetNumber), "FleetVehicleID", "FleetNumber");
            ViewBag.InspectionTypeID = new SelectList(db.InspectionTypes.OrderBy(p => p.InspectionType1), "InspectionTypeID", "InspectionType1");
            return View(cHPInspection);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHPInspection cHPInspection = await db.CHPInspections.FindAsync(id);
            if (cHPInspection == null)
            {
                return HttpNotFound();
            }

            ViewBag.CHPOfficerId = new SelectList(db.CHPOfficers.Select(p => new { Id = p.Id, OfficerName = p.OfficerLastName + ", " + p.OfficerFirstName }), "Id", "OfficerName", cHPInspection.CHPOfficerId);
            ViewBag.ContractorID = new SelectList(db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName", cHPInspection.ContractorID);
            ViewBag.FleetVehicleID = new SelectList(db.FleetVehicles.OrderBy(p => p.FleetNumber), "FleetVehicleID", "FleetNumber", cHPInspection.FleetVehicleID);
            ViewBag.InspectionTypeID = new SelectList(db.InspectionTypes.OrderBy(p => p.InspectionType1), "InspectionTypeID", "InspectionType1", cHPInspection.InspectionTypeID);

            return View(cHPInspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InspectionID,FleetVehicleID,CHPOfficerId,InspectionDate,InspectionTypeID,InspectionNotes,ContractorID")] CHPInspection cHPInspection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHPInspection).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CHPOfficerId = new SelectList(db.CHPOfficers.Select(p => new { Id = p.Id, OfficerName = p.OfficerLastName + "," + p.OfficerFirstName }), "Id", "OfficerName", cHPInspection.CHPOfficerId);
            ViewBag.ContractorID = new SelectList(db.Contractors.OrderBy(p => p.ContractCompanyName), "ContractorID", "ContractCompanyName", cHPInspection.ContractorID);
            ViewBag.FleetVehicleID = new SelectList(db.FleetVehicles.OrderBy(p => p.FleetNumber), "FleetVehicleID", "FleetNumber", cHPInspection.FleetVehicleID);
            ViewBag.InspectionTypeID = new SelectList(db.InspectionTypes.OrderBy(p => p.InspectionType1), "InspectionTypeID", "InspectionType1", cHPInspection.InspectionTypeID);

            return View(cHPInspection);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHPInspection cHPInspection = await db.CHPInspections.FindAsync(id);
            if (cHPInspection == null)
            {
                return HttpNotFound();
            }
            return View(cHPInspection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CHPInspection cHPInspection = await db.CHPInspections.FindAsync(id);
            db.CHPInspections.Remove(cHPInspection);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Json

        public async Task<ActionResult> GetInspections()
        {
            
            var data = await (from p in db.CHPInspections.Include(p => p.CHPOfficer).Include(i => i.InspectionType).Include(f => f.FleetVehicle)
                              select new
                              {
                                  InspectionID = p.InspectionID,
                                  InspectionDate = p.InspectionDate,
                                  FleetVehicle = p.FleetVehicle.VehicleNumber,
                                  CHPOfficer = p.CHPOfficer.OfficerLastName + ", " + p.CHPOfficer.OfficerFirstName,
                                  InspectionType = p.InspectionType.InspectionType1,
                                  InspectionNotes = p.InspectionNotes,
                                  Contractor = p.Contractor.ContractCompanyName
                              }).ToListAsync();


            return Json(data.OrderByDescending(p => p.InspectionDate).ThenBy(p => p.FleetVehicle), JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}
