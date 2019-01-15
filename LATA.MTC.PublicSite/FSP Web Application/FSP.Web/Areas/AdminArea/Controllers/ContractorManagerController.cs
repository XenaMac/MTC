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
    public class ContractorManagerController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.ContractorManagers.OrderBy(p => p.Contractor.ContractCompanyName).ThenBy(p => p.LastName).ThenBy(p => p.FirstName).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            ContractorManager ContractorManager = db.ContractorManagers.Single(r => r.ContractorManagerID == id);
            if (ContractorManager == null)
            {
                return HttpNotFound();
            }
            return View(ContractorManager);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            return View();
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(ContractorManager ContractorManager)
        {
            if (ModelState.IsValid)
            {

                //if ContractorManager does not exist yet       
                ContractorManager.ContractorManagerID = Guid.NewGuid();
                db.ContractorManagers.InsertOnSubmit(ContractorManager);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            return View(ContractorManager);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            ContractorManager ContractorManager = db.ContractorManagers.Single(r => r.ContractorManagerID == id);
            if (ContractorManager == null)
            {
                return HttpNotFound();
            }
            return View(ContractorManager);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(ContractorManager ContractorManager)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ContractorManagers.Attach(ContractorManager);
                    db.Refresh(RefreshMode.KeepCurrentValues, ContractorManager);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            return View(ContractorManager);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            ContractorManager ContractorManager = db.ContractorManagers.Single(r => r.ContractorManagerID == id);
            if (ContractorManager == null)
            {
                return HttpNotFound();
            }
            return View(ContractorManager);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ContractorManager ContractorManager = db.ContractorManagers.Single(r => r.ContractorManagerID == id);
            db.ContractorManagers.DeleteOnSubmit(ContractorManager);
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
