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
    [CustomAuthorization(Roles = "Admin, CHP")]
    public class CHPOfficerController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.CHPOfficers.OrderBy(p => p.OfficerFirstName).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(String id)
        {
            CHPOfficer CHPOfficer = db.CHPOfficers.Single(r => r.BadgeID == id);
            if (CHPOfficer == null)
            {
                return HttpNotFound();
            }
            return View(CHPOfficer);
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
        public ActionResult Create(CHPOfficer CHPOfficer)
        {
            if (ModelState.IsValid)
            {

                //if CHPOfficer does not exist yet                      
                db.CHPOfficers.InsertOnSubmit(CHPOfficer);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            return View(CHPOfficer);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(String id)
        {
            CHPOfficer CHPOfficer = db.CHPOfficers.Single(r => r.BadgeID == id);
            if (CHPOfficer == null)
            {
                return HttpNotFound();
            }
            return View(CHPOfficer);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(CHPOfficer CHPOfficer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CHPOfficers.Attach(CHPOfficer);
                    db.Refresh(RefreshMode.KeepCurrentValues, CHPOfficer);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(CHPOfficer);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(String id)
        {
            CHPOfficer CHPOfficer = db.CHPOfficers.Single(r => r.BadgeID == id);
            if (CHPOfficer == null)
            {
                return HttpNotFound();
            }
            return View(CHPOfficer);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(String id)
        {
            CHPOfficer CHPOfficer = db.CHPOfficers.Single(r => r.BadgeID == id);
            db.CHPOfficers.DeleteOnSubmit(CHPOfficer);
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
