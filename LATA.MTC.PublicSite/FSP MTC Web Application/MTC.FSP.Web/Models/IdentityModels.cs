using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MTC.FSP.Web.Models
{
   
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [StringLength(50)]
        [DisplayName("Nick Name")]
        public string NickName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string Mobile { get; set; }

        [DisplayName("Contractor")]
        public Guid? ContractorId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole
    {
        public bool IsDeletable { get; set; }
    }

    public class UserReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public UserReportType UserReportType { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }
    }

    public enum UserReportType
    {
        Bug,
        Improvement,
        NewFeature
    }

    public class MTCApplicationSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [StringLength(75)]
        public string Value { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class FAQAnswer
    {
        [Key]
        public int Id { get; set; }

        public int FAQId { get; set; }
        public virtual FAQ FAQ { get; set; }

        [Required]
        [StringLength(500)]
        public string Answer { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #region Merchandise

    public abstract class MerchandiseOrderBase
    {
        [Key]
        public int Id { get; set; }

        public Guid ContractorId { get; set; }

        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Pick-up Date")]
        public DateTime PickupDate { get; set; }

        [DisplayName("Pick-up Time")]
        public string PickupTime { get; set; }

        [DisplayName("Pay by Check")]
        public bool PayByCheck { get; set; }

        [DisplayName("Deduct from Invoice")]
        public bool DeductFromInvoice { get; set; }

        [DisplayName("Received By")]
        public string ReceivedBy { get; set; }

        [DisplayName("Received on")]
        public DateTime? ReceivedOn { get; set; }

        [DisplayName("Status")]
        public MerchandiseOrderStatus MerchandiseOrderStatus { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }


        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class MerchandiseOrder : MerchandiseOrderBase
    {
    }

    public class MerchandiseOrderAudit : MerchandiseOrderBase
    {
        public MerchandiseOrderAudit()
        {
        }

        public MerchandiseOrderAudit(MerchandiseOrder merchandiseOrder, DateTime auditDate, string auditUser)
        {
            MerchandiseOrderId = merchandiseOrder.Id;
            ContractorId = merchandiseOrder.ContractorId;
            ContactNumber = merchandiseOrder.ContactNumber;
            ContactName = merchandiseOrder.ContactName;
            PickupDate = merchandiseOrder.PickupDate;
            PickupTime = merchandiseOrder.PickupTime;
            PayByCheck = merchandiseOrder.PayByCheck;
            DeductFromInvoice = merchandiseOrder.DeductFromInvoice;
            ReceivedBy = merchandiseOrder.ReceivedBy;
            ReceivedOn = merchandiseOrder.ReceivedOn;
            MerchandiseOrderStatus = merchandiseOrder.MerchandiseOrderStatus;
            Comment = merchandiseOrder.Comment;

            CreatedOn = auditDate;
            CreatedBy = auditUser;
        }

        public int MerchandiseOrderId { get; set; }
        public virtual MerchandiseOrder MerchandiseOrder { get; set; }
    }

    public enum MerchandiseOrderStatus
    {
        OrderSubmitted,
        OrderFilled,
        OrderCancelled,
        OrderDeclined
    }

    public class MerchandiseOrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MerchandiseOrderId { get; set; }

        public virtual MerchandiseOrder MerchandiseOrder { get; set; }

        [Required]
        public int MerchandiseProductId { get; set; }

        public virtual MerchandiseProduct MerchandiseProduct { get; set; }

        [Required]
        public decimal UnitCost { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

    public class MerchandiseProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Unit Cost")]
        public decimal UnitCost { get; set; }

        [DisplayName("Units In Stock")]
        public int UnitsInStock { get; set; }

        [DisplayName("Size")]
        public int? MerchandiseProductSizeId { get; set; }

        public virtual MerchandiseProductSize MerchandiseProductSize { get; set; }

        public int? OrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class MerchandiseProductSize
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Size { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #endregion

    #region Backups

    public class BackupBeat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid BeatId { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class BackupProvider
    {
        [Key]
        public int Id { get; set; }

        public int BackupBeatId { get; set; }
        public virtual BackupBeat BackupBeat { get; set; }

        [Required]
        public Guid ContractorId { get; set; }

        [Required]
        public Guid FleetVehicleId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class BackupAssignment
    {
        [Key]
        public int Id { get; set; }

        public Guid BeatId { get; set; }

        [ForeignKey("PrimaryBackupBeat")]
        public int PrimaryBackupBeatId { get; set; }

        [ForeignKey("PrimaryBackupBeatId")]
        [Column(Order = 0)]
        public virtual BackupBeat PrimaryBackupBeat { get; set; }

        [ForeignKey("SecondaryBackupBeat")]
        public int SecondaryBackupBeatId { get; set; }

        [ForeignKey("SecondaryBackupBeatId")]
        [Column(Order = 1)]
        public virtual BackupBeat SecondaryBackupBeat { get; set; }

        [ForeignKey("TertiaryBackupBeat")]
        public int TertiaryBackupBeatId { get; set; }

        [ForeignKey("TertiaryBackupBeatId")]
        [Column(Order = 2)]
        public virtual BackupBeat TertiaryBackupBeat { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class BackupReason
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Reason { get; set; }

        [Required]
        [StringLength(1)]
        public string ReasonCode { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class BackupRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RequestNumber { get; set; } //system generated 1201 [month/squence]

        [DisplayName("Request Priority")]
        public bool RequestIsUrgent { get; set; }

        [Required]
        public Guid ContractorId { get; set; }

        [Required]
        public Guid BeatId { get; set; }

        public int BackupReasonId { get; set; }
        public virtual BackupReason BackupReason { get; set; }
        public string Comments { get; set; }
        public Guid SelectedBackupContractorId { get; set; }
        public BackupAssignmentLevel SelectedBackupContractorAssignmentLevel { get; set; }

        //This field is set programmatically either through service or when another contractors responds.
        public Guid? CurrentBackupContractorId { get; set; }

        public BackupAssignmentLevel CurrentBackupContractorAssignmentLevel { get; set; }
        public string SelectionReason { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string CancelledBy { get; set; }
        public int? BackupCancellationReasonId { get; set; }
        public virtual BackupCancellationReason BackupCancellationReason { get; set; }

        public string CancellationComment { get; set; }
        public bool PrimaryBackupResponseTimeExpiredOrDeclined { get; set; }
        public DateTime? PrimaryBackupResponseTimeExpiredOrDeclinedOn { get; set; }
        public bool SecondaryBackupResponseTimeExpiredOrDeclined { get; set; }

        public DateTime? SecondaryBackupResponseTimeExpiredOrDeclinedOn { get; set; }
        public bool TertiaryBackupResponseTimeExpiredOrDeclined { get; set; }

        public DateTime? TertiaryBackupResponseTimeExpiredOrDeclinedOn { get; set; }
        public bool AllBackupsNotified { get; set; }

        //this field is updated when a assignment level's response time has expired. So the new count down will start from this new value
        public DateTime LastExpiredOn { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class BackupRequestShiftAndDate
    {
        [Key]
        public int Id { get; set; }

        public int BackupRequestId { get; set; }
        public virtual BackupRequest BackupRequest { get; set; }

        public DateTime BackupDate { get; set; }

        public bool AMRequested { get; set; }
        public bool AMSatisfied { get; set; }

        public bool MIDRequested { get; set; }
        public bool MIDSatisfied { get; set; }

        public bool PMRequested { get; set; }
        public bool PMSatisfied { get; set; }
    }

    public enum BackupAssignmentLevel
    {
        Primary,
        Secondary,
        Tertiary,
        AllBackupOperators
    }

    public class BackupResponse
    {
        [Key]
        public int Id { get; set; }

        public int BackupRequestId { get; set; }
        public virtual BackupRequest BackupRequest { get; set; }

        [Required]
        public Guid ContractorId { get; set; }

        public BackupResponseStatus BackupResponseStatus { get; set; }

        public int? BackupDeclinationReasonId { get; set; }

        public BackupAssignmentLevel BackupAssignmentLevel { get; set; }

        public string Comments { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public enum BackupResponseStatus
    {
        Accepted,
        Qualified,
        Declined
    }

    public class BackupCancellationReason
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Reason { get; set; }

        [Required]
        [StringLength(1)]
        public string ReasonCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class BackupDeclinationReason
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Reason { get; set; }

        [Required]
        [StringLength(1)]
        public string ReasonCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #endregion

    #region Assets

    public class AssetStatusLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Item { get; set; }

        [StringLength(150)]
        [DisplayName("OEM Serial #")]
        public string OEMSerialNumber { get; set; }

        [StringLength(150)]
        public string Location { get; set; }

        [DisplayName("Truck #")]
        public Guid VehicleId { get; set; }

        [StringLength(150)]
        [DisplayName("IP Address")]
        public string IPAddress { get; set; }

        [DisplayName("Status")]
        public int AssetStatusId { get; set; }

        public virtual AssetStatus AssetStatus { get; set; }

        [StringLength(150)]
        [DisplayName("Repair Cycle Time (days)")]
        public string RepairCycleTimeInDays { get; set; }

        [StringLength(150)]
        [DisplayName("LATA RMA #")]
        public string LATARMANumber { get; set; }

        [StringLength(150)]
        [DisplayName("OEM RMA #")]
        public string OEMRMANumber { get; set; }

        [DisplayName("OEM RMA # Issue Date")]
        public DateTime? OEMRMANumberIssueDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class AssetWarranty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Item { get; set; }

        [StringLength(150)]
        [DisplayName("LATA ID #")]
        public string LATAIDNumber { get; set; }

        [StringLength(150)]
        [DisplayName("OEM Serial #")]
        public string OEMSerialNumber { get; set; }

        [DisplayName("Warranty End Date")]
        public DateTime WarrantyEndDate { get; set; }

        [StringLength(150)]
        [DisplayName("Operating System")]
        public string OperatingSystem { get; set; }

        [StringLength(150)]
        [DisplayName("OEM Software")]
        public string OEMSoftware { get; set; }

        [StringLength(150)]
        [DisplayName("LATA Software")]
        public string LATASoftware { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class AssetStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; }

        public string Color { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #endregion

    #region Trouble Tickets

    public abstract class TroubleTicketBase
    {
        [Key]
        public int Id { get; set; }

        public TroubleTicketType TroubleTicketType { get; set; }

        public TroubleTicketStatus TroubleTicketStatus { get; set; }

        [DisplayName("Origination Contractor")]
        public Guid? AssociatedTowContractorId { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }


        [DisplayName("Associated In-Vehicle Equipment General Contractor")]
        public Guid? AssociatedInVehicleContractorId { get; set; }

        [DisplayName("Associated In-Vehicle Equipment General Contractor")]
        public Guid? AssociatedInVehicleLATATraxContractorId { get; set; }

        [DisplayName("Truck Number")]
        public Guid VehicleId { get; set; }

        [DisplayName("Problem Started On")]
        public DateTime? ProblemStartedOn { get; set; }

        [StringLength(200)]
        [DisplayName("Shifts Missed")]
        public string ShiftsMissed { get; set; }

        [DisplayName("Date Truck Out of Service")]
        public DateTime? DateTruckOutOfService { get; set; }

        [DisplayName("Date Truck In Service")]
        public DateTime? DateTruckBackInService { get; set; }

        [DisplayName("First Date Truck Missed Service")]
        public DateTime? FirstShiftTruckMissed { get; set; }

        [DisplayName("Last Date Truck Missed Service")]
        public DateTime? LastShiftTruckMissed { get; set; }

        [DisplayName("Has Back-up Request been submitted")]
        public bool BackupRequestSubmitted { get; set; }

        [DisplayName("ReliaGate OEM Serial #")]
        public string ReliaGateOEMSerialNumber { get; set; }

        [StringLength(500)]
        [DisplayName("MTC Notes")]
        public string MTCNotes { get; set; }

        [StringLength(500)]
        public string ContractorNotes { get; set; }

        [StringLength(500)]
        public string InVehicleContractorNotes { get; set; }

        [StringLength(500)]
        public string LATANotes { get; set; }

        //MAINTENANCE

        public DateTime? TroubleShootingDate { get; set; }

        public DateTime? FixedDate { get; set; }

        public DateTime? RemovedAssetDate { get; set; }

        public DateTime? ShippedAssetDate { get; set; }

        public DateTime? ReceivedAssetDate { get; set; }

        public DateTime? InstalledAssetDate { get; set; }

        [StringLength(50)]
        public string LATARMANumber { get; set; }

        [StringLength(500)]
        public string MaintenanceNotes { get; set; }

        //REPLACEMENT ASSET

        public bool ReplacmentIsFixed { get; set; }

        public DateTime? ReplacmentDate { get; set; }

        public string ReplacementOEMSerialNumber { get; set; }

        public string ReplacementIPAddress { get; set; }

        public string ReplacementWiFiSSID { get; set; }


        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class TroubleTicket : TroubleTicketBase
    {
    }

    public class TroubleTicketAudit : TroubleTicketBase
    {
        public TroubleTicketAudit()
        {
        }

        public TroubleTicketAudit(TroubleTicket troubleTicket, DateTime auditDate, string auditUser)
        {
            TroubleTicketId = troubleTicket.Id;


            AssociatedInVehicleContractorId = troubleTicket.AssociatedInVehicleContractorId;
            AssociatedInVehicleLATATraxContractorId = troubleTicket.AssociatedInVehicleLATATraxContractorId;
            AssociatedTowContractorId = troubleTicket.AssociatedTowContractorId;
            BackupRequestSubmitted = troubleTicket.BackupRequestSubmitted;
            ContractorNotes = troubleTicket.ContractorNotes;
            DateTruckBackInService = troubleTicket.DateTruckBackInService;
            DateTruckOutOfService = troubleTicket.DateTruckOutOfService;
            FirstShiftTruckMissed = troubleTicket.FirstShiftTruckMissed;
            FixedDate = troubleTicket.FixedDate;
            InstalledAssetDate = troubleTicket.InstalledAssetDate;
            InVehicleContractorNotes = troubleTicket.InVehicleContractorNotes;
            LastShiftTruckMissed = troubleTicket.LastShiftTruckMissed;


            LATANotes = troubleTicket.LATANotes;
            LATARMANumber = troubleTicket.LATARMANumber;
            MaintenanceNotes = troubleTicket.MaintenanceNotes;
            ModifiedBy = troubleTicket.ModifiedBy;
            ModifiedOn = troubleTicket.ModifiedOn;
            MTCNotes = troubleTicket.MTCNotes;
            ProblemStartedOn = troubleTicket.ProblemStartedOn;
            ReceivedAssetDate = troubleTicket.ReceivedAssetDate;
            ReliaGateOEMSerialNumber = troubleTicket.ReliaGateOEMSerialNumber;
            RemovedAssetDate = troubleTicket.RemovedAssetDate;

            ReplacementIPAddress = troubleTicket.ReplacementIPAddress;
            ReplacementOEMSerialNumber = troubleTicket.ReplacementOEMSerialNumber;
            ReplacementWiFiSSID = troubleTicket.ReplacementWiFiSSID;
            ReplacmentDate = troubleTicket.ReplacmentDate;
            ReplacmentIsFixed = troubleTicket.ReplacmentIsFixed;
            ShiftsMissed = troubleTicket.ShiftsMissed;

            ShippedAssetDate = troubleTicket.ShippedAssetDate;
            TroubleShootingDate = troubleTicket.TroubleShootingDate;


            TroubleTicketType = troubleTicket.TroubleTicketType;
            TroubleTicketStatus = troubleTicket.TroubleTicketStatus;

            VehicleId = troubleTicket.VehicleId;

            CreatedOn = auditDate;
            CreatedBy = auditUser;
        }

        public int TroubleTicketId { get; set; }
        public virtual TroubleTicket TroubleTicket { get; set; }
    }

    public enum TroubleTicketType
    {
        Mechanical,
        InVehicleEquipmentGeneral,
        InVehcileEquipmentLATATrax,
        BackInService
    }

    public enum TroubleTicketStatus
    {
        Unresolved,
        Pending,
        Resolved
    }

    public class TroubleTicketProblem
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        public string Problem { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class TroubleTicketComponentIssue
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        public string Issue { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class TroubleTicketTroubleTicketProblem
    {
        [Key]
        public int Id { get; set; }

        public int TroubleTicketId { get; set; }
        public virtual TroubleTicket TroubleTicket { get; set; }

        public int TroubleTicketProblemId { get; set; }
        public virtual TroubleTicketProblem TroubleTicketProblem { get; set; }
    }

    public class TroubleTicketTroubleTicketComponentIssue
    {
        [Key]
        public int Id { get; set; }

        public int TroubleTicketId { get; set; }
        public virtual TroubleTicket TroubleTicket { get; set; }

        public int TroubleTicketComponentIssueId { get; set; }
        public virtual TroubleTicketComponentIssue TroubleTicketComponentIssue { get; set; }
    }

    public class TroubleTicketAffectedDriver
    {
        [Key]
        public int Id { get; set; }

        public int TroubleTicketId { get; set; }
        public virtual TroubleTicket TroubleTicket { get; set; }

        public Guid DriverId { get; set; }
    }

    public class TroubleTicketLATATraxIssue
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        [Required]
        public string Issue { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class TroubleTicketTroubleTicketLATATraxIssue
    {
        [Key]
        public int Id { get; set; }

        public int TroubleTicketId { get; set; }
        public virtual TroubleTicket TroubleTicket { get; set; }

        public int TroubleTicketLATATraxIssueId { get; set; }
        public virtual TroubleTicketLATATraxIssue TroubleTicketLATATraxIssue { get; set; }
    }

    //public class TroubleTicketLATATraxReplacementAsset
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    public int TroubleTicketId { get; set; }

    //    public bool IsFixed { get; set; }

    //    [DisplayName("ReliaGate OEM Serial #")]
    //    public String ReliaGateOEMSerialNumber { get; set; }

    //    public String IPAddress { get; set; }

    //    public String WiFiSSID { get; set; }

    //    public DateTime CreatedOn { get; set; }

    //    public String CreatedBy { get; set; }

    //    public DateTime ModifiedOn { get; set; }

    //    public String ModifiedBy { get; set; }
    //}

    #endregion

    #region Scheduling

    public abstract class ScheduleDate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Abbreviation { get; set; }

        public DateTime Date { get; set; }


        public DateTime? EndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public abstract class Schedule
    {
        [Key]
        public int Id { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? ScheduleId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScheduleName { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class HolidayDate : ScheduleDate
    {
    }

    public class CustomDate : ScheduleDate
    {
    }

    public class HolidaySchedule : Schedule
    {
        [DisplayName("Holiday")]
        public int HolidayDateId { get; set; }

        public virtual HolidayDate HolidayDate { get; set; }
    }

    public class CustomSchedule : Schedule
    {
        [DisplayName("Custom Date")]
        public int CustomDateId { get; set; }

        public virtual CustomDate CustomDate { get; set; }
    }

    public class BeatHolidaySchedule
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Beat")]
        public Guid BeatId { get; set; }

        [DisplayName("Holiday Schedule")]
        public int HolidayScheduleId { get; set; }

        public virtual HolidaySchedule HolidaySchedule { get; set; }

        [DisplayName("Number of Trucks")]
        public int NumberOfTrucks { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class BeatCustomSchedule
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Beat")]
        public Guid BeatId { get; set; }

        [DisplayName("Custom Schedule")]
        public int CustomScheduleId { get; set; }

        public virtual CustomSchedule CustomSchedule { get; set; }

        [DisplayName("Number of Trucks")]
        [DefaultValue(1)]
        public int NumberOfTrucks { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #endregion

    #region Investigations

    public class Investigation
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Guid DriverId { get; set; }

        public Guid BeatId { get; set; }

        public Guid ContractorId { get; set; }

        public int ViolationTypeId { get; set; }
        public virtual ViolationType ViolationType { get; set; }

        [StringLength(1000)]
        public string Summary { get; set; }

        [DisplayName("Investigating Officer")]
        public int CHPOfficerId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public enum ViolationTypeSeverity
    {
        Minor,
        Major,
        Flagrant,
        Decertification,
        MinorToDecertification,
        MinorToMajor
    }

    #endregion

    #region OvertimeActivities

    [MetadataType(typeof(OvertimeActivityMetaData))]
    public partial class OvertimeActivity
    {
    }

    public class OvertimeActivityMetaData
    {
        public Guid ID { get; set; }

        [DisplayName("Time of Entry")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime timeStamp { get; set; }

        [DisplayName("Shift")]
        public string Shift { get; set; }

        [DisplayName("Callsign")]
        public string CallSign { get; set; }

        [DisplayName("Overtime Code")]
        public string OverTimeCode { get; set; }

        [DisplayName("Blocks Approved")]
        public int? BlocksApproved { get; set; }

        [DisplayName("Beat")]
        public string Beat { get; set; }

        [DisplayName("Contractor")]
        public string Contractor { get; set; }

        [DisplayName("Confirmed")]
        public bool? Confirmed { get; set; }
    }

    #endregion

    #region Violations

    public class Violation
    {
        public int Id { get; set; }
        public int ViolationTypeId { get; set; }
        public virtual ViolationType ViolationType { get; set; }

        [StringLength(150)]
        public string OffenseNumber { get; set; }

        public Guid ContractorId { get; set; }
        public DateTime DateTimeOfViolation { get; set; }
        public Guid? BeatId { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? FleetVehicleId { get; set; }

        [StringLength(150)]
        public string CallSign { get; set; }

        public int ViolationStatusTypeId { get; set; }
        public virtual ViolationStatusType ViolationStatusType { get; set; }
        public string DeductionAmount { get; set; }
        public string Notes { get; set; }
        public string PenaltyForDriver { get; set; }


        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        #region Alarm Intergation

        [StringLength(150)]
        public string AlarmName { get; set; }

        public int? AlarmDuration { get; set; }
        public DateTime? AlertTime { get; set; }
        public string LengthOfViolation { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public Guid? RunID { get; set; }
        public Guid? AlarmID { get; set; }

        #endregion
    }

    public class ViolationStatusType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class ViolationType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [DisplayName("Minimum Severity")]
        public ViolationTypeSeverity ViolationTypeSeverity { get; set; }

        [DisplayName("Maximum Severity")]
        public ViolationTypeSeverity MaxViolationTypeSeverity { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(150)]
        [DisplayName("Applies To")]
        public string AppliesTo { get; set; }

        [DisplayName("Is AVL Violation(via automatic alarm)")]
        public bool IsAVLViolation { get; set; }

        [DisplayName("Applicable LATATrax Alarm")]
        public string ApplicableLATATraxAlarm { get; set; }

        [DisplayName("Is Detectable by LATATrax Alone")]
        public bool DetectableByLATATraxAlone { get; set; }

        [DisplayName("Applicable Reports")]
        public string ApplicableReports { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    #endregion

    #region Misc

    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Code { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    public class VehiclePosition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Code { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    public class Transportation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Code { get; set; }

        public string Comments { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    public class LocationAbbreviation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Position { get; set; }

        [StringLength(150)]
        public string Abbreviation { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    public class DropSite
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Drop Site ID")]
        public string FriendlyId { get; set; }

        [DisplayName("Beat Number")]
        public string BeatNumber { get; set; }

        public string Freeway { get; set; }

        [DisplayName("Cross Street")]
        public string CrossStreet { get; set; }

        [DisplayName("Drop Site Area")]
        public string DropSiteArea { get; set; }

        [DisplayName("Drop Site Number")]
        public string DropSiteNumber { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    public class AlarmSubscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("User Email")]
        public string UserEmail { get; set; }

        [Required]
        [DisplayName("Alarm Name")]
        public string SubscribedAlarmName { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
    }

    #endregion

    #region appeals

    [MetadataType(typeof(AppealsMetaData))]
    public partial class Appeal
    {
    }

    public enum AppealTypes
    {
        Violation,
        Overtime,
        Invoice
    }

    public class AppealsMetaData
    {
        public Guid AppealID { get; set; }

        [Display(Name = "Appeal Type")]
        public AppealTypes AppealType { get; set; }

        [Display(Name = "Appeal Status")]
        public Guid AppealStatusID { get; set; }

        [Display(Name = "Contact Name")]
        [Required]
        public string ContactName { get; set; }

        [Display(Name = "Contact Phone")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        //[Display(Name = "Contractor")]
        //public Guid ContractorId { get; set; }

        public Guid Beatid { get; set; }

        //public Guid DriverId { get; set; }

        public int? V_ViolationId { get; set; }

        [Display(Name = "Reason For Appeal")]
        public string V_ReasonForAppeal { get; set; }

        [Display(Name = "Number of 15-Min Blocks Granted")]
        public int? V_BlocksGranted { get; set; }

        [Display(Name = "Number of 15-Min BLocks Claimed")]
        public int? V_BlocksClaimed { get; set; }

        [Display(Name = "Appropriate Charge")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? V_AppropriateCharge { get; set; }

        [Display(Name = "Date and Time of Claimed Overtime")]
        public DateTime? O_Datetime { get; set; }

        [Display(Name = "Callsign")]
        public string O_CallSign { get; set; }

        [Display(Name = "CHP Overtime #")]
        public int? O_CHPOTNumber { get; set; }

        [Display(Name = "Number of 15 minute blocks claimed")]
        public int? O_NumOfBlocks { get; set; }

        [Display(Name = "Number of 15 minute blocks granted")]
        public int? O_BlocksInitGranted { get; set; }

        [Display(Name = "Additional Detail")]
        public string O_Detail { get; set; }

        [Display(Name = "Event Date of deduction/addition")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? I_EventDate { get; set; }

        [Display(Name = "Reason for the deduction/addition")]
        public string I_InvoiceReason { get; set; }

        [Display(Name = "Reason for the appeal")]
        public string I_AppealReason { get; set; }

        [Display(Name = "Amount initially -/+ on invoice")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal? I_InvoiceDeduction { get; set; }

        [Display(Name = "Amount contractor believes appropriate")]
        [RegularExpression(@"^\d+.\d{0,2}$")]
        [Range(0, 9999999999999999.99)]
        public decimal? I_AppealDeduction { get; set; }

        [Display(Name = "MTC Note")]
        public string MTCNote { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public virtual BeatData BeatData { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual Driver Driver { get; set; }
        //public virtual AppealStatu AppealStatu { get; set; }
    }

    #endregion

    #region MTCRateTable

    [MetadataType(typeof(MTCRateTablesMetaData))]
    public partial class MTCRateTable
    {
        public string BeatNumber { get; set; }
    }

    public class MTCRateTablesMetaData
    {
    }

    #endregion

    public class DispatchCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string CodeDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MTCDatabase")
        {
        }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class MTCDbContext : DbContext
    {
        public MTCDbContext()
            : base("MTCDatabase")
        {
        }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        public DbSet<VehiclePosition> VehiclePositions { get; set; }

        public DbSet<Transportation> Transportation { get; set; }

        public DbSet<LocationAbbreviation> LocationAbbreviations { get; set; }

        public DbSet<DropSite> DropSites { get; set; }

        public DbSet<UserReport> UserReports { get; set; }

        public DbSet<MTCApplicationSetting> MTCApplicationSettings { get; set; }

        public DbSet<FAQ> FAQs { get; set; }

        public DbSet<FAQAnswer> FAQAnswers { get; set; }

        public DbSet<MerchandiseProduct> MerchandiseProducts { get; set; }

        public DbSet<MerchandiseOrder> MerchandiseOrders { get; set; }

        public DbSet<MerchandiseOrderAudit> MerchandiseOrderAudits { get; set; }

        public DbSet<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; }

        public DbSet<MerchandiseProductSize> MerchandiseProductSizes { get; set; }


        public DbSet<BackupBeat> BackupBeats { get; set; }

        public DbSet<BackupProvider> BackupProviders { get; set; }

        public DbSet<BackupAssignment> BackupAssignments { get; set; }


        public DbSet<BackupRequest> BackupRequests { get; set; }

        public DbSet<BackupReason> BackupReasons { get; set; }

        public DbSet<BackupRequestShiftAndDate> BackupRequestShiftsAndDates { get; set; }

        public DbSet<BackupResponse> BackupResponses { get; set; }

        public DbSet<BackupDeclinationReason> BackupDeclinationReasons { get; set; }

        public DbSet<BackupCancellationReason> BackupCancellationReasons { get; set; }

        public DbSet<AssetStatusLocation> AssetStatusLocations { get; set; }

        public DbSet<AssetWarranty> AssetWarranties { get; set; }

        public DbSet<AssetStatus> AssetStatuses { get; set; }

        public DbSet<TroubleTicket> TroubleTickets { get; set; }

        public DbSet<TroubleTicketAudit> TroubleTicketAudits { get; set; }

        public DbSet<TroubleTicketProblem> TroubleTicketProblems { get; set; }

        public DbSet<TroubleTicketTroubleTicketProblem> TroubleTicketTroubleTicketProblems { get; set; }

        public DbSet<TroubleTicketComponentIssue> TroubleTicketComponentIssues { get; set; }

        public DbSet<TroubleTicketTroubleTicketComponentIssue> TroubleTicketTroubleTicketComponentIssues { get; set; }

        public DbSet<TroubleTicketAffectedDriver> TroubleTicketAffectedDrivers { get; set; }

        public DbSet<TroubleTicketLATATraxIssue> TroubleTicketLATATraxIssues { get; set; }

        public DbSet<TroubleTicketTroubleTicketLATATraxIssue> TroubleTicketTroubleTicketLATATraxIssues { get; set; }
       
        public DbSet<DispatchCode> DispatchCodes { get; set; }

        public DbSet<HolidayDate> HolidayDates { get; set; }

        public DbSet<HolidaySchedule> HolidaySchedules { get; set; }

        public DbSet<BeatHolidaySchedule> BeatHolidaySchedules { get; set; }

        public DbSet<CustomDate> CustomDates { get; set; }

        public DbSet<CustomSchedule> CustomSchedules { get; set; }

        public DbSet<BeatCustomSchedule> BeatCustomSchedules { get; set; }

        public DbSet<Investigation> Investigations { get; set; }

        public DbSet<ViolationType> ViolationTypes { get; set; }

        public DbSet<Violation> Violations { get; set; }

        public DbSet<ViolationStatusType> ViolationStatusTypes { get; set; }

        public DbSet<AlarmSubscription> AlarmSubscriptions { get; set; }

        public DbSet<OvertimeActivity> OvertimeActivities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}