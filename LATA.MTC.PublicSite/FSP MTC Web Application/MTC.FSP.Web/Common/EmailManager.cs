using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.Common
{
    public static class EmailManager
    {
        public static void SendEmail(List<MTCEmailRecipient> toRecipients, string subject, string body, List<MTCEmailRecipient> ccAddresses)
        {
            try
            {
                var emailAccount = Utilities.GetApplicationSettingValue("EmailAccount");
                var emailAccountName = Utilities.GetApplicationSettingValue("EmailAccountName");
                var emailAccountPassword = Utilities.GetApplicationSettingValue("EmailAccountPassword");
                var emailHost = Utilities.GetApplicationSettingValue("EmailHost");

                SmtpClient smtp = new SmtpClient();

#if(TOLGAPC)
                    emailHost = "smtp.gmail.com";
                    emailAccount = "latatrax@gmail.com";
                    emailAccountName = "LATA Trax Admin";
                    emailAccountPassword = "L@T@2013";

                    smtp.Port = 587;
                    smtp.EnableSsl = true;
#endif
                var fromAddress = new MailAddress(emailAccount, emailAccountName);
                var fromPassword = emailAccountPassword;

                smtp.Host = emailHost;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

                using (var message = new MailMessage
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {
                    message.From = fromAddress;

                    foreach (var toRecipient in toRecipients)
                    {
                        if (!string.IsNullOrEmpty(toRecipient.Email))
                        {
                            message.To.Add(new MailAddress(toRecipient.Email, toRecipient.Name));
                        }
                    }

                    if (ccAddresses != null)
                    {
                        foreach (var ccAddress in ccAddresses)
                        {
                            if (!string.IsNullOrEmpty(ccAddress.Email))
                            {
                                message.CC.Add(new MailAddress(ccAddress.Email, ccAddress.Name));
                            }
                        }
                    }

                    try
                    {
                        smtp.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error sending email " + ex.Message);
                    }
                }
            }

            catch
            {
            }
        }

        public static string BuildBackupRequestSubject(string backupReqeustNumber, BackupAssignmentLevel assignmentLevel)
        {
            var retValue = string.Empty;

            if (assignmentLevel == BackupAssignmentLevel.Primary)
                retValue = "First Back-up Request (" + backupReqeustNumber + ") notification";
            else if (assignmentLevel == BackupAssignmentLevel.Secondary)
                retValue = "Second Back-up Request (" + backupReqeustNumber + ") notification";
            else if (assignmentLevel == BackupAssignmentLevel.Tertiary)
                retValue = "Third Back-up Request (" + backupReqeustNumber + ") notification";
            else if (assignmentLevel == BackupAssignmentLevel.AllBackupOperators)
                retValue = "Final Back-up Request (" + backupReqeustNumber + ") notification";

            return retValue;
        }

        public static string BuildBackupResponseSubject(BackupRequest backupRequest, BackupResponse backupResponse)
        {
            string retValue = string.Empty;

            if (backupResponse.BackupResponseStatus == BackupResponseStatus.Accepted)
                retValue = "Back-up Request (" + backupRequest.RequestNumber + ") statisfied";
            else if (backupResponse.BackupResponseStatus == BackupResponseStatus.Qualified)
                retValue = "Back-up Request (" + backupRequest.RequestNumber + ") NOT statisfied";
            else if (backupResponse.BackupResponseStatus == BackupResponseStatus.Declined)
                retValue = "Back-up Request (" + backupRequest.RequestNumber + ") declined";

            return retValue;
        }

        public static string BuildBackupCancelSubject(string backupReqeustNumber)
        {
            string retValue = string.Empty;
            retValue = "Back-up Request (" + backupReqeustNumber + ") cancelled.";
            return retValue;
        }


        public static string BuildBackupRequestEmail(BackupRequest backupRequest)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (MTCDBEntities db = new MTCDBEntities())
                {
                    sb.Append(EmailBody(backupRequest));

                    sb.Append("<table>");


                    using (MTCDbContext dc = new MTCDbContext())
                    {
                        var backupProviders = dc.BackupProviders.ToList();
                        var ba = dc.BackupAssignments.FirstOrDefault(p => p.BeatId == backupRequest.BeatId);

                        var primaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.PrimaryBackupBeatId).ContractorId;
                        var primaryContractor = GetContractorById(primaryContractorId);
                        var secondaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.SecondaryBackupBeatId).ContractorId;
                        var secondaryContractor = GetContractorById(secondaryContractorId);
                        var tertiaryContractorId = backupProviders.FirstOrDefault(c => c.BackupBeatId == ba.TertiaryBackupBeatId).ContractorId;
                        var tertiaryContractor = GetContractorById(tertiaryContractorId);

                        if (backupRequest.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.Secondary)
                        {
                            sb.Append("<h3>2. Back-up Contractor responses:</h3>");
                            sb.Append("<tr>");
                            sb.Append("<td>The primary back-up contractor " + primaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                        }
                        else if (backupRequest.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.Tertiary)
                        {
                            sb.Append("<h3>2. Back-up Contractor responses:</h3>");
                            sb.Append("<tr>");
                            sb.Append("<td>The primary back-up contractor " + primaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td>The secondary back-up contractor " + secondaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                        }
                        else if (backupRequest.CurrentBackupContractorAssignmentLevel == BackupAssignmentLevel.AllBackupOperators)
                        {
                            sb.Append("<h3>2. Back-up Contractor responses:</h3>");
                            sb.Append("<tr>");
                            sb.Append("<td>The primary back-up contractor " + primaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td>The secondary back-up contractor " + secondaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td>The tertiary back-up contractor " + tertiaryContractor.ContractCompanyName + " has either declined or only partially accepted the back-up request.</td>");
                            sb.Append("</tr>");
                        }
                    }
                }

                sb.Append("<tr>");
                sb.Append("<td><br/></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='2'><h3>Please log on to the LATATrax to respond. <a href='" + GetWebUri() + "/BackupTrucks/ResponseBackup' target='_blank'>Back-up Response</a>.</h3></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td><br/></td>");
                sb.Append("</tr>");

                sb.Append("</table>");


                sb.Append(EmailFooter());
            }
            catch
            {
            }


            return sb.ToString();
        }

        public static string BuildBackupResponseEmail(BackupRequest backupRequest, BackupResponse backupResponse)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (MTCDBEntities db = new MTCDBEntities())
                {
                    sb.Append(EmailBody(backupRequest));

                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td colspan='2'><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Responder's Comments:</td>");
                    sb.Append("<td>" + backupResponse.Comments + "</td>");
                    sb.Append("</tr>");


                    sb.Append("<tr>");
                    sb.Append("<td colspan='2'><br/></td>");
                    sb.Append("</tr>");

                    using (MTCDbContext dc = new MTCDbContext())
                    {
                        if (backupResponse.BackupResponseStatus == BackupResponseStatus.Accepted)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'><h3>2. The Back-up Request will be satisfied by the following Back-up Contractors.</h3></td>");
                            sb.Append("</tr>");

                            var respondingContractor = GetContractorById(backupResponse.ContractorId);

                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'>" + respondingContractor.ContractCompanyName + "</td>");
                            sb.Append("</tr>");
                        }
                        else if (backupResponse.BackupResponseStatus == BackupResponseStatus.Qualified)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'><h3>2. The Back-up Request will be partially satisfied by the following Back-up Contractors.</h3></td>");
                            sb.Append("</tr>");

                            var respondingContractor = GetContractorById(backupResponse.ContractorId);

                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'>" + respondingContractor.ContractCompanyName + "</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'><br/></td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append(
                                "<td colspan='2'>Some or all of the Back-up Request has not been satisfied by any Back-up Contractor. The back-up Request has been referred to MTC for resolution.</td>");
                            sb.Append("</tr>");
                        }
                        else if (backupResponse.BackupResponseStatus == BackupResponseStatus.Declined)
                        {
                            var declinationReason = dc.BackupDeclinationReasons.Find(backupResponse.BackupDeclinationReasonId);
                            if (declinationReason != null)
                            {
                                sb.Append("<tr>");
                                sb.Append("<td>Declination Reason:</td>");
                                sb.Append("<td>" + declinationReason.ReasonCode + " - " + declinationReason.Reason + "</td>");
                                sb.Append("</tr>");
                            }
                        }
                    }

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2'><h3>Please log on to the LATATrax to respond. <a href='" + GetWebUri() + "/BackupTrucks/ResponseBackup' target='_blank'>Back-up Response</a>.</h3></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");

                    sb.Append(EmailFooter());
                }
            }
            catch
            {
            }

            return sb.ToString();
        }

        public static string BuildBackupCancellEmail(BackupRequest backupRequest)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (MTCDBEntities db = new MTCDBEntities())
                {
                    sb.Append(EmailBody(backupRequest));

                    var primaryContractor = GetContractorById(backupRequest.ContractorId);
                    sb.Append("<h3>2. The requestor " + primaryContractor.ContractCompanyName + " has cancelled the Back-up Request.</h3>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td colspan='2'><h3>Please log on to the LATATrax to respond. <a href='" + GetWebUri() + "/BackupTrucks/ResponseBackup' target='_blank'>Back-up Response</a>.</h3></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append(EmailFooter());
                }
            }
            catch
            {
            }


            return sb.ToString();
        }

        public static string BuildTroubleTicketEmailBody(TroubleTicket troubleTicket)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (MTCDbContext db = new MTCDbContext())
                {
                    sb.Append("<h3>1. The following Trouble Ticket has been entered or edited in LATATrax.</h3>");

                    sb.Append("<table>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Ticket #</td>");
                    sb.Append("<td>" + troubleTicket.Id + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Status</td>");
                    sb.Append("<td>" + Enum.GetName(typeof(TroubleTicketStatus), troubleTicket.TroubleTicketStatus) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Type</td>");
                    sb.Append("<td>" + Enum.GetName(typeof(TroubleTicketType), troubleTicket.TroubleTicketType) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Component Issues</td>");
                    sb.Append("<td>");
                    var ttCompomentIssues = db.TroubleTicketTroubleTicketComponentIssues.Where(p => p.TroubleTicketId == troubleTicket.Id);
                    foreach (var ttCompomentIssue in ttCompomentIssues)
                    {
                        sb.Append("<div>" + ttCompomentIssue.TroubleTicketComponentIssue.Issue + "</div");
                    }

                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");


                    using (MTCDBEntities de = new MTCDBEntities())
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>Tow Contractor:</td>");
                        sb.Append("<td>" + de.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedTowContractorId).ContractCompanyName + "</td>");
                        sb.Append("</tr>");

                        if (troubleTicket.AssociatedInVehicleContractorId != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>In-Vehicle Equipmemt General Contractor:</td>");
                            sb.Append("<td>" + de.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleContractorId).ContractCompanyName + "</td>");
                            sb.Append("</tr>");
                        }

                        if (troubleTicket.AssociatedInVehicleLATATraxContractorId != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>In-Vehicle Equipment LATATrax Contractor:</td>");
                            sb.Append("<td>" + de.Contractors.FirstOrDefault(p => p.ContractorID == troubleTicket.AssociatedInVehicleLATATraxContractorId).ContractCompanyName + "</td>");
                            sb.Append("</tr>");
                        }
                    }

                    sb.Append("<tr>");
                    sb.Append("<td>Problem Started On:</td>");
                    sb.Append("<td>" + troubleTicket.ProblemStartedOn + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>MTC Notes:</td>");
                    sb.Append("<td>" + troubleTicket.MTCNotes + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Tow-Contractor Notes:</td>");
                    sb.Append("<td>" + troubleTicket.ContractorNotes + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>In-Vehicle Contractor Notes:</td>");
                    sb.Append("<td>" + troubleTicket.InVehicleContractorNotes + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>LATA Notes:</td>");
                    sb.Append("<td>" + troubleTicket.LATANotes + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td><br/></td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Troule Ticket Details:</td>");
                    sb.Append("<td><a href='" + GetWebUri() + "/TroubleTickets/Details?id=" + troubleTicket.Id + "' target='_blank'>View Details</a></td>");
                    sb.Append("</tr>");


                    sb.Append("</table>");
                }


                sb.Append(EmailFooter());
            }
            catch
            {
            }

            return sb.ToString();
        }

        public static string BuildMerchandiseOrderEmailBody(MerchandiseOrder order, bool isUpdate)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                var sb = new StringBuilder();
                var contractCompanyName = db.Contractors.FirstOrDefault(p => p.ContractorID == order.ContractorId).ContractCompanyName;

                #region

                sb.Append("<h3>Merchandise Order Summary</h3>");

                if (isUpdate)
                {
                    sb.Append("<h4>Your original order has been changed</h4>");
                }

                sb.Append("<table>");

                sb.Append("<tr>");
                sb.Append("<td>Order ID</td>");
                sb.Append("<td>" + order.Id + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Contractor</td>");
                sb.Append("<td>" + contractCompanyName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Contact Name</td>");
                sb.Append("<td>" + order.ContactName + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Contact Number</td>");
                sb.Append("<td>" + order.ContactNumber + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Pick-up Date</td>");
                sb.Append("<td>" + order.PickupDate.ToShortDateString() + "</td>");
                sb.Append("</tr>");

                //sb.Append("<tr>");
                //sb.Append("<td>Pick-up Time</td>");
                //sb.Append("<td>" + (order.PickupTime == "911" ? "9-11 AM" : "1-5 PM") + "</td>");
                //sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Pick-up Time</td>");

                var amOrPm = "PM";
                if (order.PickupTime == "9" || order.PickupTime == "10" || order.PickupTime == "11")
                    amOrPm = "AM";

                sb.Append("<td>" + order.PickupTime + " " + amOrPm + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Payment</td>");
                sb.Append("<td>" + (order.PayByCheck ? "Check" : "Deduct from Invoice") + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Order Status</td>");

                if (order.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderCancelled)
                    sb.Append("<td>Cancelled</td>");
                else if (order.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderDeclined)
                    sb.Append("<td>Decline</td>");
                else if (order.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderFilled)
                    sb.Append("<td>Filled</td>");
                else if (order.MerchandiseOrderStatus == MerchandiseOrderStatus.OrderSubmitted)
                    sb.Append("<td>Submitted</td>");

                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Comments</td>");
                sb.Append("<td>" + order.Comment + "</td>");
                sb.Append("</tr>");


                sb.Append("</table>");

                sb.Append("</br>");

                #endregion

                using (MTCDbContext d = new MTCDbContext())
                {
                    sb.Append("For details of this order, please click <a href='" + d.MTCApplicationSettings.Where(p => p.Name == "WebRoot").FirstOrDefault().Value +
                              "/Merchandise/OrderHistory' target='_blank'>here</a>.");
                }

                return sb.ToString();
            }
        }

        public static string BuildMerchandiseProductLowInventoryEmailBody(MerchandiseProduct product)
        {
            var sb = new StringBuilder();

            sb.Append("<h3>Low inventory for " + product.DisplayName + "</h3>");

            sb.Append("<table>");

            sb.Append("<tr>");
            sb.Append("<td>Product ID</td>");
            sb.Append("<td>" + product.Id + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>Display  Name</td>");
            sb.Append("<td>" + product.DisplayName + "</td>");
            sb.Append("</tr>");

            sb.Append("<tr>");
            sb.Append("<td>Current Inventory Level</td>");
            sb.Append("<td>" + product.UnitsInStock + "</td>");
            sb.Append("</tr>");

            sb.Append("</table>");

            using (MTCDbContext d = new MTCDbContext())
            {
                sb.Append("For further details of this product, please click <a href='" + d.MTCApplicationSettings.Where(p => p.Name == "WebRoot").FirstOrDefault().Value +
                          "/MerchandiseProducts/Edit?id=" + product.Id + "' target='_blank'>here</a>.");
            }

            return sb.ToString();
        }


        public static Contractor GetContractorById(Guid contractorId)
        {
            using (MTCDBEntities db = new MTCDBEntities())
            {
                return db.Contractors.FirstOrDefault(p => p.ContractorID == contractorId);
            }
        }

        private static string EmailFooter()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string mtcEmail = Utilities.GetApplicationSettingValue("LATATraxSupportEmail");
                string mtcPhone = Utilities.GetApplicationSettingValue("LATATraxSupportPhone");

                sb.Append("<table>");

                sb.Append("<tr>");
                sb.Append("<td colspan='2'>If you have any problems loggin on or submitting your response, email</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td colspan='2'><a href='mailto:'" + mtcEmail + ">" + mtcEmail + "</a> or call " + mtcPhone + " </td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td><br/></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Thank you!</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td><br/></td>");
                sb.Append("</tr>");

                sb.Append("</table>");
            }
            catch
            {
            }


            return sb.ToString();
        }

        private static string EmailBody(BackupRequest backupRequest)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                int _urgentBackupRequestResponseTimeInMinutes = 0;
                int _standardBackupRequestResponseTimeInMinutes = 0;
                BackupReason backupReason = null;
                List<BackupRequestShiftAndDate> backupRequestShiftsAndDates = new List<BackupRequestShiftAndDate>();


                using (MTCDbContext db = new MTCDbContext())
                {
                    _urgentBackupRequestResponseTimeInMinutes = Convert.ToInt32(Utilities.GetApplicationSettingValue("UrgentBackupRequestResponseTimeInMinutes"));
                    _standardBackupRequestResponseTimeInMinutes = Convert.ToInt32(Utilities.GetApplicationSettingValue("StandardBackupRequestResponseTimeInMinutes"));

                    backupReason = db.BackupReasons.Find(backupRequest.BackupReasonId);
                    backupRequestShiftsAndDates = db.BackupRequestShiftsAndDates.Where(p => p.BackupRequestId == backupRequest.Id).ToList();
                }

                string requestPriority = "Standard (You have up to " + _standardBackupRequestResponseTimeInMinutes / 60 + " hours to respond)";
                if (backupRequest.RequestIsUrgent)
                    requestPriority = "Urgent (You have up to " + _urgentBackupRequestResponseTimeInMinutes + " minutes to respond)";

                sb.Append("<h3>1. The following Back-up Truck Service Request has been entered into LATATrax.</h3>");

                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td>Request #</td>");
                sb.Append("<td>" + backupRequest.RequestNumber + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Request Priority</td>");
                sb.Append("<td>" + requestPriority + "</td>");
                sb.Append("</tr>");

                using (var de = new MTCDBEntities())
                {
                    sb.Append("<tr>");
                    sb.Append("<td>Requesting Contractor:</td>");

                    var contractor = de.Contractors.FirstOrDefault(p => p.ContractorID == backupRequest.ContractorId);
                    if (contractor != null)
                        sb.Append("<td>" + contractor.ContractCompanyName + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>Beat needing Back-up:</td>");
                    var beat = de.BeatDatas.FirstOrDefault(p => p.ID == backupRequest.BeatId);
                    if (beat != null)
                        sb.Append("<td>" + beat.BeatName + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td>Dates and Shifts Back-up Needed:</td>");
                sb.Append("<td>");

                foreach (var b in backupRequestShiftsAndDates)
                {
                    var shifts = "";
                    if (b.AMRequested)
                        shifts = " AM ";
                    if (b.MIDRequested)
                        shifts += " MID ";
                    if (b.PMRequested)
                        shifts += " PM ";

                    sb.Append("<span><b>" + b.BackupDate.ToString("MM/dd/yyyy") + "</b>  " + shifts + "</span><br />");
                }

                sb.Append("</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Back-up Reason Code:</td>");
                if (backupReason != null)
                    sb.Append("<td>" + backupReason.ReasonCode + " - " + backupReason.Reason + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>Requestor's Comments:</td>");
                sb.Append("<td>" + backupRequest.Comments + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td><br/></td>");
                sb.Append("</tr>");

                sb.Append("</table>");
            }
            catch
            {
            }

            return sb.ToString();
        }

        private static string GetWebUri()
        {
            return Utilities.GetApplicationSettingValue("WebRoot");
        }
    }

    public class MTCEmailRecipient
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}