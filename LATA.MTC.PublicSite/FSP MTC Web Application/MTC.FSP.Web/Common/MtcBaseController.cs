using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Common
{
    public class MtcBaseController : Controller
    {
        public readonly int PageSize = 50;

        public DateTime _threeMonthsAgo = DateTime.Today.AddMonths(-3);

        public MtcBaseController()
        {
            if (System.Web.HttpContext.Current.User == null) return;

            if (!System.Web.HttpContext.Current.User.IsInRole("TowContractor")) return;

            using (var identity = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                var user = userManager.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

                if (user != null)
                {
                    if (user.ContractorId == null) return;

                    UsersContractorId = user.ContractorId;
                    using (var db = new MTCDBEntities())
                    {
                        var contractor = db.Contractors.FirstOrDefault(p => p.ContractorID == user.ContractorId);
                        if (contractor != null)
                            UsersContractorCompanyName = contractor.ContractCompanyName;
                    }
                }
                else
                {
                    UsersContractorId = null;
                }
            }
        }

        public string UsersContractorCompanyName { get; set; }
        public Guid? UsersContractorId { get; set; }

        public ActionResult CanEdit(string currentControllerName)
        {
            bool retValue = !(currentControllerName == "DailySchedule" && HttpContext.User.IsInRole("TowContractor"));

            if (currentControllerName == "Violations" && (HttpContext.User.IsInRole("CHPOfficer") || HttpContext.User.IsInRole("DataConsultant")))
                retValue = false;
            if (currentControllerName == "TroubleTickets" && HttpContext.User.IsInRole("DataConsultant"))
                retValue = false;
            if (currentControllerName == "Merchandise" && HttpContext.User.IsInRole("DataConsultant"))
                retValue = false;
            if (currentControllerName == "Scheduling" && (HttpContext.User.IsInRole("DataConsultant") || HttpContext.User.IsInRole("TowContractor")))
                retValue = false;
            if (currentControllerName == "AssetStatusLocations" && HttpContext.User.IsInRole("DataConsultant"))
                retValue = false;
            if (currentControllerName == "AssetWarranties" && HttpContext.User.IsInRole("DataConsultant"))
                retValue = false;

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentUser()
        {
            try
            {
                return Json(GetCurrentUserContext(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewConfirmation(string message)
        {
            ViewBag.Message = message;
            return View("_Confirmation");
        }

        protected UserListViewModel GetCurrentUserContext()
        {
            UserListViewModel appUser = null;

            if (System.Web.HttpContext.Current.User != null)
            {
                using (var identity = new ApplicationDbContext())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                    appUser = new UserListViewModel(userManager.FindById(User.Identity.GetUserId()));
                }

                if (appUser.ContractorId != null)
                    using (var db = new MTCDBEntities())
                    {
                        var contractor = db.Contractors.Include(f => f.ContractorType).FirstOrDefault(p => p.ContractorID == appUser.ContractorId);
                        if (contractor != null)
                            appUser.ContractorTypeName = contractor.ContractorType.ContractorTypeName;
                    }
            }
            return appUser;
        }

        #region Refresh Intervals and Application Settings

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetLiveIncidentRefreshInterval()
        {
            return Json(Utilities.GetApplicationSettingValue("LiveIncidentsRefreshRate"), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetTruckRefreshRate()
        {
            return Json(Utilities.GetApplicationSettingValue("TruckRefreshRate"), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetAlertsRefreshRate()
        {
            return Json(Utilities.GetApplicationSettingValue("AlertsRefreshRate"), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetAssistEntryUrl()
        {
            return Json(Utilities.GetApplicationSettingValue("AssistEntryUrl"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Autocompletes

        public ActionResult GetDirections()
        {
            var directions = new List<string>();
            directions.Add("N");
            directions.Add("S");
            directions.Add("W");
            directions.Add("E");

            return Json(directions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFreewaysByDirection(string name_startsWith)
        {
            using (var db = new MTCDBEntities())
            {
                var freeways = from q in db.Freeways
                               where q.FreewayName.StartsWith(name_startsWith)
                               orderby q.FreewayName
                               select q.FreewayName;

                return Json(freeways.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///     Make this later role depending
        /// </summary>
        /// <param name="name_startsWith"></param>
        /// <returns></returns>
        public ActionResult GetLocations(string name_startsWith)
        {
            using (var db = new MTCDBEntities())
            {
                var locations = from q in db.Locations
                                where q.Location1.StartsWith(name_startsWith)
                                orderby q.Location1
                                select q.LocationCode;

                return Json(locations.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Dropdowns

        [OutputCache(Duration = 60)]
        public ActionResult GetDrivers()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.Drivers
                           select new
                           {
                               Id = q.DriverID,
                               Text = q.LastName + ", " + q.FirstName
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetCallSigns()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.MTCBeatsCallSigns
                           select new
                           {
                               Id = q.BeatID,
                               Text = q.CallSign
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetAlarmTypes()
        {
            using (var dbc = new MTCDbContext())
            {
                var data = (from q in dbc.Violations
                            select new
                            {
                                Id = q.AlarmName,
                                Text = q.AlarmName
                            }).Where(a => a.Text != null).Distinct().OrderBy(AlarmName => AlarmName);
                ;

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetContractors(string contractorTypeName)
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.Contractors
                           select new
                           {
                               Id = q.ContractorID,
                               Text = q.ContractCompanyName,
                               Type = q.ContractorType.ContractorTypeName
                           };

                if (!string.IsNullOrEmpty(contractorTypeName))
                    data = data.Where(p => p.Type.StartsWith(contractorTypeName));

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupProvidingContractors()
        {
            using (var db = new MTCDBEntities())
            {
                var contractors = db.Contractors.ToList();

                using (var dbNew = new MTCDbContext())
                {
                    var backupProviders = dbNew.BackupProviders.ToList();

                    var data = from p in backupProviders
                               join c in contractors on p.ContractorId equals c.ContractorID
                               select new
                               {
                                   Id = c.ContractorID,
                                   Text = c.ContractCompanyName,
                                   Type = c.ContractorType.ContractorTypeName
                               };

                    return Json(data.OrderBy(c => c.Text).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetVehicles()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.FleetVehicles
                           select new
                           {
                               Id = q.FleetVehicleID,
                               Text = q.VehicleNumber
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [OutputCache(Duration = 60)]
        public ActionResult GetCHPOfficers()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.CHPOfficers
                           select new
                           {
                               q.Id,
                               Text = q.OfficerLastName + ", " + q.OfficerFirstName
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetContractorVehicles(Guid contractorId)
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.FleetVehicles
                           where q.ContractorID == contractorId
                           select new
                           {
                               Id = q.FleetVehicleID,
                               Text = q.VehicleNumber
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetContractorDrivers(Guid contractorId)
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.Drivers
                           where q.ContractorID == contractorId
                           select new
                           {
                               Id = q.DriverID,
                               Text = q.LastName + ", " + q.FirstName
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetContractorBeats(Guid contractorId)
        {
            using (var db = new MTCDBEntities())
            {
                var data = (from q in db.BeatDatas
                            join b in db.Contracts on q.ID equals b.BeatId
                            where b.ContractorID == contractorId
                            select new BeatViewModel2
                            {
                                Id = q.ID,
                                Text = q.BeatName
                            }).ToList();

                foreach (var item in data)
                    item.Value = Convert.ToInt32(item.Text);

                return Json(data.OrderBy(p => p.Value).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetBeatNumbers()
        {
            try
            {
                using (var db = new MTCDBEntities())
                {
                    var data = db.BeatDatas.ToList().Select(b => new BeatViewModel2
                    {
                        Id = b.ID,
                        Text = b.BeatID,
                        Value = Convert.ToInt32(b.BeatID)
                    });

                    return Json(data.OrderBy(p => p.Value).ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [OutputCache(Duration = 60)]
        public ActionResult GetCallSignsByBeatId(int beatId)
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.MTCBeatsCallSigns
                           where q.BeatID == beatId
                           select new
                           {
                               Id = q.BeatID,
                               Text = q.CallSign
                           };

                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetIncidentTypes()
        {
            using (var db = new MTCDBEntities())
            {
                //var data = from q in db.IncidentTypes
                //           select new
                //           {
                //               Id = q.IncidentTypeID,
                //               Text = q.IncidentType1
                //           };
                //return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);

                var data = new List<string>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetTrafficSpeeds()
        {
            using (var db = new MTCDBEntities())
            {
                //var data = from q in db.TrafficSpeeds
                //           select new
                //           {
                //               Id = q.TrafficSpeedID,
                //               Text = q.TrafficSpeed1
                //           };
                //return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);

                var data = new List<string>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetFreeways()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.Freeways
                           select new
                           {
                               Id = q.FreewayID,
                               Text = q.FreewayName
                           };
                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetBeatsFreewaysByBeat(string beatNumber)
        {
            using (var db = new MTCDBEntities())
            {
                var BN = new Guid(beatNumber);
                var data = from q in db.BeatsFreeways
                           where q.BeatID == BN
                           select new
                           {
                               Id = q.FreewayID,
                               Text = q.FreewayID
                           };


                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetVehiclePositions()
        {
            using (var db = new MTCDBEntities())
            {
                //var data = from q in db.VehiclePositions
                //           select new
                //           {
                //               Id = q.VehiclePositionID,
                //               Text = q.VehiclePosition1
                //           };
                //return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetVehicleTypes()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.VehicleTypeLUs
                           select new
                           {
                               q.Id,
                               Text = q.Name + " (" + q.Code + ")",
                               q.Code
                           };
                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetTowLocations()
        {
            using (var db = new MTCDBEntities())
            {
                //var data = from q in db.TowLocations
                //           select new
                //           {
                //               Id = q.TowLocationID,
                //               Text = q.TowLocationCode + " (" + q.TowLocation1 + ")"
                //           };
                //return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);

                var data = new List<string>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetServiceTypes()
        {
            using (var db = new MTCDBEntities())
            {
                //var data = from q in db.ServiceTypes
                //           select new
                //           {
                //               Id = q.ServiceTypeID,
                //               Text = q.ServiceTypeCode + " (" + q.ServiceType1 + ")"
                //           };
                //return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);

                var data = new List<string>();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 60)]
        public ActionResult GetBeatSchedules()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from q in db.BeatSchedules
                           select new
                           {
                               Id = q.BeatScheduleID,
                               Text = q.ScheduleName
                           };
                return Json(data.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region helpers

        protected List<BeatViewModel> GetBeatList()
        {
            using (var db = new MTCDBEntities())
            {
                var beats = from b in db.BeatDatas
                            select new BeatViewModel
                            {
                                BeatId = b.ID,
                                BeatNumber = b.BeatName
                            };
                return beats.ToList();
            }
        }

        protected List<ContractorViewModel> GetContractorList()
        {
            using (var db = new MTCDBEntities())
            {
                var contractors = from c in db.Contractors
                                  select new ContractorViewModel
                                  {
                                      ContractorId = c.ContractorID,
                                      ContractorCompanyName = c.ContractCompanyName,
                                      Email = c.Email,
                                      ContactName = c.ContactLastName + ", " + c.ContactFirstName,
                                      Phone = c.OfficeTelephone
                                  };
                return contractors.ToList();
            }
        }

        protected List<VehicleViewModel> GetVehicleList()
        {
            using (var db = new MTCDBEntities())
            {
                var vehicles = from c in db.FleetVehicles
                               select new VehicleViewModel
                               {
                                   FleetVehicleId = c.FleetVehicleID,
                                   VehicleMake = c.VehicleMake,
                                   VehicleModel = c.VehicleModel,
                                   VehicleNumber = c.VehicleNumber
                               };
                return vehicles.ToList();
            }
        }

        protected List<DriverViewModel> GetDriverList()
        {
            using (var db = new MTCDBEntities())
            {
                var drivers = from c in db.Drivers
                              select new DriverViewModel
                              {
                                  DriverId = c.DriverID,
                                  DriverFullName = c.FirstName + " " + c.LastName,
                                  FSPIDNumber = c.FSPIDNumber
                              };
                return drivers.ToList();
            }
        }

        protected List<CHPOfficerViewModel> GetCHPOfficerList()
        {
            using (var db = new MTCDBEntities())
            {
                var officers = from c in db.CHPOfficers
                               select new CHPOfficerViewModel
                               {
                                   Id = c.Id,
                                   OfficeFullName = c.OfficerLastName + ", " + c.OfficerFirstName
                               };
                return officers.ToList();
            }
        }

        public ActionResult GetViolationDetails(int vid)
        {
            var vd = new ViolationDetail();
            using (var dbc = new MTCDbContext())
            {
                var violation = dbc.Violations.FirstOrDefault(p => p.Id == vid);

                if (violation == null)
                {
                    return Json(vd, JsonRequestBehavior.AllowGet);
                }

                vd.Callsign = violation.CallSign;
                vd.Status = violation.ViolationStatusType.Name;
                vd.OffenseNumb = violation.OffenseNumber;
                vd.Deduction = violation.DeductionAmount;
                vd.Notes = violation.Notes;
                vd.BeatId = new Guid(violation.BeatId.ToString());
                vd.DriverId = new Guid(violation.DriverId.ToString());
                vd.VehicleId = new Guid(violation.FleetVehicleId.ToString());
                vd.Severity = Enum.GetName(typeof(ViolationTypeSeverity), violation.ViolationType.ViolationTypeSeverity);
                vd.LengthOfViolation = violation.LengthOfViolation;
            }

            using (var db = new MTCDBEntities())
            {
                var dbDriver = db.Drivers.FirstOrDefault(p => p.DriverID == vd.DriverId);

                if (dbDriver != null)
                    vd.Driver = $"{dbDriver.FirstName} {dbDriver.LastName}";

                var beat = (from b in db.BeatDatas
                            where b.ID == vd.BeatId
                            select b.BeatName).FirstOrDefault();

                var vehicle = (from veh in db.FleetVehicles
                               where veh.FleetVehicleID == vd.VehicleId
                               select veh.FleetNumber).FirstOrDefault();

                vd.Beat = beat;
                vd.FleetVehicle = vehicle;
            }

            return Json(vd, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}