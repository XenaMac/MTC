using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class MTCApplicationSettingsController : Controller
    {
        private readonly MTCDbContext db = new MTCDbContext();

        // GET: MTCApplicationSettings
        public ActionResult Index()
        {
            return View(db.MTCApplicationSettings.ToList());
        }

        // GET: MTCApplicationSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MTCApplicationSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Name,Value,Description,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] MTCApplicationSetting mTCApplicationSetting)
        {
            if (ModelState.IsValid)
            {
                mTCApplicationSetting.CreatedOn = DateTime.UtcNow;
                mTCApplicationSetting.ModifiedOn = DateTime.UtcNow;
                mTCApplicationSetting.CreatedBy = HttpContext.User.Identity.Name;
                mTCApplicationSetting.ModifiedBy = HttpContext.User.Identity.Name;

                db.MTCApplicationSettings.Add(mTCApplicationSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mTCApplicationSetting);
        }

        // GET: MTCApplicationSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MTCApplicationSetting mTCApplicationSetting = db.MTCApplicationSettings.Find(id);
            if (mTCApplicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(mTCApplicationSetting);
        }

        // POST: MTCApplicationSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,Name,Value,Description,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] MTCApplicationSetting mTCApplicationSetting)
        {
            if (ModelState.IsValid)
            {
                mTCApplicationSetting.ModifiedOn = DateTime.UtcNow;
                mTCApplicationSetting.ModifiedBy = HttpContext.User.Identity.Name;

                db.Entry(mTCApplicationSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mTCApplicationSetting);
        }

        // GET: MTCApplicationSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MTCApplicationSetting mTCApplicationSetting = db.MTCApplicationSettings.Find(id);
            if (mTCApplicationSetting == null)
            {
                return HttpNotFound();
            }
            return View(mTCApplicationSetting);
        }

        // POST: MTCApplicationSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MTCApplicationSetting mTCApplicationSetting = db.MTCApplicationSettings.Find(id);
            db.MTCApplicationSettings.Remove(mTCApplicationSetting);
            db.SaveChanges();
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