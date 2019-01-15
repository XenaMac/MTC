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
    public class ViolationStatusTypesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();


        public async Task<ActionResult> Index()
        {
            return View(await db.ViolationStatusTypes.ToListAsync());
        }

        public async Task<ActionResult> GetAll()
        {            
            var data = db.ViolationStatusTypes.OrderByDescending(p => p.IsDefault).ToList().Select(p => new
            {
                Id = p.Id,
                Text = p.Name
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,IsDefault")] ViolationStatusType violationStatusType)
        {
            if (ModelState.IsValid)
            {
                violationStatusType.CreatedBy = HttpContext.User.Identity.Name;
                violationStatusType.ModifiedBy = HttpContext.User.Identity.Name;
                violationStatusType.CreatedOn = DateTime.Now;
                violationStatusType.ModifiedOn = DateTime.Now;

                db.ViolationStatusTypes.Add(violationStatusType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(violationStatusType);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationStatusType violationStatusType = await db.ViolationStatusTypes.FindAsync(id);
            if (violationStatusType == null)
            {
                return HttpNotFound();
            }
            return View(violationStatusType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,IsDefault,CreatedOn,CreatedBy")] ViolationStatusType violationStatusType)
        {
            if (ModelState.IsValid)
            {

                violationStatusType.ModifiedBy = HttpContext.User.Identity.Name;
                violationStatusType.ModifiedOn = DateTime.Now;


                db.Entry(violationStatusType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(violationStatusType);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViolationStatusType violationStatusType = await db.ViolationStatusTypes.FindAsync(id);
            if (violationStatusType == null)
            {
                return HttpNotFound();
            }
            return View(violationStatusType);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ViolationStatusType violationStatusType = await db.ViolationStatusTypes.FindAsync(id);
            db.ViolationStatusTypes.Remove(violationStatusType);
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
