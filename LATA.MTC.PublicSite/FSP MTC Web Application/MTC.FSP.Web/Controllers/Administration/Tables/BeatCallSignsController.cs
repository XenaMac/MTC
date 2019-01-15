using System;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.Administration.Tables
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner")]
    public class BeatCallSignsController : Controller
    {
        public ActionResult GetAll()
        {
            using (var db = new MTCDBEntities())
            {
                var data = from b in db.BeatDatas
                    select new BeatCallSignsViewModel
                    {
                        BeatId = b.ID,
                        BeatNumber = b.BeatID,
                        OnCallAreas = "",
                        Freq = "",
                        CHPArea = "",
                        CallSigns = db.MTCBeatsCallSigns.Where(p => p.BeatID.ToString() == b.BeatID).Select(c => new CallSignViewModel
                        {
                            CallSign = c.CallSign
                        })
                    };
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOne(Guid? beatId)
        {
            using (var db = new MTCDBEntities())
            {
                var data = from b in db.BeatDatas
                    where b.ID == beatId
                    select new BeatCallSignsViewModel
                    {
                        BeatId = b.ID,
                        BeatNumber = b.BeatName,
                        //OnCallAreas = b.OnCallAreas,
                        //Freq = b.Freq,
                        //CHPArea = b.CHPArea,
                        CallSigns = db.MTCBeatsCallSigns.Where(p => p.BeatID.ToString() == b.BeatID).Select(c => new CallSignViewModel
                        {
                            CallSign = c.CallSign
                        })
                    };
                return Json(data.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage(Guid? BeatId)
        {
            ViewBag.BeatId = BeatId;
            return View();
        }

        public ActionResult Save(BeatCallSignsViewModel model)
        {
            using (var db = new MTCDBEntities())
            {
                var beat = db.BeatDatas.FirstOrDefault(b => b.ID == model.BeatId);
                if (beat == null) return Json(model.BeatId, JsonRequestBehavior.AllowGet);

                //beat.OnCallAreas = model.OnCallAreas;
                //beat.Freq = model.Freq;
                //beat.CHPArea = model.CHPArea;
                db.SaveChanges();


                if (db.MTCBeatsCallSigns.Any(p => p.BeatID.ToString() == model.BeatNumber) && model.CallSigns != null)
                    db.MTCBeatsCallSigns.RemoveRange(db.MTCBeatsCallSigns.Where(p => p.BeatID.ToString() == model.BeatNumber).ToList());

                if (model.CallSigns != null)
                    foreach (var callSign in model.CallSigns)
                        db.MTCBeatsCallSigns.Add(new MTCBeatsCallSign
                        {
                            BeatID = Convert.ToInt32(model.BeatNumber),
                            CallSign = callSign.CallSign
                        });
                db.SaveChanges();

                return Json(model.BeatId, JsonRequestBehavior.AllowGet);
            }
        }
    }
}