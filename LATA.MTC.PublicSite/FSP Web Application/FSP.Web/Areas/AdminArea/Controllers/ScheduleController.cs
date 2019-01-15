using FSP.Domain.Model;
using FSP.Web.Filters;
using FSP.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class ScheduleController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View(db.BeatSchedules.ToList());
        }

        public ActionResult IndexNew()
        {
            var model = new List<LocalBeatSchedule>();
            var results = db.GetDailySchedules();


            string weekdayFilter = "WD";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                weekdayFilter = "WE";

            ViewBag.Heading = DateTime.Today.ToString("MMMM dd, yyyy") + " Schedule";

            foreach (var item in results)
            {
                var itemIsValid = false;

                //make sure contract of contracting company is still valid
                var contracts = from q in db.Contracts
                                join c in db.Contractors on q.ContractorID equals c.ContractorID
                                where c.ContractCompanyName == item.ContractCompanyName
                                select q;

                foreach (var contract in contracts)
                {
                    if (contract.StartDate <= DateTime.Today && contract.EndDate >= DateTime.Today)
                        itemIsValid = true;
                }

                if (itemIsValid)
                {
                    model.Add(new LocalBeatSchedule
                    {
                        BeatNumber = item.beatnumber,
                        ScheduleName = item.ScheduleName,
                        ScheduleTimeTable = item.ScheduleTimeTable,
                        Supervisor = item.Supervisor,
                        CellPhone = item.CellPhone,
                        ContractCompanyName = item.ContractCompanyName,
                        PhoneNumber = item.PhoneNumber,
                        Weekday = item.Weekday
                    });
                }
            }

            if (model.Count() > 1)
                return View(model.Where(p => p.Weekday == weekdayFilter).OrderBy(p => p.BeatNumber).ThenBy(p => p.ScheduleName));
            else
                return View(model);

        }

        public ActionResult Create()
        {
            BeatSchedule model = new BeatSchedule();
            model.StartDate = DateTime.Today;
            model.EndDate = new DateTime(2050, 1, 1);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BeatSchedule BeatSchedule)
        {
            if (ModelState.IsValid)
            {
                if (!db.BeatSchedules.Any(p => p.ScheduleName == BeatSchedule.ScheduleName))
                {
                    BeatSchedule.BeatScheduleID = Guid.NewGuid();
                    db.BeatSchedules.InsertOnSubmit(BeatSchedule);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(BeatSchedule);
        }

        public ActionResult Edit(Guid id)
        {
            BeatSchedule BeatSchedule = db.BeatSchedules.Single(r => r.BeatScheduleID == id);
            if (BeatSchedule == null)
            {
                return HttpNotFound();
            }
            return View(BeatSchedule);
        }

        [HttpPost]
        public ActionResult Edit(BeatSchedule BeatSchedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.BeatSchedules.Attach(BeatSchedule);
                    db.Refresh(RefreshMode.KeepCurrentValues, BeatSchedule);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(BeatSchedule);
        }

        public ActionResult Delete(Guid id)
        {
            BeatSchedule BeatSchedule = db.BeatSchedules.Single(r => r.BeatScheduleID == id);
            if (BeatSchedule == null)
            {
                return HttpNotFound();
            }
            return View(BeatSchedule);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BeatSchedule BeatSchedule = db.BeatSchedules.Single(r => r.BeatScheduleID == id);
            db.BeatSchedules.DeleteOnSubmit(BeatSchedule);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

    }
}
