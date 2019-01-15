using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using Microsoft.SqlServer.Types;
using System.Threading;
using System.IO;
using System.Text;

namespace FPSService.TowTruck
{
    
    public class TowTruck
    {

        #region  " Initialization "

        public GPS GPSPosition { get; set; }
        public TowTruckExtended Extended { get; set; }
        public LastMessageRec LastMessage { get; set; }
        public TowTruckStatus Status { get; set; }
        public State State { get; set; }
        public TowTruckDriver Driver { get; set; }
        private Logging.EventLogger logger;
        public string Identifier;
        public DateTime LastUpdateSrv { get; set; }
        public delegate void TowTruckChangedDelegate(TowTruck towTruck);
        public string TruckNumber;
        public TowTruckChangedDelegate TowTruckChangedEventHandler;
        public TowTruckChangedDelegate TowTruckGPSUpdateEventHandler;
        public Hashtable IDHash = new Hashtable();
        private MessageQueue ttMsgQueue = new MessageQueue();
        public MessageQueue TTQueue { get { return ttMsgQueue; } }
        public bool Reflect { get; set; }
        private Thread processMessageThread;
        private IPEndPoint ttEndPoint;
        private object lockObject = new object();
        public object TowTruckLock { get { return lockObject; } }
        private UdpClient udpClient;
        private char[] delimitCharacters = { '<', '>' };
        private Queue<string> TokenList = new Queue<string>();
        private Stack<String> tokenStack = new Stack<string>();
        private Stack<AttributeNode> attributeStack = new Stack<AttributeNode>();
        private AttributeNode CurrentAttribute = null;
        private AttributeNode BaseNode = null;
        private String currentValue = "";
        public List<MiscData.Assist> theseRequests;
        public string beatNumber = "NOBEAT";
        public string onBeat = "NOBEAT";
        //public AssignedBeat assignedBeat;
        public string location = "Not Set";
        private bool inYard = false; //current packet set up in a tow yard
        private bool prevYard = false;//previous packet set us in a tow yard
        //public MiscData.BeatSchedule thisSchedule;
        public string earlyRollin = "";
        public string CHPEarlyRollInNumber = "";
        //add the alarms to the truck
        public TowTruckAlarms tta;
        public TruckStatus tts;
        public Guid runID;
        public bool wentOnPatrol = false;
        public bool rolledIn = false;
        public string incidentID = "0";
        public string cadCallSign;
        public string shiftType;
        public Guid currentIncidentID; //probably won't need this anymore
        public Incident currentIncident;
        public List<Assist> currentAssists;
        public WAZE.WAZEClass currentWAZE;

        public TowTruck(string _ipaddr)
        {

            GPSPosition = new GPS();
            GPSPosition.Time = DateTime.Now.ToString();
            GPSPosition.Lat = 0.0;
            GPSPosition.Lon = 0.0;
            GPSPosition.MLat = 0.0;
            GPSPosition.MLon = 0.0;
            Extended = new TowTruckExtended();
            logger = new Logging.EventLogger();
            LastMessage = new LastMessageRec();
            Status = new TowTruckStatus();
            Driver = new TowTruckDriver();
            Driver.LastName = "Not Logged On";
            Driver.AssignedShift = "NA";
            Driver.callSign = "NA";

            Identifier = _ipaddr;
            TruckNumber = "NOID";
            Status.VehicleStatus = "Waiting for Driver Login";
            
            processMessageThread = new Thread(new ThreadStart(ParseMessage));
            processMessageThread.Name = "TTProcessMessage";
            processMessageThread.Start();
            ttEndPoint = new IPEndPoint(IPAddress.Parse(_ipaddr), 9009);
            udpClient = new UdpClient();
            State = new State();
            State.CarID = "NOID";
            State.GpsRate = 0;
            State.Log = "F";
            State.ServerIP = "127.0.0.1";
            State.SFTPServerIP = "127.0.0.1";
            theseRequests = new List<MiscData.Assist>();
            tta = new TowTruckAlarms();
            tts = new TruckStatus();
            runID = new Guid("00000000-0000-0000-0000-000000000000");
            currentIncidentID = new Guid("00000000-0000-0000-0000-000000000000");
            cadCallSign = "NA";
            shiftType = "NA";
            currentIncident = null;
            currentAssists = null;
            currentWAZE = null;
        }

        #endregion

        #region " UDP Message Parsers "

        private void ParseMessage()
        {
            string message = TTQueue.WaitForMessage();
            if (message.Contains("</Id>"))
            {
                int ID = GetID(message);
                if (!IDHash.ContainsValue(ID))
                {
                    IDHash.Add(ID, ID);
                    if (Reflect) WriteMessage(message);
                    //logger.writeToLogFileFromTT(message) need to implement to track message sending
                    string[] parsed = message.Split(delimitCharacters);
                    foreach (string item in parsed)
                    {
                        if (item.Trim() != "") parseElement(item.Trim());
                    }
                    if (tokenStack.Count > 0)
                    {
                        IDHash.Remove(ID);
                        tokenStack = new Stack<string>();
                    }
                }
                else if (!message.Contains("</Ack>"))
                {
                    string MyAck = "<Ack><Id>" + ID.ToString() + "</Id></Ack>";
                    WriteMessage(MyAck);
                }
            }
        }
        
        public void parseElement(String token)
        {
            if (token[0] == '/')
            {
                if (tokenStack.Count > 0 && token == "/" + tokenStack.Last())
                {
                    // check if it is a message
                    TowTruckMessage msg = checkIfMessage(tokenStack.Last());
                    if (msg != null)
                    {
                        AddAttributesToMessage(msg, BaseNode);
                        msg.Execute(this);
                        CurrentAttribute = null;
                        BaseNode = null;
                    }
                    tokenStack = new Stack<string>();
                    return;
                }

                bool found = false;
                String element = "";
                while (!found && tokenStack.Count > 1)
                {
                    // find attribute pair
                    if (tokenStack.Count == 0) return; // ignore
                    element = tokenStack.Pop();
                    if (token == "/" + element)
                    {
                        found = true;
                    }
                    else
                    {
                        currentValue = element;
                    }
                }
                createSiblingNode(element, currentValue);
            }
            else
            {
                tokenStack.Push(token);
            }
        }

