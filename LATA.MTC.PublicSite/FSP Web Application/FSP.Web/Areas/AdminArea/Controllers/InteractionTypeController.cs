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
    public class InteractionTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/VehiclePositions/

        public ActionResult Index()
        {
            return View(db.InteractionTypes.OrderBy(p => p.InteractionType1).ToList());
        }

        //
        // GET: /AdminArea/VehiclePositions/Details/5

        public ActionResult Details(Guid id)
        {
            InteractionType InteractionType = db.InteractionTypes.Single(r => r.InteractionTypeID == id);
            if (InteractionType == null)
            {
                return HttpNotFound();
            }
            return View(InteractionType);
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
        public ActionResult Create(InteractionType InteractionType)
        {
            if (ModelState.IsValid)
            {
                //if InteractionType does not exist yet       
                InteractionType.InteractionTypeID = Guid.NewGuid();
                db.InteractionTypes.InsertOnSubmit(InteractionType);
                db.SubmitChanges();

                return RedirectToAction("Index");
            }

            return View(InteractionType);
        }

        //
        // GET: /AdminArea/VehiclePositions/Edit/5

        public ActionResult Edit(Guid id)
        {
            InteractionType InteractionType = db.InteractionTypes.Single(r => r.InteractionTypeID == id);
            if (InteractionType == null)
            {
                return HttpNotFound();
            }
            return View(InteractionType);
        }

        //
        // POST: /AdminArea/VehiclePositions/Edit/5

        [HttpPost]
        public ActionResult Edit(InteractionType InteractionType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.InteractionTypes.Attach(InteractionType);
                    db.Refresh(RefreshMode.KeepCurrentValues, InteractionType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(InteractionType);
        }

        //
        // GET: /AdminArea/VehiclePositions/Delete/5

        public ActionResult Delete(Guid id)
        {
            InteractionType InteractionType = db.InteractionTypes.Single(r => r.InteractionTypeID == id);
            if (InteractionType == null)
            {
                return HttpNotFound();
            }
            return View(InteractionType);
        }

        //
        // POST: /AdminArea/VehiclePositions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InteractionType InteractionType = db.InteractionTypes.Single(r => r.InteractionTypeID == id);
            db.InteractionTypes.DeleteOnSubmit(InteractionType);
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
