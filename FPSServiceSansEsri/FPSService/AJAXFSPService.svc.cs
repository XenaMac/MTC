using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Security;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;

namespace FPSService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AJAXFSPService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        Logging.EventLogger logger = new Logging.EventLogger();

        #region " Truck status, state, position, and list data "

        [OperationContract]
        [WebGet]
        public void KillTruck(string ip)
        {
            DataClasses.GlobalData.RemoveTowTruck(ip);
        }

        [OperationContract]
        [WebGet]
        public string GetHighwaysByBeat(string beatNumber)
        {
            List<string> highways = new List<string>();
            var hList = from h in DataClasses.GlobalData.highwaysBeats
                        where h.BeatNumber == beatNumber
                        select h;
            foreach (MiscData.HighwaysBeats h in hList)
            {
                highways.Add(h.HighwayNumber);
            }

            highways = highways.OrderBy(s => s).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(highways);
        }

        [OperationContract]
        [WebGet]
        public List<MiscData.TrucksDrivers> GetDrivers()
        {
            List<MiscData.TrucksDrivers> truckDrivers = new List<MiscData.TrucksDrivers>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                truckDrivers.Add(new MiscData.TrucksDrivers
                {
                    TruckID = thisTruck.Extended.FleetVehicleID,
                    TruckNumber = thisTruck.TruckNumber,
                    DriverID = thisTruck.Driver.DriverID,
                    DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName,
                    ContractorName = thisTruck.Extended.ContractorName
                });
            }
            return truckDrivers;
        }

        [OperationContract]
        [WebGet]
        public void PostEarlyRollInReason(string _reason, string _dt, string _log, string _type)
        {
            //OCTA CODE - NOT NUSED IN MTC
            //this was originally specced to handle only early roll ins and was later adapted to handle mistimed events in general
            //we support late log on, roll out, on patrol, early roll in, and early log off.  All functionality works essentially the
            //same and gets logged into the EarlyRollIns table in the fsp database.
            /* OLD OCTA CODE - NOT USED IN MTC
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck;
            thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                //find the driver id
                DateTime dt = Convert.ToDateTime(_dt);
                Guid DriverID = thisTruck.Driver.DriverID;
                Guid BeatID = thisTruck.assignedBeat.BeatID;
                string vehicleID = thisTruck.TruckNumber;
                if (string.IsNullOrEmpty(vehicleID))
                {
                    vehicleID = "BAD";
                }
                if (DriverID != new Guid("00000000-0000-0000-0000-000000000000") && BeatID != new Guid("00000000-0000-0000-0000-000000000000")
                    && !string.IsNullOrEmpty(vehicleID))
                {
                    SQL.SQLCode mySQL = new SQL.SQLCode();
                    mySQL.LogEarlyRollIn(DriverID, _reason, BeatID, vehicleID, dt, _log, _type);
                    thisTruck.earlyRollin = _reason;
                }
            }*/
        }

        [OperationContract]
        [WebGet]
        public string MakeSurveyNum()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            string TruckNumber;
            TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruck(ip);
            if (t == null)
            {
                return "INVALID TRUCK";
            }
            else
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                return mySQL.GetSurveyNum(t.beatNumber);
            }
        }

        private string MakeMsgID()
        {
            DateTime dtSeventy = Convert.ToDateTime("01/01/1970 00:00:00");
            TimeSpan tsSpan = DateTime.Now - dtSeventy;
            double ID = tsSpan.TotalMilliseconds;
            Int64 id = Convert.ToInt64(ID);
            return id.ToString();
        }

        [OperationContract]
        [WebGet]
        public void SendMessage(string IPAddr)
        {
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(IPAddr);
            string msg = "<Reboot><Id=" + MakeMsgID() + "></Id></Reboot>";
            if (thisTruck != null)
            {
                thisTruck.SendMessage(msg);
                KillTruck(IPAddr);
            }
        }

        [OperationContract]
        [WebGet]
        public string GetDate()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(DateTime.Now);
            //return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        private string makeCADDate(DateTime dt)
        {
            string cadDate = ".DT/";

            string month = dt.Month.ToString();
            string year = dt.Year.ToString();
            string day = dt.Day.ToString();
            while (day.Length < 2)
            {
                day = "0" + day;
            }
            while (month.Length < 2)
            {
                month = "0" + month;
            }

            year = year.Substring(2, 2);

            cadDate += month + day + year;

            return cadDate;
        }

        private string makeCADTime(DateTime dt)
        {
            string cadTime = ".TM/";

            string hour = dt.Hour.ToString();
            string minute = dt.Minute.ToString();
            string second = dt.Second.ToString();

            while (hour.Length < 2)
            {
                hour = "0" + hour;
            }
            while (minute.Length < 2)
            {
                minute = "0" + minute;
            }
            while (second.Length < 2)
            {
                second = "0" + second;
            }

            cadTime += hour + minute + second;

            return cadTime;
        }

        [OperationContract]
        [WebGet]
        public string SetTruckStatus(string Status)
        {
            try
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                string ip = endpoint.Address;
                //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
                if (ip == "::1")
                { ip = "127.0.0.1"; }
                TowTruck.TowTruck thisTruck;
                thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
                bool lateStatusChange = false;
                if (thisTruck == null)
                { return "Couldn't find truck"; }

                else
                {
                    string CurrentStatus = thisTruck.Status.VehicleStatus;
                    if (CurrentStatus != Status)
                    {
                        thisTruck.Status.StatusStarted = DateTime.Now;
                        thisTruck.tts.stopStatus(CurrentStatus, thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                            thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                        thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisTruck.Driver.schedule.scheduleID);
                        
                        thisTruck.tts.startStatus(Status, thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                            thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                        thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisTruck.Driver.schedule.scheduleID);
                    }
                    if (Status.Contains("Off") && Status != "Off Patrol")
                    {
                        thisTruck.Status.VehicleStatus = "On Patrol";
                        //thisTruck.Driver.BreakStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                    }
                    else
                    {
                        if (CurrentStatus == "On Lunch")
                        {
                            TimeSpan ts = DateTime.Now - thisTruck.Driver.LunchStarted;
                            int LunchTime = Convert.ToInt32(ts.TotalMinutes);
                            //thisTruck.Driver.LunchStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                            //SQL.SQLCode mySQL = new SQL.SQLCode();
                            mySQL.SetBreakTime(thisTruck.Driver.DriverID, "Lunch", LunchTime);
                            

                        }
                        if (CurrentStatus == "On Break")
                        {
                            TimeSpan ts = DateTime.Now - thisTruck.Driver.BreakStarted;
                            int BreakTime = Convert.ToInt32(ts.TotalMinutes);
                            //thisTruck.Driver.BreakStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                            //SQL.SQLCode mySQL = new SQL.SQLCode();
                            mySQL.SetBreakTime(thisTruck.Driver.DriverID, "Break", BreakTime);
                            
                        }
                        thisTruck.Status.VehicleStatus = Status;
                    }
                    if (Status.ToUpper() == "ON BREAK")
                    {
                        thisTruck.Driver.BreakStarted = DateTime.Now;
                        //send CAD 10-7: unavailable
                        string cadMSG = "US."; //set update status command
                        cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                        cadMSG += ".S/107" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                        Global.cSender.sendMessage(cadMSG);
                    }
                    if (Status.ToUpper() == "ON LUNCH")
                    {
                        thisTruck.Driver.LunchStarted = DateTime.Now;
                        //send CAD 10-7: unavailable
                        string cadMSG = "US."; //set update status command
                        cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                        cadMSG += ".S/107" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                        Global.cSender.sendMessage(cadMSG);
                    }
                    if (Status == "Roll In OK")
                    {
                        thisTruck.Status.VehicleStatus = "Roll In";
                        Status = "Roll In";
                        
                    }

                    if (Status.ToUpper() == "ENROUTE")
                    {
                        //need to let CAD know
                        string cadMSG = "US.";
                        cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                        cadMSG += ".S/ENRT";
                        /*
                        if (thisTruck.incidentID != "0")
                        {
                            cadMSG += ".II/" + thisTruck.incidentID + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now); ;
                        }
                         * */
                        Global.cSender.sendMessage(cadMSG);
                    }
                    if (Status == "ON PATROL")
                    {
                        if (thisTruck.wentOnPatrol == false)
                        {
                            //check the times
                            if (!string.IsNullOrEmpty(thisTruck.Driver.schedule.ScheduleName))
                            {
                                DateTime scheduleStart = thisTruck.Driver.schedule.start.AddMinutes(DataClasses.GlobalData.OnPatrollLeeway);
                                string shiftType = thisTruck.Driver.schedule.ScheduleName;
                                DateTime now = DateTime.Now;
                                if (now > scheduleStart)
                                {
                                    //got a misfired logon, log it and then stop the alarm.
                                    thisTruck.tta.startAlarm("LateOnPatrol", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                                        thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon,
                                        thisTruck.runID, thisTruck.location, thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head);
                                    TowTruck.AlarmTimer alarm = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer alFind) { return alFind.alarmName == "LateOnPatrol"; });
                                    alarm.hasAlarm = true;
                                    alarm.alarmValue = DateTime.Now.ToString() + "|" + thisTruck.Driver.schedule.start;
                                    alarm.comment = DateTime.Now.ToString() + "|" + thisTruck.Driver.schedule.start;
                                    thisTruck.tta.stopAlarm("LateOnPatrol", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                                        thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon,
                                        thisTruck.runID, thisTruck.location, thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head,
                                        thisTruck.Driver.callSign, thisTruck.Driver.schedule.scheduleID, thisTruck.Driver.schedule.ScheduleType);
                                    alarm.hasAlarm = true;//on patrol alarms are single-shot and when we stop the alarm it resets the alarmstatus to false, needs to be kept true
                                }
                                //Tell the CAD System what's up
                                //send CAD 10-8: available
                                //since the truck hasn't been on patrol yet, send an addunit message to the CAD
                                string cadMSG = "AU.";
                                cadMSG += thisTruck.shiftType + "." + thisTruck.Driver.callSign;
                                Global.cSender.sendMessage(cadMSG);
                                cadMSG = "US."; //set update status command
                                cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                                cadMSG += ".S/108" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                                Global.cSender.sendMessage(cadMSG);
                                #region " OLD CAD MESSAGE CODE "
                                /*
                                string cDate = DateTime.Now.ToShortDateString();
                                DateTime morning = Convert.ToDateTime(cDate + " 09:59:00");
                                DateTime mid = Convert.ToDateTime(cDate + " 14:29:00");
                                string cadAU = "AU.";
                                if (thisTruck.Driver.schedule.start <= morning)
                                {
                                    //morning shift
                                    shiftType = "AM";
                                }
                                else if (thisTruck.Driver.schedule.start > morning && thisTruck.Driver.schedule.start <= mid)
                                {
                                    shiftType = "MID";
                                }
                                else if (thisTruck.Driver.schedule.start > mid)
                                {
                                    shiftType = "PM";
                                }
                                string CadMsg = "US.";
                                if (shiftType == "AM")
                                {
                                    CadMsg += "A";
                                    cadAU += "A.";
                                }
                                else if (shiftType == "MID")
                                {
                                    CadMsg += "*";
                                    cadAU += "*.";
                                }
                                else if (shiftType == "PM")
                                {
                                    CadMsg += "B"; //FIX THIS
                                    cadAU += "B.";
                                }
                                string CallSign = thisTruck.Driver.callSign;

                                CadMsg += CallSign;
                                cadAU += CallSign;
                                CadMsg += ".S/108";
                                CadMsg += makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                                Global.cSender.sendMessage(cadAU); //Add the unit to the Cad System
                                Global.cSender.sendMessage(CadMsg); //Mark the unit status as On Patrol
                                 */
                                #endregion
                            }
                        }
                        else //need to tell the CAD the truck is back on patrol but not check for an alarm state at this point.
                        {
                            string cadMSG = "US."; //set update status command
                            cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                            cadMSG += ".S/108" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                            Global.cSender.sendMessage(cadMSG);
                        }
                        thisTruck.wentOnPatrol = true;
                    }
                    if (Status == "ROLL IN")
                    {
                        if (thisTruck.rolledIn == false)
                        {
                            if (!string.IsNullOrEmpty(thisTruck.Driver.schedule.ScheduleName))
                            {
                                DateTime dtShiftEnd = thisTruck.Driver.schedule.end.AddMinutes(DataClasses.GlobalData.RollInLeeway * -1);
                                if (DateTime.Now < dtShiftEnd)
                                {
                                    //got an early roll in.
                                    thisTruck.tta.startAlarm("EarlyOutOfService", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                                       thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon,
                                       thisTruck.runID, thisTruck.location, thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head);
                                    TowTruck.AlarmTimer alarm = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer alFind) { return alFind.alarmName == "EarlyOutOfService"; });
                                    alarm.hasAlarm = true;
                                    alarm.alarmValue = DateTime.Now.ToString() + "|" + dtShiftEnd.ToString();
                                    alarm.comment = DateTime.Now.ToString() + "|" + dtShiftEnd.ToString();
                                    thisTruck.tta.stopAlarm("EarlyOutOfService", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                                        thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon,
                                        thisTruck.runID, thisTruck.location, thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head,
                                        thisTruck.Driver.callSign, thisTruck.Driver.schedule.scheduleID, thisTruck.Driver.schedule.ScheduleType);
                                    alarm.hasAlarm = true; //roll in alarms are single-shot and when we stop the alarm it resets the alarmstatus to false, needs to be kept true
                                }
                                
                            }
                        } //end if

                        //mark truck unavailable (107)
                        //string cadMSG = "US."; //set update status command
                        //cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                        //cadMSG += ".S/107" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                        //Global.cSender.sendMessage(cadMSG);
                        //delete the truck from the CAD
                        string cadMSG = "DU.";
                        cadMSG += thisTruck.shiftType + "." + thisTruck.Driver.callSign;
                        Global.cSender.sendMessage(cadMSG);


                        thisTruck.rolledIn = true;
                    }
                    if (Status == "ON INCIDENT" && CurrentStatus != "ON INCIDENT") //we're firing two messages to the CAD, one for on incident, one for post assist
                    {
                        //Check to see current incidentid status, if 0, request a new ID from the CAD by setting the truck status = 1097
                        if (thisTruck.incidentID == "0")
                        {
                            #region " Old CAD CODE "
                            /*
                            //check the shift type, it will impact the call sign
                            string cDate = DateTime.Now.ToShortDateString();
                            DateTime morning = Convert.ToDateTime(cDate + " 09:59:00");
                            DateTime mid = Convert.ToDateTime(cDate + " 14:29:00");
                            string shiftType = string.Empty;
                            string CallSign = thisTruck.Driver.callSign;
                            if (thisTruck.Driver.schedule.start <= morning)
                            {
                                //morning shift
                                shiftType = "AM";
                            }
                            else if (thisTruck.Driver.schedule.start > morning && thisTruck.Driver.schedule.start <= mid)
                            {
                                shiftType = "MID";
                            }
                            else if (thisTruck.Driver.schedule.start > mid)
                            {
                                shiftType = "PM";
                            }

                            
                            //construct the message

                            string CadMsg = "US.";
                            if (shiftType == "AM")
                            {
                                CadMsg += "A.";
                            }
                            else if (shiftType == "MID")
                            {
                                CadMsg += "*.";
                            }
                            else if (shiftType == "PM")
                            {
                                CadMsg += "B."; //FIX THIS
                            }
                            CadMsg += CallSign;
                            string dt = string.Empty;
                            string mo = DateTime.Now.Month.ToString();
                            string da = DateTime.Now.Day.ToString();
                            string yr = DateTime.Now.Year.ToString();

                            while(mo.Length < 2)
                            {
                                mo = "0" + mo;
                            }

                            while (da.Length < 2)
                            {
                                da = "0" + da;
                            }
                            yr = yr.Substring(2, 2);
                            dt = mo + da + yr;
                            string td = string.Empty;
                            string hour = DateTime.Now.Hour.ToString();
                            string min = DateTime.Now.Minute.ToString();
                            string sec = DateTime.Now.Second.ToString();
                            while (hour.Length < 2)
                            {
                                hour = "0" + hour;
                            }
                            while (min.Length < 2)
                            {
                                min = "0" + min;
                            }
                            while (sec.Length < 2)
                            {
                                sec = "0" + sec;
                            }
                            td = hour + min + sec;
                            //501932.031915.082544.015F00001\US.*625-238.S/108.DT/031915.TM/082544
                            CadMsg += ".S/1097.DT/" + dt + ".TM/" + td;
                            //send the message
                            Global.cSender.sendMessage(CadMsg);
                            */
                            #endregion
                            //string cadMSG = "US."; //set update status command
                            //cadMSG += thisTruck.shiftType + thisTruck.Driver.callSign; //set shift+callsign
                            MTCIncident i = new MTCIncident();
                            Guid g = Guid.NewGuid();
                            i.fromTruck = 1;
                            i.incidentComplete = false;
                            i.Acked = true;
                            i.Beat = thisTruck.beatNumber;
                            i.Canceled = false;
                            i.IPAddr = thisTruck.Identifier;
                            i.UserPosted = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                            i.TruckNumber = thisTruck.TruckNumber;
                            i.preAssist = new MTCPreAssistData();
                            i.assistList = new List<MTCAssist>();
                            i.DatePosted = DateTime.Now;
                            g = mySQL.PostMTCIncident(thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName, thisTruck.TruckNumber, thisTruck.beatNumber,
                                1, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon);
                            i.IncidentID = g;
                            thisTruck.currentIncidentID = g;
                            DataClasses.GlobalData.Incidents.Add(i);
                            string cadMSG = "EI.T/FSP.L/#$%" + getLatLon(thisTruck) + ".U/" + thisTruck.shiftType + thisTruck.Driver.callSign + ".S/1097";
                            Global.cSender.sendMessage(cadMSG);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(thisTruck.currentIncident.CHPLogNumber))
                            {
                                string cadMSG = "US." + thisTruck.shiftType + thisTruck.Driver.callSign + ".S/1097.II/" + thisTruck.currentIncident.CHPLogNumber + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                                //Global.cSender.sendMessage(cadMSG);
                            }
                            else
                            {
                                string cadMSG = "US." + thisTruck.shiftType + thisTruck.Driver.callSign + ".S/1097" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                                //Global.cSender.sendMessage(cadMSG);
                            }
                        }
                    }
                    DataClasses.GlobalData.UpdateTowTruck(ip, thisTruck);
                    if (thisTruck.Driver != null)
                    {
                        //SQL.SQLCode mySQL = new SQL.SQLCode();
                        mySQL.LogEvent(thisTruck.Driver.DriverID, Status);
                    }

                    return Status;
                }
            }
            catch(Exception ex)
            {
                string err = ex.ToString();
                return "NOK";
            }
        }

        private string getLatLon(TowTruck.TowTruck t)
        {
            string Lat = t.GPSPosition.Lat.ToString();
            if (t.GPSPosition.Lat >= 0)
            {
                Lat = "+" + Lat;
            }
            string Lon = t.GPSPosition.Lon.ToString();
            string Head = t.GPSPosition.Head.ToString();
            return Lat + ";" + Lon + ";" + Head;
        }

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle=WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        public string GetEsriTrucks2()
        {
            List<TowTruckData> myTrucks = new List<TowTruckData>();

            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                bool HasAlarms = false;
                string _DriverName = "";
                string _CallSign = "";
                if (thisTruck.Driver.LastName != "Not Logged On")
                {
                    _DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                    _CallSign = thisTruck.Driver.callSign;
                }
                else
                {
                    _DriverName = "Not Logged On";
                    _CallSign = "NA";
                }
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }
                myTrucks.Add(new TowTruckData
                {
                    TruckNumber = thisTruck.TruckNumber,
                    IPAddress = thisTruck.Identifier,
                    Heading = thisTruck.GPSPosition.Head,
                    Speed = thisTruck.GPSPosition.Speed,
                    Lat = thisTruck.GPSPosition.Lat,
                    Lon = thisTruck.GPSPosition.Lon,
                    Alarms = HasAlarms,
                    VehicleState = thisTruck.Status.VehicleStatus,
                    SpeedingAlarm = thisTruck.Status.SpeedingAlarm,
                    SpeedingValue = thisTruck.Status.SpeedingValue,
                    SpeedingTime = thisTruck.Status.SpeedingTime,
                    OutOfBoundsAlarm = thisTruck.Status.OutOfBoundsAlarm,
                    OutOfBoundsMessage = thisTruck.Status.OutOfBoundsMessage,
                    OutOfBoundsTime = thisTruck.Status.OutOfBoundsTime,
                    CallSign = _CallSign,
                    LastMessage = thisTruck.LastMessage.LastMessageReceived,
                    ContractorName = thisTruck.Extended.ContractorName,
                    DriverName = _DriverName,
                    BeatNumber = thisTruck.beatNumber,
                    Location = "coming soon",
                    StatusStarted = thisTruck.Status.StatusStarted,
                    Cell = thisTruck.GPSPosition.Cell
                });
            }
            myTrucks = myTrucks.OrderBy(x => x.TruckNumber).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string TruckData = js.Serialize(myTrucks);
            return TruckData;
        }

        [OperationContract]
        [WebGet]
        public string GetAllTrucks()
        {
            List<TowTruckData> myTrucks = new List<TowTruckData>();

            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                bool HasAlarms = false;
                string _DriverName = "";
                string _CallSign = "";
                if (thisTruck.Driver.LastName != "Not Logged On")
                {
                    _DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                    _CallSign = thisTruck.Driver.callSign;
                }
                else
                {
                    _DriverName = "Not Logged On";
                    _CallSign = "NA";
                }
                /*
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }
                 * */

                TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });

                thisTruck.tta.checkAlarms();
                if (thisTruck.tta.hasAlarms == true)
                {
                    HasAlarms = true;
                }

                bool speedingAlarm = false;
                string speedingValue = "NA";
                DateTime speedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmSpd != null)
                {
                    speedingAlarm = alarmSpd.hasAlarm;
                    speedingValue = alarmSpd.alarmValue;
                    speedingTime = alarmSpd.alarmStart;
                }

                bool obAlarm = false;
                string obMessage = "NA";
                DateTime obTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmOB != null)
                {
                    obAlarm = alarmOB.hasAlarm;
                    obMessage = alarmOB.alarmValue;
                    obTime = alarmOB.alarmStart;
                }

                myTrucks.Add(new TowTruckData
                {
                    TruckNumber = thisTruck.TruckNumber,
                    IPAddress = thisTruck.Identifier,
                    Heading = thisTruck.GPSPosition.Head,
                    Speed = thisTruck.GPSPosition.Speed,
                    Lat = thisTruck.GPSPosition.Lat,
                    Lon = thisTruck.GPSPosition.Lon,
                    Alarms = HasAlarms,
                    VehicleState = thisTruck.tts.currentStatus,
                    SpeedingAlarm = speedingAlarm,
                    SpeedingValue = speedingValue,
                    SpeedingTime = speedingTime,
                    OutOfBoundsAlarm = obAlarm,
                    OutOfBoundsMessage = obMessage,
                    OutOfBoundsTime = obTime,
                    LastMessage = thisTruck.LastMessage.LastMessageReceived,
                    ContractorName = thisTruck.Extended.ContractorName,
                    DriverName = _DriverName,
                    CallSign = _CallSign,
                    BeatNumber = thisTruck.beatNumber,
                    Location = thisTruck.location,
                    StatusStarted = thisTruck.tts.statusStarted,
                    Cell = thisTruck.GPSPosition.Cell,
                    IsBackup = thisTruck.Extended.isBackup
                });
            }
            myTrucks = myTrucks.OrderBy(x => x.TruckNumber).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string TruckData = js.Serialize(myTrucks);
            return TruckData;
        }

        [OperationContract]
        [WebGet]
        public string GetFreeways()
        {
            List<string> freeways = BeatData.Beats.freeways;
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(freeways);
        }

        [OperationContract]
        [WebGet]
        public string GetTruckData(string ipaddr)
        {
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ipaddr);
            if (thisTruck != null)
            {
                bool HasAlarms = false;

                TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });

                thisTruck.tta.checkAlarms();
                if (thisTruck.tta.hasAlarms == true)
                {
                    HasAlarms = true;
                }

                bool speedingAlarm = false;
                string speedingValue = "NA";
                DateTime speedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmSpd != null)
                {
                    speedingAlarm = alarmSpd.hasAlarm;
                    speedingValue = alarmSpd.alarmValue;
                    speedingTime = alarmSpd.alarmStart;
                }

                bool obAlarm = false;
                string obMessage = "NA";
                DateTime obTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmOB != null)
                {
                    obAlarm = alarmOB.hasAlarm;
                    obMessage = alarmOB.alarmValue;
                    obTime = alarmOB.alarmStart;
                }
                /*
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }
                 * */

                TowTruck.TowTruckDashboardState thisState = new TowTruck.TowTruckDashboardState();
                thisState.TruckNumber = thisTruck.TruckNumber;
                thisState.IPAddress = thisTruck.Identifier;
                thisState.Direction = thisTruck.GPSPosition.Head;
                thisState.Speed = thisTruck.GPSPosition.Speed;
                thisState.Lat = thisTruck.GPSPosition.Lat;
                thisState.Lon = thisTruck.GPSPosition.Lon;
                thisState.VehicleState = thisTruck.tts.currentStatus;
                thisState.Alarms = HasAlarms;
                thisState.SpeedingAlarm = speedingAlarm;
                thisState.SpeedingValue = speedingValue;
                thisState.SpeedingTime = speedingTime;
                thisState.OutOfBoundsAlarm = obAlarm;
                thisState.OutOfBoundsMessage = obMessage;
                thisState.OutOfBoundsTime = obTime;
                thisState.Heading = thisTruck.GPSPosition.Head;
                thisState.LastMessage = thisTruck.LastMessage.LastMessageReceived;
                thisState.ContractorName = thisTruck.Extended.ContractorName;
                thisState.BeatNumber = thisTruck.beatNumber;
                thisState.GPSRate = thisTruck.State.GpsRate;
                thisState.Log = thisTruck.State.Log;
                thisState.Version = thisTruck.State.Version;
                thisState.ServerIP = thisTruck.State.ServerIP;
                thisState.SFTPServerIP = thisTruck.State.SFTPServerIP;
                thisState.GPSStatus = thisTruck.GPSPosition.Status.ToString();
                thisState.GPSDOP = thisTruck.GPSPosition.DOP.ToString();
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(thisState);
            }
            else
            { return null; }
        }

        [OperationContract]
        [WebGet]
        public string GetTruckStatus()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                string status = thisTruck.Status.VehicleStatus;
                if (thisTruck.beatNumber == "NOBEAT")
                {
                    status = "NA";
                }
                TowTruck.ClientTruckStatus thisStatus = new TowTruck.ClientTruckStatus();
                thisStatus.TruckNumber = thisTruck.TruckNumber;
                thisStatus.Speed = thisTruck.GPSPosition.Speed;
                thisStatus.TruckStatus = thisTruck.Status.VehicleStatus;
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(thisStatus);
            }
            else
            {
                return null;
            }
        }

        [OperationContract]
        [WebGet]
        public string GetAllState()
        {
            List<TowTruck.DashboardState> AllState = new List<TowTruck.DashboardState>();
            lock (DataClasses.GlobalData.currentTrucks)
            {
                foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
                {
                    AllState.Add(new TowTruck.DashboardState
                    {
                        TruckNumber = thisTruck.TruckNumber,
                        IPAddress = thisTruck.Identifier,
                        GPSRate = thisTruck.State.GpsRate,
                        Log = thisTruck.State.Log,
                        Version = thisTruck.State.Version,
                        ServerIP = thisTruck.State.ServerIP,
                        SFTPServerIP = thisTruck.State.SFTPServerIP
                    });
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(AllState);
        }

        [OperationContract]
        [WebGet]
        public string GetBeatPartners(string _beat)
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            List<beatPartner> partners = new List<beatPartner>();
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        where t.beatNumber == _beat && t.Identifier != ip
                        select t;
            foreach (TowTruck.TowTruck t in tList)
            {
                beatPartner bp = new beatPartner();
                bp.truckNumber = t.TruckNumber;
                bp.lat = t.GPSPosition.Lat;
                bp.lon = t.GPSPosition.Lon;
                bp.heading = t.GPSPosition.Head;
                bp.status = t.Status.VehicleStatus;
                partners.Add(bp);
            }
            if (partners.Count > 0)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(partners);
            }
            else
            {
                return "NA";
            }
        }

        [OperationContract]
        [WebGet]
        public string GetTruckPosition()
        {
            string DataOut = "";
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                MiscData.TruckPosition thisPosition = new MiscData.TruckPosition();
                thisPosition.TruckNumber = thisTruck.TruckNumber;
                thisPosition.Lat = thisTruck.GPSPosition.Lat;
                thisPosition.Lon = thisTruck.GPSPosition.Lon;
                thisPosition.Direction = thisTruck.GPSPosition.Head;
                thisPosition.Speed = thisTruck.GPSPosition.Speed;
                thisPosition.TruckStatus = thisTruck.Status.VehicleStatus;
                JavaScriptSerializer js = new JavaScriptSerializer();
                DataOut = js.Serialize(thisPosition);
            }
            if (thisTruck == null) //can't find truck, force a tablet log off
            {
                MiscData.TruckPosition thisPosition = new MiscData.TruckPosition();
                thisPosition.TruckNumber = "BAD TRUCK";
                thisPosition.Lat = 0.0;
                thisPosition.Lon = 0.0;
                thisPosition.Direction = 0;
                thisPosition.Speed = 0.0;
                thisPosition.TruckStatus = "LOG OFF";
                JavaScriptSerializer js = new JavaScriptSerializer();
                DataOut = js.Serialize(thisPosition);
            }
            return DataOut;
        }

        #endregion

        #region " Force beat, segment, drop site, and yard reloads "

        [OperationContract]
        [WebGet]
        public void reloadBeatBeatSegments(string beatNumber, string beatSegments)
        {
            SQL.SQLCode sql = new SQL.SQLCode();
            List<string> beatSegs = new List<string>();
            string[] splitter = beatSegments.Split(',');
            for (int i = 0; i < splitter.Count(); i++)
            {
                beatSegs.Add(splitter[i]);
            }
            sql.updateBeatBeatSegments(beatNumber, beatSegs);
        }

        [OperationContract]
        [WebGet]
        public string getBeatNumbers()
        {
            SQL.SQLCode sql = new SQL.SQLCode();
            List<string> beatNumbers = sql.getBeatNumbers();
            if (beatNumbers.Count > 0)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(beatNumbers);
            }
            else
            {
                return "NO DATA";
            }
        }

        [OperationContract]
        [WebGet]
        public string getBeatSegmentsByBeat(string beatNumber)
        {
            SQL.SQLCode sql = new SQL.SQLCode();
            List<bbs> beatSegments = sql.getSegmentsByBeat(beatNumber);
            if (beatSegments.Count > 0)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(beatSegments);
            }
            else
            {
                return "NO DATA";
            }
        }

        [OperationContract]
        [WebGet]
        public void reloadBeats()
        {
            //BeatData.Beats.LoadBeats();
            BeatData.Beats.LoadBeatInfo();
        }

        [OperationContract]
        [WebGet]
        public void reloadBeatSegments()
        {
            BeatData.BeatSegments.LoadSegments();
        }

        [OperationContract]
        [WebGet]
        public void reloadYards()
        {
            BeatData.Yards.LoadYards();
        }

        [OperationContract]
        [WebGet]
        public void reloadDrops()
        {
            BeatData.DropSites.LoadDropSites();
        }

        #endregion

        #region  " MTC Assist Data "

        [OperationContract]
        [WebGet]
        public string getIncidentID()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck;
            thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                return thisTruck.incidentID.ToString();
            }
            else
            {
                return "0";
            }
        }

        [OperationContract]
        [WebGet] 
        public string postMTCPreAssist(string incidentID, string driverName, MTCPreAssistData data)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            string TruckNumber;
            TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruck(ip);
            if (t != null)
            {
                data.DriverFirstName = t.Driver.FirstName;
                data.DriverLastName = t.Driver.LastName;
                data.DriverID = t.Driver.DriverID;
                TruckNumber = t.TruckNumber;
                data.RunID = t.runID;
            }
            else
            {
                data.DriverFirstName = "NA";
                data.DriverLastName = "NA";
                data.DriverID = new Guid("00000000-0000-0000-0000-000000000000");
                TruckNumber = ip;
                data.RunID = new Guid("00000000-0000-0000-0000-000000000000");
            }
            Guid IncidentID = new Guid(incidentID);
            //incident is created when the truck enters the scene, check to see if we've got a current incidentid
            if (t.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                IncidentID = t.currentIncidentID;
            }
            MTCIncident incident = DataClasses.GlobalData.findIncidentByID(IncidentID);
            if (incident == null)
            {
                if (data.DriverFirstName != "NA" && data.DriverLastName != "NA")
                {
                    driverName = data.DriverLastName + ", " + data.DriverFirstName;
                }
                MTCIncident inc = new MTCIncident();
                IncidentID = mySQL.PostMTCIncident(driverName, TruckNumber, data.Beat, 1, data.Lat, data.Lon);
                t.currentIncidentID = IncidentID;
                inc.fromTruck = 1;
                inc.DatePosted = DateTime.Now;
                inc.IncidentID = IncidentID;
                inc.UserPosted = driverName;
                inc.Beat = data.Beat;
                inc.preAssist = data;
                inc.assistList = new List<MTCAssist>();
                inc.IPAddr = ip;
                inc.sentToTruck = true;
                inc.TruckNumber = TruckNumber;
                inc.Acked = true;
                DataClasses.GlobalData.Incidents.Add(inc);
                //mySQL.PostMTCIncident(driverName, TruckNumber, data.Beat, 1, t.GPSPosition.Lat, t.GPSPosition.Lon);
                mySQL.PostMTCPreAssist(IncidentID, data);
            }
            else
            {
                //check to see if CAD has sent us a CHP Incident Number
                if (t.incidentID != "0")
                {
                    data.CHPLogNumber = t.incidentID;
                }
                DataClasses.GlobalData.AddPreAssist(IncidentID, data);
                mySQL.PostMTCPreAssist(IncidentID, data);
            }
            incident = DataClasses.GlobalData.findIncidentByID(IncidentID);
            if (incident != null)
            {
                string cadMSG = "UI." + t.incidentID + ".I/N.T/" + getAccidentCode(incident.preAssist.CHPIncidentType) + ".D/FSP:" + t.shiftType + t.Driver.callSign + " " + incident.preAssist.CHPIncidentType + ".R/";
                Global.cSender.sendMessage(cadMSG);
            }
            return IncidentID.ToString();
        }

        [OperationContract]
        [WebGet]
        public string postMTCAssist(string incidentID, string driverName, MTCAssist data)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            string TruckNumber;
            TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruck(ip);
            if (t != null)
            {
                
                //build a message to tell the CAD we're done with the assist
                TruckNumber = t.TruckNumber;
                data.CallSign = t.Driver.callSign;
            }
            else 
            {
                TruckNumber = ip;
                data.CallSign = "Not Logged On";
            }

            //tell the CAD the assist is done.
            string cadMSG = string.Empty;

            Guid IncidentID = new Guid(incidentID);
            //incident is created when the truck enters the scene, check to see if we've got a current incidentid
            if (t.currentIncidentID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                IncidentID = t.currentIncidentID;
            }
            MTCIncident incident = DataClasses.GlobalData.findIncidentByID(IncidentID);
            if (incident == null)
            {
                MTCIncident inc = new MTCIncident();
                IncidentID = mySQL.PostMTCIncident(driverName, TruckNumber, "0", 1, data.Lat, data.Lon);
                inc.fromTruck = 1;
                inc.DatePosted = DateTime.Now;
                inc.IncidentID = IncidentID;
                inc.UserPosted = driverName;
                inc.assistList = new List<MTCAssist>();
                inc.assistList.Add(data);
                inc.IPAddr = ip;
                inc.sentToTruck = true;
                inc.Acked = true;
                inc.incidentComplete = true;
                inc.TruckNumber = TruckNumber;
                DataClasses.GlobalData.Incidents.Add(inc);
                //mySQL.PostMTCIncident(driverName, 1, data.Lat, data.Lon);
                mySQL.PostMTCAssist(IncidentID, data);
            }
            else
            {
                DataClasses.GlobalData.AddAssist(IncidentID, data);
                incident.incidentComplete = true;
                mySQL.PostMTCAssist(IncidentID, data);
            }

            if (incident == null)
            {
                incident = DataClasses.GlobalData.findIncidentByID(IncidentID);
            }

            //send update incident
            //thisTruck.shiftType + "." + thisTruck.Driver.callSign
            //cadMSG = "UI." + t.incidentID + ".I/N" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now) + "/FSP:" + t.shiftType + t.Driver.callSign + " " + incident.preAssist.CHPIncidentType + ".R/";
            cadMSG = "UI." + t.incidentID + ".I/N.T/" + getAccidentCode(incident.preAssist.CHPIncidentType) + ".D/FSP:" + t.shiftType + t.Driver.callSign + " " + incident.preAssist.CHPIncidentType + ".R/";
            
            Global.cSender.sendMessage(cadMSG);
            //send incident complete
            cadMSG = "US."; //set update status command
            cadMSG += t.shiftType + t.Driver.callSign; //set shift+callsign
            cadMSG += ".S/1098" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
            Global.cSender.sendMessage(cadMSG);
            t.incidentID = "0"; //incident is done, clear the CAD incident ID
            t.currentIncidentID = new Guid("00000000-0000-0000-0000-000000000000");
            t.setStatus("ON PATROL", false);
            //set the truck back on patrol and let the setstatus take care of notifying the CAD
            //t.setStatus("ON PATROL", false);

            #region  " OLD CAD CODE "
            /* This shouldn't happen yet, handle it through the status changes: On Patrol = 1098, On Incident = 1097
            //let the CAD know we're done
            //check the shift type, it will impact the call sign
            string cDate = DateTime.Now.ToShortDateString();
            DateTime morning = Convert.ToDateTime(cDate + " 09:59:00");
            DateTime mid = Convert.ToDateTime(cDate + " 14:29:00");
            string shiftType = string.Empty;
            string CallSign = t.Driver.callSign;
            if (t.Driver.schedule.start <= morning)
            {
                //morning shift
                shiftType = "AM";
            }
            else if (t.Driver.schedule.start > morning && t.Driver.schedule.start <= mid)
            {
                shiftType = "MID";
            }
            else if (t.Driver.schedule.start > mid)
            {
                shiftType = "PM";
            }


            //construct the message

            string CadMsg = "US.";
            if (shiftType == "AM")
            {
                CadMsg += "A.";
            }
            else if (shiftType == "MID")
            {
                CadMsg += "*.";
            }
            else if (shiftType == "PM")
            {
                CadMsg += "B."; //FIX THIS
            }
            CadMsg += CallSign;
            string dt = string.Empty;
            string mo = DateTime.Now.Month.ToString();
            string da = DateTime.Now.Day.ToString();
            string yr = DateTime.Now.Year.ToString();

            while (mo.Length < 2)
            {
                mo = "0" + mo;
            }

            while (da.Length < 2)
            {
                da = "0" + da;
            }
            yr = yr.Substring(2, 2);
            dt = mo + da + yr;
            string td = string.Empty;
            string hour = DateTime.Now.Hour.ToString();
            string min = DateTime.Now.Minute.ToString();
            string sec = DateTime.Now.Second.ToString();
            while (hour.Length < 2)
            {
                hour = "0" + hour;
            }
            while (min.Length < 2)
            {
                min = "0" + min;
            }
            while (sec.Length < 2)
            {
                sec = "0" + sec;
            }
            td = hour + min + sec;
            //\US.A622-087.S/1098.F/FSP
            CadMsg += ".S/1098.F/FSP";
            //send the message
            Global.cSender.sendMessage(CadMsg);
            */
            #endregion
            return IncidentID.ToString();
        }

        private string getAccidentCode(string CHPIncidentType)
        {
            string accidentCode = "0";
            switch (CHPIncidentType.ToUpper())
            {
                case "ACCIDENT-NO DETAILS":
                    accidentCode = "1183";
                    break;
                case "ACCIDENT-PROPERTY DAMAGE":
                    accidentCode = "1182";
                    break;
                case "ABANDONED VEHICLE":
                    accidentCode = "1124";
                    break;
                case "DISABLED VEHICLE-OCCUPIED":
                    accidentCode = "1126";
                    break;
                case "TRAFFIC HAZARD":
                    accidentCode = "1125";
                    break;
            }

            switch (CHPIncidentType)
            {
                case "Accident-Property Damage (11-82)":
                    accidentCode = "1182";
                    break;
                case "Disabled Vehicle-Occupied (11-26)":
                    accidentCode = "1126";
                    break;
                case "Traffic Hazard (11-25)":
                    accidentCode = "1125";
                    break;
                case "Abandoned Vehicle (11-24)":
                    accidentCode = "1124";
                    break;
                case "Accident-No Details (11-83)":
                    accidentCode = "1183";
                    break;
            }

            return accidentCode;
        }

        [OperationContract]
        [WebGet]
        public string findMyAssists()
        {
            OperationContext context = OperationContext.Current;
            TowTruck.TowTruck t = findTruck(context);
            if (t.currentIncident == null || t.currentIncident.requestSent == false || t.currentIncident.acked == true) {
                return null;
            }
            else {
                assistRequest ar = new assistRequest();
                t.currentIncident.sentToTruck = true;
                ar.IncidentID = (Guid)t.currentIncident.incidentID;
                ar.CHPLogNumber = t.currentIncident.CHPLogNumber;
                ar.direction = t.currentIncident.direction;
                ar.location = t.currentIncident.FSPLocation;
                ar.dispatchCode = t.currentIncident.chpIncidentType;
                ar.comments = t.currentIncident.comment;
                ar.crossStreet = t.currentIncident.crossStreet;
                ar.freeway = t.currentIncident.freeway;
                ar.LaneNumber = t.currentIncident.laneNumber;
                ar.CHPLogNumber = t.currentIncident.CHPLogNumber;
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(ar);
            }
            /* old model
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            assistRequest ar = new assistRequest();
            MTCIncident mi = DataClasses.GlobalData.Incidents.Find(i => i.IPAddr == ip && i.Acked == false);
            if (mi == null)
            {
                return null;
            }
            else
            {
                mi.sentToTruck = true;
                ar.direction = mi.preAssist.Direction;
                ar.location = mi.preAssist.FSPLocation;
                ar.IncidentID = mi.IncidentID;
                ar.dispatchCode = mi.preAssist.DispatchCode;
                ar.comments = mi.preAssist.Comment;
                ar.crossStreet = mi.preAssist.CrossStreet;
                ar.freeway = mi.preAssist.Freeway;
                ar.LaneNumber = mi.preAssist.LaneNumber;
                ar.CHPLogNumber = mi.preAssist.CHPLogNumber;
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(ar);
            }
            */
        }

        [OperationContract]
        [WebGet]
        public string cancelAssist(string incidentID, string reason = null)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode(); 
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            assistRequest ar = new assistRequest();
            Guid IncidentID = new Guid(incidentID);
            MTCIncident mi = DataClasses.GlobalData.Incidents.Find(i => i.IPAddr == ip && i.IncidentID == IncidentID);
            if (mi == null)
            {
                return null;
            }
            else
            {
                mi.Canceled = true;
                mi.Reason = reason;
                mySQL.CancelIncident(mi);
            }
            TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruck(ip);
            if (t != null)
            {
                //this resets the background truck information
                t.currentIncidentID = new Guid("00000000-0000-0000-0000-000000000000");
                t.incidentID = "0";
            }
            return IncidentID.ToString();
        }

        [OperationContract]
        [WebGet]
        public string findServiceLocation()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                return thisTruck.location;
            }
            else
            {
                return "NO TRUCK";
            }
        }

        [OperationContract]
        [WebGet]
        public void ackAssistRequest(string IncidentID)
        {
            Guid incidentID = new Guid(IncidentID);
            OperationContext context = OperationContext.Current;
            TowTruck.TowTruck t = findTruck(context);
            if (t != null) {
                if (t.currentIncident != null) {
                    t.currentIncident.acked = true;
                }
            }
            MTCIncident mi = DataClasses.GlobalData.Incidents.Find(i => i.IncidentID == incidentID);
            if (mi != null)
            {
                mi.Acked = true;
            }
        }

        #endregion

        #region " New Incident/Assist capabilities "

        #region " Incidents "

        [OperationContract]
        [WebGet]
        public string getCurrentIncidentID() {
            try {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck != null && thisTruck.currentIncident != null)
                {
                    return thisTruck.currentIncident.incidentID.ToString();
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        [OperationContract]
        [WebGet]
        public string createIncident(Incident i) {
            string ret = "OK";
            try {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null) {
                    return "NOTRUCK";
                }
                else if (thisTruck != null && thisTruck.currentIncident != null) { 
                    //close out the incident?
                    return thisTruck.currentIncident.incidentID.ToString();
                }
                else if (thisTruck != null && thisTruck.currentIncident == null) { 
                    //wowwee kazoowee. We can make this work
                    if (i.userPosted != "CHP CAD") {
                        i.fromTruck = true;
                    }
                    else {
                        i.fromTruck = false;
                    }
                    thisTruck.addIncident(i);
                    ret = thisTruck.currentIncident.incidentID.ToString();
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }

            return ret;
        }

        [OperationContract]
        [WebGet]
        public string updateIncident(Incident i) {
            string ret = "OK";

            try {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else if (thisTruck != null && thisTruck.currentIncident == null) {
                    return "NOINCIDENT";
                }
                else
                {
                    i.fromTruck = true;
                    thisTruck.updateIncident(i);
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }

            return ret;
        }

        [OperationContract]
        [WebGet]
        public string closeIncident(Incident i) {
            string ret = "OK";

            try
            {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else
                {
                    i.fromTruck = true;
                    thisTruck.closeIncident(i);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return ret;
        }

        [OperationContract]
        [WebGet]
        public string cancelIncident(Incident i) {
            try
            {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else
                {
                    i.fromTruck = true;
                    i.canceled = true;
                    if (i.incidentID != null || i.incidentID != Guid.Empty) { 
                        //get the current incident id
                        if (thisTruck.currentIncident != null)
                        {
                            if (thisTruck.currentAssists != null) {
                                for (int ia = thisTruck.currentAssists.Count - 1; ia >= 0; ia--) {
                                    thisTruck.closeAssist(thisTruck.currentAssists[ia]);
                                }
                                /*
                                    foreach (Assist a in thisTruck.currentAssists)
                                    {
                                        thisTruck.closeAssist(a);
                                    }
                                 * */
                            }
                            i.incidentID = thisTruck.currentIncident.incidentID;
                            thisTruck.closeIncident(i);
                            //return "OK";
                        }
                        else {
                            return "NOINCIDENT";
                        }
                    }
                    thisTruck.currentAssists = null;
                    thisTruck.currentIncident = null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "OK";
        }

        [OperationContract]
        [WebGet]
        public string getCurrentIncident() {
            try {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else {
                    if (thisTruck.currentIncident == null)
                    {
                        return "NOINCIDENT";
                    }
                    else {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        return js.Serialize(thisTruck.currentIncident);
                    }
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        #endregion

        #region " Assists "

        [OperationContract]
        [WebGet]
        public string createAssist(Assist a)
        {
            string ret = "OK";
            try
            {
                if (a.assistID == null || a.assistID == Guid.Empty) {
                    a.assistID = Guid.NewGuid();
                }
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else if (thisTruck != null && thisTruck.currentIncident != null)
                {
                    //this is another assist for this incident
                    Guid assistID = thisTruck.addAssist(a);
                    ret = assistID.ToString();
                }
                else if (thisTruck != null && thisTruck.currentIncident == null)
                {
                    //there must be an active incident in order to create an assist
                    ret = "NOINCIDENT";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return ret;
        }

        [OperationContract]
        [WebGet]
        public string closeAssist(Assist a)
        {
            string ret = "OK";

            try
            {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else
                {
                    thisTruck.closeAssist(a);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return ret;
        }

        [OperationContract]
        [WebGet]
        public string getCurrentAssist()
        {
            try
            {
                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else
                {
                    if (thisTruck.currentAssists == null || thisTruck.currentAssists.Count == 0)
                    {
                        return "NOASSIST";
                    }
                    else
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        return js.Serialize(thisTruck.currentAssists);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #endregion

        #region " Helpers "

        private TowTruck.TowTruck findTruck(OperationContext o)
        {
            try
            {
                OperationContext context = o;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                string ip = endpoint.Address;
                //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
                if (ip == "::1")
                { ip = "127.0.0.1"; }
                TowTruck.TowTruck thisTruck;
                thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
                if (thisTruck != null)
                {
                    return thisTruck;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region " Consumable Data "

        [OperationContract]
        [WebGet]
        public string getBeatsFreeways()
        {
            if (DataClasses.GlobalData.beatsFreeways.Count > 0)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(DataClasses.GlobalData.beatsFreeways);
            }
            else
            {
                return "0";
            }
        }

        [OperationContract]
        [WebGet]
        public string getDispatchCodes()
        {
            List<string> dispatchCodes = new List<string>();
            if (DataClasses.GlobalData.IncidentTypes.Count > 0)
            {
                foreach (MiscData.IncidentType i in DataClasses.GlobalData.IncidentTypes)
                {
                    dispatchCodes.Add(i.CHPIncidentType);
                }
            }
            else
            {
                dispatchCodes.Add("THERE IS A PROBLEM");
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(dispatchCodes);
        }

        [OperationContract]
        [WebGet]
        public string getBeatsFreewaysByBeat(string beatNumber)
        {
            if (DataClasses.GlobalData.beatsFreeways.Count > 0)
            {
                List<BeatData.BeatFreeway> beatsfreeways = new List<BeatData.BeatFreeway>();
                var bfList = from bf in DataClasses.GlobalData.beatsFreeways
                             where bf.BeatNumber == beatNumber
                             select bf;
                foreach (BeatData.BeatFreeway bf in bfList)
                {
                    beatsfreeways.Add(bf);
                }
                if (beatsfreeways.Count > 0)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    return js.Serialize(beatsfreeways);
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        [OperationContract]
        [WebGet]
        public void updateBeatsFreeways(string _BeatID, string _BeatDescription, string _Active, string _freeways)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            List<string> _freewayList = new List<string>();
            string[] splitter = _freeways.Split(',');
            for (int i = 0; i < splitter.Count(); i++)
            {
                _freewayList.Add(splitter[i].ToString());
            }
            
            Guid BeatID = new Guid(mySQL.findBeatID(_BeatID, _BeatDescription));
            bool Active = false;
            if (_Active == "1" || _Active.ToUpper() == "TRUE")
            {
                Active = true;
            }
            BeatData.BeatFreeway bf = new BeatData.BeatFreeway();
            bf.BeatID = BeatID;
            bf.BeatDescription = _BeatDescription;
            bf.BeatNumber = _BeatID;
            bf.Active = Active;
            bf.Freeways = _freewayList;
            DataClasses.GlobalData.UpdateBeatsFreeways(bf);
            mySQL.UpdateBeatFreeway(bf);
        }

        [OperationContract]
        [WebGet]
        public string getBeatData()
        {
            if (BeatData.Beats.beatData.Count > 0)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(BeatData.Beats.beatData);
            }
            else
            {
                return "";
            }
        }

        [OperationContract]
        [WebGet]
        public string getBeatDataByBeat(string BeatNumber)
        {
            List<beatData> beats = new List<beatData>();
            var bList = from b in BeatData.Beats.beatData
                        where b.BeatNumber == BeatNumber
                        select b;
            foreach (beatData b in bList)
            {
                beats.Add(b);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(beats);
        }

        #endregion

        #region " logon, logoff, breaktime, and assorted driver operations "

        [OperationContract]
        [WebGet]
        public string getScheduleNames(string beatNumber)
        {
            var sList = from s in DataClasses.GlobalData.theseSchedules
                        where s.BeatNumber == beatNumber
                        select s;
            List<string> schedules = new List<string>();
            foreach(MiscData.BeatSchedule bs in sList)
            {
                schedules.Add(bs.ScheduleName);    
            }
            schedules = schedules.OrderBy(s => s).ToList<string>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(schedules);
        }

        [OperationContract]
        [WebGet]
        public void LogOffDriver(string _DriverID)
        {
            Guid DriverID = new Guid(_DriverID);
            DataClasses.GlobalData.ForceDriverLogoff(DriverID);
        }

        [OperationContract]
        [WebGet]
        public string GetDriver()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck;
            thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck == null)
            { return "Couldn't find Driver"; }
            else
            {
                TowTruck.TowTruckDriver thisDriver = thisTruck.Driver;
                JavaScriptSerializer js = new JavaScriptSerializer();
                string driverInfo = js.Serialize(thisDriver);
                return driverInfo;
            }
        }

        [OperationContract]
        [WebGet]
        public void SetBeat(string AssignedBeat)
        {
            try
            {
                /* OLD OCTA CODE - NOT USED IN MTC
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                string ip = endpoint.Address;
                //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
                if (ip == "::1")
                { ip = "127.0.0.1"; }

                TowTruck.TowTruck thisTruck;
                thisTruck = DataClasses.GlobalData.FindTowTruck(ip);

                if (!string.IsNullOrEmpty(AssignedBeat))
                {
                    BeatData.Beat thisBeat = BeatData.Beats.GetDriverBeat(new Guid(AssignedBeat));
                    if (thisBeat != null)
                    {
                        thisTruck.assignedBeat.BeatID = thisBeat.BeatID;
                        thisTruck.assignedBeat.BeatNumber = thisBeat.BeatNumber;
                        thisTruck.assignedBeat.BeatExtent = thisBeat.BeatExtent;
                        thisTruck.assignedBeat.Loaded = true;
                    }
                    else
                    {
                        thisTruck.assignedBeat.BeatID = new Guid("00000000-0000-0000-0000-000000000000");
                        thisTruck.assignedBeat.Loaded = false;
                    }
                }*/
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToShortDateString() + Environment.NewLine + "Error switching beats" + Environment.NewLine + ex.ToString(), true);
            }
        }

        [OperationContract]
        [WebGet]
        public string DriverLogon(string FSPIDNumber, string Password, string AssignedBeat, string AssignedShift, string CallSign, string StartODO, string _forceLogOn = "F" )
        {



            //make sure to clear the alarms when the driver logs on.
            bool NewDriver = false;
            SQL.SQLCode mySQL = new SQL.SQLCode();
            string DataOut = "";
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            bool lateLogon = false;
            string shiftType = string.Empty;
            bool validDriver = mySQL.CheckLogon(ip, FSPIDNumber, Password);
            if (validDriver == true)
            {
                TowTruck.TowTruck thisTruck;
                thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
                if (thisTruck == null)
                {
                    validDriver = false;
                    DataOut = "ERROR You have not been logged on, your truck is not connected";
                }
                else
                {
                    //first check, if IPList is not empty log it as no driver
                    if (thisTruck.State != null && !string.IsNullOrEmpty(thisTruck.State.IPList))
                    {
                        //log it and clear it
                        mySQL.logDataUse(thisTruck);
                        thisTruck.State.IPList = string.Empty;
                    }
                    //Make sure driver only logs onto one truck at a time
                    int LogonCount = DataClasses.GlobalData.GetDriverLogonCount(FSPIDNumber, thisTruck.TruckNumber);
                    int BeatCoverCount = DataClasses.GlobalData.GetCoverCount(thisTruck.TruckNumber);
                    if (LogonCount > 0)
                    {
                        validDriver = false;
                        DataOut = "ERROR: You are currently logged on to another vehicle";
                    }
                    else if (BeatCoverCount > 0)
                    {
                        validDriver = false;
                        DataOut = "ERROR: You can only patrol one beat at a time";
                    }
                    else
                    {
                        TowTruck.TowTruckDriver thisDriver = mySQL.GetDriver(FSPIDNumber);
                        thisDriver.FSPID = FSPIDNumber;
                        thisDriver.startODO = Convert.ToDouble(StartODO);
                        thisDriver.callSign = CallSign;
                        thisDriver.AssignedShift = AssignedShift;
                        if (AssignedBeat != thisTruck.beatNumber)
                        {
                            //logged onto new beat, clear any alarms
                            ClearTruckAlarms(thisTruck);
                        }
                        DataClasses.GlobalData.AddCover(thisTruck.TruckNumber, AssignedBeat);
                        if (thisTruck.Driver == null || thisTruck.Driver.FSPID != thisDriver.FSPID)
                        {
                            thisTruck.Driver = thisDriver;
                            thisTruck.Driver.FSPID = FSPIDNumber;
                            NewDriver = true;
                        }
                        if (!string.IsNullOrEmpty(AssignedBeat))
                        {
                            
                            beatInformation eb = BeatData.Beats.beatInfos.Find(b => b.BeatID == AssignedBeat);
                            if (eb != null)
                            {
                                thisTruck.beatNumber = eb.BeatID;
                            }
                            //schedule checks
                            string shiftLookup = string.Empty;
                            string day = DateTime.Now.DayOfWeek.ToString();
                            switch (AssignedShift)
                            {
                                case "Weekday-Morning":
                                    shiftLookup = "WeekdayAM";
                                    break;
                                case "Weekday-Midday":
                                    shiftLookup = "WeekdayMid";
                                    break;
                                case "Weekday-Afternoon":
                                    shiftLookup = "WeekdayPM";
                                    break;
                                case "Weekend-Morning":
                                    if (day.ToUpper() == "SATURDAY")
                                    {
                                        shiftLookup = "SaturdayAM";
                                    }
                                    else
                                    {
                                        shiftLookup = "SundayAM";
                                    }
                                    break;
                                case "Weekend-Midday":
                                    if (day.ToUpper() == "SATURDAY")
                                    {
                                        shiftLookup = "SaturdayMid";
                                    }
                                    else
                                    {
                                        shiftLookup = "SundayMid";
                                    }
                                    break;
                                case "Weekend-Afternoon":
                                    if (day.ToUpper() == "SATURDAY")
                                    {
                                        shiftLookup = "SaturdayPM";
                                    }
                                    else
                                    {
                                        shiftLookup = "SundayPM";
                                    }
                                    break;
                                case "Holiday":
                                    shiftLookup = "Holiday";
                                    break;
                                default:
                                    shiftLookup = AssignedShift;
                                    break;
                            }
                            MiscData.BeatSchedule bs = DataClasses.GlobalData.findScheduleTimes(thisDriver.TowTruckCompany, AssignedBeat, shiftLookup);
                            if (!string.IsNullOrEmpty(bs.ScheduleName))
                            {
                                thisDriver.schedule = bs;
                                string cDate = DateTime.Now.ToShortDateString();
                                DateTime morning = Convert.ToDateTime(cDate + " 09:59:00");
                                DateTime mid = Convert.ToDateTime(cDate + " 14:29:00");
                                if (thisDriver.schedule.start <= morning)
                                {
                                    //morning shift
                                    shiftType = "AM";
                                }
                                else if (thisDriver.schedule.start > morning && thisDriver.schedule.start <= mid)
                                {
                                    shiftType = "MID";
                                }
                                else if (thisDriver.schedule.start > mid)
                                {
                                    shiftType = "PM";
                                }
                                
                            }
                            else
                            {
                                shiftType = "NA";
                                bs.ScheduleName = string.Empty;
                                bs.scheduleID = new Guid("00000000-0000-0000-0000-000000000000");
                            }
                            //if(thisDriver.schedule.start)
                        }
                        string CurrentStatus = thisTruck.Status.VehicleStatus;
                        thisTruck.runID = Guid.NewGuid();
                        thisTruck.tts.startStatus("LogOn", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                            thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                        thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisTruck.Driver.schedule.scheduleID);
                        if (CurrentStatus == "On Lunch" && NewDriver == false)
                        {
                            TimeSpan ts = DateTime.Now - thisTruck.Driver.LunchStarted;
                            int LunchTime = Convert.ToInt32(ts.TotalMinutes);
                            //thisTruck.Driver.LunchStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                            mySQL = new SQL.SQLCode();
                            mySQL.SetBreakTime(thisTruck.Driver.DriverID, "Lunch", LunchTime);
                        }
                        if (CurrentStatus == "On Break" && NewDriver == false)
                        {
                            TimeSpan ts = DateTime.Now - thisTruck.Driver.BreakStarted;
                            int BreakTime = Convert.ToInt32(ts.TotalMinutes);
                            //thisTruck.Driver.BreakStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                            mySQL = new SQL.SQLCode();
                            mySQL.SetBreakTime(thisTruck.Driver.DriverID, "Break", BreakTime);
                        }
                        thisTruck.Status.VehicleStatus = "Driver Logged On";
                        string msg = "<SetVar><Id>" + MakeMsgID() + "</Id><LoggedOn>T</LoggedOn></SetVar>";
                        thisTruck.SendMessage(msg);
                        //Add check to set used break or lunch time here.
                        //mySQL.LogEvent(thisDriver.DriverID, "Driver Log On");
                        Guid g = Guid.NewGuid();
                        mySQL.logStatus(g, "LOGON", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName, thisTruck.Extended.ContractorName, thisTruck.beatNumber,
                            DateTime.Now, DateTime.Now, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                        thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisDriver.schedule.scheduleID);
                        thisTruck.Status.StatusStarted = DateTime.Now;
                        DataOut = thisDriver.LastName + "|" + thisTruck.TruckNumber + "|" + thisDriver.DriverID + "|" +
                                thisTruck.Extended.FleetVehicleID + "|" + thisTruck.Extended.ContractorID + "|" + thisTruck.beatNumber;
                        //reset state
                        thisTruck.State.IPList = string.Empty;
                        //start mileage record
                        mySQL.logMileage(thisTruck.runID, thisDriver.startODO, thisDriver.startODO);

                        thisTruck.setShiftType(); //set the scheudle + callsign data for the driver
                        
                    }
                }
            }
            else
            {
                DataOut = "ERROR: Your logon credentials cannot be verified";
            }

            
            
            return DataOut;

        }

        [OperationContract]
        [WebGet]
        public string checkCallSigns(string beat, string scheduleName)
        {
            string signs = getCallSigns(beat, scheduleName);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(signs);
        }

        [OperationContract]
        [WebGet]
        public string getCallSigns(string beat, string scheduleName)
        {
            List<CallSignList> callSignList = new List<CallSignList>();
            var bList = from cs in DataClasses.GlobalData.CallSigns
                        where cs.Beat == beat && cs.ScheduleName == scheduleName
                        select cs;
            foreach (MiscData.callSign c in bList)
            {
                CallSignList csl = new CallSignList();
                csl.callSign = c.CallSign;
                csl.scheduleName = c.ScheduleName;
                callSignList.Add(csl);
            }

            var tList = from ts in DataClasses.GlobalData.currentTrucks
                        where ts.beatNumber == beat
                        select ts;
            foreach(TowTruck.TowTruck t in tList)
            {
                string drvrCall = t.Driver.callSign;
                string drvrSchedule = t.Driver.AssignedShift;
                for (int i = callSignList.Count - 1; i >= 0; i--)
                {
                    if (callSignList[i].callSign == drvrCall && callSignList[i].scheduleName == drvrSchedule)
                    {
                        callSignList.RemoveAt(i);
                    }
                }
                /*
                    foreach (string s in callSignList)
                    {
                        if (s == drvrCall)
                        {
                            callSignList.Remove(s);
                        }
                    }
                 * */
            }
            List<string> availableCallSigns = new List<string>();
            foreach (CallSignList csl in callSignList)
            {
                availableCallSigns.Add(csl.callSign);
            }
            availableCallSigns = availableCallSigns.OrderBy(s => s).ToList<string>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(availableCallSigns);
        }

        [OperationContract]
        [WebGet]
        public string DriverLogoff(string _ok = "check", string endODO = null)
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                bool logoffOK = true;
                SQL.SQLCode mySQL = new SQL.SQLCode();

                /*
                if (thisTruck.thisSchedule != null)
                {
                    DateTime AcceptedLogOffStart = thisTruck.thisSchedule.LogOff.AddMinutes(DataClasses.GlobalData.LogOffLeeway * -1);
                    DateTime AcceptedLogOffStop = thisTruck.thisSchedule.LogOff.AddMinutes(DataClasses.GlobalData.LogOffLeeway);
                    if (DateTime.Now < AcceptedLogOffStart || DateTime.Now > AcceptedLogOffStop)
                    {
                        thisTruck.Status.LogOffAlarm = true;
                        thisTruck.Status.LogOffAlarmTime = DateTime.Now;
                        mySQL.LogAlarm("LogOff", DateTime.Now, thisTruck.Driver.DriverID, thisTruck.Extended.FleetVehicleID, thisTruck.assignedBeat.BeatID);
                    }
                    if (DateTime.Now < AcceptedLogOffStart)
                    {
                        logoffOK = false;
                    }
                }
                */
                if (_ok == "force")
                {
                    logoffOK = true;
                }

                if (logoffOK == true)
                {
                    try
                    {
                        thisTruck.tts.startStatus("LogOff", thisTruck.TruckNumber, thisTruck.Driver.FirstName + " " + thisTruck.Driver.LastName,
                                thisTruck.Driver.TowTruckCompany, thisTruck.beatNumber, thisTruck.GPSPosition.Lat, thisTruck.GPSPosition.Lon, thisTruck.runID, thisTruck.location,
                            thisTruck.GPSPosition.Speed, thisTruck.GPSPosition.Head, thisTruck.Driver.schedule.scheduleID);
                    }
                    catch
                    {
                        
                    }
                    //end mileage record
                    mySQL.logMileage(thisTruck.runID, 0, Convert.ToDouble(endODO));
                    //log data use during run
                    mySQL.logDataUse(thisTruck);
                    //log log off event
                    mySQL.LogEvent(thisTruck.Driver.DriverID, "Driver Log Off");
                    DataClasses.GlobalData.RemoveCover(thisTruck.TruckNumber);
                    mySQL.closeOutTruck(thisTruck.runID);
                    thisTruck.Status.VehicleStatus = "Waiting for Driver Login";

                    thisTruck.Driver.DriverID = new Guid("00000000-0000-0000-0000-000000000000");
                    thisTruck.Driver.FSPID = "";
                    thisTruck.Driver.FirstName = "No Driver";
                    thisTruck.Driver.LastName = "No Driver";
                    thisTruck.Driver.TowTruckCompany = "No Driver";
                    thisTruck.Driver.callSign = "NA";
                    //thisTruck.Driver.BreakStarted = Convert.ToDateTime("01/01/2001 00:00:00");
                    /*
                    thisTruck.assignedBeat.BeatID = new Guid("00000000-0000-0000-0000-000000000000");
                    thisTruck.assignedBeat.BeatExtent = null;
                    thisTruck.assignedBeat.BeatNumber = "Not Assigned";
                    thisTruck.assignedBeat.Loaded = false;
                     * */
                    thisTruck.beatNumber = "NOBEAT";
                    thisTruck.Status.SpeedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    thisTruck.Status.OutOfBoundsTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    thisTruck.Status.SpeedingValue = "0.0";
                    thisTruck.Status.OutOfBoundsMessage = "Not logged on";
                    thisTruck.Status.OutOfBoundsAlarm = false;
                    thisTruck.Status.SpeedingAlarm = false;
                    thisTruck.Status.StatusStarted = DateTime.Now;
                    thisTruck.State.IPList = string.Empty;
                    //thisTruck.thisSchedule = null;
                    thisTruck.runID = new Guid("00000000-0000-0000-0000-000000000000");
                    //reset status and alarms for truck
                    thisTruck.tts = null;
                    thisTruck.tta = null;
                    thisTruck.tts = new TowTruck.TruckStatus();
                    thisTruck.tta = new TowTruck.TowTruckAlarms();
                    thisTruck.wentOnPatrol = false;
                    thisTruck.rolledIn = false;
                    thisTruck.Driver.schedule = null;
                    thisTruck.cadCallSign = "NA";
                    string msg = "<SetVar><Id>" + MakeMsgID() + "</Id><LoggedOn>F</LoggedOn></SetVar>";
                    thisTruck.SendMessage(msg);

                    return "OK";
                }
                else
                {
                    return "early";
                }
                
            }
            else
            {
                return "notruck";
            }
        }

        [OperationContract]
        [WebGet]
        public int GetBreakDuration(string DriverID)
        {
            Guid gDriverID = new Guid(DriverID);
            SQL.SQLCode mySQL = new SQL.SQLCode();
            int BreakDuration = mySQL.GetBreakDuration(DriverID);
            return BreakDuration;
            
        }

        [OperationContract]
        [WebGet]
        public int GetLunchDuration(string DriverID)
        {
            int LunchDuration = 30;
            return LunchDuration;
        }

        [OperationContract]
        [WebGet]
        public int FindUsedBreakTime(string DriverID, string Type)
        {
            //SQL.SQLCode mySQL = new SQL.SQLCode();
            //return mySQL.GetUsedBreakTime(DriverID, Type);
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            int duration = 0;
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                if (Type.ToUpper() == "BREAK")
                {
                    TowTruck.StatusTimer tts = thisTruck.tts.statusTimers.Find(delegate(TowTruck.StatusTimer find) { return find.statusName == "OnBreak"; });
                    if (tts != null)
                    {
                        //TimeSpan ts = DateTime.Now - tts.statusStart;
                        //duration = (int)ts.TotalMinutes;
                        duration = tts.statusMinutes;
                    }
                }
                if (Type.ToUpper() == "LUNCH")
                {
                    TowTruck.StatusTimer tts = thisTruck.tts.statusTimers.Find(delegate(TowTruck.StatusTimer find) { return find.statusName == "OnLunch"; });
                    if (tts != null)
                    {
                        duration = tts.statusMinutes;
                    }
                }
                
            }
            return duration;
        }

        //Deprecated, this was used for a count down method, FindUsedBreakTime pulls from database and is used to count upwards
        [OperationContract]
        [WebGet]
        public int GetUsedBreakTime(string DriverID)
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            int diffTime = 0;
            if (thisTruck != null)
            {
                DateTime breakStarted = thisTruck.Driver.BreakStarted;
                if (breakStarted == Convert.ToDateTime("01/01/2001 00:00:00"))
                {
                    breakStarted = DateTime.Now;
                    thisTruck.Driver.BreakStarted = breakStarted;
                }
                TimeSpan ts = DateTime.Now.Subtract(breakStarted);
                diffTime = Convert.ToInt32(ts.TotalMinutes);
                SQL.SQLCode mySQL = new SQL.SQLCode();
                int BreakDuration = mySQL.GetBreakDuration(DriverID);
                diffTime = BreakDuration - diffTime;
            }
            return diffTime;
        }

        //Deprecated, this was used for a count down method, FindUsedBreakTime pulls from database and is used to count upwards
        [OperationContract]
        [WebGet]
        public int GetUsedLunchTime(string DriverID)
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            int diffTime = 0;
            if (thisTruck != null)
            {
                DateTime lunchStarted = thisTruck.Driver.LunchStarted;
                if (lunchStarted == Convert.ToDateTime("01/01/2001 00:00:00"))
                {
                    lunchStarted = DateTime.Now;
                    thisTruck.Driver.LunchStarted = lunchStarted;
                }
                TimeSpan ts = DateTime.Now.Subtract(lunchStarted);
                diffTime = Convert.ToInt32(ts.TotalMinutes);
                //SQL.SQLCode mySQL = new SQL.SQLCode();
                int LunchDuration = 30;
                diffTime = LunchDuration - diffTime;
            }
            return diffTime;
        }

        private void ClearTruckAlarms(TowTruck.TowTruck t)
        {
            t.Status.LogOnAlarm = false;
            t.Status.LogOnAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOnAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOnAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOnAlarmComments = "NA";
            t.Status.RollOutAlarm = false;
            t.Status.RollInAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmComments = "NA";
            t.Status.OnPatrolAlarm = false;
            t.Status.OnPatrolAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.OnPatrolAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.OnPatrolAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.OnPatrolAlarmComments = "NA";
            t.Status.RollInAlarm = false;
            t.Status.RollInAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.RollInAlarmComments = "NA";
            t.Status.LogOffAlarm = false;
            t.Status.LogOffAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOffAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOffAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.LogOffAlarmComments = "NA";
            t.Status.GPSIssueAlarm = false;
            t.Status.GPSIssueAlarmStart = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.GPSIssueAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.GPSIssueAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.GPSIssueAlarmComments = "NA";
            t.Status.IncidentAlarm = false;
            t.Status.IncidentAlarmTime = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.IncidentAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.IncidentAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.IncidentAlarmComments = "NA";
            t.Status.StationaryAlarm = false;
            t.Status.StationaryAlarmStart = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.StationaryAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.StationaryAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            t.Status.StationaryAlarmComments = "NA";
        }

        #endregion

        #region " find preloaded data for client tablet "

        [OperationContract]
        [WebGet]
        public string GetBeats()
        {
            List<MiscData.ClientBeat> myBeats = new List<MiscData.ClientBeat>();
            myBeats.Add(new MiscData.ClientBeat { 
                BeatName = "0",
                BeatID = new Guid("00000000-0000-0000-0000-000000000000")
            });

            foreach (beatInformation bi in BeatData.Beats.beatInfos)
            {
                MiscData.ClientBeat thisClient = new MiscData.ClientBeat();
                thisClient.BeatID = bi.ID;
                thisClient.BeatName = bi.BeatName;
                if (bi.BeatID != "NO BEAT ID")
                {
                    myBeats.Add(thisClient);
                }
            }
            /*
            foreach (BeatData.Beat thisBeat in BeatData.Beats.AllBeats)
            {
                MiscData.ClientBeat thisClient = new MiscData.ClientBeat();
                thisClient.BeatID = thisBeat.BeatID;
                thisClient.BeatName = thisBeat.BeatNumber;
                myBeats.Add(thisClient);
            }*/
            myBeats = myBeats.OrderBy(x => x.BeatName).ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(myBeats);
        }

        [OperationContract]
        [WebGet]
        public string getDropsites(string BeatNumber)
        {
            List<string> drops = new List<string>();
            List<string> orderable = new List<string>();
            drops.Add("Select");
            drops.Add("Other");
            var dsList = from d in BeatData.DropSites.dropSites
                         where d.beatID == BeatNumber
                         select d;
            foreach (dropSitePolygonData ds in dsList)
            {
                orderable.Add(ds.dropSiteDescription);
            }
            orderable = orderable.OrderBy(s => s).ToList<string>();
            foreach (string s in orderable)
            {
                drops.Add(s);
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(drops);
        }

        [OperationContract]
        [WebGet]
        public string getDropPolygons(string BeatNumber) {
            try {
                List<dropSitePolygonData> drops = new List<dropSitePolygonData>();
                var dsList = from d in BeatData.DropSites.dropSites
                             where d.beatID == BeatNumber
                             select d;
                foreach (dropSitePolygonData ds in dsList) {
                    drops.Add(ds);
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(drops);
            }
            catch (Exception ex) {
                return "ERROR:" + ex.Message;
            }
        }
        
        [OperationContract]
        [WebGet]
        public string GetPreloadedData(string Type)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string JSONData = "";
            switch(Type)
            {
                
                case "IncidentTypes":
                    List<MiscData.IncidentType> IncidentTypes = new List<MiscData.IncidentType>();
                    foreach (MiscData.IncidentType thisIncident in DataClasses.GlobalData.IncidentTypes)
                    {
                        IncidentTypes.Add(thisIncident);
                    }
                    JSONData = js.Serialize(IncidentTypes);
                    break;
                case "LocationAbbreviations":
                    List<MiscData.LocationAbbreviation> LocationAbbreviations = new List<MiscData.LocationAbbreviation>();
                    foreach (MiscData.LocationAbbreviation thisCode in DataClasses.GlobalData.LocationAbbreviations)
                    {
                        LocationAbbreviations.Add(thisCode);
                    }
                    JSONData = js.Serialize(LocationAbbreviations);
                    break;
                case "Transportations":
                    List<MiscData.Transportation> Transportations = new List<MiscData.Transportation>();
                    foreach (MiscData.Transportation t in DataClasses.GlobalData.Transportations)
                    {
                        Transportations.Add(t);
                    }
                    JSONData = js.Serialize(Transportations);
                    break;
                    /*
                case "ServiceTypes":
                    List<DataClasses.ClientServiceType> ServiceTypes = new List<DataClasses.ClientServiceType>();
                    foreach (MiscData.ServiceType thisServiceType in DataClasses.GlobalData.ServiceTypes)
                    {
                        ServiceTypes.Add(new DataClasses.ClientServiceType { 
                            ServiceTypeID = thisServiceType.ServiceTypeID,
                            ServiceTypeCode = thisServiceType.ServiceTypeCode,
                            ServiceTypeName = thisServiceType.ServiceTypeName
                        });
                    }
                    JSONData = js.Serialize(ServiceTypes);
                    break;
                case "TowLocations":
                    List<DataClasses.ClientTowLocation> TowLocations = new List<DataClasses.ClientTowLocation>();
                    foreach (MiscData.TowLocation thisTowLocation in DataClasses.GlobalData.TowLocations)
                    {
                        TowLocations.Add(new DataClasses.ClientTowLocation { 
                            TowLocationID = thisTowLocation.TowLocationID,
                            TowLocationCode = thisTowLocation.TowLocationCode,
                            TowLocationName = thisTowLocation.TowLocationName
                        });
                    }
                    JSONData = js.Serialize(TowLocations);
                    break;
                case "TrafficSpeeds":
                    List<DataClasses.ClientTrafficSpeed> TrafficSpeeds = new List<DataClasses.ClientTrafficSpeed>();
                    foreach (MiscData.TrafficSpeed thisSpeed in DataClasses.GlobalData.TrafficSpeeds)
                    {
                        TrafficSpeeds.Add(new DataClasses.ClientTrafficSpeed { 
                            TrafficSpeedID = thisSpeed.TrafficSpeedID,
                            TrafficSpeedCode = thisSpeed.TrafficSpeedCode
                        });
                    }
                    JSONData = js.Serialize(TrafficSpeeds);
                    break;
                     * */
                case "VehiclePositions":
                    List<MiscData.VehiclePosition> VehiclePositions = new List<MiscData.VehiclePosition>();
                    foreach (MiscData.VehiclePosition thisPosition in DataClasses.GlobalData.VehiclePositions)
                    {
                        VehiclePositions.Add(thisPosition);
                    }
                    JSONData = js.Serialize(VehiclePositions);
                    break;
                case "VehicleTypes":
                    List<MiscData.VehicleType> VehicleTypes = new List<MiscData.VehicleType>();
                    foreach (MiscData.VehicleType thisType in DataClasses.GlobalData.VehicleTypes)
                    {
                        VehicleTypes.Add(thisType);
                    }
                    JSONData = js.Serialize(VehicleTypes);
                    break;
            }
            return JSONData;
        }
        #endregion

        #region " New poly info for trucks "

        [OperationContract]
        [WebGet]
        public string getFenceNames() {
            try {
                List<string> fences = new List<string>();
                foreach (beatPolygonData b in BeatData.Beats.beatList)
                {
                    fences.Add(b.BeatID);
                }
                return JsonConvert.SerializeObject(fences);
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        #endregion

        #region " Truck Messages (Not assist) "

        [OperationContract]
        [WebGet]
        public string GetTruckMessages()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;
            //temp fix, endpoint.Address is returning IPv6 loopback address when tested locally
            if (ip == "::1")
            { ip = "127.0.0.1"; }
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(ip);
            if (thisTruck != null)
            {
                List<TruckMessage> myMessages = DataClasses.GlobalData.GetMessagesByTruck(thisTruck.Identifier);
                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(myMessages);
            }
            else
            {
                return "";
            }
        }

        [OperationContract]
        [WebGet]
        public void AckMessage(string MessageID)
        {
            if (string.IsNullOrEmpty(MessageID))
            {
                return;
            }
            Guid msgID = new Guid(MessageID);
            DataClasses.GlobalData.AckTruckMessage(msgID);
        }

        [OperationContract]
        [WebGet]
        public void AckMessageWithResponse(string MessageID, string Response)
        {
            if (string.IsNullOrEmpty(MessageID))
            {
                return;
            }
            Guid msgID = new Guid(MessageID);
            DataClasses.GlobalData.AckTruckMessageWithResponse(msgID, Response);
        }

        [OperationContract]
        [WebGet]
        public string AckWazeMessage(string UUID, string Response, bool Accepted) {
            WAZE.wazeXML w = WAZE.GetWAZE.wazes.Find(delegate(WAZE.wazeXML find) { return find.uuid == UUID; });

            TruckMessage foundMessage = DataClasses.GlobalData.theseMessages.Find(delegate (TruckMessage myMessage) { return myMessage.WazeUUID == UUID; });

            if (w != null)
            {
                w.accepted = Accepted;
                w.acked = true;
                w.AckedTime = DateTime.Now;
                w.ackMessage = Response;
                SQL.SQLCode sql = new SQL.SQLCode();
                DataClasses.GlobalData.AckWazeMessage(UUID, Response);
                sql.logAckedWaze(w);
                DataClasses.GlobalData.AckTruckMessageWithResponse(foundMessage.MessageID, Response);
            }
            

            if (Accepted)
            {
                //make a new incident and add the Waze UUID to it
                Incident i = new Incident();

                OperationContext context = OperationContext.Current;
                TowTruck.TowTruck thisTruck = findTruck(context);
                if (thisTruck == null)
                {
                    return "NOTRUCK";
                }
                else if (thisTruck != null && thisTruck.currentIncident != null)
                {
                    //close out the incident?
                    return thisTruck.currentIncident.incidentID.ToString();
                }
                else if (thisTruck != null && thisTruck.currentIncident == null)
                {
                    i.userPosted = "WAZE";
                    i.fromTruck = true;
                    i.wazeID = w.uuid;
                    thisTruck.addIncident(i);
                    thisTruck.setStatus("ON INCIDENT", false);
                    return thisTruck.currentIncident.incidentID.ToString();
                }
            }
            else {
                w.accepted = Accepted;
                w.acked = true;
                w.AckedTime = DateTime.Now;
                w.ackMessage = Response;
                SQL.SQLCode sql = new SQL.SQLCode();
                DataClasses.GlobalData.AckWazeMessage(UUID, Response);
                sql.logAckedWaze(w);
                DataClasses.GlobalData.AckTruckMessageWithResponse(foundMessage.MessageID, Response);
                return "DECLINED";
            }
            return Accepted.ToString();
        }

        #endregion

        #region " Esri Token "

        /* Deprecated
        [OperationContract]
        [WebGet]
        public async Task<string> getToken() {
            var opts = new Esri.ArcGISRuntime.Security.IdentityManager.GenerateTokenOptions();
            opts.TokenAuthenticationType = Esri.ArcGISRuntime.Security.IdentityManager.TokenAuthenticationType.ArcGISToken;
            opts.Referer = new Uri("http://localhost/tablet");
            //restServicesURL = ConfigurationManager.AppSettings["RESTServicesURL"].ToString();
            IdentityManager.Credential cred = await IdentityManager.Current.GenerateCredentialAsync(
                ConfigurationManager.AppSettings["RESTServicesURL"].ToString(),
                "LATA_AGS",
                "T6axi28!6_20z6J",
                opts
                );
            return cred.Token;
        }
        */
        #endregion
    }

    public class EsriGeometry
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public SpatialReference spatialReference { get; set; }
    }

    public class SpatialReference
    {
        public int wkid { get; set; }
    }

    public class EsriTruck
    {
        public string TruckNumber { get; set; }
        public string CallSign { get; set; }
        public int Direction { get; set; }
        public double Speed { get; set; }
        public string VehicleState { get; set; }
        public string Alarms { get; set; }
        public string SpeedingAlarm { get; set; }
        public long SpeedingTime { get; set; }
        public string OutOfBoundsAlarms { get; set; }
        public string OutOfBoundsMessage { get; set; }
        public long OutOfBoundsTime { get; set; }
        public int Heading { get; set; }
        public string BeatNumber { get; set; }
        public string IPAddress { get; set; }
        public long LastMessage { get; set; }
        public string ContractorName { get; set; }
        public string DriverName { get; set; }
        public long StatusStarted { get; set; }
        public EsriGeometry Geometry { get; set; }
    }

    public class assistRequest
    {
        public Guid IncidentID { get; set; }
        public string direction { get; set; }
        public string location { get; set; }
        public string dispatchCode { get; set; }
        public string freeway { get; set; }
        public string comments { get; set; }
        public string crossStreet { get; set; }
        public string LaneNumber { get; set; }
        public string CHPLogNumber { get; set; }
    }

    public class beatPartner
    {
        public string truckNumber { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int heading { get; set; }
        public string status { get; set; }
    }

    public class CallSignList
    {
        public string callSign { get; set; }
        public string scheduleName { get; set; }
    }

    public class bbs
    {
        public string beatNumber { get; set; }
        public string beatSegmentNumber { get; set; }
        public string objectID { get; set; }
    }
}
