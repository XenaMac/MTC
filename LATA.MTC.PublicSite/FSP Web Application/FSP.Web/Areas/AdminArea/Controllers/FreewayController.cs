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
    public class FreewayController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.Freeways.OrderBy(p => p.FreewayName).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(int id)
        {
            Freeway Freeway = db.Freeways.Single(r => r.FreewayID == id);
            if (Freeway == null)
            {
                return HttpNotFound();
            }
            return View(Freeway);
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
        public ActionResult Create(Freeway Freeway)
        {
            if (ModelState.IsValid)
            {
                if (db.Freeways.Any(p => p.FreewayID == Freeway.FreewayID) == false)
                {
                    //if Freeway does not exist yet                
                    db.Freeways.InsertOnSubmit(Freeway);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(Freeway);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(int id)
        {
            Freeway Freeway = db.Freeways.Single(r => r.FreewayID == id);
            if (Freeway == null)
            {
                return HttpNotFound();
            }
            return View(Freeway);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(Freeway Freeway)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Freeways.Attach(Freeway);
                    db.Refresh(RefreshMode.KeepCurrentValues, Freeway);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(Freeway);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(int id)
        {
            Freeway Freeway = db.Freeways.Single(r => r.FreewayID == id);
            if (Freeway == null)
            {
                return HttpNotFound();
            }
            return View(Freeway);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Freeway Freeway = db.Freeways.Single(r => r.FreewayID == id);
            db.Freeways.DeleteOnSubmit(Freeway);
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