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
    public class TroubleTicketProblemsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
       
        public async Task<ActionResult> Index()
        {
            return View(await db.TroubleTicketProblems.ToListAsync());
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Problem")] TroubleTicketProblem troubleTicketProblem)
        {
            if (ModelState.IsValid)
            {

                troubleTicketProblem.CreatedBy = HttpContext.User.Identity.Name;
                troubleTicketProblem.CreatedOn = DateTime.Now;

                troubleTicketProblem.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketProblem.ModifiedOn = DateTime.Now;

                db.TroubleTicketProblems.Add(troubleTicketProblem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(troubleTicketProblem);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketProblem troubleTicketProblem = await db.TroubleTicketProblems.FindAsync(id);
            if (troubleTicketProblem == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketProblem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Problem,CreatedOn,CreatedBy")] TroubleTicketProblem troubleTicketProblem)
        {
            if (ModelState.IsValid)
            {

                troubleTicketProblem.ModifiedBy = HttpContext.User.Identity.Name;
                troubleTicketProblem.ModifiedOn = DateTime.Now;

                db.Entry(troubleTicketProblem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(troubleTicketProblem);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TroubleTicketProblem troubleTicketProblem = await db.TroubleTicketProblems.FindAsync(id);
            if (troubleTicketProblem == null)
            {
                return HttpNotFound();
            }
            return View(troubleTicketProblem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TroubleTicketProblem troubleTicketProblem = await db.TroubleTicketProblems.FindAsync(id);
            db.TroubleTicketProblems.Remove(troubleTicketProblem);
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
