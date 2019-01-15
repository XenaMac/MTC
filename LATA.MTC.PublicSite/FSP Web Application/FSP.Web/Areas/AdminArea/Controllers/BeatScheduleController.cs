using FSP.Domain.Model;
using System;
using System.Data.Linq;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using FSP.Web.Areas.AdminArea.ViewModels;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class BeatScheduleController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(String BeatID, String SelectedSchedules, String StartDate, String EndDate)
        {
            bool retvalue = false;

            try
            {
                dynamic result = JsonConvert.DeserializeObject(SelectedSchedules);

                if (result != null)
                {
                    //first remove all schedules for this beat
                    db.BeatBeatSchedules.DeleteAllOnSubmit(db.BeatBeatSchedules.Where(p => p.BeatID == Guid.Parse(BeatID)));

                    foreach (var item in result)
                    {
                        BeatBeatSchedule beatBeatSchedule = new Domain.Model.BeatBeatSchedule();
                        beatBeatSchedule.BeatBeatScheduleID = Guid.NewGuid();
                        beatBeatSchedule.BeatID = Guid.Parse(BeatID);
                        beatBeatSchedule.BeatScheduleID = Guid.Parse(item.ToString());
                        db.BeatBeatSchedules.InsertOnSubmit(beatBeatSchedule);
                    }

                    db.SubmitChanges();
                    retvalue = true;
                }
            }
            catch { }

            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(String BeatID)
        {
            bool retvalue = false;

            try
            {
                //first remove all schedules for this beat
                db.BeatBeatSchedules.DeleteAllOnSubmit(db.BeatBeatSchedules.Where(p => p.BeatID == Guid.Parse(BeatID)));
                db.SubmitChanges();
                retvalue = true;
            }
            catch { }

            return Json(retvalue, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetSchedules()
        {
            var data = from q in db.BeatSchedules
                       orderby q.Weekday, q.OnPatrol
                       select new
                       {
                           ScheduleName = q.ScheduleName,
                           ScheduleID = q.BeatScheduleID,
                           OnPatrol = q.OnPatrol,
                           RollIn = q.RollIn,
                           Weekday = q.Weekday,
                           Start = q.StartDate.ToShortDateString(),
                           End = q.EndDate.ToShortDateString()
                       };

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBeatsWithoutSchedules()
        {
            List<Guid> beatsWithSchedules = db.BeatBeatSchedules.Select(p => p.BeatID).Distinct().ToList();

            var allBeats = from q in db.vBeats
                           orderby q.BeatNumber
                           select new
                           {
                               BeatNumber = q.BeatNumber,
                               BeatID = q.BeatID
                           };

            var beatsWithoutSchedule = from q in allBeats
                                       where !beatsWithSchedules.Contains(q.BeatID)
                                       select q;


            return Json(beatsWithoutSchedule.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBeatSchedules()
        {
            List<BeatBeatScheduleViewModel> returnList = new List<BeatBeatScheduleViewModel>();

            try
            {
                var model = (from q in db.BeatBeatSchedules
                             join s in db.BeatSchedules on q.BeatScheduleID equals s.BeatScheduleID
                             join v in db.vBeats on q.BeatID equals v.BeatID
                             select new
                             {
                                 BeatBeatScheduleID = q.BeatBeatScheduleID,
                                 Beat = v.BeatNumber,
                                 BeatID = v.BeatID,
                                 Schedule = s,
                                 OnPatrol = s.OnPatrol

                             }).ToList();




                returnList = (from q in model
                              group q by q.Beat into g
                              select new BeatBeatScheduleViewModel
                              {
                                  Beat = g.Key,
                                  BeatID = g.FirstOrDefault().BeatID,
                                  BeatBeatScheduleID = g.FirstOrDefault().BeatBeatScheduleID,
                                  //Schedule = String.Join(", ", g.ToList().OrderBy(p => p.OnPatrol).Select(p => p.Schedule)),
                                  //Schedule = g.FirstOfDefault().
                                  Schedule = (from q in g
                                              select new BeatScheduleViewModel
                                              {
                                                  ScheduleName = q.Schedule.ScheduleName,
                                                  Start = q.Schedule.StartDate.ToShortDateString(),
                                                  End = q.Schedule.EndDate.ToShortDateString()
                                              }).OrderBy(p => p.ScheduleName).ToList(),
                                  ScheduleList = g.ToList().Select(p => p.Schedule.ScheduleName).ToList()
                              }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Json(returnList.OrderBy(p => p.Beat), JsonRequestBehavior.AllowGet);
        }
    }
}
