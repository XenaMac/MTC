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

namespace MTC.FSP.Web.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    public class YearlyCalendarsController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.YearlyCalendars.OrderBy(p => p.dayName).ToListAsync());
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyCalendar yearlyCalendar = await db.YearlyCalendars.FindAsync(id);
            if (yearlyCalendar == null)
            {
                return HttpNotFound();
            }
            return View(yearlyCalendar);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DateID,dayName,Date")] YearlyCalendar yearlyCalendar)
        {
            if (ModelState.IsValid)
            {
                yearlyCalendar.DateID = Guid.NewGuid();
                db.YearlyCalendars.Add(yearlyCalendar);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(yearlyCalendar);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyCalendar yearlyCalendar = await db.YearlyCalendars.FindAsync(id);
            if (yearlyCalendar == null)
            {
                return HttpNotFound();
            }
            return View(yearlyCalendar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DateID,dayName,Date")] YearlyCalendar yearlyCalendar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yearlyCalendar).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(yearlyCalendar);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearlyCalendar yearlyCalendar = await db.YearlyCalendars.FindAsync(id);
            if (yearlyCalendar == null)
            {
                return HttpNotFound();
            }
            return View(yearlyCalendar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            YearlyCalendar yearlyCalendar = await db.YearlyCalendars.FindAsync(id);
            db.YearlyCalendars.Remove(yearlyCalendar);
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
