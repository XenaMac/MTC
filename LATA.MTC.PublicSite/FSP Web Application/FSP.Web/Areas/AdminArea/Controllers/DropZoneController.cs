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
    public class DropZoneController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            return View(db.vDropZones.OrderBy(p => p.DropZoneNumber).ToList());
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            vDropZone vDropZone = db.vDropZones.Single(r => r.DropZoneID == id);
            if (vDropZone == null)
            {
                return HttpNotFound();
            }
            return View(vDropZone);
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
        public ActionResult Create(vDropZone vDropZone)
        {
            if (ModelState.IsValid)
            {

                //if vDropZone does not exist yet       
                vDropZone.DropZoneID = Guid.NewGuid();
                db.vDropZones.InsertOnSubmit(vDropZone);
                db.SubmitChanges();


                return RedirectToAction("Index");
            }

            return View(vDropZone);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            vDropZone vDropZone = db.vDropZones.Single(r => r.DropZoneID == id);
            if (vDropZone == null)
            {
                return HttpNotFound();
            }
            return View(vDropZone);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(vDropZone vDropZone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.vDropZones.Attach(vDropZone);
                    db.Refresh(RefreshMode.KeepCurrentValues, vDropZone);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(vDropZone);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            vDropZone vDropZone = db.vDropZones.Single(r => r.DropZoneID == id);
            if (vDropZone == null)
            {
                return HttpNotFound();
            }
            return View(vDropZone);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            vDropZone vDropZone = db.vDropZones.Single(r => r.DropZoneID == id);
            db.vDropZones.DeleteOnSubmit(vDropZone);
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
