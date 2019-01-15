using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor,DataConsultant")]
    public class ShiftEntryController : Controller
    {
        // GET: ShiftEntry
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveIncident(ShiftEntryViewModel _incident)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ShiftEntryViewModel.Assist[] assists = js.Deserialize<ShiftEntryViewModel.Assist[]>(_incident.Assists.ToString());

            using (MTCDBEntities db = new MTCDBEntities())
            {
                try
                {
                    MTCIncident Incident = new MTCIncident();

                    Incident.IncidentID = Guid.NewGuid();
                    Incident.Lat = 0.00;
                    Incident.Lat = 0.00;
                    Incident.UserPosted = HttpContext.User.Identity.Name;
                    Incident.DatePosted = DateTime.Now;
                    Incident.fromTruck = 0;
                    Incident.TruckNumber = _incident.Truck;
                    Incident.Beat = _incident.Beat;

                    //Add new incident
                    db.MTCIncidents.Add(Incident);

                    //save changes
                    db.SaveChanges();

                    //add preassist
                    foreach (ShiftEntryViewModel.Assist assist in assists)
                    {
                        //Add PreAssist
                        MTCPreAssist PA = new MTCPreAssist();
                        PA.PreAssistID = Guid.NewGuid();
                        PA.IncidentID = Incident.IncidentID;
                        PA.Direction = assist.DIR;
                        PA.Position = assist.POS;
                        PA.CHPIncidentType = assist.IC;
                        PA.CHPLogNumber = assist.CHPIncLogNum;
                        PA.Lat = 0.00;
                        PA.Lon = 0.00;
                        PA.datePosted = DateTime.Now;
                        PA.Freeway = assist.Highway;
                        PA.IncidentSurveyNumber = _incident.IncidentSurvNum;
                        PA.DriverID = new Guid(_incident.Driver);  

                        db.MTCPreAssists.Add(PA);

                        //Add Assists
                        MTCAssist AS = new MTCAssist();
                        AS.MTCAssistID = Guid.NewGuid();
                        AS.IncidentID = Incident.IncidentID;
                        AS.TrafficCollision = assist.TC;
                        AS.DebrisOnly = assist.DO;
                        AS.Breakdown = assist.BD;
                        AS.Other = assist.O;
                        AS.TransportType = assist.TransCode;
                        AS.StartODO = _incident.LogOnOd;
                        AS.EndODO = _incident.LogOffOd;
                        AS.DropSiteBeat = _incident.Beat;
                        AS.DropSite = assist.Area;
                        AS.State = assist.LicPlateState;
                        AS.LicensePlateNumber = assist.LicPlateNum;
                        AS.VehicleType = assist.VehType;
                        AS.DetailNote = assist.DetailNote;
                        AS.Lat = 0.00;
                        AS.Lon = 0.00;
                        AS.datePosted = DateTime.Now;
                        AS.CallSign = _incident.CallSign;
                        AS.TimeOnInc = assist.TimeOnInc;
                        AS.TimeOffInc = assist.TimeOffInc;

                        db.MTCAssists.Add(AS);

                        //Add ActionTaken
                        string[] AT = assist.AP.Split(',');
                        for(int i = 0; i <= AT.Length -1; i++)
                        {
                            MTCActionTaken action = new MTCActionTaken();
                            action.MTCActionTakenID = Guid.NewGuid();
                            action.MTCAssistID = AS.MTCAssistID;
                            action.ActionTaken = AT[i].ToString();

                            //add action taken
                            db.MTCActionTakens.Add(action);
                        }

                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

                

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}