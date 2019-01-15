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
    public class AssetStatusController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.AssetStatuses.ToListAsync());
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StatusName,Color")] AssetStatus assetStatus)
        {
            if (ModelState.IsValid)
            {

                assetStatus.ModifiedBy = HttpContext.User.Identity.Name;
                assetStatus.ModifiedOn = DateTime.Now;
                assetStatus.CreatedBy = HttpContext.User.Identity.Name;
                assetStatus.CreatedOn = DateTime.Now;

                db.AssetStatuses.Add(assetStatus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(assetStatus);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetStatus assetStatus = await db.AssetStatuses.FindAsync(id);
            if (assetStatus == null)
            {
                return HttpNotFound();
            }
            return View(assetStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StatusName,Color,CreatedOn,CreatedBy")] AssetStatus assetStatus)
        {
            if (ModelState.IsValid)
            {

                assetStatus.ModifiedBy = HttpContext.User.Identity.Name;
                assetStatus.ModifiedOn = DateTime.Now;

                db.Entry(assetStatus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assetStatus);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetStatus assetStatus = await db.AssetStatuses.FindAsync(id);
            if (assetStatus == null)
            {
                return HttpNotFound();
            }
            return View(assetStatus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssetStatus assetStatus = await db.AssetStatuses.FindAsync(id);
            db.AssetStatuses.Remove(assetStatus);
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
