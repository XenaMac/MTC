using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Configuration;
using System.Web.Script.Serialization;
using System.ServiceModel;
//using Newtonsoft.Json;

namespace FPSService.DataClasses
{
    //This sends current truck data to a secondary server or servers for demo and testing purposes
    public class TruckDumper
    {
        Logging.MessageLogger logger = new Logging.MessageLogger("TowTruckDumper.txt");
        Logging.EventLogger evtLog;
        Timer dumpTimer;

        public TruckDumper()
        {
            string forward = ConfigurationManager.AppSettings["forward"].ToString();
            if (forward.ToUpper() != "TRUE")
            {
                return;
            }
            else
            {
                dumpTimer = new Timer(30000);
                dumpTimer.Elapsed += new ElapsedEventHandler(dumpTimer_Elapsed);
                dumpTimer.Start();
            }
        }

        void dumpTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                /*
                if (DataClasses.GlobalData.currentTrucks.Count > 0)
                {
                    Guid _beatID = new Guid("00000000-0000-0000-0000-000000000000");
                    string[] srvListString = ConfigurationManager.AppSettings["OtherServers"].ToString().Split('|');
                    //List<TowTruck.TowTruck> allTrucks = new List<TowTruck.TowTruck>();
                    List<srSecondaryService.CopyTruck> ctList = new List<srSecondaryService.CopyTruck>();
                    foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
                    {
                        if (t.assignedBeat.BeatID != null && t.assignedBeat.BeatID != new Guid("00000000-0000-0000-0000-000000000000"))
                        {
                            _beatID = t.GPSPosition.BeatID;
                        }

                        TowTruck.AlarmTimer alarmSpd = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                        TowTruck.AlarmTimer alarmOB = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });
                        TowTruck.AlarmTimer alarmOP = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOP) { return findOP.alarmName == "OnPatrol"; });
                        TowTruck.AlarmTimer alarmInc = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findInc) { return findInc.alarmName == "Incident"; });
                        TowTruck.AlarmTimer alarmSta = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSta) { return findSta.alarmName == "OnPatrol"; });
                        TowTruck.AlarmTimer alarmGPS = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findGPS) { return findGPS.alarmName == "GPSIssue"; });


                        bool _HasAlarms = false;
                        t.tta.checkAlarms();
                        if (t.tta.hasAlarms)
                        {
                            _HasAlarms = true;
                        }

                        #region " Speeding "

                        bool speedingAlarm = false;
                        string speedingValue = "NA";
                        DateTime speedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                        if (alarmSpd != null)
                        {
                            speedingAlarm = alarmSpd.hasAlarm;
                            speedingValue = alarmSpd.alarmValue;
                            speedingTime = alarmSpd.alarmStart;
                        }

                        #endregion

                        #region " Off Beat "

                        bool obAlarm = false;
                        string obMessage = "NA";
                        DateTime obTime = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime obCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime obExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        if (alarmOB != null)
                        {
                            obAlarm = alarmOB.hasAlarm;
                            obMessage = alarmOB.alarmValue;
                            obTime = alarmOB.alarmStart;
                            obExcused = alarmOB.alarmExcused;
                            obCleared = alarmOB.alarmExcused;
                        }

                        #endregion

                        #region " On Patrol "

                        bool opAlarm = false;
                        DateTime opTime = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime opCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime opExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        string opComments = "NA";

                        if (alarmOP != null)
                        {
                            opAlarm = alarmOP.hasAlarm;
                            opTime = alarmOP.GetStartDateTime();
                            opCleared = alarmOP.alarmCleared;
                            opExcused = alarmOP.alarmExcused;
                            opComments = alarmOP.alarmValue;
                        }

                        #endregion

                        #region " Incident "

                        bool incAlarm = false;
                        DateTime incTime = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime incCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime incExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        string incComments = "NA";

                        if (alarmInc != null)
                        {
                            incAlarm = alarmInc.hasAlarm;
                            incTime = alarmInc.GetStartDateTime();
                            incCleared = alarmInc.alarmCleared;
                            incExcused = alarmInc.alarmExcused;
                            incComments = alarmInc.alarmValue;
                        }

                        #endregion

                        #region " GPS "

                        bool gpsAlarm = false;
                        DateTime gpsTime = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime gpsCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        DateTime gpsExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        string gpsComments = "NA";

                        if (alarmGPS != null)
                        {
                            gpsAlarm = alarmGPS.hasAlarm;
                            gpsTime = alarmGPS.GetStartDateTime();
                            gpsCleared = alarmGPS.alarmCleared;
                            gpsExcused = alarmGPS.alarmExcused;
                            gpsComments = alarmGPS.alarmValue;
                        }

                        #endregion

                        #region " Stationary "

                        bool staAlarm = false;
                        DateTime staTime = alarmSta.GetStartDateTime();
                        DateTime staCleared = alarmSta.alarmCleared;
                        DateTime staExcused = alarmSta.alarmExcused;
                        string staComments = alarmSta.alarmValue;

                        #endregion
                        
                    }

                    for (int i = 0; i < srvListString.Count(); i++)
                    {
                        srSecondaryService.TowTruckServiceClient sr = new srSecondaryService.TowTruckServiceClient();
                        sr.Endpoint.Address = new EndpointAddress(new Uri("http://" + srvListString[i].ToString() + ":9007/TowTruckService.svc"));
                        foreach (srSecondaryService.CopyTruck t in ctList)
                        {
                            sr.SingleTruckDump(t);
                        }
                        //sr.TruckDump(ctList.ToArray());
                    }
                        
                }*/
            }
            catch(Exception ex)
            {
                evtLog = new Logging.EventLogger();
                evtLog.LogEvent("Error dumping trucks to secondary service" + Environment.NewLine + ex.ToString(), true);
                logger.writeToLogFile(DateTime.Now.ToString() + Environment.NewLine + "Error dumping trucks to secondary service" + Environment.NewLine + ex.ToString() + Environment.NewLine +
                    Environment.NewLine);
            }
        }
    }
}