        private void createSiblingNode(String attribute, String value)
        {
            AttributeNode newNode = new AttributeNode();
            newNode.Attribute = attribute;
            newNode.Value = value;
            if (BaseNode == null)
            {
                BaseNode = newNode;
            }
            else
            {
                CurrentAttribute.NextAttribute = newNode;
            }
            CurrentAttribute = newNode;
            currentValue = "";
        }

        private void AddAttributesToMessage(TowTruckMessage msg, AttributeNode node)
        {
            // currently only support on level deep XML packets
            while (node != null)
            {
                msg.AddAttribute(node.Attribute, node.Value);
                AttributeNode next = node.NextAttribute;
                node = null;
                node = next;
            }
        }

        private TowTruckMessage checkIfMessage(string token)
        {
            switch (token)
            {
                case "GPS":
                    return new GPSMessage();
            }
            return null;
        }

        public void WriteMessage(String message)
        {
            lock (this)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                try
                {
                    //logger.writeToLogFileToLMT(message); need to implement to track message sending
                    udpClient.Send(buffer, buffer.Length, ttEndPoint);
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private int GetID(string msg)
        {
            int fID = msg.IndexOf("<Id>");
            fID += 4;
            int lID = msg.IndexOf("</Id>");
            string ID = msg.Substring(fID, lID - fID);
            return Convert.ToInt32(ID);
        }

        #endregion

        public void setShiftType()
        {
            string cDate = DateTime.Now.ToShortDateString();
            DateTime morning = Convert.ToDateTime(cDate + " 09:59:00");
            DateTime mid = Convert.ToDateTime(cDate + " 14:29:00");
            if (this.Driver.schedule != null)
            {
                string CallSign = this.Driver.callSign;
                if (this.Driver.schedule.start <= morning)
                {
                    shiftType = "A";
                }
                else if (this.Driver.schedule.start > morning && this.Driver.schedule.start <= mid)
                {
                    //shiftType = "MID";
                    shiftType = "*";
                }
                else if (this.Driver.schedule.start > mid)
                {
                    //shiftType = "PM";
                    shiftType = "B";
                }
            }
            else
            {
                if (DateTime.Now <= morning)
                {
                    shiftType = "A";
                }
                else if (DateTime.Now > morning && DateTime.Now <= mid)
                {
                    shiftType = "*";
                }
                else if(DateTime.Now > mid)
                {
                    shiftType = "B";
                }
            }
        }

        public void UpdateGPS(GPS thisGPS)
        {
            //thisGPS.BeatID = BeatData.Beats.FindBeatID(thisGPS.Position);
            //if (thisGPS.BeatID != new Guid("00000000-0000-0000-0000-000000000000"))
            //{
            //    thisGPS.BeatSegmentID = BeatData.Beats.FindBeatSegmentID(thisGPS.BeatID, thisGPS.Position);
            //}
            this.GPSPosition = thisGPS;
            //Find a way to quickly iterate beats and look for intersections of GPS info.  IF a beat is found, use that data
            //to look for a beat segment.  

            //Not acking anymore.
            //UDP.SendMessage msgAck = new UDP.SendMessage();
            //msgAck.SendMyMessage("<Ack><Id>" + this.GPSPosition.Id.ToString() + "</Id></Ack>", this.Identifier);
        }

        public void UpdateState(State thisState)
        {
            if (!string.IsNullOrEmpty(thisState.CarID))
            { this.State.CarID = thisState.CarID; }
            if (thisState.GpsRate != 0)
            { this.State.GpsRate = thisState.GpsRate; }
            if (thisState.Log != null)
            { this.State.Log = thisState.Log; }
            if (!string.IsNullOrEmpty(thisState.ServerIP))
            { this.State.ServerIP = thisState.ServerIP; }
            if (!string.IsNullOrEmpty(thisState.SFTPServerIP))
            {
                this.State.SFTPServerIP = thisState.SFTPServerIP;
            }
            if (!string.IsNullOrEmpty(thisState.Version))
            {
                this.State.Version = thisState.Version;
            }
            if (!string.IsNullOrEmpty(thisState.IPList))
            {
                this.State.IPList = thisState.IPList;
            }
            if (!string.IsNullOrEmpty(thisState.BillStartDay))
            {
                this.State.BillStartDay = thisState.BillStartDay;
            }
            if (!string.IsNullOrEmpty(thisState.LastBillReset))
            {
                this.State.LastBillReset = thisState.LastBillReset;
            }
            if (!string.IsNullOrEmpty(thisState.DataUsed))
            {
                this.State.DataUsed = thisState.DataUsed;
            }
            if (!string.IsNullOrEmpty(thisState.IgnTimeoutSecs))
            {
                this.State.IgnTimeoutSecs = thisState.IgnTimeoutSecs;
            }

            //UDP.SendMessage msgAck = new UDP.SendMessage();
            //msgAck.SendMyMessage("<Ack><Id>" + thisState.Id.ToString() + "</Id></Ack>", this.Identifier);
            /*
            if (!string.IsNullOrEmpty(thisState.IPList) && this.Status.VehicleStatus == "Waiting for Driver Login")
            {
                //IPList should only be populated at driver log off.  The normal logoff event would reset these values but it may not get back
                //in time to finish the log off event so this short circuits it so we can log the driver information along with the date useage
                SQL.SQLCode mySql = new SQL.SQLCode();
                mySql.logDataUse(this);
                this.Driver.FirstName = "No Driver";
                this.Driver.LastName = "No Driver";
                
            }
             * */
        }

        public void startStatus(string _statusName) //unused
        {
            this.tts.startStatus(_statusName, this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head, 
                this.Driver.schedule.scheduleID);
        }

        public void endStatus(string _statusName) //unused
        {
            this.tts.stopStatus(_statusName, this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                this.Driver.schedule.scheduleID);
        }

        public void SendMessage(string _msg)
        {
            UDP.SendMessage msg = new UDP.SendMessage();
            msg.SendMyMessage(_msg, this.Identifier);
        }

        public void TowTruckChanged()
        {
            if (TowTruckChangedEventHandler != null)
            {
                TowTruckChangedEventHandler(this);
            }
        }

        public void setStatus(string Status, bool cadCommand)
        {
            string CurrentStatus = this.Status.VehicleStatus;
            if (CurrentStatus != Status)
            {
                this.Status.StatusStarted = DateTime.Now;
                this.tts.stopStatus(CurrentStatus, this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                    this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.schedule.scheduleID);
                
                this.tts.startStatus(Status, this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                    this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.schedule.scheduleID);
                this.Status.VehicleStatus = Status;
            }
            if (Status == "ON PATROL")
            {

                if (wentOnPatrol == false)
                {
                    //check the times
                    if (this.Driver.schedule != null)
                    {
                         DateTime scheduleStart = this.Driver.schedule.start.AddMinutes(DataClasses.GlobalData.OnPatrollLeeway);
                         DateTime now = DateTime.Now;
                         if (now > scheduleStart)
                         {
                            //got a misfired logon, log it and then stop the alarm.
                            this.tta.startAlarm("LateOnPatrol", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                                this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon,
                                this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head);
                            AlarmTimer alarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "LateOnPatrol"; });
                            alarm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                                this.beatNumber, "LateOnPatrol", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                            alarm.hasAlarm = true;
                            alarm.alarmValue = DateTime.Now.ToString() + "|" + this.Driver.schedule.start.ToString();
                            alarm.comment = DateTime.Now.ToString() + "|" + this.Driver.schedule.start.ToString();
                            this.tta.stopAlarm("LateOnPatrol", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                                this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon,
                                this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                            alarm.hasAlarm = true;
                        }
                         string cadMSG = "AU.";
                         cadMSG += this.shiftType + "." + this.Driver.callSign;
                         Global.cSender.sendMessage(cadMSG);
                         cadMSG = "US."; //set update status command
                         cadMSG += this.shiftType + this.Driver.callSign; //set shift+callsign
                         cadMSG += ".S/108" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                         Global.cSender.sendMessage(cadMSG);
                    }

                }
                //Tell the CAD System what's up
                if (!cadCommand) //if this was initiated by the CAD just put the truck into the correct state and don't notify the CAD
                {
                    //tell the CAD the unit is available
                    string cadMSG = "US."; //set update status command
                    cadMSG += this.shiftType + this.Driver.callSign; //set shift+callsign
                    cadMSG += ".S/108" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                    Global.cSender.sendMessage(cadMSG);
                }
                wentOnPatrol = true;
            }
            if (Status == "ROLL IN")
            {
                if (rolledIn == false)
                {
                    if (!string.IsNullOrEmpty(this.Driver.schedule.ScheduleName))
                    {
                        DateTime dtShiftEnd = this.Driver.schedule.end.AddMinutes(DataClasses.GlobalData.RollInLeeway * -1);
                        if (DateTime.Now < dtShiftEnd)
                        {
                            //got an early roll in.
                            this.tta.startAlarm("EarlyOutOfService", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                               this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon,
                               this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head);
                            AlarmTimer alarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "EarlyOutOfService"; });
                            alarm.hasAlarm = true;
                            alarm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                                this.beatNumber, "EarlyOutOfService", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                            alarm.alarmValue = DateTime.Now.ToString() + "|" + dtShiftEnd.ToString();
                            alarm.comment = DateTime.Now.ToString() + "|" + dtShiftEnd.ToString();
                            this.tta.stopAlarm("EarlyOutOfService", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                                this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon,
                                this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                            alarm.hasAlarm = true;
                        }
                        string shiftType = this.Driver.schedule.ScheduleName;

                    }
                }
                if (!cadCommand) //if this was initiated by the CAD just put the truck into the correct state and don't notify the CAD
                {
                    //tell the CAD the unit is unavailable
                    string cadMSG = "US."; //set update status command
                    cadMSG += this.shiftType + this.Driver.callSign; //set shift+callsign
                    cadMSG += ".S/107" + makeCADDate(DateTime.Now) + makeCADTime(DateTime.Now);
                    Global.cSender.sendMessage(cadMSG);
                }
                rolledIn = true;
            }
        }

