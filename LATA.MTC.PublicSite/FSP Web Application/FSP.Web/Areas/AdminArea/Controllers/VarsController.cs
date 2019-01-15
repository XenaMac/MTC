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
    public class VarsController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        public ActionResult Index()
        {
            return View(db.Vars.Where(p => p.VarName.Contains("Leeway")).OrderBy(p => p.VarName).ToList());
        }
      
        public ActionResult Edit(Guid id)
        {
            Var var = db.Vars.Single(r => r.VarID == id);
            if (var == null)
            {
                return HttpNotFound();
            }
            return View(var);
        }

        [HttpPost]
        public ActionResult Edit(Var var)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vars.Attach(var);
                    db.Refresh(RefreshMode.KeepCurrentValues, var);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View(var);
        }
        

    }
}
