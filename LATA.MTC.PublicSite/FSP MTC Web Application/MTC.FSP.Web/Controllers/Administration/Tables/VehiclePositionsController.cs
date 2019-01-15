using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class VehiclePositionsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.VehiclePositions.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Code")] VehiclePosition vehiclePosition)
        {
            if (ModelState.IsValid)
            {
                vehiclePosition.CreatedBy = HttpContext.User.Identity.Name;
                vehiclePosition.CreatedOn = DateTime.Now;
                vehiclePosition.ModifiedBy = HttpContext.User.Identity.Name;
                vehiclePosition.ModifiedOn = DateTime.Now;

                db.VehiclePositions.Add(vehiclePosition);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vehiclePosition);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclePosition vehiclePosition = await db.VehiclePositions.FindAsync(id);
            if (vehiclePosition == null)
            {
                return HttpNotFound();
            }
            return View(vehiclePosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Code,CreatedOn,CreatedBy")] VehiclePosition vehiclePosition)
        {
            if (ModelState.IsValid)
            {
                vehiclePosition.ModifiedBy = HttpContext.User.Identity.Name;
                vehiclePosition.ModifiedOn = DateTime.Now;

                db.Entry(vehiclePosition).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehiclePosition);
        }

        // GET: VehiclePositions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiclePosition vehiclePosition = await db.VehiclePositions.FindAsync(id);
            if (vehiclePosition == null)
            {
                return HttpNotFound();
            }
            return View(vehiclePosition);
        }

        // POST: VehiclePositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VehiclePosition vehiclePosition = await db.VehiclePositions.FindAsync(id);
            db.VehiclePositions.Remove(vehiclePosition);
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
