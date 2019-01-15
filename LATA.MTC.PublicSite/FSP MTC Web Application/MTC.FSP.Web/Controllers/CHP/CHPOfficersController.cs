using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.CHP
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class CHPOfficersController : Controller
    {
        private readonly MTCDBEntities db = new MTCDBEntities();

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var cHPOfficer = await db.CHPOfficers.FindAsync(id);
            if (cHPOfficer == null)
                return HttpNotFound();
            return View(cHPOfficer);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var cHPOfficer = await db.CHPOfficers.FindAsync(id);
            db.CHPOfficerBeats.RemoveRange(db.CHPOfficerBeats.Where(p => p.CHPOfficerId == id));
            db.CHPInspections.RemoveRange(db.CHPInspections.Where(p => p.CHPOfficerId == id));
            db.CHPOfficers.Remove(cHPOfficer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage(int? Id)
        {
            ViewBag.CHPOfficerId = Id;
            return View();
        }

        public ActionResult Save(CHPOfficer chpOfficer, List<BeatViewModel> Beats)
        {
            var isNew = chpOfficer.Id == 0 ? true : false;
            CHPOfficer officer = null;
            if (isNew)
                officer = new CHPOfficer();
            else
                officer = db.CHPOfficers.Find(chpOfficer.Id);

            officer.BadgeID = chpOfficer.BadgeID;
            officer.OfficerLastName = chpOfficer.OfficerLastName;
            officer.OfficerFirstName = chpOfficer.OfficerFirstName;
            officer.Email = chpOfficer.Email;
            officer.Phone = chpOfficer.Phone;

            if (isNew)
                db.CHPOfficers.Add(officer);

            db.SaveChanges();

            if (db.CHPOfficerBeats.Any(p => p.CHPOfficerId == officer.Id))
                db.CHPOfficerBeats.RemoveRange(db.CHPOfficerBeats.Where(p => p.CHPOfficerId == officer.Id).ToList());

            if (Beats != null)
                foreach (var beat in Beats)
                    db.CHPOfficerBeats.Add(new CHPOfficerBeat
                    {
                        CHPOfficerId = officer.Id,
                        BeatId = beat.BeatId
                    });
            db.SaveChanges();

            return Json(officer.Id, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }


        #region Json

        public ActionResult GetCHPOfficers()
        {
            var data = db.CHPOfficers.Select(p => new
            {
                p.Id,
                BadgeId = p.BadgeID,
                p.OfficerFirstName,
                p.OfficerLastName,
                p.Email,
                p.Phone,
                Beats = (from q in db.CHPOfficerBeats
                    join b in db.BeatDatas on q.BeatId equals b.ID
                    where q.CHPOfficerId == p.Id
                    orderby b.BeatName
                    select new BeatViewModel
                    {
                        BeatId = b.ID,
                        BeatNumber = b.BeatName
                    }).ToList()
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCHPOfficer(int id)
        {
            var data = db.CHPOfficers.Where(p => p.Id == id).ToList().Select(c => new
            {
                c.Id,
                BadgeId = c.BadgeID,
                c.OfficerFirstName,
                c.OfficerLastName,
                c.Email,
                c.Phone,
                Beats = (from q in db.CHPOfficerBeats
                    join b in db.BeatDatas on q.BeatId equals b.ID
                    where q.CHPOfficerId == c.Id
                    orderby b.BeatName
                    select new BeatViewModel
                    {
                        BeatId = b.ID,
                        BeatNumber = b.BeatName
                    }).ToList()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAvailableBeats()
        {
            var unavailableBeatIds = db.CHPOfficerBeats.Select(p => p.BeatId);
            var availableBeats = db.BeatDatas.Where(p => !unavailableBeatIds.Contains(p.ID)).Select(p => new
            {
                Id = p.BeatID,
                Text = p.BeatName
            });
            return Json(availableBeats.OrderBy(p => p.Text).ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}