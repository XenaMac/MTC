using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Web.TowTruckServiceRef;
using System.Configuration;
using FSP.Web.Helpers;
using Microsoft.SqlServer.Types;
using FSP.Domain.Model;
using FSP.Web.Hubs;
using System.Timers;

namespace FSP.Web.Controllers
{
    //[Authorize]
    public class MapController : Helpers.ControllerWithHub<TowTruckHub>
    {
        private FSPDataContext dc = new FSPDataContext();

        private static List<TowTruck> towTrucks;
        private static Timer timer = null;
        private static Timer startUpTimer = null;
        private static int updateIntervalInMilliseconds = 15000;
        private static DateTime currentUpdateTime = DateTime.Now;


        //this variable is used to give the UI time before accepting items
        private static int iCounter = 0;
        private static List<TowTruck2> towTrucks2;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }



        public ActionResult GetTowTrucks()
        {

            Random ran = new Random();
            TowTruck truck = null;

            try
            {
                if (ConfigurationManager.AppSettings["IsDemo"] == "true")
                {
                    #region Demo
                    Util.WriteToLog("Doing Tow Truck Demo...");

                    if (towTrucks == null)
                    {
                        //set up initial locations
                        towTrucks = new List<TowTruck>();

                        //#region Create a Two Truck for each State

                        //truck = new TowTruck();
                        //truck.TruckId = 123;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7319;
                        //truck.Lon = -117.7927;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "InService";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 124;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7619;
                        //truck.Lon = -117.8227;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "MajorAlarm";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 234;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7919;
                        //truck.Lon = -117.8527;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "MinorAlarm";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 235;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7939;
                        //truck.Lon = -117.7597;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "ModerateAlarm";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 345;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7939;
                        //truck.Lon = -117.8997;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "NIS";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 346;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.7939;
                        //truck.Lon = -117.9597;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "OCTA";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 456;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.8939;
                        //truck.Lon = -117.9597;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "OnAssist";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 457;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.6939;
                        //truck.Lon = -117.9597;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "OnBreak";
                        //towTrucks.Add(truck);

                        //truck = new TowTruck();
                        //truck.TruckId = 567;
                        //truck.Speed = ran.Next(0, 70);
                        //truck.Lat = 33.6939;
                        //truck.Lon = -117.6597;
                        //truck.Heading = ran.Next(0, 180);
                        //truck.LastUpdate = DateTime.Now;
                        //truck.State = "TBD";
                        //towTrucks.Add(truck);

                        //#endregion

                        Util.WriteToLog(towTrucks.Count() + " tow trucks created for demo.");
                    }
                    else
                    {
                        //we have a trucks in the list. Now update their location and data
                        foreach (var towTruck in towTrucks)
                        {
                            //set random speed
                            towTruck.Speed = ran.Next(0, 70);

                            //add a degree to heading
                            towTruck.Heading = towTruck.Heading + 1;

                            int number = ran.Next(0, 20);
                            if (number % 2 != 0)
                            {
                                towTruck.Lat = towTruck.Lat - 0.0010;
                                towTruck.Lon = towTruck.Lon + 0.0010;
                            }
                            else
                            {
                                towTruck.Lat = towTruck.Lat + 0.0010;
                                towTruck.Lon = towTruck.Lon - 0.0010;
                            }

                            if (towTruck.Lat < 33.6539 || towTruck.Lat > 33.7939)
                                towTruck.Lat = 33.7319;


                            if (towTruck.Lon < -117.9597 || towTruck.Lon > -117.6597)
                                towTruck.Lon = -117.7927;


                            towTruck.LastUpdate = DateTime.Now;

                        }

                        Util.WriteToLog(towTrucks.Count() + " tow trucks updated for demo.");
                    }


                    #endregion
                }
                else
                {
                    Util.WriteToLog("Contacting Service");

                    TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient();
                    TowTruckData[] serviceTowTrucks = service.CurrentTrucks();
                    Util.WriteToLog(serviceTowTrucks.Count() + " tow trucks retrieved from Service");


                    if (towTrucks == null)
                        towTrucks = new List<TowTruck>();


                    Boolean addTruck = false;


                    lock (towTrucks)
                    {
                        for (int i = 0; i < serviceTowTrucks.Length; i++)
                        {
                            TowTruckData serviceTowTruck = serviceTowTrucks[i];

                            if (towTrucks.Any(p => p.TruckId == serviceTowTruck.TruckID))
                            {
                                truck = towTrucks.Single(p => p.TruckId == serviceTowTruck.TruckID);
                                addTruck = false;
                            }
                            else
                            {
                                truck = new TowTruck();
                                addTruck = true;
                            }

                            truck.TruckId = serviceTowTruck.TruckID;
                            truck.Speed = serviceTowTruck.Speed;
                            truck.Lat = serviceTowTruck.Lat;
                            truck.Lon = serviceTowTruck.Lon;
                            truck.Heading = serviceTowTruck.Heading;
                            truck.LastUpdate = DateTime.Now;

                            #region State

                            if (serviceTowTruck.VehicleState == "Waiting for Driver Login")
                                truck.State = "NIS";


                            #endregion


                            if (addTruck)
                                towTrucks.Add(truck);
                        }
                    }

                }

            }
            catch { }

            return Json(towTrucks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeats()
        {
            List<MapPolygonLine> returnList = new List<MapPolygonLine>();


            try
            {
                var beatsQuery = (from q in dc.vBeats
                                  where q.Active == true
                                  select q).ToList();

                foreach (var beat in beatsQuery)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(beat.BeatExtent.ToString());


                    List<MapPolygonLinePoint> linePoints = new List<MapPolygonLinePoint>();
                    for (int i = 0; i < geo.STNumPoints(); i++)
                    {
                        try
                        {
                            SqlGeography point = geo.STPointN(i);
                            Double lat = Convert.ToDouble(point.Lat.ToString());
                            Double lon = Convert.ToDouble(point.Long.ToString());
                            linePoints.Add(new MapPolygonLinePoint
                            {
                                Lat = lat,
                                Lon = lon
                            });
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                        }

                    }

                    returnList.Add(new MapPolygonLine
                    {
                        Name = beat.BeatDescription,
                        Points = linePoints
                    });
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatSegments()
        {
            List<MapPolygonLine> returnList = new List<MapPolygonLine>();


            try
            {
                var beatsQuery = (from q in dc.vBeatSegments
                                  select q).ToList();

                foreach (var beat in beatsQuery)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(beat.BeatSegmentExtent.ToString());


                    List<MapPolygonLinePoint> linePoints = new List<MapPolygonLinePoint>();
                    for (int i = 0; i < geo.STNumPoints(); i++)
                    {
                        try
                        {
                            SqlGeography point = geo.STPointN(i);
                            Double lat = Convert.ToDouble(point.Lat.ToString());
                            Double lon = Convert.ToDouble(point.Long.ToString());
                            linePoints.Add(new MapPolygonLinePoint
                            {
                                Lat = lat,
                                Lon = lon
                            });
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                        }

                    }

                    returnList.Add(new MapPolygonLine
                    {
                        Name = beat.BeatSegmentDescription + " " + beat.BeatSegmentNumber,
                        Points = linePoints
                    });
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
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

        //new
        public MapController()
        {
            if (timer == null)
            {
                //set up initial locations
                towTrucks2 = new List<TowTruck2>();

                //this should only execute once on app start. timer is a static variable.
                Util.WriteToLog(DateTime.Now + ": Timer Started ==================================================================");

                startUpTimer = new Timer();
                startUpTimer.Elapsed += startUpTimer_Elapsed;
                startUpTimer.Interval = 10000;
                startUpTimer.Start();
            }

            iCounter = 0;
        }

        void startUpTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            startUpTimer.Stop();

            //this.GetAllTrains();


            if (timer == null)
            {
                //this should only execute once on app start. timer is a static variable.
                Util.WriteToLog(DateTime.Now + ": Timer Started ==================================================================");

                timer = new Timer();
                timer.Elapsed += timer_Elapsed;
                timer.Interval = updateIntervalInMilliseconds;
                timer.Start();
            }


        }

        public ActionResult Index2()
        {
            return View();
        }

        #region Demo System

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Util.WriteToLog(DateTime.Now + ": Timer Tick ==================================================================");
            currentUpdateTime = DateTime.Now;

            if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            {
                Demo2();
            }
            else
            {
                QueryService();
            }


        }
        void QueryService()
        {
            TowTruck2 truck = null;


            int iTruckUpdatesCount = 0;
            int iTruckDeleteCount = 0;

            try
            {
                Util.WriteToLog(DateTime.Now + " Contacting Service");

                TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient();
                TowTruckData[] serviceTowTrucks = service.CurrentTrucks();
                Util.WriteToLog(DateTime.Now + " " + serviceTowTrucks.Count() + " tow trucks retrieved from Service");

                Boolean addTruck = false;

                lock (towTrucks2)
                {
                    for (int i = 0; i < serviceTowTrucks.Length; i++)
                    {
                        TowTruckData serviceTowTruck = serviceTowTrucks[i];

                        if (towTrucks2.Any(p => p.TruckId.ToString() == serviceTowTruck.TruckID))
                        {
                            truck = towTrucks2.Single(p => p.TruckId.ToString() == serviceTowTruck.TruckID);
                            addTruck = false;
                            iTruckUpdatesCount += 1;
                        }
                        else
                        {
                            iTruckUpdatesCount += 1;
                            truck = new TowTruck2();
                            truck.Old = false;
                            addTruck = true;
                        }

                        int truckId;
                        Int32.TryParse(serviceTowTruck.TruckID, out truckId);

                        truck.TruckId = truckId;
                        truck.Speed = serviceTowTruck.Speed;
                        truck.Lat = serviceTowTruck.Lat;
                        truck.Lon = serviceTowTruck.Lon;
                        truck.Heading = serviceTowTruck.Heading;
                        truck.LastUpdate = "0 seconds ago";

                        #region State

                        truck.StateDescription = serviceTowTruck.VehicleState;

                        if (serviceTowTruck.VehicleState == "Waiting for Driver Login")
                            truck.StateImage = @"http://localhost/FSP.Web/Content/Images/NIS.png";

                        #endregion


                        if (addTruck)
                            towTrucks2.Add(truck);

                        // Notify the connected clients
                        Hub.Clients.addOrUpdateTruck(truck);

                    }


                    #region remove

                    try
                    {
                        for (int i = 0; i < towTrucks2.Count(); i++)
                        {
                            try
                            {
                                TowTruck2 uiTowTruck = towTrucks2[i];

                                if (!serviceTowTrucks.Any(p => p.TruckID == uiTowTruck.TruckId.ToString()))
                                {
                                    iTruckDeleteCount += 1;
                                    uiTowTruck.Old = true;
                                    //this ui tow truck is not in service any more. so remove it
                                    //towTrucks2.Remove(uiTowTruck);

                                    //Hub notification
                                    Hub.Clients.deleteTruck(uiTowTruck.TruckId);

                                }
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message);
                            }

                        }
                        towTrucks2.RemoveAll(p => p.Old == true);
                    }
                    catch (Exception ex)
                    {
                        Util.WriteToLog(ex.Message);
                    }



                    #endregion

                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks in local list: " + towTrucks2.Count());
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks updated: " + iTruckUpdatesCount);
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks deleted: " + iTruckDeleteCount);

                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
            }


        }

        void Demo1()
        {
            Random ran = new Random();
            System.Diagnostics.Debug.WriteLine(DateTime.Now + " Tick Count:" + iCounter);
            if (iCounter == 5)
            {
                TowTruck2 truck = null;

                truck = new TowTruck2();
                truck.TruckId = 123;
                truck.StateImage = @"http://localhost/FSP.Web/Content/Images/NIS.png";
                truck.StateDescription = "Waiting for Driver to Login in";
                truck.Heading = 20;
                truck.BeatId = 1;
                truck.ContractorId = 1;
                truck.Lat = 33.7319;
                truck.Lon = -117.7927;
                truck.Speed = 25;
                truck.LastUpdate = "3 seconds ago";

                towTrucks2.Add(truck);
                Hub.Clients.addOrUpdateTruck(truck);
                System.Diagnostics.Debug.WriteLine(DateTime.Now + ": Added truck: " + truck.TruckId);


                truck = new TowTruck2();
                truck.TruckId = 456;
                truck.StateImage = @"http://localhost/FSP.Web/Content/Images/MajorAlarm.png";
                truck.StateDescription = "Accident with fire truck";
                truck.Heading = 90;
                truck.BeatId = 1;
                truck.ContractorId = 1;
                truck.Lat = 33.6619;
                truck.Lon = -117.7527;
                truck.Speed = 25;
                truck.LastUpdate = "10 seconds ago";

                towTrucks2.Add(truck);
                Hub.Clients.addOrUpdateTruck(truck);
                System.Diagnostics.Debug.WriteLine(DateTime.Now + ": Added truck: " + truck.TruckId);


            }
            else if (iCounter > 5)
            {
                //we have a trucks in the list. Now update their location and data
                foreach (var truck in towTrucks2)
                {
                    //set random speed
                    truck.Speed = ran.Next(0, 70);

                    //add a degree to heading
                    truck.Heading = truck.Heading + 10;

                    int number = ran.Next(0, 20);
                    if (number % 2 != 0)
                    {
                        truck.Lat = truck.Lat - 0.0020;
                        truck.Lon = truck.Lon + 0.0020;
                    }
                    else
                    {
                        truck.Lat = truck.Lat + 0.0020;
                        truck.Lon = truck.Lon - 0.0020;
                    }

                    if (truck.Lat < 33.6539 || truck.Lat > 33.7939)
                        truck.Lat = 33.7319;


                    if (truck.Lon < -117.9597 || truck.Lon > -117.6597)
                        truck.Lon = -117.7927;

                    truck.LastUpdate = ran.Next(0, 10).ToString() + " seconds ago";


                    Hub.Clients.addOrUpdateTruck(truck);

                    System.Diagnostics.Debug.WriteLine(DateTime.Now + ": Updated truck: " + truck.TruckId);
                }
            }



            if (iCounter == 30)
            {
                TowTruck2 truck = towTrucks2.FirstOrDefault();

                towTrucks2.Remove(truck);
                // Notify the connected clients
                Hub.Clients.deleteTruck(truck.TruckId);

                System.Diagnostics.Debug.WriteLine(DateTime.Now + ": Removed truck: " + truck.TruckId);
            }


            iCounter += 1;
        }
        void Demo2()
        {
            try
            {
                Random ran = new Random();
                int iTruckUpdatesCount = 0;
                int iTruckDeleteCount = 0;

                lock (towTrucks2)
                {
                    int NewTruckId = ran.Next(1, 5);

                    if (towTrucks2.Any(p => p.TruckId == NewTruckId))
                    {
                        #region update
                        TowTruck2 truck = towTrucks2.Single(p => p.TruckId == NewTruckId);
                        truck.Speed = ran.NextDouble() * 100;
                        //add a degree to heading
                        truck.Heading = truck.Heading + 10;
                        truck.LastUpdate = ran.Next(0, 20) + " seconds ago";

                        iTruckUpdatesCount += 1;

                        int number = ran.Next(0, 20);
                        if (number % 2 != 0)
                        {
                            truck.Lat = truck.Lat - 0.0050;
                            truck.Lon = truck.Lon + 0.0050;
                        }
                        else
                        {
                            truck.Lat = truck.Lat + 0.0050;
                            truck.Lon = truck.Lon - 0.0050;
                        }

                        if (truck.Lat < 33.6539 || truck.Lat > 33.7939)
                            truck.Lat = 33.7319;


                        if (truck.Lon < -117.9597 || truck.Lon > -117.6597)
                            truck.Lon = -117.7927;

                        // Notify the connected clients
                        Hub.Clients.addOrUpdateTruck(truck);

                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Updating Truck " + truck.TruckId);

                        #endregion
                    }
                    else
                    {
                        #region add

                        iTruckUpdatesCount += 1;

                        TowTruck2 truck = new TowTruck2();
                        truck.Old = false;
                        truck.TruckId = NewTruckId;
                        truck.StateImage = @"http://localhost/FSP.Web/Content/Images/NIS.png";
                        truck.StateDescription = "Waiting for Driver to Login in";
                        truck.Heading = 20;
                        truck.BeatId = 1;
                        truck.ContractorId = 1;
                        truck.Lat = 33.7319;
                        truck.Lon = -117.7927;
                        truck.Speed = 25;
                        truck.LastUpdate = "0 seconds ago";

                        towTrucks2.Add(truck);

                        // Notify the connected clients
                        Hub.Clients.addOrUpdateTruck(truck);

                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Adding Truck " + truck.TruckId);

                        #endregion
                    }


                    #region remove

                    var removeItem = false;

                    //removeItem = NewId % 2 == 0;
                    removeItem = iCounter % 20 == 0;

                    if (removeItem)
                    {
                        TowTruck2 truck = towTrucks2.FirstOrDefault();
                        truck.Old = true;
                        //towTrucks2.Remove(truck);

                        Hub.Clients.deleteTruck(truck.TruckId);
                        System.Diagnostics.Debug.WriteLine(DateTime.Now + " HUB: Removing Truck " + truck.TruckId);

                        towTrucks2.RemoveAll(p => p.Old == true);
                    }

                    #endregion
                }


                System.Diagnostics.Debug.WriteLine("================Server: Total Number of trucks in local list: " + towTrucks2.Count());
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

    }

    public class TowTruck2
    {
        public int TruckId { get; set; }
        public String StateImage { get; set; }
        public String StateDescription { get; set; }
        public Double Heading { get; set; }
        public int BeatId { get; set; }
        public int ContractorId { get; set; }
        public Double Lat { get; set; }
        public Double Lon { get; set; }
        public Double Speed { get; set; }
        public String LastUpdate { get; set; }
        //internal prop
        public Boolean Old { get; set; }

    }

    public class TowTruck
    {
        public String TruckId { get; set; }
        public Double Speed { get; set; }
        public int Direction { get; set; }
        public Double Lat { get; set; }
        public Double Lon { get; set; }
        public String State { get; set; }
        public Boolean Alarm { get; set; }
        public String AlarmValue { get; set; }
        public Double Heading { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class MapPolygonLine
    {
        public String Name { get; set; }
        public List<MapPolygonLinePoint> Points { get; set; }
    }

    public class MapPolygonLinePoint
    {
        public Double Lat { get; set; }
        public Double Lon { get; set; }
    }
}
