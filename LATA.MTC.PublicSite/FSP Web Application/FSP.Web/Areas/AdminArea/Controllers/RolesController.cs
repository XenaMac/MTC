using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Filters;


namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class RolesController : Controller
    {
        //FSPDataContext db = new FSPDataContext();

        ////
        //// GET: /AdminArea/Roles/

        //public ActionResult Index()
        //{
        //    return View(db.Roles.ToList());
        //}

        ////
        //// GET: /AdminArea/Roles/Details/5

        //public ActionResult Details(Guid id)
        //{
        //    Role role = db.Roles.Single(r => r.RoleID == id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        ////
        //// GET: /AdminArea/Roles/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /AdminArea/Roles/Create

        //[HttpPost]
        //public ActionResult Create(Role role)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (db.Roles.Any(p => p.RoleName == role.RoleName) == false)
        //        {
        //            //if role does not exist yet
        //            role.RoleID = Guid.NewGuid();
        //            db.Roles.InsertOnSubmit(role);
        //            db.SubmitChanges();
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    return View(role);
        //}

        ////
        //// GET: /AdminArea/Roles/Edit/5

        //public ActionResult Edit(Guid id)
        //{
        //    Role role = db.Roles.Single(r => r.RoleID == id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        ////
        //// POST: /AdminArea/Roles/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Role role)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (db.Roles.Any(p => p.RoleName == role.RoleName) == false)
        //        {
        //            db.Roles.Attach(role);
        //            db.Refresh(RefreshMode.KeepCurrentValues, role);                 
        //            db.SubmitChanges();
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    return View(role);
        //}

        ////
        //// GET: /AdminArea/Roles/Delete/5

        //public ActionResult Delete(Guid id)
        //{
        //    Role role = db.Roles.Single(r => r.RoleID == id);
        //    if (role == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(role);
        //}

        ////
        //// POST: /AdminArea/Roles/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    Role role = db.Roles.Single(r => r.RoleID == id);
        //    db.Roles.DeleteOnSubmit(role);
        //    db.SubmitChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}