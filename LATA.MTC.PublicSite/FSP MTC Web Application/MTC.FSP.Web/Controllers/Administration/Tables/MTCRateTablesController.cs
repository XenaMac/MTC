using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    public class MTCRateTablesController : Controller
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();

        public ActionResult Create()
        {
            ViewBag.Beats = new SelectList(_db.BeatDatas.OrderBy(b => b.BeatName), "ID", "BeatName");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RateTableID,BeatID,CurrentMonthRate,p100,p150,p200,p250,p300,p350,p400,p450,p500,p550,p600,p650,p700,p750,p800")] MTCRateTable mTCRateTable)
        {
            if (ModelState.IsValid)
            {
                mTCRateTable.RateTableID = Guid.NewGuid();
                _db.MTCRateTables.Add(mTCRateTable);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Beat = new SelectList(_db.BeatDatas.OrderBy(b => b.BeatName), "ID", "BeatName");
            return View(mTCRateTable);
        }
       
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var mTCRateTable = _db.MTCRateTables.Find(id);
            if (mTCRateTable == null)
                return HttpNotFound();

            var beatnum = (from b in _db.BeatDatas
                where b.ID == mTCRateTable.BeatID
                select b.BeatName).FirstOrDefault();

            mTCRateTable.BeatNumber = beatnum;
            return View(mTCRateTable);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var mTCRateTable = _db.MTCRateTables.Find(id);
            _db.MTCRateTables.Remove(mTCRateTable);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var mTCRateTable = _db.MTCRateTables.Find(id);
            if (mTCRateTable == null)
                return HttpNotFound();
            return View(mTCRateTable);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var mTCRateTable = _db.MTCRateTables.Find(id);
            if (mTCRateTable == null)
                return HttpNotFound();

            var record = _db.MTCRateTables.FirstOrDefault(t => t.RateTableID == id);
            if (record != null)
            {
                var beatnum = (from b in _db.BeatDatas
                    where b.ID == record.BeatID
                    select b.BeatName).FirstOrDefault();
                mTCRateTable.BeatNumber = beatnum;
            }

            return View(mTCRateTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RateTableID,BeatID,CurrentMonthRate,p100,p150,p200,p250,p300,p350,p400,p450,p500,p550,p600,p650,p700,p750,p800")] MTCRateTable mTCRateTable)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(mTCRateTable).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RateTableID = new SelectList(_db.MTCRateTables, "RateTableID", "RateTableID", mTCRateTable.RateTableID);
            ViewBag.RateTableID = new SelectList(_db.MTCRateTables, "RateTableID", "RateTableID", mTCRateTable.RateTableID);
            return View(mTCRateTable);
        }
       
        public ActionResult Index()
        {
            var mTCRateTables = _db.MTCRateTables.ToList();
            var beats = _db.BeatDatas.ToList();

            if (mTCRateTables.Count != beats.Count)
                ViewBag.OffCount = "true";
            else
                ViewBag.OffCount = "false";

            foreach (var rt in mTCRateTables)
            {
                var beatnum = (from b in beats
                    where b.ID == rt.BeatID
                    select b.BeatName).FirstOrDefault();

                rt.BeatNumber = beatnum;
            }

            return View(mTCRateTables.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}