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
    public class VehicleTypesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.VehicleTypes.ToListAsync());
        }                

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Code")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                vehicleType.CreatedBy = HttpContext.User.Identity.Name;
                vehicleType.CreatedOn = DateTime.Now;
                vehicleType.ModifiedBy = HttpContext.User.Identity.Name;
                vehicleType.ModifiedOn = DateTime.Now;

                db.VehicleTypes.Add(vehicleType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vehicleType);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = await db.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Code,CreatedOn,CreatedBy")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                vehicleType.ModifiedBy = HttpContext.User.Identity.Name;
                vehicleType.ModifiedOn = DateTime.Now;

                db.Entry(vehicleType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehicleType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = await db.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VehicleType vehicleType = await db.VehicleTypes.FindAsync(id);
            db.VehicleTypes.Remove(vehicleType);
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
