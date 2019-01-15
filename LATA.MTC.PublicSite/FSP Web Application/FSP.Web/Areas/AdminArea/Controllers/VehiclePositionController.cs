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
    public class VehiclePositionController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/VehiclePositions/

        public ActionResult Index()
        {
            return View(db.VehiclePositions.OrderBy(p => p.VehiclePosition1).ToList());
        }

        //
        // GET: /AdminArea/VehiclePositions/Details/5

        public ActionResult Details(Guid id)
        {
            VehiclePosition VehiclePosition = db.VehiclePositions.Single(r => r.VehiclePositionID == id);
            if (VehiclePosition == null)
            {
                return HttpNotFound();
            }
            return View(VehiclePosition);
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
        public ActionResult Create(VehiclePosition VehiclePosition)
        {
            if (ModelState.IsValid)
            {
                if (db.VehiclePositions.Any(p => p.VehiclePosition1 == VehiclePosition.VehiclePosition1) == false)
                {
                    //if VehiclePosition does not exist yet
                    VehiclePosition.VehiclePositionID = Guid.NewGuid();
                    db.VehiclePositions.InsertOnSubmit(VehiclePosition);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(VehiclePosition);
        }

        //
        // GET: /AdminArea/VehiclePositions/Edit/5

        public ActionResult Edit(Guid id)
        {
            VehiclePosition VehiclePosition = db.VehiclePositions.Single(r => r.VehiclePositionID == id);
            if (VehiclePosition == null)
            {
                return HttpNotFound();
            }
            return View(VehiclePosition);
        }

        //
        // POST: /AdminArea/VehiclePositions/Edit/5

        [HttpPost]
        public ActionResult Edit(VehiclePosition VehiclePosition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.VehiclePositions.Attach(VehiclePosition);
                    db.Refresh(RefreshMode.KeepCurrentValues, VehiclePosition);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(VehiclePosition);
        }

        //
        // GET: /AdminArea/VehiclePositions/Delete/5

        public ActionResult Delete(Guid id)
        {
            VehiclePosition VehiclePosition = db.VehiclePositions.Single(r => r.VehiclePositionID == id);
            if (VehiclePosition == null)
            {
                return HttpNotFound();
            }
            return View(VehiclePosition);
        }

        //
        // POST: /AdminArea/VehiclePositions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VehiclePosition VehiclePosition = db.VehiclePositions.Single(r => r.VehiclePositionID == id);
            db.VehiclePositions.DeleteOnSubmit(VehiclePosition);
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