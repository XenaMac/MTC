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
    public class ViolationTypesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();


        public async Task<ActionResult> Index()
        {
            return View(await db.ViolationTypes.ToListAsync());
        }

        public ActionResult GetAll()
        {
            var data = db.ViolationTypes.OrderBy(p => p.Code).ToList().Select(p => new
            {
                Id = p.Id,
                Text = p.Code + ". " + p.Name
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,Name,Description,ViolationTypeSeverity,MaxViolationTypeSeverity,AppliesTo,IsAVLViolation,ApplicableLATATraxAlarm,DetectableByLATATraxAlone,ApplicableReports")] ViolationType violationType)
        {
            if (ModelState.IsValid)
            {
                violationType.CreatedBy = HttpContext.User.Identity.Name;
                violationType.CreatedOn = DateTime.Now;

                violationType.ModifiedBy = HttpContext.User.Identity.Name;
                violationType.ModifiedOn = DateTime.Now;

                db.ViolationTypes.Add(violationType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(violationType);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationType violationType = await db.ViolationTypes.FindAsync(id);
            if (violationType == null)
            {
                return HttpNotFound();
            }
            return View(violationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,Name,Description,ViolationTypeSeverity,MaxViolationTypeSeverity,AppliesTo,IsAVLViolation,ApplicableLATATraxAlarm,DetectableByLATATraxAlone,ApplicableReports,CreatedOn,CreatedBy")] ViolationType violationType)
        {
            if (ModelState.IsValid)
            {
                violationType.ModifiedBy = HttpContext.User.Identity.Name;
                violationType.ModifiedOn = DateTime.Now;

                db.Entry(violationType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(violationType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationType violationType = await db.ViolationTypes.FindAsync(id);
            if (violationType == null)
            {
                return HttpNotFound();
            }
            return View(violationType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViolationType violationType = await db.ViolationTypes.FindAsync(id);
            db.ViolationTypes.Remove(violationType);
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
