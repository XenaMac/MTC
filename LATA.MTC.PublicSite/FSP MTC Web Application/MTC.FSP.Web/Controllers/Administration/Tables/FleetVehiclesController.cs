using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
   [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class FleetVehiclesController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();


        public ActionResult Index()
        {
            var fleetVehicles = db.FleetVehicles.Include(f => f.Contractor);
            return View(fleetVehicles.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FleetVehicle fleetVehicle = db.FleetVehicles.Find(id);
            if (fleetVehicle == null)
            {
                return HttpNotFound();
            }
            return View(fleetVehicle);
        }

        #region Json
        public ActionResult Get(Guid? id)
        {
            var data = (from f in db.FleetVehicles
                        where f.FleetVehicleID == id
                        select new
                        {
                            ProgramStartDate = f.ProgramStartDate,
                            FleetNumber = f.FleetNumber,
                            VehicleType = f.VehicleType,
                            VehicleYear = f.VehicleYear,
                            VehicleMake = f.VehicleMake,
                            VehicleModel = f.VehicleModel,
                            VIN = f.VIN,
                            LicensePlate = f.LicensePlate,
                            RegistrationExpireDate = f.RegistrationExpireDate,
                            InsuranceExpireDate = f.InsuranceExpireDate,
                            LastCHPInspection = f.LastCHPInspection,
                            Comments = f.Comments,
                            ProgramEndDate = f.ProgramEndDate,
                            FAW = f.FAW,
                            RAWR = f.RAWR,
                            RAW = f.RAW,
                            GVW = f.GVW,
                            GVWR = f.GVWR,
                            Wheelbase = f.Wheelbase,
                            Overhang = f.Overhang,
                            MAXTW = f.MAXTW,
                            MAXTWCALCDATE = f.MAXTWCALCDATE,
                            FuelType = f.FuelType,
                            VehicleNumber = f.VehicleNumber,
                            IPAddress = f.IPAddress,
                            TAIP = f.TAIP,
                            AgreementNumber = f.AgreementNumber,
                            IsBackup = f.IsBackup

                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save([Bind(Include = "FleetVehicleID,ContractorID,ProgramStartDate,FleetNumber,VehicleType,VehicleYear,VehicleMake,VehicleModel,VIN,LicensePlate,RegistrationExpireDate,InsuranceExpireDate,LastCHPInspection,Comments,ProgramEndDate,FAW,RAW,RAWR,GVW,GVWR,Wheelbase,Overhang,MAXTW,MAXTWCALCDATE,FuelType,VehicleNumber,IPAddress,TAIP,AgreementNumber,IsBackup")] FleetVehicle fleetVehicle)
        {
            if (fleetVehicle.FleetVehicleID != null && fleetVehicle.FleetVehicleID != Guid.Empty)
            {
                db.Entry(fleetVehicle).State = EntityState.Modified;
            }
            else
            {
                fleetVehicle.FleetVehicleID = Guid.NewGuid();
                db.FleetVehicles.Add(fleetVehicle);
            }

            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveFleetVehicle(Guid fleetVehicleId)
        {
            if (db.FleetVehicles.Any(p => p.FleetVehicleID == fleetVehicleId))
            {
                FleetVehicle fV = db.FleetVehicles.FirstOrDefault(p => p.FleetVehicleID == fleetVehicleId);
                if (fV != null)
                {
                    db.FleetVehicles.Remove(fV);
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
        public ActionResult Create([Bind(Include = "FleetVehicleID,ContractorID,ProgramStartDate,FleetNumber,VehicleType,VehicleYear,VehicleMake,VehicleModel,VIN,LicensePlate,RegistrationExpireDate,InsuranceExpireDate,LastCHPInspection,Comments,ProgramEndDate,FAW,RAW,RAWR,GVW,GVWR,Wheelbase,Overhang,MAXTW,MAXTWCALCDATE,FuelType,VehicleNumber,IPAddress,TAIP,AgreementNumber")] FleetVehicle fleetVehicle)
        {
            if (ModelState.IsValid)
            {
                fleetVehicle.FleetVehicleID = Guid.NewGuid();
                db.FleetVehicles.Add(fleetVehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", fleetVehicle.ContractorID);
            return View(fleetVehicle);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FleetVehicle fleetVehicle = db.FleetVehicles.Find(id);
            if (fleetVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", fleetVehicle.ContractorID);
            return View(fleetVehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FleetVehicleID,ContractorID,ProgramStartDate,FleetNumber,VehicleType,VehicleYear,VehicleMake,VehicleModel,VIN,LicensePlate,RegistrationExpireDate,InsuranceExpireDate,LastCHPInspection,Comments,ProgramEndDate,FAW,RAW,RAWR,GVW,GVWR,Wheelbase,Overhang,MAXTW,MAXTWCALCDATE,FuelType,VehicleNumber,IPAddress,TAIP,AgreementNumber")] FleetVehicle fleetVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fleetVehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Address", fleetVehicle.ContractorID);
            return View(fleetVehicle);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FleetVehicle fleetVehicle = db.FleetVehicles.Find(id);
            if (fleetVehicle == null)
            {
                return HttpNotFound();
            }
            return View(fleetVehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FleetVehicle fleetVehicle = db.FleetVehicles.Find(id);
            db.FleetVehicles.Remove(fleetVehicle);
            db.SaveChanges();
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
