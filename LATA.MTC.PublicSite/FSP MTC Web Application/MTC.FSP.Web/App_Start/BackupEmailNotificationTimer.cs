using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web
{
    public class BackupEmailNotificationTimer
    {
        private static Timer _aTimer;
        private readonly int _standardBackupRequestResponseTimeInMinutes;
        private readonly double _timerExecutionIntervalLengthInMilliseconds = 60000;
        private readonly int _urgentBackupRequestResponseTimeInMinutes;

        public BackupEmailNotificationTimer()
        {
            _urgentBackupRequestResponseTimeInMinutes = Convert.ToInt32(Utilities.GetApplicationSettingValue("UrgentBackupRequestResponseTimeInMinutes"));
            _standardBackupRequestResponseTimeInMinutes = Convert.ToInt32(Utilities.GetApplicationSettingValue("StandardBackupRequestResponseTimeInMinutes"));

            _aTimer = new Timer(_timerExecutionIntervalLengthInMilliseconds);
            _aTimer.Elapsed += aTimer_Elapsed;
            _aTimer.Enabled = true;

            CheckBackupRequests();
        }

        private void aTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckBackupRequests();
        }

        private void CheckBackupRequests()
        {
            using (var db = new MTCDbContext())
            {
                var backupRequests = db.BackupRequests.Where(p => p.IsCancelled == false).ToList().Select(p => new
                {
                    p.Id,
                    p.ContractorId,
                    p.BeatId,
                    p.LastExpiredOn,
                    p.PrimaryBackupResponseTimeExpiredOrDeclined,
                    p.SecondaryBackupResponseTimeExpiredOrDeclined,
                    p.TertiaryBackupResponseTimeExpiredOrDeclined,
                    p.AllBackupsNotified,
                    Resolved = db.BackupResponses.Any(l => l.BackupRequestId == p.Id && (l.BackupResponseStatus == BackupResponseStatus.Accepted || l.BackupResponseStatus == BackupResponseStatus.Qualified)),
                    p.SelectedBackupContractorId,
                    p.SelectedBackupContractorAssignmentLevel,
                    p.RequestIsUrgent
                });


                foreach (var unResolvedBackupRequest in backupRequests.Where(p => p.Resolved == false))
                {
                    //if the time the backup request was made is longer than the initial respoinse time, send email to next guy in line
                    var now = DateTime.Now;
                    var timeBackupRequestWasMade = unResolvedBackupRequest.LastExpiredOn;

                    var timeSinceBackupRequestWasMadeInMinutes = now.Subtract(timeBackupRequestWasMade).Minutes;

                    var responseTimeInMinutes = _standardBackupRequestResponseTimeInMinutes;
                    if (unResolvedBackupRequest.RequestIsUrgent)
                        responseTimeInMinutes = _urgentBackupRequestResponseTimeInMinutes;

                    var backupProviders = db.BackupProviders.ToList();

                    if (timeSinceBackupRequestWasMadeInMinutes >= responseTimeInMinutes)
                    {
                        var backupRequest = db.BackupRequests.Find(unResolvedBackupRequest.Id);
                        var backupAssignment = db.BackupAssignments.FirstOrDefault(p => p.BeatId == unResolvedBackupRequest.BeatId);

                        var requestor = EmailManager.GetContractorById(unResolvedBackupRequest.ContractorId);
                        var primaryBackupContractor = EmailManager.GetContractorById(backupProviders.FirstOrDefault(p => p.BackupBeatId == backupAssignment.PrimaryBackupBeatId).ContractorId); //EmailManager.GetContractorById(backupAssignment.PrimaryBackupContractorId);
                        var secondaryBackupContractor = EmailManager.GetContractorById(backupProviders.FirstOrDefault(p => p.BackupBeatId == backupAssignment.SecondaryBackupBeatId).ContractorId); //EmailManager.GetContractorById(backupAssignment.PrimaryBackupContractorId);
                        var tertiaryBackupContractor = EmailManager.GetContractorById(backupProviders.FirstOrDefault(p => p.BackupBeatId == backupAssignment.TertiaryBackupBeatId).ContractorId); //EmailManager.GetContractorById(backupAssignment.PrimaryBackupContractorId);

                        if (backupAssignment != null)
                        {
                            var ccRecipients = new List<MTCEmailRecipient>();
                            var toRecipients = new List<MTCEmailRecipient>();
                            var toRecipient = new MTCEmailRecipient();
                            var backupAssignmentLevel = BackupAssignmentLevel.Primary;

                            if (unResolvedBackupRequest.PrimaryBackupResponseTimeExpiredOrDeclined == false)
                            {
                                //PBUO's time has expired. Now, start clock for SBUO.

                                #region

                                backupRequest.PrimaryBackupResponseTimeExpiredOrDeclined = true;
                                backupRequest.PrimaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                                backupRequest.CurrentBackupContractorId = backupProviders.FirstOrDefault(p => p.BackupBeatId == backupAssignment.SecondaryBackupBeatId).ContractorId; //backupAssignment.SecondaryBackupContractorId;
                                backupRequest.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.Secondary;
                                backupRequest.LastExpiredOn = DateTime.Now;
                                db.SaveChanges();

                                //Email to SBUO
                                backupAssignmentLevel = BackupAssignmentLevel.Secondary;
                                toRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = secondaryBackupContractor.Email,
                                    Name = secondaryBackupContractor.ContractCompanyName
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                                });

                                #endregion
                            }
                            else if (unResolvedBackupRequest.PrimaryBackupResponseTimeExpiredOrDeclined &&
                                     unResolvedBackupRequest.SecondaryBackupResponseTimeExpiredOrDeclined == false)
                            {
                                //SBUO's time has expired. Now, start clock for TBUO.

                                #region Seconday

                                backupRequest.SecondaryBackupResponseTimeExpiredOrDeclined = true;
                                backupRequest.SecondaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                                backupRequest.CurrentBackupContractorId = backupProviders.FirstOrDefault(p => p.BackupBeatId == backupAssignment.TertiaryBackupBeatId).ContractorId; //backupAssignment.TertiaryBackupContractorId;
                                backupRequest.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.Tertiary;
                                backupRequest.LastExpiredOn = DateTime.Now;
                                db.SaveChanges();

                                //Email to TBUO
                                backupAssignmentLevel = BackupAssignmentLevel.Tertiary;
                                toRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = tertiaryBackupContractor.Email,
                                    Name = tertiaryBackupContractor.ContractCompanyName
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                                });

                                #endregion
                            }
                            else if (unResolvedBackupRequest.PrimaryBackupResponseTimeExpiredOrDeclined &&
                                     unResolvedBackupRequest.SecondaryBackupResponseTimeExpiredOrDeclined &&
                                     unResolvedBackupRequest.TertiaryBackupResponseTimeExpiredOrDeclined == false)
                            {
                                //TBUO's time has expired. Send email to all.

                                #region

                                backupRequest.TertiaryBackupResponseTimeExpiredOrDeclined = true;
                                backupRequest.TertiaryBackupResponseTimeExpiredOrDeclinedOn = DateTime.Now;
                                backupRequest.CurrentBackupContractorId = null;
                                backupRequest.CurrentBackupContractorAssignmentLevel = BackupAssignmentLevel.AllBackupOperators;
                                backupRequest.LastExpiredOn = DateTime.Now;
                                backupAssignmentLevel = BackupAssignmentLevel.AllBackupOperators;
                                backupRequest.AllBackupsNotified = true;
                                db.SaveChanges();

                                //Email to all BUOs
                                foreach (var backupProvider in backupProviders)
                                    if (backupProvider.ContractorId != primaryBackupContractor.ContractorID &&
                                        backupProvider.ContractorId != secondaryBackupContractor.ContractorID &&
                                        backupProvider.ContractorId != tertiaryBackupContractor.ContractorID)
                                    {
                                        var backupContractor = EmailManager.GetContractorById(backupProvider.ContractorId);
                                        toRecipients.Add(new MTCEmailRecipient {Email = backupContractor.Email, Name = backupContractor.ContractCompanyName});
                                    }

                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = Utilities.GetApplicationSettingValue("MTCContactEmail"),
                                    Name = Utilities.GetApplicationSettingValue("MTCContactName")
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = requestor.Email,
                                    Name = requestor.ContractCompanyName
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = primaryBackupContractor.Email,
                                    Name = primaryBackupContractor.ContractCompanyName
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = secondaryBackupContractor.Email,
                                    Name = secondaryBackupContractor.ContractCompanyName
                                });
                                ccRecipients.Add(new MTCEmailRecipient
                                {
                                    Email = tertiaryBackupContractor.Email,
                                    Name = tertiaryBackupContractor.ContractCompanyName
                                });

                                #endregion
                            }

                            var body = EmailManager.BuildBackupRequestEmail(backupRequest);

                            //Emails
                            if (toRecipients.Count() > 0 && ccRecipients.Count() > 0)
                                EmailManager.SendEmail(toRecipients,
                                    EmailManager.BuildBackupRequestSubject(backupRequest.RequestNumber,
                                        backupAssignmentLevel),
                                    body,
                                    ccRecipients);
                        }
                    }
                }
            }
        }
    }
}


//try
//                   {
//                       var subject = "RE: Request for Back-up Truck Service";

//                       var selectedBackupContractor = db.Contractors.FirstOrDefault(p => p.ContractorID == selectedBackupContractorId);
//                       if (selectedBackupContractor != null)
//                       {
//                           var email = selectedBackupContractor.Email; 
//                           var name = selectedBackupContractor.ContractCompanyName;

//                           var requestingContractor = db.Contractors.FirstOrDefault(p => p.ContractorID == backupRequest.ContractorId);
//                           var requestingContractorEmail = requestingContractor.Email;
//                           var requestingContractorName = requestingContractor.ContractCompanyName;

//                           //copy all other assigned back-up contractors

//                           SendEmail(email, name, subject, sb.ToString(), requestingContractorEmail, requestingContractorName);
//                       }
//                   }
//                   catch { }