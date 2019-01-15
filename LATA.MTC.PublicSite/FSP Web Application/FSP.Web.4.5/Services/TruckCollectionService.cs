using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;
using FSP.Domain.Model;
using FSP.Web._4._5.Helpers;
using FSP.Web._4._5.Hubs;
using FSP.Web._4._5.Models;
using FSP.Web._4._5.TowTruckServiceRef;
using Microsoft.AspNet.SignalR;

namespace FSP.Web._4._5.Services
{
    public class TruckCollectionService
    {
        private FSPDataContext dc = new FSPDataContext();
        //private static Timer timer = null;
        //private static DateTime currentUpdateTime = DateTime.Now;
        //private static List<UITowTruck> towTrucks;

        private Timer timer = null;
        private DateTime currentUpdateTime = DateTime.Now;
        private List<UITowTruck> towTrucks;
        private String _contractorName = String.Empty;

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
            //CHECK CONTRACTOR LOGIN AND COMPANY
            var userIdentity = HttpContext.Current.User.Identity;
            var user = this.dc.Users.Where(p => p.Email == userIdentity.Name).FirstOrDefault();

            if (user.Role.RoleName == "Contractor")
            {
                _contractorName = this.dc.Contractors.Where(p => p.ContractorID == user.ContractorID).FirstOrDefault().ContractCompanyName;
            }
            else
            {
                _contractorName = String.Empty;
            }

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
            Util.WriteToLog(DateTime.Now + ": Timer Tick ==================================================================", "ServiceTimer.txt");
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
                var addTruck = true;

