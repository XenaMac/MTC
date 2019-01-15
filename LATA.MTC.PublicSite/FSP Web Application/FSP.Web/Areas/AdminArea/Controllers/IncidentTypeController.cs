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
    public class IncidentTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/IncidentTypes/

        public ActionResult Index()
        {
            return View(db.IncidentTypes.OrderBy(p => p.IncidentType1).ToList());
        }

        //
        // GET: /AdminArea/IncidentTypes/Details/5

        public ActionResult Details(Guid id)
        {
            IncidentType IncidentType = db.IncidentTypes.Single(r => r.IncidentTypeID == id);
            if (IncidentType == null)
            {
                return HttpNotFound();
            }
            return View(IncidentType);
        }

        //
        // GET: /AdminArea/IncidentTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminArea/IncidentTypes/Create

        [HttpPost]
        public ActionResult Create(IncidentType IncidentType)
        {
            if (ModelState.IsValid)
            {
                if (db.IncidentTypes.Any(p => p.IncidentType1 == IncidentType.IncidentType1) == false)
                {
                    //if IncidentType does not exist yet
                    IncidentType.IncidentTypeID = Guid.NewGuid();
                    db.IncidentTypes.InsertOnSubmit(IncidentType);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(IncidentType);
        }

        //
        // GET: /AdminArea/IncidentTypes/Edit/5

        public ActionResult Edit(Guid id)
        {
            IncidentType IncidentType = db.IncidentTypes.Single(r => r.IncidentTypeID == id);
            if (IncidentType == null)
            {
                return HttpNotFound();
            }
            return View(IncidentType);
        }

        //
        // POST: /AdminArea/IncidentTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(IncidentType IncidentType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.IncidentTypes.Attach(IncidentType);
                    db.Refresh(RefreshMode.KeepCurrentValues, IncidentType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(IncidentType);
        }

        //
        // GET: /AdminArea/IncidentTypes/Delete/5

        public ActionResult Delete(Guid id)
        {
            IncidentType IncidentType = db.IncidentTypes.Single(r => r.IncidentTypeID == id);
            if (IncidentType == null)
            {
                return HttpNotFound();
            }
            return View(IncidentType);
        }

        //
        // POST: /AdminArea/IncidentTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            IncidentType IncidentType = db.IncidentTypes.Single(r => r.IncidentTypeID == id);
            db.IncidentTypes.DeleteOnSubmit(IncidentType);
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