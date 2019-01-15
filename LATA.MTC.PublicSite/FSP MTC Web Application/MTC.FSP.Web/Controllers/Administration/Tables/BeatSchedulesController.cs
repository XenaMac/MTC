using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class BeatSchedulesController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        // GET: BeatSchedules
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> GetBeatSchedules()
        {
            //List<MtcBeatScheduleViewModel> returnList = new List<MtcBeatScheduleViewModel>();

            //var model = (from q in db.BeatSchedules
            //             join v in db.Beats on q.BeatId equals v.BeatID
            //             select new
            //             {
            //                 Beat = v.BeatNumber,
            //                 BeatID = v.BeatID,
            //             }).ToList();



            //returnList = (from q in model
            //              group q by q.Beat into g
            //              select new MtcBeatScheduleViewModel
            //              {
            //                  Beat = g.Key,
            //                  BeatID = g.FirstOrDefault().BeatID,
            //                  BeatBeatScheduleID = g.FirstOrDefault().BeatBeatScheduleID,
            //                  Schedule = (from q in g
            //                              select new BeatScheduleViewModel
            //                              {
            //                                  ScheduleName = q.Schedule.ScheduleName,
            //                                  //Start = q.Schedule.StartDate.ToShortDateString(),
            //                                  //End = q.Schedule.EndDate.ToShortDateString()
            //                              }).OrderBy(p => p.ScheduleName).ToList(),
            //                  ScheduleList = g.ToList().Select(p => p.Schedule.ScheduleName).ToList()
            //              }).ToList();


            //return Json(returnList.OrderBy(p => p.Beat), JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetSchedules()
        {
            var data = from q in db.BeatSchedules
                       select new
                       {
                           BeatScheduleId = q.BeatScheduleID,
                           ScheduleName = q.ScheduleName,
                           ScheduleID = q.BeatScheduleID,
                           //OnPatrol = q.OnPatrol,
                           //RollIn = q.RollIn,
                           //Weekday = q.Weekday,
                           //Start = q.StartDate,
                           //End = q.EndDate
                       };

            return Json(data.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBeatsWithoutSchedules()
        {
            //List<Guid> beatsWithSchedules = db.BeatBeatSchedules.Select(p => p.BeatID).Distinct().ToList();

            //var allBeats = from q in db.Beats
            //               orderby q.BeatNumber
            //               select new
            //               {
            //                   BeatNumber = q.BeatNumber,
            //                   BeatID = q.BeatID
            //               };

            //var beatsWithoutSchedule = from q in allBeats
            //                           where !beatsWithSchedules.Contains(q.BeatID)
            //                           select q;


            //return Json(beatsWithoutSchedule.ToList(), JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Save(String BeatId, List<BeatSchedule> SelectedSchedules)
        {
            bool retvalue = false;
            Guid gBeatId = Guid.Parse(BeatId);

            //first remove all schedules for this beat
            //var currentSelection = db.BeatBeatSchedules.Where(p => p.BeatID == gBeatId).ToList();
            //db.BeatBeatSchedules.RemoveRange(currentSelection);
            //await db.SaveChangesAsync();

            //foreach (var selectedSchedule in SelectedSchedules)
            //{
            //    BeatBeatSchedule beatBeatSchedule = new BeatBeatSchedule();
            //    beatBeatSchedule.BeatBeatScheduleID = Guid.NewGuid();
            //    beatBeatSchedule.BeatID = gBeatId;
            //    beatBeatSchedule.BeatScheduleID = selectedSchedule.BeatScheduleID;
            //    db.BeatBeatSchedules.Add(beatBeatSchedule);
            //}

            await db.SaveChangesAsync();
            retvalue = true;



            return Json(retvalue, JsonRequestBehavior.AllowGet);
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
