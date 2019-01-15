using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    public class AppealsController : Controller
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();
        private readonly MTCDbContext _dbc = new MTCDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public async Task<ActionResult> AdminAppealEdit(Guid? id)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var appeal = await _db.Appeals.FindAsync(id);
            var vid = Convert.ToInt32(appeal?.V_ViolationId);

            #region If Violation

            if (appeal?.AppealType == "Violation")
            {
                var violation = (from v in _dbc.Violations
                                 where v.Id == vid
                                 select v).FirstOrDefault();

                var vehicle = (from fv in _db.FleetVehicles
                               where fv.FleetVehicleID == violation.FleetVehicleId
                               select fv).FirstOrDefault();

                ViewBag.Violation = "<p><strong>Violation: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Date/Time of Violation: </strong>" + violation?.DateTimeOfViolation;
                ViewBag.Violation += "<p><strong>Severity: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Length of Violation: </strong>" + violation?.LengthOfViolation;
                ViewBag.Violation += "<p><strong>Driver: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Fleet Vehicle: </strong>" + vehicle?.FleetNumber;
                ViewBag.Violation += "<p><strong>Beat: </strong>" + appeal.BeatData?.BeatName;
                ViewBag.Violation += "<p><strong>Callsign: </strong>" + violation?.CallSign;
                ViewBag.Violation += "<p><strong>Status: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Deducation: </strong>" + violation?.DeductionAmount;
                ViewBag.Violation += "<p><strong>Notes: </strong>" + violation?.Notes;
                ViewBag.Violation += "</p>";
            }

            #endregion

            ViewBag.AppealStatusID = new SelectList(_db.AppealStatus, "AppealStatusId", "AppealStatus", appeal?.AppealStatu.AppealStatusID).OrderBy(d => d.Text);

            ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                             select new
                                             {
                                                 Value = w.DriverID,
                                                 Text = w.FirstName + " " + w.LastName
                                             }, "Value", "Text", appeal?.Driver?.DriverID);

            ViewBag.Beat = new SelectList(_db.BeatDatas, "ID", "BeatName", appeal?.Beatid).OrderBy(d => d.Text);

            var cs = Convert.ToInt32(appeal?.BeatData?.BeatName);
            ViewBag.Callsign = new SelectList(from q in _db.MTCBeatsCallSigns
                                              where q.BeatID == cs
                                              select new
                                              {
                                                  Id = q.BeatID,
                                                  Text = q.CallSign
                                              }, "Text", "Text", appeal?.O_CallSign);

            #region block dropdown

            ViewBag.BlocksInvoiced = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal?.O_BlocksInitGranted);

            ViewBag.BlocksAppealed = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal?.O_NumOfBlocks);

            #endregion

            return View(appeal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminAppealEdit(Appeal appeal)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            try
            {
                var dbAppeal = _db.Appeals.FirstOrDefault(p => p.AppealID == appeal.AppealID);
                if (dbAppeal != null)
                {
                    dbAppeal.AppealStatusID = appeal.AppealStatusID;
                    dbAppeal.MTCNote = appeal.MTCNote;
                    dbAppeal.ModifiedBy = user.UserName;
                    dbAppeal.ModifiedDate = DateTime.Now;

                    _db.Entry(dbAppeal).State = EntityState.Modified;

                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var violations = new List<Violation>();

            foreach (var item in _db.Appeals)
                if (item.V_ViolationId != null)
                {
                    var vio = (from v in _dbc.Violations
                               where v.Id == item.V_ViolationId
                               select v).FirstOrDefault();
                    violations.Add(vio);
                }

            ViewBag.Violations = violations;

            return View("AdministratorAppeals", await _db.Appeals.ToListAsync());
        }


        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
        public async Task<ActionResult> AdministratorAppeals()
        {
            var violations = new List<Violation>();

            foreach (var item in _db.Appeals)
                if (item.V_ViolationId != null)
                {
                    var vio = (from v in _dbc.Violations
                               where v.Id == item.V_ViolationId
                               select v).FirstOrDefault();
                    violations.Add(vio);
                }

            ViewBag.Violations = violations;

            return View(await _db.Appeals.ToListAsync());
        }


        [CustomAuthorize(Roles = "TowContractor,InVehicleContractor")]
        public async Task<ActionResult> ContractorAppeals()
        {
            var violations = new List<Violation>();

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            foreach (var item in _db.Appeals)
                if (item.V_ViolationId != null)
                {
                    var vio = (from v in _dbc.Violations
                               where v.Id == item.V_ViolationId
                               select v).FirstOrDefault();
                    violations.Add(vio);
                }

            ViewBag.Violations = violations;

            return View(await _db.Appeals.Where(a => a.Contractor.ContractorID == user.ContractorId).ToListAsync());
        }

        public async Task<ActionResult> Create()
        {
            if (!User.Identity.IsAuthenticated) return View();

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var contractorName = (from c in _db.Contractors
                                  where c.ContractorID == user.ContractorId
                                  select c.ContractCompanyName).FirstOrDefault();

            ViewBag.ContractorName = contractorName;
            ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                             select new
                                             {
                                                 Value = w.DriverID,
                                                 Text = w.FirstName + " " + w.LastName
                                             }, "Value", "Text");
            ViewBag.Beat = new SelectList(_db.BeatDatas, "ID", "BeatName").OrderBy(d => d.Text);
            var minus2Months = DateTime.Now.AddMonths(-2);
            ViewBag.Violations = new SelectList(
                from w in _dbc.Violations.ToList()
                    .Where(w => w.ContractorId == user.ContractorId && w.CreatedOn > minus2Months)
                select new
                {
                    Value = w.Id,
                    Text = w.AlarmName + " --|-- " + w.DateTimeOfViolation
                }, "Value", "Text");

            #region block dropdown

            ViewBag.Blocks = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", 1);

            ViewBag.BlocksGranted = new SelectList(new[]
                {
                    new {ID = "0", Name = "0"},
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", 0);

            #endregion

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Appeal appeal, FormCollection form)
        {
            var model = new EditAccountViewModel();
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                appeal.AppealID = Guid.NewGuid();
                appeal.AppealType = form["AppealType"];
                appeal.CreatedDate = DateTime.Now;
                appeal.CreatedBy = user.UserName;
                appeal.ModifiedBy = user.UserName;
                appeal.ModifiedDate = DateTime.Now;
                appeal.Beatid = new Guid(form["BeatId"]);
                var did = new Guid(form["DriverId"]);
                appeal.Driver = (from d in _db.Drivers
                                 where d.DriverID == did
                                 select d).FirstOrDefault();
                if (user.ContractorId != null)
                {
                    var cid = Guid.Parse(user.ContractorId.ToString());
                    appeal.Contractor = (from c in _db.Contractors
                                         where c.ContractorID == cid
                                         select c).FirstOrDefault();
                }

                appeal.AppealStatu = (from g in _db.AppealStatus
                                      where g.AppealStatus == "Submitted"
                                      select g).First();

                //Violation
                if (appeal.AppealType == "Violation")
                    appeal.V_ViolationId = Convert.ToInt32(form["ViolationId"]);

                _db.Appeals.Add(appeal);

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch
                {
                    return View();
                }

                return RedirectToAction("ContractorAppeals");
            }

            var contractorName = (from c in _db.Contractors
                                  where c.ContractorID == user.ContractorId
                                  select c.ContractCompanyName).FirstOrDefault();

            ViewBag.ContractorName = contractorName;
            ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                             select new
                                             {
                                                 Value = w.DriverID,
                                                 Text = w.FirstName + " " + w.LastName
                                             }, "Value", "Text");
            ViewBag.Beat = new SelectList(_db.BeatDatas, "BeatName", "BeatName").OrderBy(d => d.Text);

            var minus2Months = DateTime.Now.AddMonths(-2);

            ViewBag.Violations = new SelectList(
                from w in _dbc.Violations.ToList()
                    .Where(w => w.ContractorId == user.ContractorId && w.CreatedOn > minus2Months)
                select new
                {
                    Value = w.Id,
                    Text = w.AlarmName + " --|-- " + w.DateTimeOfViolation
                }, "Value", "Text");

            #region block dropdown

            ViewBag.Blocks = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", 1);

            ViewBag.BlocksGranted = new SelectList(new[]
                {
                    new {ID = "0", Name = "0"},
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", 0);

            #endregion

            return View(appeal);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appeal = await _db.Appeals.FindAsync(id);
            var vid = Convert.ToInt32(appeal?.V_ViolationId);
            if (appeal?.AppealType == "Violation")
            {
                var violation = (from v in _dbc.Violations
                                 where v.Id == vid
                                 select v).FirstOrDefault();

                ViewBag.VioName = violation?.AlarmName;
                ViewBag.VioDateTime = violation?.DateTimeOfViolation;
            }

            return View(appeal);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var appeal = await _db.Appeals.FindAsync(id);
            if (appeal != null) _db.Appeals.Remove(appeal);
            await _db.SaveChangesAsync();
            return RedirectToAction("ContractorAppeals");
        }


        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appeal = await _db.Appeals.FindAsync(id);
            if (appeal == null)
                return HttpNotFound();
            return View(appeal);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            var model = new EditAccountViewModel();
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appeal = await _db.Appeals.FindAsync(id);
            var vid = Convert.ToInt32(appeal?.V_ViolationId);

            #region If Violation

            if (appeal?.AppealType == "Violation")
            {
                var violation = (from v in _dbc.Violations
                                 where v.Id == vid
                                 select v).FirstOrDefault();

                var vehicle = (from fv in _db.FleetVehicles
                               where fv.FleetVehicleID == violation.FleetVehicleId
                               select fv).FirstOrDefault();

                ViewBag.Violation = "<p><strong>Violation: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Date/Time of Violation: </strong>" + violation?.DateTimeOfViolation;
                ViewBag.Violation += "<p><strong>Severity: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Length of Violation: </strong>" + violation?.LengthOfViolation;
                ViewBag.Violation += "<p><strong>Driver: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Fleet Vehicle: </strong>" + vehicle?.FleetNumber;
                ViewBag.Violation += "<p><strong>Beat: </strong>" + appeal.BeatData?.BeatName;
                ViewBag.Violation += "<p><strong>Callsign: </strong>" + violation?.CallSign;
                ViewBag.Violation += "<p><strong>Status: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Deducation: </strong>" + violation?.DeductionAmount;
                ViewBag.Violation += "<p><strong>Notes: </strong>" + violation?.Notes;
                ViewBag.Violation += "</p>";
            }

            #endregion

            ViewBag.AppealStatus =
                new SelectList(_db.AppealStatus, "AppealStatusId", "AppealStatus", appeal?.AppealStatu.AppealStatusID)
                    .OrderBy(d => d.Text);
            ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                             select new
                                             {
                                                 Value = w.DriverID,
                                                 Text = w.FirstName + " " + w.LastName
                                             }, "Value", "Text", appeal?.Driver.DriverID);
            ViewBag.Beat = new SelectList(_db.BeatDatas, "ID", "BeatName", appeal?.Beatid).OrderBy(d => d.Text);
            var CS = Convert.ToInt32(appeal?.BeatData?.BeatName);
            ViewBag.Callsign = new SelectList(from q in _db.MTCBeatsCallSigns
                                              where q.BeatID == CS
                                              select new
                                              {
                                                  Id = q.BeatID,
                                                  Text = q.CallSign
                                              }, "Text", "Text", appeal?.O_CallSign);

            #region block dropdown

            ViewBag.BlocksInvoiced = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal?.O_BlocksInitGranted);

            ViewBag.BlocksAppealed = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal?.O_NumOfBlocks);

            #endregion

            if (appeal == null)
                return HttpNotFound();

            return View(appeal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Appeal appeal)
        {
            var model = new EditAccountViewModel();
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                _db.Entry(appeal).State = EntityState.Modified;
                appeal.ModifiedBy = user.UserName;
                appeal.ModifiedDate = DateTime.Now;
                await _db.SaveChangesAsync();
                return RedirectToAction("ContractorAppeals");
            }

            var vid = Convert.ToInt32(appeal.V_ViolationId);

            #region if violation

            if (appeal.AppealType == "Violation")
            {
                var violation = (from v in _dbc.Violations
                                 where v.Id == vid
                                 select v).FirstOrDefault();

                var vehicle = (from fv in _db.FleetVehicles
                               where fv.FleetVehicleID == violation.FleetVehicleId
                               select fv).FirstOrDefault();

                ViewBag.Violation = "<p><strong>Violation: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Date/Time of Violation: </strong>" + violation?.DateTimeOfViolation;
                ViewBag.Violation += "<p><strong>Severity: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Length of Violation: </strong>" + violation?.LengthOfViolation;
                ViewBag.Violation += "<p><strong>Driver: </strong>" + violation?.AlarmName;
                ViewBag.Violation += "<p><strong>Fleet Vehicle: </strong>" + vehicle?.FleetNumber;
                ViewBag.Violation += "<p><strong>Beat: </strong>" + appeal.BeatData?.BeatName;
                ViewBag.Violation += "<p><strong>Callsign: </strong>" + violation?.CallSign;
                ViewBag.Violation += "<p><strong>Status: </strong>" + violation?.ViolationStatusType.Name;
                ViewBag.Violation += "<p><strong>Deducation: </strong>" + violation?.DeductionAmount;
                ViewBag.Violation += "<p><strong>Notes: </strong>" + violation?.Notes;
                ViewBag.Violation += "</p>";
            }

            #endregion

            ViewBag.AppealStatus =
                new SelectList(_db.AppealStatus, "AppealStatusId", "AppealStatus", appeal.AppealStatu.AppealStatusID)
                    .OrderBy(d => d.Text);
            ViewBag.Drivers = new SelectList(from w in _db.Drivers.Where(m => m.ContractorID == user.ContractorId)
                                             select new
                                             {
                                                 Value = w.DriverID,
                                                 Text = w.FirstName + " " + w.LastName
                                             }, "Value", "Text", appeal?.Driver.DriverID);
            ViewBag.Beat = new SelectList(_db.BeatDatas, "ID", "BeatName", appeal.Beatid).OrderBy(d => d.Text);
            var CS = Convert.ToInt32(appeal?.BeatData?.BeatName);
            ViewBag.Callsign = new SelectList(from q in _db.MTCBeatsCallSigns
                                              where q.BeatID == CS
                                              select new
                                              {
                                                  Id = q.BeatID,
                                                  Text = q.CallSign
                                              }, "Text", "Text", appeal.O_CallSign);

            #region block dropdown

            ViewBag.BlocksInvoiced = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal.O_BlocksInitGranted);

            ViewBag.BlocksAppealed = new SelectList(new[]
                {
                    new {ID = "1", Name = "1"},
                    new {ID = "2", Name = "2"},
                    new {ID = "3", Name = "3"},
                    new {ID = "4", Name = "4"},
                    new {ID = "5", Name = "5"},
                    new {ID = "6", Name = "6"},
                    new {ID = "7", Name = "7"},
                    new {ID = "8", Name = "8"},
                    new {ID = "9", Name = "9"},
                    new {ID = "10", Name = "10"},
                    new {ID = "11", Name = "11"},
                    new {ID = "12", Name = "12"},
                    new {ID = "13", Name = "13"},
                    new {ID = "14", Name = "14"},
                    new {ID = "15", Name = "15"}
                },
                "ID", "Name", appeal.O_NumOfBlocks);

            #endregion

            return View("ContractorAppeals");
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,InVehicleContractor")]
        public async Task<ActionResult> Index()
        {
            return View(await _db.Appeals.ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}