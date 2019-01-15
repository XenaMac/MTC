using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Filters;


namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class LocationController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/VehiclePositions/

        public ActionResult Index()
        {
            return View(db.Locations.OrderBy(p => p.Location1).ToList());
        }

        //
        // GET: /AdminArea/VehiclePositions/Details/5

        public ActionResult Details(Guid id)
        {
            Location Location = db.Locations.Single(r => r.LocationID == id);
            if (Location == null)
            {
                return HttpNotFound();
            }
            return View(Location);
        }

        //
        // GET: /AdminArea/VehiclePositions/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminArea/VehiclePositions/Create

        [HttpPost]
        public ActionResult Create(Location Location)
        {
            if (ModelState.IsValid)
            {
                //if Location does not exist yet       
                Location.LocationID = Guid.NewGuid();
                db.Locations.InsertOnSubmit(Location);
                db.SubmitChanges();

                return RedirectToAction("Index");
            }

            return View(Location);
        }

        //
        // GET: /AdminArea/VehiclePositions/Edit/5

        public ActionResult Edit(Guid id)
        {
            Location Location = db.Locations.Single(r => r.LocationID == id);
            if (Location == null)
            {
                return HttpNotFound();
            }
            return View(Location);
        }

        //
        // POST: /AdminArea/VehiclePositions/Edit/5

        [HttpPost]
        public ActionResult Edit(Location Location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Locations.Attach(Location);
                    db.Refresh(RefreshMode.KeepCurrentValues, Location);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(Location);
        }

        //
        // GET: /AdminArea/VehiclePositions/Delete/5

        public ActionResult Delete(Guid id)
        {
            Location Location = db.Locations.Single(r => r.LocationID == id);
            if (Location == null)
            {
                return HttpNotFound();
            }
            return View(Location);
        }

        //
        // POST: /AdminArea/VehiclePositions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Location Location = db.Locations.Single(r => r.LocationID == id);
            db.Locations.DeleteOnSubmit(Location);
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