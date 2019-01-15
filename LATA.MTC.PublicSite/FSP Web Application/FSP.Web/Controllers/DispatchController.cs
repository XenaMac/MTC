using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.ViewModels;

namespace FSP.Web.Controllers
{
    [Authorize] //any logged in user role can see the map
    public class DispatchController : Controller
    {
        private FSPDataContext dc = new FSPDataContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetDispatchs()
        {

            var Dispatchs = new List<Dispatch>();

            //if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            //{
            #region demo data

            Dispatchs.Add(new Dispatch
            {
                BeatNumber = "221",
                TruckNumber = "390",
                ContractorName = "ABC Towing",
                DriverName = "John Smith",
                ServiceDate = "8/1/2012",
                BeatStartTime = "05:30:30",
                Status = "10-8",
                Alarms = "-",
                LastUpdateTime = "07:31:30",
                Freeway = "SR 22",
                Direction = "EB",
                LastLocation = "Between Euclid St. and Harbor Blvd.",
                AssistDescription = "-",
                IncidentCode = "1124",
                ServiceCode = "B",
                VehicleDescription = "Black Sedan",
                VehicleLicensePlateNumber = "KCS220",
                TowLocation = "-",
                IsVisible = true

            });

            Dispatchs.Add(new Dispatch
            {
                BeatNumber = "222",
                TruckNumber = "391",
                ContractorName = "XYZ Towing",
                DriverName = "George Jones",
                ServiceDate = "7/1/2012",
                BeatStartTime = "00:30:30",
                Status = "10-7",
                Alarms = "Speeding",
                LastUpdateTime = "07:31:38",
                Freeway = "405",
                Direction = "NB",
                LastLocation = "Between Irvine Center Dr. and Technology",
                AssistDescription = "-",
                IncidentCode = "1234",
                ServiceCode = "DD",
                VehicleDescription = "White SUV",
                VehicleLicensePlateNumber = "UVW123",
                TowLocation = "Irvine Impound",
                IsVisible = true

            });

            #endregion
            //}
            //else
            //{
            //    //call db or service
            //}

            return Json(Dispatchs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index1()
        {
            DispatchViewModel model = new DispatchViewModel();
            model.Dispatchs = new List<Dispatch>();

            if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            {
                #region demo data

                model.Dispatchs.Add(new Dispatch
                {
                    BeatNumber = "221",
                    TruckNumber = "390",
                    ContractorName = "ABC Towing",
                    DriverName = "John Smith",
                    ServiceDate = "8/1/2012",
                    BeatStartTime = "05:30:30",
                    Status = "10-8",
                    Alarms = "-",
                    LastUpdateTime = "07:31:30",
                    Freeway = "SR 22",
                    Direction = "EB",
                    LastLocation = "Between Euclid St. and Harbor Blvd.",
                    AssistDescription = "-",
                    IncidentCode = "1124",
                    ServiceCode = "B",
                    VehicleDescription = "Black Sedan",
                    VehicleLicensePlateNumber = "KCS220",
                    TowLocation = "-",
                    IsVisible = true

                });

                model.Dispatchs.Add(new Dispatch
                {
                    BeatNumber = "222",
                    TruckNumber = "390",
                    ContractorName = "ABC Towing",
                    DriverName = "George Jones",
                    ServiceDate = "8/1/2012",
                    BeatStartTime = "00:30:30",
                    Status = "10-8",
                    Alarms = "Speeding",
                    LastUpdateTime = "07:31:38",
                    Freeway = "SR 22",
                    Direction = "EB",
                    LastLocation = "Between Euclid St. and Harbor Blvd.",
                    AssistDescription = "-",
                    IncidentCode = "1",
                    ServiceCode = "1",
                    VehicleDescription = "White SUV",
                    VehicleLicensePlateNumber = "UVW123",
                    TowLocation = "-",
                    IsVisible = true

                });

                #endregion
            }
            else
            {
                //call db or service
            }

            return View(model);
        }
        public ActionResult Index2()
        {
            DispatchViewModel model = new DispatchViewModel();
            model.Dispatchs = new List<Dispatch>();

            if (ConfigurationManager.AppSettings["IsDemo"] == "true")
            {
                #region demo data

                model.Dispatchs.Add(new Dispatch
                {
                    BeatNumber = "221",
                    TruckNumber = "390",
                    ContractorName = "ABC Towing",
                    DriverName = "John Smith",
                    ServiceDate = "8/1/2012",
                    BeatStartTime = "05:30:30",
                    Status = "10-8",
                    Alarms = "-",
                    LastUpdateTime = "07:31:30",
                    Freeway = "SR 22",
                    Direction = "EB",
                    LastLocation = "Between Euclid St. and Harbor Blvd.",
                    AssistDescription = "-",
                    IncidentCode = "1124",
                    ServiceCode = "B",
                    VehicleDescription = "Black Sedan",
                    VehicleLicensePlateNumber = "KCS220",
                    TowLocation = "-",
                    IsVisible = true

                });

                model.Dispatchs.Add(new Dispatch
                {
                    BeatNumber = "222",
                    TruckNumber = "390",
                    ContractorName = "ABC Towing",
                    DriverName = "George Jones",
                    ServiceDate = "8/1/2012",
                    BeatStartTime = "00:30:30",
                    Status = "10-8",
                    Alarms = "Speeding",
                    LastUpdateTime = "07:31:38",
                    Freeway = "SR 22",
                    Direction = "EB",
                    LastLocation = "Between Euclid St. and Harbor Blvd.",
                    AssistDescription = "-",
                    IncidentCode = "1",
                    ServiceCode = "1",
                    VehicleDescription = "White SUV",
                    VehicleLicensePlateNumber = "UVW123",
                    TowLocation = "-",
                    IsVisible = true

                });

                #endregion
            }
            else
            {
                //call db or service
            }

            return View(model);
        }
        public ActionResult Index3()
        {
            return View();
        }

        #region Other Calls

        public ActionResult GetDirections(String name_startsWith)
        {
            List<String> directions = new List<string>();
            directions.Add("North");
            directions.Add("South");
            directions.Add("West");
            directions.Add("East");

            return Json(directions, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFreewaysByDirection(String name_startsWith)
        {
            var freeways = from q in dc.Freeways
                           where q.FreewayName.StartsWith(name_startsWith)
                           orderby q.FreewayName
                           select q.FreewayName;

            return Json(freeways, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Make this later role depending
        /// </summary>
        /// <param name="name_startsWith"></param>
        /// <returns></returns>
        public ActionResult GetLocations(String name_startsWith)
        {
            var locations = from q in dc.Locations
                            where q.Location1.StartsWith(name_startsWith)
                            orderby q.Location1
                            select q.Location1;

            return Json(locations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCrossStreetFromBeatSegmentDescription(String name_startsWith)
        {
            var beatsDescription = from q in dc.vBeatSegments
                                   where q.BeatSegmentDescription.Contains(name_startsWith)
                                   orderby q.BeatSegmentDescription
                                   select q.BeatSegmentDescription;

            return Json(beatsDescription, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBeatsFromDispatchInputForm(String beatSegmentDescription)
        {
            var beats = from b in dc.vBeats
                                   join bbs in dc.BeatBeatSegments on b.BeatID equals bbs.BeatID
                                   join bs in dc.vBeatSegments on bbs.BeatSegmentID equals bs.BeatSegmentID
                                   where bs.BeatSegmentDescription == beatSegmentDescription
                                   select b.BeatNumber;


            return Json(beats, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (dc != null)
            {
                dc.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
