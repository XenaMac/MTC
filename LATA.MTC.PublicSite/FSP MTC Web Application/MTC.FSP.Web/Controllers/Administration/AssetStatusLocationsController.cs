using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,DataConsultant")]
    public class AssetStatusLocationsController : MtcBaseController
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            var assetStatusLocations = db.AssetStatusLocations.Include(a => a.AssetStatus);
            return View(await assetStatusLocations.ToListAsync());
        }

        public ActionResult GetAll()
        {

            var vehicles = this.GetVehicleList();

            var assetStatusLocations = db.AssetStatusLocations.ToList().Select(q => new
            {
                Id = q.Id,
                Item = q.Item,
                AssetStatus = q.AssetStatus,
                IPAddress = q.IPAddress,
                LATARMANumber = q.LATARMANumber,
                Location = q.Location,
                OEMRMANumber = q.OEMRMANumber,
                OEMRMANumberIssueDate = q.OEMRMANumberIssueDate,
                OEMSerialNumber = q.OEMSerialNumber,
                RepairCycleTimeInDays = q.RepairCycleTimeInDays,
                Truck = vehicles.FirstOrDefault(p => p.FleetVehicleId == q.VehicleId)
            });

            return Json(assetStatusLocations, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.AssetStatusId = new SelectList(db.AssetStatuses, "Id", "StatusName");
            using (MTCDBEntities dbOld = new MTCDBEntities())
            {
                ViewBag.VehicleId = new SelectList(dbOld.FleetVehicles.OrderBy(p => p.FleetNumber).ToList(), "FleetVehicleID", "FleetNumber");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Item,OEMSerialNumber,Location,VehicleId,IPAddress,AssetStatusId,RepairCycleTimeInDays,LATARMANumber,OEMRMANumber,OEMRMANumberIssueDate")] AssetStatusLocation assetStatusLocation)
        {
            if (ModelState.IsValid)
            {
                assetStatusLocation.CreatedBy = HttpContext.User.Identity.Name;
                assetStatusLocation.ModifiedBy = HttpContext.User.Identity.Name;
                assetStatusLocation.CreatedOn = DateTime.Now;
                assetStatusLocation.ModifiedOn = DateTime.Now;


                db.AssetStatusLocations.Add(assetStatusLocation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AssetStatusId = new SelectList(db.AssetStatuses, "Id", "StatusName", assetStatusLocation.AssetStatusId);
            return View(assetStatusLocation);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetStatusLocation assetStatusLocation = await db.AssetStatusLocations.FindAsync(id);
            if (assetStatusLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetStatusId = new SelectList(db.AssetStatuses, "Id", "StatusName", assetStatusLocation.AssetStatusId);

            using (MTCDBEntities dbOld = new MTCDBEntities())
            {
                ViewBag.VehicleId = new SelectList(dbOld.FleetVehicles.OrderBy(p => p.FleetNumber).ToList(), "FleetVehicleID", "FleetNumber", assetStatusLocation.VehicleId);
            }

            return View(assetStatusLocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Item,OEMSerialNumber,Location,VehicleId,IPAddress,AssetStatusId,RepairCycleTimeInDays,LATARMANumber,OEMRMANumber,OEMRMANumberIssueDate,CreatedOn,CreatedBy")] AssetStatusLocation assetStatusLocation)
        {
            if (ModelState.IsValid)
            {
                assetStatusLocation.ModifiedBy = HttpContext.User.Identity.Name;
                assetStatusLocation.ModifiedOn = DateTime.Now;


                db.Entry(assetStatusLocation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AssetStatusId = new SelectList(db.AssetStatuses, "Id", "StatusName", assetStatusLocation.AssetStatusId);
            return View(assetStatusLocation);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetStatusLocation assetStatusLocation = await db.AssetStatusLocations.FindAsync(id);
            if (assetStatusLocation == null)
            {
                return HttpNotFound();
            }
            return View(assetStatusLocation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssetStatusLocation assetStatusLocation = await db.AssetStatusLocations.FindAsync(id);
            db.AssetStatusLocations.Remove(assetStatusLocation);
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
    }
}
