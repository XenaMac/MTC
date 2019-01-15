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
    public class DispatchCodesController : Controller
    {
        private readonly MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.DispatchCodes.ToListAsync());
        }

        public ActionResult GetAll()
        {
            var data = from d in db.DispatchCodes
                select new
                {
                    d.Id,
                    Name = d.CodeDescription + "(" + d.Code + ")"
                };

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,CodeDescription")] DispatchCode dispatchCode)
        {
            if (ModelState.IsValid)
            {
                dispatchCode.CreatedBy = HttpContext.User.Identity.Name;
                dispatchCode.CreatedOn = DateTime.Now;
                dispatchCode.ModifiedBy = HttpContext.User.Identity.Name;
                dispatchCode.ModifiedOn = DateTime.Now;


                db.DispatchCodes.Add(dispatchCode);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dispatchCode);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispatchCode dispatchCode = await db.DispatchCodes.FindAsync(id);
            if (dispatchCode == null)
            {
                return HttpNotFound();
            }
            return View(dispatchCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,Code,CodeDescription,CreatedOn,CreatedBy")] DispatchCode dispatchCode)
        {
            if (ModelState.IsValid)
            {
                dispatchCode.ModifiedBy = HttpContext.User.Identity.Name;
                dispatchCode.ModifiedOn = DateTime.Now;

                db.Entry(dispatchCode).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dispatchCode);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispatchCode dispatchCode = await db.DispatchCodes.FindAsync(id);
            if (dispatchCode == null)
            {
                return HttpNotFound();
            }
            return View(dispatchCode);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DispatchCode dispatchCode = await db.DispatchCodes.FindAsync(id);
            db.DispatchCodes.Remove(dispatchCode);
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