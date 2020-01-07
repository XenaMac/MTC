using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    public class BeatsFreewaysController : Controller
    {
        private readonly MTCDBEntities _db = new MTCDBEntities();

        public ActionResult Create()
        {
            ViewBag.BeatID = new SelectList(_db.BeatDatas, "ID", "BeatName").OrderBy(d => d.Text);
            ViewBag.FreewayID = new SelectList(_db.Freeways, "FreewayID", "FreewayID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BeatFreewayID,BeatID,FreewayID,Active,StartDate,EndDate")] BeatsFreeway beatsFreeway)
        {
            if (ModelState.IsValid)
            {
                _db.BeatsFreeways.Add(beatsFreeway);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BeatID = new SelectList(_db.BeatDatas, "ID", "BeatName", beatsFreeway.BeatID);
            ViewBag.FreewayID = new SelectList(_db.Freeways, "FreewayID", "FreewayName", beatsFreeway.FreewayID);
            return View(beatsFreeway);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var beatsFreeway = await _db.BeatsFreeways.FindAsync(id);
            if (beatsFreeway == null)
                return HttpNotFound();
            return View(beatsFreeway);
        }
        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var beatsFreeway = await _db.BeatsFreeways.FindAsync(id);
            _db.BeatsFreeways.Remove(beatsFreeway);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int id)
        {
            var beatsFreeway = await _db.BeatsFreeways.FindAsync(id);
            if (beatsFreeway == null)
                return HttpNotFound();
            return View(beatsFreeway);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var beatsFreeway = await _db.BeatsFreeways.FindAsync(id);
            if (beatsFreeway == null)
                return HttpNotFound();
            ViewBag.BeatID = new SelectList(_db.BeatDatas, "ID", "BeatName", beatsFreeway.BeatID);
            ViewBag.FreewayID = new SelectList(_db.Freeways, "FreewayID", "FreewayName", beatsFreeway.FreewayID);
            return View(beatsFreeway);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BeatFreewayID,BeatID,FreewayID,Active,StartDate,EndDate")] BeatsFreeway beatsFreeway)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(beatsFreeway).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BeatID = new SelectList(_db.BeatDatas, "BeatID", "BeatDescription", beatsFreeway.BeatID);
            ViewBag.FreewayID = new SelectList(_db.Freeways, "FreewayID", "FreewayName", beatsFreeway.FreewayID);
            return View(beatsFreeway);
        }

        public async Task<ActionResult> Index()
        {
            var beatsFreeways = _db.BeatsFreeways.Include(b => b.Freeway).OrderBy(c => c.BeatID);
            return View(await beatsFreeways.ToListAsync());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}