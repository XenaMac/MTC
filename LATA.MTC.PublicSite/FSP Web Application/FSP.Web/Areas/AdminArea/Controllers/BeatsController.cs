using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FSP.Domain.Model;
using FSP.Web.Filters;

namespace FSP.Web.Areas.AdminArea.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class BeatsController : Controller
    {
        FSPDataContext dc = new FSPDataContext();

        public ActionResult Index()
        {
            return View(dc.vBeats.ToList());
        }

        public ActionResult GetAll()
        {
            var query = from q in dc.vBeats
                        orderby q.BeatNumber
                        select new
                        {
                            Id = q.BeatID.ToString(),
                            Text = q.BeatNumber
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatsByContractId(Guid id)
        {
            var query = from q in dc.vBeats
                        join c in dc.ContractsBeats on q.BeatID equals c.BeatID
                        where c.ContractID.Equals(id)
                        orderby q.BeatNumber
                        select new
                        {
                            Id = q.BeatID.ToString(),
                            Text = q.BeatNumber
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

    }
}
