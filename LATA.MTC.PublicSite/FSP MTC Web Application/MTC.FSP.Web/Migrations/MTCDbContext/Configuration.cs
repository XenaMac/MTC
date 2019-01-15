namespace MTC.FSP.Web.Migrations.MTCDbContext
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MTC.FSP.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MTC.FSP.Web.Models.MTCDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MTCDbContext context)
        {
            return;

            //context.AlarmSubscriptions.AddOrUpdate(
            //      p => new { p.UserEmail, p.SubscribedAlarmName },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "LONGBREAK", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "LONGLUNCH", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "STATIONARY", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "OFFBEAT", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "LATEONPATROL", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "EARLYOUTOFSERVICE", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "SPEEDING", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "GPSISSUE", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "LONGINCIDENT", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new AlarmSubscription { UserEmail = "tkoseoglu@live.com", SubscribedAlarmName = "OVERTIMEACTIVITY", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //);

            //context.MTCApplicationSettings.AddOrUpdate(
            //p => p.Name,
            //new MTCApplicationSetting { Name = "MTCMarketingContactEmail", Value = "fsp@mtc.ca.gov", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "MTCMarketingContactPhone", Value = "1.800.555.555", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "MTCMarketingContactName", Value = "Jane Doe", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "MTCContactName", Value = "MTC", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "LATATraxSupportName", Value = "LATATrax Support", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "MerchandiseOrderFormRecipientName", Value = "Jeanne Woodfin", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //new MTCApplicationSetting { Name = "MotoristSurveyUrl", Value = "http://38.124.164.212/MTCMotoristSurvey/Admin/Logon.aspx", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //);        
            
            //context.ViolationStatusTypes.AddOrUpdate(
            //      p => p.Name,
            //      new ViolationStatusType { Name = "Not Checked", IsDefault = true, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new ViolationStatusType { Name = "Confirmed", IsDefault = false, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new ViolationStatusType { Name = "Cleared", IsDefault = false, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //      new ViolationStatusType { Name = "Deducted", IsDefault = false, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //);

            //context.ViolationTypes.AddOrUpdate(
            //   p => p.Code,
            //   new ViolationType { Code = "1", Name = "Sleeping", Description = "Sleeping or giving the appearance of sleeping while in or around an FSP vehicle during FSPhours", ViolationTypeSeverity = Models.ViolationTypeSeverity.Major, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new ViolationType { Code = "2", Name = "Using poor judgement", Description = "Any act or violation of FSP policy that is deemed to be egregious, neglicent, intentional or malicious in nature can result in the following penalties", ViolationTypeSeverity = Models.ViolationTypeSeverity.MinorToDecertification, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            // );



            //context.DispatchCodes.AddOrUpdate(
            //   p => p.Code,
            //   new DispatchCode { Code = "11-83", CodeDescription = "Accident-No Details", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new DispatchCode { Code = "11-82", CodeDescription = "Accident-Property Damage", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new DispatchCode { Code = "11-24", CodeDescription = "Abandoned Vehicle", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new DispatchCode { Code = "11-26", CodeDescription = "Disabled Vehicle-Occupied", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new DispatchCode { Code = "11-25", CodeDescription = "Traffic Hazard", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }

            // );

            //context.TroubleTicketLATATraxIssues.AddOrUpdate(
            //   p => p.Issue,
            //   new TroubleTicketLATATraxIssue { Issue = "System does not power on when truck starts", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "Log on issues", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "WiFi issues", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "Tablet will not connet to WiFi", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "Tablet screen freezes up", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "Screen does not show proper status", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketLATATraxIssue { Issue = "On Break/On Lunch issues", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            // );


            //context.TroubleTicketComponentIssues.AddOrUpdate(
            //   p => p.Issue,
            //   new TroubleTicketComponentIssue { Issue = "Radio works poorly", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Radio does not work at all", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Radio handset", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Radio antenna", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Microphone", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Push to Talk", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new TroubleTicketComponentIssue { Issue = "Data antenna", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            // );

            //context.TroubleTicketProblems.AddOrUpdate(
            //    p => p.Problem,
            //    new TroubleTicketProblem { Problem = "Engine Failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Engine fluid leak", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Hydraulic fluid leak", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Chassis integrity", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Flat tire", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Winch/sling failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Brake Failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "PA system failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new TroubleTicketProblem { Problem = "Other", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //  );


            //context.AssetStatuses.AddOrUpdate(
            //    p => p.StatusName,
            //    new AssetStatus { StatusName = "Up-Installed", Color = "Green", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Up-RFI", Color = "Green", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Up-Shipped", Color = "Green", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Up-Awaiting Installation", Color = "Green", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Down-Awaiting Troubleshooting", Color = "Red", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Down-Awaiting Shipping", Color = "Red", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Down-Shipped", Color = "Red", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new AssetStatus { StatusName = "Being Repaired", Color = "Red", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //  );


            //context.BackupReasons.AddOrUpdate(
            //    p => p.ReasonCode,
            //    new BackupReason { ReasonCode = "A", Reason = "Regular truck down due to mechanical failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupReason { ReasonCode = "B", Reason = "Regular truck was involved in an accident", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupReason { ReasonCode = "C", Reason = "Regular truck has a flat tire", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupReason { ReasonCode = "D", Reason = "Lack of drivers", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupReason { ReasonCode = "E", Reason = "Taken out of service by CHP", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //  );

            //context.BackupDeclinationReasons.AddOrUpdate(
            //    p => p.ReasonCode,
            //    new BackupDeclinationReason { ReasonCode = "A", Reason = "Back-up alreday in use", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupDeclinationReason { ReasonCode = "B", Reason = "Back-up down due to mechanical failure", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupDeclinationReason { ReasonCode = "C", Reason = "Back-up down to involvement in an accident", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupDeclinationReason { ReasonCode = "D", Reason = "Lack of drivers", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //    new BackupDeclinationReason { ReasonCode = "E", Reason = "Back-up taken out of service by CHP", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //  );

            // context.BackupCancellationReasons.AddOrUpdate(
            //   p => p.ReasonCode,
            //   new BackupCancellationReason { ReasonCode = "A", Reason = "Truck repaired", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new BackupCancellationReason { ReasonCode = "B", Reason = "Drivers available", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new BackupCancellationReason { ReasonCode = "C", Reason = "Conflicts resolved", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //   new BackupCancellationReason { ReasonCode = "D", Reason = "Other", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            // );


            // context.MerchandiseProductSizes.AddOrUpdate(
            //  p => p.Size,
            //  new MerchandiseProductSize { Size = "M", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProductSize { Size = "L", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProductSize { Size = "XL", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProductSize { Size = "2XL", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProductSize { Size = "3XL", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProductSize { Size = "4XL", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //);

            //context.SaveChanges();

            //context.MerchandiseProducts.AddOrUpdate(
            //  p => p.Id,
            //  new MerchandiseProduct { DisplayName = "3.5 FSP Patch", UnitCost = 1.40m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "7 FSP Patch", UnitCost = 2.70m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "15x15 Magnetic Sign", UnitCost = 17.50m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "20x20 Magnetic Sign", UnitCost = 24.05m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Metal Sign Bracket", UnitCost = 40.00m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "FSP Mug", UnitCost = 4.00m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "FSP Baseball Cap", UnitCost = 5.50m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },

            //  new MerchandiseProduct { DisplayName = "Blue FSP Short Sleeve T-Shirt", UnitCost = 7.60m, MerchandiseProductSizeId = 1, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Blue FSP Short Sleeve T-Shirt", UnitCost = 7.60m, MerchandiseProductSizeId = 2, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Blue FSP Short Sleeve T-Shirt", UnitCost = 7.60m, MerchandiseProductSizeId = 3, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Blue FSP Short Sleeve T-Shirt", UnitCost = 7.60m, MerchandiseProductSizeId = 4, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Blue FSP Short Sleeve T-Shirt", UnitCost = 7.60m, MerchandiseProductSizeId = 5, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },

            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 1, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 2, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 3, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 4, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 5, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Lime Reflective FSP Safety Vest", UnitCost = 49.00m, MerchandiseProductSizeId = 6, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },

            //  new MerchandiseProduct { DisplayName = "Out of Service Magnet", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Daily Shift Records- DSR (1,000/box)", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Survey Cards (500/box)", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "422's (200/pack)", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Multilanguage Card", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" },
            //  new MerchandiseProduct { DisplayName = "Motorist Brochures (3,600/box)", UnitCost = 0.0m, Description = "", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, CreatedBy = "tkoseoglu@live.com", ModifiedBy = "tkoseoglu@live.com" }
            //);

        }
    }
}
