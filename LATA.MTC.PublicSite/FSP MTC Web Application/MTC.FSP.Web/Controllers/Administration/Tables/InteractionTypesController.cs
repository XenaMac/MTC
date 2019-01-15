using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
   [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class InteractionTypesController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.InteractionTypes.OrderBy(p => p.InteractionType1).ToListAsync());
        }
      
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InteractionTypeID,InteractionType1")] InteractionType interactionType)
        {
            if (ModelState.IsValid)
            {
                interactionType.InteractionTypeID = Guid.NewGuid();
                db.InteractionTypes.Add(interactionType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(interactionType);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InteractionType interactionType = await db.InteractionTypes.FindAsync(id);
            if (interactionType == null)
            {
                return HttpNotFound();
            }
            return View(interactionType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InteractionTypeID,InteractionType1")] InteractionType interactionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interactionType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(interactionType);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InteractionType interactionType = await db.InteractionTypes.FindAsync(id);
            if (interactionType == null)
            {
                return HttpNotFound();
            }
            return View(interactionType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            InteractionType interactionType = await db.InteractionTypes.FindAsync(id);
            db.InteractionTypes.Remove(interactionType);
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
