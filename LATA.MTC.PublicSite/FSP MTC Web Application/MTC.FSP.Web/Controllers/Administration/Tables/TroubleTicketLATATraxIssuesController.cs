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
    public class TroubleTicketLATATraxIssuesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.TroubleTicketLATATraxIssues.ToListAsync());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Issue")] TroubleTicketLATATraxIssue troubleTicketLATATraxIssue)
        {
            if (ModelState.IsValid)
            {

                troubleTicketLATATraxIssue.CreatedBy = HttpContext.User.Identity.Name;
                troubleTicketLATATraxIssue.CreatedOn = DateTime.Now;

                troubleTicketLATATraxIssue.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketLATATraxIssue.ModifiedOn = DateTime.Now;

                db.TroubleTicketLATATraxIssues.Add(troubleTicketLATATraxIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(troubleTicketLATATraxIssue);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketLATATraxIssue troubleTicketLATATraxIssue = await db.TroubleTicketLATATraxIssues.FindAsync(id);
            if (troubleTicketLATATraxIssue == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketLATATraxIssue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Issue,CreatedOn,CreatedBy")] TroubleTicketLATATraxIssue troubleTicketLATATraxIssue)
        {
            if (ModelState.IsValid)
            {
                troubleTicketLATATraxIssue.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketLATATraxIssue.ModifiedOn = DateTime.Now;

                db.Entry(troubleTicketLATATraxIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(troubleTicketLATATraxIssue);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketLATATraxIssue troubleTicketLATATraxIssue = await db.TroubleTicketLATATraxIssues.FindAsync(id);
            if (troubleTicketLATATraxIssue == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketLATATraxIssue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TroubleTicketLATATraxIssue troubleTicketLATATraxIssue = await db.TroubleTicketLATATraxIssues.FindAsync(id);
            db.TroubleTicketLATATraxIssues.Remove(troubleTicketLATATraxIssue);
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
