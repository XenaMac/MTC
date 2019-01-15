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
    public class TransportationsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.Transportation.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Code,Comments,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Transportation transportation)
        {
            if (ModelState.IsValid)
            {
                transportation.CreatedBy = HttpContext.User.Identity.Name;
                transportation.CreatedOn = DateTime.Now;
                transportation.ModifiedBy = HttpContext.User.Identity.Name;
                transportation.ModifiedOn = DateTime.Now;

                db.Transportation.Add(transportation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(transportation);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transportation transportation = await db.Transportation.FindAsync(id);
            if (transportation == null)
            {
                return HttpNotFound();
            }
            return View(transportation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Code,Comments,CreatedOn,CreatedBy")] Transportation transportation)
        {
            if (ModelState.IsValid)
            {
                transportation.ModifiedBy = HttpContext.User.Identity.Name;
                transportation.ModifiedOn = DateTime.Now;
                db.Entry(transportation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(transportation);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transportation transportation = await db.Transportation.FindAsync(id);
            if (transportation == null)
            {
                return HttpNotFound();
            }
            return View(transportation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transportation transportation = await db.Transportation.FindAsync(id);
            db.Transportation.Remove(transportation);
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
