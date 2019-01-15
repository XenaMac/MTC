using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FSP.Domain.Model;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
     [CustomAuthorization(Roles = "Admin")]
    public class UsersController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Users/

        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /AdminArea/Users/Details/5

        public ActionResult Details(Guid id)
        {
            User user = db.Users.Single(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /AdminArea/Users/Create

        //public ActionResult Create()
        //{
        //    ViewBag.Roles = db.Roles.OrderBy(p => p.RoleName).ToList(); 
        //    return View();
        //}

        ////
        //// POST: /AdminArea/Users/Create

        //[HttpPost]
        //public ActionResult Create(User user, Guid RoleID)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Guid userId = Guid.NewGuid();
        //        MembershipCreateStatus createStatus;
        //        ((FSPMembershipProvider)Membership.Provider).CreateFSPUser(userId, user.Email, "OCT@2012", RoleID, true, out createStatus, user.FirstName, user.LastName, user.Address, user.City, user.State, user.Zip, user.PhoneNumber);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", Helpers.Util.ErrorCodeToString(createStatus));
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Roles = db.Roles.OrderBy(p => p.RoleName).ToList();     
        //    return View(user);
        //}

        //
        // GET: /AdminArea/Users/Edit/5

        public ActionResult Edit(Guid id)
        {
            User user = db.Users.Single(u => u.UserID == id);

            ViewBag.Roles = db.Roles.OrderBy(p => p.RoleName).ToList();
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList(); 
         
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /AdminArea/Users/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
            
                db.Users.Attach(user);
                db.Refresh(RefreshMode.KeepCurrentValues, user);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = db.Roles.OrderBy(p => p.RoleName).ToList();
            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList(); 
            return View(user);
        }

        //
        // GET: /AdminArea/Users/Delete/5

        public ActionResult Delete(Guid id)
        {
            User user = db.Users.Single(u => u.UserID == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Approve(Guid id)
        {
            User user = db.Users.Single(u => u.UserID == id);
            user.IsApproved = true;
            db.SubmitChanges();


            return RedirectToAction("Index");
        }



        //
        // POST: /AdminArea/Users/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Single(u => u.UserID == id);
            db.Users.DeleteOnSubmit(user);
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