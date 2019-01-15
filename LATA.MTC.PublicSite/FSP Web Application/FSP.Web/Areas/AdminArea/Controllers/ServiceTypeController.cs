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
    public class ServiceTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.ServiceTypes.OrderBy(p => p.ServiceType1).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            ServiceType ServiceType = db.ServiceTypes.Single(r => r.ServiceTypeID == id);
            if (ServiceType == null)
            {
                return HttpNotFound();
            }
            return View(ServiceType);
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
        public ActionResult Create(ServiceType ServiceType)
        {
            if (ModelState.IsValid)
            {
                if (!db.ServiceTypes.Any(p => p.ServiceType1 == ServiceType.ServiceType1))
                {
                    //if ServiceType does not exist yet       
                    ServiceType.ServiceTypeID = Guid.NewGuid();
                    db.ServiceTypes.InsertOnSubmit(ServiceType);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(ServiceType);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            ServiceType ServiceType = db.ServiceTypes.Single(r => r.ServiceTypeID == id);
            if (ServiceType == null)
            {
                return HttpNotFound();
            }
            return View(ServiceType);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(ServiceType ServiceType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ServiceTypes.Attach(ServiceType);
                    db.Refresh(RefreshMode.KeepCurrentValues, ServiceType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(ServiceType);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            ServiceType ServiceType = db.ServiceTypes.Single(r => r.ServiceTypeID == id);
            if (ServiceType == null)
            {
                return HttpNotFound();
            }
            return View(ServiceType);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ServiceType ServiceType = db.ServiceTypes.Single(r => r.ServiceTypeID == id);
            db.ServiceTypes.DeleteOnSubmit(ServiceType);
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
