using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    public class MerchandiseProductSizesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
       
        public async Task<ActionResult> Index()
        {
            return View(await db.MerchandiseProductSizes.ToListAsync());
        }       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Size")] MerchandiseProductSize merchandiseProductSize)
        {
            if (ModelState.IsValid)
            {
                merchandiseProductSize.CreatedBy = HttpContext.User.Identity.Name;
                merchandiseProductSize.ModifiedBy = HttpContext.User.Identity.Name;
                merchandiseProductSize.CreatedOn = DateTime.Now;
                merchandiseProductSize.ModifiedOn = DateTime.Now;

                db.MerchandiseProductSizes.Add(merchandiseProductSize);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(merchandiseProductSize);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchandiseProductSize merchandiseProductSize = await db.MerchandiseProductSizes.FindAsync(id);
            if (merchandiseProductSize == null)
            {
                return HttpNotFound();
            }
            return View(merchandiseProductSize);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Size,CreatedOn,CreatedBy")] MerchandiseProductSize merchandiseProductSize)
        {
            if (ModelState.IsValid)
            {
                
                merchandiseProductSize.ModifiedBy = HttpContext.User.Identity.Name;
                merchandiseProductSize.ModifiedOn = DateTime.Now;

                db.Entry(merchandiseProductSize).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(merchandiseProductSize);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchandiseProductSize merchandiseProductSize = await db.MerchandiseProductSizes.FindAsync(id);
            if (merchandiseProductSize == null)
            {
                return HttpNotFound();
            }
            return View(merchandiseProductSize);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MerchandiseProductSize merchandiseProductSize = await db.MerchandiseProductSizes.FindAsync(id);
            db.MerchandiseProductSizes.Remove(merchandiseProductSize);
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
