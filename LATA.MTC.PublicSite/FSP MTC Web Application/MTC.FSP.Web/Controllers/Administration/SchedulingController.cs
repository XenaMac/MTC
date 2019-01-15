using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,DataConsultant")]
    public class SchedulingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Holiday Dates
        public ActionResult GetHolidayDates()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.HolidayDates.OrderBy(p => p.Name).ToList().Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    Abbreviation = s.Abbreviation,
                    Date = s.Date,
                    Times = db.HolidaySchedules.Where(p => p.HolidayDateId == s.Id).ToList().Select(t => new
                    {
                        ScheduleName = t.ScheduleName,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime
                    }).ToList()
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveHolidayDate(ScheduleDateViewModel model)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                HolidayDate holidayDate = null;
                bool isNew = false;
                if (model.Id > 0)
                {
                    holidayDate = db.HolidayDates.Find(model.Id);
                }
                else
                {
                    holidayDate = new HolidayDate();
                    holidayDate.CreatedOn = DateTime.Now;
                    holidayDate.CreatedBy = HttpContext.User.Identity.Name;
                    isNew = true;
                }

                if (model.Times != null)
                {

                    foreach (var time in model.Times)
                    {
                        if (db.HolidaySchedules.Any(p => p.HolidayDateId == holidayDate.Id && p.ScheduleName == time.ScheduleName))
                        {
                            var holidaySchedule = db.HolidaySchedules.FirstOrDefault(p => p.HolidayDateId == holidayDate.Id && p.ScheduleName == time.ScheduleName);
                            holidaySchedule.StartTime = TimeSpan.Parse(time.StartTime);
                            holidaySchedule.EndTime = TimeSpan.Parse(time.EndTime);
                            holidaySchedule.ModifiedBy = HttpContext.User.Identity.Name;
                            holidaySchedule.ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            db.HolidaySchedules.Add(new HolidaySchedule
                            {
                                HolidayDateId = holidayDate.Id,
                                ScheduleId = Guid.NewGuid(),
                                ScheduleName = time.ScheduleName,
                                StartTime = TimeSpan.Parse(time.StartTime),
                                EndTime = TimeSpan.Parse(time.EndTime),
                                CreatedBy = HttpContext.User.Identity.Name,
                                ModifiedBy = HttpContext.User.Identity.Name,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                        }
                    }

                }


                holidayDate.Name = model.Name;
                holidayDate.Abbreviation = model.Abbreviation;
                holidayDate.Date = model.Date;

                holidayDate.ModifiedOn = DateTime.Now;
                holidayDate.ModifiedBy = HttpContext.User.Identity.Name;

                if (isNew)
                    db.HolidayDates.Add(holidayDate);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveHolidayDate(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var response = new TransactionResult();
                try
                {
                    HolidayDate holidayDate = db.HolidayDates.Find(id);
                    if (holidayDate != null)
                    {
                        var holidayScheduleIds = db.HolidaySchedules.Where(p => p.HolidayDateId == id).Select(p => p.Id).ToList();
                        db.BeatHolidaySchedules.RemoveRange(db.BeatHolidaySchedules.Where(p => holidayScheduleIds.Contains(p.HolidayScheduleId)));
                        db.HolidaySchedules.RemoveRange(db.HolidaySchedules.Where(p => p.HolidayDateId == id));
                        db.HolidayDates.Remove(holidayDate);

                        db.SaveChanges();
                    }

                    response.HasError = false;
                    response.Message = String.Empty;


                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.Message = ex.InnerException.Message;
                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }

        }

        #endregion

        #region Holiday Schedules
        public ActionResult GetHolidaySchedules()
        {
            var beatHolidaySchedules = new List<BeatHolidaySchedule>();
            var holidaySchedules = new List<HolidaySchedule>();
            var holidayDates = new List<HolidayDate>();

            using (MTCDbContext dbb = new MTCDbContext())
            {
                beatHolidaySchedules = dbb.BeatHolidaySchedules.ToList();
                holidaySchedules = dbb.HolidaySchedules.ToList();
                holidayDates = dbb.HolidayDates.ToList();
            }

            using (MTCDBEntities db = new MTCDBEntities())
            {

                var data = from bhs in beatHolidaySchedules
                           join hs in holidaySchedules on bhs.HolidayScheduleId equals hs.Id
                           join h in holidayDates on hs.HolidayDateId equals h.Id
                           join b in db.BeatDatas on bhs.BeatId equals b.ID
                           join cc in db.Contracts on b.ID equals cc.BeatId
                           join c in db.Contractors on cc.ContractorID equals c.ContractorID
                           select new
                           {
                               Id = bhs.Id,
                               BeatId = b.ID,
                               BeatNumber = b.BeatName,
                               NumberOfTrucks = bhs.NumberOfTrucks,
                               ContractorId = c.ContractorID,
                               ContractCompanyName = c.ContractCompanyName,
                               ScheduleName = hs.ScheduleName,
                               StartTime = hs.StartTime,
                               HolidayDay = h.Abbreviation,
                               HolidayDateId = h.Id,
                               HolidayScheduleId = hs.Id,
                               EndTime = hs.EndTime,
                               StartDate = cc.StartDate,
                               EndDate = cc.EndDate,
                               BackupTruckCount = cc.BackupTruckCount
                           };

                return Json(data.OrderBy(p => p.ScheduleName).ToList(), JsonRequestBehavior.AllowGet);


            }

        }

        public ActionResult GetHolidayScheduleTimes(int holidayDateId)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.HolidaySchedules.Where(p => p.HolidayDateId == holidayDateId).Select(p => new
                {
                    Id = p.Id,
                    Name = p.ScheduleName
                }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveHolidaySchedule(int? id, int holidayScheduleId, Guid beatId, int numberOfTrucks)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                BeatHolidaySchedule bhs = null;
                var isNew = false;
                if (id > 0)
                {
                    bhs = db.BeatHolidaySchedules.Find(id);
                    bhs.ModifiedBy = HttpContext.User.Identity.Name;
                    bhs.ModifiedOn = DateTime.Now;
                }
                else
                {
                    bhs = new BeatHolidaySchedule();
                    bhs.CreatedBy = HttpContext.User.Identity.Name;
                    bhs.ModifiedBy = HttpContext.User.Identity.Name;
                    bhs.CreatedOn = DateTime.Now;
                    bhs.ModifiedOn = DateTime.Now;
                    isNew = true;
                }

                bhs.BeatId = beatId;
                bhs.HolidayScheduleId = holidayScheduleId;
                bhs.NumberOfTrucks = numberOfTrucks;

                if (isNew)
                    db.BeatHolidaySchedules.Add(bhs);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveHolidaySchedule(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var response = new TransactionResult();
                try
                {
                    BeatHolidaySchedule beatHolidaySchedule = db.BeatHolidaySchedules.Find(id);
                    if (beatHolidaySchedule != null)
                    {
                        db.BeatHolidaySchedules.Remove(beatHolidaySchedule);
                        db.SaveChanges();
                    }

                    response.HasError = false;
                    response.Message = String.Empty;


                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.Message = ex.InnerException.Message;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
                              
            }

        }

        #endregion

        #region Regular Schedules

        public ActionResult GetSchedules()
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                var data = from bbs in db.BeatBeatSchedules
                           join bs in db.BeatSchedules on bbs.BeatScheduleID equals bs.BeatScheduleID
                           join b in db.BeatDatas on bbs.BeatID equals b.ID
                           join cc in db.Contracts on b.ID equals cc.BeatId
                           join c in db.Contractors on cc.ContractorID equals c.ContractorID
                           select new
                           {
                               Id = bs.BeatScheduleID,
                               BeatId = b.ID,
                               BeatNumber = b.BeatName,
                               NumberOfTrucks = bbs.NumberOfTrucks,
                               ContractCompanyName = c.ContractCompanyName,
                               ContractorId = c.ContractorID,
                               ScheduleName = bs.ScheduleName,
                               BeatScheduleId = bs.BeatScheduleID,
                               StartTime = bs.StartTime,
                               EndTime = bs.EndTime,
                               Sunday = bs.Sunday,
                               Monday = bs.Monday,
                               Tuesday = bs.Tuesday,
                               Wednesday = bs.Wednesday,
                               Thursday = bs.Thursday,
                               Friday = bs.Friday,
                               Saturday = bs.Saturday,
                               StartDate = cc.StartDate,
                               EndDate = cc.EndDate,
                               BackupTruckCount = cc.BackupTruckCount
                           };

                return Json(data.OrderBy(p => p.ScheduleName).ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveSchedule(BeatScheduleViewModel model)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                BeatSchedule bs = db.BeatSchedules.FirstOrDefault(p => p.BeatScheduleID == model.BeatScheduleId);
                var isNew = false;
                if (bs == null)
                {
                    bs = new BeatSchedule();
                    bs.BeatScheduleID = Guid.NewGuid();
                    isNew = true;
                }
                bs.ScheduleName = model.ScheduleName;
                bs.Sunday = model.Sunday;
                bs.Monday = model.Monday;
                bs.Tuesday = model.Tuesday;
                bs.Wednesday = model.Wednesday;
                bs.Thursday = model.Thursday;
                bs.Friday = model.Friday;
                bs.Saturday = model.Saturday;
                bs.StartTime = TimeSpan.Parse(model.StartTime);
                bs.EndTime = TimeSpan.Parse(model.EndTime);

                if (isNew)
                {
                    db.BeatSchedules.Add(bs);

                    BeatBeatSchedule bbs = new BeatBeatSchedule();
                    bbs.BeatBeatScheduleID = Guid.NewGuid();
                    bbs.BeatScheduleID = bs.BeatScheduleID;
                    bbs.BeatID = model.BeatId;
                    bbs.NumberOfTrucks = model.NumberOfTrucks;
                    db.BeatBeatSchedules.Add(bbs);
                }
                else
                {
                    BeatBeatSchedule bbs = db.BeatBeatSchedules.Where(b => b.BeatScheduleID == model.BeatScheduleId).Where(b => b.BeatID == model.BeatId).FirstOrDefault();
                    bbs.NumberOfTrucks = model.NumberOfTrucks;
                }

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveSchedule(Guid id)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                var response = new TransactionResult();
                try
                {
                    BeatSchedule bs = db.BeatSchedules.Find(id);
                    if (bs != null)
                    {
                        db.BeatBeatSchedules.RemoveRange(db.BeatBeatSchedules.Where(p => p.BeatScheduleID == bs.BeatScheduleID));
                        db.BeatSchedules.Remove(bs);
                        db.SaveChanges();
                    }

                    response.HasError = false;
                    response.Message = String.Empty;


                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.Message = ex.InnerException.Message;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
                              
            }

        }

        #endregion

        #region Custom Dates
        public ActionResult GetCustomDates()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.CustomDates.OrderBy(p => p.Name).ToList().Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    Abbreviation = s.Abbreviation,
                    Date = s.Date,
                    EndDate = s.EndDate,
                    Times = db.CustomSchedules.Where(p => p.CustomDateId == s.Id).ToList().Select(t => new
                    {
                        ScheduleName = t.ScheduleName,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime
                    }).ToList()
                });
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveCustomDate(ScheduleDateViewModel model)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                CustomDate CustomDate = null;
                bool isNew = false;
                if (model.Id > 0)
                {
                    CustomDate = db.CustomDates.Find(model.Id);
                }
                else
                {
                    CustomDate = new CustomDate();
                    CustomDate.CreatedOn = DateTime.Now;
                    CustomDate.CreatedBy = HttpContext.User.Identity.Name;
                    isNew = true;
                }

                if (model.Times != null)
                {

                    foreach (var time in model.Times)
                    {
                        if (db.CustomSchedules.Any(p => p.CustomDateId == CustomDate.Id && p.ScheduleName == time.ScheduleName))
                        {
                            var CustomSchedule = db.CustomSchedules.FirstOrDefault(p => p.CustomDateId == CustomDate.Id && p.ScheduleName == time.ScheduleName);
                            CustomSchedule.StartTime = TimeSpan.Parse(time.StartTime);
                            CustomSchedule.EndTime = TimeSpan.Parse(time.EndTime);
                            CustomSchedule.ModifiedBy = HttpContext.User.Identity.Name;
                            CustomSchedule.ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            db.CustomSchedules.Add(new CustomSchedule
                            {
                                CustomDateId = CustomDate.Id,
                                ScheduleId = Guid.NewGuid(),
                                ScheduleName = time.ScheduleName,
                                StartTime = TimeSpan.Parse(time.StartTime),
                                EndTime = TimeSpan.Parse(time.EndTime),
                                CreatedBy = HttpContext.User.Identity.Name,
                                ModifiedBy = HttpContext.User.Identity.Name,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now

                            });
                        }
                    }

                }


                CustomDate.Name = model.Name;
                CustomDate.Abbreviation = model.Abbreviation;
                CustomDate.Date = model.Date;
                CustomDate.EndDate = model.EndDate;

                CustomDate.ModifiedOn = DateTime.Now;
                CustomDate.ModifiedBy = HttpContext.User.Identity.Name;

                if (isNew)
                    db.CustomDates.Add(CustomDate);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveCustomDate(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {                               
                var response = new TransactionResult();
                try
                {
                    CustomDate CustomDate = db.CustomDates.Find(id);
                    if (CustomDate != null)
                    {
                        var customScheduleIds = db.CustomSchedules.Where(p => p.CustomDateId == id).Select(p => p.Id).ToList();
                        db.BeatCustomSchedules.RemoveRange(db.BeatCustomSchedules.Where(p => customScheduleIds.Contains(p.CustomScheduleId)));
                        db.CustomSchedules.RemoveRange(db.CustomSchedules.Where(p => p.CustomDateId == id));
                        db.CustomDates.Remove(CustomDate);
                        db.SaveChanges();
                    }

                    response.HasError = false;
                    response.Message = String.Empty;


                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.Message = ex.InnerException.Message;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Custom Schedules
        public ActionResult GetCustomSchedules()
        {
            var beatCustomSchedules = new List<BeatCustomSchedule>();
            var CustomSchedules = new List<CustomSchedule>();
            var CustomDates = new List<CustomDate>();

            using (MTCDbContext dbb = new MTCDbContext())
            {
                beatCustomSchedules = dbb.BeatCustomSchedules.ToList();
                CustomSchedules = dbb.CustomSchedules.ToList();
                CustomDates = dbb.CustomDates.ToList();
            }

            using (MTCDBEntities db = new MTCDBEntities())
            {

                var data = from bhs in beatCustomSchedules
                           join hs in CustomSchedules on bhs.CustomScheduleId equals hs.Id
                           join h in CustomDates on hs.CustomDateId equals h.Id
                           join b in db.BeatDatas on bhs.BeatId equals b.ID
                           join cc in db.Contracts on b.ID equals cc.BeatId
                           join c in db.Contractors on cc.ContractorID equals c.ContractorID
                           select new
                           {
                               Id = bhs.Id,
                               BeatId = b.ID,
                               BeatNumber = b.BeatName,
                               NumberOfTrucks = bhs.NumberOfTrucks,
                               ContractorId = c.ContractorID,
                               ContractCompanyName = c.ContractCompanyName,
                               ScheduleName = hs.ScheduleName,
                               StartTime = hs.StartTime,
                               //CustomDay = h.Abbreviation,
                               //CustomDateId = h.Id,
                               CustomDate = h,
                               CustomScheduleId = hs.Id,
                               EndTime = hs.EndTime,
                               StartDate = cc.StartDate,
                               EndDate = cc.EndDate,
                               BackupTruckCount = cc.BackupTruckCount
                           };

                return Json(data.OrderBy(p => p.ScheduleName).ToList(), JsonRequestBehavior.AllowGet);


            }

        }

        public ActionResult GetCustomScheduleTimes(int customDateId)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.CustomSchedules.Where(p => p.CustomDateId == customDateId).Select(p => new
                {
                    Id = p.Id,
                    Name = p.ScheduleName
                }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveCustomSchedule(int? id, int customScheduleId, Guid beatId, int numberOfTrucks)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                BeatCustomSchedule bhs = null;
                var isNew = false;
                if (id > 0)
                {
                    bhs = db.BeatCustomSchedules.Find(id);
                    bhs.ModifiedBy = HttpContext.User.Identity.Name;
                    bhs.ModifiedOn = DateTime.Now;
                }
                else
                {
                    bhs = new BeatCustomSchedule();
                    bhs.CreatedBy = HttpContext.User.Identity.Name;
                    bhs.ModifiedBy = HttpContext.User.Identity.Name;
                    bhs.CreatedOn = DateTime.Now;
                    bhs.ModifiedOn = DateTime.Now;
                    isNew = true;
                }

                bhs.BeatId = beatId;
                bhs.CustomScheduleId = customScheduleId;
                bhs.NumberOfTrucks = numberOfTrucks;

                if (isNew)
                    db.BeatCustomSchedules.Add(bhs);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveCustomSchedule(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {                               
                var response = new TransactionResult();
                try
                {
                    BeatCustomSchedule beatCustomSchedules = db.BeatCustomSchedules.Find(id);
                    if (beatCustomSchedules != null)
                    {
                        db.BeatCustomSchedules.Remove(beatCustomSchedules);              
                        db.SaveChanges();
                    }

                    response.HasError = false;
                    response.Message = String.Empty;


                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.Message = ex.InnerException.Message;
                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }

        }

        #endregion

        public ActionResult GetScheduleAvailabilityForDateAndBeat(Guid beatId, String date)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                ScheduleAvailability returnItem = new ScheduleAvailability();
                returnItem.AMAvailable = false;
                returnItem.MIDAvailable = false;
                returnItem.PMAvailable = false;

                var dtDate = Convert.ToDateTime(date);
                var todaysDay = dtDate.DayOfWeek;

                //regular and weekend schedule

                var weekDayAndweekEnds = from bs in db.BeatSchedules
                                         join bbs in db.BeatBeatSchedules on bs.BeatScheduleID equals bbs.BeatScheduleID
                                         where bbs.BeatID == beatId
                                         select new
                                         {
                                             ScheduleName = bs.ScheduleName,
                                             Sunday = bs.Sunday,
                                             Monday = bs.Monday,
                                             Tuesday = bs.Tuesday,
                                             Wednesday = bs.Wednesday,
                                             Thursday = bs.Thursday,
                                             Friday = bs.Friday
                                         };


                if (weekDayAndweekEnds.Any())
                {
                    //we have schedule for this beat
                    if (weekDayAndweekEnds.Any(p => p.ScheduleName.Contains("AM")))
                        returnItem.AMAvailable = true;
                    if (weekDayAndweekEnds.Any(p => p.ScheduleName.Contains("MID")))
                        returnItem.MIDAvailable = true;
                    if (weekDayAndweekEnds.Any(p => p.ScheduleName.Contains("PM")))
                        returnItem.PMAvailable = true;
                }



                //holiday schedule
                using (MTCDbContext dbb = new MTCDbContext())
                {
                    var holidays = from bhs in dbb.BeatHolidaySchedules
                                   join hs in dbb.HolidaySchedules on bhs.HolidayScheduleId equals hs.Id
                                   join h in dbb.HolidayDates on hs.HolidayDateId equals h.Id
                                   where bhs.BeatId == beatId && h.Date == dtDate
                                   select new
                                   {
                                       Id = bhs.Id,
                                       ScheduleName = hs.ScheduleName,
                                       StartTime = hs.StartTime,
                                       HolidayDay = h.Abbreviation,
                                       HolidayDateId = h.Id,
                                       HolidayScheduleId = hs.Id,
                                       EndTime = hs.EndTime
                                   };

                    if (holidays.Any())
                    {
                        //we have holiday schedule for this beat
                        if (holidays.Any(p => p.ScheduleName == "AM"))
                            returnItem.AMAvailable = true;
                        else
                            returnItem.AMAvailable = false;

                        if (holidays.Any(p => p.ScheduleName == "MID"))
                            returnItem.MIDAvailable = true;
                        else
                            returnItem.MIDAvailable = false;

                        if (holidays.Any(p => p.ScheduleName == "PM"))
                            returnItem.PMAvailable = true;
                        else
                            returnItem.PMAvailable = false;
                    }
                }


                //custom schedule
                using (MTCDbContext dbb = new MTCDbContext())
                {
                    var customDates = from bhs in dbb.BeatCustomSchedules
                                      join hs in dbb.CustomSchedules on bhs.CustomScheduleId equals hs.Id
                                      join h in dbb.CustomDates on hs.CustomDateId equals h.Id
                                      where bhs.BeatId == beatId && h.Date == dtDate
                                      select new
                                      {
                                          Id = bhs.Id,
                                          ScheduleName = hs.ScheduleName,
                                          StartTime = hs.StartTime,
                                          HolidayDay = h.Abbreviation,
                                          HolidayDateId = h.Id,
                                          HolidayScheduleId = hs.Id,
                                          EndTime = hs.EndTime
                                      };

                    if (customDates.Any())
                    {
                        //we have holiday schedule for this beat
                        if (customDates.Any(p => p.ScheduleName == "AM"))
                            returnItem.AMAvailable = true;
                        else
                            returnItem.AMAvailable = false;

                        if (customDates.Any(p => p.ScheduleName == "MID"))
                            returnItem.MIDAvailable = true;
                        else
                            returnItem.MIDAvailable = false;

                        if (customDates.Any(p => p.ScheduleName == "PM"))
                            returnItem.PMAvailable = true;
                        else
                            returnItem.PMAvailable = false;
                    }
                }

                return Json(returnItem, JsonRequestBehavior.AllowGet);
            }

        }
    }

    public class ScheduleAvailability
    {
        public bool AMAvailable { get; set; }
        public bool MIDAvailable { get; set; }
        public bool PMAvailable { get; set; }
    }
}