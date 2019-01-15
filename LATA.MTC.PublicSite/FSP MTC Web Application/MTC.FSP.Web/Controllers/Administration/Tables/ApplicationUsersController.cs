using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationUsersController()
        {

        }

        public ApplicationUsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index()
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                List<UserListViewModel> model = new List<UserListViewModel>();
                foreach (var user in identity.Users.ToList())
                {
                    model.Add(new UserListViewModel(user));                    
                }

                using (MTCDBEntities db = new MTCDBEntities())
                {
                    ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
                }

                return View(model);
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                var user = await this.UserManager.FindByIdAsync(id);
                var model = new UserListViewModel(user);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(String UserId, String SelectedRoleId)
        {
            if (!String.IsNullOrEmpty(UserId) && !String.IsNullOrEmpty(SelectedRoleId))
            {
                using (ApplicationDbContext identity = new ApplicationDbContext())
                {                   
                    //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                    //userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                    //{
                    //    AllowOnlyAlphanumericUserNames = false,
                    //    RequireUniqueEmail = true
                    //};

                    var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(identity));

                    var currentUser = await this.UserManager.FindByIdAsync(UserId);
                    if (currentUser != null)
                    {
                        IdentityUserRole userRole = new IdentityUserRole();
                        userRole.RoleId = SelectedRoleId;
                        userRole.UserId = currentUser.Id;
                        //make sure user is only assigned to ONE role
                        currentUser.Roles.Clear();
                        currentUser.Roles.Add(userRole);
                        
                        //userManager.Update(currentUser);

                        IdentityResult result = await this.UserManager.UpdateAsync(currentUser);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrors(result);
                        }

                    }
                }
               
            }

            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                var user = await userManager.FindByIdAsync(UserId);
                var model = new UserListViewModel(user);
                return View(model);
            }
        }

        public async Task<ActionResult> Delete(String id)
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                var user = await this.UserManager.FindByIdAsync(id);
                var model = new UserListViewModel(user);
                return View(model);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(String id)
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(identity));
                var user = await this.UserManager.FindByIdAsync(id);
                this.UserManager.Delete(user);

                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
