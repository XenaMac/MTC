using FSP.Domain.Model;
using FSP.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;

namespace FSP.Web.Helpers
{
    public static class Util
    {
        #region Status Codes
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        public static void WriteToLog(String message, String logName)
        {
            if (ConfigurationManager.AppSettings["IsLoggingEnabled"] == "true")
            {
                //if logging is enabled

                try
                {
                    String sLogFilePath = ConfigurationManager.AppSettings["LogFileLocation"] + logName;
                    StreamWriter sw = null;
                    if (!File.Exists(sLogFilePath))
                        sw = File.CreateText(sLogFilePath);
                    else
                        sw = File.AppendText(sLogFilePath);

                    sw.WriteLine(DateTime.Now + " " + message);
                    sw.Close();
                }
                catch { }
            }

            System.Diagnostics.Debug.WriteLine(message);

        }

        public static void WriteToEventLog(String message)
        {
            try
            {
                string sSource = "FSPWeb";
                string sLog = "Application";

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, message, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                WriteToLog(ex.Message, "Error.txt");
            }

            System.Diagnostics.Debug.WriteLine(message);

        }

        public static void SendEmail(string subject, string body)
        {
            String toEmailAddress = ConfigurationManager.AppSettings["OCTAAdminEmailAddress"];
            String toName = ConfigurationManager.AppSettings["OCTAAdminName"];

            var fromAddress = new MailAddress("latatrax@gmail.com", "LATA Trax");
            var toAddress = new MailAddress(toEmailAddress, toName);
            const string fromPassword = "L@T@2013";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public static void CreateAssist(UIIncidentDispatch incidentDispatch)
        {
            using (FSPDataContext dc = new FSPDataContext())
            {
                using (TowTruckServiceRef.TowTruckServiceClient service = new TowTruckServiceRef.TowTruckServiceClient())
                {
                    TowTruckServiceRef.IncidentIn thisIncident = new TowTruckServiceRef.IncidentIn();

                    Guid IncidentID = Guid.NewGuid();
                    DateTime TimeStamp = DateTime.Now;

                    Guid CreatedBy = MembershipExtensions.GetUserId();

                    thisIncident.IncidentID = IncidentID;
                    thisIncident.FreewayID = Convert.ToInt32(incidentDispatch.FreewayId);
                    thisIncident.LocationID = dc.Locations.Where(p => p.LocationCode == incidentDispatch.LocationName).FirstOrDefault().LocationID;
                    thisIncident.TimeStamp = TimeStamp;
                    thisIncident.CreatedBy = CreatedBy;
                    thisIncident.Description = incidentDispatch.Description;
                    thisIncident.IncidentNumber = String.Empty;

                    //create incident
                    service.AddIncident(thisIncident);

                    //for each truck create assit

                    foreach (var truck in incidentDispatch.SelectedTrucks)
                    {
                        TowTruckServiceRef.AssistReq thisAssist = new TowTruckServiceRef.AssistReq();
                        thisAssist.AssistID = Guid.NewGuid();

                        thisAssist.IncidentID = IncidentID;

                        FleetVehicle dbTruck = dc.FleetVehicles.Where(p => p.VehicleNumber == truck).FirstOrDefault();

                        thisAssist.FleetVehicleID = dbTruck.FleetVehicleID;
                        thisAssist.ContractorID = dbTruck.ContractorID;

                        thisAssist.DispatchTime = DateTime.Now;
                        thisAssist.x1097 = DateTime.Now;
                        service.AddAssist(thisAssist);
                    }

                }
            }
         
        }


    }
}