                lock (towTrucks)
                {
                    for (int i = 0; i < serviceTowTrucks.Length; i++)
                    {
                        TowTruckData serviceTowTruck = serviceTowTrucks[i];

                        if (!String.IsNullOrEmpty(_contractorName))
                        {
                            if (_contractorName == serviceTowTruck.ContractorName)
                                addTruck = true;
                            else
                                addTruck = false;
                        }
                        else
                        {
                            addTruck = true;
                        }


                        if (addTruck)
                        {
                            try
                            {


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

                                truck.SpeedingTime = serviceTowTruck.SpeedingTime.ToString();
                                truck.SpeedingValue = serviceTowTruck.SpeedingValue.ToString();
                                truck.OutOfBoundsMessage = serviceTowTruck.OutOfBoundsMessage;
                                truck.OutOfBoundsTime = serviceTowTruck.OutOfBoundsTime.ToString();
                                truck.HasAlarm = serviceTowTruck.Alarms;
                                truck.LastMessage = serviceTowTruck.LastMessage.ToShortTimeString();

                                #region State

                                truck.VehicleState = serviceTowTruck.VehicleState;

                                if (serviceTowTruck.VehicleState == "Waiting for Driver Login")
                                    truck.VehicleStateIconUrl = "TBD.png";

                                else if (serviceTowTruck.VehicleState == "Driver Logged On")
                                    truck.VehicleStateIconUrl = "LoggedOn.png";

                                else if (serviceTowTruck.VehicleState == "On Patrol")
                                    truck.VehicleStateIconUrl = "OnPatrol.png";

                                else if (serviceTowTruck.VehicleState == "On Incident")
                                    truck.VehicleStateIconUrl = "OnAssist.png";

                                else if (serviceTowTruck.VehicleState == "On Break" || serviceTowTruck.VehicleState == "On Lunch" || serviceTowTruck.VehicleState == "Roll In" || serviceTowTruck.VehicleState == "Roll Out")
                                    truck.VehicleStateIconUrl = "NIS.png";

                                else if (serviceTowTruck.Alarms)
                                {
                                    //All alarms red (OnAlarm.pn) expect speeding is yellow
                                    truck.VehicleStateIconUrl = "Alarm.png";

                                    if (serviceTowTruck.SpeedingAlarm)
                                        truck.VehicleStateIconUrl = "Speeding.png";
                                }

                                #endregion

                                #endregion

                                serviceTowTruck = null;
                                UpdateTruck(truck, hasTruckChanged, isTruckNew);
                            }
                            catch (Exception ex)
                            {
                                Util.WriteToLog(ex.Message, "Error.txt");
                            }
                        }



                    }

                    iTruckDeleteCount = CleanupTruckList(iTruckDeleteCount, serviceTowTrucks);

                    serviceTowTrucks = null;

                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks in local list: " + towTrucks.Count(), "Update.txt");
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks updated: " + iTruckUpdatesCount, "Update.txt");
                    Util.WriteToLog(DateTime.Now + "================Server: Total Number of trucks deleted: " + iTruckDeleteCount, "Update.txt");

                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message, "Error.txt");
            }


        }

        private int CleanupTruckList(int iTruckDeleteCount, TowTruckData[] serviceTowTrucks)
        {
            foreach (UITowTruck uiTowTruck in towTrucks)
            {
                try
                {
                    //check to see if a truck in local list exists in the service
                    if (!serviceTowTrucks.Any(p => p.TruckNumber == uiTowTruck.TruckNumber.ToString()))
                    {
                        iTruckDeleteCount += 1;
                        uiTowTruck.Old = true;

                        //Hub notification
                        GetClients().deleteTruck(uiTowTruck.TruckNumber);

                        Util.WriteToLog(DateTime.Now + "================Server: Deleting Truck: " + uiTowTruck.TruckNumber + " " + uiTowTruck.BeatNumber, "RemovingTruck.txt");
                    }
                }
                catch { }
            }

            if (towTrucks.Any(p => p.Old == true))
            {
                Util.WriteToLog(DateTime.Now + "================Server: Truck List Before Delete: " + towTrucks.Count(), "RemovingTruck.txt");
                towTrucks.RemoveAll(p => p.Old == true);
                Util.WriteToLog(DateTime.Now + "================Server: Truck List After Delete: " + towTrucks.Count(), "RemovingTruck.txt");
            }




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
                    Util.WriteToLog("HUB: Truck " + truck.TruckNumber + " " + truck.BeatNumber + " changed ", "HasChanges.txt");

                    truck.LastUpdate = 0;
                    //Notify clients about an existing truck only if truck has "changed"
                    GetClients().addOrUpdateTruck(truck);
                }
                else
                {
                    Util.WriteToLog("HUB: NO CHANGES TO " + truck.TruckNumber + " " + truck.BeatNumber, "HasNoChanges.txt");

                    truck.LastUpdate += Convert.ToInt32(ConfigurationManager.AppSettings["UpdateIntervalInMilliseconds"]) / 1000;

                    //Notify clients about an existing truck only if truck has "changed"
                    GetClients().updateLastUpdateTime(truck.TruckNumber, truck.LastUpdate, truck.LastMessage);
                }
            }

            truck = null;
        }

        private bool HasTruckChanged(ref UITowTruck truck, TowTruckData serviceTowTruck)
        {
            var returnValue = false;

            try
            {

                if (truck.Lat != serviceTowTruck.Lat)
                    returnValue = true;

                if (truck.Lon != serviceTowTruck.Lon)
                    returnValue = true;

                if (truck.Speed != serviceTowTruck.Speed)
                    returnValue = true;

                if (truck.Heading != serviceTowTruck.Heading)
                    returnValue = true;

                if (serviceTowTruck.BeatNumber != null)
                {
                    if (truck.BeatNumber != serviceTowTruck.BeatNumber.ToString())
                        returnValue = true;
                }

                if (serviceTowTruck.LastMessage != null)
                {
                    if (truck.LastMessage != serviceTowTruck.LastMessage.ToString())
                        returnValue = true;
                }

                if (serviceTowTruck.DriverName != null)
                {
                    if (truck.DriverName != serviceTowTruck.DriverName.ToString())
                        returnValue = true;
                }

                if (serviceTowTruck.ContractorName != null)
                {
                    if (truck.ContractorName != serviceTowTruck.ContractorName.ToString())
                        returnValue = true;
                }

                if (serviceTowTruck.Location != null)
                {
                    if (truck.Location != serviceTowTruck.Location.ToString())
                        returnValue = true;
                }

            }
            catch { }


            return returnValue;

        }

        private TowTruckData[] GetAllTrucksInService()
        {
            TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient();

            TowTruckData[] serviceTowTrucks = service.CurrentTrucks();
            Util.WriteToLog(DateTime.Now + " " + serviceTowTrucks.Count() + " tow trucks retrieved from Service", "ServiceCall.txt");

            service = null;

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