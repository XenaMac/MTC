using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
  [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class ContractorsController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        #region Views

        public async Task<ActionResult> Index()
        {
            return View(await db.Contractors.OrderBy(p => p.ContractCompanyName).Include(f => f.ContractorType).ToListAsync());
        }

        public async Task<ActionResult> Backups()
        {
            return View();
        }

        public ActionResult Manage(Guid? Id)
        {
            ViewBag.ContractorId = Id;
            return View();
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = await db.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Contractor contractor = await db.Contractors.FindAsync(id);
            db.Contractors.Remove(contractor);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Json

        public ActionResult GetContractor(Guid id)
        {
            var data = db.Contractors.Where(p => p.ContractorID == id).ToList().Select(c => new ContractorDetailsViewModel
            {
                ContractorId = c.ContractorID,
                ContractorTypeId = c.ContractorTypeId,
                Address = c.Address,
                City = c.City,
                Comments = c.Comments,
                ContactFirstName = c.ContactFirstName,
                ContactLastName = c.ContactLastName,
                ContractCompanyName = c.ContractCompanyName,
                Email = c.Email,
                MCPExpiration = c.MCPExpiration,
                MCPNumber = c.MCPNumber,
                OfficeTelephone = c.OfficeTelephone,
                State = c.State,
                Zip = c.Zip,
                FleetVehicles = (from q in db.FleetVehicles
                                 where q.ContractorID == c.ContractorID
                                 orderby q.VehicleNumber
                                 select new VehicleViewModel
                                 {
                                     FleetVehicleId = q.FleetVehicleID,
                                     VehicleNumber = q.VehicleNumber,
                                     VehicleMake = q.VehicleMake,
                                     VehicleModel = q.VehicleModel
                                 }).ToList(),
                Drivers = (from d in db.Drivers
                           where d.ContractorID == c.ContractorID
                           orderby d.LastName
                           select new DriverViewModel
                           {
                               DriverId = d.DriverID,
                               FirstName = d.FirstName,
                               LastName = d.LastName,
                               DriverLicenseNumber = d.DriversLicenseNumber,
                               FSPIDNumber = d.FSPIDNumber
                           }).ToList(),
                //CHPInspections = (from d in db.CHPInspections
                //                  where d.ContractorID == c.ContractorID
                //                  orderby d.BadgeID
                //                  select new CHPInspectionViewModel
                //                  {
                //                      InspectionId = d.InspectionID,
                //                      InspectionDate = d.InspectionDate,
                //                      InspectionNotes = d.InspectionNotes
                //                  }).ToList(),
                ContractorManagers = (from d in db.ContractorManagers
                                      where d.ContractorID == c.ContractorID
                                      orderby d.LastName
                                      select new ContractorManagerViewModel
                                      {
                                          ContractorManagerId = d.ContractorManagerID,
                                          FirstName = d.FirstName,
                                          LastName = d.LastName,
                                          PhoneNumber = d.PhoneNumber
                                      }).ToList()
            });

            //Driver Interactions
            //Insurance Carriers

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveContractor(ContractorDetailsViewModel model)
        {
            var isNew = model.ContractorId == Guid.Empty ? true : false;
            Contractor contractor = null;
            if (isNew)
            {
                contractor = new Contractor();
                contractor.ContractorID = Guid.NewGuid();
            }
            else
            {
                contractor = db.Contractors.Find(model.ContractorId);
            }

            contractor.Address = model.Address;
            contractor.City = model.City;
            contractor.Comments = model.Comments;
            contractor.ContactFirstName = model.ContactFirstName;
            contractor.ContactLastName = model.ContactLastName;
            contractor.ContractCompanyName = model.ContractCompanyName;
            contractor.Email = model.Email;
            contractor.OfficeTelephone = model.OfficeTelephone;
            contractor.MCPNumber = model.MCPNumber;
            contractor.MCPExpiration = model.MCPExpiration;
            contractor.State = model.State;
            contractor.Zip = model.Zip;
            contractor.ContractorTypeId = model.ContractorTypeId;


            if (isNew)
                db.Contractors.Add(contractor);

            db.SaveChanges();

            return Json(contractor.ContractorID, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetContractorTypes()
        {
            var data = db.ContractorTypes.ToList().Select(p => new
            {
                ContractorTypeId = p.ContractorTypeId,
                ContractorTypeName = p.ContractorTypeName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //public ActionResult Create()
        //{
        //    ViewBag.ContractorTypeId = new SelectList(db.ContractorTypes, "ContractorTypeId", "ContractorTypeName");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ContractorTypeId,ContactName,Address,OfficeTelephone,MCPNumber,MCPExpiration,Comments,ContractCompanyName,City,State,Zip,Email")] Contractor contractor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        contractor.ContractorID = Guid.NewGuid();
        //        db.Contractors.Add(contractor);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ContractorTypeId = new SelectList(db.ContractorTypes, "ContractorTypeId", "ContractorTypeName");
        //    return View(contractor);
        //}

        //public async Task<ActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Contractor contractor = await db.Contractors.FindAsync(id);
        //    ViewBag.ContractorTypeId = new SelectList(db.ContractorTypes, "ContractorTypeId", "ContractorTypeName", contractor.ContractorTypeId);
        //    if (contractor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(contractor);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ContractorID,ContactName,ContractorTypeId,Address,OfficeTelephone,MCPNumber,MCPExpiration,Comments,ContractCompanyName,City,State,Zip,Email")] Contractor contractor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(contractor).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ContractorTypeId = new SelectList(db.ContractorTypes, "ContractorTypeId", "ContractorTypeName", contractor.ContractorTypeId);
        //    return View(contractor);
        //}

    }
}
