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
    public class LocationAbbreviationsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.LocationAbbreviations.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Position,Abbreviation,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] LocationAbbreviation locationAbbreviation)
        {
            if (ModelState.IsValid)
            {
                locationAbbreviation.CreatedBy = HttpContext.User.Identity.Name;
                locationAbbreviation.CreatedOn = DateTime.Now;
                locationAbbreviation.ModifiedBy = HttpContext.User.Identity.Name;
                locationAbbreviation.ModifiedOn = DateTime.Now;
                db.LocationAbbreviations.Add(locationAbbreviation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(locationAbbreviation);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationAbbreviation locationAbbreviation = await db.LocationAbbreviations.FindAsync(id);
            if (locationAbbreviation == null)
            {
                return HttpNotFound();
            }
            return View(locationAbbreviation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Position,Abbreviation,CreatedOn,CreatedBy")] LocationAbbreviation locationAbbreviation)
        {
            if (ModelState.IsValid)
            {
                locationAbbreviation.ModifiedBy = HttpContext.User.Identity.Name;
                locationAbbreviation.ModifiedOn = DateTime.Now;

                db.Entry(locationAbbreviation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(locationAbbreviation);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationAbbreviation locationAbbreviation = await db.LocationAbbreviations.FindAsync(id);
            if (locationAbbreviation == null)
            {
                return HttpNotFound();
            }
            return View(locationAbbreviation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LocationAbbreviation locationAbbreviation = await db.LocationAbbreviations.FindAsync(id);
            db.LocationAbbreviations.Remove(locationAbbreviation);
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
