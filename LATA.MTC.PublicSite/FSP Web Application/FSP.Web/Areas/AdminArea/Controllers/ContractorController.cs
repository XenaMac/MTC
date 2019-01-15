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
    public class ContractorController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.Contractors.OrderBy(p => p.ContractCompanyName).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            Contractor Contractor = db.Contractors.Single(r => r.ContractorID == id);
            if (Contractor == null)
            {
                return HttpNotFound();
            }
            return View(Contractor);
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
        public ActionResult Create(Contractor Contractor)
        {
            if (ModelState.IsValid)
            {

                //if Contractor does not exist yet       
                Contractor.ContractorID = Guid.NewGuid();
                db.Contractors.InsertOnSubmit(Contractor);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            return View(Contractor);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            Contractor Contractor = db.Contractors.Single(r => r.ContractorID == id);
            if (Contractor == null)
            {
                return HttpNotFound();
            }
            return View(Contractor);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(Contractor Contractor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contractors.Attach(Contractor);
                    db.Refresh(RefreshMode.KeepCurrentValues, Contractor);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(Contractor);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            Contractor Contractor = db.Contractors.Single(r => r.ContractorID == id);
            if (Contractor == null)
            {
                return HttpNotFound();
            }
            return View(Contractor);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Contractor Contractor = db.Contractors.Single(r => r.ContractorID == id);
            db.Contractors.DeleteOnSubmit(Contractor);
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
