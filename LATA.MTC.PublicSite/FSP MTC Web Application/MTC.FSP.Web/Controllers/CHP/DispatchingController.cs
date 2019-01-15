using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.TowTruckServiceRef;

namespace MTC.FSP.Web.Controllers.CHP
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class DispatchingController : MtcBaseController
    {
        [HttpPost]
        public ActionResult DoDispatch(DispatchViewModel model)
        {
            var returnValue = new MtcReturnValue {OperationSuccess = true, Message = string.Empty};

            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    if (model?.Trucks != null)
                        using (var db = new MTCDBEntities())
                        {
                            foreach (var truck in model.Trucks)
                            {
                                var assist = new MTCPreAssistData();


                                var dispatchCode = string.Empty;

                                using (var dc = new MTCDbContext())
                                {
                                    dispatchCode = dc.DispatchCodes.Find(model.DispatchCodeId).Code;
                                }


                                assist.LaneNumber = model.LaneNumber;
                                assist.DispatchCode = dispatchCode;
                                assist.Direction = model.Direction;
                                assist.FSPLocation = model.Location;
                                assist.CrossStreet = model.CrossStreet1;
                                assist.Comment = model.Comments;
                                assist.Freeway = model.Freeway;


                                assist.Beat = string.Empty;
                                assist.CHPIncidentType = string.Empty;
                                assist.CHPLogNumber = string.Empty;
                                assist.IncidentSurveyNumber = string.Empty;
                                assist.Lat = double.MinValue;
                                assist.Lon = double.MinValue;
                                assist.LocationofInitialDispatch = string.Empty;
                                assist.Position = string.Empty;


                                var dbTruck = db.FleetVehicles.FirstOrDefault(p => p.VehicleNumber == truck.truckNumber);

                                if (dbTruck != null)
                                {
                                    service.addAssist(HttpContext.User.Identity.Name, truck.ipAddress, assist);
                                }
                                else
                                {
                                    returnValue.OperationSuccess = false;
                                    returnValue.Message = "Truck Not Found";
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                returnValue.OperationSuccess = false;
                returnValue.Message = ex.Message;
            }


            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }
    }

    public class DispatchViewModel
    {
        public string Direction { get; set; }

        public string Freeway { get; set; }

        public string Location { get; set; }

        public string CrossStreet1 { get; set; }

        public string CrossStreet2 { get; set; }

        public string Comments { get; set; }

        public string LaneNumber { get; set; }

        public List<MtcTruck> Trucks { get; set; }

        public int DispatchCodeId { get; set; }
    }
}