using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class YearlyCalendarController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View(db.YearlyCalendars.OrderBy(p => p.Date).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(YearlyCalendar yearlyCalendar)
        {
            if (ModelState.IsValid)
            {
                if (!db.YearlyCalendars.Any(p => p.dayName == yearlyCalendar.dayName))
                {
                    //if VehicleType does not exist yet       
                    yearlyCalendar.DateID = Guid.NewGuid();
                    db.YearlyCalendars.InsertOnSubmit(yearlyCalendar);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(yearlyCalendar);
        }


        public ActionResult Edit(Guid id)
        {
            YearlyCalendar YearlyCalendar = db.YearlyCalendars.Single(r => r.DateID == id);
            if (YearlyCalendar == null)
            {
                return HttpNotFound();
            }
            return View(YearlyCalendar);
        }

        [HttpPost]
        public ActionResult Edit(YearlyCalendar yearlyCalendar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.YearlyCalendars.Attach(yearlyCalendar);
                    db.Refresh(RefreshMode.KeepCurrentValues, yearlyCalendar);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(yearlyCalendar);
        }


        public ActionResult Delete(Guid id)
        {
            YearlyCalendar YearlyCalendar = db.YearlyCalendars.Single(r => r.DateID == id);
            if (YearlyCalendar == null)
            {
                return HttpNotFound();
            }
            return View(YearlyCalendar);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            YearlyCalendar YearlyCalendar = db.YearlyCalendars.Single(r => r.DateID == id);
            db.YearlyCalendars.DeleteOnSubmit(YearlyCalendar);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}