        #region  " Cad Helpers "

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

        #endregion

        public void logOff()
        {
            if (this.Driver.DriverID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                DataClasses.GlobalData.ForceDriverLogoff(this.Driver.DriverID);
            }
            setStatus("LogOff", false);
        }

        public void TowTruckGPSUpdate()
        {
            if (TowTruckGPSUpdateEventHandler != null)
            {
                TowTruckGPSUpdateEventHandler(this);
            }

            if (this.tts.currentStatus == "Waiting for Driver Login" || this.tts.currentStatus == "NA")
            {
                DataClasses.GlobalData.RemoveCover(this.TruckNumber);
            }

            #region " Location Checking " 
            //Trucks can be in a yard, at a drop site, or on a segment.

            #region " Location Checking : Has Assigned Beat "

            if (this.beatNumber != "NOBEAT")
            {
                inYard = false;
                bool OnBeat = BeatData.Beats.checkBeatIntersect(this.GPSPosition.Lat, this.GPSPosition.Lon, beatNumber);
                this.location = "OFF BEAT";
                if (OnBeat == true)
                {
                    inYard = false;
                    if (this.Status.VehicleStatus.ToUpper() == "ROLL OUT" || this.Status.VehicleStatus.ToUpper() == "ROLLOUT")
                    {
                        ///setStatus("ON PATROL", false);
                    }
                    string segment = BeatData.BeatSegments.findBeatSegByBeat(this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon);
                    if (!string.IsNullOrEmpty(segment) && segment != "NOT FOUND")
                    {
                        this.location = segment;
                    }
                }
                else
                {
                    //check inYard
                    string YardName = BeatData.Yards.findYard(this.GPSPosition.Lat, this.GPSPosition.Lon);
                    if (!string.IsNullOrEmpty(YardName) && YardName != "NOT FOUND")
                    {
                        location = YardName;
                        inYard = true;
                        if (this.Status.VehicleStatus == "ON PATROL")
                        {
                            //setStatus("ROLL IN", false);
                        }
                    }
                    //check inDropSite
                    string drop = BeatData.DropSites.findDropSite(this.GPSPosition.Lat, this.GPSPosition.Lon);
                    if (!string.IsNullOrEmpty(drop) && drop != "NOT FOUND")
                    {
                        this.location = drop;
                    }
                }
                if (prevYard == true && inYard == false)
                {
                    //setStatus("ROLL OUT", false);
                }
                prevYard = inYard;
            }

            #endregion

            #region " Location Checking : No assigned beat "
            else
            {
                this.location = "NOBEAT";
                //check inYard
                string YardName = BeatData.Yards.findYard(this.GPSPosition.Lat, this.GPSPosition.Lon);
                if (!string.IsNullOrEmpty(YardName) && YardName != "NOT FOUND")
                {
                    location = YardName;
                }
                //check inDropSite
                string drop = BeatData.DropSites.findDropSite(this.GPSPosition.Lat, this.GPSPosition.Lon);
                if (!string.IsNullOrEmpty(drop) && drop != "NOT FOUND")
                {
                    this.location = drop;
                }
            }

            #endregion

            #endregion

            #region " Alarm Checking "

            #region " Alarm Checking : Logged on Beat Assigned "

            if (this.beatNumber != "NOBEAT")
            {

                #region " Long Incident Alarm Checking "
                //On Incident - Long Incident
                if (this.tts.currentStatus == "On Incident" || this.tts.currentStatus.ToUpper() == "ONINCIDENT") //track time spent on incident, if over x minutes, raise an alarm.
                {
                    
                    tta.startAlarm("LongIncident", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                        this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                    AlarmTimer lngIncAlarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "LongIncident"; });
                    lngIncAlarm.hasAlarm = false;
                    lngIncAlarm.alarmValue = "LONG INCIDENT Since: " + lngIncAlarm.GetStartTime();
                    if (this.tta.getAlarmStart("LongIncident").AddMinutes(DataClasses.GlobalData.ExtendedLeeway) < DateTime.Now)
                    {
                        lngIncAlarm.hasAlarm = true;
                        lngIncAlarm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                        this.beatNumber, "LongIncident", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    }
                }
                else
                {
                    AlarmTimer longIncidentAlarm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongIncident"; });
                    if (longIncidentAlarm != null)
                    {
                        tta.stopAlarm("LongIncident", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        longIncidentAlarm.hasAlarm = false;

                    }
                }

                #endregion

                #region " Overtime Activity "

                //check for overtime activity
                if (this.tts.currentStatus == "On Incident" || this.tts.currentStatus == "OnIncident")
                {
                    if (this.Driver.schedule != null)
                    {
                        if (this.Driver.schedule.end < DateTime.Now)
                        {
                            this.tta.startAlarm("OvertimeActivity", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName,
                                this.Driver.TowTruckCompany, this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon,
                                this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head);
                            AlarmTimer alarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "OvertimeActivity"; });
                            alarm.hasAlarm = true;
                            alarm.alarmValue = "OVERTIME ACTIVITY: " + DateTime.Now.ToString();
                            alarm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "OvertimeActivity", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        }
                    }
                }
                else
                {
                    AlarmTimer overtimeAlarm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "OvertimeActivity"; });
                    if (overtimeAlarm != null)
                    {
                        tta.stopAlarm("OvertimeActivity", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                            this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        overtimeAlarm.hasAlarm = false;
                    }
                }

