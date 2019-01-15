using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class MerchandiseOrderViewModel
    {
        public Guid ContractorId { get; set; }
        public String ContactNumber { get; set; }
        public String ContactName { get; set; }
        public DateTime PickupDate { get; set; }
        public String PickupTime { get; set; }
        public String PaymentType { get; set; }
        public List<MerchandiseOrderProductsViewModel> Products { get; set; }

    }


    public class MerchandiseOrderHistoryViewModel
    {
        public int Id { get; set; }
        public String FriendlyId { get; set; }
        public String Contractor { get; set; }
        public String ContactName { get; set; }
        public String ContactNumber { get; set; }
        public DateTime PickupDate { get; set; }
        public String PickupTime { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public MerchandiseOrderStatus MerchandiseOrderStatus { get; set; }
        public List<MerchandiseOrderProductsViewModel> OrderDetails { get; set; }

    }

    public class MerchandiseOrderDetailsViewModel
    {
        public MerchandiseOrder Order { get; set; }
        public List<MerchandiseOrderProductsViewModel> Products { get; set; }
        public List<AuditSummaryViewModel> Audits { get; set; }
        public String Contractor { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDecline { get; set; }
        public bool CanFulFill { get; set; }
    }


    public class MerchandiseOrderProductsViewModel
    {
        public int Id { get; set; }
        public String Product { get; set; }
        public Decimal UnitCost { get; set; }
        public int Quantity { get; set; }

    }



}