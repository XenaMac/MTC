using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
   [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class InspectionTypesController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.InspectionTypes.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InspectionType1,InspectionTypeCode")] InspectionType inspectionType)
        {
            if (ModelState.IsValid)
            {
                inspectionType.InspectionTypeID = Guid.NewGuid();
                db.InspectionTypes.Add(inspectionType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inspectionType);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspectionType inspectionType = await db.InspectionTypes.FindAsync(id);
            if (inspectionType == null)
            {
                return HttpNotFound();
            }
            return View(inspectionType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InspectionTypeID,InspectionType1,InspectionTypeCode")] InspectionType inspectionType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inspectionType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inspectionType);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InspectionType inspectionType = await db.InspectionTypes.FindAsync(id);
            if (inspectionType == null)
            {
                return HttpNotFound();
            }
            return View(inspectionType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            InspectionType inspectionType = await db.InspectionTypes.FindAsync(id);
            db.InspectionTypes.Remove(inspectionType);
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
