using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Helpers;
using FSP.Web.Hubs;
using FSP.Web.Models;
using Microsoft.SqlServer.Types;
using FSP.Web.Filters;
using Microsoft.AspNet.SignalR;
using FSP.Web.TowTruckServiceRef;
using System.Web.Caching;
using System.Diagnostics;
using System.Configuration;

namespace FSP.Web.Controllers
{
    [CustomAuthorization]
    public class TruckController : MyController
    {
        public ActionResult Map()
        {
            return View();
        }
        public ActionResult Grid()
        {
            return View();
        }

        //[OutputCache(CacheProfile = "TruckList")]
        public ActionResult UpdateAllTrucks()
        {
            List<UITowTruck> towTrucks = new List<UITowTruck>();
            List<TruckState> truckStates = this.GetTruckStates();

            try
            {
                if (HttpContext.Cache["Trucks"] != null)
                {
                    towTrucks = (List<UITowTruck>)HttpContext.Cache["Trucks"];
                    Debug.WriteLine("Returning CACHED truck list: " + DateTime.Now);
                }
                else
                {
                    using (TowTruckServiceClient service = new TowTruckServiceClient())
                    {
                        TowTruckData[] serviceTowTrucks = service.CurrentTrucks();

                        for (int i = 0; i < serviceTowTrucks.Length; i++)
                        {
                            #region
                            TowTruckData serviceTowTruck = serviceTowTrucks[i];

                            UITowTruck truck = new UITowTruck();

                            try
                            {
                                truck.LastUpdate = 0;
                                truck.Old = false;

                                #region Set Truck Properties

                                truck.TruckNumber = serviceTowTruck.TruckNumber == null ? "Not set" : serviceTowTruck.TruckNumber;
                                truck.BeatNumber = serviceTowTruck.BeatNumber == null ? "Not set" : serviceTowTruck.BeatNumber;
                                truck.BeatSegmentNumber = "Not set";

                                truck.Speed = serviceTowTruck.Speed;
                                truck.Lat = serviceTowTruck.Lat;
                                truck.Lon = serviceTowTruck.Lon;
                                truck.Heading = serviceTowTruck.Heading;

                                //truck.UserContractorName = this.UsersContractorCompanyName; //the logged in user's contractor name association
                                truck.DriverName = serviceTowTruck.DriverName;
                                truck.ContractorName = serviceTowTruck.ContractorName;
                                truck.Location = serviceTowTruck.OutOfBoundsMessage;

                                if (serviceTowTruck.SpeedingTime.ToString() != "1/1/0001 12:00:00 AM" && serviceTowTruck.SpeedingTime.ToString() != "1/1/2001 12:00:00 AM")
                                    truck.SpeedingTime = serviceTowTruck.SpeedingTime.ToString("hh:mm:ss tt");

                                truck.SpeedingValue = serviceTowTruck.SpeedingValue.ToString();
                                truck.OutOfBoundsMessage = serviceTowTruck.OutOfBoundsMessage;

                                if (serviceTowTruck.OutOfBoundsTime.ToString() != "1/1/0001 12:00:00 AM")
                                    truck.OutOfBoundsTime = serviceTowTruck.OutOfBoundsTime.ToString("hh:mm:ss tt");
                                truck.HasAlarm = serviceTowTruck.Alarms;

                                if (serviceTowTruck.LastMessage.ToString() != "1/1/0001 12:00:00 AM")
                                    truck.LastMessage = serviceTowTruck.LastMessage.ToString("hh:mm:ss");

                                if (serviceTowTruck.StatusStarted.ToString() != "1/1/0001 12:00:00 AM")
                                    truck.LastStatusChanged = serviceTowTruck.StatusStarted.ToString("hh:mm:ss tt");


                                truck.VehicleState = serviceTowTruck.VehicleState;
                                truck.VehicleStateIconUrl = truckStates.Where(p => p.TruckState1 == truck.VehicleState).FirstOrDefault().TruckIcon;

                                if (serviceTowTruck.Alarms && String.IsNullOrEmpty(truck.VehicleState))
                                {
                                    //All alarms red (OnAlarm.png) expect speeding is yellow
                                    truck.VehicleStateIconUrl = "Alarm.png"; //Red

                                    if (serviceTowTruck.SpeedingAlarm)
                                        truck.VehicleStateIconUrl = "Speeding.png"; //Yellow
                                }

                                #endregion

                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message, "Error.txt");
                            }

                            serviceTowTruck = null;
                            towTrucks.Add(truck);
                            truck = null;

                            #endregion
                        }

                        int refreshRate = 0;
                        Int32.TryParse(ConfigurationManager.AppSettings["ServerRefreshRate"].ToString(), out refreshRate);
                        HttpContext.Cache.Add("Trucks", towTrucks, null, DateTime.Now.AddSeconds(refreshRate), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                        serviceTowTrucks = null;
                        Debug.WriteLine("Returning FRESH truck list: " + DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message, "Error.txt");
            }

            //check to see if current user is a contractor. Then filter to only "his/her" trucks"
            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                towTrucks = towTrucks.Where(p => p.ContractorName == this.UsersContractorCompanyName).ToList();

            return Json(towTrucks.OrderBy(p => p.BeatNumber).ThenBy(p => p.TruckNumber), JsonRequestBehavior.AllowGet);

        }

        public List<TruckState> GetTruckStates()
        {
            List<TruckState> returnList = new List<TruckState>();

            if (HttpContext.Cache["truckStates"] == null)
            {
                using (FSPDataContext dc = new FSPDataContext())
                {
                    returnList = dc.TruckStates.ToList();
                }

                HttpContext.Cache.Insert("truckStates",
                                          returnList,
                                          null,
                                          Cache.NoAbsoluteExpiration,
                                          TimeSpan.FromMinutes(60));
            }
            else
            {
                returnList = (List<TruckState>)HttpContext.Cache["truckStates"];
            }

            return returnList;
        }

        [OutputCache(Duration = 10), AllowAnonymous]
        public ActionResult HaveAlarms()
        {
            bool returnValue = false;
            using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
            {
                var alarms = service.GetAllAlarms().Where(p =>
                    p.RollInAlarm == true ||
                    p.RollOutAlarm == true ||
                    p.IncidentAlarm == true ||
                    p.LogOffAlarm == true ||
                    p.LogOnAlarm == true ||
                    p.GPSIssueAlarm == true ||
                    p.SpeedingAlarm == true ||
                    p.OutOfBoundsAlarm == true ||
                    p.StationaryAlarm == true).ToList();

                if (alarms.Any())
                {
                    //if (alarms.Any(p => p.RollInAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.RollInAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.RollOutAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.RollOutAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.IncidentAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.IncidentAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.LogOffAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.LogOffAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.LogOnAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.LogOnAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.OnPatrolAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001" && p.OnPatrolAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.GPSIssueAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001" && p.GPSIssueAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;
                    //else if (alarms.Any(p => p.StationaryAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001" && p.StationaryAlarmCleared.ToString("MM/dd/yyyy") != "01/01/2001"))
                    //    returnValue = true;

                    if (alarms.Any(p => p.RollInAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.RollOutAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.IncidentAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.LogOffAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.LogOnAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.OnPatrolAlarmTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.GPSIssueAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.StationaryAlarmStart.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.SpeedingTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                    else if (alarms.Any(p => p.OutOfBoundsStartTime.ToString("MM/dd/yyyy") != "01/01/2001"))
                        returnValue = true;
                }
            }

            return Json(returnValue, JsonRequestBehavior.AllowGet);

        }

        #region Other Calls

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetDropZones()
        {
            List<UIDropZone> returnList = new List<UIDropZone>();
            try
            {

                using (FSPDataContext dc = new FSPDataContext())
                {
                    var dropZones = dc.vDropZones.ToList();

                    foreach (var dropZone in dropZones)
                    {
                        SqlGeography geo = new SqlGeography();
                        geo = SqlGeography.Parse(dropZone.Position);

                        List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                        for (int i = 1; i < geo.STNumPoints(); i++)
                        {
                            try
                            {
                                SqlGeography point = geo.STPointN(i);
                                Double lat = Convert.ToDouble(point.Lat.ToString());
                                Double lon = Convert.ToDouble(point.Long.ToString());
                                linePoints.Add(new UIMapPolygonLinePoint
                                {
                                    Lat = lat,
                                    Lon = lon
                                });
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message, "Error.txt");
                                Util.WriteToEventLog(ex.Message);
                            }

                        }

                        returnList.Add(new UIDropZone
                        {
                            DropZoneID = dropZone.DropZoneID,
                            DropZoneNumber = dropZone.DropZoneNumber,
                            Comments = dropZone.Comments,
                            DropZoneDescription = dropZone.DropZoneDescription,
                            PolygonPoints = linePoints
                        });
                    }
                }



            }
            catch { }
            Debug.WriteLine("Drop zones loaded: " + DateTime.Now);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetCallBoxes()
        {
            List<UICallBox> returnList = new List<UICallBox>();
            try
            {
                using (FSPDataContext dc = new FSPDataContext())
                {
                    var callBoxes = dc.vCallBoxes;

                    foreach (var callBox in callBoxes)
                    {
                        SqlGeography geo = new SqlGeography();
                        geo = SqlGeography.Parse(callBox.Position);

                        Double lat = Convert.ToDouble(geo.STPointN(1).Lat.ToString());
                        Double lon = Convert.ToDouble(geo.STPointN(1).Long.ToString());

                        returnList.Add(new UICallBox
                        {
                            CallBoxId = callBox.CallBoxID,
                            FreewayId = callBox.FreewayID,
                            Lat = lat,
                            Lon = lon,
                            LocationDescription = callBox.Location,
                            TelephoneNumber = callBox.TelephoneNumber,
                            SignNumber = callBox.SignNumber,
                            SiteType = callBox.SiteType,
                            Comments = callBox.Comments
                        });
                    }
                }


            }
            catch { }
            Debug.WriteLine("Call boxes loaded: " + DateTime.Now);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetBeats()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();

            try
            {
                using (FSPDataContext dc = new FSPDataContext())
                {
                    var beatsQuery = (from q in dc.vBeats
                                      where q.Active == true
                                      select q).ToList();

                    foreach (var beat in beatsQuery)
                    {
                        SqlGeography geo = new SqlGeography();
                        geo = SqlGeography.Parse(beat.BeatExtent.ToString());


                        List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                        for (int i = 1; i < geo.STNumPoints(); i++)
                        {
                            try
                            {
                                SqlGeography point = geo.STPointN(i);
                                Double lat = Convert.ToDouble(point.Lat.ToString());
                                Double lon = Convert.ToDouble(point.Long.ToString());
                                linePoints.Add(new UIMapPolygonLinePoint
                                {
                                    Lat = lat,
                                    Lon = lon
                                });
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message, "Error.txt");
                                Util.WriteToEventLog(ex.Message);
                            }

                        }

                        returnList.Add(new UIMapPolygonLine
                        {
                            Number = beat.BeatNumber.Substring(beat.BeatNumber.Length - 3, 3),
                            Description = beat.BeatNumber + ": " + beat.BeatDescription,
                            Points = linePoints,
                            Color = beat.BeatColor
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message, "Error.txt");
                Util.WriteToEventLog(ex.Message);
            }
            Debug.WriteLine("Beats loaded: " + DateTime.Now);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetBeatNumbers(String name_startsWith)
        {
            using (FSPDataContext dc = new FSPDataContext())
            {
                var beatsQuery = from q in dc.vBeats
                                 where q.Active == true && q.BeatNumber.Contains(name_startsWith)
                                 orderby q.BeatNumber
                                 select new
                                 {
                                     //Number = q.BeatNumber,
                                     Number = q.BeatNumber.Substring(q.BeatNumber.Length - 3, 3),
                                     Description = q.BeatDescription
                                 };

                Debug.WriteLine("Beat numbers loaded: " + DateTime.Now);
                return Json(beatsQuery, JsonRequestBehavior.AllowGet);
            }

        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetTowTruckContractors(String name_startsWith)
        {
            using (FSPDataContext dc = new FSPDataContext())
            {
                var contractorQuery = from q in dc.Contractors
                                      where q.ContractCompanyName.Contains(name_startsWith)
                                      orderby q.ContractCompanyName
                                      select new
                                      {
                                          Number = q.ContractCompanyName,
                                          Description = q.ContractCompanyName
                                      };
                Debug.WriteLine("Tow Truck Contractors: " + DateTime.Now);
                return Json(contractorQuery.ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        [OutputCache(CacheProfile = "OtherDataCalls")]
        public ActionResult GetBeatSegments()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();

            using (FSPDataContext dc = new FSPDataContext())
            {
                try
                {
                    var beatsQuery = (from q in dc.vBeatSegments
                                      select q).ToList();

                    foreach (var beat in beatsQuery)
                    {
                        SqlGeography geo = new SqlGeography();
                        geo = SqlGeography.Parse(beat.BeatSegmentExtent.ToString());


                        List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                        for (int i = 1; i < geo.STNumPoints(); i++)
                        {
                            try
                            {
                                SqlGeography point = geo.STPointN(i);
                                Double lat = Convert.ToDouble(point.Lat.ToString());
                                Double lon = Convert.ToDouble(point.Long.ToString());
                                linePoints.Add(new UIMapPolygonLinePoint
                                {
                                    Lat = lat,
                                    Lon = lon
                                });
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message, "Error.txt");
                                Util.WriteToEventLog(ex.Message);
                            }

                        }

                        returnList.Add(new UIMapPolygonLine
                        {
                            Number = beat.BeatSegmentNumber,
                            Description = beat.BeatSegmentNumber + ": " + beat.BeatSegmentDescription,
                            Points = linePoints
                        });
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteToLog(ex.Message, "Error.txt");
                    Util.WriteToEventLog(ex.Message);
                }
            }


            Debug.WriteLine("Beats segments loaded: " + DateTime.Now);
            return Json(returnList, JsonRequestBehavior.AllowGet);
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
