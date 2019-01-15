using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin, CHP")]
    public class CHPInspectionController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.CHPInspections.OrderByDescending(p => p.InspectionDate).ThenBy(p => p.FleetVehicle.FleetNumber).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            CHPInspection CHPInspection = db.CHPInspections.Single(r => r.InspectionID == id);
            if (CHPInspection == null)
            {
                return HttpNotFound();
            }
            return View(CHPInspection);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.FleetVehicles = db.FleetVehicles.OrderBy(p => p.VehicleNumber).ToList();
            ViewBag.CHPOfficers = db.CHPOfficers.OrderBy(p => p.OfficerLastName).ToList();
            ViewBag.InspectionTypes = db.InspectionTypes.OrderBy(p => p.InspectionType1).ToList();
            return View();
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(CHPInspection CHPInspection)
        {
            if (ModelState.IsValid)
            {

                //if CHPInspection does not exist yet             
                CHPInspection.InspectionID = Guid.NewGuid();
                db.CHPInspections.InsertOnSubmit(CHPInspection);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.FleetVehicles = db.FleetVehicles.OrderBy(p => p.VehicleNumber).ToList();
            ViewBag.CHPOfficers = db.CHPOfficers.OrderBy(p => p.OfficerLastName).ToList();
            ViewBag.InspectionTypes = db.InspectionTypes.OrderBy(p => p.InspectionType1).ToList();

            return View(CHPInspection);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.FleetVehicles = db.FleetVehicles.OrderBy(p => p.VehicleNumber).ToList();
            ViewBag.CHPOfficers = db.CHPOfficers.OrderBy(p => p.OfficerLastName).ToList();
            ViewBag.InspectionTypes = db.InspectionTypes.OrderBy(p => p.InspectionType1).ToList();

            CHPInspection CHPInspection = db.CHPInspections.Single(r => r.InspectionID == id);
            if (CHPInspection == null)
            {
                return HttpNotFound();
            }


            return View(CHPInspection);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(CHPInspection CHPInspection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CHPInspections.Attach(CHPInspection);
                    db.Refresh(RefreshMode.KeepCurrentValues, CHPInspection);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.FleetVehicles = db.FleetVehicles.OrderBy(p => p.VehicleNumber).ToList();
            ViewBag.CHPOfficers = db.CHPOfficers.OrderBy(p => p.OfficerLastName).ToList();
            ViewBag.InspectionTypes = db.InspectionTypes.OrderBy(p => p.InspectionType1).ToList();

            return View(CHPInspection);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            CHPInspection CHPInspection = db.CHPInspections.Single(r => r.InspectionID == id);
            if (CHPInspection == null)
            {
                return HttpNotFound();
            }
            return View(CHPInspection);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CHPInspection CHPInspection = db.CHPInspections.Single(r => r.InspectionID == id);
            db.CHPInspections.DeleteOnSubmit(CHPInspection);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
