using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FSP.Domain.Model;
using FSP.Web.Helpers;
using FSP.Web.Models;
using Newtonsoft.Json.Linq;
using FSP.Web.Filters;

namespace FSP.Web.Controllers
{
    [CustomAuthorization(Roles = "Admin, Dispatcher, CHP")]
    public class DispatchController : Controller
    {
        private FSPDataContext dc = new FSPDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Junk()
        {
            return View();
        }

        #region Other Calls

        //public ActionResult GetDirections()
        //{
        //    List<String> directions = new List<string>();
        //    directions.Add("N");
        //    directions.Add("S");
        //    directions.Add("W");
        //    directions.Add("E");

        //    return Json(directions, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult GetFreewaysByDirection()
        //{
        //    var freeways = from q in dc.Freeways                        
        //                   orderby q.FreewayName
        //                   select q.FreewayName;

        //    return Json(freeways, JsonRequestBehavior.AllowGet);
        //}
        ///// <summary>
        ///// Make this later role depending
        ///// </summary>
        ///// <param name="name_startsWith"></param>
        ///// <returns></returns>
        //public ActionResult GetLocations()
        //{
        //    var locations = from q in dc.Locations                           
        //                    orderby q.Location1
        //                    select q.LocationCode;

        //    return Json(locations, JsonRequestBehavior.AllowGet);
        //}



        public ActionResult GetDirections(String name_startsWith)
        {
            List<String> directions = new List<string>();
            directions.Add("N");
            directions.Add("S");
            directions.Add("W");
            directions.Add("E");

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
                            select q.LocationCode;

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
            //var beats = from b in dc.vBeats
            //            join bbs in dc.BeatBeatSegments on b.BeatID equals bbs.BeatID
            //            join bs in dc.vBeatSegments on bbs.BeatSegmentID equals bs.BeatSegmentID
            //            where bs.BeatSegmentDescription.Contains(beatSegmentDescription)
            //            select b.BeatNumber;


            //return Json(beats, JsonRequestBehavior.AllowGet);

            var beatSegments = from bs in dc.vBeatSegments
                               where bs.BeatSegmentDescription.Contains(beatSegmentDescription)
                               select bs.BeatSegmentNumber;

            return Json(beatSegments, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PostIncidentDispatch(UIIncidentDispatch incidentDispatch)
        {
            Util.CreateAssist(incidentDispatch);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PostIncidentDispatchAjax(String direction, String freeway, String location, String crossStreet1, String crossStreet2, String comments, string selectedTrucks)
        {

            Boolean returnValue = true;

            try
            {
                using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                {
                    TowTruckServiceRef.IncidentIn thisIncident = new TowTruckServiceRef.IncidentIn();

                    Guid IncidentID = Guid.NewGuid();
                    DateTime TimeStamp = DateTime.Now;

                    Guid CreatedBy = MembershipExtensions.GetUserId();
                    var trucks = new JavaScriptSerializer().Deserialize<IEnumerable<SelectedTruck>>(selectedTrucks);


                    String firstTruckBeatNumber = trucks.FirstOrDefault().beatNumberString;

                    thisIncident.IncidentID = IncidentID;
                    thisIncident.Direction = direction;
                    thisIncident.FreewayID = Convert.ToInt32(freeway);
                    thisIncident.LocationID = dc.Locations.Where(p => p.LocationCode == location).FirstOrDefault().LocationID;
                    thisIncident.BeatSegmentID = new Guid("00000000-0000-0000-0000-000000000000");
                    thisIncident.BeatNumber = firstTruckBeatNumber;
                    thisIncident.TimeStamp = TimeStamp;
                    thisIncident.CreatedBy = CreatedBy;
                    thisIncident.Description = comments;
                    thisIncident.CrossStreet1 = crossStreet1;
                    thisIncident.CrossStreet2 = crossStreet2;
                    thisIncident.IncidentNumber = String.Empty;
                    thisIncident.Location = "na";

                    //create incident
                    service.AddIncident(thisIncident);

                   

                    //for each truck create assit

                    foreach (var truck in trucks)
                    {
                        TowTruckServiceRef.AssistReq thisAssist = new TowTruckServiceRef.AssistReq();
                        thisAssist.AssistID = Guid.NewGuid();

                        thisAssist.IncidentID = IncidentID;

                        FleetVehicle dbTruck = dc.FleetVehicles.Where(p => p.VehicleNumber == truck.truckNumber).FirstOrDefault();

                        thisAssist.FleetVehicleID = dbTruck.FleetVehicleID;
                        thisAssist.ContractorID = dbTruck.ContractorID;
                        

                        thisAssist.DispatchTime = DateTime.Now;
                        thisAssist.x1097 = DateTime.Now;
                        service.AddAssist(thisAssist);
                    }

                }

            }
            catch
            {
                returnValue = false;
            }


            return Json(returnValue, JsonRequestBehavior.AllowGet);

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

    public class SelectedTruck
    {
        public String id { get; set; }
        public String truckNumber { get; set; }
        public String beatNumberString { get; set; }

    }
}
