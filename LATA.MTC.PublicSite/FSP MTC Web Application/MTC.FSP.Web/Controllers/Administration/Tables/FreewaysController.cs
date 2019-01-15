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

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    public class FreewaysController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.Freeways.ToListAsync());
        }
     
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FreewayID,FreewayName")] Freeway freeway)
        {
            if (ModelState.IsValid)
            {
                db.Freeways.Add(freeway);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(freeway);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freeway freeway = await db.Freeways.FindAsync(id);
            if (freeway == null)
            {
                return HttpNotFound();
            }
            return View(freeway);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FreewayID,FreewayName")] Freeway freeway)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freeway).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(freeway);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freeway freeway = await db.Freeways.FindAsync(id);
            if (freeway == null)
            {
                return HttpNotFound();
            }
            return View(freeway);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Freeway freeway = await db.Freeways.FindAsync(id);
            db.Freeways.Remove(freeway);
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
