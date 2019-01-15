using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;
using FSP.Domain.Model;
using FSP.Web.Helpers;
using FSP.Web.Hubs;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;
using SignalR;
using SignalR.Hubs;

namespace FSP.Web.Services
{
    public class TruckCollectionService
    {
        private FSPDataContext dc = new FSPDataContext();
        private static Timer timer = null;
        private static DateTime currentUpdateTime = DateTime.Now;
        private static List<UITowTruck> towTrucks;

        public TruckCollectionService()
        {            
            towTrucks = new List<UITowTruck>();

            timer = new Timer();
            timer.Elapsed += timer_Elapsed;
            timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["UpdateIntervalInMilliseconds"]);
            timer.Start();           
        }

        public void GetCurrentTowTrucks()
        {
            if (towTrucks != null)
            {
                foreach (var towTruck in towTrucks)
                {
                    // Notify the connected clients
                    GetClients().addOrUpdateTruck(towTruck);
                }
            }
          
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Util.WriteToLog(DateTime.Now + ": Timer Tick ==================================================================");
            currentUpdateTime = DateTime.Now;

            if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            {
                RunDemo();
            }
            else
            {
                UpdateAllTrucks();
            }
        }

        public void UpdateAllTrucks()
        {
            UITowTruck truck = null;
            int iTruckUpdatesCount = 0;
            int iTruckDeleteCount = 0;

            try
            {
                TowTruckData[] serviceTowTrucks = GetAllTrucksInService();

                var hasTruckChanged = false;
                var isTruckNew = false;

                lock (towTrucks)
                {
                    for (int i = 0; i < serviceTowTrucks.Length; i++)
                    {
                        try
                        {
                            TowTruckData serviceTowTruck = serviceTowTrucks[i];

                            if (towTrucks.Any(p => p.TruckNumber.ToString() == serviceTowTruck.TruckNumber))
                            {
                                truck = towTrucks.Single(p => p.TruckNumber == serviceTowTruck.TruckNumber);
                                hasTruckChanged = HasTruckChanged(ref truck, serviceTowTruck);

                            }
                            else
                            {
                                //new tow truck
                                iTruckUpdatesCount += 1;
                                truck = new UITowTruck();
                                truck.LastUpdate = 0;
                                truck.Old = false;

                                isTruckNew = true;
                                hasTruckChanged = true;
                            }

                            #region Set Truck Properties

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

                            #endregion

                            UpdateTruck(truck, hasTruckChanged, isTruckNew);
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                        }


                    }

                    iTruckDeleteCount = CleanupTruckList(iTruckDeleteCount, serviceTowTrucks);

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

        private int CleanupTruckList(int iTruckDeleteCount, TowTruckData[] serviceTowTrucks)
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
                        GetClients().deleteTruck(uiTowTruck.TruckNumber);

                    }
                }
                catch (Exception ex)
                {
                    Util.WriteToLog(ex.Message);
                }

            }
            towTrucks.RemoveAll(p => p.Old == true);
            return iTruckDeleteCount;
        }

        private void UpdateTruck(UITowTruck truck, bool hasTruckChanged, bool isTruckNew)
        {
            if (isTruckNew)
            {
                towTrucks.Add(truck);

                //Notify clients about new truck
                GetClients().addOrUpdateTruck(truck);
            }
            else
            {
                if (hasTruckChanged)
                {
                    Util.WriteToLog("HUB: Truck " + truck.TruckNumber + " changed ");

                    truck.LastUpdate = 0;
                    //Notify clients about an existing truck only if truck has "changed"
                    GetClients().addOrUpdateTruck(truck);
                }
                else
                {
                    Util.WriteToLog("HUB: NO CHANGES TO " + truck.TruckNumber);

                    truck.LastUpdate += Convert.ToInt32(ConfigurationManager.AppSettings["UpdateIntervalInMilliseconds"]) / 1000;

                    //Notify clients about an existing truck only if truck has "changed"
                    GetClients().updateLastUpdateTime(truck.TruckNumber, truck.LastUpdate, truck.LastMessage);
                }
            }
        }

        private bool HasTruckChanged(ref UITowTruck truck, TowTruckData serviceTowTruck)
        {
            var returnValue = false;

            if (truck.Lat != serviceTowTruck.Lat)
                returnValue = true;

            if (truck.Lon != serviceTowTruck.Lon)
                returnValue = true;

            if (truck.Speed != serviceTowTruck.Speed)
                returnValue = true;

            if (truck.Heading != serviceTowTruck.Heading)
                returnValue = true;

            if (truck.Heading != serviceTowTruck.Heading)
                returnValue = true;

            if (truck.BeatNumber != serviceTowTruck.BeatNumber.ToString())
                returnValue = true;

            if (truck.LastMessage != serviceTowTruck.LastMessage.ToString())
                returnValue = true;

            if (truck.DriverName != serviceTowTruck.DriverName.ToString())
                returnValue = true;

            if (truck.ContractorName != serviceTowTruck.ContractorName.ToString())
                returnValue = true;

            if (truck.Location != serviceTowTruck.Location.ToString())
                returnValue = true;

            return returnValue;

        }

        private TowTruckData[] GetAllTrucksInService()
        {
            TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient();
            TowTruckData[] serviceTowTrucks = service.CurrentTrucks();
            Util.WriteToLog(DateTime.Now + " " + serviceTowTrucks.Count() + " tow trucks retrieved from Service");
            return serviceTowTrucks;
        }

        private static dynamic GetClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<TowTruckHub>().Clients;
        }

        private void RunDemo()
        {

        }

    }


}