                #endregion

                #region " Off Beat Alarm Checking "

                bool OnBeat = BeatData.Beats.checkBeatIntersect(this.GPSPosition.Lat, this.GPSPosition.Lon, beatNumber);
                if (OnBeat == false && this.Status.VehicleStatus.ToUpper() == "ON PATROL")
                {
                    //start timer but don't fire alert until the truck is off beat for x minutes
                    AlarmTimer offBeatAlarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "OffBeat"; });
                    tta.startAlarm("OffBeat", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                        this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                    offBeatAlarm.hasAlarm = false;
                    offBeatAlarm.alarmValue = "OFF BEAT Since: " + offBeatAlarm.GetStartTime();
                    if (this.tta.getAlarmStart("OffBeat").AddMinutes(DataClasses.GlobalData.OffBeatLeeway) < DateTime.Now)
                    {
                        offBeatAlarm.hasAlarm = true;
                        offBeatAlarm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "OffBeat", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    }
                }
                else
                {
                    AlarmTimer offBeatAlarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "OffBeat"; });
                    tta.stopAlarm("OffBeat", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                        this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    offBeatAlarm.hasAlarm = false;
                    offBeatAlarm.alarmValue = "";
                }

                #endregion

                #region " Long Incident Alarm Checking -- DUPLICATE "
                /*Check for incident alarm - truck must be ON INCIDENT for > x time
                if (this.tts.currentStatus == "On Incident") //track time spent on incident, if over x minutes, raise an alarm.
                {
                    if (this.tts.statusStarted.AddMinutes(DataClasses.GlobalData.ExtendedLeeway) < DateTime.Now)
                    {
                        tta.startAlarm("LongIncident", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID);
                        AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongIncident"; });
                        if (alrm != null)
                        {
                            alrm.hasAlarm = true;
                        }
                    }
                    else
                    {
                        AlarmTimer longIncidentAlarm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongIncident"; });
                        if (longIncidentAlarm != null)
                        {
                            tta.stopAlarm("LongIncident", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                                this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID);
                            longIncidentAlarm.hasAlarm = false;
                        }
                    }
                }
                else //truck status changes from incident to anything else, kill the alarm if one exists
                {
                    AlarmTimer longIncidentAlarm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongIncident"; });
                    if (longIncidentAlarm != null && longIncidentAlarm.hasAlarm == true)
                    {
                        tta.stopAlarm("LongIncident", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID);
                        longIncidentAlarm.hasAlarm = false;
                    }
                }
                */
                #endregion

                #region " Long Lunch "

                if (this.tts.currentStatus == "On Lunch" || this.tts.currentStatus.ToUpper() == "ONLUNCH")
                {
                    tta.startAlarm("LongLunch", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongLunch"; });
                    if (alrm != null && alrm.GetStartDateTime().AddMinutes(30) < DateTime.Now)
                    {
                        alrm.hasAlarm = true;
                        alrm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "LongLunch", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    }
                    else
                    {
                        alrm.hasAlarm = false;
                    }
                }
                else
                {
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongLunch"; });
                    if (alrm != null && alrm.isRunning() == true)
                    {
                        tta.stopAlarm("LongLunch", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        alrm.hasAlarm = false;
                    }
                }

                #endregion

                #region " Long Break "

                if (this.tts.currentStatus == "On Break" || this.tts.currentStatus.ToUpper() == "ONBREAK")
                {
                    tta.startAlarm("LongBreak", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongBreak"; });
                    if (alrm != null && alrm.GetStartDateTime().AddMinutes(15) < DateTime.Now)
                    {
                        alrm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "LongBreak", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        alrm.hasAlarm = true;
                    }
                    else
                    {
                        alrm.hasAlarm = false;
                    }
                }
                else
                {
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "LongBreak"; });
                    if (alrm != null && alrm.isRunning() == true)
                    {
                        tta.stopAlarm("LongBreak", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        alrm.hasAlarm = false;
                    }
                }

                #endregion

                #region " Stationary "

                if (this.GPSPosition.Speed == 0 && (this.Status.VehicleStatus == "OnPatrol" || this.Status.VehicleStatus == "ON PATROL"))
                {
                    tta.startAlarm("Stationary", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "Stationary"; });
                    if (alrm != null && alrm.GetStartDateTime().AddMinutes(DataClasses.GlobalData.StationaryLeeway) < DateTime.Now)
                    {
                        alrm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "Stationary", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        alrm.hasAlarm = true;
                    }
                    else
                    {
                        alrm.hasAlarm = false;
                    }
                }
                else
                {
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "Stationary"; });
                    if (alrm != null && alrm.isRunning() == true)
                    {
                        tta.stopAlarm("Stationary", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                                this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        alrm.hasAlarm = false;
                    }
                }

                #endregion

                /**** NEED TO ADD OverTime, LateOnPatrol, EarlyOutOfService ****/
                /*** Need Schedule processing for that ****/

            } //end has beat assigned

            #endregion

            #region " Alarms always checked "

            #region " Speeding Checking "
            if (this.Driver.LastName.ToUpper() != "NO DRIVER" && this.Driver.LastName.ToUpper() != "NOT LOGGED ON")
            {
                if (this.GPSPosition.MaxSpd > DataClasses.GlobalData.SpeedingLeeway || this.GPSPosition.Speed > DataClasses.GlobalData.SpeedingLeeway)
                {
                    tta.startAlarm("Speeding", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany, this.beatNumber,
                        this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                        this.GPSPosition.Speed, this.GPSPosition.Head);
                    AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "Speeding"; });
                    if (alrm != null)
                    {
                        alrm.hasAlarm = true;
                        alrm.alarmValue = "Speed: " + this.GPSPosition.Speed.ToString() + " Max Speed: " + this.GPSPosition.MaxSpd.ToString();
                        alrm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "Speeding", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    }
                }
                else
                {
                    AlarmTimer speedingAlarm = tta.truckAlarms.Find(delegate(AlarmTimer alFind) { return alFind.alarmName == "Speeding"; });
                    if (speedingAlarm != null && speedingAlarm.isRunning())
                    {
                        tta.stopAlarm("Speeding", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany, this.beatNumber,
                            this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                        this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                        speedingAlarm.hasAlarm = false;
                        int iSpeed = tta.getAlarmDuration("Speeding");
                    }
                }
            }
            

            #endregion

            #region " GPS ISSUE Checking "

            //if (this.GPSPosition.Lat == 0.0 && this.GPSPosition.Lon == 0.0)
            if(this.GPSPosition.Status != "Valid")
            {
                tta.startAlarm("GPSIssue", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                    this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head);
                AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "GPSIssue"; });
                alrm.hasAlarm = true;
                alrm.alarmValue = "Vehicle reported " + this.GPSPosition.Lat.ToString() + " lat / " + this.GPSPosition.Lon + "lon";
                alrm.logAlarm(this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                            this.beatNumber, "GPSIssue", this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location, this.GPSPosition.Speed, this.GPSPosition.Head,
                                this.Driver.callSign, this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
            }
            else
            {
                AlarmTimer alrm = tta.truckAlarms.Find(delegate(AlarmTimer find) { return find.alarmName == "GPSIssue"; });
                if (alrm != null && alrm.isRunning() == true)
                {
                    tta.stopAlarm("GPSIssue", this.TruckNumber, this.Driver.FirstName + " " + this.Driver.LastName, this.Driver.TowTruckCompany,
                    this.beatNumber, this.GPSPosition.Lat, this.GPSPosition.Lon, this.runID, this.location,
                    this.GPSPosition.Speed, this.GPSPosition.Head, this.Driver.callSign,
                                this.Driver.schedule.scheduleID, this.Driver.schedule.ScheduleType);
                    alrm.hasAlarm = false;
                    alrm.alarmValue = "NO ISSUE";
                }
            }

            #endregion

            #endregion

            #endregion
        }

        #region " New Assist and Incident stuff "

        private void wazer(Incident i, List<Assist> aList, bool close) {
            try {
                if (!string.IsNullOrEmpty(currentIncident.chpIncidentType) &&
                        currentIncident.chpIncidentType != "Meet With (11-98)" &&
                        currentIncident.chpIncidentType != "Test Code (11-99)" &&
                        currentIncident.chpIncidentType.ToUpper() != "OTHER")
                {
                    string wazeType = "";
                    string wazeSubType = "";
                    if (currentIncident.chpIncidentType == "Abandoned Vehicle (11-24)" ||
                        currentIncident.chpIncidentType == "Disabled Vehicle-Occupied (11-26)" ||
                        currentIncident.chpIncidentType == "Traffic Hazard (11-25)")
                    {
                        wazeType = "HAZARD";
                        wazeSubType = "HAZARD_ON_ROAD";
                    }
                    if (currentIncident.chpIncidentType == "Accident-No Details (11-83)" ||
                        currentIncident.chpIncidentType == "Accident-Property Damage (11-82)")
                    {
                        wazeType = "ACCIDENT";
                        wazeSubType = string.Empty;
                    }
                    WAZE.WAZEClass w = new WAZE.WAZEClass();
                    WAZE.PostWAZE pw = new WAZE.PostWAZE();
                    string detailNote = "NA";
                    if (aList != null && aList.Count > 0 && !string.IsNullOrEmpty(aList[0].detailNote)) { 
                        detailNote = aList[0].detailNote;
                    }
                    if (currentWAZE != null && !string.IsNullOrEmpty(currentWAZE.description) && currentWAZE.description != "NA") {
                        detailNote = currentWAZE.description;
                    }
                    //street = freeway + direction from Incident object
                    //description = incident type - (11,10,etc) + position (RS, CD, etc)
                    //per MTC request 03/01/2017 EL
                    string street = i.direction + " " + i.freeway;
                    string description = i.chpIncidentType.Replace(" (11-83)", "").Replace(" (11-82)", "").Replace(" (11-24)", "").Replace(" (11-26)", "").Replace(" (11-25)","");
                    string position = i.positionIncident;
                    switch (position) { 
                        case "RS":
                            position = "On Shoulder";
                            break;
                        case "CD":
                            position = "In Center-Divide";
                            break;
                        case "Ramp":
                            position = "On Ramp";
                            break;
                        case "InLane":
                            position = "In Lane";
                            break;
                    }
                    position = description + " " + position;
                    if (close)
                    {
                        string polyLine = currentIncident.lat.ToString() + "," + currentIncident.lon.ToString() + "," + currentIncident.lat.ToString() + "," + currentIncident.lon.ToString();
                        w = pw.makeWaze(currentIncident.incidentID.ToString(), (DateTime)currentIncident.incidentDatePosted,
                            DateTime.Now, wazeType, wazeSubType, position,
                            street, polyLine, (DateTime)currentIncident.incidentDatePosted, DateTime.Now);
                        currentWAZE = w;
                        pw.postWazeData(w, this.Driver.callSign, this.Driver.LastName + ", " + this.Driver.FirstName);
                    }
                    else {
                        string polyLine = currentIncident.lat.ToString() + "," + currentIncident.lon.ToString() + "," + currentIncident.lat.ToString() + "," + currentIncident.lon.ToString();
                        w = pw.makeWaze(currentIncident.incidentID.ToString(), (DateTime)currentIncident.incidentDatePosted,
                            DateTime.Now, wazeType, wazeSubType, position,
                            street, polyLine, (DateTime)currentIncident.incidentDatePosted, DateTime.Now.AddMinutes(60));
                        currentWAZE = w;
                        pw.postWazeData(w, this.Driver.callSign, this.Driver.LastName + ", " + this.Driver.FirstName);
                    }

                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void addIncident(Incident i) {
            try {
                if (this.currentIncident == null){
                    if (i.incidentID == null || i.incidentID == Guid.Empty)
                    {
                        i.incidentID = Guid.NewGuid();
                        i.incidentDatePosted = DateTime.Now;
                        if (string.IsNullOrEmpty(i.userPosted)) {
                            i.userPosted = this.Driver.FirstName + " " + this.Driver.LastName;
                        }
                        
                        i.callSign = this.Driver.callSign;
                        //i.fromTruck = true;
                        i.lat = this.GPSPosition.Lat;
                        i.lon = this.GPSPosition.Lon;
                        i.canceled = false;
                        i.reasonCanceled = "NA";
                        i.beat = this.beatNumber;
                        i.truckNumber = this.TruckNumber;
                        i.truckStatusID = this.tts.statusID;
                        i.driverLastName = this.Driver.LastName;
                        i.driverFirstName = this.Driver.FirstName;
                        i.driverID = this.Driver.DriverID;
                        i.runID = this.runID;
                        i.timeOnIncident = DateTime.Now;
                        i.requestSent = i.requestSent;
                        
                        i.dispatchLocation = BeatData.BeatSegments.findBeatSeg(this.GPSPosition.Lat, this.GPSPosition.Lon);
                        this.currentIncident = i;
                        //Log that sucka
                        SQL.SQLCode sql = new SQL.SQLCode();
                        sql.logNewIncident(currentIncident);
                    }
                    else
                    {
                        i.incidentDatePosted = DateTime.Now;
                        if (string.IsNullOrEmpty(i.userPosted)) {
                            i.userPosted = this.Driver.FirstName + " " + this.Driver.LastName;
                        }
                        i.callSign = this.Driver.callSign;
                        i.fromTruck = i.fromTruck;
                        i.lat = this.GPSPosition.Lat;
                        i.lon = this.GPSPosition.Lon;
                        i.canceled = false;
                        i.reasonCanceled = "NA";
                        i.beat = this.beatNumber;
                        i.truckNumber = this.TruckNumber;
                        i.truckStatusID = this.tts.statusID;
                        i.driverLastName = this.Driver.LastName;
                        i.driverFirstName = this.Driver.FirstName;
                        i.driverID = this.Driver.DriverID;
                        i.runID = this.runID;
                        i.timeOnIncident = DateTime.Now;
                        i.requestSent = i.requestSent;
                        i.dispatchLocation = BeatData.BeatSegments.findBeatSeg(this.GPSPosition.Lat, this.GPSPosition.Lon);
                        this.currentIncident = i;
                        SQL.SQLCode sql = new SQL.SQLCode();
                        sql.logNewIncident(currentIncident);
                    }
                    //determine if this is wazeable information
                    wazer(currentIncident, currentAssists, false);
                }
                else { 
                    throw new Exception("Another incident exists and has not been closed");
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void updateIncident(Incident i) {

            try {
                if (this.currentIncident == null)
                {
                    throw new Exception("No current incident to update");
                }
                else {
                    if (i.incidentID == null || i.incidentID == Guid.Empty || this.currentIncident == null)
                    {
                        //can't update this without incident id
                        throw new Exception("Cannot log: null incident id");
                    }
                    else if (i.incidentID != currentIncident.incidentID) {
                        throw new Exception("Incorrect incident id for update");
                    }
                    else
                    {
                        if (i.briefUpdatePosted == true && currentIncident.briefUpdatePosted == false)
                        {
                            i.timeOfBriefUpdate = DateTime.Now;
                            i.briefUpdateLat = this.GPSPosition.Lat;
                            i.briefUpdateLon = this.GPSPosition.Lon;
                        }
                        currentIncident.canceled = i.canceled;
                        currentIncident.reasonCanceled = i.reasonCanceled;
                        if (i.logID != null && i.logID != Guid.Empty) {
                            currentIncident.logID = i.logID;
                        }
                        if (!string.IsNullOrEmpty(i.wazeID)) {
                            currentIncident.wazeID = i.wazeID;
                        }
                        
                        currentIncident.truckStatusID = this.tts.statusID;
                        if (!string.IsNullOrEmpty(i.FSPLocation)) {
                            currentIncident.FSPLocation = i.FSPLocation;
                        }
                        if (!string.IsNullOrEmpty(i.dispatchLocation)) {
                            currentIncident.dispatchLocation = i.dispatchLocation;
                        }
                        if (!string.IsNullOrEmpty(i.direction)) {
                            currentIncident.direction = i.direction;
                        }
                        
                        currentIncident.positionIncident = i.positionIncident;
                        currentIncident.laneNumber = i.laneNumber;
                        if (!string.IsNullOrEmpty(i.chpIncidentType)) {
                            currentIncident.chpIncidentType = i.chpIncidentType;
                        }
                        
                        currentIncident.briefUpdateLat = i.briefUpdateLat;
                        currentIncident.briefUpdateLon = i.briefUpdateLon;
                        currentIncident.freeway = i.freeway;
                        currentIncident.briefUpdatePosted = i.briefUpdatePosted;
                        currentIncident.timeOfBriefUpdate = i.timeOfBriefUpdate;
                        currentIncident.CHPLogNumber = i.CHPLogNumber;
                        if (string.IsNullOrEmpty(currentIncident.incidentSurveyNumber)) {
                            currentIncident.incidentSurveyNumber = i.incidentSurveyNumber;
                        }
                        currentIncident.driverLastName = this.Driver.LastName;
                        currentIncident.driverFirstName = this.Driver.FirstName;
                        currentIncident.driverID = this.Driver.DriverID;
                        currentIncident.runID = this.runID;
                        currentIncident.requestSent = i.requestSent;
                        currentIncident.comment = i.comment;
                        //currentIncident.incidentDatePosted = DateTime.Now;
                        currentIncident.dispatchLocation = BeatData.BeatSegments.findBeatSeg(this.GPSPosition.Lat, this.GPSPosition.Lon);
                        SQL.SQLCode sql = new SQL.SQLCode();
                        sql.logNewIncident(currentIncident);
                        wazer(currentIncident, currentAssists, false);
                    }
                }
                
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void closeIncident(Incident i) {
            try
            {
                if (currentIncident == null)
                {
                    throw new Exception("Cannot close, no current active incident for this truck");
                }
                else if (currentIncident.incidentID != i.incidentID)
                {
                    throw new Exception("Incorrect incidentID");
                }
                else
                {
                    currentIncident.callSign = this.Driver.callSign;
                    currentIncident.fromTruck = i.fromTruck;
                    currentIncident.lat = this.GPSPosition.Lat;
                    currentIncident.lon = this.GPSPosition.Lon;
                    currentIncident.canceled = i.canceled;
                    currentIncident.reasonCanceled = i.reasonCanceled;
                    currentIncident.beat = this.beatNumber;
                    currentIncident.truckNumber = this.TruckNumber;
                    if (i.logID != null && i.logID != Guid.Empty)
                    {
                        currentIncident.logID = i.logID;
                    }
                    if (!string.IsNullOrEmpty(i.wazeID))
                    {
                        currentIncident.wazeID = i.wazeID;
                    }
                    currentIncident.truckStatusID = tts.statusID;
                    if (!string.IsNullOrEmpty(i.FSPLocation))
                    {
                        currentIncident.FSPLocation = i.FSPLocation;
                    }
                    if (!string.IsNullOrEmpty(i.dispatchLocation))
                    {
                        currentIncident.dispatchLocation = i.dispatchLocation;
                    }
                    if (!string.IsNullOrEmpty(i.direction))
                    {
                        currentIncident.direction = i.direction;
                    }
                    currentIncident.positionIncident = i.positionIncident;
                    currentIncident.laneNumber = i.laneNumber;
                    currentIncident.chpIncidentType = i.chpIncidentType;
                    //currentIncident.briefUpdateLat = i.briefUpdateLat;
                    //currentIncident.briefUpdateLon = i.briefUpdateLon;
                    currentIncident.freeway = i.freeway;
                    //currentIncident.briefUpdatePosted = i.briefUpdatePosted;
                    //currentIncident.timeOfBriefUpdate = i.timeOfBriefUpdate;
                    currentIncident.CHPLogNumber = i.CHPLogNumber;
                    currentIncident.incidentSurveyNumber = i.incidentSurveyNumber;
                    currentIncident.driverLastName = this.Driver.LastName;
                    currentIncident.driverFirstName = this.Driver.FirstName;
                    currentIncident.driverID = this.Driver.DriverID;
                    currentIncident.runID = this.runID;
                    currentIncident.timeOffIncident = DateTime.Now;
                    //currentIncident.incidentDatePosted = DateTime.Now;
                    currentIncident.comment = i.comment;
                    currentIncident.dispatchLocation = BeatData.BeatSegments.findBeatSeg(this.GPSPosition.Lat, this.GPSPosition.Lon);
                    //Log it to SQL
                    SQL.SQLCode sql = new SQL.SQLCode();
                    sql.logNewIncident(currentIncident);
                    wazer(currentIncident, currentAssists, true);
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                string err = ex.ToString();
            }
            finally {
                currentAssists = null;
                currentIncident = null;
                currentWAZE = null;
            }
        }

        public Guid addAssist(Assist a) {

            if (a.assistID == null || a.assistID == Guid.Empty) {
                a.assistID = Guid.NewGuid();
            }

            if (currentIncident == null || currentIncident.incidentID != a.incidentID) {
                //no current incident or incoming assist has incorrect incident id
                throw new Exception("NOINCIDENT");
            }
            try {
                if (currentAssists != null)
                {
                    foreach (Assist aFind in currentAssists)
                    {
                        if (aFind.assistID == a.assistID)
                        {
                            //we already have an assist with that id
                            throw new Exception("ASSISTIDALREADYEXISTS");
                        }
                    }
                    Assist an = new Assist();
                    an.assistID = a.assistID;
                    an.incidentID = a.incidentID;
                    an.assistDatePosted = DateTime.Now;
                    an.lastAssistInIncidentReport = a.lastAssistInIncidentReport;
                    an.problemType = a.problemType;
                    an.problemDetail = a.problemDetail;
                    an.problemNote = a.problemNote;
                    an.otherNote = a.otherNote;
                    an.transportType = a.transportType;
                    an.StartODO = a.StartODO;
                    an.EndODO = a.EndODO;
                    an.dropSite = a.dropSite;
                    an.state = a.state;
                    an.licensePlate = a.licensePlate;
                    an.vehicleType = a.vehicleType;
                    an.OTAuthorizationNumber = a.OTAuthorizationNumber;
                    an.detailNote = a.detailNote;
                    an.assistLat = this.GPSPosition.Lat;
                    an.assistLon = this.GPSPosition.Lon;
                    an.dropSiteOther = a.dropSiteOther;
                    an.callSign = this.Driver.callSign;
                    an.timeOnAssist = DateTime.Now;
                    an.actionTaken = a.actionTaken;
                    an.dropSiteBeat = a.dropSiteBeat;
                    an.PTN = a.PTN;
                    //an.timeOffAssist = a.timeOffAssist;
                    currentAssists.Add(an);
                    SQL.SQLCode sql = new SQL.SQLCode();
                    sql.logNewAssist(an);
                }
                else {
                    if (currentAssists == null) {
                        currentAssists = new List<Assist>();
                    }
                    Assist an = new Assist();
                    an.assistID = a.assistID;
                    an.incidentID = a.incidentID;
                    an.assistDatePosted = DateTime.Now;
                    an.lastAssistInIncidentReport = a.lastAssistInIncidentReport;
                    an.problemType = a.problemType;
                    an.problemDetail = a.problemDetail;
                    an.problemNote = a.problemNote;
                    an.otherNote = a.otherNote;
                    an.transportType = a.transportType;
                    an.StartODO = a.StartODO;
                    an.EndODO = a.EndODO;
                    an.dropSite = a.dropSite;
                    an.state = a.state;
                    an.licensePlate = a.licensePlate;
                    an.vehicleType = a.vehicleType;
                    an.OTAuthorizationNumber = a.OTAuthorizationNumber;
                    an.detailNote = a.detailNote;
                    an.assistLat = this.GPSPosition.Lat;
                    an.assistLon = this.GPSPosition.Lon;
                    an.dropSiteOther = a.dropSiteOther;
                    an.callSign = this.Driver.callSign;
                    an.timeOnAssist = DateTime.Now;
                    an.actionTaken = a.actionTaken;
                    an.dropSiteBeat = a.dropSiteBeat;
                    an.PTN = a.PTN;
                    //an.timeOffAssist = a.timeOffAssist;
                    currentAssists.Add(an);
                    SQL.SQLCode sql = new SQL.SQLCode();
                    sql.logNewAssist(an);

                }
                wazer(currentIncident, currentAssists, false);
                return (Guid)a.assistID;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void closeAssist(Assist a) {
            bool found = false;
            if (a.assistID == null || a.assistID == Guid.Empty)
            {
                throw new Exception("BADASSISTID");
            }

            if (currentIncident == null || currentIncident.incidentID != a.incidentID)
            {
                //no current incident or incoming assist has incorrect incident id
                throw new Exception("NOINCIDENT");
            }
                //see if we can find the assist
                try {
                    if (currentAssists != null)
                    {
                        for (int i = currentAssists.Count - 1; i >= 0;i-- )
                        {
                            if (currentAssists[i].assistID == a.assistID)
                            {
                                found = true;
                                // this is the one getting closed out
                                currentAssists[i].lastAssistInIncidentReport = a.lastAssistInIncidentReport;
                                currentAssists[i].problemType = a.problemType;
                                currentAssists[i].problemDetail = a.problemDetail;
                                currentAssists[i].problemNote = a.problemNote;
                                currentAssists[i].otherNote = a.otherNote;
                                currentAssists[i].transportType = a.transportType;
                                currentAssists[i].StartODO = a.StartODO;
                                currentAssists[i].EndODO = a.EndODO;
                                currentAssists[i].dropSite = a.dropSite;
                                currentAssists[i].state = a.state;
                                currentAssists[i].licensePlate = a.licensePlate;
                                currentAssists[i].vehicleType = a.vehicleType;
                                currentAssists[i].OTAuthorizationNumber = a.OTAuthorizationNumber;
                                currentAssists[i].detailNote = a.detailNote;
                                currentAssists[i].assistLat = this.GPSPosition.Lat;
                                currentAssists[i].assistLon = this.GPSPosition.Lon;
                                currentAssists[i].dropSiteOther = a.dropSiteOther;
                                currentAssists[i].callSign = this.Driver.callSign;
                                //an.timeOnAssist = DateTime.Now;
                                currentAssists[i].timeOffAssist = DateTime.Now;
                                currentAssists[i].actionTaken = a.actionTaken;
                                currentAssists[i].dropSiteBeat = a.dropSiteBeat;
                                currentAssists[i].PTN = a.PTN;
                                //currentAssists.Add(an);
                                //Log it
                                SQL.SQLCode sql = new SQL.SQLCode();
                                sql.logNewAssist(currentAssists[i]);
                                wazer(currentIncident, currentAssists, true);
                                //Remove it
                                currentAssists.RemoveAt(i);


                            }
                        }
                        if (a.lastAssistInIncidentReport == true)
                        {
                            currentAssists = null;
                        }
                        if (found == null) {
                            throw new Exception("ASSISTNOTFOUND");
                        }
                    }
                    else
                    {
                        throw new Exception("ASSISTNOTFOUND");
                    }
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message);
                }
        }
        

        #endregion
    }
}