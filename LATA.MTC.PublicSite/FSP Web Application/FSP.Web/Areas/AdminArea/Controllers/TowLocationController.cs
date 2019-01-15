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
    public class TowLocationController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.TowLocations.OrderBy(p => p.TowLocation1).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            TowLocation TowLocation = db.TowLocations.Single(r => r.TowLocationID == id);
            if (TowLocation == null)
            {
                return HttpNotFound();
            }
            return View(TowLocation);
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
        public ActionResult Create(TowLocation TowLocation)
        {
            if (ModelState.IsValid)
            {
                if (!db.TowLocations.Any(p => p.TowLocation1 == TowLocation.TowLocation1))
                {
                    //if TowLocation does not exist yet       
                    TowLocation.TowLocationID = Guid.NewGuid();
                    db.TowLocations.InsertOnSubmit(TowLocation);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(TowLocation);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            TowLocation TowLocation = db.TowLocations.Single(r => r.TowLocationID == id);
            if (TowLocation == null)
            {
                return HttpNotFound();
            }
            return View(TowLocation);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(TowLocation TowLocation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.TowLocations.Attach(TowLocation);
                    db.Refresh(RefreshMode.KeepCurrentValues, TowLocation);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(TowLocation);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            TowLocation TowLocation = db.TowLocations.Single(r => r.TowLocationID == id);
            if (TowLocation == null)
            {
                return HttpNotFound();
            }
            return View(TowLocation);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TowLocation TowLocation = db.TowLocations.Single(r => r.TowLocationID == id);
            db.TowLocations.DeleteOnSubmit(TowLocation);
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
