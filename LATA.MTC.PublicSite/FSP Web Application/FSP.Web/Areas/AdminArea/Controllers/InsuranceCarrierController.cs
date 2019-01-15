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
    [CustomAuthorization(Roles = "Admin")]
    public class InsuranceCarrierController : MyController
    {
        FSPDataContext db = new FSPDataContext();
      
        public ActionResult Index()
        {
           
            List<InsuranceCarrier> data = (from q in db.InsuranceCarriers
                                           orderby q.CarrierName
                                           select q).ToList();

            //if (!String.IsNullOrEmpty(this.UsersContractorCompanyName))
            //    data = data.Where(p => p.Contractor.ContractCompanyName == this.UsersContractorCompanyName).ToList();

            return View(data);
        }
       
        public ActionResult Details(Guid id)
        {
            InsuranceCarrier InsuranceCarrier = db.InsuranceCarriers.Single(r => r.InsuranceID == id);
            if (InsuranceCarrier == null)
            {
                return HttpNotFound();
            }
            return View(InsuranceCarrier);
        }       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(InsuranceCarrier InsuranceCarrier)
        {
            if (ModelState.IsValid)
            {
                if (!db.InsuranceCarriers.Any(p => p.CarrierName == InsuranceCarrier.CarrierName))
                {
                    //if InsuranceCarrier does not exist yet       
                    InsuranceCarrier.InsuranceID = Guid.NewGuid();
                    db.InsuranceCarriers.InsertOnSubmit(InsuranceCarrier);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(InsuranceCarrier);
        }
     

        public ActionResult Edit(Guid id)
        {
            InsuranceCarrier InsuranceCarrier = db.InsuranceCarriers.Single(r => r.InsuranceID == id);
            if (InsuranceCarrier == null)
            {
                return HttpNotFound();
            }
            return View(InsuranceCarrier);
        }        

        [HttpPost]
        public ActionResult Edit(InsuranceCarrier InsuranceCarrier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.InsuranceCarriers.Attach(InsuranceCarrier);
                    db.Refresh(RefreshMode.KeepCurrentValues, InsuranceCarrier);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(InsuranceCarrier);
        }
     
        public ActionResult Delete(Guid id)
        {
            InsuranceCarrier InsuranceCarrier = db.InsuranceCarriers.Single(r => r.InsuranceID == id);
            if (InsuranceCarrier == null)
            {
                return HttpNotFound();
            }
            return View(InsuranceCarrier);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InsuranceCarrier InsuranceCarrier = db.InsuranceCarriers.Single(r => r.InsuranceID == id);
            db.InsuranceCarriers.DeleteOnSubmit(InsuranceCarrier);
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
