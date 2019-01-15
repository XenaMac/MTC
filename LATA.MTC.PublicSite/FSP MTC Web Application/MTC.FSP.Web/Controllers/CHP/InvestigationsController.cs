using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.CHP
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class InvestigationsController : MtcBaseController
    {
        // GET: Investigation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInvestigations()
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var beats = this.GetBeatList();
                var contractors = this.GetContractorList();
                var drivers = this.GetDriverList();
                var chpOfficers = this.GetCHPOfficerList();

                var rawData = db.Investigations.OrderBy(p => p.Date).ToList();
                var data = new List<InvestigationViewModel>();

                foreach (var rawItem in rawData)
                {
                    try
                    {
                        InvestigationViewModel model = new InvestigationViewModel();
                        model.Id = rawItem.Id;
                        model.Date = rawItem.Date;
                        model.BeatId = rawItem.BeatId;
                        model.BeatNumber = rawItem.BeatId != null ? beats.FirstOrDefault(b => b.BeatId == rawItem.BeatId).BeatNumber : String.Empty;
                        model.ContractorId = rawItem.ContractorId;
                        model.ContractCompanyName = rawItem.ContractorId != null ? contractors.FirstOrDefault(b => b.ContractorId == rawItem.ContractorId).ContractorCompanyName : String.Empty;
                        model.DriverId = rawItem.DriverId;
                        model.DriverName = rawItem.DriverId != null ? drivers.FirstOrDefault(b => b.DriverId == rawItem.DriverId).DriverFullName : String.Empty;
                        model.ViolationTypeId = rawItem.ViolationTypeId;
                        model.ViolationTypeName = rawItem.ViolationType.Name;
                        model.Summary = rawItem.Summary;
                        model.CHPOfficerId = rawItem.CHPOfficerId;
                        model.InvestigatingOfficer = rawItem.CHPOfficerId != null ? chpOfficers.FirstOrDefault(b => b.Id == rawItem.CHPOfficerId).OfficeFullName : String.Empty;

                        data.Add(model);
                    }
                    catch { }
                }

                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveInvestigation(InvestigationViewModel model)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                Investigation investigation = null;
                bool isNew = false;
                if (model.Id > 0)
                {
                    investigation = db.Investigations.Find(model.Id);
                }
                else
                {
                    investigation = new Investigation();
                    investigation.CreatedOn = DateTime.Now;
                    investigation.CreatedBy = HttpContext.User.Identity.Name;
                    isNew = true;
                }

                investigation.Date = model.Date;
                investigation.DriverId = model.DriverId;
                investigation.BeatId = model.BeatId;
                investigation.ContractorId = model.ContractorId;
                investigation.CHPOfficerId = model.CHPOfficerId;
                investigation.Summary = model.Summary;
                investigation.ViolationTypeId = model.ViolationTypeId;

                investigation.ModifiedOn = DateTime.Now;
                investigation.ModifiedBy = HttpContext.User.Identity.Name;

                if (isNew)
                    db.Investigations.Add(investigation);

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult RemoveInvestigation(int id)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                Investigation investigation = db.Investigations.Find(id);
                if (investigation != null)
                {
                    db.Investigations.Remove(investigation);
                    db.SaveChanges();
                }

                return Json(true, JsonRequestBehavior.AllowGet);

            }

        }
    }
}