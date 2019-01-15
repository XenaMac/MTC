using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.CHP
{
    public class OvertimeActivitiesController : Controller
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();

        public ActionResult Create()
        {
            ViewBag.Shift = new SelectList(_db.MTCSchedules, "ScheduleName", "ScheduleName");
            ViewBag.Callsign = new SelectList(_db.MTCBeatsCallSigns, "Callsign", "Callsign");
            var blocks = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var blocksApproved = new SelectList(blocks);
            ViewBag.BlocksApproved = blocksApproved;
            ViewBag.Beat = new SelectList(_db.BeatDatas, "BeatName", "BeatName").OrderBy(d => d.Text);
            ViewBag.Contractor = new SelectList(_db.Contractors, "ContractCompanyName", "ContractCompanyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OvertimeActivity overtimeActivity, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                overtimeActivity.ID = Guid.NewGuid();
                _db.OvertimeActivities.Add(overtimeActivity);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Shift = new SelectList(_db.MTCSchedules, "ScheduleId", "ScheduleName");
            ;
            ViewBag.Callsign = new SelectList(_db.MTCBeatsCallSigns, "Callsign", "Callsign");
            var blocks = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var blocksApproved = new SelectList(blocks);
            ViewBag.BlocksApproved = blocksApproved;
            ViewBag.Beat = new SelectList(_db.BeatDatas, "BeatName", "BeatName").OrderBy(d => d.Text);
            ViewBag.Contractor = new SelectList(_db.Contractors, "ContractorId", "ContractCompanyName");
            return View(overtimeActivity);
        }

        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var overtimeActivity = await _db.OvertimeActivities.FindAsync(id);
            if (overtimeActivity == null)
                return HttpNotFound();
            return View(overtimeActivity);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var overtimeActivity = await _db.OvertimeActivities.FindAsync(id);
            if (overtimeActivity != null) _db.OvertimeActivities.Remove(overtimeActivity);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var overtimeActivity = await _db.OvertimeActivities.FindAsync(id);
            if (overtimeActivity == null)
                return HttpNotFound();
            return View(overtimeActivity);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var overtimeActivity = await _db.OvertimeActivities.FindAsync(id);
            ViewBag.Shifts = new SelectList(_db.MTCSchedules, "ScheduleName", "ScheduleName", overtimeActivity.Shift);
            ViewBag.Callsign = new SelectList(_db.MTCBeatsCallSigns, "Callsign", "Callsign", overtimeActivity.CallSign);
            var blocks = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var blocksApproved = new SelectList(blocks);
            ViewBag.BlocksApproved = blocksApproved;
            ViewBag.Beat = new SelectList(_db.BeatDatas, "BeatName", "BeatName", overtimeActivity.Beat).OrderBy(d => d.Text);
            ViewBag.Contractor = new SelectList(_db.Contractors, "ContractCompanyName", "ContractCompanyName", overtimeActivity.Contractor);

            return View(overtimeActivity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,timeStamp,Shift,CallSign,OverTimeCode,BlocksApproved,Beat,Contractor,Confirmed")] OvertimeActivity overtimeActivity, FormCollection Form)
        {
            if (ModelState.IsValid)
            {
                overtimeActivity.Shift = Request.Form["Shifts"];
                _db.Entry(overtimeActivity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Shift = new SelectList(_db.MTCSchedules, "ScheduleName", "ScheduleName");
            ViewBag.Callsign = new SelectList(_db.MTCBeatsCallSigns, "Callsign", "Callsign");
            var blocks = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var blocksApproved = new SelectList(blocks);
            ViewBag.BlocksApproved = blocksApproved;
            ViewBag.Beat = new SelectList(_db.BeatDatas, "BeatName", "BeatName").OrderBy(d => d.Text);
            ViewBag.Contractor = new SelectList(_db.Contractors, "ContractCompanyName", "ContractCompanyName");
            return View(overtimeActivity);
        }

        public async Task<ActionResult> Index()
        {
            var twoMonthsAgo = DateTime.Now.AddMonths(-2);
            return View(await _db.OvertimeActivities.Where(o => o.timeStamp >= twoMonthsAgo).ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}