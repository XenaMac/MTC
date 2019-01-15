using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using FSP.Web.Helpers;
using Microsoft.SqlServer.Types;
using FSP.Domain.Model;
using FSP.Web.Hubs;
using System.Timers;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;

namespace FSP.Web.Controllers
{
    //[Authorize] //any logged in user role can see the map
    public class MapController : Helpers.ControllerWithHub<TowTruckHub>
    {
        #region Variables
        private FSPDataContext dc = new FSPDataContext();

        private static List<UITowTruck> towTrucks;
        private static Timer timer = null;
        private static DateTime currentUpdateTime = DateTime.Now;
        #endregion

        #region Views
        /// <summary>
        /// Simply return view
        /// </summary>
        /// <returns></returns>
        public ActionResult Map()
        {
            return View();
        }
        public ActionResult Grid()
        {
            return View();
        }

        #endregion

        #region Main
        public ActionResult StartTruckUpdate()
        {
            if (timer == null)
            {
                timer = new Timer();
                timer.Elapsed += timer_Elapsed;
                timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateIntervalInMilliseconds"]);
                timer.Start();
            }

            if (towTrucks == null)
            {
                //only of first call instantiate these objects. Any subsequent calls just register to hub and will be updated
                towTrucks = new List<UITowTruck>();

                if (ConfigurationManager.AppSettings["IsDemo"] == "true")
                    this.RunDemo();
                else
                    this.UpdateAllTrucks();
            }
            else
            {
                //update all to update new UI
                foreach (var towTruck in towTrucks)
                {
                    // Notify the connected clients
                    Hub.Clients.addOrUpdateTruck(towTruck);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Util.WriteToLog(DateTime.Now + ": Timer Tick ==================================================================");
            currentUpdateTime = DateTime.Now;

            if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            {
                iCounter += 1;
                RunDemo();
            }
            else
            {
                this.UpdateAllTrucks();
            }


        }

        #region Demo System

        int iCounter = 0;
        void RunDemo()
        {
            try
            {
                Random ran = new Random();
                int iTruckUpdatesCount = 0;
                int iTruckDeleteCount = 0;

                lock (towTrucks)
                {
                    int NewTruckId = ran.Next(1, 15);

                    if (towTrucks.Any(p => p.TruckNumber == NewTruckId.ToString()))
                    {
                        #region update
                        UITowTruck truck = towTrucks.Single(p => p.TruckNumber == NewTruckId.ToString());
                        truck.Speed = ran.NextDouble() * 100;
                        //add a degree to heading
                        truck.Heading = truck.Heading + 10;
                        truck.LastUpdate += ran.Next(0, 20);

                        iTruckUpdatesCount += 1;

                        truck.Lat = truck.Lat + ran.Next(0, 1000) / 1000;
                        truck.Lon = truck.Lon + ran.Next(0, 1000) / 1000;

                        if (truck.Lat < 33.6539 || truck.Lat > 33.7939)
                            truck.Lat = 33.7319;

                        if (truck.Lon < -117.9597 || truck.Lon > -117.6597)
                            truck.Lon = -117.7927;

                        // Notify the connected clients
                        Hub.Clients.addOrUpdateTruck(truck);

                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Updating Truck " + truck.TruckNumber);

                        #endregion
                    }
                    else
                    {
                        #region add

                        iTruckUpdatesCount += 1;

                        UITowTruck truck = new UITowTruck();
                        truck.Old = false;
                        truck.TruckNumber = NewTruckId.ToString();
                        truck.VehicleStateIconUrl = "NIS.png";
                        truck.VehicleState = "Waiting for Driver to Login in";
                        truck.Heading = 20;
                        truck.BeatNumber = ran.Next().ToString();
                        truck.BeatSegmentNumber = ran.Next().ToString();
                        truck.ContractorId = 1;
                        truck.Lat = 33.7319 + ran.Next(0, 1000) / 1000;
                        truck.Lon = -117.7927 + ran.Next(0, 1000) / 1000;
                        truck.Speed = 25;
                        truck.LastUpdate = 0;

                        towTrucks.Add(truck);

                        // Notify the connected clients
                        Hub.Clients.addOrUpdateTruck(truck);

                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Adding Truck " + truck.TruckNumber);

                        #endregion
                    }


                    #region remove

                    var removeItem = false;

                    //removeItem = NewId % 2 == 0;
                    removeItem = iCounter % 20 == 0;

                    if (removeItem)
                    {
                        UITowTruck truck = towTrucks.FirstOrDefault();
                        truck.Old = true;
                        //towTrucks2.Remove(truck);

                        Hub.Clients.deleteTruck(truck.TruckNumber);
                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Removing Truck " + truck.TruckNumber);

                        towTrucks.RemoveAll(p => p.Old == true);
                    }

                    #endregion
                }


                System.Diagnostics.Debug.WriteLine("================Server: Total Number of trucks in local list: " + towTrucks.Count());
                System.Diagnostics.Debug.WriteLine("================Server: Total Number of trucks updated: " + iTruckUpdatesCount);
                System.Diagnostics.Debug.WriteLine("================Server: Total Number of trucks deleted: " + iTruckDeleteCount);

                iCounter += 1;
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
            }
        }

        #endregion

        void UpdateAllTrucks()
        {
            UITowTruck truck = null;


            int iTruckUpdatesCount = 0;
            int iTruckDeleteCount = 0;

            try
            {
                Util.WriteToLog(DateTime.Now + " Contacting Service");

                TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient();
                TowTruckData[] serviceTowTrucks = service.CurrentTrucks();
                Util.WriteToLog(DateTime.Now + " " + serviceTowTrucks.Count() + " tow trucks retrieved from Service");

                Boolean addTruck = false;
                Boolean truckHasChanged = false;

                lock (towTrucks)
                {
                    for (int i = 0; i < serviceTowTrucks.Length; i++)
                    {
                        try
                        {
                            TowTruckData serviceTowTruck = serviceTowTrucks[i];


                            //check if this tow truck already exist in list
                            if (towTrucks.Any(p => p.TruckNumber.ToString() == serviceTowTruck.TruckNumber))
                            {
                                truck = towTrucks.Single(p => p.TruckNumber == serviceTowTruck.TruckNumber);
                                addTruck = false;
                                iTruckUpdatesCount += 1;

                                if (truck.Lat != serviceTowTruck.Lat)
                                    truckHasChanged = true;

                                if (truck.Lon != serviceTowTruck.Lon)
                                    truckHasChanged = true;

                                if (truck.Speed != serviceTowTruck.Speed)
                                    truckHasChanged = true;

                                if (truck.Heading != serviceTowTruck.Heading)
                                    truckHasChanged = true;

                                if (truck.Heading != serviceTowTruck.Heading)
                                    truckHasChanged = true;

                                if (truck.BeatNumber != serviceTowTruck.BeatNumber.ToString())
                                    truckHasChanged = true;

                                if (truck.LastMessage != serviceTowTruck.LastMessage.ToString())
                                    truckHasChanged = true;

                                if (truck.DriverName != serviceTowTruck.DriverName.ToString())
                                    truckHasChanged = true;

                                if (truck.ContractorName != serviceTowTruck.ContractorName.ToString())
                                    truckHasChanged = true;

                                if (truck.Location != serviceTowTruck.Location.ToString())
                                    truckHasChanged = true;
                                
                            }
                            else
                            {
                                //new tow truck
                                iTruckUpdatesCount += 1;
                                truck = new UITowTruck();
                                truck.LastUpdate = 0;
                                truck.Old = false;
                                addTruck = true;
                            }

                            truck.TruckNumber = serviceTowTruck.TruckNumber == null ? "Not set" : serviceTowTruck.TruckNumber;
                            truck.BeatNumber = serviceTowTruck.BeatNumber == null ? "Not set" : serviceTowTruck.BeatNumber;
                            truck.BeatSegmentNumber = "Not set";

                            truck.Speed = serviceTowTruck.Speed;
                            truck.Lat = serviceTowTruck.Lat;
                            truck.Lon = serviceTowTruck.Lon;
                            truck.Heading = serviceTowTruck.Heading;
                            truck.LastMessage = serviceTowTruck.LastMessage.ToString();
                            
                            truck.DriverName = serviceTowTruck.DriverName;
                            truck.ContractorName = serviceTowTruck.ContractorName;
                            truck.Location = serviceTowTruck.Location;


                            #region State

                            truck.VehicleState = serviceTowTruck.VehicleState;

                            if (serviceTowTruck.VehicleState == "Waiting for Driver Login")
                                truck.VehicleStateIconUrl = "NIS.png";
                            else if (serviceTowTruck.VehicleState == "Driver Logged On")
                                truck.VehicleStateIconUrl = "InService.png";
                            else if (serviceTowTruck.VehicleState == "On Patrol")
                                truck.VehicleStateIconUrl = "InService.png";
                            else if (serviceTowTruck.VehicleState == "On Assist")
                                truck.VehicleStateIconUrl = "OnAssist.png";
                            else if (serviceTowTruck.VehicleState == "On Break")
                                truck.VehicleStateIconUrl = "OnBreak.png";
                            else if (serviceTowTruck.Alarms)
                            {
                                if (serviceTowTruck.SpeedingAlarm)
                                    truck.VehicleStateIconUrl = "MajorAlarm.png";
                                else if (serviceTowTruck.OutOfBoundsAlarm)
                                    truck.VehicleStateIconUrl = "MajorAlarm.png";
                            }

                            else
                                truck.VehicleStateIconUrl = "OCTA.png";
                            #endregion


                            if (addTruck)
                            {
                                towTrucks.Add(truck);

                                //Notify clients about new truck
                                Hub.Clients.addOrUpdateTruck(truck);
                            }
                            else
                            {
                                if (truckHasChanged)
                                {
                                    Util.WriteToLog("HUB: Truck " + truck.TruckNumber + " changed ");

                                    truck.LastUpdate = 0;
                                    //Notify clients about an existing truck only if truck has "changed"
                                    Hub.Clients.addOrUpdateTruck(truck);
                                }
                                else
                                {
                                    Util.WriteToLog("HUB: NO CHANGES TO " + truck.TruckNumber);

                                    truck.LastUpdate += Convert.ToInt32(ConfigurationManager.AppSettings["UpdateIntervalInMilliseconds"]) / 1000;

                                    //Notify clients about an existing truck only if truck has "changed"
                                    Hub.Clients.updateLastUpdateTime(truck.TruckNumber, truck.LastUpdate, truck.LastMessage);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                        }


                    }


                    #region remove

                    try
                    {
                        for (int i = 0; i < towTrucks.Count(); i++)
                        {
                            try
                            {
                                UITowTruck uiTowTruck = towTrucks[i];

                                if (!serviceTowTrucks.Any(p => p.TruckNumber == uiTowTruck.TruckNumber.ToString()))
                                {
                                    iTruckDeleteCount += 1;
                                    uiTowTruck.Old = true;
                                    //this ui tow truck is not in service any more. so remove it
                                    //towTrucks2.Remove(uiTowTruck);

                                    //Hub notification
                                    Hub.Clients.deleteTruck(uiTowTruck.TruckNumber);

                                }
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message);
                            }

                        }
                        towTrucks.RemoveAll(p => p.Old == true);
                    }
                    catch (Exception ex)
                    {
                        Util.WriteToLog(ex.Message);
                    }



                    #endregion

                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks in local list: " + towTrucks.Count());
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks updated: " + iTruckUpdatesCount);
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks deleted: " + iTruckDeleteCount);

                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
            }


        }

        #endregion

        #region Other Calls

        public ActionResult GetCallBoxes()
        {
            List<UICallBox> returnList = new List<UICallBox>();
            try
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
                        TelephoneNumber = callBox.TelephoneNumber
                    });
                }

            }
            catch { }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeats()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();


            try
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
                            Util.WriteToLog(ex.Message);
                            Util.WriteToEventLog(ex.Message);
                        }

                    }

