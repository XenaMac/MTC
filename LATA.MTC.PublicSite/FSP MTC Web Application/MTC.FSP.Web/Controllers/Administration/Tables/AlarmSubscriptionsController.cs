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
    public class AlarmSubscriptionsController : Controller
    {
        private MTCDbContext db = new MTCDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.AlarmSubscriptions.ToListAsync());
        }

        public ActionResult MyAlarms()
        {
            return View();
        }

        public ActionResult GetMyAlarms()
        {
            var data = db.AlarmSubscriptions.Where(p => p.UserEmail == HttpContext.User.Identity.Name).Select(p => p.SubscribedAlarmName).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveMyAlarms(List<String> alarms)
        {
            if (alarms != null)
            {
                db.AlarmSubscriptions.RemoveRange(db.AlarmSubscriptions.Where(p => p.UserEmail == HttpContext.User.Identity.Name));

                foreach (var alarm in alarms)
                {
                    AlarmSubscription sub = new AlarmSubscription();

                    sub.UserEmail = HttpContext.User.Identity.Name;
                    sub.SubscribedAlarmName = alarm.ToUpper();

                    sub.CreatedBy = HttpContext.User.Identity.Name;
                    sub.ModifiedBy = HttpContext.User.Identity.Name;
                    sub.ModifiedOn = DateTime.Now;
                    sub.CreatedOn = DateTime.Now;

                    db.AlarmSubscriptions.Add(sub);
                }

                db.SaveChanges();

            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserEmail,SubscribedAlarmName")] AlarmSubscription alarmSubscription)
        {
            if (ModelState.IsValid)
            {
                alarmSubscription.CreatedBy = HttpContext.User.Identity.Name;
                alarmSubscription.ModifiedBy = HttpContext.User.Identity.Name;
                alarmSubscription.ModifiedOn = DateTime.Now;
                alarmSubscription.CreatedOn = DateTime.Now;

                db.AlarmSubscriptions.Add(alarmSubscription);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(alarmSubscription);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlarmSubscription alarmSubscription = await db.AlarmSubscriptions.FindAsync(id);
            if (alarmSubscription == null)
            {
                return HttpNotFound();
            }
            return View(alarmSubscription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserEmail,SubscribedAlarmName,CreatedOn,CreatedByy")] AlarmSubscription alarmSubscription)
        {
            if (ModelState.IsValid)
            {
                alarmSubscription.ModifiedBy = HttpContext.User.Identity.Name;
                alarmSubscription.ModifiedOn = DateTime.Now;

                db.Entry(alarmSubscription).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(alarmSubscription);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlarmSubscription alarmSubscription = await db.AlarmSubscriptions.FindAsync(id);
            if (alarmSubscription == null)
            {
                return HttpNotFound();
            }
            return View(alarmSubscription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AlarmSubscription alarmSubscription = await db.AlarmSubscriptions.FindAsync(id);
            db.AlarmSubscriptions.Remove(alarmSubscription);
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
