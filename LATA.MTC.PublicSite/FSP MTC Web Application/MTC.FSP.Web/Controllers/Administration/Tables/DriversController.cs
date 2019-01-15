using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.Common;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class DriversController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        
        public async Task<ActionResult> Index()
        {
            var drivers = db.Drivers.Include(d => d.Contractor);
            return View(await drivers.ToListAsync());
        }

        
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        #region Json
        public ActionResult Get(Guid? id)
        {
            var data = (from f in db.Drivers
                        where f.DriverID == id
                        select new
                        {
                            DriverId = f.DriverID,
                            ContractorId = f.ContractorID,
                            BeatID = f.BeatID,
                            DateAdded = f.DateAdded,

                            LastName = f.LastName,
                            FirstName = f.FirstName,
                            FSPIDNumber = f.FSPIDNumber,
                            ProgramStartDate = f.ProgramStartDate,
                            TrainingCompletionDate = f.TrainingCompletionDate,
                            DOB = f.DOB,
                            LicenseExpirationDate = f.LicenseExpirationDate,
                            DL64ExpirationDate = f.DL64ExpirationDate,
                            MedicalCardExpirationDate = f.MedicalCardExpirationDate,
                            LastPullNoticeDate = f.LastPullNoticeDate,
                            UDF = f.UDF,
                            Comments = f.Comments,
                            ContractorEndDate = f.ContractorEndDate,
                            ProgramEndDate = f.ProgramEndDate,
                            ContractorStartDate = f.ContractorStartDate,
                            Password = f.Password,
                            DL64Number = f.DL64Number,
                            DriversLicenseNumber = f.DriversLicenseNumber,
                            AddedtoC3Database = f.AddedtoC3Database

                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save([Bind(Include = "DriverID,ContractorID,LastName,FirstName,FSPIDNumber,ProgramStartDate,TrainingCompletionDate,DOB,LicenseExpirationDate,DL64ExpirationDate,MedicalCardExpirationDate,LastPullNoticeDate,DateAdded,UDF,Comments,ContractorEndDate,ProgramEndDate,ContractorStartDate,BeatID,Password,DL64Number,DriversLicenseNumber,AddedtoC3Database")] Driver driver)
        {
            if (driver.DriverID != null && driver.DriverID != Guid.Empty)
            {
                db.Entry(driver).State = EntityState.Modified;
            }
            else
            {
                driver.DriverID = Guid.NewGuid();
                driver.DateAdded = DateTime.Today;

                db.Drivers.Add(driver);
            }

            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(Guid driverId)
        {
            if (db.Drivers.Any(p => p.DriverID == driverId))
            {
                Driver d = db.Drivers.FirstOrDefault(p => p.DriverID == driverId);
                if (d != null)
                {
                    db.Drivers.Remove(d);
                    db.SaveChanges();
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        public ActionResult Create()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DriverID,ContractorID,LastName,FirstName,FSPIDNumber,ProgramStartDate,TrainingCompletionDate,DOB,LicenseExpirationDate,DL64ExpirationDate,MedicalCardExpirationDate,LastPullNoticeDate,DateAdded,UDF,Comments,ContractorEndDate,ProgramEndDate,ContractorStartDate,BeatID,Password,DL64Number,DriversLicenseNumber,AddedtoC3Database")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                driver.DriverID = Guid.NewGuid();
                db.Drivers.Add(driver);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", driver.ContractorID);
            return View(driver);
        }

     
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", driver.ContractorID);
            return View(driver);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DriverID,ContractorID,LastName,FirstName,FSPIDNumber,ProgramStartDate,TrainingCompletionDate,DOB,LicenseExpirationDate,DL64ExpirationDate,MedicalCardExpirationDate,LastPullNoticeDate,DateAdded,UDF,Comments,ContractorEndDate,ProgramEndDate,ContractorStartDate,BeatID,Password,DL64Number,DriversLicenseNumber,AddedtoC3Database")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driver).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", driver.ContractorID);
            return View(driver);
        }
        
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
