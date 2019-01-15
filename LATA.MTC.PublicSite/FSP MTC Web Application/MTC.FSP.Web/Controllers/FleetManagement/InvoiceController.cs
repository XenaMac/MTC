using MTC.FSP.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MTC.FSP.Web.Models;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Data.Entity.Validation;
using MTC.FSP.Web.Controllers.Operations;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    public class InvoiceController : Controller
    {
        MTCDbContext dbc = new MTCDbContext();
        MTCDBEntities db = new MTCDBEntities();
        BackupTrucksController BUC = new BackupTrucksController();
        Random random = new Random();

        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        //Get summary
        public ActionResult fillMonthDropDown()
        {
            string[] monthDropDown = new string[3];
            monthDropDown[2] = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

            for (int i = 0; i <= 2; i++)
            {
                monthDropDown[i] = DateTime.Now.AddMonths(-i).ToString("MMMM", CultureInfo.InvariantCulture);
            }

            return Json(monthDropDown.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Get: GetBeatContractorInfo
        public ActionResult GetBeatContractorInfo(string beatid, int month, string contractorid)
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);
            String BeatNumb = db.BeatDatas.Where(b => b.ID == BID).Select(b => b.BeatName).FirstOrDefault();
            String MTCAddress = dbc.MTCApplicationSettings.Where(s => s.Name == "MTCAddress").Select(c => c.Value).FirstOrDefault();

            var CInfo = (from contract in db.Contracts
                         join contractor in db.Contractors on contract.ContractorID equals contractor.ContractorID
                         where contract.BeatId == BID 
                         && contract.ContractorID == CID
                         select new
                         {
                             contractnumber = contract.AgreementNumber ,
                             contractexp = contract.EndDate.ToString(),
                             contractbeg = contract.StartDate.ToString(),
                             companyname = contractor.ContractCompanyName,
                             address1 = contractor.Address.ToString(),
                             city = contractor.City,
                             state = contractor.State,
                             zip = contractor.Zip,
                             contact = contractor.ContactFirstName + " " + contractor.ContactLastName,
                             email = contractor.Email,
                             phone = contractor.OfficeTelephone,
                             invoiceNumber = contractor.ContractCompanyName.Trim().Replace(" ", "").Substring(0, 5) + "_" + BeatNumb + "_" + month.ToString() + DateTime.Now.Year,
                             MTCAddress = MTCAddress
                         }).FirstOrDefault();

            return Json(CInfo, JsonRequestBehavior.AllowGet);

        }

        //Get: Geting Invoiced Holidays  THIS IS OLD NO USING
        public ActionResult GetServiceSummary(string beatid, int month, string contractorid)
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);
            int startDays = 1;
            int endDays = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);
            InvoiceServiceData ISD = new InvoiceServiceData();
            int workedOnHoliday = 0;
            decimal regularHours = 0;
            int MinusHolShifts = 0;

            Contract contract = db.Contracts.Where(c => c.ContractorID == CID).Where(c => c.BeatId == BID).FirstOrDefault();
            
            //Is end date in middle of month
            if (contract.EndDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && contract.EndDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
            {
                startDays = 1;
                endDays = contract.EndDate.Day;
            }

            if (contract.StartDate > new DateTime(DateTime.Now.Year, month, 1) && contract.StartDate < new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month)))
            { 
                startDays = contract.StartDate.Day;
                endDays = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);
            }

            for (int i = startDays; i <= endDays; i++)
            {
                DateTime dt = new DateTime((int)DateTime.Now.Year, (int)month, i);

                #region 2: Get Holiday dates and schedules

                //Holiday dates and schedule
                var beatHolidaySchedules = new List<BeatHolidaySchedule>();
                var holidaySchedules = new List<HolidaySchedule>();
                var holidayDates = new List<HolidayDate>();

                //Set Holiday dates and schedules
                beatHolidaySchedules = dbc.BeatHolidaySchedules.ToList();
                holidaySchedules = dbc.HolidaySchedules.ToList();
                holidayDates = dbc.HolidayDates.ToList();


                //find holiday dates and schedules worked
                HolidayDate Holidate = (from hd in dbc.HolidayDates
                                         where hd.Date == dt
                                         select hd).FirstOrDefault();

                //Does holiday exist?
                if (Holidate != null)
                {
                    //Add holidays
                    ISD.HolDays --;

                    //if worked, add holidays, shifts and hours
                    var HSWorked = (from bhs in beatHolidaySchedules
                                    join hs in holidaySchedules on bhs.HolidayScheduleId equals hs.Id
                                    join h in holidayDates on hs.HolidayDateId equals h.Id
                                    join b in db.BeatDatas on bhs.BeatId equals b.ID
                                    join cc in db.Contracts on b.ID equals cc.BeatId
                                    join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                    where cc.ContractorID == CID
                                    && b.ID == BID
                                    && h.Date == dt
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
                                        HolidayDayDate = h.Date,
                                        HolidayScheduleId = hs.Id,
                                        EndTime = hs.EndTime,
                                        StartDate = cc.StartDate,
                                        EndDate = cc.EndDate,
                                        BackupTruckCount = cc.BackupTruckCount
                                    });

                    if (HSWorked.Count() != 0)
                    {
                        //set workedOnHoliday
                        workedOnHoliday ++;

                        //Add Shifts
                        foreach (var sw in HSWorked)
                        {
                            ISD.HolShifts++;
                            TimeSpan hours = sw.EndTime - sw.StartTime;
                            ISD.HolContractHours += Convert.ToDecimal(hours.TotalHours * sw.NumberOfTrucks);
                            ISD.HolOnPatrolHours += Convert.ToDecimal(hours.TotalHours * sw.NumberOfTrucks);
                        }

                        //Subtract normal day's shifts from total shifts and Hours
                        var Schedules = from bbs in db.BeatBeatSchedules
                                        join bs in db.BeatSchedules on bbs.BeatScheduleID equals bs.BeatScheduleID
                                        join b in db.BeatDatas on bbs.BeatID equals b.ID
                                        join cc in db.Contracts on b.ID equals cc.BeatId
                                        join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                        where b.ID == BID &&
                                        c.ContractorID == CID 
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

                        foreach (var sched in Schedules)
                        {
                            switch (Holidate.Date.DayOfWeek.ToString())
                            {
                                case "Sunday":
                                    if(sched.Sunday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Monday":
                                    if (sched.Monday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Tuesday":
                                    if (sched.Tuesday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Wednesday":
                                    if (sched.Wednesday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Thursday":
                                    if (sched.Thursday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Friday":
                                    if (sched.Friday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                                case "Saturday":
                                    if (sched.Saturday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        MinusHolShifts++;
                                        regularHours += Convert.ToDecimal(hours.TotalHours * sched.NumberOfTrucks);
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //Subtract day's shifts from total shifts and Hours
                        var Schedules = from bbs in db.BeatBeatSchedules
                                        join bs in db.BeatSchedules on bbs.BeatScheduleID equals bs.BeatScheduleID
                                        join b in db.BeatDatas on bbs.BeatID equals b.ID
                                        join cc in db.Contracts on b.ID equals cc.BeatId
                                        join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                        where b.ID == BID &&
                                        c.ContractorID == CID
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
                       
                        foreach (var sched in Schedules)
                        {
                            switch (Holidate.Date.DayOfWeek.ToString())
                            {
                                case "Sunday":
                                    if (sched.Sunday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                       
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Monday":
                                    if (sched.Monday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);

                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Tuesday":
                                    if (sched.Tuesday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Wednesday":
                                    if (sched.Wednesday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Thursday":
                                    if (sched.Thursday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Friday":
                                    if (sched.Friday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                                case "Saturday":
                                    if (sched.Saturday == true)
                                    {
                                        TimeSpan hours = (TimeSpan)(sched.EndTime - sched.StartTime);
                                        
                                        //Keep this for if they worked to subtract from regular hours
                                        MinusHolShifts++;
                                        regularHours += hours.Hours * sched.NumberOfTrucks;
                                    }
                                    break;
                            }
                        }
                    }
                }

                #endregion

                #region 3: Get Normal Week and Weekend dates and schedules

                bool sunday = false;
                bool saturday = false;
                bool weekday = false;
                bool custom = false;

                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count > 0)
                {
                    //Sum up all contract hours and onPatrolHours
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        if (result.BScheduleType == "CUS")
                        {
                            //custom Totals
                            custom = true;
                            ISD.CustomShifts++;
                            ISD.CustomContractHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                            ISD.CustomOnPatrolHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                        } else if (result.BScheduleType == "SUN")
                        {
                            //Sun Totals
                            sunday = true;
                            ISD.SunShifts++;
                            ISD.SunContractHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                            ISD.SunOnPatrolHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                        }
                        else if (result.BScheduleType == "SAT")
                        {
                            //Sat Totals
                            saturday = true;
                            ISD.SatShifts++;
                            ISD.SatContractHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                            ISD.SatOnPatrolHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                        }
                        else
                        {
                            //Reg Schedule Totals
                            weekday = true;
                            ISD.WeekShifts++;
                            ISD.WeekContractHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                            ISD.WeekOnPatrolHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                        }
                    }

                    //Total Days of Schedules
                    if (custom)
                    {
                        ISD.CustomDays++;
                    } 
                    else if (sunday)
                    {
                        ISD.SunDays++;
                    }
                    else if (saturday)
                    {
                        ISD.SatDays++;
                    }
                    else if (weekday)
                    {
                        ISD.WeekDays++;
                    }

                    ////Get totals
                    //ISD.TotalDays = ISD.CustomDays + ISD.HolDays + ISD.SunDays + ISD.SatDays + ISD.WeekDays;
                    //ISD.TotalContractHours = ISD.CustomContractHours + ISD.HolContractHours + ISD.SunContractHours + ISD.SatContractHours + ISD.WeekContractHours;
                    //ISD.TotalShifts = ISD.CustomShifts + ISD.HolShifts + ISD.SunShifts + ISD.SatShifts + ISD.WeekShifts;

                    //ISD.CustomOnPatrolHours = ISD.CustomContractHours;
                    //ISD.HolOnPatrolHours = ISD.HolContractHours;
                    //ISD.SunOnPatrolHours = ISD.SunContractHours;
                    //ISD.SatOnPatrolHours = ISD.SatContractHours;
                    //ISD.WeekOnPatrolHours = ISD.WeekContractHours;
                    //ISD.TotalOnPatrolHours = ISD.TotalContractHours;

                    //Nullify bool days
                    sunday = false;
                    saturday = false;
                    weekday = false;
                }

                #endregion
            }
             
            //Totalling Everything don't forget to add or subtract holiday hours if worked
            if (workedOnHoliday > 0)
            {
                ISD.TotalDays = (ISD.WeekDays + ISD.SatDays + ISD.SunDays + ISD.HolDays) + workedOnHoliday + ISD.CustomDays;
                ISD.TotalShifts = (ISD.WeekShifts + ISD.SatShifts + ISD.SunShifts - MinusHolShifts) + ISD.HolShifts + ISD.CustomShifts;
                ISD.TotalContractHours = (ISD.WeekContractHours + ISD.SatContractHours + ISD.SunContractHours - regularHours) + ISD.HolContractHours + ISD.CustomContractHours;
                ISD.TotalOnPatrolHours = (ISD.WeekContractHours + ISD.SatContractHours + ISD.SunContractHours - regularHours) + ISD.HolContractHours + ISD.CustomContractHours;
            }
            else
            {
                ISD.TotalDays = ISD.WeekDays + ISD.SatDays + ISD.SunDays + ISD.HolDays + ISD.CustomDays;
                ISD.TotalShifts = ISD.WeekShifts + ISD.SatShifts + ISD.SunDays + ISD.HolShifts + ISD.CustomShifts;
                ISD.TotalContractHours = ISD.WeekContractHours + ISD.SatContractHours + ISD.SunContractHours + ISD.HolContractHours + ISD.CustomContractHours;
                ISD.TotalOnPatrolHours = ISD.WeekOnPatrolHours + ISD.SatOnPatrolHours + ISD.SunOnPatrolHours + ISD.HolOnPatrolHours + ISD.CustomOnPatrolHours;
            }

            //added this due to logic error
            ISD.HolShifts = MinusHolShifts;
            ISD.HolContractHours = regularHours * -1;
            ISD.HolOnPatrolHours = regularHours * -1;

            return Json(ISD, JsonRequestBehavior.AllowGet);
        }

        //Get: SetServiceSummary2
        public ActionResult GetServiceSummary2(string beatid, int month, string contractorid)
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);
            InvoiceSummaryData ISD = new InvoiceSummaryData();
            int startDay = 1;
            int endDay = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);

            Contract contract = db.Contracts.Where(c => c.ContractorID == CID).Where(c => c.BeatId == BID).FirstOrDefault();

            //Is end date in middle of month
            if (contract.EndDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && contract.EndDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
            {
                startDay = 1;
                endDay = contract.EndDate.Day;
            }

            if (contract.StartDate > new DateTime(DateTime.Now.Year, month, 1) && contract.StartDate < new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month)))
            {
                startDay = contract.StartDate.Day;
                endDay = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);
            }

            //Get Monthly Days
            ISD.WeekDaysInMonth = WeekdaysInMonth(startDay, endDay, month);
            ISD.SaturDaysInMonth = SaturdaysInMonth(startDay, endDay, month);
            ISD.SunDaysInMonth = SundaysInMonth(startDay, endDay, month);
            ISD.HolidaysInMonth = HolidaysInMonth(startDay, endDay, month);
            ISD.CustomDaysInMonth = CustomDaysInMonth(startDay, endDay, month, BID);

            //Get workdays Worked in Month
            ISD.WeekDaysWorkedInMonth = WeekdaysWorkedInMonth(startDay, endDay, month, BID, ISD);
            ISD.SaturDaysWorkedInMonth = SaturdaysWorkedInMonth(startDay, endDay, month, BID, ISD);
            ISD.SunDaysWorkedInMonth = SundaysWorkedInMonth(startDay, endDay, month, BID, ISD);
            ISD.HolidaysWorkedInMonth = HolidaysWorkedInMonth(BID, CID, ISD);
            ISD.CustomDaysWorkedInMonth = CustomDaysWorkedInMonth(BID, ISD);

            //Get Workdays Shifts and Hours in Month
            ISD = GetWeekdayShiftsAndHours(BID, ISD);
            ISD = GetSaturdayShiftsAndHours(BID, ISD);
            ISD = GetSundayShiftsAndHours(BID, ISD);
            ISD = GetHolidayShiftsAndHours(month, BID, CID, ISD);
            ISD = GetCustomShiftsAndHours(BID, ISD);

            //TOTALS
            ISD.TotalWeekDaysInMonth = ISD.WeekDaysInMonth.Count();
            ISD.TotalWeekDaysWorkedInMonth = ISD.WeekDaysWorkedInMonth.Count();
            ISD.TotalSaturDaysInMonth = ISD.SaturDaysInMonth.Count();
            ISD.TotalSaturDaysWorkedInMonth = ISD.SaturDaysWorkedInMonth.Count();
            ISD.TotalSunDaysInMonth = ISD.SunDaysInMonth.Count();
            ISD.TotalSunDaysWorkedInMonth = ISD.SunDaysWorkedInMonth.Count();
            ISD.TotalHolidaysInMonth = ISD.HolidaysInMonth.Count();
            ISD.TotalHolidaysWorkedInMonth = ISD.HolidaysWorkedInMonth.Count();
            ISD.TotalCustomDaysInMonth = ISD.CustomDaysInMonth.Count();
            ISD.TotalCustomDaysWorkedInMonth = ISD.CustomDaysWorkedInMonth.Count();
            ISD.TotalWeekdaysWorkedShifts = ISD.WeekDayShiftsInMonth;
            ISD.TotalWeekDaysWorkedHours = ISD.WeekDayHoursInMonth;

            ISD.TotalDays = ISD.TotalWeekDaysWorkedInMonth + ISD.TotalSaturDaysWorkedInMonth + ISD.TotalSunDaysWorkedInMonth + ISD.TotalHolidaysWorkedInMonth + ISD.TotalCustomDaysWorkedInMonth;
            ISD.TotalShifts = ISD.TotalWeekdaysWorkedShifts + ISD.TotalSaturDaysWorkedShifts + ISD.TotalSunDaysWorkedShifts + ISD.TotalHolidaysWorkedShifts + ISD.TotalCustomDaysWorkedShifts;
            ISD.TotalHoursWorked = ISD.TotalWeekDaysWorkedHours + ISD.TotalSaturDaysWorkedHours + ISD.TotalSunDaysWorkedHours + ISD.TotalHolidaysWorkedHours + ISD.TotalCustomDaysWorkedHours;

            return Json(ISD, JsonRequestBehavior.AllowGet);
        }

        #region Invoice Month Days
        //Get: WeekdaysInMonth
        public List<DateTime> WeekdaysInMonth(int startday, int endDay, int month)
        {
            List<DateTime> WeekdaysInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime();

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                else
                {
                    dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                }
                #endregion

                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    WeekdaysInMonth.Add(dt);
                }
            }

            return WeekdaysInMonth;
        }

        //Get: SaturdaysInMonth
        public List<DateTime> SaturdaysInMonth(int startday, int endDay, int month) 
        {
            List<DateTime> SaturdaysInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime();

                int nowmonth = DateTime.Now.Month;

                if(month == 11 || month == 12)
                {
                    if(nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                else
                {
                    dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                }
                #endregion

                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    SaturdaysInMonth.Add(dt);
                }
            }

            return SaturdaysInMonth;
        }

        //Get: SundaysInMonth
        public List<DateTime> SundaysInMonth(int startday, int endDay, int month)
        {
            List<DateTime> SundaysInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime();

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                else
                {
                    dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                }
                #endregion

                if (dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    SundaysInMonth.Add(dt);
                }
            }

            return SundaysInMonth;
        }

        //Get: HolidaysInMonth
        public List<DateTime> HolidaysInMonth(int startday, int endDay, int month)
        {
            List<DateTime> HolidaysInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime();

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                else
                {
                    dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                }
                #endregion

                //Holiday dates and schedule
                var beatHolidaySchedules = new List<BeatHolidaySchedule>();
                var holidaySchedules = new List<HolidaySchedule>();
                var holidayDates = new List<HolidayDate>();

                //Set Holiday dates and schedules
                beatHolidaySchedules = dbc.BeatHolidaySchedules.ToList();
                holidaySchedules = dbc.HolidaySchedules.ToList();
                holidayDates = dbc.HolidayDates.ToList();


                //find holiday dates and schedules worked
                HolidayDate Holidate = (from hd in dbc.HolidayDates
                                        where hd.Date == dt
                                        select hd).FirstOrDefault();

                //Does holiday exist?
                if (Holidate != null)
                {
                    HolidaysInMonth.Add(dt);
                }
            }

            return HolidaysInMonth;
        }

        //Get: CustomDaysInMonth
        public List<DateTime> CustomDaysInMonth(int startday, int endDay, int month, Guid BID) 
        {
            List<DateTime> CustomDaysInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime();

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                else
                {
                    dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                }
                #endregion

                CustomDate cd = (from c in dbc.CustomDates
                                 where c.Date <= dt &&
                                 c.EndDate >= dt
                                 select c).FirstOrDefault();

                if(cd != null)
                {
                    CustomDaysInMonth.Add(dt);
                }
            }

            return CustomDaysInMonth;
        }
        #endregion

        #region Monthly Days Worked
        //Get: WeekdaysWorkedInMonth
        public List<DateTime> WeekdaysWorkedInMonth(int startday, int endDay, int month, Guid BID, InvoiceSummaryData ISD)
        {
            List<DateTime> WeekdaysWorkedInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime((int)DateTime.Now.Year, (int)month, i);

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                #endregion

                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count != 0)
                {
                    if (!ISD.HolidaysInMonth.Contains(dt) && !ISD.SaturDaysInMonth.Contains(dt) && !ISD.SunDaysInMonth.Contains(dt))
                    { 
                        WeekdaysWorkedInMonth.Add(dt);
                    }
                }           
            }

            return WeekdaysWorkedInMonth;
        }

        //Get: SaturdaysWorkedInMonth
        public List<DateTime> SaturdaysWorkedInMonth(int startday, int endDay, int month, Guid BID, InvoiceSummaryData ISD)
        {
            List<DateTime> SaturdaysWorkedInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime((int)DateTime.Now.Year, (int)month, i);

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                #endregion

                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count != 0)
                {
                    if (ISD.SaturDaysInMonth.Contains(dt))
                    {
                        SaturdaysWorkedInMonth.Add(dt);
                    }
                }
            }

            return SaturdaysWorkedInMonth;
        }

        //Get: SundaysWorkedInMonth
        public List<DateTime> SundaysWorkedInMonth(int startday, int endDay, int month, Guid BID, InvoiceSummaryData ISD)
        {
            List<DateTime> SundaysWorkedInMonth = new List<DateTime>();

            for (int i = startday; i <= endDay; i++)
            {
                #region if nov or dec need to subtract year
                DateTime dt = new DateTime((int)DateTime.Now.Year, (int)month, i);

                int nowmonth = DateTime.Now.Month;

                if (month == 11 || month == 12)
                {
                    if (nowmonth == 1 || nowmonth == 2)
                    {
                        dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, i);

                    }
                }
                #endregion

                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count != 0)
                {
                    if (ISD.SunDaysInMonth.Contains(dt))
                    {
                        SundaysWorkedInMonth.Add(dt);
                    }
                }
            }

            return SundaysWorkedInMonth;
        }

        //Get: HolidaysWorkedInMonth
        public List<DateTime> HolidaysWorkedInMonth(Guid BID, Guid CID, InvoiceSummaryData ISD)
        {
            List<DateTime> HolidaysWorkedInMonth = new List<DateTime>();
            
            //Holiday dates and schedule
            var beatHolidaySchedules = new List<BeatHolidaySchedule>();
            var holidaySchedules = new List<HolidaySchedule>();
            var holidayDates = new List<HolidayDate>();

            //Set Holiday dates and schedules
            beatHolidaySchedules = dbc.BeatHolidaySchedules.ToList();
            holidaySchedules = dbc.HolidaySchedules.ToList();
            holidayDates = dbc.HolidayDates.ToList();

            foreach (DateTime holiday in ISD.HolidaysInMonth)
            {
                var HSWorked = (from bhs in beatHolidaySchedules
                                join hs in holidaySchedules on bhs.HolidayScheduleId equals hs.Id
                                join h in holidayDates on hs.HolidayDateId equals h.Id
                                join b in db.BeatDatas on bhs.BeatId equals b.ID
                                join cc in db.Contracts on b.ID equals cc.BeatId
                                join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                where cc.ContractorID == CID
                                && b.ID == BID
                                && h.Date == holiday
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
                                    HolidayDayDate = h.Date,
                                    HolidayScheduleId = hs.Id,
                                    EndTime = hs.EndTime,
                                    StartDate = cc.StartDate,
                                    EndDate = cc.EndDate,
                                    BackupTruckCount = cc.BackupTruckCount
                                });

                if (HSWorked.Count() != 0)
                {
                    HolidaysWorkedInMonth.Add(holiday);
                }
            }

            return HolidaysWorkedInMonth;
        }

        //Get: CustomDaysWorkedInMonth
        public List<DateTime> CustomDaysWorkedInMonth(Guid BID, InvoiceSummaryData ISD)
        {
            List<DateTime> CustomDaysWorkedInMonth = new List<DateTime>();
          
            foreach (DateTime date in ISD.CustomDaysInMonth)
            {
                bool isCustom = false;

                var schedules = (db.SchedulesSearch(date, BID)).ToList();

                if (schedules.Count > 0)
                {
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        if (result.BScheduleType == "CUS")
                        {
                            isCustom = true;
                        }
                    }

                    //add to customer dates
                    if(isCustom)
                    {
                        CustomDaysWorkedInMonth.Add(date);
                    }
                }
            }

            return CustomDaysWorkedInMonth;
        }
        #endregion

        #region GetShiftsAndHours

        public InvoiceSummaryData GetWeekdayShiftsAndHours(Guid BID, InvoiceSummaryData ISD)
        {
            foreach(DateTime dt in ISD.WeekDaysWorkedInMonth)
            {
                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count > 0)
                {
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        ISD.WeekDayShiftsInMonth++;
                        ISD.WeekDayHoursInMonth += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                    }
                }
            }

            return ISD;
        }

        public InvoiceSummaryData GetSaturdayShiftsAndHours(Guid BID, InvoiceSummaryData ISD)
        {
            foreach (DateTime dt in ISD.SaturDaysWorkedInMonth)
            {
                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count > 0)
                {
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        ISD.TotalSaturDaysWorkedShifts++;
                        ISD.TotalSaturDaysWorkedHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                    }
                }
            }

            return ISD;
        }

        public InvoiceSummaryData GetSundayShiftsAndHours(Guid BID, InvoiceSummaryData ISD)
        {
            foreach (DateTime dt in ISD.SunDaysWorkedInMonth)
            {
                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count > 0)
                {
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        ISD.TotalSunDaysWorkedShifts++;
                        ISD.TotalSunDaysWorkedHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                    }
                }
            }

            return ISD;
        }

        public InvoiceSummaryData GetHolidayShiftsAndHours(int month, Guid BID, Guid CID, InvoiceSummaryData ISD)
        {
            //Holiday dates and schedule
            var beatHolidaySchedules = new List<BeatHolidaySchedule>();
            var holidaySchedules = new List<HolidaySchedule>();
            var holidayDates = new List<HolidayDate>();

            //Set Holiday dates and schedules
            beatHolidaySchedules = dbc.BeatHolidaySchedules.ToList();
            holidaySchedules = dbc.HolidaySchedules.ToList();
            holidayDates = dbc.HolidayDates.ToList();

            foreach (DateTime dt in ISD.HolidaysWorkedInMonth)
            {
                var HSWorked = (from bhs in beatHolidaySchedules
                                join hs in holidaySchedules on bhs.HolidayScheduleId equals hs.Id
                                join h in holidayDates on hs.HolidayDateId equals h.Id
                                join b in db.BeatDatas on bhs.BeatId equals b.ID
                                join cc in db.Contracts on b.ID equals cc.BeatId
                                join c in db.Contractors on cc.ContractorID equals c.ContractorID
                                where cc.ContractorID == CID
                                && b.ID == BID
                                && h.Date == dt
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
                                    HolidayDayDate = h.Date,
                                    HolidayScheduleId = hs.Id,
                                    EndTime = hs.EndTime,
                                    StartDate = cc.StartDate,
                                    EndDate = cc.EndDate,
                                    BackupTruckCount = cc.BackupTruckCount
                                });

                if (HSWorked.Count() != 0)
                {
                    foreach (var sw in HSWorked)
                    {
                        ISD.TotalHolidaysWorkedShifts++;
                        TimeSpan hours = sw.EndTime - sw.StartTime;
                        ISD.TotalHolidaysWorkedHours += Convert.ToDecimal(hours.TotalHours * sw.NumberOfTrucks);
                    }
                }
            }

            return ISD;
        }

        public InvoiceSummaryData GetCustomShiftsAndHours(Guid BID, InvoiceSummaryData ISD)
        {
            foreach (DateTime dt in ISD.CustomDaysWorkedInMonth)
            {
                var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                if (schedules.Count > 0)
                {
                    foreach (SchedulesSearch_Result result in schedules)
                    {
                        if (result.BScheduleType == "CUS")
                        {
                            ISD.TotalCustomDaysWorkedShifts++;
                            ISD.TotalCustomDaysWorkedHours += (result.TotalShiftHours / 60) * result.NumberOfTrucks;
                        }
                    }
                }
            }

            return ISD;
        }

        #endregion

        //Get: Get Base Rate
        public ActionResult GetBaseRate(string beatid)
        {
            Guid BID = new Guid(beatid);
            InvoiceSummaryViewModel ISVM = new InvoiceSummaryViewModel();

            #region Get fuel and payrate
            var pNumb = (from setting in dbc.MTCApplicationSettings
                         where setting.Name == "FuelRate"
                         select setting.Value).First();

            var pr = (from r in db.MTCRateTables
                      where r.BeatID == BID
                      select r).SingleOrDefault();

            switch (pNumb)
            {
                case "p100":
                    ISVM.PayRate = (Decimal)pr.p100;
                    ISVM.FuelRate = 1.0M;
                    break;
                case "p150":
                    ISVM.PayRate = (Decimal)pr.p150;
                    ISVM.FuelRate = 1.50M;
                    break;
                case "p200":
                    ISVM.PayRate = (Decimal)pr.p200;
                    ISVM.FuelRate = 2.00M;
                    break;
                case "p250":
                    ISVM.PayRate = (Decimal)pr.p250;
                    ISVM.FuelRate = 2.50M;
                    break;
                case "p300":
                    ISVM.PayRate = (Decimal)pr.p300;
                    ISVM.FuelRate = 3.00M;
                    break;
                case "p350":
                    ISVM.PayRate = (Decimal)pr.p350;
                    ISVM.FuelRate = 3.50M;
                    break;
                case "p400":
                    ISVM.PayRate = (Decimal)pr.p400;
                    ISVM.FuelRate = 4.00M;
                    break;
                case "p450":
                    ISVM.PayRate = (Decimal)pr.p450;
                    ISVM.FuelRate = 4.50M;
                    break;
                case "p500":
                    ISVM.PayRate = (Decimal)pr.p500;
                    ISVM.FuelRate = 5.00M;
                    break;
                case "p550":
                    ISVM.PayRate = (Decimal)pr.p550;
                    ISVM.FuelRate = 5.50M;
                    break;
                case "p600":
                    ISVM.PayRate = (Decimal)pr.p600;
                    ISVM.FuelRate = 6.00M;
                    break;
                case "p650":
                    ISVM.PayRate = (Decimal)pr.p650;
                    ISVM.FuelRate = 6.50M;
                    break;
                case "p700":
                    ISVM.PayRate = (Decimal)pr.p700;
                    ISVM.FuelRate = 7.00M;
                    break;
                case "p750":
                    ISVM.PayRate = (Decimal)pr.p750;
                    ISVM.FuelRate = 7.50M;
                    break;
                case "p800":
                    ISVM.PayRate = (Decimal)pr.p800;
                    ISVM.FuelRate = 8.00M;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            #endregion

            return Json(ISVM, JsonRequestBehavior.AllowGet);
        }

        //Get: Get Invoice Additions
        public ActionResult GetInvoiceAdditions(string beatid, int month, string contractorid)
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);

            #region if nov or dec need to subtract year
            DateTime dt = new DateTime();

            int nowmonth = DateTime.Now.Month;

            if (month == 11 || month == 12)
            {
                if (nowmonth == 1 || nowmonth == 2)
                {
                    dt = new DateTime((int)DateTime.Now.Year - 1, (int)month, 5);

                }
            }
            else
            {
                dt = new DateTime((int)DateTime.Now.Year, (int)month, 5);
            }
            #endregion 

            InvoiceSummaryViewModel ISVM = GetBaseRateMethod(beatid);
            List<InvoiceAdditions> Additions = new List<InvoiceAdditions>();

            #region get overtime
            var overtime = (from o in db.OvertimeActivities
                            join b in db.BeatDatas on o.Beat equals b.BeatName
                            where b.ID == BID &&  
                            o.Confirmed == true &&
                            o.timeStamp.Month == month &&
                            o.timeStamp.Year == dt.Year
                            select new InvoiceAdditions
                            {
                                category = "Overtime",
                                date = o.timeStamp.ToString(),
                                description = o.Shift + ", " + o.CallSign + ", " + o.OverTimeCode,
                                id = Guid.NewGuid(),
                                TimeAdded = o.BlocksApproved ?? 1,
                                Rate = ISVM.PayRate,
                                shiftDay = o.Shift
                            }).ToList();

            foreach (InvoiceAdditions a in overtime)
            {
                if (a.shiftDay.Contains("Weekday"))
                {
                    a.shiftDay = "WEEKDAY";
                }
                else if (a.shiftDay.Contains("Saturday"))
                {
                    a.shiftDay = "SAT";
                }
                else if (a.shiftDay.Contains("Sunday"))
                {
                    a.shiftDay = "SUN";
                }
                else if (a.shiftDay.ToUpper().Contains("CUS"))
                {
                    a.shiftDay = "CUS";
                }
                else if (a.shiftDay.ToUpper().Contains("HOL"))
                {
                    a.shiftDay = "HOL";
                }
                decimal rate = a.Rate / 4;
                a.Cost = rate * a.TimeAdded;

                Additions.Add(a);
            }
            #endregion
              
            #region get backup pay
            var backups = (from b in dbc.BackupRequests
                           join bursad in dbc.BackupRequestShiftsAndDates on b.Id equals bursad.BackupRequestId
                               where b.SelectedBackupContractorId == CID &&
                               b.BeatId == BID &&
                               b.IsCancelled == false &&
                               bursad.BackupDate.Month == month
                               select new
                                {
                                    RequestNumber = b.RequestNumber,
                                    BeatID = b.BeatId,
                                    //BeatNumber = b. db.Beats.FirstOrDefault(p => b.BeatId == p.BeatID).BeatNumber,
                                    RequestComments = b.Comments,
                                    ResponseComments = dbc.BackupResponses.Where(p => p.BackupRequestId == b.Id ).Select(p => p.Comments).FirstOrDefault(),
                                    IsResolved = dbc.BackupResponses.Any(p => p.BackupRequestId == b.Id && (p.BackupResponseStatus == BackupResponseStatus.Accepted)),
                                    BackupRequestShiftsAndDates = dbc.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == b.Id).Where(p => p.BackupDate.Month == month).Select(s => new
                                        {
                                            Id = s.Id,
                                            BackupRequestId = s.BackupRequestId,
                                            BackupDate = s.BackupDate,
                                            AMRequested = s.AMRequested,
                                            AMSatisfied = s.AMSatisfied,
                                            MIDRequested = s.MIDRequested,
                                            MIDSatisfied = s.MIDSatisfied,
                                            PMRequested = s.PMRequested,
                                            PMSatisfied = s.PMSatisfied,
                                        }).FirstOrDefault()
                                }).ToList();

            foreach(var bu in backups)
            {
                //1. If IsResolved = true then ...
                //2. If AMSatisfied get shift hours
                //3. If MIDSatisfied get shift hours
                //4. If PMSatisfied get shift hours
                //5. Create new InvoiceAddition and add to list
                
                if(bu.IsResolved == true)
                {
                    InvoiceAdditions addon = new InvoiceAdditions();
                    addon.id = Guid.NewGuid();
                    addon.category = "Backup Resolved";
                    addon.date = bu.BackupRequestShiftsAndDates.BackupDate.ToShortDateString();
                    addon.description = bu.RequestNumber + " | " + (bu.RequestComments ?? "No RqComments") + " | " + (bu.ResponseComments ?? "No RspComments");
                    addon.Rate = ISVM.PayRate;
                    addon.TimeAdded = 0;
                    var schedules = (db.SchedulesSearch(bu.BackupRequestShiftsAndDates.BackupDate, bu.BeatID)).ToList();
                    if(bu.BackupRequestShiftsAndDates.AMSatisfied == true)
                    {
                        foreach(var schedule in schedules)
                        {
                            if(schedule.BScheduleName == "AM")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                addon.TimeAdded += duration.Hours*4;
                                addon.shiftDay = schedule.BScheduleType;
                            }
                        }
                        
                    }

                    if (bu.BackupRequestShiftsAndDates.MIDSatisfied == true)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.BScheduleName == "MID")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                addon.TimeAdded += duration.Hours*4;
                                addon.shiftDay = schedule.BScheduleType;
                            }
                        }
                    }

                    if (bu.BackupRequestShiftsAndDates.PMSatisfied == true)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.BScheduleName == "PM")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                addon.TimeAdded += duration.Hours*4;
                                addon.shiftDay = schedule.BScheduleType;
                            }
                        }
                    }

                    addon.Cost = addon.TimeAdded * addon.Rate/4;

                    Additions.Add(addon);
                }
            }
            #endregion

            #region get succesful appeals
            //InvoiceAppeals
            var invoiceAppeals = (from a in db.Appeals
                                  where a.AppealType == "Invoice" &&
                                  a.Beatid == BID &&
                                  a.Contractor.ContractorID == CID &&
                                  (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                  a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                  select new InvoiceAdditions
                                  {
                                      category = "Accepted Invoice Appeal",
                                      date = a.I_EventDate.ToString(),
                                      id = Guid.NewGuid(),
                                      description = a.I_AppealReason,
                                      Cost = a.I_AppealDeduction ?? 0
                                  }).ToList();

            foreach (InvoiceAdditions a in invoiceAppeals)
            {
                Additions.Add(a);
            }

            //ViolationAppeals
            var violationAppeals = (from a in db.Appeals
                                    where a.AppealType == "Violation" &&
                                    a.Beatid == BID &&
                                    a.Contractor.ContractorID == CID &&
                                    (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                    a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                    select new InvoiceAdditions
                                    {
                                        category = "Accepted Violation Appeal",
                                        date = a.V_ViolationId.ToString(),
                                        id = Guid.NewGuid(),
                                        description = a.V_ReasonForAppeal,
                                        Cost = a.V_AppropriateCharge ?? 0
                                    }).ToList();

            foreach (InvoiceAdditions a in violationAppeals)
            {
                int id = Convert.ToInt32(a.date);
                var violation = (from v in dbc.Violations
                                 where v.Id == id
                                 select v).FirstOrDefault();

                a.date = violation.DateTimeOfViolation.ToString();

                Additions.Add(a);
            }

            //Overtime
            var overtimeAppeals = (from a in db.Appeals
                                   where a.AppealType == "Overtime" &&
                                   a.Beatid == BID &&
                                   a.Contractor.ContractorID == CID &&
                                   (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                   a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                   select new InvoiceAdditions
                                   {
                                       category = "Accepted Overtime Appeal",
                                       date = a.O_Datetime.ToString(),
                                       id = Guid.NewGuid(),
                                       description = a.O_Detail,
                                       TimeAdded = a.O_BlocksInitGranted ?? 1,
                                       Rate = ISVM.PayRate
                                   }).ToList();

            foreach (InvoiceAdditions a in overtimeAppeals)
            {
                if (Convert.ToDateTime(a.date).Month == month)
                {
                    decimal rate = a.Rate / 4;
                    a.Cost = rate * a.TimeAdded;

                    Additions.Add(a);
                }   
            }
            
            #endregion

            return Json(Additions.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Get: GetInvoiceDeductions
        public ActionResult GetInvoiceDeductions(string beatid, int month, string contractorid) 
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);
            InvoiceSummaryViewModel ISVM = GetBaseRateMethod(beatid);
            List<InvoiceDeductions> Deductions = new List<InvoiceDeductions>();

            #region get all violations

            List<Violation> violations = dbc.Violations.Where(v => v.ContractorId == CID).Where(v => v.BeatId == BID).Where(v => v.DateTimeOfViolation.Month == month).Where(v => v.DateTimeOfViolation.Year == DateTime.Now.Year).Where(v => v.ViolationStatusTypeId == 2).ToList();

            foreach(Violation vio in violations)
            {
                InvoiceDeductions ID = new InvoiceDeductions();
                InvoiceDeductions AdditionalFine = new InvoiceDeductions();
                var driver = db.Drivers.Where(d => d.DriverID == vio.DriverId).Select(n => n.FirstName + " " + n.LastName).FirstOrDefault();
                var truckId = db.FleetVehicles.Where(t => t.FleetVehicleID == vio.FleetVehicleId).Select(n => n.FleetNumber).FirstOrDefault();

                //Create innitial deduction
                ID.id = Guid.NewGuid();
                ID.category = vio.ViolationType.Name;
                ID.date = vio.DateTimeOfViolation.ToString();
                ID.description = "ID: " + vio.Id + " | Code: " + vio.ViolationType.Code + " | Driver: " + driver + " | Truck #: " + truckId;
                if(vio.PenaltyForDriver != null)
                {
                    ID.description += " | Invoice Notes: " + vio.PenaltyForDriver;
                }

                //Add two deductions for each of these vio.ViolationTypeId == 19 || 
                if(vio.ViolationType.Code == "46" || vio.ViolationType.Code == "49")
                {
                    ID.Rate = Math.Round(ISVM.PayRate / 4, 2);
                    ID.TimeAdded = Convert.ToInt32(vio.LengthOfViolation.Trim()) / 15;
                    ID.Cost = Math.Round(ID.Rate * ID.TimeAdded, 2);

                    //create additional fine option deduction
                    AdditionalFine.id = Guid.NewGuid();
                    AdditionalFine.category = vio.ViolationType.Name + " Fine";
                    AdditionalFine.date = vio.DateTimeOfViolation.ToString();
                    AdditionalFine.description = "ID: " + vio.Id + " | Code: " + vio.ViolationType.Code + " | Driver: " + driver + " | Truck #: " + truckId;
                    if (vio.PenaltyForDriver != null)
                    {
                        AdditionalFine.description += " | Invoice Notes: " + vio.PenaltyForDriver;
                    }
                    AdditionalFine.Rate = 0.0033M;
                    AdditionalFine.TimeAdded = 0;
                    AdditionalFine.Cost = 0.00M;
                }
                else
                {
                    ID.Rate = Math.Round(ISVM.PayRate / 4, 2);
                    ID.TimeAdded = 0;
                    ID.Cost = 0.00M;
                }

                if(AdditionalFine.category != null)
                {
                    Deductions.Add(ID);
                    Deductions.Add(AdditionalFine);
                    AdditionalFine = null;
                }
                else
                {
                    Deductions.Add(ID);
                }

            }

            #endregion

            #region get all supplies
            InvoiceDeductions SM = new InvoiceDeductions();
            List<MerchandiseOrder> supplies = dbc.MerchandiseOrders.Where(s => s.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderFilled).Where(s => s.PickupDate.Month == month).Where(s => s.DeductFromInvoice == true).Where(s => s.ContractorId == CID).ToList();

            foreach(MerchandiseOrder MO in supplies)
            {
                SM.id = Guid.NewGuid();
                SM.category = "Supplies/Merchandise";
                SM.date = MO.CreatedOn.ToString();
                var orders = dbc.MerchandiseOrderDetails.Where(d => d.MerchandiseOrderId == MO.Id).ToList();
                SM.description = MO.ContactName + ": ";

                foreach(MerchandiseOrderDetail detail in orders)
                {
                    SM.description += "|" + detail.MerchandiseProduct.DisplayName;
                    SM.Cost += detail.UnitCost * detail.Quantity;
                }

                SM.Rate = 0.0062M;
                SM.TimeAdded = 0;
                Deductions.Add(SM);
            }

            #endregion

            #region get backup requests
            var backups = (from b in dbc.BackupRequests
                           join bursad in dbc.BackupRequestShiftsAndDates on b.Id equals bursad.BackupRequestId
                           where b.SelectedBackupContractorId == CID &&
                           b.BeatId == BID &&
                           b.IsCancelled == false &&
                           bursad.BackupDate.Month == month
                           select new
                           {
                               RequestNumber = b.RequestNumber,
                               BeatID = b.BeatId,
                               //BeatNumber = b. db.Beats.FirstOrDefault(p => b.BeatId == p.BeatID).BeatNumber,
                               RequestComments = b.Comments,
                               ResponseComments = dbc.BackupResponses.Where(p => p.BackupRequestId == b.Id).Select(p => p.Comments).FirstOrDefault(),
                               IsResolved = dbc.BackupResponses.Any(p => p.BackupRequestId == b.Id && (p.BackupResponseStatus == BackupResponseStatus.Accepted)),
                               BackupRequestShiftsAndDates = dbc.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == b.Id).Where(p => p.BackupDate.Month == month).Select(s => new
                               {
                                   Id = s.Id,
                                   BackupRequestId = s.BackupRequestId,
                                   BackupDate = s.BackupDate,
                                   AMRequested = s.AMRequested,
                                   AMSatisfied = s.AMSatisfied,
                                   MIDRequested = s.MIDRequested,
                                   MIDSatisfied = s.MIDSatisfied,
                                   PMRequested = s.PMRequested,
                                   PMSatisfied = s.PMSatisfied,
                               }).FirstOrDefault()
                           }).ToList();

            foreach (var bu in backups)
            {
                //1. If IsResolved = true then ...
                //2. If AMSatisfied get shift hours
                //3. If MIDSatisfied get shift hours
                //4. If PMSatisfied get shift hours
                //5. Create new InvoiceAddition and add to list

                if (bu.IsResolved == true)
                {
                    InvoiceDeductions BUID = new InvoiceDeductions();
                    BUID.id = Guid.NewGuid();
                    BUID.category = "Backup Resolved";
                    BUID.date = bu.BackupRequestShiftsAndDates.BackupDate.ToShortDateString();
                    BUID.description = bu.RequestNumber + " | " +  (bu.RequestComments ?? "No RqComments") + " | " + (bu.ResponseComments ?? "No RspComments");
                    BUID.Rate = ISVM.PayRate;
                    BUID.TimeAdded = 0;
                    var schedules = (db.SchedulesSearch(bu.BackupRequestShiftsAndDates.BackupDate, bu.BeatID)).ToList();
                    if (bu.BackupRequestShiftsAndDates.AMSatisfied == true)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.BScheduleName == "AM")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                BUID.TimeAdded += duration.Hours * 4;
                                BUID.shiftDay = schedule.BScheduleType;
                            }
                        }

                    }

                    if (bu.BackupRequestShiftsAndDates.MIDSatisfied == true)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.BScheduleName == "MID")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                BUID.TimeAdded += duration.Hours * 4;
                                BUID.shiftDay = schedule.BScheduleType;
                            }
                        }
                    }

                    if (bu.BackupRequestShiftsAndDates.PMSatisfied == true)
                    {
                        foreach (var schedule in schedules)
                        {
                            if (schedule.BScheduleName == "PM")
                            {
                                TimeSpan duration = DateTime.Parse(schedule.EndTime.ToString()).Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                BUID.TimeAdded += duration.Hours * 4;
                                BUID.shiftDay = schedule.BScheduleType;
                            }
                        }
                    }

                    BUID.Cost = BUID.TimeAdded * BUID.Rate / 4;

                    Deductions.Add(BUID);
                }
            }
            #endregion

            //Sort deductions by date

            return Json(Deductions.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Get: GetBeatContractors
        public ActionResult GetBeatContractors(string beatid)
        {
            Guid BID = new Guid(beatid);

            var contracts = db.Contracts.Where(c => c.BeatId == BID).Select(c => new { 
                c.ContractID,
                c.ContractorID,
                c.Contractor.ContractCompanyName,
                c.AgreementNumber,
                c.StartDate,
                c.EndDate
            });

            return Json(contracts, JsonRequestBehavior.AllowGet);
        }

        //Get: GetAnomolies
        public ActionResult GetAnamolies(string beatid, int month, string contractorid)
        {
            Guid BID = new Guid(beatid);
            Guid CID = new Guid(contractorid);
            int startDays = 1;
            int endDays = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);
            List<InvoiceAnamolies> AL = new List<InvoiceAnamolies>();

            Contract contract = db.Contracts.Where(c => c.ContractorID == CID).Where(c => c.BeatId == BID).FirstOrDefault();

            List<DateTime> CustomDates = (from days in dbc.CustomDates
                                          where days.Date.Month == month &&
                                          days.Date.Day > 1 && days.Date.Day < 7
                                          select days.Date).ToList();

            List<DateTime> HolidayDates = (from hdays in dbc.HolidayDates
                                           where hdays.Date.Month == month &&
                                           hdays.Date.Day > 1 && hdays.Date.Day < 7
                                           select hdays.Date).ToList();

            //Is end date in middle of month
            if (contract.EndDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && contract.EndDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
            {
                startDays = 1;
                endDays = contract.EndDate.Day;
            }

            if (contract.StartDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && contract.StartDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
            {
                startDays = contract.StartDate.Day;
                endDays = DateTime.DaysInMonth(DateTime.Now.Year, (int)month);
            }

            for (int i = startDays; i <= endDays; i++)
            {
                DateTime dt = new DateTime((int)DateTime.Now.Year, (int)month, i);
                if (!CustomDates.Contains(dt) && !HolidayDates.Contains(dt))
                {
                    var schedules = (db.SchedulesSearch(dt, BID)).ToList();

                    if (schedules.Count > 0)
                    {
                        foreach (SchedulesSearch_Result result in schedules)
                        {
                            //Is there a login for it. If not then anamolie
                            var anamolie = db.TruckStatus.Where(ts => ts.ScheduleID == result.ScheduleID).Where(ts => ts.StatusName.Trim() == "LogOn").Where(ts => ts.StatusStart.Month == dt.Month).Where(ts => ts.StatusStart.Day == dt.Day).FirstOrDefault();

                            if (anamolie == null)
                            {
                                InvoiceAnamolies a = new InvoiceAnamolies();
                                a.ID = Guid.NewGuid();
                                a.BeatNumber = result.BeatNumber;
                                a.ScheduledName = db.BeatSchedules.Where(bs => bs.BeatScheduleID == result.ScheduleID).Select(bs => bs.ScheduleName).FirstOrDefault();
                                a.date = dt.ToShortDateString();
                                a.Description = "No Log On for Beat Schedule";

                                AL.Add(a);
                            }
                        }
                    }
                }
            }

            return Json(AL, JsonRequestBehavior.AllowGet);
        }

        //Post: SaveInvoice
        [HttpPost]
        public ActionResult SaveInvoice(Invoice _invoice)
        {
            string message = null;

            MTC_Invoice invoice = db.MTC_Invoice.Where(i => i.InvoiceID == _invoice.InvoiceID).FirstOrDefault();

            if(invoice != null)
            {
                #region remove current invoice
                List<MTC_Invoice_Summary> summary = db.MTC_Invoice_Summary.Where(s => s.InvoiceID == _invoice.InvoiceID).ToList();
                List<MTC_Invoice_Addition> additions = db.MTC_Invoice_Addition.Where(a => a.InvoiceID == _invoice.InvoiceID).ToList();
                List<MTC_Invoice_Deductions> deductions = db.MTC_Invoice_Deductions.Where(d => d.InvoiceID == _invoice.InvoiceID).ToList();
                List<MTC_Invoice_Anomalies> anomolies = db.MTC_Invoice_Anomalies.Where(a => a.InvoiceID == _invoice.InvoiceID).ToList();

                //remove anomolies
                foreach(MTC_Invoice_Anomalies anomolie in anomolies)
                {
                    db.MTC_Invoice_Anomalies.Remove(anomolie);
                    db.SaveChanges();
                }

                //remove deductions
                foreach (MTC_Invoice_Deductions d in deductions)
                {
                    db.MTC_Invoice_Deductions.Remove(d);
                    db.SaveChanges();
                }

                //remove additions
                foreach(MTC_Invoice_Addition a in additions)
                {
                    db.MTC_Invoice_Addition.Remove(a);
                    db.SaveChanges();
                }

                //remove summary
                foreach(MTC_Invoice_Summary s in summary)
                {
                    db.MTC_Invoice_Summary.Remove(s);
                    db.SaveChanges();
                }

                db.MTC_Invoice.Remove(invoice);
                db.SaveChanges();
                #endregion

                #region install new invoice
                //Add Invoice
                MTC_Invoice NI = new MTC_Invoice();
                NI.InvoiceID = _invoice.InvoiceID;
                NI.ContractorID = _invoice.ContractorID;
                NI.Month = _invoice.Month;
                NI.BeatID = _invoice.BeatID;
                NI.FuelRate = _invoice.FuelRate;
                NI.BaseRate = _invoice.BaseRate;
                NI.Notes = _invoice.Notes;
                NI.CreatedBy = User.Identity.GetUserName();
                NI.ModifiedBy = User.Identity.GetUserName();
                NI.CreatedDate = DateTime.Now;
                NI.ModifiedDate = DateTime.Now;

                db.MTC_Invoice.Add(NI);

                //Add summary
                foreach (InvoiceSummary _sum in _invoice.Summaries)
                {
                    MTC_Invoice_Summary Sum = new MTC_Invoice_Summary();
                    Sum.InvoiceID = _invoice.InvoiceID;
                    Sum.Row = _sum.Row;
                    Sum.Days = _sum.Days;
                    Sum.Shifts = _sum.Shifts;
                    Sum.ContractHours = _sum.ContractHours;
                    Sum.OnPatrolHours = _sum.OnPatrolHours;
                    Sum.CreatedDate = DateTime.Now;
                    Sum.CreatedBy = User.Identity.GetUserName();
                    Sum.ModifiedDate = DateTime.Now;
                    Sum.ModifiedBy = User.Identity.GetUserName();

                    db.MTC_Invoice_Summary.Add(Sum);
                }

                //Add Additions
                if (_invoice.Additions != null)
                {
                    foreach (InvoiceAdditions IA in _invoice.Additions)
                    {
                        MTC_Invoice_Addition NIA = new MTC_Invoice_Addition();
                        NIA.InvoiceID = _invoice.InvoiceID;
                        NIA.Category = IA.category;
                        NIA.Cost = (double)IA.Cost;
                        NIA.Date = Convert.ToDateTime(IA.date);
                        NIA.Description = IA.description;
                        NIA.Rate = (double)IA.Rate;
                        NIA.TimeAdded = IA.TimeAdded;
                        NIA.CreatedBy = User.Identity.GetUserName();
                        NIA.ModifiedBy = User.Identity.GetUserName();
                        NIA.CreatedDate = DateTime.Now;
                        NIA.ModifiedDate = DateTime.Now;

                        db.MTC_Invoice_Addition.Add(NIA);
                    }
                }

                //Add Deductions
                if (_invoice.Deductions != null)
                {
                    foreach (InvoiceDeductions ID in _invoice.Deductions)
                    {
                        MTC_Invoice_Deductions NID = new MTC_Invoice_Deductions();
                        NID.InvoiceID = _invoice.InvoiceID;
                        NID.Category = ID.category;
                        NID.Cost = (double)ID.Cost;
                        NID.Date = Convert.ToDateTime(ID.date);
                        NID.Description = ID.description;
                        NID.Rate = (double)ID.Rate;
                        NID.TimeAdded = ID.TimeAdded;
                        NID.CreatedBy = User.Identity.GetUserName();
                        NID.ModifiedBy = User.Identity.GetUserName();
                        NID.CreatedDate = DateTime.Now;
                        NID.ModifiedDate = DateTime.Now;

                        db.MTC_Invoice_Deductions.Add(NID);
                    }
                }

                try
                {
                    db.SaveChanges();
                    message = "Success";
                }
                catch (Exception x)
                {
                    message = x.Message;
                }

                #endregion
            }
            else
            {
                #region add new invoice

                //Add Invoice
                MTC_Invoice NI = new MTC_Invoice();
                NI.InvoiceID = _invoice.InvoiceID;
                NI.ContractorID = _invoice.ContractorID;
                NI.Month = _invoice.Month;
                NI.BeatID = _invoice.BeatID;
                NI.FuelRate = _invoice.FuelRate;
                NI.BaseRate = _invoice.BaseRate;
                NI.Notes = _invoice.Notes;
                NI.CreatedBy = User.Identity.GetUserName(); ;
                NI.ModifiedBy = User.Identity.GetUserName();
                NI.CreatedDate = DateTime.Now;
                NI.ModifiedDate = DateTime.Now;

                db.MTC_Invoice.Add(NI);

                //Add summary
                foreach (InvoiceSummary _sum in _invoice.Summaries)
                {
                    MTC_Invoice_Summary Sum = new MTC_Invoice_Summary();
                    Sum.InvoiceID = _invoice.InvoiceID;
                    Sum.Row = _sum.Row;
                    Sum.Days = _sum.Days;
                    Sum.Shifts = _sum.Shifts;
                    Sum.ContractHours = _sum.ContractHours;
                    Sum.OnPatrolHours = _sum.OnPatrolHours;
                    Sum.CreatedDate = DateTime.Now;
                    Sum.CreatedBy = User.Identity.GetUserName();
                    Sum.ModifiedDate = DateTime.Now;
                    Sum.ModifiedBy = User.Identity.GetUserName();

                    db.MTC_Invoice_Summary.Add(Sum);
                }

                //Add Additions
                if (_invoice.Additions != null)
                {
                    foreach (InvoiceAdditions IA in _invoice.Additions)
                    {
                        MTC_Invoice_Addition NIA = new MTC_Invoice_Addition();
                        NIA.InvoiceID = _invoice.InvoiceID;
                        NIA.Category = IA.category;
                        NIA.Cost = (double)IA.Cost;
                        NIA.Date = Convert.ToDateTime(IA.date);
                        NIA.Description = IA.description;
                        NIA.Rate = (double)IA.Rate;
                        NIA.TimeAdded = IA.TimeAdded;
                        NIA.CreatedBy = User.Identity.GetUserName();
                        NIA.ModifiedBy = User.Identity.GetUserName();
                        NIA.CreatedDate = DateTime.Now;
                        NIA.ModifiedDate = DateTime.Now;

                        db.MTC_Invoice_Addition.Add(NIA);
                    }
                }

                //Add Deductions
                if (_invoice.Deductions != null)
                {
                    foreach (InvoiceDeductions ID in _invoice.Deductions)
                    {
                        MTC_Invoice_Deductions NID = new MTC_Invoice_Deductions();
                        NID.InvoiceID = _invoice.InvoiceID;
                        NID.Category = ID.category;
                        NID.Cost = (double)ID.Cost;
                        NID.Date = Convert.ToDateTime(ID.date);
                        NID.Description = ID.description;
                        NID.Rate = (double)ID.Rate;
                        NID.TimeAdded = ID.TimeAdded;
                        NID.CreatedBy = User.Identity.GetUserName();
                        NID.ModifiedBy = User.Identity.GetUserName();
                        NID.CreatedDate = DateTime.Now;
                        NID.ModifiedDate = DateTime.Now;

                        db.MTC_Invoice_Deductions.Add(NID);
                    }
                }

                //Add Anamolies
                if (_invoice.Anamolies != null)
                {
                    foreach (InvoiceAnamolies ID in _invoice.Anamolies)
                    {
                        MTC_Invoice_Anomalies NAN = new MTC_Invoice_Anomalies();
                        NAN.InvoiceID = _invoice.InvoiceID;
                        NAN.Description = ID.Description;
                        NAN.Date = Convert.ToDateTime(ID.date);
                        NAN.CreatedBy = User.Identity.GetUserName();
                        NAN.ModifiedBy = User.Identity.GetUserName();
                        NAN.CreatedDate = DateTime.Now;
                        NAN.ModifiedDate = DateTime.Now; ;

                        db.MTC_Invoice_Anomalies.Add(NAN);
                    }
                }

                try
                {
                    db.SaveChanges();
                    message = "Success";
                }
                //catch (Exception x)
                //{
                //    message = x.Message;
                //}
                catch (DbEntityValidationException dbEx)
                {
                    message = dbEx.Message;
                }

                #endregion
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        //Get:InvoiceExist
        public ActionResult InvoiceExists(string invoiceNumber)
        {
            bool exists = false;

            string invoiceNum = db.MTC_Invoice.Where(i => i.InvoiceID == invoiceNumber).Select(i => i.InvoiceID).FirstOrDefault();
            if (invoiceNum != null)
            {
                exists = true;
            }

            return Json(exists.ToString(), JsonRequestBehavior.AllowGet);
        }

        //Get: All Invoices
        public ActionResult GetInvoices()
        {
            var Invoices = (from invoice in db.MTC_Invoice
                            select new
                            {
                                InvoiceNumber = invoice.InvoiceID,
                                ModDate = invoice.ModifiedDate.Month + "/" + invoice.ModifiedDate.Day + "/" + invoice.ModifiedDate.Year,
                                ModBy = invoice.ModifiedBy,
                                Notes = invoice.Notes
                            }).OrderBy(i => i.InvoiceNumber).OrderBy(i => i.ModDate).ToList();
                
                //db.MTC_Invoice.Select(x => new { x.InvoiceID, x.Notes, x.ModifiedDate }).OrderBy(x => x.InvoiceID).OrderBy( x => x.ModifiedDate.to).ToList();

            return Json(Invoices.ToList(), JsonRequestBehavior.AllowGet);
        }

        //Get: Get Invoice 
        public ActionResult GetInvoice(string invoiceNumber)
        {
            Invoice _invoice = new Invoice();
            MTC_Invoice invoice = db.MTC_Invoice.Where(i => i.InvoiceID == invoiceNumber).FirstOrDefault();

            _invoice.BaseRate = (float) invoice.BaseRate;
            _invoice.BeatID = invoice.BeatID;
            _invoice.BeatNum = db.BeatDatas.Where(b => b.ID == invoice.BeatID).Select(b => b.BeatName).FirstOrDefault();
            _invoice.ContractorID = invoice.ContractorID;
            _invoice.FuelRate = (float) invoice.FuelRate;
            _invoice.InvoiceID = invoice.InvoiceID;
            _invoice.Month = invoice.Month;
            _invoice.Notes = invoice.Notes;
            _invoice.Summaries = new List<InvoiceSummary>();
            _invoice.Additions = new List<InvoiceAdditions>();
            _invoice.Deductions = new List<InvoiceDeductions>();
            _invoice.Anamolies = new List<InvoiceAnamolies>();

            var summaries = db.MTC_Invoice_Summary.Where(s => s.InvoiceID == invoiceNumber).ToList();
            foreach (MTC_Invoice_Summary sum in summaries)
            {
                InvoiceSummary Summary = new InvoiceSummary();
                Summary.ContractHours = (float) sum.ContractHours;
                Summary.Days = (float)sum.Days;
                Summary.OnPatrolHours = (float)sum.OnPatrolHours;
                Summary.Row = sum.Row;
                Summary.Shifts = (float)sum.Shifts;
                _invoice.Summaries.Add(Summary);
            }
            var additions = db.MTC_Invoice_Addition.Where(a => a.InvoiceID == invoiceNumber).OrderBy(a => a.Date).ToList();
            foreach (MTC_Invoice_Addition add in additions)
            {
                InvoiceAdditions addition = new InvoiceAdditions();
                addition.id = add.AdditionID; 
                addition.category = add.Category;
                addition.Cost = (Decimal) add.Cost;
                addition.date = add.Date.ToString();
                addition.description = add.Description;
                addition.Rate = (Decimal) add.Rate;
                addition.TimeAdded = add.TimeAdded;
                addition.MENum = random.Next(0, 1000);
                _invoice.Additions.Add(addition);
                _invoice.Additions.OrderBy(a => a.date);
            }
            var deductions = db.MTC_Invoice_Deductions.Where(a => a.InvoiceID == invoiceNumber).OrderBy(d => d.Date).ToList();
            foreach (MTC_Invoice_Deductions ded in deductions)
            {
                InvoiceDeductions deduction = new InvoiceDeductions();
                deduction.id = ded.DeductionID;
                deduction.category = ded.Category;
                deduction.Cost = (Decimal)ded.Cost;
                deduction.date = ded.Date.ToString();
                deduction.description = ded.Description;
                deduction.Rate = (Decimal)ded.Rate;
                deduction.TimeAdded = ded.TimeAdded;
                deduction.MENum = random.Next(0, 1000);
                _invoice.Deductions.Add(deduction);
                _invoice.Deductions.OrderBy(b => b.date);
            }
            var anomalies = db.MTC_Invoice_Anomalies.Where(a => a.InvoiceID == invoiceNumber).OrderBy(i => i.Date).ToList();
            foreach (MTC_Invoice_Anomalies nom in anomalies)
            {
                InvoiceAnamolies anomalie = new InvoiceAnamolies();
                anomalie.ID = nom.AnomalyID;
                anomalie.Description = nom.Description;
                anomalie.date = nom.Date.ToString();
                anomalie.BeatNumber = _invoice.BeatNum;
                _invoice.Anamolies.Add(anomalie);
                _invoice.Anamolies.OrderBy(i => i.date);
            }

            Guid BID = _invoice.BeatID;
            Guid CID = _invoice.ContractorID;
            int startDay = 1;
            int endDay = DateTime.DaysInMonth(DateTime.Now.Year, (int)_invoice.Month);

            Contract contract = db.Contracts.Where(c => c.ContractorID == CID).Where(c => c.BeatId == BID).FirstOrDefault();

            //Is end date in middle of month
            if (contract.EndDate > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && contract.EndDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)))
            {
                startDay = 1;
                endDay = contract.EndDate.Day;
            }

            if (contract.StartDate > new DateTime(DateTime.Now.Year, _invoice.Month, 1) && contract.StartDate < new DateTime(DateTime.Now.Year, _invoice.Month, DateTime.DaysInMonth(DateTime.Now.Year, _invoice.Month)))
            {
                startDay = contract.StartDate.Day;
                endDay = DateTime.DaysInMonth(DateTime.Now.Year, (int)_invoice.Month);
            }

            //Get Monthly Days
            _invoice.WeekDaysInMonth = WeekdaysInMonth(startDay, endDay, _invoice.Month);
            _invoice.SaturDaysInMonth = SaturdaysInMonth(startDay, endDay, _invoice.Month);
            _invoice.SunDaysInMonth = SundaysInMonth(startDay, endDay, _invoice.Month);
            _invoice.HolidaysInMonth = HolidaysInMonth(startDay, endDay, _invoice.Month);
            _invoice.CustomDaysInMonth = CustomDaysInMonth(startDay, endDay, _invoice.Month, BID);

            //TOTALS
            _invoice.TotalWeekDaysInMonth = _invoice.WeekDaysInMonth.Count();
            _invoice.TotalSaturDaysInMonth = _invoice.SaturDaysInMonth.Count();
            _invoice.TotalSunDaysInMonth = _invoice.SunDaysInMonth.Count();
            _invoice.TotalHolidaysInMonth = _invoice.HolidaysInMonth.Count();
            _invoice.TotalCustomDaysInMonth = _invoice.CustomDaysInMonth.Count();

            return Json(_invoice, JsonRequestBehavior.AllowGet);
        }

        // GET: EditInvoice
        public ActionResult EditInvoice()
        {
            return View();
        }

        // GET: DeleteInvoice
        public ActionResult DeleteInvoice(string invoiceNumber)
        {
            db.MTC_Invoice_Anomalies.RemoveRange(db.MTC_Invoice_Anomalies.Where(i => i.InvoiceID == invoiceNumber));
            db.MTC_Invoice_Deductions.RemoveRange(db.MTC_Invoice_Deductions.Where(i => i.InvoiceID == invoiceNumber));
            db.MTC_Invoice_Addition.RemoveRange(db.MTC_Invoice_Addition.Where(i => i.InvoiceID == invoiceNumber));
            db.MTC_Invoice_Summary.RemoveRange(db.MTC_Invoice_Summary.Where(i => i.InvoiceID == invoiceNumber));
            db.SaveChanges();

            db.MTC_Invoice.RemoveRange(db.MTC_Invoice.Where(i => i.InvoiceID == invoiceNumber));
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //UserManagerContext
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private InvoiceSummaryViewModel GetBaseRateMethod(string beatid)
        {
            Guid BID = new Guid(beatid);
            InvoiceSummaryViewModel ISVM = new InvoiceSummaryViewModel();

            #region Get fuel and payrate
            var pNumb = (from setting in dbc.MTCApplicationSettings
                         where setting.Name == "FuelRate"
                         select setting.Value).First();

            var pr = (from r in db.MTCRateTables
                      where r.BeatID == BID
                      select r).SingleOrDefault();

            switch (pNumb)
            {
                case "p100":
                    ISVM.PayRate = (Decimal)pr.p100;
                    ISVM.FuelRate = 1.0M;
                    break;
                case "p150":
                    ISVM.PayRate = (Decimal)pr.p150;
                    ISVM.FuelRate = 1.50M;
                    break;
                case "p200":
                    ISVM.PayRate = (Decimal)pr.p200;
                    ISVM.FuelRate = 2.00M;
                    break;
                case "p250":
                    ISVM.PayRate = (Decimal)pr.p250;
                    ISVM.FuelRate = 2.50M;
                    break;
                case "p300":
                    ISVM.PayRate = (Decimal)pr.p300;
                    ISVM.FuelRate = 3.00M;
                    break;
                case "p350":
                    ISVM.PayRate = (Decimal)pr.p350;
                    ISVM.FuelRate = 3.50M;
                    break;
                case "p400":
                    ISVM.PayRate = (Decimal)pr.p400;
                    ISVM.FuelRate = 4.00M;
                    break;
                case "p450":
                    ISVM.PayRate = (Decimal)pr.p450;
                    ISVM.FuelRate = 4.50M;
                    break;
                case "p500":
                    ISVM.PayRate = (Decimal)pr.p500;
                    ISVM.FuelRate = 5.00M;
                    break;
                case "p550":
                    ISVM.PayRate = (Decimal)pr.p550;
                    ISVM.FuelRate = 5.50M;
                    break;
                case "p600":
                    ISVM.PayRate = (Decimal)pr.p600;
                    ISVM.FuelRate = 6.00M;
                    break;
                case "p650":
                    ISVM.PayRate = (Decimal)pr.p650;
                    ISVM.FuelRate = 6.50M;
                    break;
                case "p700":
                    ISVM.PayRate = (Decimal)pr.p700;
                    ISVM.FuelRate = 7.00M;
                    break;
                case "p750":
                    ISVM.PayRate = (Decimal)pr.p750;
                    ISVM.FuelRate = 7.50M;
                    break;
                case "p800":
                    ISVM.PayRate = (Decimal)pr.p800;
                    ISVM.FuelRate = 8.00M;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            #endregion

            return ISVM;
        } 
    }

}