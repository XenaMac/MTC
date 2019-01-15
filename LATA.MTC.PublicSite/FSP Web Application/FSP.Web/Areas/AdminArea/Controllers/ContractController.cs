using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Areas.AdminArea.ViewModels;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class ContractController : Controller
    {
        FSPDataContext db = new FSPDataContext();

        //
        // GET: /AdminArea/Freeways/

        public ActionResult Index()
        {
            var dbContracts = db.Contracts.OrderBy(p => p.Contractor.ContractCompanyName).ToList();
            var dbBeats = db.vBeats.OrderBy(p => p.BeatNumber).ToList();
            var dbContractsBeats = db.ContractsBeats.ToList();

            var model = new ContractIndexViewModel();
            model.Contracts = new List<ContractIndex>();

            foreach (var dbContract in dbContracts)
            {
                var selectedBeats = (from q in dbContractsBeats
                                     join b in dbBeats on q.BeatID equals b.BeatID
                                     where q.ContractID == dbContract.ContractID
                                     orderby b.BeatNumber
                                     select b.BeatNumber).ToList();

                model.Contracts.Add(new ContractIndex
                {
                    Contract = dbContract,
                    SelectedBeats = selectedBeats.OrderBy(p => p).ToList()
                });
            }

            model.Contracts = model.Contracts.OrderBy(p=>p.Contract.Contractor.ContractCompanyName).ThenBy(p => p.SelectedBeats.FirstOrDefault()).ToList();

            return View(model);
        }

        //
        // GET: /AdminArea/Freeways/Details/5

        public ActionResult Details(Guid id)
        {
            Contract Contract = db.Contracts.Single(r => r.ContractID == id);
            if (Contract == null)
            {
                return HttpNotFound();
            }
            return View(Contract);
        }

        //
        // GET: /AdminArea/Freeways/Create

        public ActionResult Create()
        {
            var model = new ContractCreateViewModel();
            Contract contract = new Contract();
            contract.EndDate = DateTime.Today;
            contract.StartDate = DateTime.Today;
            model.Contract = contract;
            model.SelectedBeats = new List<Guid>();

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            return View(model);
        }

        //
        // POST: /AdminArea/Freeways/Create

        [HttpPost]
        public ActionResult Create(ContractCreateViewModel ContractCreateViewModel)
        {
            if (ModelState.IsValid)
            {

                //if Contract does not exist yet       
                Guid contractId = Guid.NewGuid();
                ContractCreateViewModel.Contract.ContractID = contractId;
                ContractCreateViewModel.Contract.BeatID = db.vBeats.FirstOrDefault().BeatID; //obsolete
                db.Contracts.InsertOnSubmit(ContractCreateViewModel.Contract);
                db.SubmitChanges();

                //add beats
                foreach (var beat in ContractCreateViewModel.SelectedBeats)
                {
                    ContractsBeat contractsBeat = new ContractsBeat();
                    contractsBeat.BeatID = beat;
                    contractsBeat.ContractID = contractId;
                    db.ContractsBeats.InsertOnSubmit(contractsBeat);
                }
                db.SubmitChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            return View(ContractCreateViewModel);
        }

        //
        // GET: /AdminArea/Freeways/Edit/5

        public ActionResult Edit(Guid id)
        {
            Contract Contract = db.Contracts.Single(r => r.ContractID == id);

            if (Contract == null)
            {
                return HttpNotFound();
            }

            var model = new ContractCreateViewModel();
            model.Contract = Contract;
            model.SelectedBeats = new List<Guid>();


            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();

            return View(model);
        }

        //
        // POST: /AdminArea/Freeways/Edit/5

        [HttpPost]
        public ActionResult Edit(ContractCreateViewModel ContractCreateViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Contracts.Attach(ContractCreateViewModel.Contract);
                    db.Refresh(RefreshMode.KeepCurrentValues, ContractCreateViewModel.Contract);
                    db.SubmitChanges();

                    //remove existing ones
                    var existingBeats = db.ContractsBeats.Where(p => p.ContractID == ContractCreateViewModel.Contract.ContractID);

                    foreach (var existingBeat in existingBeats)
                    {
                        db.ContractsBeats.DeleteOnSubmit(existingBeat);
                    }

                    db.SubmitChanges();

                    //re-add beats
                    foreach (var beat in ContractCreateViewModel.SelectedBeats)
                    {
                        ContractsBeat contractsBeat = new ContractsBeat();
                        contractsBeat.BeatID = beat;
                        contractsBeat.ContractID = ContractCreateViewModel.Contract.ContractID;
                        db.ContractsBeats.InsertOnSubmit(contractsBeat);
                    }
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            ViewBag.Contractors = db.Contractors.OrderBy(p => p.ContractCompanyName).ToList();
            return View(ContractCreateViewModel.Contract);
        }

        //
        // GET: /AdminArea/Freeways/Delete/5

        public ActionResult Delete(Guid id)
        {
            Contract Contract = db.Contracts.Single(r => r.ContractID == id);
            if (Contract == null)
            {
                return HttpNotFound();
            }
            return View(Contract);
        }

        //
        // POST: /AdminArea/Freeways/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Contract Contract = db.Contracts.Single(r => r.ContractID == id);
            db.Contracts.DeleteOnSubmit(Contract);
            db.SubmitChanges();

            var existingBeats = db.ContractsBeats.Where(p => p.ContractID == id);

            foreach (var existingBeat in existingBeats)
            {
                db.ContractsBeats.DeleteOnSubmit(existingBeat);
            }

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
