using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class VarsController : Controller
    {
        private readonly MTCDBEntities db = new MTCDBEntities();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VarID,VarName,VarValue,Description,Units,IsMTCAlarm,FriendlyVarName")] Var var)
        {
            if (ModelState.IsValid)
            {
                var.VarID = Guid.NewGuid();
                db.Vars.Add(var);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(var);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var var = await db.Vars.FindAsync(id);
            if (var == null)
                return HttpNotFound();
            return View(var);
        }

        // POST: Vars/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var var = await db.Vars.FindAsync(id);
            db.Vars.Remove(var);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var var = await db.Vars.FindAsync(id);
            if (var == null)
                return HttpNotFound();
            return View(var);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VarID,VarName,VarValue,Description,Units,IsMTCAlarm,FriendlyVarName")] Var var)
        {
            if (ModelState.IsValid)
            {
                db.Entry(var).State = EntityState.Modified;
                db.SaveChanges();

                using (var service = new TowTruckServiceClient())
                {
                    service.UpdateVar(var.VarName, var.VarValue);
                }

                //Var dbVar = db.Vars.Find(var.VarID);
                //if (dbVar != null)
                //{
                //    dbVar.FriendlyVarName = var.FriendlyVarName;
                //    dbVar.Description = var.Description;
                //    dbVar.Units = var.Units;
                //    dbVar.IsMTCAlarm = var.IsMTCAlarm;
                //    db.Entry(dbVar).State = EntityState.Modified;
                //    await db.SaveChangesAsync();
                //}

                return RedirectToAction("Index");
            }
            return View(var);
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Vars.Where(p => p.VarName.Contains("Leeway")).OrderBy(p => p.VarName).ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}