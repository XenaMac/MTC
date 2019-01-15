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
    public class InspectionTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/InspectionTypes/

        public ActionResult Index()
        {
            return View(db.InspectionTypes.OrderBy(p => p.InspectionType1).ToList());
        }

        //
        // GET: /AdminArea/InspectionTypes/Details/5

        public ActionResult Details(Guid id)
        {
            InspectionType InspectionType = db.InspectionTypes.Single(r => r.InspectionTypeID == id);
            if (InspectionType == null)
            {
                return HttpNotFound();
            }
            return View(InspectionType);
        }

        //
        // GET: /AdminArea/InspectionTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminArea/InspectionTypes/Create

        [HttpPost]
        public ActionResult Create(InspectionType InspectionType)
        {
            if (ModelState.IsValid)
            {
                if (db.InspectionTypes.Any(p => p.InspectionTypeID == InspectionType.InspectionTypeID) == false)
                {
                    //if InspectionType does not exist yet                
                    db.InspectionTypes.InsertOnSubmit(InspectionType);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(InspectionType);
        }

        //
        // GET: /AdminArea/InspectionTypes/Edit/5

        public ActionResult Edit(Guid id)
        {
            InspectionType InspectionType = db.InspectionTypes.Single(r => r.InspectionTypeID == id);
            if (InspectionType == null)
            {
                return HttpNotFound();
            }
            return View(InspectionType);
        }

        //
        // POST: /AdminArea/InspectionTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(InspectionType InspectionType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.InspectionTypes.Attach(InspectionType);
                    db.Refresh(RefreshMode.KeepCurrentValues, InspectionType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(InspectionType);
        }

        //
        // GET: /AdminArea/InspectionTypes/Delete/5

        public ActionResult Delete(Guid id)
        {
            InspectionType InspectionType = db.InspectionTypes.Single(r => r.InspectionTypeID == id);
            if (InspectionType == null)
            {
                return HttpNotFound();
            }
            return View(InspectionType);
        }

        //
        // POST: /AdminArea/InspectionTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InspectionType InspectionType = db.InspectionTypes.Single(r => r.InspectionTypeID == id);
            db.InspectionTypes.DeleteOnSubmit(InspectionType);
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
