using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Data
{
    public class ReportsController : MtcBaseController
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();
        private readonly MTCDbContext _dbc = new MTCDbContext();
        private readonly MTCMotoristSurveysEntities _dbMs = new MTCMotoristSurveysEntities();

        private ApplicationUserManager _userManager;

        private ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        #region Export

        public async Task<ActionResult> ExportToExcel()
        {
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            var DSRS = new List<DSR>();
            var incidents = new List<MTCIncident>();

            #region GetContractorsAndDrivers

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (HttpContext.User.IsInRole("TowContractor"))
            {
                ViewBag.Contractor = true;
                ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                                 select new
                                                 {
                                                     Value = w.DriverID,
                                                     Text = w.FirstName + " " + w.LastName
                                                 }, "Value", "Text");

                incidents = (from inc in _db.MTCIncidents
                             join pre in _db.MTCPreAssists on inc.IncidentID equals pre.IncidentID
                             where inc.DatePosted >= twoMonthsAgo &&
                                   pre.Driver.ContractorID == user.ContractorId
                             select inc).ToList();
            }
            else
            {
                ViewBag.Contractor = false;
                ViewBag.Contractors = new SelectList(_db.Contractors, "ContractorID", "ContractCompanyName");
                ViewBag.Drivers = new SelectList(from w in _db.Drivers
                                                 select new
                                                 {
                                                     Value = w.DriverID,
                                                     Text = w.FirstName + " " + w.LastName
                                                 }, "Value", "Text");

                incidents = (from inc in _db.MTCIncidents
                             join pre in _db.MTCPreAssists on inc.IncidentID equals pre.IncidentID
                             where inc.DatePosted >= twoMonthsAgo &&
                                   pre.Driver.ContractorID == user.ContractorId
                             select inc).ToList();
            }

            #endregion

            #region list of DSR

            foreach (var incident in incidents)
            {
                var dsr = new DSR();
                var preAssists = (from pre in _db.MTCPreAssists
                                  where pre.IncidentID == incident.IncidentID
                                  select pre).ToList();

                var assists = (from ass in _db.MTCAssists
                               where ass.IncidentID == incident.IncidentID
                               select ass).ToList();

                dsr.PreAssists = preAssists;
                dsr.Assists = assists;

                foreach (var assist in assists)
                {
                    var acts = (from act in _db.MTCActionTakens
                                where act.MTCAssistID == assist.MTCAssistID
                                select act).ToList();

                    dsr.ActionsTaken = acts;
                }

                var beatId = new Guid(assists[0].DropSiteBeat);
                dsr.DatePosted = incident.DatePosted;
                dsr.Callsign = assists[0].CallSign;
                dsr.CHPLogNumber = preAssists[0].CHPLogNumber;
                dsr.DriverFirstName = preAssists[0].Driver.FirstName;
                dsr.DriverLastName = preAssists[0].Driver.LastName;
                dsr.DriverID = preAssists[0].DriverID;
                dsr.DropSite = assists[0].DropSite;
                dsr.DropSiteBeat = (from beat in _db.BeatDatas
                                    where beat.ID == beatId
                                    select beat.BeatName).First();
                dsr.EndODO = assists[0].EndODO;
                dsr.fromTruck = incident.fromTruck;
                dsr.IncidentSurveyNumber = preAssists[0].IncidentSurveyNumber;
                dsr.OTAuthorizationNumber = assists[0].OTAuthorizationNumber;
                dsr.StartODO = assists[0].StartODO;
                dsr.ContractCompany = preAssists[0].Driver.Contractor.ContractCompanyName;
                DSRS.Add(dsr);
            }

            #endregion


            var sb = new StringBuilder();
            //static file name, can be changes as per requirement
            var sFileName = "DSR.xls";

            if (incidents.Any())
            {
                sb.Append("<table style='1px solid black; font-size:12px;'>");
                sb.Append("<tr>");
                sb.Append("<td style='background-color: #000000; color: #FFFFFF;'><b>Date Posted</b></td>");
                sb.Append("<td style='background-color: #000000; color: #FFFFFF;'><b>From Truck</b></td>");
                sb.Append("<td style='background-color: #000000; color: #FFFFFF;'><b>Cancelled</b></td>");
                sb.Append("<td style='background-color: #000000; color: #FFFFFF;'><b>Reason</b></td>");
                sb.Append("</tr>");

                foreach (var result in incidents)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + result.DatePosted + "</td>");
                    sb.Append("<td>" + result.fromTruck + "</td>");
                    sb.Append("<td>" + result.Reason + "</td>");
                    sb.Append("<td>" + result.Canceled + "</td>");
                    sb.Append("</tr>");

                    foreach (var pre in result.MTCPreAssists)
                    {
                        sb.Append("<table style='1px solid black; font-size:12px;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='background-color: #99CCFF;'><b> </b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b> </b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>PRE ASSIST</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Direction</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>FSP Location</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Dispatch Location</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Position</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Lane Number</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>CHPIncident Number</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>CHP Log Number</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Incident Survey Number</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Highway</b></td>");
                        sb.Append("<td style='background-color: #99CCFF;'><b>Driver</b></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td> </td>");
                        sb.Append("<td> </td>");
                        sb.Append("<td>" + pre.Direction + "</td>");
                        sb.Append("<td>" + pre.FSPLocation + "</td>");
                        sb.Append("<td>" + pre.DispatchLocation + "</td>");
                        sb.Append("<td>" + pre.Position + "</td>");
                        sb.Append("<td>" + pre.LaneNumber + "</td>");
                        sb.Append("<td>" + pre.CHPIncidentType + "</td>");
                        sb.Append("<td>" + pre.CHPLogNumber + "</td>");
                        sb.Append("<td>" + pre.IncidentSurveyNumber + "</td>");
                        sb.Append("<td>" + pre.Freeway + "</td>");
                        sb.Append("<td>" + pre.Driver.FirstName + " " + pre.Driver.LastName + "</td>");
                        sb.Append("</tr>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                    }

                    foreach (var ass in result.MTCAssists)
                    {
                        sb.Append("<table style='1px solid black; font-size:12px;'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='background-color: #66FF66;'><b> </b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b> </b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>ASSIST</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Traffic Collision</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Debris Only</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Breakdown</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Other</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Transport Type</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Beat</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Drop Site</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>License State</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>License Number</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Vehicle Type</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>Call Sign</b></td>");
                        sb.Append("<td style='background-color: #66FF66;'><b>OT Auth Number</b></td>");
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td> </td>");
                        sb.Append("<td> </td>");
                        sb.Append("<td>" + ass.TrafficCollision + "</td>");
                        sb.Append("<td>" + ass.Breakdown + "</td>");
                        sb.Append("<td>" + ass.DebrisOnly + "</td>");
                        sb.Append("<td>" + ass.Other + "</td>");
                        sb.Append("<td>" + ass.TransportType + "</td>");
                        sb.Append("<td>" + ass.DropSiteBeat + "</td>");
                        sb.Append("<td>" + ass.DropSite + "</td>");
                        sb.Append("<td>" + ass.State + "</td>");
                        sb.Append("<td>" + ass.LicensePlateNumber + "</td>");
                        sb.Append("<td>" + ass.VehicleType + "</td>");
                        sb.Append("<td>" + ass.CallSign + "</td>");
                        sb.Append("<td>" + ass.OTAuthorizationNumber + "</td>");
                        sb.Append("</tr>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        foreach (var at in ass.MTCActionTakens)
                        {
                            sb.Append("<table style='1px solid black; font-size:12px;'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='background-color: #FFCC80;'><b> </b></td>");
                            sb.Append("<td style='background-color: #FFCC80;'><b> </b></td>");
                            sb.Append("<td style='background-color: #FFCC80;'><b>ACTION TAKEN</b></td>");
                            sb.Append("<td style='background-color: #FFCC80;'><b>" + at.ActionTaken + "</b></td>");
                            sb.Append("</tr>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                        }
                    }
                }

                sb.Append("</tr>");
                sb.Append("</table>");
            }

            HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
            Response.ContentType = "application/vnd.ms-excel";
            var buffer = Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }

        #endregion

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        #region Alarm Reports

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> LateOnPatrol()
        {
            ViewBag.AlarmName = "LateOnPatrol";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> EarlyOutOfService()
        {
            ViewBag.AlarmName = "EarlyOutOfService";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> OffBeat()
        {
            ViewBag.AlarmName = "OffBeat";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> Stationary()
        {
            ViewBag.AlarmName = "Stationary";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> GPSIssue()
        {
            ViewBag.AlarmName = "GPSIssue";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> LongBreak()
        {
            ViewBag.AlarmName = "LongBreak";
            return View("AlarmsReport");
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> Overtime()
        {
            ViewBag.AlarmName = "OvertimeActivity";
            return View("AlarmsReport");
        }

        #endregion

        #region Incident Reports

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> DSR()
        {
            var TwoMonthsAgo = DateTime.Now.AddMonths(-1);
            var DSRS = new List<DSR>();
            var incidents = new List<MTCIncident>();

            #region GetContractorsAndDrivers

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (HttpContext.User.IsInRole("TowContractor"))
            {
                ViewBag.Contractor = true;
                ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                                 select new
                                                 {
                                                     Value = w.DriverID,
                                                     Text = w.FirstName + " " + w.LastName
                                                 }, "Value", "Text");

                incidents = (from inc in _db.MTCIncidents
                             join pre in _db.MTCPreAssists on inc.IncidentID equals pre.IncidentID
                             join ass in _db.MTCAssists on inc.IncidentID equals ass.IncidentID
                             where inc.DatePosted >= TwoMonthsAgo &&
                                   pre.Driver.ContractorID == user.ContractorId
                             select inc).ToList();
            }
            else
            {
                ViewBag.Contractor = false;
                ViewBag.Contractors = new SelectList(_db.Contractors, "ContractorID", "ContractCompanyName");
                ViewBag.Drivers = new SelectList(from w in _db.Drivers
                                                 select new
                                                 {
                                                     Value = w.DriverID,
                                                     Text = w.FirstName + " " + w.LastName
                                                 }, "Value", "Text");

                incidents = (from inc in _db.MTCIncidents
                             join pre in _db.MTCPreAssists on inc.IncidentID equals pre.IncidentID
                             join ass in _db.MTCAssists on inc.IncidentID equals ass.IncidentID
                             where inc.DatePosted >= TwoMonthsAgo
                             select inc).ToList();
            }

            #endregion

            #region list of DSR

            foreach (var incident in incidents)
            {
                var dsr = new DSR();
                var PreAssists = (from pre in _db.MTCPreAssists
                                  where pre.IncidentID == incident.IncidentID
                                  select pre).ToList();

                var Assists = (from ass in _db.MTCAssists
                               where ass.IncidentID == incident.IncidentID
                               select ass).ToList();

                dsr.PreAssists = PreAssists;
                dsr.Assists = Assists;

                foreach (var assist in Assists)
                {
                    var Acts = (from act in _db.MTCActionTakens
                                where act.MTCAssistID == assist.MTCAssistID
                                select act).ToList();

                    dsr.ActionsTaken = Acts;
                }

                //Guid beatid = new Guid(Assists[0].DropSiteBeat);
                dsr.DatePosted = incident.DatePosted;
                dsr.Callsign = Assists[0].CallSign;
                dsr.CHPLogNumber = PreAssists[0].CHPLogNumber;
                dsr.DriverFirstName = PreAssists[0].DriverFirstName;
                dsr.DriverLastName = PreAssists[0].DriverLastName;
                dsr.DriverID = PreAssists[0].DriverID;
                dsr.DropSite = Assists[0].DropSite;
                dsr.Beat = incident.Beat;
                //dsr.DropSiteBeat = (from beat in db.Beats
                //where beat.BeatID == beatid
                //select beat.BeatNumber).First().ToString();
                dsr.EndODO = Assists[0].EndODO;
                dsr.fromTruck = incident.fromTruck;
                dsr.IncidentSurveyNumber = PreAssists[0].IncidentSurveyNumber;
                dsr.OTAuthorizationNumber = Assists[0].OTAuthorizationNumber;
                dsr.StartODO = Assists[0].StartODO;
                if (PreAssists[0].Driver != null)
                    dsr.ContractCompany = PreAssists[0].Driver.Contractor.ContractCompanyName;
                else
                    dsr.ContractCompany = "N/A";


                DSRS.Add(dsr);
            }

            #endregion

            return View(DSRS);
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> AssistsLogged()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,MTC,DataConsultant,FSPPartner,CHPOfficer,TowContractor")]
        public async Task<ActionResult> WazeLogged()
        {
            return View();
        }

        #endregion

        #region Admin Reports

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public async Task<ActionResult> SurveyResults()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public async Task<ActionResult> InvoiceSummary()
        {
            return View("InvoiceSummary");
        }

        #endregion

        #region Data

        [HttpPost]
        public ActionResult GetAssistsLogged(AssistsLoggedQuery query)
        {
            if (query == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var data = from i in _db.Incidents
                           //join pa in _db.MTCPreAssists on i.IncidentID equals pa.IncidentID
                       join a in _db.Assists on i.IncidentID equals a.IncidentID
                       //join at in _db.MTCActionTakens on a.MTCAssistID equals at.MTCAssistID
                       join cs in _db.MTCBeatsCallSigns on a.CallSign equals cs.CallSign
                       join b in _db.BeatDatas on i.Beat equals b.BeatID
                       join con in _db.Contracts on b.ID equals con.BeatId
                       join c in _db.Contractors on con.ContractorID equals c.ContractorID
                       orderby i.IncidentDatePosted descending
                       select new
                       {
                           //DispatchType = pa.CHPIncidentType,
                           DispatchType = i.UserPosted == "CHP CAD" ? "CAD" : "Driver",
                           Date = i.IncidentDatePosted,
                           BeatNumber = cs.BeatID,
                           a.CallSign,
                           i.TruckNumber,
                           c.ContractCompanyName,
                           DriverId = i.DriverID,
                           i.DriverFIrstName,
                           i.DriverLastName,
                           CHPIncident = i.CHPIncidentType,
                           x1097Time = i.IncidentDatePosted,
                           x1098Time = a.AssistDatePosted,
                           i.Direction,
                           Highway = i.Freeway,
                           i.PositionIncident,
                           Area = i.FSPLocation,
                           ProblemCode = i.CHPIncidentType,
                           ActionCode = a.ActionTaken,
                           TransportCode = a.TransportType,
                           a.VehicleType,
                           a.State,
                           a.LicensePlate,
                           SurveyId = i.IncidentSurveyNumber,
                           OTNumber = a.OTAuthorizationNumber,
                           AssistNotes = a.ProblemNote,
                           AssistOtherNotes = a.OtherNote,
                           AssistDetailNotes = a.DetailNote,
                           PreAssistNotes = i.Comment
                       };


            if (!string.IsNullOrEmpty(UsersContractorCompanyName))
                data = data.Where(p => p.ContractCompanyName == UsersContractorCompanyName);


            #region query

            if (query != null)
            {
                if (query.DatePostedStart != null)
                    data = data.Where(p => p.Date >= query.DatePostedStart);
                if (query.DatePostedEnd != null)
                    data = data.Where(p => p.Date <= query.DatePostedEnd);
                if (!string.IsNullOrEmpty(query.CallSign))
                    data = data.Where(p => p.CallSign.Contains(query.CallSign));
                if (query.DriverId != null)
                    data = data.Where(p => p.DriverId == query.DriverId);
                if (!string.IsNullOrEmpty(query.BeatNumber))
                    data = data.Where(p => p.BeatNumber.ToString() == query.BeatNumber);
                if (!string.IsNullOrEmpty(query.ContractCompanyName))
                    data = data.Where(p => p.ContractCompanyName == query.ContractCompanyName);
            }

            #endregion

            if (query.Format == "json")
                return Json(data, JsonRequestBehavior.AllowGet);

            if (query.Format == "excel")
            {
                var sw = new StringWriter();

                sw.WriteLine(
                    "\"DispatchType\",\"Date\",\"BeatNumber\",\"CallSign\",\"TruckNumber\",\"DriverName\",\"Contractor\"," +
                    "\"CHPIncident\",\"x1097Time\",\"x1098Time\",\"Direction\",\"Highway\",\"Position\",\"Area\",\"ProblemCode\"," +
                    "\"ActionCode\",\"TransportCode\",\"VehicleType\",\"State\",\"LicensePlateNumber\",\"SurveyId\",\"OTNumber\",\"Notes\"");

                foreach (var item in data)
                {
                    var notes = new StringBuilder();
                    if (!string.IsNullOrEmpty(item.AssistNotes) && item.AssistNotes != "null")
                        notes.Append($"{item.AssistNotes}-");
                    if (!string.IsNullOrEmpty(item.AssistOtherNotes) && item.AssistOtherNotes != "null")
                        notes.Append($"{item.AssistOtherNotes}-");
                    if (!string.IsNullOrEmpty(item.AssistDetailNotes) && item.AssistDetailNotes != "null")
                        notes.Append($"{item.AssistDetailNotes}-");
                    if (!string.IsNullOrEmpty(item.PreAssistNotes) && item.PreAssistNotes != "null")
                        notes.Append($"{item.PreAssistNotes}");

                    sw.WriteLine(
                        "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\",\"{19}\",\"{20}\",\"{21}\",\"{22}\"",
                        item.DispatchType, item.Date, item.BeatNumber, item.CallSign, item.TruckNumber,
                        $"{item.DriverLastName}, {item.DriverFIrstName}", item.ContractCompanyName,
                        item.CHPIncident, item.x1097Time, item.x1098Time, item.Direction, item.Highway,
                        item.PositionIncident, item.Area, item.ProblemCode, item.ActionCode, item.TransportCode,
                        item.VehicleType, item.State, item.LicensePlate, item.SurveyId, item.OTNumber, notes.ToString());
                }


                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=AssistsLogged.csv");
                Response.ContentType = "text/csv";
                Response.Write(sw.ToString());
                Response.End();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSurveys()
        {
            var data = await (from s in _dbMs.Surveys
                              select new
                              {
                                  s.SurveyID,
                                  s.SurveyName
                              }).ToListAsync();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSurveyQuestions(Guid surveyId)
        {
            var data = await (from q in _dbMs.Questions
                              join sq in _dbMs.SurveysQuestions on q.QuestionID equals sq.QuestionID
                              where sq.SurveyID == surveyId
                              orderby sq.QuestionNumber
                              select new
                              {
                                  q.QuestionID,
                                  q.QuestionText
                              }).ToListAsync();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetSurveyQuestionAnswers(Guid questionId)
        {
            var data = await (from a in _dbMs.Answers
                              join pa in _dbMs.PostedAnswers on a.AnswerID equals pa.AnswerID
                              where a.QuestionID == questionId
                              orderby a.SortOrder
                              select new
                              {
                                  a.AnswerText,
                                  AnswerValue = pa.AnswerVal
                              }).ToListAsync();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetSurveyQuestionsAndAnswers(SurveyQuery query)
        {
            if (query.SurveyId != null)
            {
                var data = await (from q in _dbMs.Questions
                                  join sq in _dbMs.SurveysQuestions on q.QuestionID equals sq.QuestionID
                                  join a in _dbMs.Answers on sq.QuestionID equals a.QuestionID
                                  join pa in _dbMs.PostedAnswers on a.AnswerID equals pa.AnswerID
                                  where sq.SurveyID == query.SurveyId
                                  orderby sq.QuestionNumber, a.SortOrder
                                  select new
                                  {
                                      q.QuestionID,
                                      q.QuestionText,
                                      a.AnswerText,
                                      AnswerValue = pa.AnswerVal
                                  }).ToListAsync();

                if (query.Format == "json")
                    return Json(data, JsonRequestBehavior.AllowGet);
                if (query.Format == "excel")
                {
                    var sw = new StringWriter();
                    sw.WriteLine("\"QuestionText\",\"AnswerText\",\"AnswerValue\"");
                    foreach (var item in data)
                        sw.WriteLine("\"{0}\",\"{1}\",\"{2}\"", item.QuestionText, item.AnswerText, item.AnswerValue);

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment;filename=SurveyResults.csv");
                    Response.ContentType = "text/csv";
                    Response.Write(sw.ToString());
                    Response.End();
                }
            }


            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetAlarmReport(AlarmReportQuery query)
        {
            var data = from t in _db.TruckAlerts
                       join pa in _db.MTCPreAssists on t.runID equals pa.RunID into mtcPreAssists
                       from leftMtcPreAssists in mtcPreAssists.DefaultIfEmpty()
                       join i in _db.MTCIncidents on leftMtcPreAssists.IncidentID equals i.IncidentID into mtcIncidents
                       from leftMtcIncidents in mtcIncidents.DefaultIfEmpty()
                       join a in _db.MTCAssists on leftMtcIncidents.IncidentID equals a.IncidentID into mtcAssists
                       from leftMtcAssists in mtcAssists.DefaultIfEmpty()
                       join d in _db.Drivers on t.DriverName equals d.FirstName + " " + d.LastName into drivers
                       from leftDrivers in drivers.DefaultIfEmpty()
                       where t.AlertName == query.AlarmName
                             && t.ScheduleType > 0
                       //&& t.AlertEnd > pa.datePosted
                       select new
                       {
                           AlertId = t.AlertID,
                           Date = t.AlertStart,
                           BeatNumber = t.Beat,
                           t.CallSign,
                           DriverId = leftDrivers.FSPIDNumber,
                           DriverName = leftDrivers.LastName + ", " + leftDrivers.FirstName,
                           t.TruckNumber,
                           ContractCompanyName = t.ContractorCompany,
                           AlarmTime = t.AlertStart,
                           AlarmDuration = t.AlertMins,
                           AlarmLocation = t.location,
                           LatLon = t.lat + "~" + t.lon,
                           t.Heading,
                           t.Speed,
                           ScheduleId = t.ScheduleID,
                           x1097 = leftMtcPreAssists.datePosted,
                           x1098 = leftMtcAssists.datePosted,
                           x1097Location = leftMtcPreAssists.FSPLocation
                       };

            if (!string.IsNullOrEmpty(UsersContractorCompanyName))
                data = data.Where(p => p.ContractCompanyName == UsersContractorCompanyName);

            #region query

            if (query != null)
            {
                if (query.StartDate != null)
                    data = data.Where(p => p.Date >= query.StartDate);
                if (query.EndDate != null)
                    data = data.Where(p => p.Date <= query.EndDate);
                if (!string.IsNullOrEmpty(query.CallSign))
                    data = data.Where(p => p.CallSign.Contains(query.CallSign));
                if (!string.IsNullOrEmpty(query.DriverName))
                    data = data.Where(p => p.DriverName == query.DriverName);
                if (!string.IsNullOrEmpty(query.BeatNumber))
                    data = data.Where(p => p.BeatNumber == query.BeatNumber);
                if (!string.IsNullOrEmpty(query.TruckNumber))
                    data = data.Where(p => p.TruckNumber == query.TruckNumber);
            }

            #endregion

            var returnList = new List<AlarmReportViewModel>();

            foreach (var item in data.ToList())
                if (!returnList.Any(p => p.AlertId == item.AlertId))
                {
                    var record = new AlarmReportViewModel();
                    record.AlertId = item.AlertId;
                    record.Date = item.Date;
                    record.BeatNumber = item.BeatNumber;
                    record.CallSign = item.CallSign;
                    record.DriverId = item.DriverId;
                    record.DriverName = item.DriverName;
                    record.TruckNumber = item.TruckNumber;
                    record.AlarmTime = item.AlarmTime;
                    record.AlarmDuration = item.AlarmDuration;
                    record.AlarmLocation = item.AlarmLocation;
                    record.LatLon = item.LatLon;
                    record.Heading = item.Heading;
                    record.Speed = item.Speed;
                    record.ScheduleId = item.ScheduleId;
                    record.x1097 = item.x1097;
                    record.x1098 = item.x1098;
                    record.x1097Location = item.x1097Location;


                    record.RegularShift = _db.BeatSchedules.Where(p => p.BeatScheduleID == item.ScheduleId).Select(h =>
                        new ScheduleViewModel
                        {
                            ScheduleName = h.ScheduleName,
                            StartTime = h.StartTime.ToString().Replace(":00.0000000", ""),
                            EndTime = h.EndTime.ToString().Replace(":00.0000000", "")
                        }).FirstOrDefault();


                    record.HolidayShift = _dbc.HolidaySchedules.Where(p => p.ScheduleId == item.ScheduleId).Select(h =>
                        new ScheduleViewModel
                        {
                            ScheduleName = h.ScheduleName,
                            StartTime = h.StartTime.ToString().Replace(":00.0000000", ""),
                            EndTime = h.EndTime.ToString().Replace(":00.0000000", "")
                        }).FirstOrDefault();

                    record.CustomShift = _dbc.CustomSchedules.Where(p => p.ScheduleId == item.ScheduleId).Select(c =>
                        new ScheduleViewModel
                        {
                            ScheduleName = c.ScheduleName,
                            StartTime = c.StartTime.ToString().Replace(":00.0000000", ""),
                            EndTime = c.EndTime.ToString().Replace(":00.0000000", "")
                        }).FirstOrDefault();


                    returnList.Add(record);
                }

            if (query.Format == "json")
                return Json(returnList, JsonRequestBehavior.AllowGet);
            if (query.Format == "excel")
            {
                var sw = new StringWriter();

                sw.WriteLine(
                    "\"AlertId\",\"Date\",\"BeatNumber\",\"CallSign\",\"DriverId\",\"DriverName\",\"TruckNumber\"," +
                    "\"AlarmTime\",\"AlarmDuration\",\"LatLon\",\"Heading\",\"Speed\",\"ScheduleId\",\"x1097\",\"x1098\"," +
                    "\"x1097Location\",\"ScheduleName\",\"StartTime\",\"EndTime\"");

                foreach (var item in returnList)
                {
                    var scheduleName = "";
                    var startTime = "";
                    var endTime = "";

                    if (item.RegularShift != null)
                    {
                        scheduleName = item.RegularShift.ScheduleName;
                        startTime = item.RegularShift.StartTime;
                        endTime = item.RegularShift.EndTime;
                    }
                    else if (item.HolidayShift != null)
                    {
                        scheduleName = item.HolidayShift.ScheduleName;
                        startTime = item.HolidayShift.StartTime;
                        endTime = item.HolidayShift.EndTime;
                    }
                    else if (item.CustomShift != null)
                    {
                        scheduleName = item.CustomShift.ScheduleName;
                        startTime = item.CustomShift.StartTime;
                        endTime = item.CustomShift.EndTime;
                    }


                    sw.WriteLine(
                        "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\"",
                        item.AlertId, item.Date, item.BeatNumber, item.CallSign, item.DriverId, item.DriverName,
                        item.TruckNumber, item.AlarmTime, item.AlarmDuration, item.LatLon, item.Heading, item.Speed,
                        item.ScheduleId, item.x1097, item.x1098, item.x1097Location, scheduleName, startTime, endTime);
                }


                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=" + query.AlarmName + ".csv");
                Response.ContentType = "text/csv";
                Response.Write(sw.ToString());
                Response.End();
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetInvoiceSummary(int month, bool export)
        {
            var LISVM = new List<InvoiceSummaryViewModel>();
            var start = new DateTime(DateTime.Now.Year, month, 1);
            var end = new DateTime(DateTime.Now.Year, month, DateTime.DaysInMonth(DateTime.Now.Year, month));

            foreach (var contract in _db.Contracts.Where(c => c.StartDate <= start).Where(c => c.EndDate >= end))
            {
                var ISVM = new InvoiceSummaryViewModel();

                for (var i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, month); i++)
                    try
                    {
                        var dt = new DateTime(DateTime.Now.Year, month, i);

                        var schedules = _db.SchedulesSearch(dt, contract.BeatId).ToList();

                        #region Get fuel and payrate

                        var pNumb = (from setting in _dbc.MTCApplicationSettings
                                     where setting.Name == "FuelRate"
                                     select setting.Value).FirstOrDefault();

                        var pr = (from r in _db.MTCRateTables
                                  where r.BeatID == contract.BeatId
                                  select r).SingleOrDefault();

                        switch (pNumb)
                        {
                            case "p100":
                                ISVM.PayRate = (decimal)pr.p100;
                                ISVM.FuelRate = (decimal)1.0;
                                break;
                            case "p150":
                                ISVM.PayRate = (decimal)pr.p150;
                                ISVM.FuelRate = (decimal)1.50;
                                break;
                            case "p200":
                                ISVM.PayRate = (decimal)pr.p200;
                                ISVM.FuelRate = (decimal)2.00;
                                break;
                            case "p250":
                                ISVM.PayRate = (decimal)pr.p250;
                                ISVM.FuelRate = (decimal)2.50;
                                break;
                            case "p300":
                                ISVM.PayRate = pr.p300;
                                ISVM.FuelRate = (decimal)3.00;
                                break;
                            case "p350":
                                ISVM.PayRate = pr.p350;
                                ISVM.FuelRate = (decimal)3.50;
                                break;
                            case "p400":
                                ISVM.PayRate = pr.p400;
                                ISVM.FuelRate = (decimal)4.00;
                                break;
                            case "p450":
                                ISVM.PayRate = pr.p450;
                                ISVM.FuelRate = (decimal)4.50;
                                break;
                            case "p500":
                                ISVM.PayRate = pr.p500;
                                ISVM.FuelRate = (decimal)5.00;
                                break;
                            case "p550":
                                ISVM.PayRate = pr.p550;
                                ISVM.FuelRate = (decimal)5.50;
                                break;
                            case "p600":
                                ISVM.PayRate = (decimal)pr.p600;
                                ISVM.FuelRate = (decimal)6.00;
                                break;
                            case "p650":
                                ISVM.PayRate = (decimal)pr.p650;
                                ISVM.FuelRate = (decimal)6.50;
                                break;
                            case "p700":
                                ISVM.PayRate = (decimal)pr.p700;
                                ISVM.FuelRate = (decimal)7.00;
                                break;
                            case "p750":
                                ISVM.PayRate = (decimal)pr.p750;
                                ISVM.FuelRate = (decimal)7.50;
                                break;
                            case "p800":
                                ISVM.PayRate = (decimal)pr.p800;
                                ISVM.FuelRate = (decimal)8.00;
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }

                        #endregion

                        if (schedules.Count > 0)
                        {
                            //Get un-Summed Data
                            ISVM.BeatNumber = Convert.ToInt32(schedules[0].BeatNumber);
                            ISVM.InvoiceNumber =
                                schedules[0].ContractCompanyName.Trim().Replace(" ", "").Substring(0, 5) + "_" +
                                _db.BeatDatas.Where(b => b.ID == contract.BeatId).Select(b => b.BeatName)
                                    .FirstOrDefault() + "_" + month + DateTime.Now.Year;
                            ISVM.TotalDays = DateTime.DaysInMonth(DateTime.Now.Year, month);
                            ISVM.TotalShifts += schedules.Count();

                            //Sum up all contract hours and onPatrolHours
                            foreach (var result in schedules)
                            {
                                ISVM.ContractHours += schedules[0].TotalShiftHours / 60 * result.NumberOfTrucks;
                                ISVM.OnPatrolHours += schedules[0].TotalShiftHours / 60 * result.NumberOfTrucks;
                            }

                            ISVM.BasePay = ISVM.ContractHours * ISVM.PayRate;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                #region get total additions

                var CID = (from b in _db.Contracts
                           where b.BeatId == contract.BeatId
                           select b.ContractID).FirstOrDefault();

                var Additions = new List<InvoiceAdditions>();

                #region get overtime

                var overtime = (from o in _db.OvertimeActivities
                                join b in _db.BeatDatas on o.Beat equals b.BeatID
                                where b.ID == contract.BeatId &&
                                      o.Confirmed == true &&
                                      o.timeStamp.Month == month &&
                                      o.timeStamp.Year == DateTime.Now.Year
                                select new InvoiceAdditions
                                {
                                    category = "Overtime",
                                    date = o.timeStamp.ToString(),
                                    description = o.OverTimeCode,
                                    TimeAdded = o.BlocksApproved ?? 1,
                                    Rate = ISVM.PayRate,
                                    shiftDay = o.Shift,
                                    Cost = (o.BlocksApproved ?? 1) * (ISVM.PayRate / 4)
                                }).ToList();

                foreach (InvoiceAdditions addition in overtime)
                    Additions.Add(addition);

                #endregion

                #region get backup pay

                var backups = _dbc.BackupRequests.ToList().Where(b => b.SelectedBackupContractorId == CID)
                    .Where(b => b.IsCancelled == false).Select(b => new
                    {
                        b.RequestNumber,
                        BeatID = b.BeatId,
                        _db.BeatDatas.FirstOrDefault(p => b.BeatId == p.ID).BeatName,
                        RequestComments = b.Comments,
                        ResponseComments = _dbc.BackupResponses.Where(p => p.BackupRequestId == b.Id)
                            .Select(p => p.Comments).FirstOrDefault(),
                        IsResolved = _dbc.BackupResponses.Any(p =>
                            p.BackupRequestId == b.Id && p.BackupResponseStatus == BackupResponseStatus.Accepted),
                        BackupRequestShiftsAndDates = _dbc.BackupRequestShiftsAndDates
                            .Where(p => p.BackupRequestId == b.Id).Where(p => p.BackupDate.Month == month).Select(s =>
                                new
                                {
                                    s.Id,
                                    s.BackupRequestId,
                                    s.BackupDate,
                                    s.AMRequested,
                                    s.AMSatisfied,
                                    s.MIDRequested,
                                    s.MIDSatisfied,
                                    s.PMRequested,
                                    s.PMSatisfied
                                }).FirstOrDefault()
                    }).ToList();

                foreach (var bu in backups)
                    //1. If IsResolved = true then ...
                    //2. If AMSatisfied get shift hours
                    //3. If MIDSatisfied get shift hours
                    //4. If PMSatisfied get shift hours
                    //5. Create new InvoiceAddition and add to list

                    if (bu.IsResolved)
                    {
                        var addon = new InvoiceAdditions();
                        addon.category = "Backup Resolved";
                        addon.date = bu.BackupRequestShiftsAndDates.BackupDate.ToShortDateString();
                        addon.description = bu.RequestComments + " | " + bu.ResponseComments;
                        addon.Rate = ISVM.PayRate;
                        addon.TimeAdded = 0;
                        var schedules2 = _db.SchedulesSearch(bu.BackupRequestShiftsAndDates.BackupDate, bu.BeatID)
                            .ToList();
                        if (bu.BackupRequestShiftsAndDates.AMSatisfied)
                            foreach (var schedule in schedules2)
                                if (schedule.BScheduleName == "AM")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    addon.TimeAdded += duration.Hours * 4;
                                    addon.shiftDay = schedule.BScheduleType;
                                }

                        if (bu.BackupRequestShiftsAndDates.MIDSatisfied)
                            foreach (var schedule in schedules2)
                                if (schedule.BScheduleName == "MID")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    addon.TimeAdded += duration.Hours * 4;
                                    addon.shiftDay = schedule.BScheduleType;
                                }

                        if (bu.BackupRequestShiftsAndDates.PMSatisfied)
                            foreach (var schedule in schedules2)
                                if (schedule.BScheduleName == "PM")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    addon.TimeAdded += duration.Hours * 4;
                                    addon.shiftDay = schedule.BScheduleType;
                                }

                        addon.Cost = addon.TimeAdded * addon.Rate / 4;

                        Additions.Add(addon);
                    }

                #endregion

                #region get succesful appeals

                //InvoiceAppeals
                var invoiceAppeals = (from a in _db.Appeals
                                      where a.AppealType == "Invoice" &&
                                            a.Beatid == contract.BeatId &&
                                            (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                             a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                      select new InvoiceAdditions
                                      {
                                          category = "Accepted Invoice Appeal",
                                          date = a.I_EventDate.ToString(),
                                          description = a.I_AppealReason,
                                          Cost = a.I_AppealDeduction ?? 0
                                      }).ToList();

                foreach (var a in invoiceAppeals)
                    Additions.Add(a);

                //ViolationAppeals
                var violationAppeals = (from a in _db.Appeals
                                        where a.AppealType == "Violation" &&
                                              a.Beatid == contract.BeatId &&
                                              (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                               a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                        select new InvoiceAdditions
                                        {
                                            category = "Accepted Violation Appeal",
                                            date = a.V_ViolationId.ToString(),
                                            description = a.V_ReasonForAppeal,
                                            Cost = a.V_AppropriateCharge ?? 0
                                        }).ToList();

                foreach (var a in violationAppeals)
                {
                    var id = Convert.ToInt32(a.date);
                    var violation = (from v in _dbc.Violations
                                     where v.Id == id
                                     select v).FirstOrDefault();

                    a.date = violation.DateTimeOfViolation.ToString();

                    Additions.Add(a);
                }

                //Overtime
                var overtimeAppeals = (from a in _db.Appeals
                                       where a.AppealType == "Overtime" &&
                                             a.Beatid == contract.BeatId &&
                                             (a.AppealStatu.AppealStatusID.ToString() == "b2080cf8-da32-4cce-a4be-cc1b56e8b479" ||
                                              a.AppealStatu.AppealStatusID.ToString() == "bb0fe413-dd67-4cb8-b7de-d66d28faaebd")
                                       select new InvoiceAdditions
                                       {
                                           category = "Accepted Overtime Appeal",
                                           date = a.O_Datetime.ToString(),
                                           description = a.O_Detail,
                                           TimeAdded = a.O_BlocksInitGranted ?? 1,
                                           Rate = ISVM.PayRate
                                       }).ToList();

                foreach (var a in overtimeAppeals)
                {
                    var rate = a.Rate / 4;
                    a.Cost = rate * a.TimeAdded;

                    Additions.Add(a);
                }

                #endregion

                foreach (var add in Additions)
                {
                    var time = (decimal)add.TimeAdded / 4;
                    ISVM.OnPatrolHours += time;
                    ISVM.TotalAdditions += add.Cost;
                }

                #endregion

                #region get total deductions

                var Deductions = new List<InvoiceDeductions>();

                #region get all violations

                var violations = _dbc.Violations.Where(v => v.ContractorId == CID)
                    .Where(v => v.DateTimeOfViolation.Month == month).Where(v => v.ViolationStatusTypeId == 2).ToList();

                foreach (var vio in violations)
                {
                    var ID = new InvoiceDeductions();
                    var AdditionalFine = new InvoiceDeductions();


                    //Create innitial deduction
                    ID.category = vio.ViolationType.Name;
                    ID.date = vio.DateTimeOfViolation.ToString();
                    ID.description = "Violation ID: " + vio.Id + ", " + vio.ViolationType.Description;

                    //Add two deductions for each of these vio.ViolationTypeId == 19 || 
                    if (vio.ViolationTypeId == 46 || vio.ViolationTypeId == 49)
                    {
                        ID.Rate = Math.Round(ISVM.PayRate / 4, 2);
                        ID.TimeAdded = Convert.ToInt32(vio.LengthOfViolation.Trim()) / 15;
                        ID.Cost = Math.Round(ID.Rate * ID.TimeAdded, 2);

                        //create additional fine option deduction
                        AdditionalFine.category = vio.ViolationType.Name + " Fine";
                        AdditionalFine.date = vio.DateTimeOfViolation.ToString();
                        AdditionalFine.description = "Violation ID: " + vio.Id + ", " + vio.ViolationType.Description;
                        AdditionalFine.Rate = 0.0033M;
                        AdditionalFine.TimeAdded = 0;
                        AdditionalFine.Cost = 0.00M;
                    }
                    else
                    {
                        ID.Rate = Math.Round(ISVM.PayRate / 4, 2);
                        ID.TimeAdded = 0;
                        ID.Cost = 0.00M;
                    }

                    if (AdditionalFine.category != null)
                    {
                        Deductions.Add(ID);
                        Deductions.Add(AdditionalFine);
                        AdditionalFine = null;
                    }
                    else
                    {
                        Deductions.Add(ID);
                    }

                    Deductions.Add(ID);
                }

                #endregion

                #region get all supplies

                var SM = new InvoiceDeductions();
                var supplies = _dbc.MerchandiseOrders
                    .Where(s => s.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderFilled)
                    .Where(s => s.PickupDate.Month == month).Where(s => s.DeductFromInvoice)
                    .Where(s => s.ContractorId == CID).ToList();

                foreach (var MO in supplies)
                {
                    SM.category = "Supplies/Merchandise";
                    SM.date = MO.CreatedOn.ToString();
                    var orders = _dbc.MerchandiseOrderDetails.Where(d => d.MerchandiseOrderId == MO.Id)
                        .Select(d => d.MerchandiseProduct);
                    SM.description = MO.ContactName + ": ";
                    foreach (var detail in orders)
                        SM.description += "|" + detail.DisplayName;
                    SM.Rate = 0.0062M;
                    SM.TimeAdded = 0;
                    SM.Cost = _dbc.MerchandiseOrderDetails.Where(d => d.MerchandiseOrderId == MO.Id)
                        .Sum(d => d.UnitCost);
                    Deductions.Add(SM);
                }

                #endregion

                #region get backup requests

                var BUs = _dbc.BackupRequests.ToList().Where(b => b.ContractorId == CID)
                    .Where(b => b.IsCancelled == false).Select(b => new
                    {
                        b.RequestNumber,
                        BeatID = b.BeatId,
                        _db.BeatDatas.FirstOrDefault(p => b.BeatId == p.ID).BeatName,
                        RequestComments = b.Comments,
                        ResponseComments = _dbc.BackupResponses.Where(p => p.BackupRequestId == b.Id)
                            .Select(p => p.Comments).FirstOrDefault(),
                        IsResolved = _dbc.BackupResponses.Any(p =>
                            p.BackupRequestId == b.Id && p.BackupResponseStatus == BackupResponseStatus.Accepted),
                        BackupRequestShiftsAndDates = _dbc.BackupRequestShiftsAndDates
                            .Where(p => p.BackupRequestId == b.Id).Where(p => p.BackupDate.Month == month).Select(s =>
                                new
                                {
                                    s.Id,
                                    s.BackupRequestId,
                                    s.BackupDate,
                                    s.AMRequested,
                                    s.AMSatisfied,
                                    s.MIDRequested,
                                    s.MIDSatisfied,
                                    s.PMRequested,
                                    s.PMSatisfied
                                }).FirstOrDefault()
                    }).ToList();

                foreach (var bu in backups)
                    //1. If IsResolved = true then ...
                    //2. If AMSatisfied get shift hours
                    //3. If MIDSatisfied get shift hours
                    //4. If PMSatisfied get shift hours
                    //5. Create new InvoiceAddition and add to list

                    if (bu.IsResolved)
                    {
                        var BUID = new InvoiceDeductions();
                        BUID.category = "Backup Resolved";
                        BUID.date = bu.BackupRequestShiftsAndDates.BackupDate.ToShortDateString();
                        BUID.description = bu.RequestComments + " | " + bu.ResponseComments;
                        BUID.Rate = ISVM.PayRate;
                        BUID.TimeAdded = 0;
                        var schedules = _db.SchedulesSearch(bu.BackupRequestShiftsAndDates.BackupDate, bu.BeatID)
                            .ToList();
                        if (bu.BackupRequestShiftsAndDates.AMSatisfied)
                            foreach (var schedule in schedules)
                                if (schedule.BScheduleName == "AM")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    BUID.TimeAdded += duration.Hours * 4;
                                    BUID.shiftDay = schedule.BScheduleType;
                                }

                        if (bu.BackupRequestShiftsAndDates.MIDSatisfied)
                            foreach (var schedule in schedules)
                                if (schedule.BScheduleName == "MID")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    BUID.TimeAdded += duration.Hours * 4;
                                    BUID.shiftDay = schedule.BScheduleType;
                                }

                        if (bu.BackupRequestShiftsAndDates.PMSatisfied)
                            foreach (var schedule in schedules)
                                if (schedule.BScheduleName == "PM")
                                {
                                    var duration = DateTime.Parse(schedule.EndTime.ToString())
                                        .Subtract(DateTime.Parse(schedule.StartTime.ToString()));
                                    BUID.TimeAdded += duration.Hours * 4;
                                    BUID.shiftDay = schedule.BScheduleType;
                                }

                        BUID.Cost = BUID.TimeAdded * BUID.Rate / 4;

                        Deductions.Add(BUID);
                    }

                #endregion

                foreach (var add in Deductions)
                    ISVM.TotalDeductions += add.Cost;

                #endregion

                ISVM.TotalInvoice = ISVM.BasePay + ISVM.TotalAdditions - ISVM.TotalDeductions;

                if (ISVM.BeatNumber != 0)
                    LISVM.Add(ISVM);
            }

            //order the list by beat number
            var listofcrap = LISVM.OrderBy(b => b.BeatNumber);

            if (export == false)
            {
                return Json(listofcrap.ToList(), JsonRequestBehavior.AllowGet);
            }

            var sw = new StringWriter();
            sw.WriteLine(
                "\"Beat Number\",\"Invoice Number\",\"Total Days\",\"Total Shifts\",\"Contract Hours\",\"On Patrol Hours\",\"Fuel Rate\",\"Pay Rate\",\"Base Pay\",\"Total Additions\",\"Total Deductions\",\"Total Invoice\"");
            foreach (var item in LISVM)
                sw.WriteLine(
                    $"\"{item.BeatNumber}\",\"{item.InvoiceNumber}\",\"{item.TotalDays}\",\"{item.TotalShifts}\",\"{item.ContractHours}\",\"{item.OnPatrolHours}\",\"{item.FuelRate}\",\"{item.PayRate}\",\"{item.BasePay}\",\"{item.TotalAdditions}\",\"{item.TotalDeductions}\",\"{item.TotalInvoice}\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=InvoiceSummary_" + month + ".csv");
            Response.ContentType = "text/csv";
            Response.Write(sw.ToString());
            Response.End();

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetWazeMessagesReport()
        {
            var data = await (from w in _db.WAZEIncomings
                              where w.AckTime != null &&
                                    w.AckMessage != null
                              select new
                              {
                                  w.PubDate,
                                  w.Type,
                                  w.SubType,
                                  w.Lat,
                                  lon = w.Lon,
                                  Realiability = w.Reliability,
                                  ThumbsUp = w.nThumbsUp,
                                  w.Confidence,
                                  w.Street,
                                  w.DispatchedTo,
                                  w.DispatchTime,
                                  w.AckTime,
                                  w.AckMessage
                              }).ToListAsync();
            ;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}