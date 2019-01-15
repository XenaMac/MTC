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
    [CustomAuthorization(Roles = "Admin, CHP")]
    public class DriverInteractionController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            List<DriverInteraction> data = (from q in db.DriverInteractions
                                            orderby q.Contractor.ContractCompanyName, q.Driver.LastName
                                            select q).ToList();

            return View(data);
        }

        public ActionResult Details(Guid id)
        {
            DriverInteraction DriverInteraction = db.DriverInteractions.Single(r => r.InteractionID == id);
            if (DriverInteraction == null)
            {
                return HttpNotFound();
            }
            return View(DriverInteraction);
        }

        public ActionResult Create()
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.Drivers = (from q in db.Drivers
                               select new
                               {
                                   FullName = q.LastName + ", " + q.FirstName,
                                   DriverID = q.DriverID
                               }).OrderBy(p => p.FullName);
            ViewBag.InteractionTypes = db.InteractionTypes.OrderBy(p => p.InteractionType1).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Create(DriverInteraction DriverInteraction)
        {
            if (ModelState.IsValid)
            {

                //if DriverInteraction does not exist yet       
                DriverInteraction.InteractionID = Guid.NewGuid();

                db.DriverInteractions.InsertOnSubmit(DriverInteraction);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.Drivers = (from q in db.Drivers
                               select new
                               {
                                   FullName = q.LastName + ", " + q.FirstName,
                                   DriverID = q.DriverID
                               }).OrderBy(p => p.FullName);
            ViewBag.InteractionTypes = db.InteractionTypes.OrderBy(p => p.InteractionType1).ToList();


            return View(DriverInteraction);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.Drivers = (from q in db.Drivers
                               select new
                               {
                                   FullName = q.LastName + ", " + q.FirstName,
                                   DriverID = q.DriverID
                               }).OrderBy(p => p.FullName);
            ViewBag.InteractionTypes = db.InteractionTypes.OrderBy(p => p.InteractionType1).ToList();

            DriverInteraction DriverInteraction = db.DriverInteractions.Single(r => r.InteractionID == id);
            if (DriverInteraction == null)
            {
                return HttpNotFound();
            }
            return View(DriverInteraction);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(DriverInteraction DriverInteraction)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DriverInteractions.Attach(DriverInteraction);
                    db.Refresh(RefreshMode.KeepCurrentValues, DriverInteraction);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            ViewBag.Drivers = (from q in db.Drivers
                               select new
                               {
                                   FullName = q.LastName + ", " + q.FirstName,
                                   DriverID = q.DriverID
                               }).OrderBy(p => p.FullName);
            ViewBag.InteractionTypes = db.InteractionTypes.OrderBy(p => p.InteractionType1).ToList();

            return View(DriverInteraction);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            DriverInteraction DriverInteraction = db.DriverInteractions.Single(r => r.InteractionID == id);
            if (DriverInteraction == null)
            {
                return HttpNotFound();
            }
            return View(DriverInteraction);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DriverInteraction DriverInteraction = db.DriverInteractions.Single(r => r.InteractionID == id);
            db.DriverInteractions.DeleteOnSubmit(DriverInteraction);
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
