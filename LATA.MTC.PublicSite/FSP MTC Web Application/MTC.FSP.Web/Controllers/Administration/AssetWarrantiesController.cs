using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,DataConsultant")]
    public class AssetWarrantiesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
      
        public async Task<ActionResult> Index()
        {
            return View(await db.AssetWarranties.ToListAsync());
        }

        public ActionResult GetAll()
        {
            var assetWarranties = db.AssetWarranties.ToList().Select(q => new
            {
                Id = q.Id,
                Item = q.Item,
                LATAIDNumber = q.LATAIDNumber,
                OEMSerialNumber = q.OEMSerialNumber,
                WarrantyEndDate = q.WarrantyEndDate,
                OperatingSystem = q.OperatingSystem,
                OEMSoftware = q.OEMSoftware,
                LATASoftware = q.LATASoftware,
                Notes = q.Notes
            });
            return Json(assetWarranties, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Item,LATAIDNumber,OEMSerialNumber,WarrantyEndDate,OperatingSystem,OEMSoftware,LATASoftware,Notes")] AssetWarranty assetWarranty)
        {
            if (ModelState.IsValid)
            {

                assetWarranty.CreatedBy = HttpContext.User.Identity.Name;
                assetWarranty.ModifiedBy = HttpContext.User.Identity.Name;
                assetWarranty.CreatedOn = DateTime.Now;
                assetWarranty.ModifiedOn = DateTime.Now;

                db.AssetWarranties.Add(assetWarranty);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(assetWarranty);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetWarranty assetWarranty = await db.AssetWarranties.FindAsync(id);
            if (assetWarranty == null)
            {
                return HttpNotFound();
            }
            return View(assetWarranty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Item,LATAIDNumber,OEMSerialNumber,WarrantyEndDate,OperatingSystem,OEMSoftware,LATASoftware,Notes,CreatedOn,CreatedBy")] AssetWarranty assetWarranty)
        {
            if (ModelState.IsValid)
            {
                assetWarranty.ModifiedBy = HttpContext.User.Identity.Name;             
                assetWarranty.ModifiedOn = DateTime.Now;

                db.Entry(assetWarranty).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(assetWarranty);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetWarranty assetWarranty = await db.AssetWarranties.FindAsync(id);
            if (assetWarranty == null)
            {
                return HttpNotFound();
            }
            return View(assetWarranty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssetWarranty assetWarranty = await db.AssetWarranties.FindAsync(id);
            db.AssetWarranties.Remove(assetWarranty);
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
