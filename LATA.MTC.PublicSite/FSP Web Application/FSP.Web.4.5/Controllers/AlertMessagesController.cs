using FSP.Domain.Model;
using FSP.Web.Filters;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;
using Microsoft.AspNet.SignalR.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FSP.Web.Controllers
{
    [CustomAuthorization(Roles = "Admin, Dispatcher, CHP")]
    public class AlertMessagesController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Alerts()
        {
            ViewBag.Heading = DateTime.Today.ToString("MMMM dd, yyyy") + " Current Alarms";
            return View();
        }

        public ActionResult DriversAlertComments()
        {
            ViewBag.Heading = DateTime.Today.ToString("MMMM dd, yyyy") + " Driver's Alert Comments";
            return View();
        }

        [OutputCache(Duration = 10)]
        public ActionResult GetAlerts()
        {
            AlarmStatus[] alarmStatuses = new AlarmStatus[0];
            List<AlarmStatus> returnList = new List<AlarmStatus>();
            using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
            {
                alarmStatuses = service.GetAllAlarms();
                Debug.WriteLine(alarmStatuses.Count() + " alarms received");
            }

            bool isValid = false;
            foreach (var item in alarmStatuses)
            {
                isValid = false;

                if (item.RollInAlarm == true && item.RollInAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.RollOutAlarm == true && item.RollOutAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.IncidentAlarm == true && item.IncidentAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.LogOffAlarm == true && item.LogOffAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.LogOnAlarm == true && item.LogOnAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.GPSIssueAlarm == true && item.GPSIssueAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.OnPatrolAlarm == true && item.OnPatrolAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.SpeedingAlarm == true && item.SpeedingTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
                if (item.OutOfBoundsAlarm == true && item.OutOfBoundsStartTime.ToString("MM/dd/yyyy") != "01/01/2001")
                    isValid = true;
               
                

                if (isValid)
                    returnList.Add(item);
            }

            var alarms = returnList.OrderBy(p => p.BeatNumber).ThenBy(p => p.VehicleNumber).ThenBy(p => p.DriverName).ToList();
            Debug.WriteLine(alarms.Count() + " alarms returned");
            return Json(alarms, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ClearAlarm(string id, string alarmType)
        {
            String retValue = String.Empty;
            if (!String.IsNullOrEmpty(alarmType) && !String.IsNullOrEmpty(id))
            {
                using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                {
                    service.ClearAlarm(id, alarmType);
                    Debug.WriteLine("Alarm Cleared for " + id);
                    retValue = "Thank you! Alarm successfully cleared";
                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ExcuseAlarm(string vehicleNumber, string beatNumber, string alarmType, string driverName, string comments)
        {
            String retValue = String.Empty;
            if (!String.IsNullOrEmpty(alarmType) && !String.IsNullOrEmpty(vehicleNumber))
            {
                using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                {
                    service.ExcuseAlarm(vehicleNumber, beatNumber, alarmType, driverName, comments);
                    Debug.WriteLine("Alarm Excused for " + vehicleNumber);
                    retValue = "Thank you! Alarm successfully excused";
                }
            }
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendAlertMessage(String alertMessage, string selectedTrucks)
        {
            List<FleetVehicle> fleetVehicles = new List<FleetVehicle>();
            List<User> users = new List<User>();
            using (FSPDataContext dc = new FSPDataContext())
            {
                fleetVehicles = dc.FleetVehicles.ToList();
                users = dc.Users.ToList();
            }

            using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
            {
                var trucks = new JavaScriptSerializer().Deserialize<IEnumerable<SelectedTruck>>(selectedTrucks);
                foreach (var truck in trucks)
                {
                    TowTruckServiceRef.TruckMessage message = new TowTruckServiceRef.TruckMessage();
                    message.MessageID = Guid.NewGuid();
                    message.MessageText = alertMessage;
                    message.TruckIP = fleetVehicles.Where(p => p.VehicleNumber == truck.truckNumber).FirstOrDefault().IPAddress;
                    message.UserID = users.Where(p => p.Email == User.Identity.Name).FirstOrDefault().UserID;
                    message.SentTime = DateTime.Now;
                    service.SendMessage(message);
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateAlarm(AlarmHistory alarm)
        {
            String retValue = String.Empty;
            if (alarm != null)
            {
                using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                {
                    if (alarm.IsExcused)
                    {
                        service.ExcuseAlarm(alarm.VehicleNumber, alarm.BeatNumber, alarm.AlarmType, alarm.DriverName, alarm.Comments);
                        retValue = "Thank you! Alarm successfully excused";
                    }

                    else
                    {
                        service.UnexcuseAlarm(alarm.VehicleNumber, alarm.BeatNumber, alarm.AlarmType, alarm.DriverName, alarm.Comments);
                        retValue = "Thank you! Alarm successfully un-excused";
                    }

                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        public ActionResult History()
        {
            ViewBag.Heading = DateTime.Today.ToString("MMMM dd, yyyy") + " Alarm Detail";
            return View();
        }

        //[OutputCache(Duration = 60, VaryByParam = "beat;driver;date")]
        [HttpPost]
        public ActionResult GetAlarmHistory(String beat, String driver, String date, String alarmType, bool? isExcused)
        {
            List<AlarmHistory> returnList = new List<AlarmHistory>();

            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            DateTime end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            if (!String.IsNullOrEmpty(date))
            {
                DateTime dtDate = Convert.ToDateTime(date);

                start = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 0, 0, 0);
                end = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 23, 59, 59);
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["fspConnectionString"].ToString()))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("GetAlarmHistory", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartTime", start);
                command.Parameters.AddWithValue("@EndTime", end);

                // Open the connection in a try/catch block.  
                // Create and execute the DataReader, writing the result 
                // set to the console window. 
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnList.Add(new AlarmHistory
                        {
                            BeatNumber = reader[0].ToString(),
                            ContractCompanyName = reader[1].ToString(),
                            VehicleNumber = reader[2].ToString(),
                            DriverName = reader[3].ToString(),
                            AlarmTime = Convert.ToDateTime(reader[4].ToString()),
                            AlarmType = reader[5].ToString(),
                            Comments = reader[6].ToString(),
                            ExcuseTime = reader[7].ToString()
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

            if (!String.IsNullOrEmpty(beat))
            {
                returnList = returnList.Where(p => p.BeatNumber == beat).ToList();
            }

            if (!String.IsNullOrEmpty(driver))
            {
                returnList = returnList.Where(p => p.DriverName == driver).ToList();
            }

            if (!String.IsNullOrEmpty(alarmType))
            {
                returnList = returnList.Where(p => p.AlarmType.Replace(" ", "").ToLower() == alarmType.ToLower()).ToList();
            }

            if (isExcused != null)
            {
                if (isExcused == true)
                    returnList = returnList.Where(p => !String.IsNullOrEmpty(p.ExcuseTime)).ToList();
                else
                    returnList = returnList.Where(p => String.IsNullOrEmpty(p.ExcuseTime)).ToList();
            }

            return Json(returnList.OrderBy(p => p.BeatNumber).ThenBy(p => p.ContractCompanyName).ThenBy(p => p.VehicleNumber).ThenBy(p => p.DriverName).ToList(), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetDriversAlertComments(String beat, String driver, String date, String alarmType)
        {
            List<DriversAlertComment> returnList = new List<DriversAlertComment>();

            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
            DateTime end = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            if (!String.IsNullOrEmpty(date))
            {
                DateTime dtDate = Convert.ToDateTime(date);

                start = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 0, 0, 0);
                end = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, 23, 59, 59);
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["fspConnectionString"].ToString()))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("GetEarlyRollIns", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
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
                        returnList.Add(new DriversAlertComment
                        {
                            DriverLastName = reader[0].ToString(),
                            DriverFirstName = reader[1].ToString(),
                            DriverFullName =  reader[0].ToString() + ", " + reader[1].ToString(),
                            Datestamp = reader[2].ToString(),
                            Explanation = reader[3].ToString(),
                            CHPLogNumber = reader[4].ToString(),
                            VehicleNumber = reader[5].ToString(),
                            BeatNumber = reader[6].ToString(),
                            ExceptionType = reader[7].ToString()
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

            if (!String.IsNullOrEmpty(beat))
            {
                returnList = returnList.Where(p => p.BeatNumber == beat).ToList();
            }

            if (!String.IsNullOrEmpty(driver))
            {
                returnList = returnList.Where(p => p.DriverFullName == driver).ToList();
            }

            if (!String.IsNullOrEmpty(alarmType))
            {
                returnList = returnList.Where(p => p.ExceptionType.Replace(" ", "").ToLower() == alarmType.ToLower()).ToList();
            }
           
            return Json(returnList.OrderBy(p => p.BeatNumber).ThenBy(p => p.Datestamp).ToList(), JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Duration = 60)]
        public ActionResult GetAllBeats()
        {
            var query = from q in db.vBeats
                        orderby q.BeatNumber
                        select new
                        {
                            Id = q.BeatID.ToString(),
                            Text = q.BeatNumber
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetAllDrivers()
        {
            var query = from q in db.Drivers
                        orderby q.LastName, q.FirstName
                        select new
                        {
                            Id = q.DriverID.ToString(),
                            Text = q.LastName + ", " + q.FirstName
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetAlarmTypes()
        {
            //LOGON
            //ROLLIN
            //ROLLOUT
            //ONPATROL
            //LOGOFF
            //INCIDENT
            //GPSISSUE
            //STATIONARY

            List<String> alarmTypes = new List<string>() { "LOGON", "ROLLIN", "ONPATROL", "LOGOFF", "INCIDENT", "GPSISSUE", "STATIONARY" };
            return Json(alarmTypes, JsonRequestBehavior.AllowGet);
        }


    }

}
