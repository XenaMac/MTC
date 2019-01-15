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
    public class DriverController : MyController
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {          
            List<Driver> data = (from q in db.Drivers
                                 orderby q.FirstName
                                 select q).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                data = data.Where(p => p.Contractor.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            return View(data);
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            Driver Driver = db.Drivers.Single(r => r.DriverID == id);
            if (Driver == null)
            {
                return HttpNotFound();
            }
            return View(Driver);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();
            
            ViewBag.Contractors = contractors;
            ViewBag.Beats = db.vBeats.OrderBy(p => p.BeatNumber).ToList();
            return View();
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(Driver Driver)
        {
            if (ModelState.IsValid)
            {

                //if Driver does not exist yet       
                Driver.DriverID = Guid.NewGuid();
                Driver.DateAdded = DateTime.Now;


                db.Drivers.InsertOnSubmit(Driver);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
           
            ViewBag.Beats = db.vBeats.OrderBy(p => p.BeatNumber).ToList();
            return View(Driver);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            var contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
                contractors = contractors.Where(p => p.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            ViewBag.Contractors = contractors;
            ViewBag.Beats = db.vBeats.OrderBy(p => p.BeatNumber).ToList();
            Driver Driver = db.Drivers.Single(r => r.DriverID == id);
            if (Driver == null)
            {
                return HttpNotFound();
            }
            return View(Driver);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(Driver Driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Drivers.Attach(Driver);
                    db.Refresh(RefreshMode.KeepCurrentValues, Driver);
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
            ViewBag.Beats = db.vBeats.OrderBy(p => p.BeatNumber).ToList();
            return View(Driver);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            Driver Driver = db.Drivers.Single(r => r.DriverID == id);
            if (Driver == null)
            {
                return HttpNotFound();
            }
            return View(Driver);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Driver Driver = db.Drivers.Single(r => r.DriverID == id);
            db.Drivers.DeleteOnSubmit(Driver);
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
