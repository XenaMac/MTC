using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;
using MTC.FSP.Web.ViewModels;

namespace MTC.FSP.Web.Controllers.FleetManagement
{
    public class MerchandiseController : MtcBaseController
    {
        #region Views

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,DataConsultant")]
        public ActionResult Order()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,DataConsultant")]
        public ActionResult OrderHistory()
        {
            return View();
        }

        [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,TowContractor,DataConsultant")]
        public ActionResult OrderDetails(int? id)
        {
            ViewBag.OrderId = id.ToString();
            return View();
        }

        #endregion

        #region CRUD

        public ActionResult GetOrders()
        {
            IEnumerable<Contractor> contractors;
            using (MTCDBEntities dc = new MTCDBEntities())
            {
                contractors = dc.Contractors.ToList();
            }


            using (MTCDbContext db = new MTCDbContext())
            {
                var dateLimit = DateTime.Today.AddMonths(-3);

                var data = from o in db.MerchandiseOrders
                    where o.CreatedOn >= dateLimit || o.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderSubmitted
                    select o;

                var returnList = data.OrderByDescending(p => p.Id).ToList().Select(q => new MerchandiseOrderHistoryViewModel
                {
                    Id = q.Id,
                    FriendlyId = q.Id.ToString().PadLeft(8, "0"[0]),
                    ContactName = q.ContactName,
                    ContactNumber = q.ContactNumber,
                    Contractor = contractors.FirstOrDefault(p => p.ContractorID == q.ContractorId).ContractCompanyName,
                    PickupDate = q.PickupDate,
                    PickupTime = q.PickupTime,
                    CreatedBy = q.CreatedBy,
                    CreatedOn = q.CreatedOn,
                    MerchandiseOrderStatus = q.MerchandiseOrderStatus,
                    OrderDetails = db.MerchandiseOrderDetails.Where(p => p.MerchandiseOrderId == q.Id).ToList().Select(d => new MerchandiseOrderProductsViewModel
                    {
                        Id = d.Id,
                        Product = d.MerchandiseProduct.DisplayName,
                        UnitCost = d.UnitCost,
                        Quantity = d.Quantity
                    }).ToList()
                }).ToList();

                //if user is a contractor, only show them THEIR orders
                if (!string.IsNullOrEmpty(UsersContractorCompanyName))
                {
                    returnList = returnList.Where(p => p.Contractor == UsersContractorCompanyName).ToList();
                }

                return Json(returnList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetOrderDetails(int? id)
        {
            IEnumerable<Contractor> contractors;
            using (MTCDBEntities dc = new MTCDBEntities())
            {
                contractors = dc.Contractors.ToList();
            }

            using (MTCDbContext db = new MTCDbContext())
            {
                MerchandiseOrderDetailsViewModel model = new MerchandiseOrderDetailsViewModel();
                model.Order = db.MerchandiseOrders.Find(id);
                model.CanCancel = (model.Order.CreatedBy == HttpContext.User.Identity.Name ? true : false) && model.Order.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderSubmitted;
                model.CanDecline = User.IsInRole("Admin");
                model.CanFulFill = User.IsInRole("Admin");
                model.Contractor = contractors.FirstOrDefault(p => p.ContractorID == model.Order.ContractorId).ContractCompanyName;
                model.Products = db.MerchandiseOrderDetails.Where(p => p.MerchandiseOrderId == id).ToList().Select(d => new MerchandiseOrderProductsViewModel
                {
                    Id = d.Id,
                    Product = d.MerchandiseProduct.DisplayName,
                    UnitCost = d.UnitCost,
                    Quantity = d.Quantity
                }).ToList();
                model.Audits = GetAuditSummary(model.Order.Id);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            return Json(UndoOrder(orderId, MerchandiseOrderStatus.OrderCancelled), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeclineOrder(int orderId)
        {
            return Json(UndoOrder(orderId, MerchandiseOrderStatus.OrderDeclined), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FulFillOrder(int orderId)
        {
            bool retValue = true;

            using (MTCDbContext db = new MTCDbContext())
            {
                MerchandiseOrder order = db.MerchandiseOrders.Find(orderId);
                if (order != null)
                {
                    AuditMerchandiseOrder(order);
                    order.MerchandiseOrderStatus = MerchandiseOrderStatus.OrderFilled;
                    order.ModifiedBy = HttpContext.User.Identity.Name;
                    order.ModifiedOn = DateTime.Now;
                    db.SaveChanges();
                    SendEmails(order, true);
                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult SubmitOrder(MerchandiseOrderViewModel model)
        {
            int retValue = 0;
            //Update Inventory Level
            //Send Email
            //Persist data

            if (model != null)
            {
                if (model.Products != null && model.Products.Any())
                {
                    using (MTCDbContext db = new MTCDbContext())
                    {
                        MerchandiseOrder order = new MerchandiseOrder();
                        order.ContactName = model.ContactName;
                        order.ContactNumber = model.ContactNumber;
                        order.ContractorId = model.ContractorId;
                        order.CreatedBy = HttpContext.User.Identity.Name;
                        order.CreatedOn = DateTime.Now;
                        order.PickupDate = DateTime.Now.AddDays(12);
                        order.PickupTime = model.PickupTime;
                        order.MerchandiseOrderStatus = MerchandiseOrderStatus.OrderSubmitted;

                        if (model.PaymentType == "check")
                            order.PayByCheck = true;
                        else
                            order.DeductFromInvoice = true;

                        db.MerchandiseOrders.Add(order);
                        db.SaveChanges();


                        foreach (var product in model.Products)
                        {
                            MerchandiseOrderDetail orderDetail = new MerchandiseOrderDetail();
                            orderDetail.MerchandiseOrderId = order.Id;
                            orderDetail.MerchandiseProductId = product.Id;
                            orderDetail.Quantity = product.Quantity;
                            orderDetail.UnitCost = product.UnitCost;

                            db.MerchandiseOrderDetails.Add(orderDetail);

                            //update inventory levels
                            MerchandiseProduct dbproduct = db.MerchandiseProducts.Find(product.Id);
                            if (dbproduct != null)
                                dbproduct.UnitsInStock -= product.Quantity;

                            db.SaveChanges();

                            CheckInventoryLevels(product.Id);
                        }

                        retValue = order.Id;
                        SendEmails(order, false);
                    }
                }
            }

            return Json(retValue.ToString().PadLeft(8, "0"[0]), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrder(MerchandiseOrder model)
        {
            bool retValue = true;

            if (model != null)
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    MerchandiseOrder order = db.MerchandiseOrders.Find(model.Id);
                    if (order != null)
                    {
                        AuditMerchandiseOrder(order);

                        order.ContactName = model.ContactName;
                        order.ContactNumber = model.ContactNumber;
                        order.PickupDate = model.PickupDate;
                        order.PickupTime = model.PickupTime;
                        order.ReceivedBy = model.ReceivedBy;
                        order.ReceivedOn = model.ReceivedOn;
                        order.Comment = model.Comment;

                        order.ModifiedBy = HttpContext.User.Identity.Name;
                        order.ModifiedOn = DateTime.Now;

                        db.SaveChanges();
                        SendEmails(order, true);
                    }
                }
            }

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region private methods

        private void CheckInventoryLevels(int productId)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var merchandiseInventoryNotificationThreshold = Utilities.GetApplicationSettingValue("MerchandiseInventoryNotificationThreshold");
                if (!string.IsNullOrEmpty(merchandiseInventoryNotificationThreshold))
                {
                    var product = db.MerchandiseProducts.Find(productId);
                    if (product != null)
                    {
                        if (product.UnitsInStock < Convert.ToInt32(merchandiseInventoryNotificationThreshold))
                        {
                            //Send Email Notification
                            SendLowInventoryNotificationEmail(product);
                        }
                    }
                }
            }
        }

        private void AuditMerchandiseOrder(MerchandiseOrder order)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                db.MerchandiseOrderAudits.Add(new MerchandiseOrderAudit(order, DateTime.Now, HttpContext.User.Identity.Name));
                db.SaveChanges();
            }
        }

        private void SendEmails(MerchandiseOrder order, bool isUpdate)
        {
            var contractCompanyName = string.Empty;
            using (MTCDBEntities db = new MTCDBEntities())
            {
                contractCompanyName = db.Contractors.FirstOrDefault(p => p.ContractorID == order.ContractorId).ContractCompanyName;
            }

            var emailBody = EmailManager.BuildMerchandiseOrderEmailBody(order, isUpdate);

            var recipientSubject = contractCompanyName + " " + (isUpdate ? " Order Change" : " Order Confirmation");
            var mtcSubject = contractCompanyName + " " + (isUpdate ? " Order Change Confirmation" : " New Merchandise Order");

            MTCEmailRecipient toRecipient = new MTCEmailRecipient {Email = order.CreatedBy, Name = ""};
            List<MTCEmailRecipient> toRecipients = new List<MTCEmailRecipient> {toRecipient};
            EmailManager.SendEmail(toRecipients, recipientSubject, emailBody, null);

            //One email to main MTC recipient
            using (MTCDbContext db = new MTCDbContext())
            {
                MTCEmailRecipient mtcRecipient = new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("MerchandiseOrderFormRecipient"),
                    Name = Utilities.GetApplicationSettingValue("MerchandiseOrderFormRecipientName")
                };
                List<MTCEmailRecipient> mtcRecipients = new List<MTCEmailRecipient> {mtcRecipient};
                EmailManager.SendEmail(mtcRecipients, mtcSubject, emailBody, null);
            }
        }

        private void SendLowInventoryNotificationEmail(MerchandiseProduct product)
        {
            using (MTCDbContext db = new MTCDbContext())
            {
                var emailBody = EmailManager.BuildMerchandiseProductLowInventoryEmailBody(product);
                var subject = product.DisplayName + " Low Inventory Notification";

                MTCEmailRecipient mtcRecipient = new MTCEmailRecipient
                {
                    Email = Utilities.GetApplicationSettingValue("MerchandiseOrderFormRecipient"),
                    Name = Utilities.GetApplicationSettingValue("MerchandiseOrderFormRecipientName")
                };

                List<MTCEmailRecipient> mtcRecipients = new List<MTCEmailRecipient> {mtcRecipient};

                EmailManager.SendEmail(mtcRecipients, subject, emailBody, null);
            }
        }

        private List<AuditSummaryViewModel> GetAuditSummary(int orderId)
        {
            List<AuditSummaryViewModel> changeSummary = new List<AuditSummaryViewModel>();

            using (MTCDbContext db = new MTCDbContext())
            {
                var changes = db.MerchandiseOrderAudits.Where(p => p.MerchandiseOrderId == orderId).ToList();

                //add current order
                var order = db.MerchandiseOrders.FirstOrDefault(p => p.Id == orderId);
                changes.Add(new MerchandiseOrderAudit(order, Convert.ToDateTime(order.ModifiedOn), order.ModifiedBy));


                if (changes.Count() > 1)
                {
                    for (int i = changes.Count() - 1; i > 0; i--)
                    {
                        try
                        {
                            MerchandiseOrderAudit newChange = changes[i];
                            MerchandiseOrderAudit prevChange = changes[i - 1];

                            StringBuilder changeContent = new StringBuilder();
                            DateTime dateOfChange = DateTime.MinValue;
                            string changedBy = string.Empty;

                            Type firstType = newChange.GetType();

                            var previousValue = string.Empty;
                            var newValue = string.Empty;
                            var propertyName = string.Empty;

                            foreach (PropertyInfo property in firstType.GetProperties())
                            {
                                try
                                {
                                    propertyName = property.Name;
                                    var friendlyName = property.GetCustomAttributes(typeof (DisplayNameAttribute), false).Cast<DisplayNameAttribute>().FirstOrDefault();
                                    var displayName = string.Empty;
                                    if (friendlyName != null)
                                        displayName = friendlyName.DisplayName;
                                    else
                                        displayName = property.Name;

                                    #region Lookups

                                    if (property.GetValue(newChange) != null)
                                    {
                                        newValue = property.GetValue(newChange).ToString();
                                    }
                                    else
                                        newValue = "Blank";

                                    if (property.GetValue(prevChange) != null)
                                    {
                                        previousValue = property.GetValue(prevChange).ToString();
                                    }
                                    else
                                        previousValue = "Blank";

                                    #endregion

                                    if (propertyName != "Id" &&
                                        propertyName != "CreatedOn" &&
                                        propertyName != "ModifiedOn" &&
                                        propertyName != "CreatedBy" &&
                                        propertyName != "MerchandiseOrder" &&
                                        propertyName != "ModifiedBy")
                                    {
                                        Debug.WriteLine("Comparing property: " + displayName + ": NEW " + newValue + ": OLD " + previousValue);

                                        if (!newValue.Equals(previousValue))
                                        {
                                            changeContent.Append("<b>" + displayName + "</b> changed from <span class='text-danger'>" + previousValue + "</span> to <span class='text-success'>" +
                                                                 newValue + "</span><br/>");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }
                            }

                            if (changeContent.Length > 0)
                            {
                                dateOfChange = prevChange.ModifiedOn != null ? Convert.ToDateTime(prevChange.ModifiedOn) : Convert.ToDateTime(prevChange.CreatedOn);
                                changedBy = !string.IsNullOrEmpty(newChange.ModifiedBy) ? newChange.ModifiedBy : newChange.CreatedBy;

                                changeSummary.Add(new AuditSummaryViewModel
                                {
                                    ChangeContent = changeContent.ToString(),
                                    ChangeDate = dateOfChange,
                                    ChangedBy = changedBy
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }


                return changeSummary;
            }
        }

        private bool UndoOrder(int orderId, MerchandiseOrderStatus status)
        {
            bool retValue = true;

            using (MTCDbContext db = new MTCDbContext())
            {
                MerchandiseOrder order = db.MerchandiseOrders.Find(orderId);
                if (order != null)
                {
                    AuditMerchandiseOrder(order);
                    order.MerchandiseOrderStatus = status;
                    order.ModifiedBy = HttpContext.User.Identity.Name;
                    order.ModifiedOn = DateTime.Now;

                    //re-stock inventory
                    var orderProducts = db.MerchandiseOrderDetails.Where(p => p.MerchandiseOrderId == orderId).ToList();
                    foreach (var orderProduct in orderProducts)
                    {
                        MerchandiseProduct dbproduct = db.MerchandiseProducts.Find(orderProduct.MerchandiseProductId);
                        if (dbproduct != null)
                            dbproduct.UnitsInStock += orderProduct.Quantity;
                    }

                    db.SaveChanges();

                    SendEmails(order, true);
                }
            }

            return retValue;
        }

        #endregion
    }
}