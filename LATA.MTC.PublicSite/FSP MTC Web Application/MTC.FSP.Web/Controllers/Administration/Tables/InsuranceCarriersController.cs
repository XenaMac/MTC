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
    public class InsuranceCarriersController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        // GET: InsuranceCarriers
        public async Task<ActionResult> Index()
        {
            return View(await db.InsuranceCarriers.ToListAsync());
        }      

        // GET: InsuranceCarriers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InsuranceCarriers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InsuranceID,CarrierName,InsurancePolicyNumber,ExpirationDate,PolicyName,PhoneNumber,Fax,Email")] InsuranceCarrier insuranceCarrier)
        {
            if (ModelState.IsValid)
            {
                insuranceCarrier.InsuranceID = Guid.NewGuid();
                db.InsuranceCarriers.Add(insuranceCarrier);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(insuranceCarrier);
        }

        // GET: InsuranceCarriers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InsuranceCarrier insuranceCarrier = await db.InsuranceCarriers.FindAsync(id);
            if (insuranceCarrier == null)
            {
                return HttpNotFound();
            }
            return View(insuranceCarrier);
        }

        // POST: InsuranceCarriers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InsuranceID,CarrierName,InsurancePolicyNumber,ExpirationDate,PolicyName,PhoneNumber,Fax,Email")] InsuranceCarrier insuranceCarrier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuranceCarrier).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(insuranceCarrier);
        }

        // GET: InsuranceCarriers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InsuranceCarrier insuranceCarrier = await db.InsuranceCarriers.FindAsync(id);
            if (insuranceCarrier == null)
            {
                return HttpNotFound();
            }
            return View(insuranceCarrier);
        }

        // POST: InsuranceCarriers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            InsuranceCarrier insuranceCarrier = await db.InsuranceCarriers.FindAsync(id);
            db.InsuranceCarriers.Remove(insuranceCarrier);
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
