using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.TowTruckServiceRef;


namespace MTC.FSP.Web.Controllers.Operations
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer")]
    public class DriverMessagesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendDriverMessage(string message, int requireAck, List<MtcTruck> trucks)
        {
            using (var service = new TowTruckServiceClient())
            {
                foreach (var truck in trucks)
                {
                    var svcMessage = new TowTruckServiceRef.TruckMessage
                    {
                        MessageID = Guid.NewGuid(),
                        MessageText = message,
                        TruckIP = truck.ipAddress,
                        UserEmail = HttpContext.User.Identity.Name,
                        SentTime = DateTime.Now,
                        MessageType = requireAck
                    };

                    service.SendMessage(svcMessage);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTodaysSummary()
        {
            using (var service = new TowTruckServiceClient())
            {
                var messages = service.getAllMessages();
                var data = new List<TowTruckServiceRef.TruckMessage>();
                foreach (var message in messages)
                {
                    var add = false;
                    if (message.SentTime >= DateTime.Today)
                        if (message.MessageText.Length >= 50)
                        {
                            message.MessageText = message.MessageText.Substring(0, 50) + "...";
                        }

                    add = true;

                    if (add)
                        data.Add(message);
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}