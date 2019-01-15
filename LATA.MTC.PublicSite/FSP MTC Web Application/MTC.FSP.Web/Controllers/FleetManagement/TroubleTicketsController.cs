using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.FleetManagement
{


    public class TroubleTicketsController : MtcBaseController
    {
        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,InVehicleContractor,DataConsultant")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,InVehicleContractor,DataConsultant")]
        public async Task<ActionResult> Details(int? id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                TroubleTicket troubleTicket = await db.TroubleTickets.FindAsync(id);
                if (troubleTicket == null)
                {
                    return HttpNotFound();
                }
                return View(troubleTicket);
            }

        }

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortal()
        {
            return View();
        }

        #region Admin Portal Pages

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortalMechanical()
        {
            ViewBag.TroubleTicketType = 0;
            return View("AdminPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortalInVehicleEquipmentGeneral()
        {
            ViewBag.TroubleTicketType = 1;
            return View("AdminPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortalInVehicleEqupmentLATATrax()
        {
            ViewBag.TroubleTicketType = 2;
            return View("AdminPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortalBackInService()
        {
            ViewBag.TroubleTicketType = 3;
            return View("AdminPortalTroubleTickets");
        }

        #endregion

        [CustomAuthorize(Roles = "Admin,DataConsultant")]
        public ActionResult AdminPortalTroubleTickets()
        {
            return View();
        }


        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortal()
        {
            return View();
        }

        #region Tow Contractor Portal Pages

        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortalMechanical()
        {
            ViewBag.TroubleTicketType = 0;
            return View("TowContractorPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortalInVehicleEquipmentGeneral()
        {
            ViewBag.TroubleTicketType = 1;
            return View("TowContractorPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortalInVehicleEqupmentLATATrax()
        {
            ViewBag.TroubleTicketType = 2;
            return View("TowContractorPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortalBackInService()
        {
            ViewBag.TroubleTicketType = 3;
            return View("TowContractorPortalTroubleTickets");
        }

        #endregion

        [CustomAuthorize(Roles = "Admin,TowContractor,DataConsultant")]
        public ActionResult TowContractorPortalTroubleTickets()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult InVehicleContractorPortal()
        {
            return View();
        }

        #region In-Vehicle Contractor Portal Pages


        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult InVehicleContractorPortalInVehicleEquipmentGeneral()
        {
            ViewBag.TroubleTicketType = 1;
            return View("InVehicleContractorPortalTroubleTickets");
        }

        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult InVehicleContractorPortalInVehicleEqupmentLATATrax()
        {
            ViewBag.TroubleTicketType = 2;
            return View("InVehicleContractorPortalTroubleTickets");
        }

        #endregion

        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult InVehicleContractorPortalTroubleTickets()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult AtVehicle()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,InVehicleContractor,DataConsultant")]
        public ActionResult Maintenance()
        {
            return View();
        }

        #region Json

        public async Task<ActionResult> GetTroubleTickets()
        {
            var data = this.GetTickets();

            if (this.UsersContractorId != null)
                data = data.Where(p => p.AssociatedTowContractorId == this.UsersContractorId || p.AssociatedInVehicleContractorId == this.UsersContractorId || p.AssociatedInVehicleLATATraxContractorId == this.UsersContractorId);

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetUnResolvedTroubleTickets()
        {
            var data = this.GetTickets();

            if (this.UsersContractorId != null)
                data = data.Where(p => p.AssociatedTowContractorId == this.UsersContractorId || p.AssociatedInVehicleContractorId == this.UsersContractorId || p.AssociatedInVehicleLATATraxContractorId == this.UsersContractorId);

            data = data.Where(p => p.TroubleTicketStatus == TroubleTicketStatus.Pending || p.TroubleTicketStatus == TroubleTicketStatus.Unresolved);

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetTowContractorsTroubleTickets(Guid? contractorId, String contractorType)
        {
            var data = this.GetTickets();

            if (contractorId != null)
                data = data.Where(p => p.AssociatedTowContractorId == contractorId);

            data = data.Where(p => p.TroubleTicketStatus == TroubleTicketStatus.Pending || p.TroubleTicketStatus == TroubleTicketStatus.Unresolved);

            if (contractorType == "General")
                data = data.Where(p => p.AssociatedInVehicleContractorId != null);
            else
                data = data.Where(p => p.AssociatedInVehicleLATATraxContractorId != null);


            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> GetTroubleTicketsSnapshot()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                DateTime thirtyDaysAgo = DateTime.Today.AddDays(-30);

                var tt = db.TroubleTickets.Where(p => p.CreatedOn >= thirtyDaysAgo).OrderByDescending(p => p.CreatedOn).ToList();

                var data = (from t in tt
                            group t by t.TroubleTicketStatus into g
                            select new
                            {
                                Status = Enum.GetName(typeof(TroubleTicketStatus), g.Key),
                                NumberOfMechanical = g.Count(p => p.TroubleTicketType == TroubleTicketType.Mechanical),
                                NumberOfInVehicleEquipmentGeneral = g.Count(p => p.TroubleTicketType == TroubleTicketType.InVehicleEquipmentGeneral),
                                NumberOfInVehcileEquipmentLATATrax = g.Count(p => p.TroubleTicketType == TroubleTicketType.InVehcileEquipmentLATATrax),
                                NumberOfBackInService = g.Count(p => p.TroubleTicketType == TroubleTicketType.BackInService)
                            }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<ActionResult> SaveTroubleTicket(TroubleTicketViewModel model)
        {
            bool retValue = true;
            TroubleTicket troubleTicket = null;
            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    if (model.Id == 0)
                    {
                        troubleTicket = new TroubleTicket();
                        troubleTicket.CreatedBy = HttpContext.User.Identity.Name;
                        troubleTicket.CreatedOn = DateTime.Now;
                        troubleTicket.TroubleTicketStatus = TroubleTicketStatus.Unresolved;
                    }
                    else
                    {
                        troubleTicket = db.TroubleTickets.Find(model.Id);

                        this.AuditTroubleTicket(troubleTicket);

                    }

                    troubleTicket.BackupRequestSubmitted = model.BackupRequestSubmitted;

                    troubleTicket.AssociatedTowContractorId = model.AssociatedTowContractorId;
                    troubleTicket.AssociatedInVehicleContractorId = model.AssociatedInVehicleContractorId;
                    troubleTicket.AssociatedInVehicleLATATraxContractorId = model.AssociatedInVehicleLATATraxContractorId;

                    troubleTicket.DateTruckBackInService = model.DateTruckBackInService;
                    troubleTicket.DateTruckOutOfService = model.DateTruckOutOfService;

                    troubleTicket.MTCNotes = model.MTCNotes;
                    troubleTicket.ContractorNotes = model.ContractorNotes;
                    troubleTicket.InVehicleContractorNotes = model.InVehicleContractorNotes;
                    troubleTicket.LATANotes = model.LATANotes;

                    troubleTicket.ReliaGateOEMSerialNumber = model.ReliaGateOEMSerialNumber;

                    #region

                    if (model.TroubleTicketProblemIds != null)
                    {
                        db.TroubleTicketTroubleTicketProblems.RemoveRange(db.TroubleTicketTroubleTicketProblems.Where(p => p.TroubleTicketId == troubleTicket.Id));

                        foreach (var TroubleTicketProblemId in model.TroubleTicketProblemIds)
                        {
                            db.TroubleTicketTroubleTicketProblems.Add(new TroubleTicketTroubleTicketProblem
                            {
                                TroubleTicketProblemId = TroubleTicketProblemId,
                                TroubleTicketId = troubleTicket.Id
                            });
                        }
                    }

                    if (model.TroubleTicketComponentIssueIds != null)
                    {
                        db.TroubleTicketTroubleTicketComponentIssues.RemoveRange(db.TroubleTicketTroubleTicketComponentIssues.Where(p => p.TroubleTicketId == troubleTicket.Id));

                        foreach (var TroubleTicketComponentIssueId in model.TroubleTicketComponentIssueIds)
                        {
                            db.TroubleTicketTroubleTicketComponentIssues.Add(new TroubleTicketTroubleTicketComponentIssue
                            {
                                TroubleTicketComponentIssueId = TroubleTicketComponentIssueId,
                                TroubleTicketId = troubleTicket.Id
                            });
                        }
                    }

                    if (model.TroubleTicketLATATraxIssueIds != null)
                    {
                        db.TroubleTicketTroubleTicketLATATraxIssues.RemoveRange(db.TroubleTicketTroubleTicketLATATraxIssues.Where(p => p.TroubleTicketId == troubleTicket.Id));

                        foreach (var TroubleTicketLATATraxIssueId in model.TroubleTicketLATATraxIssueIds)
                        {
                            db.TroubleTicketTroubleTicketLATATraxIssues.Add(new TroubleTicketTroubleTicketLATATraxIssue
                            {
                                TroubleTicketLATATraxIssueId = TroubleTicketLATATraxIssueId,
                                TroubleTicketId = troubleTicket.Id
                            });
                        }
                    }

                    if (model.TroubleTicketDriverIds != null)
                    {
                        db.TroubleTicketAffectedDrivers.RemoveRange(db.TroubleTicketAffectedDrivers.Where(p => p.TroubleTicketId == troubleTicket.Id));

                        foreach (var TroubleTicketDriverId in model.TroubleTicketDriverIds)
                        {
                            db.TroubleTicketAffectedDrivers.Add(new TroubleTicketAffectedDriver
                            {
                                DriverId = TroubleTicketDriverId,
                                TroubleTicketId = troubleTicket.Id
                            });
                        }
                    }
                    #endregion

                    troubleTicket.ContactName = model.ContactName;
                    troubleTicket.ContactPhone = model.ContactPhone;
                    troubleTicket.ProblemStartedOn = model.ProblemStartedOn;
                    troubleTicket.ShiftsMissed = model.ShiftsMissed;
                    troubleTicket.TroubleTicketStatus = model.TroubleTicketStatus;
                    troubleTicket.TroubleTicketType = model.TroubleTicketType;
                    troubleTicket.VehicleId = model.VehicleId;

                    troubleTicket.ModifiedBy = HttpContext.User.Identity.Name;
                    troubleTicket.ModifiedOn = DateTime.Now;

                    if (model.Id == 0)
                        db.TroubleTickets.Add(troubleTicket);

                    await db.SaveChangesAsync();


                    using (MTCDBEntities dc = new MTCDBEntities())
                    {
                        List<MTCEmailRecipient> ccRecipients = new List<MTCEmailRecipient>();
                        List<MTCEmailRecipient> toRecipients = new List<MTCEmailRecipient>();

                        #region
                        if (troubleTicket.AssociatedTowContractorId != null)
                        {
                            var towContractor = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedTowContractorId);

                            if (towContractor != null)
                            {
                                toRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = towContractor.Email,
                                    Name = towContractor.ContractCompanyName
                                });
                            }
                        }

                        if (troubleTicket.AssociatedInVehicleContractorId != null)
                        {
                            var inVehicleCon = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleContractorId);
                            if (inVehicleCon != null)
                            {
                                toRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = inVehicleCon.Email,
                                    Name = inVehicleCon.ContractCompanyName
                                });
                            }
                        }

                        if (troubleTicket.AssociatedInVehicleLATATraxContractorId != null)
                        {
                            var inVehicleLATATraxCon = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleLATATraxContractorId);
                            if (inVehicleLATATraxCon != null)
                            {
                                toRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = inVehicleLATATraxCon.Email,
                                    Name = inVehicleLATATraxCon.ContractCompanyName
                                });
                            }
                        }
                        #endregion

                        ccRecipients.Add(new MTCEmailRecipient
                        {
                            Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                            Name = Utilities.GetApplicationSettingValue("MTCContactName")
                        });

                        if (toRecipients.Any())
                        {
                            var body = EmailManager.BuildTroubleTicketEmailBody(troubleTicket);
                            var subject = "Trouble Ticket [" + troubleTicket.Id.ToString().PadLeft(8, "0"[0]) + "]";
                            if (troubleTicket.VehicleId != Guid.Empty)
                            {
                                using (MTCDBEntities dbBase = new MTCDBEntities())
                                {
                                    var vehicle = dbBase.FleetVehicles.FirstOrDefault(p => p.FleetVehicleID == troubleTicket.VehicleId);
                                    if (vehicle != null)
                                        subject += " for Truck " + vehicle.VehicleNumber;
                                }
                            }
                            EmailManager.SendEmail(toRecipients, subject, body, ccRecipients);
                        }


                    }


                }
            }
            catch
            {
                retValue = false;
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveTroubleTicketReplacementData(int Id, bool ReplacmentIsFixed, DateTime ReplacmentDate, String ReplacementOEMSerialNumber,
            String ReplacementIPAddress, String ReplacementWiFiSSID, String InVehicleContractorNotes)
        {
            bool retValue = true;

            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    TroubleTicket troubleTicket = db.TroubleTickets.Find(Id);

                    if (troubleTicket != null)
                    {

                        this.AuditTroubleTicket(troubleTicket);

                        troubleTicket.ReplacmentIsFixed = ReplacmentIsFixed;
                        troubleTicket.ReplacmentDate = ReplacmentDate;
                        troubleTicket.ReplacementOEMSerialNumber = ReplacementOEMSerialNumber;
                        troubleTicket.ReplacementIPAddress = ReplacementIPAddress;
                        troubleTicket.ReplacementWiFiSSID = ReplacementWiFiSSID;
                        troubleTicket.InVehicleContractorNotes = InVehicleContractorNotes;

                        troubleTicket.ModifiedBy = HttpContext.User.Identity.Name;
                        troubleTicket.ModifiedOn = DateTime.Now;

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                retValue = false;
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> SaveTroubleTicketMaintenanceData(int Id, DateTime? TroubleShootingDate, DateTime? FixedDate, DateTime? RemovedAssetDate, DateTime? ShippedAssetDate,
           DateTime? ReceivedAssetDate, DateTime? InstalledAssetDate, String LATARMANumber, String MaintenanceNotes)
        {
            bool retValue = true;

            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    TroubleTicket troubleTicket = db.TroubleTickets.Find(Id);

                    if (troubleTicket != null)
                    {

                        this.AuditTroubleTicket(troubleTicket);

                        troubleTicket.TroubleShootingDate = TroubleShootingDate;
                        troubleTicket.FixedDate = FixedDate;
                        troubleTicket.RemovedAssetDate = RemovedAssetDate;
                        troubleTicket.ShippedAssetDate = ShippedAssetDate;
                        troubleTicket.ReceivedAssetDate = ReceivedAssetDate;
                        troubleTicket.InstalledAssetDate = InstalledAssetDate;
                        troubleTicket.LATARMANumber = LATARMANumber;
                        troubleTicket.MaintenanceNotes = MaintenanceNotes;

                        troubleTicket.ModifiedBy = HttpContext.User.Identity.Name;
                        troubleTicket.ModifiedOn = DateTime.Now;

                        await db.SaveChangesAsync();
                    }
                }
            }
            catch
            {
                retValue = false;
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> ResolveTroubleTicketAdmin(int Id, DateTime firstShiftTruckMissed, DateTime lastShiftTruckMissed, String contractorNotes, String mtcNotes)
        {
            bool retValue = true;

            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    TroubleTicket troubleTicket = db.TroubleTickets.Find(Id);

                    if (troubleTicket != null)
                    {
                        this.AuditTroubleTicket(troubleTicket);

                        troubleTicket.FirstShiftTruckMissed = firstShiftTruckMissed;
                        troubleTicket.LastShiftTruckMissed = lastShiftTruckMissed;
                        troubleTicket.ContractorNotes = contractorNotes;
                        troubleTicket.MTCNotes = mtcNotes;
                        troubleTicket.TroubleTicketStatus = TroubleTicketStatus.Resolved;
                        troubleTicket.ModifiedBy = HttpContext.User.Identity.Name;
                        troubleTicket.ModifiedOn = DateTime.Now;

                        await db.SaveChangesAsync();
                    }

                    this.EmailTicketResolution(troubleTicket, db);

                }
            }
            catch
            {
                retValue = false;
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ResolveTroubleTicket(int Id)
        {
            bool retValue = true;

            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    TroubleTicket troubleTicket = db.TroubleTickets.Find(Id);

                    if (troubleTicket != null)
                    {
                        this.AuditTroubleTicket(troubleTicket);

                        troubleTicket.TroubleTicketStatus = TroubleTicketStatus.Resolved;
                        troubleTicket.ModifiedBy = HttpContext.User.Identity.Name;
                        troubleTicket.ModifiedOn = DateTime.Now;

                        await db.SaveChangesAsync();
                    }


                    this.EmailTicketResolution(troubleTicket, db);

                }
            }
            catch
            {
                retValue = false;
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetTroubleTicketProblems()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.TroubleTicketProblems.OrderBy(p => p.Problem).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetTroubleTicketComponentIssues()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.TroubleTicketComponentIssues.OrderBy(p => p.Issue).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetTroubleTicketLATATraxIssues()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var data = db.TroubleTicketLATATraxIssues.OrderBy(p => p.Issue).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region private methods

        private IEnumerable<TroubleTicketListViewModel> GetTickets()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var contractors = this.GetContractorList();
                var vehicles = this.GetVehicleList();
                var drivers = this.GetDriverList();

                var troubleTicketProblems = (from q in db.TroubleTicketTroubleTicketProblems
                                             join t in db.TroubleTicketProblems on q.TroubleTicketProblemId equals t.Id
                                             select new TroubleTicketProblemsViewModel
                                             {
                                                 TroubleTicketProblemId = q.TroubleTicketProblemId,
                                                 Problem = t.Problem,
                                                 TroubleTicketId = q.TroubleTicketId
                                             }).ToList();

                var troubleTicketComponentIssues = (from q in db.TroubleTicketTroubleTicketComponentIssues
                                                    join t in db.TroubleTicketComponentIssues on q.TroubleTicketComponentIssueId equals t.Id
                                                    select new TroubleTickeComponentIssuesViewModel
                                                    {
                                                        TroubleTicketComponentIssueId = q.TroubleTicketComponentIssueId,
                                                        Issue = t.Issue,
                                                        TroubleTicketId = q.TroubleTicketId
                                                    }).ToList();

                var troubleTicketLATATraxIssues = (from q in db.TroubleTicketTroubleTicketLATATraxIssues
                                                   join t in db.TroubleTicketLATATraxIssues on q.TroubleTicketLATATraxIssueId equals t.Id
                                                   select new TroubleTickeLATATraxIssuesViewModel
                                                   {
                                                       TroubleTicketLATATraxIssueId = q.TroubleTicketLATATraxIssueId,
                                                       Issue = t.Issue,
                                                       TroubleTicketId = q.TroubleTicketId
                                                   }).ToList();

                var troubleTicketDrivers = db.TroubleTicketAffectedDrivers.ToList().Select(q => new TroubleTickeDriversViewModel
                {
                    DriverId = q.DriverId,
                    //DriverFullName = drivers.FirstOrDefault(p => p.DriverId == q.DriverId).DriverFullName,
                    DriverFullName = "",
                    TroubleTicketId = q.TroubleTicketId
                }).ToList();


                var thirtyDaysAgo = DateTime.Today.AddDays(-90);


                var data = db.TroubleTickets.Where(p => p.CreatedOn >= thirtyDaysAgo).ToList().Select(p => new TroubleTicketListViewModel
                {
                    Id = p.Id,
                    TroubleTicketStatus = p.TroubleTicketStatus,
                    TroubleTicketStatusName = Enum.GetName(typeof(TroubleTicketStatus), p.TroubleTicketStatus),
                    TroubleTicketType = p.TroubleTicketType,
                    TroubleTicketTypeName = Enum.GetName(typeof(TroubleTicketType), p.TroubleTicketType),

                    AssociatedTowContractorId = p.AssociatedTowContractorId,
                    AssociatedInVehicleContractorId = p.AssociatedInVehicleContractorId,
                    AssociatedInVehicleLATATraxContractorId = p.AssociatedInVehicleLATATraxContractorId,

                    ContactName = p.ContactName,
                    ContactPhone = p.ContactPhone,

                    TowContractCompanyName = contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedTowContractorId).ContractorCompanyName,
                    TowContactPhone = contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedTowContractorId).Phone,
                    TowContactName = contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedTowContractorId).ContactName,

                    InVehicleContractCompanyName = p.AssociatedInVehicleContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleContractorId).ContractorCompanyName : String.Empty,
                    InVehicleContactPhone = p.AssociatedInVehicleContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleContractorId).Phone : String.Empty,
                    InVehicleContactName = p.AssociatedInVehicleContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleContractorId).ContactName : String.Empty,

                    InVehicleLATATraxContractCompanyName = p.AssociatedInVehicleLATATraxContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleLATATraxContractorId).ContractorCompanyName : String.Empty,
                    InVehicleLATATraxContactPhone = p.AssociatedInVehicleLATATraxContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleLATATraxContractorId).Phone : String.Empty,
                    InVehicleLATATraxContactName = p.AssociatedInVehicleLATATraxContractorId != null ? contractors.FirstOrDefault(c => c.ContractorId == p.AssociatedInVehicleLATATraxContractorId).ContactName : String.Empty,

                    VehicleId = p.VehicleId,
                    TruckNumber = TruckNumberReturn(p.VehicleId),
                    TroubleTicketProblems = troubleTicketProblems.Where(t => t.TroubleTicketId == p.Id).ToList(),
                    TroubleTicketComponentIssues = troubleTicketComponentIssues.Where(t => t.TroubleTicketId == p.Id).ToList(),
                    TroubleTicketLATATraxIssues = troubleTicketLATATraxIssues.Where(t => t.TroubleTicketId == p.Id).ToList(),
                    TroubleTicketDrivers = troubleTicketDrivers.Where(t => t.TroubleTicketId == p.Id).OrderBy(t => t.DriverFullName).ToList(),
                    ProblemStartedOn = p.ProblemStartedOn,
                    ShiftsMissed = p.ShiftsMissed,
                    ContractorNotes = p.ContractorNotes,
                    MTCNotes = p.MTCNotes,
                    InVehicleContractorNotes = p.InVehicleContractorNotes,
                    LATANotes = p.LATANotes,
                    DateTruckOutOfService = p.DateTruckOutOfService,
                    DateTruckBackInService = p.DateTruckBackInService,
                    FirstShiftTruckMissed = p.FirstShiftTruckMissed,
                    LastShiftTruckMissed = p.LastShiftTruckMissed,
                    ReliaGateOEMSerialNumber = p.ReliaGateOEMSerialNumber,

                    ReplacmentIsFixed = p.ReplacmentIsFixed,
                    ReplacmentDate = p.ReplacmentDate,
                    ReplacementWiFiSSID = p.ReplacementWiFiSSID,
                    ReplacementOEMSerialNumber = p.ReplacementOEMSerialNumber,
                    ReplacementIPAddress = p.ReplacementIPAddress,

                    TroubleShootingDate = p.TroubleShootingDate,
                    FixedDate = p.FixedDate,
                    RemovedAssetDate = p.RemovedAssetDate,
                    ShippedAssetDate = p.ShippedAssetDate,
                    ReceivedAssetDate = p.ReceivedAssetDate,
                    InstalledAssetDate = p.InstalledAssetDate,
                    LATARMANumber = p.LATARMANumber,
                    MaintenanceNotes = p.MaintenanceNotes,

                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    ModifiedOn = p.ModifiedOn,
                    ModifiedBy = p.ModifiedBy
                });

                return data;
            }

        }

        private string TruckNumberReturn(Guid VehicleId)
        {
            string number = "Null: Truck deleted?";
            List<VehicleViewModel> vehicles = this.GetVehicleList();
            VehicleViewModel truckNumber = vehicles.Where(c => c.FleetVehicleId == VehicleId).FirstOrDefault();

            if(truckNumber != null)
            {
                number = truckNumber.VehicleNumber;
            }

            return number;
        }

        private void EmailTicketResolution(TroubleTicket troubleTicket, MTCDbContext db)
        {
            using (MTCDBEntities dc = new MTCDBEntities())
            {
                List<MTCEmailRecipient> ccRecipients = new List<MTCEmailRecipient>();
                List<MTCEmailRecipient> toRecipients = new List<MTCEmailRecipient>();


                if (troubleTicket.AssociatedTowContractorId != null)
                {
                    var towContractor = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedTowContractorId);
                    if (towContractor != null)
                    {
                        ccRecipients.Add(new MTCEmailRecipient
                        {
                            Email = towContractor.Email,
                            Name = towContractor.ContractCompanyName
                        });
                    }
                }

                if (troubleTicket.AssociatedInVehicleContractorId != null)
                {
                    var inVehicleContractor = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleContractorId);
                    if (inVehicleContractor != null)
                    {
                        ccRecipients.Add(new MTCEmailRecipient
                        {
                            Email = inVehicleContractor.Email,
                            Name = inVehicleContractor.ContractCompanyName
                        });
                    }
                }

                if (troubleTicket.AssociatedInVehicleLATATraxContractorId != null)
                {
                    var inVehicleLATATraxContractor = dc.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleLATATraxContractorId);
                    if (inVehicleLATATraxContractor != null)
                    {
                        ccRecipients.Add(new MTCEmailRecipient
                        {
                            Email = inVehicleLATATraxContractor.Email,
                            Name = inVehicleLATATraxContractor.ContractCompanyName
                        });
                    }
                }


                toRecipients.Add(new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                });

                if (ccRecipients.Any())
                {
                    var body = EmailManager.BuildTroubleTicketEmailBody(troubleTicket);
                    EmailManager.SendEmail(toRecipients, "Resolved Trouble Ticket [" + troubleTicket.Id.ToString().PadLeft(8, "0"[0]) + "]", body, ccRecipients);
                }

            }
        }

        private void AuditTroubleTicket(TroubleTicket troubleTicket)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                db.TroubleTicketAudits.Add(new TroubleTicketAudit(troubleTicket, DateTime.Now, HttpContext.User.Identity.Name));
                db.SaveChanges();
            }
        }

        #endregion

    }
}