using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Operations
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor")]
    public class BackupTrucksController : MtcBaseController
    {
        #region Views

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MtcInstructions()
        {
            return View();
        }

        public ActionResult BackupResources()
        {
            return View();
        }

        public ActionResult RequestBackup()
        {
            return View();
        }

        public ActionResult ResponseBackup()
        {
            return View();
        }

        public ActionResult Status()
        {
            return View();
        }

        #endregion

        #region Admin

        #region back-up beats

        public ActionResult GetUrgentBackupValue()
        {
            var data = Utilities.GetApplicationSettingValue("UrgentBackupRequestResponseTimeInMinutes");
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStandardBackupValue()
        {
            var data = Utilities.GetApplicationSettingValue("StandardBackupRequestResponseTimeInMinutes");
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetBackupBeats()
        {
            var beats = GetBeatList();
            using (var db = new MTCDbContext())
            {
                var data = db.BackupBeats.ToList().Select(p => new BackupBeatViewModel
                {
                    Id = p.Id,
                    BeatId = p.BeatId,
                    BeatNumber = beats.FirstOrDefault(b => b.BeatId == p.BeatId)?.BeatNumber,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveBackupBeat(BackupBeat model)
        {
            using (var db = new MTCDbContext())
            {
                if (model.Id == 0)
                {
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = HttpContext.User.Identity.Name;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = HttpContext.User.Identity.Name;
                    db.BackupBeats.Add(model);
                }
                else
                {
                    var bp = db.BackupBeats.Find(model.Id);
                    bp.BeatId = model.BeatId;
                    bp.ModifiedOn = DateTime.Now;
                    bp.ModifiedBy = HttpContext.User.Identity.Name;
                }

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveBackupBeat(int id)
        {
            using (var db = new MTCDbContext())
            {
                var bP = db.BackupBeats.Find(id);
                if (bP == null) return Json(false, JsonRequestBehavior.AllowGet);
                db.BackupBeats.Remove(bP);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region  back-up providers

        public ActionResult GetBackupProviders()
        {
            var beats = GetBeatList();
            var contractors = GetContractorList();
            var vehicles = GetVehicleList();

            using (var db = new MTCDbContext())
            {
                var data = db.BackupProviders.ToList().Select(p => new BackupProviderViewModel
                {
                    Id = p.Id,
                    BackupBeatId = p.BackupBeatId,
                    BeatId = p.BackupBeat.BeatId,
                    Beat = beats.FirstOrDefault(b => b.BeatId == p.BackupBeat.BeatId),
                    ContractorId = p.ContractorId,
                    Contractor = contractors.FirstOrDefault(c => c.ContractorId == p.ContractorId),
                    FleetVehicleId = p.FleetVehicleId,
                    FleetVehicle = vehicles.FirstOrDefault(c => c.FleetVehicleId == p.FleetVehicleId),
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                });


                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveBackupProvider(BackupProvider model)
        {
            using (var db = new MTCDbContext())
            {
                if (model.Id == 0)
                {
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = HttpContext.User.Identity.Name;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = HttpContext.User.Identity.Name;
                    db.BackupProviders.Add(model);
                }
                else
                {
                    var bp = db.BackupProviders.Find(model.Id);
                    bp.ContractorId = model.ContractorId;
                    bp.BackupBeatId = model.BackupBeatId;
                    bp.FleetVehicleId = model.FleetVehicleId;
                    bp.ModifiedOn = DateTime.Now;
                    bp.ModifiedBy = HttpContext.User.Identity.Name;
                }

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveBackupProvider(int id)
        {
            using (var db = new MTCDbContext())
            {
                var bP = db.BackupProviders.Find(id);
                if (bP == null) return Json(false, JsonRequestBehavior.AllowGet);

                db.BackupProviders.Remove(bP);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region back-up assignments

        public ActionResult GetBackupAssignments()
        {
            var beats = GetBeatList();

            using (var db = new MTCDbContext())
            {
                var backupBeats = db.BackupBeats.ToList();
                var data = db.BackupAssignments.ToList().Select(p => new BackupAssignmentViewModel
                {
                    Id = p.Id,
                    BeatId = p.BeatId,
                    BeatNumber = (from b in beats where b.BeatId == p.BeatId select b.BeatNumber).FirstOrDefault(),
                    PrimaryBackupBeatId = p.PrimaryBackupBeatId,
                    PrimaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == backupBeats.FirstOrDefault(t => t.Id == p.PrimaryBackupBeatId)?.BeatId)?.BeatNumber,
                    SecondaryBackupBeatId = p.SecondaryBackupBeatId,
                    SecondaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == backupBeats.FirstOrDefault(t => t.Id == p.SecondaryBackupBeatId)?.BeatId)?.BeatNumber,
                    TertiaryBackupBeatId = p.TertiaryBackupBeatId,
                    TertiaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == backupBeats.FirstOrDefault(t => t.Id == p.TertiaryBackupBeatId)?.BeatId)?.BeatNumber,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                });

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveBackupAssignment(BackupAssignment model)
        {
            using (var db = new MTCDbContext())
            {
                if (model.Id == 0)
                {
                    model.CreatedOn = DateTime.Now;
                    model.CreatedBy = HttpContext.User.Identity.Name;
                    model.ModifiedOn = DateTime.Now;
                    model.ModifiedBy = HttpContext.User.Identity.Name;
                    db.BackupAssignments.Add(model);
                }
                else
                {
                    var bp = db.BackupAssignments.Find(model.Id);
                    if (bp != null)
                    {
                        bp.BeatId = model.BeatId;
                        bp.PrimaryBackupBeatId = model.PrimaryBackupBeatId;
                        bp.SecondaryBackupBeatId = model.SecondaryBackupBeatId;
                        bp.TertiaryBackupBeatId = model.TertiaryBackupBeatId;

                        bp.ModifiedOn = DateTime.Now;
                        bp.ModifiedBy = HttpContext.User.Identity.Name;
                    }
                }

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemoveBackupAssignment(int id)
        {
            using (var db = new MTCDbContext())
            {
                var bP = db.BackupAssignments.Find(id);
                if (bP == null) return Json(false, JsonRequestBehavior.AllowGet);
                db.BackupAssignments.Remove(bP);
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #endregion

        #region requesting and responding to back-up

        public ActionResult GetBackupReasons()
        {
            using (var db = new MTCDbContext())
            {
                var data = from b in db.BackupReasons
                           select new
                           {
                               b.Id,
                               Text = b.ReasonCode + " - " + b.Reason
                           };
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupDeclinationReasons()
        {
            using (var db = new MTCDbContext())
            {
                var data = from b in db.BackupDeclinationReasons
                           select new
                           {
                               b.Id,
                               Text = b.ReasonCode + " - " + b.Reason
                           };
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupCancellationReasons()
        {
            using (var db = new MTCDbContext())
            {
                var data = from b in db.BackupCancellationReasons
                           select new
                           {
                               b.Id,
                               Text = b.ReasonCode + " - " + b.Reason
                           };
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupRequests(Guid? contractorId)
        {
            using (var db = new MTCDbContext())
            {
                var beats = GetBeatList();
                var contractors = GetContractorList();


                var allBus = (from bu in db.BackupRequests
                              join ba in db.BackupAssignments on bu.BeatId equals ba.BeatId
                              where bu.IsCancelled == false
                              orderby bu.CreatedOn descending
                              select new
                              {
                                  bu.Id,
                                  bu.RequestNumber,
                                  bu.ContractorId,
                                  bu.BeatId,
                                  bu.SelectedBackupContractorId,
                                  bu.CurrentBackupContractorId,
                                  PrimaryBackupContractorId = db.BackupProviders.FirstOrDefault(p => p.BackupBeatId == ba.PrimaryBackupBeatId).ContractorId,
                                  SecondaryBackupContractorId = db.BackupProviders.FirstOrDefault(p => p.BackupBeatId == ba.SecondaryBackupBeatId).ContractorId,
                                  TertiaryBackupContractorId = db.BackupProviders.FirstOrDefault(p => p.BackupBeatId == ba.TertiaryBackupBeatId).ContractorId,
                                  bu.RequestIsUrgent,
                                  bu.PrimaryBackupResponseTimeExpiredOrDeclined,
                                  bu.SecondaryBackupResponseTimeExpiredOrDeclined,
                                  bu.TertiaryBackupResponseTimeExpiredOrDeclined,
                                  bu.CreatedOn,
                                  BackupResponses = db.BackupResponses.Where(p => p.BackupRequestId == bu.Id).ToList(),
                                  bu.AllBackupsNotified
                              }).ToList();

                var data = allBus.Select(bu => new
                {
                    bu.Id,
                    bu.RequestNumber,
                    RequestingContractor = contractors.FirstOrDefault(c => c.ContractorId == bu.ContractorId),
                    bu.SelectedBackupContractorId,
                    bu.CurrentBackupContractorId,
                    BeatNeedingBackup = beats.FirstOrDefault(beat => beat.BeatId == bu.BeatId),
                    PrimaryBackupContractor = contractors.FirstOrDefault(c => c.ContractorId == bu.PrimaryBackupContractorId),
                    SecondaryBackupContractor = contractors.FirstOrDefault(c => c.ContractorId == bu.SecondaryBackupContractorId),
                    TertiaryBackupContractor = contractors.FirstOrDefault(c => c.ContractorId == bu.TertiaryBackupContractorId),
                    bu.RequestIsUrgent,
                    bu.PrimaryBackupResponseTimeExpiredOrDeclined,
                    PrimaryBackupResponded = bu.BackupResponses.Any(p => p.ContractorId == bu.PrimaryBackupContractorId),
                    bu.SecondaryBackupResponseTimeExpiredOrDeclined,
                    SecondaryBackupResponded = bu.BackupResponses.Any(p => p.ContractorId == bu.SecondaryBackupContractorId),
                    bu.TertiaryBackupResponseTimeExpiredOrDeclined,
                    TertiaryBackupResponded = bu.BackupResponses.Any(p => p.ContractorId == bu.TertiaryBackupContractorId),
                    bu.CreatedOn,
                    BackupAccepted = bu.BackupResponses.Any(p => p.BackupResponseStatus == 0),
                    bu.AllBackupsNotified,
                    BackupRequestShiftsAndDates = db.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == bu.Id).Select(s => new
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
                    }).ToList()
                });

                if (contractorId != null)
                    data = data.Where(d => d.CurrentBackupContractorId == contractorId && d.AllBackupsNotified == false ||
                                           d.PrimaryBackupResponded && d.SecondaryBackupResponded && d.TertiaryBackupResponded && d.BackupAccepted == false ||
                                           d.PrimaryBackupResponseTimeExpiredOrDeclined && d.SecondaryBackupResponseTimeExpiredOrDeclined && d.TertiaryBackupResponseTimeExpiredOrDeclined && d.AllBackupsNotified);

                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupAssignemtByBeat(Guid beatId)
        {
            using (var db = new MTCDbContext())
            {
                var contractors = GetContractorList();
                var beats = GetBeatList();
                var backupBeats = db.BackupBeats.ToList();
                var backupProviders = db.BackupProviders.ToList();

                var data = db.BackupAssignments.Where(p => p.BeatId == beatId).ToList();

                var returnList = new List<BackupAssignmentViewModel>();
                foreach (var ba in data)
                {
                    var primaryBackupBeatId = backupBeats.FirstOrDefault(t => t.Id == ba.PrimaryBackupBeatId)?.BeatId;
                    var primaryBackupContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.PrimaryBackupBeatId)?.ContractorId;

                    var secondaryBackupBeatId = backupBeats.FirstOrDefault(t => t.Id == ba.SecondaryBackupBeatId)?.BeatId;
                    var secondaryBackupContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.SecondaryBackupBeatId)?.ContractorId;

                    var tertiaryBackupBeatId = backupBeats.FirstOrDefault(t => t.Id == ba.TertiaryBackupBeatId)?.BeatId;
                    var tertiaryBackupContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.TertiaryBackupBeatId)?.ContractorId;

                    var m = new BackupAssignmentViewModel
                    {
                        Id = ba.Id,
                        BeatId = ba.BeatId,
                        BeatNumber = beats.FirstOrDefault(b => b.BeatId == ba.BeatId)?.BeatNumber,
                        PrimaryBackupBeatId = ba.PrimaryBackupBeatId,
                        PrimaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == primaryBackupBeatId)?.BeatNumber,
                        PrimaryBackupContractorId = primaryBackupContractorId,
                        PrimaryBackupContractorName = contractors.FirstOrDefault(p => p.ContractorId == primaryBackupContractorId)?.ContractorCompanyName,
                        SecondaryBackupBeatId = ba.SecondaryBackupBeatId,
                        SecondaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == secondaryBackupBeatId)?.BeatNumber,
                        SecondaryBackupContractorId = secondaryBackupContractorId,
                        SecondaryBackupContractorName = contractors.FirstOrDefault(p => p.ContractorId == secondaryBackupContractorId)?.ContractorCompanyName,
                        TertiaryBackupBeatId = ba.TertiaryBackupBeatId,
                        TertiaryBackupBeatNumber = beats.FirstOrDefault(b => b.BeatId == tertiaryBackupBeatId)?.BeatNumber,
                        TertiaryBackupContractorId = tertiaryBackupContractorId,
                        TertiaryBackupContractorName = contractors.FirstOrDefault(p => p.ContractorId == tertiaryBackupContractorId)?.ContractorCompanyName
                    };
                    returnList.Add(m);
                }

                return Json(returnList.FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBackupRequestStatuses()
        {
            using (var db = new MTCDbContext())
            {
                var beats = GetBeatList();
                var contractors = GetContractorList();
                var backupResponses = db.BackupResponses.ToList().Select(r => new
                {
                    r.Id,
                    r.BackupRequestId,
                    r.BackupResponseStatus,
                    r.Comments,
                    r.ContractorId,
                    contractors.FirstOrDefault(c => c.ContractorId == r.ContractorId)?.ContractorCompanyName,
                    r.CreatedBy,
                    r.CreatedOn,
                    r.ModifiedBy,
                    r.ModifiedOn,
                    r.BackupDeclinationReasonId,
                    r.BackupAssignmentLevel
                });
                var cancellationReasons = db.BackupCancellationReasons.ToList().Select(c => new
                {
                    c.Id,
                    Text = c.ReasonCode + " - " + c.Reason
                });


                var data = db.BackupRequests.ToList().Select(b => new
                {
                    b.Id,
                    b.RequestNumber,
                    b.ContractorId,
                    contractors.FirstOrDefault(c => c.ContractorId == b.ContractorId)?.ContractorCompanyName,
                    b.CreatedOn,
                    b.BeatId,
                    beats?.Where(p => p.BeatId == b.BeatId)?.FirstOrDefault()?.BeatNumber,
                    BackupResponses = backupResponses.Where(p => p.BackupRequestId == b.Id).ToList(),
                    HasPermissionToCancel = b.ContractorId == UsersContractorId || User.IsInRole("Admin"),
                    b.IsCancelled,
                    b.CancellationComment,
                    b.CancelledBy,
                    b.CancelledOn,
                    b.PrimaryBackupResponseTimeExpiredOrDeclined,
                    b.SecondaryBackupResponseTimeExpiredOrDeclined,
                    b.TertiaryBackupResponseTimeExpiredOrDeclined,
                    BackupCancellationReason = b.BackupCancellationReasonId != null ? cancellationReasons.FirstOrDefault(p => p.Id == b.BackupCancellationReasonId)?.Text : string.Empty,
                    IsResolved = db.BackupResponses.Any(p => p.BackupRequestId == b.Id && p.BackupResponseStatus == BackupResponseStatus.Accepted),
                    IsPartiallyResolved = db.BackupResponses.Any(p => p.BackupRequestId == b.Id && p.BackupResponseStatus == BackupResponseStatus.Qualified),
                    BackupRequestShiftsAndDates = db.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == b.Id).Select(s => new
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
                    }).ToList()
                }).ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RequestBackup(BackupRequest model, List<BackupRequestShiftAndDate> BackupDatesAndShifts)
        {
            var retvalue = true;

            using (var db = new MTCDbContext())
            {
                var currentMonth = DateTime.Today.ToString("MM");

                model.RequestNumber = currentMonth + (db.BackupRequests.Count(p => p.RequestNumber.StartsWith(currentMonth)) + 1).ToString().PadLeft(2, "0"[0]);
                model.CreatedOn = DateTime.Now;
                model.LastExpiredOn = DateTime.Now;
                model.CreatedBy = HttpContext.User.Identity.Name;
                model.ModifiedOn = DateTime.Now;
                model.ModifiedBy = HttpContext.User.Identity.Name;
                model.IsCancelled = false;

                if (BackupDatesAndShifts != null)
                    foreach (var item in BackupDatesAndShifts)
                        db.BackupRequestShiftsAndDates.Add(item);

                Contractor toContractor = null;
                var backupProviders = db.BackupProviders.ToList();
                var ba = db.BackupAssignments.FirstOrDefault(p => p.BeatId == model.BeatId);
                var assignmentLevel = BackupAssignmentLevel.Primary;

                if (model.SelectedBackupContractorId == Guid.Empty)
                {
                    //no prior arrangements have been made. Get the primary back-up for this contractorId and beatId combination.
                    if (ba != null)
                    {
                        var baProvider = backupProviders.FirstOrDefault(p => p.BackupBeatId == ba.PrimaryBackupBeatId);
                        if (baProvider != null)
                            model.SelectedBackupContractorId = baProvider.ContractorId;
                        model.SelectedBackupContractorAssignmentLevel = assignmentLevel;
                        toContractor = EmailManager.GetContractorById(model.SelectedBackupContractorId);
                    }
                }
                else
                {
                    toContractor = EmailManager.GetContractorById(model.SelectedBackupContractorId);
                    assignmentLevel = model.SelectedBackupContractorAssignmentLevel;
                }

                model.CurrentBackupContractorId = model.SelectedBackupContractorId;
                model.CurrentBackupContractorAssignmentLevel = model.SelectedBackupContractorAssignmentLevel;
                db.BackupRequests.Add(model);
                db.SaveChanges();


                var requestingContractor = EmailManager.GetContractorById(model.ContractorId);
                var toRecipients = new List<MTCEmailRecipient>();
                var ccRecipients = new List<MTCEmailRecipient>();

                toRecipients.Add(new MTCEmailRecipient
                {
                    Email = toContractor?.Email,
                    Name = toContractor?.ContractCompanyName
                });

                ccRecipients.Add(new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                });
                ccRecipients.Add(new MTCEmailRecipient
                {
                    Email = requestingContractor.Email,
                    Name = requestingContractor.ContractCompanyName
                });

                var body = EmailManager.BuildBackupRequestEmail(model);

                EmailManager.SendEmail(toRecipients, EmailManager.BuildBackupRequestSubject(model.RequestNumber, assignmentLevel), body, ccRecipients);
            }
            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RespondToBackupRequest(BackupResponse model, List<BackupRequestShiftAndDate> BackupDatesAndShifts)
        {
            var retvalue = true;
            using (var db = new MTCDbContext())
            {
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = HttpContext.User.Identity.Name;
                model.ModifiedOn = DateTime.Now;
                model.ModifiedBy = HttpContext.User.Identity.Name;

                db.BackupResponses.Add(model);

                if (model.BackupResponseStatus == BackupResponseStatus.Qualified)
                    if (BackupDatesAndShifts != null)
                        foreach (var item in BackupDatesAndShifts)
                        {
                            var baDs = db.BackupRequestShiftsAndDates.Find(item.Id);
                            if (baDs == null) continue;

                            if (item.AMRequested)
                                baDs.AMSatisfied = item.AMSatisfied;

                            if (item.MIDRequested)
                                baDs.MIDSatisfied = item.MIDSatisfied;

                            if (item.PMRequested)
                                baDs.PMSatisfied = item.PMSatisfied;
                        }


                db.SaveChanges();

                var br = db.BackupRequests.Find(model.BackupRequestId);
                var backupProviders = db.BackupProviders.ToList();
                var ba = db.BackupAssignments.FirstOrDefault(p => p.BeatId == br.BeatId);

                var toContractor = EmailManager.GetContractorById(br.ContractorId);

                var primaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.PrimaryBackupBeatId).ContractorId;
                var primaryContractor = EmailManager.GetContractorById(primaryContractorId);
                var secondaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.SecondaryBackupBeatId).ContractorId;
                var secondaryContractor = EmailManager.GetContractorById(secondaryContractorId);
                var tertiaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.TertiaryBackupBeatId).ContractorId;
                var tertiaryContractor = EmailManager.GetContractorById(tertiaryContractorId);

                var ccRecipients = new List<MTCEmailRecipient>();
                var toRecipients = new List<MTCEmailRecipient>();

                if (model.BackupResponseStatus == BackupResponseStatus.Declined || model.BackupResponseStatus == BackupResponseStatus.Qualified)
                {
                    #region

                    if (br.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.Primary)
                    {
                        if (model.BackupResponseStatus == BackupResponseStatus.Declined)
                        {
                            br.PrimaryBackupResponseTimeExpiredOrDeclined = true;
                            br.PrimaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                        }

                        br.CurrentBackupContractorId = secondaryContractorId;
                        br.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.Secondary;
                    }
                    else if (br.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.Secondary)
                    {
                        if (model.BackupResponseStatus == BackupResponseStatus.Declined)
                        {
                            br.SecondaryBackupResponseTimeExpiredOrDeclined = true;
                            br.SecondaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                        }

                        br.CurrentBackupContractorId = tertiaryContractorId;
                        br.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.Tertiary;
                    }
                    else if (br.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.Tertiary)
                    {
                        if (model.BackupResponseStatus == BackupResponseStatus.Declined)
                        {
                            br.TertiaryBackupResponseTimeExpiredOrDeclined = true;
                            br.TertiaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                        }

                        br.CurrentBackupContractorId = null;
                        br.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.AllBackupOperators;

                        foreach (var backupProvider in backupProviders)
                        {
                            var backupProviderContractor = EmailManager.GetContractorById(backupProvider.ContractorId);
                            ccRecipients.Add(new MTCEmailRecipient
                            {
                                Email = backupProviderContractor.Email,
                                Name = backupProviderContractor.ContractCompanyName
                            });
                        }
                    }

                    #endregion

                    br.ModifiedBy = HttpContext.User.Identity.Name;
                    br.ModifiedOn = DateTime.Now;

                    if (model.BackupResponseStatus == BackupResponseStatus.Qualified)
                    {
                        //check if maybe now all shifts have been covered. then set status to accepted.

                        var backupDatesAndShifts = db.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == br.Id).ToList();
                        var allShiftsSatisfied = true;

                        foreach (var item in backupDatesAndShifts)
                            if (item.AMSatisfied == false || item.MIDSatisfied == false || item.PMSatisfied == false)
                                allShiftsSatisfied = false;

                        if (allShiftsSatisfied)
                            model.BackupResponseStatus = BackupResponseStatus.Accepted;
                    }

                    db.SaveChanges();
                }

                toRecipients.Add(new MTCEmailRecipient
                {
                    Email = toContractor.Email,
                    Name = toContractor.ContractCompanyName
                });

                ccRecipients.Add(new MTCEmailRecipient { Email = primaryContractor.Email, Name = primaryContractor.ContractCompanyName });
                ccRecipients.Add(new MTCEmailRecipient { Email = secondaryContractor.Email, Name = secondaryContractor.ContractCompanyName });
                ccRecipients.Add(new MTCEmailRecipient { Email = tertiaryContractor.Email, Name = tertiaryContractor.ContractCompanyName });
                ccRecipients.Add(new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                });

                EmailManager.SendEmail(toRecipients, EmailManager.BuildBackupResponseSubject(br, model), EmailManager.BuildBackupResponseEmail(br, model), ccRecipients);
            }
            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CancelBackupRequest(int Id, int cancellationReasonId, string comments)
        {            
            using (var db = new MTCDbContext())
            {
                var br = db.BackupRequests.Find(Id);
                if (br == null) return Json(false, JsonRequestBehavior.AllowGet);

                br.IsCancelled = true;
                br.BackupCancellationReasonId = cancellationReasonId;
                br.CancellationComment = comments;
                br.CancelledBy = HttpContext.User.Identity.Name;
                br.CancelledOn = DateTime.Now;
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}