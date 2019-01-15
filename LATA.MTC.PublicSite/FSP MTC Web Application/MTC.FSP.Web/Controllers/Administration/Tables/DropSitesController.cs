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
    public class DropSitesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.DropSites.ToListAsync());
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FriendlyId,BeatNumber,Freeway,CrossStreet,DropSiteArea,DropSiteNumber,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] DropSite dropSite)
        {
            if (ModelState.IsValid)
            {
                db.DropSites.Add(dropSite);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dropSite);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DropSite dropSite = await db.DropSites.FindAsync(id);
            if (dropSite == null)
            {
                return HttpNotFound();
            }
            return View(dropSite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FriendlyId,BeatNumber,Freeway,CrossStreet,DropSiteArea,DropSiteNumber,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] DropSite dropSite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dropSite).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dropSite);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DropSite dropSite = await db.DropSites.FindAsync(id);
            if (dropSite == null)
            {
                return HttpNotFound();
            }
            return View(dropSite);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DropSite dropSite = await db.DropSites.FindAsync(id);
            db.DropSites.Remove(dropSite);
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
