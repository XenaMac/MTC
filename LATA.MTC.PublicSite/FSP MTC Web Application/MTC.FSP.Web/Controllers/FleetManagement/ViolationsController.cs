using System;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    public class ViolationsController : MtcBaseController
    {
        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,DataConsultant")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,DataConsultant")]
        public ActionResult GetViolations(ViolationsSearchQuery query)
        {
            var currPage = query.Page.GetValueOrDefault(0);
            var currPageSize = query.PageSize.GetValueOrDefault(10);

            using (var db = new MTCDbContext())
            {
                var beats = this.GetBeatList();
                var contractors = this.GetContractorList();
                var drivers = this.GetDriverList();
                var fleetVehicles = this.GetVehicleList();

                var relavantViolations = db.Violations
                                        .Where(p => p.CreatedOn >= _threeMonthsAgo || p.ViolationStatusType.Name == "Not Checked")
                                        .Select(s => new
                                        {
                                            s.Id,
                                            s.ViolationTypeId,
                                            ViolationTypeCode = s.ViolationType.Code,
                                            ViolationTypeName = s.ViolationType.Name,
                                            ViolationSeverity = s.ViolationType.ViolationTypeSeverity,
                                            s.OffenseNumber,
                                            s.ContractorId,
                                            s.DateTimeOfViolation,
                                            s.BeatId,
                                            s.DriverId,
                                            s.FleetVehicleId,
                                            s.CallSign,
                                            s.ViolationStatusTypeId,
                                            ViolationStatusTypeName = s.ViolationStatusType.Name,
                                            s.DeductionAmount,
                                            s.Notes,
                                            s.PenaltyForDriver,
                                            s.AlarmName,
                                            s.AlertTime,
                                            s.LengthOfViolation,
                                            s.CreatedBy,
                                            s.CreatedOn
                                        });

                #region query


                if (query.StartDate != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.DateTimeOfViolation >= query.StartDate);
                }
                if (query.EndDate != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.DateTimeOfViolation <= query.EndDate);
                }
                if (!string.IsNullOrEmpty(query.CallSign))
                {
                    relavantViolations = relavantViolations.Where(p => p.CallSign.Contains(query.CallSign));
                }
                if (query.ViolationTypeId != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.ViolationTypeId == query.ViolationTypeId);
                }
                if (query.DriverId != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.DriverId == query.DriverId);
                }
                if (query.BeatId != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.BeatId == query.BeatId);
                }
                if (query.VehicleId != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.FleetVehicleId == query.VehicleId);
                }
                if (query.ContractorId != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.ContractorId == query.ContractorId);
                }
                if (query.AlarmName != null)
                {
                    relavantViolations = relavantViolations.Where(p => p.AlarmName == query.AlarmName);
                }

                #endregion

                var filteredData = relavantViolations.OrderByDescending(p => p.CreatedOn)
                    .Skip(currPage * currPageSize)
                    .Take(currPageSize)
                    .ToList();

                var data = filteredData.Select(s => new ViolationViewModel
                {
                    Id = s.Id,
                    ViolationTypeId = s.ViolationTypeId,
                    ViolationTypeCode = s.ViolationTypeCode,
                    ViolationTypeName = s.ViolationTypeName,
                    ViolationSeverity = Enum.GetName(typeof(ViolationTypeSeverity), s.ViolationSeverity),
                    OffenseNumber = s.OffenseNumber,
                    ContractorId = s.ContractorId,
                    DateTimeOfViolation = s.DateTimeOfViolation,
                    BeatId = s.BeatId,
                    DriverId = s.DriverId,
                    FleetVehicleId = s.FleetVehicleId,
                    CallSign = s.CallSign,
                    ViolationStatusTypeId = s.ViolationStatusTypeId,
                    ViolationStatusTypeName = s.ViolationStatusTypeName,
                    DeductionAmount = s.DeductionAmount,
                    Notes = s.Notes,
                    PenaltyForDriver = s.PenaltyForDriver,
                    AlarmName = s.AlarmName,
                    AlertTime = s.AlertTime,
                    LengthOfViolation = s.LengthOfViolation,
                    CreatedBy = s.CreatedBy,
                    CreatedOn = s.CreatedOn,
                    ContractCompanyName = contractors.FirstOrDefault(b => b.ContractorId == s.ContractorId)?.ContractorCompanyName,
                    BeatNumber = beats.FirstOrDefault(b => b.BeatId == s.BeatId)?.BeatNumber,
                    DriverName = drivers.FirstOrDefault(b => b.DriverId == s.DriverId)?.DriverFullName,
                    DriverFspIdNumber = drivers.FirstOrDefault(b => b.DriverId == s.DriverId)?.FSPIDNumber,
                    TruckNumber = fleetVehicles.FirstOrDefault(b => b.FleetVehicleId == s.FleetVehicleId)?.VehicleNumber
                });

                var totalCount = relavantViolations.Count();
                var dataPaged = new
                {
                    Page = currPage,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                    Items = data.ToList()
                };

                return Json(dataPaged, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
        [HttpPost]
        public ActionResult SaveViolation(ViolationViewModel model)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                Violation Violation = null;
                bool isNew = false;
                if (model.Id > 0)
                {
                    Violation = db.Violations.Find(model.Id);
                }
                else
                {
                    Violation = new Violation();
                    Violation.CreatedOn = DateTime.Now;
                    Violation.CreatedBy = HttpContext.User.Identity.Name;
                    isNew = true;
                }

                Violation.ViolationTypeId = model.ViolationTypeId;
                Violation.ContractorId = model.ContractorId;
                Violation.DateTimeOfViolation = model.DateTimeOfViolation;

                Violation.BeatId = model.BeatId;
                Violation.DriverId = model.DriverId;
                Violation.FleetVehicleId = model.FleetVehicleId;
                Violation.CallSign = model.CallSign;
                Violation.OffenseNumber = model.OffenseNumber;
                Violation.ViolationStatusTypeId = model.ViolationStatusTypeId;
                Violation.DeductionAmount = model.DeductionAmount;
                Violation.Notes = model.Notes;
                Violation.PenaltyForDriver = model.PenaltyForDriver;
                Violation.LengthOfViolation = "0";

                Violation.ModifiedOn = DateTime.Now;
                Violation.ModifiedBy = HttpContext.User.Identity.Name;

                if (isNew)
                    db.Violations.Add(Violation);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
        public ActionResult RemoveViolation(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                Violation Violation = db.Violations.Find(id);
                if (Violation != null)
                {
                    db.Violations.Remove(Violation);
                    db.SaveChanges();
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}