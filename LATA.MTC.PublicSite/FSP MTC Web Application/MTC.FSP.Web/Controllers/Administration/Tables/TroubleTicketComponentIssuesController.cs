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
    public class TroubleTicketComponentIssuesController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.TroubleTicketComponentIssues.ToListAsync());
        }       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Issue")] TroubleTicketComponentIssue troubleTicketComponentIssue)
        {
            if (ModelState.IsValid)
            {
                troubleTicketComponentIssue.CreatedBy = HttpContext.User.Identity.Name;
                troubleTicketComponentIssue.CreatedOn = DateTime.Now;

                troubleTicketComponentIssue.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketComponentIssue.ModifiedOn = DateTime.Now;


                db.TroubleTicketComponentIssues.Add(troubleTicketComponentIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(troubleTicketComponentIssue);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketComponentIssue troubleTicketComponentIssue = await db.TroubleTicketComponentIssues.FindAsync(id);
            if (troubleTicketComponentIssue == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketComponentIssue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Issue,CreatedOn,CreatedBy")] TroubleTicketComponentIssue troubleTicketComponentIssue)
        {
            if (ModelState.IsValid)
            {
                troubleTicketComponentIssue.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketComponentIssue.ModifiedOn = DateTime.Now;

                db.Entry(troubleTicketComponentIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(troubleTicketComponentIssue);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketComponentIssue troubleTicketComponentIssue = await db.TroubleTicketComponentIssues.FindAsync(id);
            if (troubleTicketComponentIssue == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketComponentIssue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TroubleTicketComponentIssue troubleTicketComponentIssue = await db.TroubleTicketComponentIssues.FindAsync(id);
            db.TroubleTicketComponentIssues.Remove(troubleTicketComponentIssue);
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
