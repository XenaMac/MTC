using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration
{
    [CustomAuthorize(Roles = "Admin,MTC,CHPOfficer,TowContractor,InVehicleContractor,DataConsultant")]
    public class AdministrationController : MtcBaseController
    {        
        public ActionResult AboutReport()
        {
            ViewBag.MTCContactEmail = Utilities.GetApplicationSettingValue("MTCMarketingContactEmail");
            ViewBag.MTCContactPhone = Utilities.GetApplicationSettingValue("MTCMarketingContactPhone");
            ViewBag.LATATraxSupportEmail = Utilities.GetApplicationSettingValue("LATATraxSupportEmail");
            ViewBag.LATATraxSupportPhone = Utilities.GetApplicationSettingValue("LATATraxSupportPhone");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ReportBug(UserReport userReport)
        {
            if (ModelState.IsValid)
            {
                userReport.CreatedOn = DateTime.UtcNow;
                userReport.CreatedBy = HttpContext.User.Identity.Name;
                userReport.UserReportType = UserReportType.Bug;

                using (MTCDbContext db = new MTCDbContext())
                {
                    db.UserReports.Add(userReport);
                    await db.SaveChangesAsync();

                    return RedirectToAction("ViewConfirmation", new { message = "Thank you! Your request has been processed." });
                }

            }
            return View("AboutReport");
        }

        [HttpPost]
        public async Task<ActionResult> ReportRequest(UserReport userReport, bool? isImprovement)
        {
            if (ModelState.IsValid)
            {
                userReport.CreatedOn = DateTime.UtcNow;
                userReport.CreatedBy = HttpContext.User.Identity.Name;
                userReport.UserReportType = isImprovement == true ? UserReportType.Improvement : UserReportType.NewFeature;

                using (MTCDbContext db = new MTCDbContext())
                {
                    db.UserReports.Add(userReport);
                    await db.SaveChangesAsync();

                    return RedirectToAction("ViewConfirmation", new { message = "Thank you! Your request has been processed." });
                }

            }
            return View("AboutReport");
        }

    }
}