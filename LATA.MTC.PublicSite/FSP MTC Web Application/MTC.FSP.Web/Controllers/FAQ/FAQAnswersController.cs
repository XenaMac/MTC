using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.FAQ
{
    [CustomAuthorize]
    public class FAQAnswersController : Controller
    {
        private MTCDbContext db = new MTCDbContext();
        
       
        public ActionResult Create(int faqId)
        {
            ViewBag.FAQId = new SelectList(db.FAQs, "Id", "Question", faqId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FAQId,Answer")] FAQAnswer fAQAnswer)
        {
            if (ModelState.IsValid)
            {

                fAQAnswer.CreatedOn = DateTime.UtcNow;
                fAQAnswer.CreatedBy = HttpContext.User.Identity.Name;

                fAQAnswer.ModifiedOn = DateTime.UtcNow;
                fAQAnswer.ModifiedBy = HttpContext.User.Identity.Name;

                db.FAQAnswers.Add(fAQAnswer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "FAQs");
            }

            ViewBag.FAQId = new SelectList(db.FAQs, "Id", "Question", fAQAnswer.FAQId);
            return View(fAQAnswer);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQAnswer fAQAnswer = await db.FAQAnswers.FindAsync(id);
            if (fAQAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.FAQId = new SelectList(db.FAQs, "Id", "Question", fAQAnswer.FAQId);
            return View(fAQAnswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FAQId,Answer,CreatedBy,CreatedOn")] FAQAnswer fAQAnswer)
        {
            if (ModelState.IsValid)
            {
                fAQAnswer.ModifiedOn = DateTime.UtcNow;
                fAQAnswer.ModifiedBy = HttpContext.User.Identity.Name;

                db.Entry(fAQAnswer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "FAQs");
            }
            ViewBag.FAQId = new SelectList(db.FAQs, "Id", "Question", fAQAnswer.FAQId);
            return View(fAQAnswer);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAQAnswer fAQAnswer = await db.FAQAnswers.FindAsync(id);
            if (fAQAnswer == null)
            {
                return HttpNotFound();
            }
            return View(fAQAnswer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FAQAnswer fAQAnswer = await db.FAQAnswers.FindAsync(id);
            db.FAQAnswers.Remove(fAQAnswer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "FAQs");
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
