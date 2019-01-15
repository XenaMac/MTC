using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Monitoring
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor,InVehicleContractor")]
    public class DailyScheduleController : MtcBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDailySchedule()
        {
            List<DailyScheduleViewModel> returnList = new List<DailyScheduleViewModel>();

            using (MTCDBEntities db = new MTCDBEntities())
            {
                var today = DateTime.Today;
                //var today = new DateTime(2015, 12, 31, 0, 0, 0);
                
                var beatHolidaySchedules = new List<BeatHolidaySchedule>();
                var holidaySchedules = new List<HolidaySchedule>();
                var holidayDates = new List<HolidayDate>();
                var beatCustomSchedules = new List<BeatCustomSchedule>();
                var customSchedules = new List<CustomSchedule>();
                var customDates = new List<CustomDate>();

                using (MTCDbContext dbb = new MTCDbContext())
                {
                    beatHolidaySchedules = dbb.BeatHolidaySchedules.ToList();
                    holidaySchedules = dbb.HolidaySchedules.ToList();
                    holidayDates = dbb.HolidayDates.ToList();

                    beatCustomSchedules = dbb.BeatCustomSchedules.ToList();
                    customSchedules = dbb.CustomSchedules.ToList();
                    customDates = dbb.CustomDates.ToList();
                }

                //check holidays first. if we have a holiday schedule return only that.
                var todaysHolidaySchedules = (from bhs in beatHolidaySchedules
                                              join hs in holidaySchedules on bhs.HolidayScheduleId equals hs.Id
                                              join h in holidayDates on hs.HolidayDateId equals h.Id
                                              join b in db.BeatDatas on bhs.BeatId equals b.ID
                                              join cc in db.Contracts on b.ID equals cc.BeatId
                                              join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                              where h.Date.ToShortDateString() == today.ToShortDateString()
                                              select new DailyScheduleViewModel
                                              {
                                                  ScheduleType = "Holiday",
                                                  BeatNumber = b.BeatName,
                                                  ContractCompanyName = c.ContractCompanyName,
                                                  ContactName = c.ContactLastName + ", " + c.ContactFirstName,
                                                  ScheduleName = hs.ScheduleName,
                                                  StartTime = hs.StartTime,
                                                  EndTime = hs.EndTime
                                              }).ToList();

                if (todaysHolidaySchedules.Any() || holidayDates.Any(p => p.Date.ToShortDateString() == today.ToShortDateString()))
                {
                    //T.K. added 1.4.2016
                    returnList.AddRange(todaysHolidaySchedules);
                }
                else
                {
                    //custom schedules
                    var todaysCustomSchedules = (from bhs in beatCustomSchedules
                                                 join hs in customSchedules on bhs.CustomScheduleId equals hs.Id
                                                 join h in customDates on hs.CustomDateId equals h.Id
                                                 join b in db.BeatDatas on bhs.BeatId equals b.ID
                                                 join cc in db.Contracts on b.ID equals cc.BeatId
                                                 join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                                 where today >= h.Date && today <= h.EndDate
                                                 select new DailyScheduleViewModel
                                                 {
                                                     ScheduleType = "Custom",
                                                     BeatNumber = b.BeatName,
                                                     ContractCompanyName = c.ContractCompanyName,
                                                     ContactName = c.ContactLastName + ", " + c.ContactFirstName,
                                                     ScheduleName = hs.ScheduleName,
                                                     StartTime = hs.StartTime,
                                                     EndTime = hs.EndTime
                                                 }).ToList();

                    returnList.AddRange(todaysCustomSchedules);


                    var regularScheduleData = (from bbs in db.BeatBeatSchedules
                                               join bs in db.BeatSchedules on bbs.BeatScheduleID equals bs.BeatScheduleID
                                               join b in db.BeatDatas on bbs.BeatID equals b.ID
                                               join cc in db.Contracts on b.ID equals cc.BeatId
                                               join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                               select new DailyScheduleViewModel
                                               {
                                                   ScheduleType = "Regular",
                                                   BeatNumber = b.BeatName,
                                                   ContractCompanyName = c.ContractCompanyName,
                                                   ContactName = c.ContactLastName + ", " + c.ContactFirstName,
                                                   ScheduleName = bs.ScheduleName,
                                                   StartTime = bs.StartTime,
                                                   EndTime = bs.EndTime,
                                                   Sunday = bs.Sunday,
                                                   Monday = bs.Monday,
                                                   Tuesday = bs.Tuesday,
                                                   Wednesday = bs.Wednesday,
                                                   Thursday = bs.Thursday,
                                                   Friday = bs.Friday,
                                                   Saturday = bs.Saturday
                                               }).ToList();

                    //filter to today
                    var dayOfTheWeek = today.DayOfWeek;

                    if (dayOfTheWeek == DayOfWeek.Monday)
                        regularScheduleData = regularScheduleData.Where(p => p.Monday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Tuesday)
                        regularScheduleData = regularScheduleData.Where(p => p.Tuesday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Wednesday)
                        regularScheduleData = regularScheduleData.Where(p => p.Wednesday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Thursday)
                        regularScheduleData = regularScheduleData.Where(p => p.Thursday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Friday)
                        regularScheduleData = regularScheduleData.Where(p => p.Friday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Saturday)
                        regularScheduleData = regularScheduleData.Where(p => p.Saturday == true).ToList();
                    else if (dayOfTheWeek == DayOfWeek.Sunday)
                        regularScheduleData = regularScheduleData.Where(p => p.Sunday == true).ToList();


                    foreach (var regular in regularScheduleData)
                    {
                        if (!returnList.Any(p => p.BeatNumber == regular.BeatNumber &&
                                                 p.ContractCompanyName == regular.ContractCompanyName &&
                                                 p.ScheduleType == "Custom"))
                        {
                            //only add this regular schedule, if a custom schedule does NOT exist for it
                            returnList.Add(regular);
                        }
                    }


                }

                return Json(returnList.ToList(), JsonRequestBehavior.AllowGet);

            }
        }
    }
}