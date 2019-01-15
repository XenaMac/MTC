using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class ApplicationRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationRoles
        public async Task<ActionResult> Index()
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                var roles = identity.ApplicationRoles.ToList();
                return View(roles.OrderBy(p => p.IsDeletable).ThenBy(p => p.Name));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext identity = new ApplicationDbContext())
                {
                    var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(identity));
                    applicationRole.IsDeletable = true;
                    RoleManager.Create(applicationRole);
                    return RedirectToAction("Index");
                }

            }

            return View();
        }

        public async Task<ActionResult> Edit(String id)
        {
            using (ApplicationDbContext identity = new ApplicationDbContext())
            {
                var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(identity));
                var role = await RoleManager.FindByIdAsync(id);
                return View(role);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext identity = new ApplicationDbContext())
                {
                    var RoleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(identity));
                    var role = await RoleManager.FindByIdAsync(applicationRole.Id);
                    if (role != null)
                    {
                        RoleManager.Update(role);
                    }

                    return RedirectToAction("Index");
                }

            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
