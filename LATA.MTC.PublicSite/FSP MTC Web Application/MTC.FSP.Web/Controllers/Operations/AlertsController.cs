using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers.Operations
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class AlertsController : MtcBaseController
    {
        #region Views

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlertDetails()
        {
            return View();
        }

        public ActionResult DriverAlertComments()
        {
            return View();
        }

        #endregion

        #region Data Calls

        /// <summary>
        ///     //"LONGBREAK":
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 10)]
        public ActionResult GetAlerts()
        {
            using (var service = new TowTruckServiceClient())
            {
                var data = service.GetAllAlarms().ToList().Select(a => new
                {
                    //BeatNumber = !string.IsNullOrEmpty(a.BeatNumber) ? Convert.ToInt32(a.BeatNumber) : 0,
                    a.BeatNumber,
                    a.CallSign,
                    a.VehicleNumber,
                    a.ContractCompanyName,
                    a.DriverName,

                    a.RollInAlarmID,
                    a.RollInAlarm,
                    a.RollInAlarmTime,
                    a.RollInAlarmDuration,
                    a.RollInAlarmExcused,

                    a.GPSIssueAlarmID,
                    a.GPSIssueAlarm,
                    a.GPSIssueAlarmStart,
                    a.GPSIssueDuration,
                    a.GPSIssueAlarmExcused,

                    a.OnPatrolAlarmID,
                    a.OnPatrolAlarm,
                    a.OnPatrolAlarmTime,
                    a.OnPatrolDuration,
                    a.OnPatrolAlarmExcused,

                    a.LongBreakAlarmID,
                    a.LongBreakAlarm,
                    a.LongBreakAlarmStart,
                    a.LongBreakDuration,
                    a.LongBreakAlarmExcused,

                    a.IncidentAlarmID,
                    a.IncidentAlarm,
                    a.IncidentAlarmTime,
                    a.IncidentDuration,
                    a.IncidentAlarmExcused,

                    a.OutOfBoundsAlarmID,
                    a.OutOfBoundsAlarm,
                    a.OutOfBoundsStartTime,
                    a.OutOfBoundsDuration,
                    a.OutOfBoundsExcused,

                    a.OvertimeAlarmID,
                    a.OvertimeAlarm,
                    a.OvertimeAlarmStart,
                    a.OvertimeAlarmDuration,
                    a.OvertimeAlarmExcused,

                    a.SpeedingAlarm,
                    a.SpeedingTime,
                    a.SpeedingAlarmID,

                    a.StationaryAlarmID,
                    a.StationaryAlarm,
                    a.StationaryAlarmStart,
                    a.StationaryAlarmDuration,
                    a.StationaryAlarmExcused,
                });
                return Json(data.OrderBy(p => p.BeatNumber), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAlertDetails(Guid? beatId, Guid? driverId, string date, string alertType, bool? isExcused)
        {
            IEnumerable<AlertDetail> returnList;

            var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            var end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            if (!string.IsNullOrEmpty(date))
            {
                var dtDate = Convert.ToDateTime(date);

                start = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 0, 0, 0);
                end = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 23, 59, 59);
            }

            using (MTCDBEntities db = new MTCDBEntities())
            {
                returnList = db.TruckAlerts.Where(p => p.AlertStart >= start && p.AlertStart <= end).ToList().Select(a => new AlertDetail
                {
                    AlertId = a.AlertID,
                    BeatNumber = !string.IsNullOrEmpty(a.Beat) ? Convert.ToInt32(a.Beat) : 0,                    
                    ContractCompanyName = a.ContractorCompany,
                    DriverName = a.DriverName,
                    VehicleNumber = a.TruckNumber,
                    AlarmTime = a.AlertStart,
                    AlarmDuration = a.AlertMins,
                    AlarmType = a.AlertName,
                    Comments = a.Comment,
                    ExcuseTime = a.ExcuseTime,
                    ExcusedBy = a.ExcusedBy,
                    IsExcused = a.ExcuseTime != null,
                    CallSign = a.CallSign
                });

                #region filtering

                if (beatId != null)
                {
                    var dbBeat = db.BeatDatas.FirstOrDefault(p => p.ID == beatId);
                    if (dbBeat != null)
                    {
                        returnList = returnList.Where(p => p.BeatNumber.ToString() == dbBeat.BeatName);
                    }
                }

                if (driverId != null)
                {
                    Driver dbDriver = db.Drivers.FirstOrDefault(d => d.DriverID == driverId);
                    if (dbDriver != null)
                    {
                        returnList = returnList.Where(p => p.DriverName == dbDriver.FirstName + " " + dbDriver.LastName);
                    }
                }

                if (!string.IsNullOrEmpty(alertType))
                {
                    returnList = returnList.Where(p => p.AlarmType.Replace(" ", "").ToLower() == alertType.ToLower());
                }

                if (isExcused != null)
                {
                    returnList = returnList.Where(p => p.IsExcused == isExcused).ToList();
                }

                #endregion

                return Json(returnList.OrderBy(p => p.BeatNumber).ThenBy(p => p.ContractCompanyName).ThenBy(p => p.VehicleNumber).ThenBy(p => p.DriverName).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDriverAlertComments(string beat, string driver, string date, string alarmType)
        {
            List<DriverAlertComment> returnList = new List<DriverAlertComment>();

            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            DateTime end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            if (!string.IsNullOrEmpty(date))
            {
                DateTime dtDate = Convert.ToDateTime(date);

                start = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 0, 0, 0);
                end = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 23, 59, 59);
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MTCDatabase"].ToString()))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("GetEarlyRollIns", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dtStart", start);
                command.Parameters.AddWithValue("@dtEnd", end);

                // Open the connection in a try/catch block.  
                // Create and execute the DataReader, writing the result 
                // set to the console window. 
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnList.Add(new DriverAlertComment
                        {
                            DriverFullName = reader[0].ToString(),
                            Datestamp = reader[1].ToString(),
                            Comment = reader[2].ToString(),
                            VehicleNumber = reader[3].ToString(),
                            BeatNumber = reader[4].ToString(),
                            ExceptionType = reader[5].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }

            if (!string.IsNullOrEmpty(beat))
            {
                returnList = returnList.Where(p => p.BeatNumber == beat).ToList();
            }

            if (!string.IsNullOrEmpty(driver))
            {
                returnList = returnList.Where(p => p.DriverFullName == driver).ToList();
            }

            if (!string.IsNullOrEmpty(alarmType))
            {
                returnList = returnList.Where(p => p.ExceptionType.Replace(" ", "").ToLower() == alarmType.ToLower()).ToList();
            }

            return Json(returnList.OrderBy(p => p.BeatNumber).ThenBy(p => p.Datestamp).ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Actions

        [HttpPost]
        public ActionResult ClearAlert(string vehicleNumber, string alertType, Guid alertId)
        {
            bool retValue = false;
            if (!string.IsNullOrEmpty(alertType) && !string.IsNullOrEmpty(vehicleNumber))
            {
                using (TowTruckServiceClient service = new TowTruckServiceClient())
                {
                    service.ClearAlarm(vehicleNumber, alertType, alertId);
                    Debug.WriteLine("Alarm Cleared for " + vehicleNumber);
                    retValue = true;
                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExcuseAlert(string vehicleNumber, string alertType, string comments, Guid alertId)
        {
            bool retValue = false;
            if (!string.IsNullOrEmpty(alertType) && !string.IsNullOrEmpty(vehicleNumber))
            {
                using (TowTruckServiceClient service = new TowTruckServiceClient())
                {
                    service.ExcuseAlarm(vehicleNumber, alertType, HttpContext.User.Identity.Name, alertId, comments);
                    Debug.WriteLine("Alarm Excused for " + vehicleNumber);
                    retValue = true;
                }
            }
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAlertDetail(AlertDetail alertDetail)
        {
            string retValue = string.Empty;
            if (alertDetail != null)
            {
                using (var service = new TowTruckServiceClient())
                {
                    if (alertDetail.IsExcused)
                    {
                        service.ExcuseAlarm(alertDetail.VehicleNumber, alertDetail.AlarmType, HttpContext.User.Identity.Name, alertDetail.AlertId, alertDetail.Comments);
                        retValue = "Thank you! Alarm successfully excused";
                    }
                    else
                    {
                        service.UnexcuseAlarm(alertDetail.VehicleNumber, alertDetail.BeatNumber.ToString(), alertDetail.AlarmType, alertDetail.DriverName, alertDetail.Comments);
                        retValue = "Thank you! Alarm successfully un-excused";
                    }
                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}