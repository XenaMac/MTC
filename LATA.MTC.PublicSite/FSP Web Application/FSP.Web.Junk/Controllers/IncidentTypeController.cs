using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Web.Junk.Models;

namespace FSP.Web.Junk.Controllers
{
    public class IncidentTypeController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View(db.IncidentTypes.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminArea/IncidentTypes/Create

        [HttpPost]
        public ActionResult Create(IncidentType IncidentType)
        {
            if (ModelState.IsValid)
            {
                if (db.IncidentTypes.Any(p => p.IncidentType1 == IncidentType.IncidentType1) == false)
                {
                    //if IncidentType does not exist yet
                    IncidentType.IncidentTypeID = Guid.NewGuid();
                    db.IncidentTypes.InsertOnSubmit(IncidentType);
                    db.SubmitChanges();
                }

                return RedirectToAction("Index");
            }

            return View(IncidentType);
        }


        public ActionResult Edit(Guid id)
        {
            IncidentType IncidentType = db.IncidentTypes.Single(r => r.IncidentTypeID == id);
            if (IncidentType == null)
            {
                return HttpNotFound();
            }
            return View(IncidentType);
        }

        [HttpPost]
        public ActionResult Edit(IncidentType IncidentType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.IncidentTypes.Attach(IncidentType);
                    db.Refresh(RefreshMode.KeepCurrentValues, IncidentType);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch { }

            return View(IncidentType);
        }


    }
}
