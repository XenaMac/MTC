using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class ContractsController : Controller
    {
        private MTCDBEntities db = new MTCDBEntities();

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult Manage(Guid? Id)
        {
            ViewBag.ContractId = Id;
            return View();
        }

        public ActionResult SaveContract(Contract model)
        {
            var isNew = model.ContractID == Guid.Empty ? true : false;
            Contract contract = null;
            if (isNew)
            {
                contract = new Contract();
                contract.ContractID = Guid.NewGuid();
            }
            else
            {
                contract = db.Contracts.Find(model.ContractID);
            }

            contract.AgreementNumber = model.AgreementNumber;
            contract.StartDate = model.StartDate;
            contract.EndDate = model.EndDate;
            contract.MaxObligation = model.MaxObligation;
            contract.ContractorID = model.ContractorID;
            contract.BeatId = model.BeatId;

            if (isNew)
                db.Contracts.Add(contract);

            db.SaveChanges();

            return Json(contract.ContractID, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            db.ContractsBeats.RemoveRange(db.ContractsBeats.Where(p => p.ContractID == id));
            db.Contracts.Remove(contract);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        #region Json

        public ActionResult GetContracts()
        {
            var data = db.Contracts.ToList().Select(c => new
            {
                ContractId = c.ContractID,
                AgreementNumber = c.AgreementNumber,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MaxObligation = c.MaxObligation,
                ContractorId = c.ContractorID,
                ContractorCompanyName = c.Contractor.ContractCompanyName,
                Beat = (from b in db.BeatDatas
                        where b.ID == c.BeatId
                        select b.BeatName).FirstOrDefault()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContract(Guid id)
        {
            var data = db.Contracts.Where(p => p.ContractID == id).ToList().Select(c => new
            {
                ContractID = c.ContractID,
                AgreementNumber = c.AgreementNumber,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                MaxObligation = c.MaxObligation,
                ContractorId = c.ContractorID,
                BeatId = c.BeatId
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
    }
}
