using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Helpers;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class FleetVehicleController : MyController
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {                       
            List<FleetVehicle> data = (from q in db.FleetVehicles
                                      orderby q.VehicleNumber
                                      select q).ToList();
            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                data = data.Where(p => p.Contractor.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            return View(data);
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            FleetVehicle FleetVehicle = db.FleetVehicles.Single(r => r.FleetVehicleID == id);
            if (FleetVehicle == null)
            {
                return HttpNotFound();
            }
            return View(FleetVehicle);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
            return View();
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(FleetVehicle FleetVehicle)
        {
            if (ModelState.IsValid)
            {
                if (!db.FleetVehicles.Any(p => p.VehicleNumber == FleetVehicle.VehicleNumber))
                {
                    //if FleetVehicle does not exist yet       
                    FleetVehicle.FleetVehicleID = Guid.NewGuid();
                    db.FleetVehicles.InsertOnSubmit(FleetVehicle);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
            return View(FleetVehicle);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
            FleetVehicle FleetVehicle = db.FleetVehicles.Single(r => r.FleetVehicleID == id);
            if (FleetVehicle == null)
            {
                return HttpNotFound();
            }
            return View(FleetVehicle);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(FleetVehicle FleetVehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.FleetVehicles.Attach(FleetVehicle);
                    db.Refresh(RefreshMode.KeepCurrentValues, FleetVehicle);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
            return View(FleetVehicle);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            FleetVehicle FleetVehicle = db.FleetVehicles.Single(r => r.FleetVehicleID == id);
            if (FleetVehicle == null)
            {
                return HttpNotFound();
            }
            return View(FleetVehicle);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FleetVehicle FleetVehicle = db.FleetVehicles.Single(r => r.FleetVehicleID == id);
            db.FleetVehicles.DeleteOnSubmit(FleetVehicle);
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
