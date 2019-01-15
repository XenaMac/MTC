using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FPSService
{
    public class Global : System.Web.HttpApplication
    {

        public static CADIntegration.CADListener cListener;
        public static CADIntegration.CADSender cSender;
        public UDP.UDPServer myServer;

        protected void Application_Start(object sender, EventArgs e)
        {

            myServer = new UDP.UDPServer();
            DataClasses.TowTruckCleanser myCleanser = new DataClasses.TowTruckCleanser();
            
            

            MiscData.LogonCheck myCheck = new MiscData.LogonCheck();
            Logging.EventLogger myLogger = new Logging.EventLogger();
            myLogger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "FSP Service Started", false);
            //Data loaded once during run of application.
            SQL.SQLCode mySQL = new SQL.SQLCode();
            int SpeedingValue = Convert.ToInt32(mySQL.GetVarValue("Speeding"));
            DataClasses.GlobalData.SpeedingLeeway = SpeedingValue;
            mySQL.LoadIncidentTypes();
            mySQL.LoadLocationAbbreviations();
            mySQL.LoadVehiclePositions();
            mySQL.LoadVehicleTypes();
            mySQL.LoadTransportations();
            mySQL.LoadHighwaysBeats();
            /* Base Data Loads, used by the tablets: Deprecated.  We're not using these values anymore.
            mySQL.LoadCode1098s();
            mySQL.LoadFreeways();
            mySQL.LoadServiceTypes();
            mySQL.LoadTowLocations();
            mySQL.LoadTrafficSpeeds();
            mySQL.LoadContractors();  */
            mySQL.LoadBeatsFreeways();
            mySQL.LoadLeeways();
            mySQL.LoadBeatSchedules();
            mySQL.LoadCallSigns();
            mySQL.LoadSchedules();
            mySQL.LoadFreeways();
            //this is the new way to load polygon data, comment if necessary for production
            /*
            mySQL.loadBeatData();
            mySQL.loadBeatSegmentData();
            mySQL.loadYardData();
            mySQL.loadDropSiteData();
             */
            /* DEPRECATED - We're removing Esri, replaced with db data, uncomment if necessary for production*/
            BeatData.EsriBeats.LoadBeats();
            BeatData.EsriBeats.LoadBeatData();
            BeatData.EsriSegments.LoadSegments();
            BeatData.EsriYards.LoadYards();
            BeatData.EsriDropSites.LoadDropSites();

            DataClasses.BulkLogger myBulkLogger = new DataClasses.BulkLogger();
            DataClasses.TruckDumper myDumper = new DataClasses.TruckDumper();
            DataClasses.SurveyControlChecker check = new DataClasses.SurveyControlChecker(); //loop the running surveys and kill any that should be stopped
            MiscData.DataDumper dump = new MiscData.DataDumper(); //dump the playback data every 30 seconds

            /* ****************
             * CAD INTEGRATORS
             * ***************/
            cListener = new CADIntegration.CADListener();
            cSender = new CADIntegration.CADSender();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //This section allows clients other than IE to make sure it's OK to do cross domain scripting.  IE doesn't care and will
            //attemp cross-domain scripting without needing this pre-header information.
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
                              "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
                              "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age",
                              "1728000");
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logging.EventLogger myLogger = new Logging.EventLogger();
            myLogger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "FSP Service Has Stopped", false);
            cSender.Disconnect();
            cListener.DisconnectListener();
            myServer.disconnectUDP();
        }
    }
}