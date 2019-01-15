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
    [CustomAuthorization(Roles = "Admin")]
    public class VehicleTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.VehicleTypes.OrderBy(p => p.VehicleType1).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            VehicleType VehicleType = db.VehicleTypes.Single(r => r.VehicleTypeID == id);
            if (VehicleType == null)
            {
                return HttpNotFound();
            }
            return View(VehicleType);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(VehicleType VehicleType)
        {
            if (ModelState.IsValid)
            {
                if (!db.VehicleTypes.Any(p => p.VehicleType1 == VehicleType.VehicleType1))
                {
                    //if VehicleType does not exist yet       
                    VehicleType.VehicleTypeID = Guid.NewGuid();
                    db.VehicleTypes.InsertOnSubmit(VehicleType);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(VehicleType);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            VehicleType VehicleType = db.VehicleTypes.Single(r => r.VehicleTypeID == id);
            if (VehicleType == null)
            {
                return HttpNotFound();
            }
            return View(VehicleType);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(VehicleType VehicleType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.VehicleTypes.Attach(VehicleType);
                    db.Refresh(RefreshMode.KeepCurrentValues, VehicleType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(VehicleType);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            VehicleType VehicleType = db.VehicleTypes.Single(r => r.VehicleTypeID == id);
            if (VehicleType == null)
            {
                return HttpNotFound();
            }
            return View(VehicleType);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VehicleType VehicleType = db.VehicleTypes.Single(r => r.VehicleTypeID == id);
            db.VehicleTypes.DeleteOnSubmit(VehicleType);
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
