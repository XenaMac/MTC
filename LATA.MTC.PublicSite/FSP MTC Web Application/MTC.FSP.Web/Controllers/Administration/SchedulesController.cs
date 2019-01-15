using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers
{

    [CustomAuthorize(Roles = "Admin,Contractor")]
    public class SchedulesController : Controller
    {
        public ActionResult Index()
        {

            using (MTCDBEntities db = new MTCDBEntities())
            {
                var model = new List<MtcBeatSchedule>();
                var dbSchedules = db.GetDailySchedules();

                string weekdayFilter = "WD";
                if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    weekdayFilter = "WE";

                ViewBag.Heading = DateTime.Today.ToString("MMMM dd, yyyy") + " Schedule";


                foreach (var dbSchedule in dbSchedules)
                {
                    var itemIsValid = false;

                    //make sure contract of contracting company is still valid
                    var contracts = from q in db.Contracts
                                    join c in db.Contractors on q.ContractorID equals c.ContractorID
                                    where c.ContractCompanyName == dbSchedule.ContractCompanyName
                                    select q;

                    foreach (var contract in contracts)
                    {
                        if (contract.StartDate <= DateTime.Today && contract.EndDate >= DateTime.Today)
                            itemIsValid = true;
                    }

                    if (itemIsValid)
                    {
                        model.Add(new MtcBeatSchedule
                        {
                            BeatNumber = dbSchedule.beatnumber,
                            ScheduleName = dbSchedule.ScheduleName,
                            ScheduleTimeTable = dbSchedule.ScheduleTimeTable,
                            Supervisor = dbSchedule.Supervisor,
                            CellPhone = dbSchedule.CellPhone,
                            ContractCompanyName = dbSchedule.ContractCompanyName,
                            PhoneNumber = dbSchedule.PhoneNumber,
                            Weekday = dbSchedule.Weekday
                        });
                    }
                }

                if (model.Count() > 1)
                    return View(model.Where(p => p.Weekday == weekdayFilter).OrderBy(p => p.BeatNumber).ThenBy(p => p.ScheduleName));
                else
                    return View(model);


            }

        }

        public ActionResult WeeklySchedule()
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                return View(db.BeatSchedules.ToList());
            }
        }

        public ActionResult CreateWeeklySchedule()
        {
            BeatSchedule model = new BeatSchedule();
            //model.StartDate = DateTime.Today;
            //model.EndDate = new DateTime(2050, 1, 1);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateWeeklySchedule(BeatSchedule BeatSchedule)
        {
            if (ModelState.IsValid)
            {
                using (MTCDBEntities db = new MTCDBEntities())
                {
                    if (!db.BeatSchedules.Any(p => p.ScheduleName == BeatSchedule.ScheduleName))
                    {
                        BeatSchedule.BeatScheduleID = Guid.NewGuid();
                        db.BeatSchedules.Add(BeatSchedule);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("WeeklySchedule");
            }

            return View(BeatSchedule);
        }

        public ActionResult EditWeeklySchedule(Guid id)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                BeatSchedule BeatSchedule = db.BeatSchedules.Single(r => r.BeatScheduleID == id);
                if (BeatSchedule == null)
                {
                    return HttpNotFound();
                }
                return View(BeatSchedule);
            }

        }

        [HttpPost]
        public ActionResult EditWeeklySchedule(BeatSchedule BeatSchedule)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(BeatSchedule).State = System.Data.Entity.EntityState.Modified;                  
                    db.SaveChanges();

                    return RedirectToAction("WeeklySchedule");
                }
                return View(BeatSchedule);
            }
          
        }

        public ActionResult DeleteWeeklySchedule(Guid id)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                BeatSchedule BeatSchedule = db.BeatSchedules.Single(r => r.BeatScheduleID == id);
                if (BeatSchedule == null)
                {
                    return HttpNotFound();
                }
                return View(BeatSchedule);
            }
          
        }

        [HttpPost, ActionName("DeleteWeeklySchedule")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                BeatSchedule BeatSchedule = db.BeatSchedules.Find(id);
                db.BeatSchedules.Remove(BeatSchedule);
                db.SaveChanges();
                return RedirectToAction("WeeklySchedule");
            }
        
        }
    }
}