using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    [CustomAuthorize(Roles = "Admin")]
    public class MerchandiseProductsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
       
        public async Task<ActionResult> IndexNG()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetProducts()
        {
            var merchandiseProducts = db.MerchandiseProducts.Include(m => m.MerchandiseProductSize).OrderBy(p => p.OrderNumber);
            return Json(merchandiseProducts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.Sizes = new SelectList(db.MerchandiseProductSizes, "Id", "Size");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DisplayName,Description,UnitCost,UnitsInStock,MerchandiseProductSizeId")] MerchandiseProduct merchandiseProduct)
        {
            if (ModelState.IsValid)
            {
                merchandiseProduct.ModifiedBy = HttpContext.User.Identity.Name;
                merchandiseProduct.ModifiedOn = DateTime.Now;
                merchandiseProduct.CreatedBy = HttpContext.User.Identity.Name;
                merchandiseProduct.CreatedOn = DateTime.Now;
                merchandiseProduct.OrderNumber = db.MerchandiseProducts.Max(p => p.OrderNumber) + 1;

                db.MerchandiseProducts.Add(merchandiseProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("IndexNG");
            }

            ViewBag.Sizes = new SelectList(db.MerchandiseProductSizes, "Id", "Size", merchandiseProduct.MerchandiseProductSizeId);
            return View(merchandiseProduct);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchandiseProduct merchandiseProduct = await db.MerchandiseProducts.FindAsync(id);
            if (merchandiseProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sizes = new SelectList(db.MerchandiseProductSizes, "Id", "Size", merchandiseProduct.MerchandiseProductSizeId);
            return View(merchandiseProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DisplayName,Description,UnitCost,UnitsInStock,MerchandiseProductSizeId,CreatedOn,CreatedBy,OrderNumber")] MerchandiseProduct merchandiseProduct)
        {
            if (ModelState.IsValid)
            {
                merchandiseProduct.ModifiedBy = HttpContext.User.Identity.Name;
                merchandiseProduct.ModifiedOn = DateTime.Now;

                db.Entry(merchandiseProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexNG");
            }
            ViewBag.Sizes = new SelectList(db.MerchandiseProductSizes, "Id", "Size", merchandiseProduct.MerchandiseProductSizeId);
            return View(merchandiseProduct);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MerchandiseProduct merchandiseProduct = await db.MerchandiseProducts.FindAsync(id);
            if (merchandiseProduct == null)
            {
                return HttpNotFound();
            }
            return View(merchandiseProduct);
        }

        public async Task<ActionResult> MoveUp(int id)
        {
            //Product1 OrderNumber = 3
            //Product2 OrderNumber = 4

            MerchandiseProduct Product2 = await db.MerchandiseProducts.FindAsync(id);
            MerchandiseProduct Product1 = null;

            var counter = 1;
            var newOrderValue = Product2.OrderNumber;
            do
            {
                newOrderValue = newOrderValue - counter;
                Product1 = await db.MerchandiseProducts.FirstOrDefaultAsync(p => p.OrderNumber == newOrderValue);
                counter++;
            } while (Product1 == null);

            Product1.OrderNumber = Product2.OrderNumber;
            Product2.OrderNumber = newOrderValue;
                        
            await db.SaveChangesAsync();

            this.FixOrderNumbers();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MoveDown(int id)
        {
            //Product2 OrderNumber = 3
            //Product1 OrderNumber = 4

            MerchandiseProduct Product2 = await db.MerchandiseProducts.FindAsync(id);
            MerchandiseProduct Product1 = null;
            var counter = 0;
            var newOrderValue = Product2.OrderNumber;
            do
            {
                counter++;
                newOrderValue = newOrderValue + counter;
                Product1 = await db.MerchandiseProducts.FirstOrDefaultAsync(p => p.OrderNumber == newOrderValue);
                
            } while (Product1 == null);

            Product1.OrderNumber = Product2.OrderNumber;
            Product2.OrderNumber = newOrderValue;
                        
            await db.SaveChangesAsync();

            this.FixOrderNumbers();

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var merchandiseProduct = await db.MerchandiseProducts.FindAsync(id);
            db.MerchandiseProducts.Remove(merchandiseProduct);

            var orderDetails = await db.MerchandiseOrderDetails.Where(p => p.MerchandiseProductId == id).ToListAsync();
            db.MerchandiseOrderDetails.RemoveRange(orderDetails);

            await db.SaveChangesAsync();

            this.FixOrderNumbers();

            return RedirectToAction("IndexNG");
        }


        private void FixOrderNumbers()
        {
            var products = db.MerchandiseProducts.OrderBy(p => p.OrderNumber).ToList();
            var counter = 1;
            foreach (var product in products)
            {
                product.OrderNumber = counter;
                counter++;
            }
            db.SaveChanges();
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