                    returnList.Add(new UIMapPolygonLine
                    {
                        Number = beat.BeatNumber,
                        Description = beat.BeatNumber + ": " + beat.BeatDescription,
                        Points = linePoints
                    });
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
                Util.WriteToEventLog(ex.Message);
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatNumbers(String name_startsWith)
        {
            var beatsQuery = from q in dc.vBeats
                             where q.Active == true && q.BeatNumber.Contains(name_startsWith)
                             orderby q.BeatNumber
                             select new
                             {
                                 Number = q.BeatNumber,
                                 Description = q.BeatDescription
                             };

            return Json(beatsQuery, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatSegments()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();


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
                            Util.WriteToLog(ex.Message);
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
                Util.WriteToLog(ex.Message);
                Util.WriteToEventLog(ex.Message);
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfActiveBeats()
        {
            var beatsQuery = from q in dc.vBeats
                             where q.Active == true
                             select new
                             {
                                 BeatId = q.BeatID,
                                 BeatName = q.BeatDescription
                             };

            return Json(beatsQuery.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfActiveTowTruckContractors()
        {
            var contractorQuery = from q in dc.Contractors
                                  select new
                                  {
                                      ContractorID = q.ContractorID,
                                      ContractCompanyName = q.ContractCompanyName
                                  };

            return Json(contractorQuery.ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}
