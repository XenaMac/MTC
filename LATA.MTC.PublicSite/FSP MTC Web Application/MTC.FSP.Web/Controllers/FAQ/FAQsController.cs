using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.FAQ
{
    [CustomAuthorize]
    public class FAQsController : Controller
    {
        private readonly MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            var faqs = await db.FAQs.OrderBy(p => p.Question).ToListAsync();
            var data = faqs.Select(p => new FAQViewModel
            {
                FAQ = p,
                FAQAnswer = db.FAQAnswers.Where(a => a.FAQId == p.Id)
            });

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Question")] Models.FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                fAQ.CreatedOn = DateTime.UtcNow;
                fAQ.CreatedBy = HttpContext.User.Identity.Name;

                fAQ.ModifiedOn = DateTime.UtcNow;
                fAQ.ModifiedBy = HttpContext.User.Identity.Name;

                db.FAQs.Add(fAQ);
                await db.SaveChangesAsync();


                MTCEmailRecipient toRecipient = new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("LATATraxSupportEmail"),
                    Name = Utilities.GetApplicationSettingValue("LATATraxSupportName")
                };
                List<MTCEmailRecipient> toRecipients = new List<MTCEmailRecipient> { toRecipient };
                EmailManager.SendEmail(toRecipients, "New FAQ", fAQ.Question, null);


                return RedirectToAction("Index");
            }

            return View(fAQ);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.FAQ fAQ = await db.FAQs.FindAsync(id);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Question,CreatedBy,CreatedOn")] Models.FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                fAQ.ModifiedOn = DateTime.UtcNow;
                fAQ.ModifiedBy = HttpContext.User.Identity.Name;

                db.Entry(fAQ).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fAQ);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.FAQ fAQ = await db.FAQs.FindAsync(id);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var fAq = await db.FAQs.FindAsync(id);
            db.FAQAnswers.RemoveRange(db.FAQAnswers.Where(p => p.FAQId == id));
            db.FAQs.Remove(fAq);
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

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public ActionResult TrainingMedia()
        {
            return View();
        }
    }
}