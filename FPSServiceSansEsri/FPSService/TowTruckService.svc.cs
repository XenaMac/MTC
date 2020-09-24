using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using Newtonsoft.Json.Serialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace FPSService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TowTruckService" in code, svc and config file together.
    [AspNetCompatibilityRequirements(
     RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TowTruckService : ITowTruckService
    {
        private Logging.EventLogger logger; //error logging
        private Guid fakeID = new Guid("00000000-0000-0000-0000-000000000000");
        public void UpdateTruckNumber(string IPAddress, string TruckNumber, Guid ContractorID)
        {

        }

        #region " Force beat, segment, drop site, and yard reloads "

        public void reloadBeats()
        {
            //BeatData.Beats.LoadBeats();
            BeatData.Beats.LoadBeatInfo();
        }

        public void reloadBeatSegments()
        {
            BeatData.BeatSegments.LoadSegments();
        }

        public void reloadYards()
        {
            BeatData.Yards.LoadYards();
        }

        public void reloadDrops()
        {
            BeatData.DropSites.LoadDropSites();
        }

        #endregion

        #region  " Messages from trucks "

        public List<TruckMessage> getAllMessages()
        {
            List<TruckMessage> msgs = DataClasses.GlobalData.theseMessages;
            return msgs;
        }

        public List<TruckMessage> getMessagesByDriverID(string driverID)
        {
            List<TruckMessage> msgs = new List<TruckMessage>();
            string truckIP = string.Empty;
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        where t.Driver.FSPID == driverID
                        select t;
            foreach (TowTruck.TowTruck t in tList)
            {
                truckIP = t.Identifier;
            }
            var mList = from m in DataClasses.GlobalData.theseMessages
                        where m.TruckIP == truckIP
                        select m;
            foreach (TruckMessage tm in mList)
            {
                msgs.Add(tm);
            }
            return msgs;
        }

        public List<TruckMessage> getMessagesByCallSign(string CallSign)
        {
            List<TruckMessage> msgs = new List<TruckMessage>();
            string truckIP = string.Empty;
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        where t.Driver.callSign == CallSign
                        select t;
            foreach (TowTruck.TowTruck t in tList)
            {
                truckIP = t.Identifier;
            }
            var mList = from m in DataClasses.GlobalData.theseMessages
                        where m.TruckIP == truckIP
                        select m;
            foreach (TruckMessage tm in mList)
            {
                msgs.Add(tm);
            }
            return msgs;
        }

        public List<TruckMessage> getMessagesByBeat(string beatNumber)
        {
            List<TruckMessage> msgs = new List<TruckMessage>();
            List<string> truckIPs = new List<string>();
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        where t.beatNumber == beatNumber
                        select t;
            foreach (TowTruck.TowTruck t in tList)
            {
                truckIPs.Add(t.Identifier);
            }
            var mList = from m in DataClasses.GlobalData.theseMessages
                        where truckIPs.Contains(m.TruckIP)
                        select m;
            foreach (TruckMessage tm in mList)
            {
                msgs.Add(tm);
            }
            return msgs;
        }

        public List<TruckMessage> getMessagesBySender(string senderEmail)
        {
            List<TruckMessage> msgs = new List<TruckMessage>();
            var mList = from m in DataClasses.GlobalData.theseMessages
                        where m.UserEmail == senderEmail
                        select m;
            foreach (TruckMessage tm in mList)
            {
                msgs.Add(tm);
            }
            return msgs;
        }

        #endregion

        #region " New Esri-Free polygon stuff "
  
        public List<beatInformation> getAllBeats()
        {
            try
            {
                return BeatData.Beats.beatInfos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public beatInformation getBeatByBeatID(string BeatID)
        {
            try
            {
                beatInformation find = BeatData.Beats.beatInfos.Find(delegate (beatInformation found) { return found.BeatID == BeatID; });
                if (find != null)
                {
                    return find;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public List<beatSegmentPolygonData> getAllSegmentPolygons()
        {
            try
            {
                return BeatData.BeatSegments.bsPolyList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public beatSegmentPolygonData getSegmentPolygon(string SegmentID)
        {
            try
            {
                beatSegmentPolygonData found = BeatData.BeatSegments.bsPolyList.Find(delegate (beatSegmentPolygonData find) { return find.segmentID == SegmentID; });
                if (found != null)
                {
                    return found;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public beatSegmentPolygonData getSegmentPolygonById(Guid id) {
            try {
                beatSegmentPolygonData found = BeatData.BeatSegments.bsPolyList.Find(delegate(beatSegmentPolygonData find) { return find.ID == id; });
                return found;
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }

        public List<yardPolygonData> getAllYards()
        {
            try
            {
                return BeatData.Yards.yards;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public yardPolygonData getYardPolygon(string YardID)
        {
            try
            {
                yardPolygonData found = BeatData.Yards.yards.Find(delegate (yardPolygonData find) { return find.YardID == YardID; });
                if (found != null)
                {
                    return found;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<dropSitePolygonData> getAllDropSites()
        {
            try
            {
                return BeatData.DropSites.dropSites;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<esriCB> getAllCallBoxes() {
            try {
                return BeatData.CallBoxes.callBoxList;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public esriCB getCallBoxByID(string callBoxID) {
            try {
                esriCB cb = BeatData.CallBoxes.callBoxList.Find(delegate (esriCB find) { return find.CallBoxID == callBoxID; });
                if (cb != null)
                {
                    return cb;
                }
                else {
                    throw new Exception("Callbox not found");
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public dropSitePolygonData getDropSite(string DropSiteID)
        {
            try
            {
                dropSitePolygonData found = BeatData.DropSites.dropSites.Find(delegate (dropSitePolygonData find) { return find.dropSiteID == DropSiteID; });
                if (found != null)
                {
                    return found;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*
        public void updateBeatPolygonData(beatPolygonData b) {
            try {
                BeatData.Beats.addBeatPolygonData(b);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        */

        public void updateBeatInfoData(beatInformation b) {
            try {
                BeatData.Beats.addBeatInfoData(b);
                SQL.SQLCode sql = new SQL.SQLCode();
                string xml = "string";
                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(b.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, b);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }
                //sql.logData("beatinfo", xml);
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }
        public void updateBeatSegmentData(beatSegmentPolygonData b) {
            try {
                BeatData.BeatSegments.addBeatSegment(b);
                SQL.SQLCode sql = new SQL.SQLCode();
                string xml = "string";
                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(b.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, b);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }
                //sql.logData("beatsegment", xml);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void updateYardData(yardPolygonData y) {
            try {
                BeatData.Yards.addYard(y);
                SQL.SQLCode sql = new SQL.SQLCode();
                string xml = "string";
                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(y.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, y);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }
                //sql.logData("yards", xml);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void updateDropSiteData(dropSitePolygonData d) {
            try {
                BeatData.DropSites.addDropSite(d);
                SQL.SQLCode sql = new SQL.SQLCode();
                string xml = "string";
                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(d.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, d);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }
                //sql.logData("dropsite", xml);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void updateCallBox(esriCB c) {
            try {
                BeatData.CallBoxes.addCallBoxPolygonData(c);
                SQL.SQLCode sql = new SQL.SQLCode();
                string xml = "string";
                XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
                // Initializes a new instance of the XmlDocument class.          
                XmlSerializer xmlSerializer = new XmlSerializer(c.GetType());
                // Creates a stream whose backing store is memory. 
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, c);
                    xmlStream.Position = 0;
                    //Loads the XML document from the specified string.
                    xmlDoc.Load(xmlStream);
                    xml = xmlDoc.InnerXml;
                }
                //sql.logData("callbox", xml);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void deleteBeat(Guid id) {
            try {
                BeatData.Beats.deleteBeatInfoData(id);
                SQL.SQLCode sql = new SQL.SQLCode();
                //sql.logData("deletebeatinfo", id.ToString());
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }

        public void deleteBeatSegment(Guid id)
        {
            try {
                BeatData.BeatSegments.deleteBeatSegmentPolygonData(id);
                SQL.SQLCode sql = new SQL.SQLCode();
                //sql.logData("deletebeatsegment", id.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void deleteCallBox(Guid id)
        {
            try {
                BeatData.CallBoxes.deleteCallBoxPolygonData(id);
                SQL.SQLCode sql = new SQL.SQLCode();
                //sql.logData("deletecallbox", id.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void deleteDropSite(Guid id)
        {
            try {
                BeatData.DropSites.deleteDropSite(id);
                SQL.SQLCode sql = new SQL.SQLCode();
                //sql.logData("deletedropsite", id.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void deleteYard(Guid id)
        {
            try {
                BeatData.Yards.deleteYard(id);
                SQL.SQLCode sql = new SQL.SQLCode();
                //sql.logData("deletedyard", id.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        #region  Waze Stuff

        public void setConfidence(int val, string type) {
            switch (type.ToUpper()) { 
                case "CONFIDENCE":
                    WAZE.GetWAZE.setMinConfidence(val);
                    break;
                case "RELIABILITY":
                    WAZE.GetWAZE.setMinReliability(val);
                    break;
                case "NUMTHUMBSUP":
                    WAZE.GetWAZE.setMinNumThumbsUp(val);
                    break;
            }
        }

        public int getConfidence(string type) {
            switch (type.ToUpper())
            {
                case "CONFIDENCE":
                    return WAZE.GetWAZE.minConfidence;
                case "RELIABILITY":
                    return WAZE.GetWAZE.minReliability;
                case "NUMTHUMBSUP":
                    return WAZE.GetWAZE.minNumThumbsUp;
                default:
                    return -1;
            }
        }

        public void addWAZE(string uuid, string title, double lat, double lon, int nThumbsUp, int confidence, int reliability, string street) {
            WAZE.wazeXML x = new WAZE.wazeXML();
            x.title = title;
            x.uuid = uuid;
            x.lat = lat;
            x.lon = lon;
            string segment = BeatData.BeatSegments.findBeatSeg(x.lat, x.lon);
            if (segment != "NOT FOUND")
            {
                x.segment = segment;
            }
            x.pubDate = DateTime.Now;
            x.nThumbsUp = nThumbsUp;
            x.confidence = confidence;
            x.reliability = reliability;
            x.street = "TEST:" + street;
            x.type = "ACCIDENT";
            x.subtype = "ACCIDENT_MAJOR";
            WAZE.GetWAZE.addWazeData(x);
        }

        public string getActiveBeats() {
            return WAZE.GetWAZE.getActiveBeats();
        }

        public void setActiveBeats(string beats) {
            WAZE.GetWAZE.setActiveBeats(beats);
        }

        #endregion

        public List<RunStatus> getTruckRunStatus(string truckNumber)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            List<RunStatus> runs = mySQL.getRunStatus(truckNumber);
            return runs;
        }

        public void UpdateVar(string varName, string varValue)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.SetVarValue(varName, varValue);
            mySQL.LoadLeeways();
        }

        public void rebootTruck(string IPAddress)
        {
            string msg = "<Reboot>";
            msg += "<Id>" + MakeMsgID() + "</Id>";
            msg += "</Reboot>";
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(IPAddress);
            if (thisTruck != null)
            {
                thisTruck.SendMessage(msg);
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

        public string getTruck(string truckNumber) {
            TowTruck.TowTruck t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find) { return find.TruckNumber == truckNumber; });
            if (t != null)
            {
                //JavaScriptSerializer js = new JavaScriptSerializer();
                string tData = "HI!";
                return tData;
            }
            else {
                return "not found";
            }
        }

        public List<MTCIncidentScreenData> GetAllIncidents()
        {
            
            List<MTCIncidentScreenData> incidents = new List<MTCIncidentScreenData>();
            /* Old way
            var iList = from i in DataClasses.GlobalData.Incidents
                        join v in DataClasses.GlobalData.currentTrucks on i.IPAddr equals v.Identifier
                        select new { i.preAssist.Beat, v.Driver.callSign, v.TruckNumber, i.preAssist.DispatchCode, i.preAssist.Comment, i.DatePosted, v.Driver.LastName, v.Driver.FirstName, v.tts.currentStatus, v.beatNumber, i.assistList, i.Canceled, i.Acked, i.IncidentID, v.Extended.ContractorName, i.preAssist.CHPIncidentType, i.preAssist.FSPLocation};
            foreach (var group in iList)
            {
                MTCIncidentScreenData i = new MTCIncidentScreenData();
                i.IncidentID = group.IncidentID.ToString();
                i.Beat = group.beatNumber;
                i.CallSign = group.callSign;
                i.TruckNumber = group.TruckNumber;
                i.Driver = group.LastName + ", " + group.FirstName;
                i.DispatchSummaryMessage = group.Comment;
                i.ContractorName = group.ContractorName;
                i.Time = group.DatePosted;
                i.DispatchNumber = group.DispatchCode;
                i.State = group.currentStatus;
                if (group.assistList.Count > 0 || group.Canceled == true)
                {
                    i.IsIncidentComplete = "Yes";
                }
                else
                {
                    i.IsIncidentComplete = "No";
                }
                i.isAcked = group.Acked.ToString();
                i.IncidentType = group.CHPIncidentType;
                i.Location = group.FSPLocation;
                incidents.Add(i);
            }
             */
            // new way
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks) {
                if (t.currentIncident != null) {
                    MTCIncidentScreenData i = new MTCIncidentScreenData();
                    i.IncidentID = t.currentIncident.incidentID.ToString();
                    i.Beat = t.currentIncident.beat;
                    i.CallSign = t.currentIncident.callSign;
                    i.TruckNumber = t.TruckNumber;
                    i.Driver = t.currentIncident.driverLastName + ", " + t.currentIncident.driverFirstName;
                    i.DispatchSummaryMessage = t.currentIncident.comment;
                    i.ContractorName = t.Extended.ContractorName;
                    i.Time = (DateTime)t.currentIncident.incidentDatePosted;
                    i.DispatchNumber = t.currentIncident.chpIncidentType;
                    i.State = t.Status.VehicleStatus;
                    i.IsIncidentComplete = "No";
                    i.isAcked = t.currentIncident.acked.ToString();
                    i.IncidentType = t.currentIncident.chpIncidentType;
                    i.Location = t.currentIncident.FSPLocation;
                    i.UserPosted = t.currentIncident.userPosted;
                    incidents.Add(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            List<MTCIncidentScreenData> sList = sql.getDailyIncidents();
            foreach (MTCIncidentScreenData d in sList) {
                incidents.Add(d);
            }
            return incidents;
        }

        public string findSeg(double lat, double lon)
        {
            string segName = BeatData.BeatSegments.findBeatSeg(lat, lon);
            return segName;
        }

        public string findBeat(double lat, double lon)
        {
            string beatName = BeatData.Beats.findBeat(lat, lon);
            return beatName;
        }

        public void addAssist(string UserName, string IPAddress, MTCPreAssistData data)
        {
            // old way
            /*
            MTCIncident mi = new MTCIncident();
            mi.assistList = new List<MTCAssist>();
            mi.DatePosted = DateTime.Now;
            mi.fromTruck = 0;
            mi.preAssist = data;
            mi.UserPosted = UserName;
            mi.Acked = false;
            mi.sentToTruck = false;
            mi.IPAddr = IPAddress;
            TowTruck.TowTruck found = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find) { return find.Identifier == IPAddress; });
            if (found != null)
            {
                mi.TruckNumber = found.TruckNumber;
            }
            else
            {
                mi.TruckNumber = IPAddress;
            }
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mi.IncidentID = mySQL.PostMTCIncident(UserName, mi.TruckNumber, "0", 0, 0.0, 0.0);
            DataClasses.GlobalData.addIncident(mi);
             */
            //new way
            TowTruck.TowTruck found = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck find)
            {
                return find.Identifier == IPAddress;
            });
            if(found != null){
                Incident i = new Incident();
                i.fromTruck = false;
                i.userPosted = UserName;
                i.direction = data.Direction;
                i.freeway = data.Freeway;
                i.chpIncidentType = data.DispatchCode;
                i.comment = data.Comment;
                i.crossStreet = data.CrossStreet;
                i.FSPLocation = data.FSPLocation;
                i.requestSent = true;
                if (found.currentIncident == null) {
                    found.addIncident(i);
                }
            }

        }

        public void SingleTruckDump(CopyTruck t)
        {
            try
            {
                TowTruck.TowTruck fTruck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck dTruck) { return dTruck.Identifier == t.Identifier; });
                if (fTruck == null)
                {
                    //if no data is being forwarded, the truck could be destroyed by the target service, we don't
                    //want to recreate one with null gps data.
                    /*
                    TowTruck.TowTruck newTruck = new TowTruck.TowTruck(t.Identifier);
                    newTruck.GPSPosition.Lat = 0;
                    newTruck.GPSPosition.Lon = 0;
                    DataClasses.GlobalData.currentTrucks.Add(newTruck);
                     * */
                }
                else
                {
                    //update truck status
                    fTruck.Status = new TowTruck.TowTruckStatus();
                    TowTruck.AlarmTimer alrmSpeeding = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "Speeding"; });
                    if (alrmSpeeding != null)
                    {
                        fTruck.Status.SpeedingAlarm = alrmSpeeding.hasAlarm;
                        fTruck.Status.SpeedingValue = alrmSpeeding.alarmValue;
                        fTruck.Status.SpeedingTime = alrmSpeeding.GetStartDateTime();
                    }
                    //fTruck.Status.SpeedingAlarm = t.Status.SpeedingAlarm;
                    //fTruck.Status.SpeedingValue = t.Status.SpeedingValue;
                    //fTruck.Status.SpeedingTime = t.Status.SpeedingTime;
                    fTruck.beatNumber = t.BeatNumber;
                    //fTruck.assignedBeat.BeatID = t.BeatID;
                    //fTruck.Status.SpeedingLocation = t.Status.SpeedingLocation;

                    TowTruck.AlarmTimer alrmOB = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "OffBeat"; });
                    if (alrmOB != null)
                    {
                        fTruck.Status.OutOfBoundsAlarm = alrmOB.hasAlarm;
                        fTruck.Status.OutOfBoundsMessage = alrmOB.alarmValue;
                        fTruck.Status.OutOfBoundsTime = alrmOB.GetStartDateTime();
                        fTruck.Status.OutOfBoundsStartTime = alrmOB.GetStartDateTime();
                    }

                    //TowTruck.StatusTimer stVS = fTruck.tts.s

                    fTruck.Status.VehicleStatus = fTruck.tts.currentStatus;
                    fTruck.Status.StatusStarted = fTruck.tts.statusStarted;

                    TowTruck.AlarmTimer alrmLogOn = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "LogOn"; });

                    /* probably not using Log On, Roll Out, Roll In or Log Off
                    fTruck.Status.LogOnAlarm = t.Status.LogOnAlarm;
                    fTruck.Status.LogOnAlarmTime = t.Status.LogOnAlarmTime;
                    fTruck.Status.LogOnAlarmCleared = t.Status.LogOnAlarmCleared;
                    fTruck.Status.LogOnAlarmExcused = t.Status.LogOnAlarmExcused;
                    fTruck.Status.LogOnAlarmComments = t.Status.LogOnAlarmComments;
                    fTruck.Status.RollOutAlarm = t.Status.RollOutAlarm;
                    fTruck.Status.RollOutAlarmTime = t.Status.RollOutAlarmTime;
                    fTruck.Status.RollOutAlarmCleared = t.Status.RollOutAlarmCleared;
                    fTruck.Status.RollOutAlarmExcused = t.Status.RollOutAlarmExcused;
                    fTruck.Status.RollOutAlarmComments = t.Status.RollOutAlarmComments;
                    fTruck.Status.OnPatrolAlarm = t.Status.OnPatrolAlarm;
                    fTruck.Status.OnPatrolAlarmTime = t.Status.OnPatrolAlarmTime;
                    fTruck.Status.OnPatrolAlarmCleared = t.Status.OnPatrolAlarmCleared;
                    fTruck.Status.OnPatrolAlarmExcused = t.Status.OnPatrolAlarmExcused;
                    fTruck.Status.OnPatrolAlarmComments = t.Status.OnPatrolAlarmComments;
                    fTruck.Status.RollInAlarm = t.Status.RollInAlarm;
                    fTruck.Status.RollInAlarmTime = t.Status.RollInAlarmTime;
                    fTruck.Status.RollInAlarmCleared = t.Status.RollInAlarmCleared;
                    fTruck.Status.RollInAlarmExcused = t.Status.RollInAlarmExcused;
                    fTruck.Status.RollInAlarmComments = t.Status.RollInAlarmComments;
                    fTruck.Status.LogOffAlarm = t.Status.LogOffAlarm;
                    fTruck.Status.LogOffAlarmTime = t.Status.LogOffAlarmTime;
                    fTruck.Status.LogOffAlarmCleared = t.Status.LogOffAlarmCleared;
                    fTruck.Status.LogOffAlarmExcused = t.Status.LogOffAlarmExcused;
                    fTruck.Status.LogOffAlarmComments = t.Status.LogOffAlarmComments;
                     * */

                    TowTruck.AlarmTimer alrmInc = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "LongIncident"; });
                    if (alrmInc != null)
                    {
                        fTruck.Status.IncidentAlarm = alrmInc.hasAlarm;
                        fTruck.Status.IncidentAlarmTime = alrmInc.GetStartDateTime();
                        fTruck.Status.IncidentAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.IncidentAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.IncidentAlarmComments = "NA";
                    }
                    /*
                    fTruck.Status.IncidentAlarm = t.Status.IncidentAlarm;
                    fTruck.Status.IncidentAlarmTime = t.Status.IncidentAlarmTime;
                    fTruck.Status.IncidentAlarmCleared = t.Status.IncidentAlarmCleared;
                    fTruck.Status.IncidentAlarmExcused = t.Status.IncidentAlarmExcused;
                    fTruck.Status.IncidentAlarmComments = t.Status.IncidentAlarmComments;
                     * */

                    TowTruck.AlarmTimer alrmGPS = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "GPSIssue"; });
                    if (alrmGPS != null)
                    {
                        fTruck.Status.GPSIssueAlarm = alrmGPS.hasAlarm;
                        fTruck.Status.GPSIssueAlarmStart = alrmGPS.GetStartDateTime();
                        fTruck.Status.GPSIssueAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.GPSIssueAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.GPSIssueAlarmComments = "NA";
                    }
                    /*
                    fTruck.Status.GPSIssueAlarm = t.Status.GPSIssueAlarm;  //handles NO GPS
                    fTruck.Status.GPSIssueAlarmStart = t.Status.GPSIssueAlarmStart; //handles NO GPS
                    fTruck.Status.GPSIssueAlarmCleared = t.Status.GPSIssueAlarmCleared;
                    fTruck.Status.GPSIssueAlarmExcused = t.Status.GPSIssueAlarmExcused;
                    fTruck.Status.GPSIssueAlarmComments = t.Status.GPSIssueAlarmComments;
                     * */

                    TowTruck.AlarmTimer alrmStat = fTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer a) { return a.alarmName == "Stationary"; });
                    if (alrmStat != null)
                    {
                        fTruck.Status.StationaryAlarm = alrmStat.hasAlarm;
                        fTruck.Status.StationaryAlarmStart = alrmStat.GetStartDateTime();
                        fTruck.Status.StationaryAlarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.StationaryAlarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                        fTruck.Status.StationaryAlarmComments = "NA";
                    }
                    /*
                    fTruck.Status.StationaryAlarm = t.Status.StationaryAlarm; //handles no movement, speed = 0
                    fTruck.Status.StationaryAlarmStart = t.Status.StationaryAlarmStart; //handles no movement, speed = 0
                    fTruck.Status.StationaryAlarmCleared = t.Status.StationaryAlarmCleared;
                    fTruck.Status.StationaryAlarmExcused = t.Status.StationaryAlarmExcused;
                    fTruck.Status.StationaryAlarmComments = t.Status.StationaryAlarmComments;
                     * */
                    //update driver
                    fTruck.Driver.DriverID = t.Driver.DriverID;
                    fTruck.Driver.LastName = t.Driver.LastName;
                    fTruck.Driver.FirstName = t.Driver.FirstName;
                    fTruck.Driver.TowTruckCompany = t.Driver.TowTruckCompany;
                    fTruck.Driver.FSPID = t.Driver.FSPID;
                    fTruck.Driver.AssignedBeat = t.Driver.AssignedBeat;
                    //fTruck.Driver.BeatScheduleID = new Guid();
                    fTruck.Driver.BreakStarted = t.Driver.BreakStarted;
                    fTruck.Driver.LunchStarted = t.Driver.LunchStarted;
                    //Extended data
                    fTruck.Extended.ContractorName = t.Extended.ContractorName;
                    fTruck.Extended.ContractorID = t.Extended.ContractorID;
                    fTruck.Extended.TruckNumber = t.Extended.TruckNumber;
                    fTruck.Extended.FleetNumber = t.Extended.FleetNumber;
                    fTruck.Extended.ProgramStartDate = t.Extended.ProgramStartDate;
                    fTruck.Extended.VehicleType = t.Extended.VehicleType;
                    fTruck.Extended.VehicleYear = t.Extended.VehicleYear;
                    fTruck.Extended.VehicleMake = t.Extended.VehicleMake;
                    fTruck.Extended.VehicleModel = t.Extended.VehicleModel;
                    fTruck.Extended.LicensePlate = t.Extended.LicensePlate;
                    fTruck.Extended.RegistrationExpireDate = t.Extended.RegistrationExpireDate;
                    fTruck.Extended.InsuranceExpireDate = t.Extended.InsuranceExpireDate;
                    fTruck.Extended.LastCHPInspection = t.Extended.LastCHPInspection;
                    fTruck.Extended.ProgramEndDate = t.Extended.ProgramEndDate;
                    fTruck.Extended.FAW = t.Extended.FAW;
                    fTruck.Extended.RAW = t.Extended.RAW;
                    fTruck.Extended.RAWR = t.Extended.RAWR;
                    fTruck.Extended.GVW = t.Extended.GVW;
                    fTruck.Extended.GVWR = t.Extended.GVWR;
                    fTruck.Extended.Wheelbase = t.Extended.Wheelbase;
                    fTruck.Extended.Overhang = t.Extended.Overhang;
                    fTruck.Extended.MAXTW = t.Extended.MAXTW;
                    fTruck.Extended.MAXTWCALCDATE = t.Extended.MAXTWCALCDATE;
                    fTruck.Extended.FuelType = t.Extended.FuelType;
                    fTruck.Extended.FleetVehicleID = t.Extended.FleetVehicleID;
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent("Error receiving single truck dump" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void TruckDump(List<CopyTruck> trucks)
        {
            try
            {
                foreach (CopyTruck t in trucks)
                {
                    TowTruck.TowTruck fTruck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck truck){return truck.Identifier == t.Identifier;});
                    if (fTruck == null)
                    {
                        //if no data is being forwarded, the truck could be destroyed by the target service, we don't
                        //want to recreate one with null gps data.
                        /*
                        TowTruck.TowTruck newTruck = new TowTruck.TowTruck(t.Identifier);
                        newTruck.GPSPosition.Lat = 0;
                        newTruck.GPSPosition.Lon = 0;
                        DataClasses.GlobalData.currentTrucks.Add(newTruck);
                         * */
                    }
                    else
                    {
                        //update truck status
                        fTruck.Status = new TowTruck.TowTruckStatus();
                        #region " old code "
                        /*
                        fTruck.Status.SpeedingAlarm = t.Status.SpeedingAlarm;
                        fTruck.Status.SpeedingValue = t.Status.SpeedingValue;
                        fTruck.Status.SpeedingTime = t.Status.SpeedingTime;
                        fTruck.assignedBeat.BeatNumber = t.BeatNumber;
                        fTruck.assignedBeat.BeatID = t.BeatID;
                        //fTruck.Status.SpeedingLocation = t.Status.SpeedingLocation;
                        fTruck.Status.OutOfBoundsAlarm = t.Status.OutOfBoundsAlarm;
                        fTruck.Status.OutOfBoundsMessage = t.Status.OutOfBoundsMessage;
                        fTruck.Status.OutOfBoundsTime = t.Status.OutOfBoundsTime;
                        fTruck.Status.OutOfBoundsStartTime = t.Status.OutOfBoundsStartTime;
                        fTruck.Status.VehicleStatus = t.Status.VehicleStatus;
                        fTruck.Status.StatusStarted = t.Status.StatusStarted;
                        fTruck.Status.LogOnAlarm = t.Status.LogOnAlarm;
                        fTruck.Status.LogOnAlarmTime = t.Status.LogOnAlarmTime;
                        fTruck.Status.LogOnAlarmCleared = t.Status.LogOnAlarmCleared;
                        fTruck.Status.LogOnAlarmExcused = t.Status.LogOnAlarmExcused;
                        fTruck.Status.LogOnAlarmComments = t.Status.LogOnAlarmComments;
                        fTruck.Status.RollOutAlarm = t.Status.RollOutAlarm;
                        fTruck.Status.RollOutAlarmTime = t.Status.RollOutAlarmTime;
                        fTruck.Status.RollOutAlarmCleared = t.Status.RollOutAlarmCleared;
                        fTruck.Status.RollOutAlarmExcused = t.Status.RollOutAlarmExcused;
                        fTruck.Status.RollOutAlarmComments = t.Status.RollOutAlarmComments;
                        fTruck.Status.OnPatrolAlarm = t.Status.OnPatrolAlarm;
                        fTruck.Status.OnPatrolAlarmTime = t.Status.OnPatrolAlarmTime;
                        fTruck.Status.OnPatrolAlarmCleared = t.Status.OnPatrolAlarmCleared;
                        fTruck.Status.OnPatrolAlarmExcused = t.Status.OnPatrolAlarmExcused;
                        fTruck.Status.OnPatrolAlarmComments = t.Status.OnPatrolAlarmComments;
                        fTruck.Status.RollInAlarm = t.Status.RollInAlarm;
                        fTruck.Status.RollInAlarmTime = t.Status.RollInAlarmTime;
                        fTruck.Status.RollInAlarmCleared = t.Status.RollInAlarmCleared;
                        fTruck.Status.RollInAlarmExcused = t.Status.RollInAlarmExcused;
                        fTruck.Status.RollInAlarmComments = t.Status.RollInAlarmComments;
                        fTruck.Status.LogOffAlarm = t.Status.LogOffAlarm;
                        fTruck.Status.LogOffAlarmTime = t.Status.LogOffAlarmTime;
                        fTruck.Status.LogOffAlarmCleared = t.Status.LogOffAlarmCleared;
                        fTruck.Status.LogOffAlarmExcused = t.Status.LogOffAlarmExcused;
                        fTruck.Status.LogOffAlarmComments = t.Status.LogOffAlarmComments;
                        fTruck.Status.IncidentAlarm = t.Status.IncidentAlarm;
                        fTruck.Status.IncidentAlarmTime = t.Status.IncidentAlarmTime;
                        fTruck.Status.IncidentAlarmCleared = t.Status.IncidentAlarmCleared;
                        fTruck.Status.IncidentAlarmExcused = t.Status.IncidentAlarmExcused;
                        fTruck.Status.IncidentAlarmComments = t.Status.IncidentAlarmComments;
                        fTruck.Status.GPSIssueAlarm = t.Status.GPSIssueAlarm;  //handles NO GPS
                        fTruck.Status.GPSIssueAlarmStart = t.Status.GPSIssueAlarmStart; //handles NO GPS
                        fTruck.Status.GPSIssueAlarmCleared = t.Status.GPSIssueAlarmCleared;
                        fTruck.Status.GPSIssueAlarmExcused = t.Status.GPSIssueAlarmExcused;
                        fTruck.Status.GPSIssueAlarmComments = t.Status.GPSIssueAlarmComments;
                        fTruck.Status.StationaryAlarm = t.Status.StationaryAlarm; //handles no movement, speed = 0
                        fTruck.Status.StationaryAlarmStart = t.Status.StationaryAlarmStart; //handles no movement, speed = 0
                        fTruck.Status.StationaryAlarmCleared = t.Status.StationaryAlarmCleared;
                        fTruck.Status.StationaryAlarmExcused = t.Status.StationaryAlarmExcused;
                        fTruck.Status.StationaryAlarmComments = t.Status.StationaryAlarmComments;
                        //update driver
                        fTruck.Driver.DriverID = t.Driver.DriverID;
                        fTruck.Driver.LastName = t.Driver.LastName;
                        fTruck.Driver.FirstName = t.Driver.FirstName;
                        fTruck.Driver.TowTruckCompany = t.Driver.TowTruckCompany;
                        fTruck.Driver.FSPID = t.Driver.FSPID;
                        fTruck.Driver.AssignedBeat = t.Driver.AssignedBeat;
                        fTruck.Driver.BeatScheduleID = t.Driver.BeatScheduleID;
                        fTruck.Driver.BreakStarted = t.Driver.BreakStarted;
                        fTruck.Driver.LunchStarted = t.Driver.LunchStarted;
                        //Extended data
                        fTruck.Extended.ContractorName = t.Extended.ContractorName;
                        fTruck.Extended.ContractorID = t.Extended.ContractorID;
                        fTruck.Extended.TruckNumber = t.Extended.TruckNumber;
                        fTruck.Extended.FleetNumber = t.Extended.FleetNumber;
                        fTruck.Extended.ProgramStartDate = t.Extended.ProgramStartDate;
                        fTruck.Extended.VehicleType = t.Extended.VehicleType;
                        fTruck.Extended.VehicleYear = t.Extended.VehicleYear;
                        fTruck.Extended.VehicleMake = t.Extended.VehicleMake;
                        fTruck.Extended.VehicleModel = t.Extended.VehicleModel;
                        fTruck.Extended.LicensePlate = t.Extended.LicensePlate;
                        fTruck.Extended.RegistrationExpireDate = t.Extended.RegistrationExpireDate;
                        fTruck.Extended.InsuranceExpireDate = t.Extended.InsuranceExpireDate;
                        fTruck.Extended.LastCHPInspection = t.Extended.LastCHPInspection;
                        fTruck.Extended.ProgramEndDate = t.Extended.ProgramEndDate;
                        fTruck.Extended.FAW = t.Extended.FAW;
                        fTruck.Extended.RAW = t.Extended.RAW;
                        fTruck.Extended.RAWR = t.Extended.RAWR;
                        fTruck.Extended.GVW = t.Extended.GVW;
                        fTruck.Extended.GVWR = t.Extended.GVWR;
                        fTruck.Extended.Wheelbase = t.Extended.Wheelbase;
                        fTruck.Extended.Overhang = t.Extended.Overhang;
                        fTruck.Extended.MAXTW = t.Extended.MAXTW;
                        fTruck.Extended.MAXTWCALCDATE = t.Extended.MAXTWCALCDATE;
                        fTruck.Extended.FuelType = t.Extended.FuelType;
                        fTruck.Extended.FleetVehicleID = t.Extended.FleetVehicleID;
                         * */
                        #endregion


                    }
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent("Error receiving truck dump" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void TruckDump(string truckList)
        {
            try
            {
                JsonSerializer des = new JsonSerializer();
                
                //List<TowTruck.TowTruck> trucks = des.Deserialize<List<TowTruck.TowTruck>>(truckList);
                List<TowTruck.TowTruck> trucks = JsonConvert.DeserializeObject<List<TowTruck.TowTruck>>(truckList);
                foreach (TowTruck.TowTruck t in trucks)
                {
                    TowTruck.TowTruck thisFoundTruck = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck.TowTruck truck) { return truck.Identifier == t.Identifier; });
                    if (thisFoundTruck == null)
                    {
                        DataClasses.GlobalData.currentTrucks.Add(t);
                    }
                    else
                    {
                        if (t.Status != null)
                        {
                            thisFoundTruck.Status = t.Status;
                        }
                        if (t.Extended != null)
                        {
                            thisFoundTruck.Extended = t.Extended;
                        }
                        if (t.Driver != null)
                        {
                            thisFoundTruck.Driver = t.Driver;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void SetSpeedingValue(int NewSpeed)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.SetVarValue("Speeding", NewSpeed.ToString());
            DataClasses.GlobalData.SpeedingLeeway = NewSpeed;
        }

        #region "OCTA find, set and remove incidents - DEPRECATED AND COMMENTED OUT "

        /*

        public List<IncidentScreenData> GetAllIncidents()
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            List<IncidentScreenData> AllIncidents = new List<IncidentScreenData>();
            //since we can have multiple assists per incident, we need to loop through incidents and assists associated with that incident to build the result set
            foreach (MiscData.Incident thisIncident in DataClasses.GlobalData.Incidents)
            {
                List<MiscData.Assist> theseAssists = DataClasses.GlobalData.GetAssistsByIncident(thisIncident.IncidentID);
                if (theseAssists.Count() > 0) //we have assists associated with incident
                {
                    foreach (MiscData.Assist thisAssist in theseAssists)
                    {
                        AllIncidents.Add(new IncidentScreenData {
                            IncidentID = thisIncident.IncidentID,
                            Direction = thisIncident.Direction,
                            Location = thisIncident.Location,
                            Freeway = DataClasses.GlobalData.FindFreewayNameByID(thisIncident.FreewayID),
                            TimeStamp = thisIncident.TimeStamp.ToString(),
                            CreatedBy = thisIncident.CreatedBy,
                            Description = thisIncident.Description,
                            IncidentNumber = thisIncident.IncidentNumber,
                            CrossStreet1 = thisIncident.CrossStreet1,
                            CrossStreet2 = thisIncident.CrossStreet2,
                            BeatNumber = thisIncident.BeatNumber,
                            TruckNumber = DataClasses.GlobalData.FindTruckNumberByID(thisAssist.FleetVehicleID),
                            Driver = mySQL.FindDriverNameByID(thisAssist.DriverID),
                            State = DataClasses.GlobalData.FindTowTruckStatusByID(thisAssist.FleetVehicleID),
                            ContractorName = DataClasses.GlobalData.FindContractorByTruckNumber(DataClasses.GlobalData.FindTruckNumberByID(thisAssist.FleetVehicleID))
                        });
                    }
                }
                else
                {
                    AllIncidents.Add(new IncidentScreenData {
                        IncidentID = thisIncident.IncidentID,
                        Direction = thisIncident.Direction,
                        Location = thisIncident.Location,
                        Freeway = DataClasses.GlobalData.FindFreewayNameByID(thisIncident.FreewayID),
                        TimeStamp = thisIncident.TimeStamp.ToString(),
                        CreatedBy = thisIncident.CreatedBy,
                        Description = thisIncident.Description,
                        IncidentNumber = thisIncident.IncidentNumber,
                        CrossStreet1 = thisIncident.CrossStreet1,
                        CrossStreet2 = thisIncident.CrossStreet2,
                        BeatNumber = thisIncident.BeatNumber,
                        TruckNumber = "No Assist Assigned",
                        Driver = "No Assist Assigned",
                        State = "No Assist Assigned"
                    });
                }

            }

            mySQL = null;
            return AllIncidents;
        }

        //public List<IncidentIn> GetIncidents()
        //{
        //    List<IncidentIn> AllIncidents = new List<IncidentIn>();
        //    foreach (MiscData.Incident thisIncident in DataClasses.GlobalData.Incidents)
        //    {
        //        AllIncidents.Add(new IncidentIn { 
        //            IncidentID = thisIncident.IncidentID,
        //            Location = thisIncident.Location,
        //            FreewayID = thisIncident.FreewayID,
        //            LocationID = thisIncident.LocationID,
        //            BeatNumber = thisIncident.BeatNumber,
        //            TimeStamp = thisIncident.TimeStamp,
        //            CreatedBy = thisIncident.CreatedBy,
        //            Description = thisIncident.Description,
        //            IncidentNumber = thisIncident.IncidentNumber
        //        });
        //    }
        //    return AllIncidents;
        //}

        public IncidentIn FindIncidentByID(Guid IncidentID)
        {
            MiscData.Incident thisIncident = DataClasses.GlobalData.FindIncidentByID(IncidentID);
            if (thisIncident == null)
            {
                return null;
            }
            else
            {
                IncidentIn thisIncidentIn = new IncidentIn();
                thisIncidentIn.IncidentID = thisIncident.IncidentID;
                thisIncidentIn.Location = thisIncident.Location;
                thisIncidentIn.FreewayID = thisIncident.FreewayID;
                thisIncidentIn.LocationID = thisIncident.LocationID;
                thisIncidentIn.BeatNumber = thisIncident.BeatNumber;
                thisIncidentIn.TimeStamp = thisIncident.TimeStamp;
                thisIncidentIn.Description = thisIncident.Description;
                thisIncidentIn.IncidentNumber = thisIncident.IncidentNumber;
                return thisIncidentIn;
            }
        }

        private string GenerateNumber(string Type)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            string NextDispatchNumber = mySQL.GetNextIncidentNumber().ToString();
            string Year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();
            while (Month.Length < 2)
            {
                Month = "0" + Month;
            }
            string Day = DateTime.Now.Day.ToString();
            while (Day.Length < 2)
            {
                Day = "0" + Day;
            }
            string DispatchNumber = Year + Month + Day + Type + NextDispatchNumber;
            return DispatchNumber;
        }

        public void AddIncident(IncidentIn thisIncidentIn)
        {

            //string DispatchNumber = GenerateNumber("i");

            MiscData.Incident thisIncident = new MiscData.Incident();
            thisIncident.Direction = thisIncidentIn.Direction;
            thisIncident.IncidentID = thisIncidentIn.IncidentID;
            thisIncident.Location = thisIncidentIn.Location;
            thisIncident.FreewayID = thisIncidentIn.FreewayID;
            thisIncident.LocationID = thisIncidentIn.LocationID;
            thisIncident.BeatNumber = thisIncidentIn.BeatNumber;
            thisIncident.TimeStamp = thisIncidentIn.TimeStamp;
            thisIncident.CreatedBy = thisIncidentIn.CreatedBy;
            thisIncident.Description = thisIncidentIn.Description;
            //thisIncident.IncidentNumber = thisIncidentIn.IncidentNumber;
            thisIncident.CrossStreet1 = thisIncidentIn.CrossStreet1;
            thisIncident.CrossStreet2 = thisIncidentIn.CrossStreet2;
            thisIncident.IncidentNumber = GenerateNumber("i");
            DataClasses.GlobalData.AddIncident(thisIncident);
        }

        public void ClearIncident(Guid IncidentID)
        {
            DataClasses.GlobalData.ClearIndicent(IncidentID);
        }

         * 
         */
        #endregion

        #region "OCTA find, set and remove assists - DEPRECATED AND COMMENTED OUT "

        /*
        public void AddAssist(AssistReq thisReq)
        {
            //string DispatchNumber = GenerateNumber("a");

            MiscData.Assist thisRequest = new MiscData.Assist();
            thisRequest.AssistID = thisReq.AssistID;
            thisRequest.AssistNumber = GenerateNumber("a");
            thisRequest.IncidentID = thisReq.IncidentID;
            thisRequest.FleetVehicleID = thisReq.FleetVehicleID;
            thisRequest.DispatchTime = thisReq.DispatchTime;
            //thisRequest.ServiceTypeID = thisReq.ServiceTypeID;
            thisRequest.DropZone = thisReq.DropZone;
            thisRequest.Make = thisReq.Make;
            thisRequest.VehicleTypeID = thisReq.VehicleTypeID;
            thisRequest.VehiclePositionID = thisReq.VehiclePositionID;
            thisRequest.Color = thisReq.Color;
            thisRequest.LicensePlate = thisReq.LicensePlate;
            thisRequest.State = thisReq.State;
            thisRequest.StartOD = thisReq.StartOD;
            thisRequest.EndOD = thisReq.EndOD;
            thisRequest.TowLocationID = thisReq.TowLocationID;
            thisRequest.Tip = thisReq.Tip;
            thisRequest.TipDetail = thisReq.TipDetail;
            thisRequest.CustomerLastName = thisReq.CustomerLastName;
            thisRequest.Comments = thisReq.Comments;
            thisRequest.IsMDC = thisReq.IsMDC;
            thisRequest.x1097 = thisReq.x1097;
            thisRequest.x1098 = thisReq.x1098;
            thisRequest.ContractorID = thisReq.ContractorID;
            thisRequest.LogNumber = thisReq.LogNumber;
            thisRequest.DriverID = FindDriverID(thisReq.FleetVehicleID);
            thisRequest.Lat = thisReq.Lat;
            thisRequest.Lon = thisRequest.Lon;
            thisRequest.AssistComplete = false;
            //Make sure incident has correct beat number
            DataClasses.GlobalData.UpdateIncidentBeat(thisReq.IncidentID, thisReq.FleetVehicleID);
            DataClasses.GlobalData.AddAssist(thisRequest);
        }

        private Guid FindDriverID(Guid TruckID)
        {
            Guid DriverID = new Guid("00000000-0000-0000-0000-000000000000");
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruckByTruckID(TruckID);
            if (thisTruck != null)
            {
                DriverID = thisTruck.Driver.DriverID;
            }
            return DriverID;
        }

        public List<AssistScreenData> GetAllAssists()
        {
            List<AssistScreenData> AllAssists = new List<AssistScreenData>();
            SQL.SQLCode mySQL = new SQL.SQLCode();
            string[] ServiceText;
            foreach (MiscData.Assist thisAssist in DataClasses.GlobalData.Assists)
            {
                if (thisAssist.SelectedServices != null && thisAssist.SelectedServices.Count() > 0)
                {
                    ServiceText = new string[thisAssist.SelectedServices.Count()];
                    for (int i = 0; i < thisAssist.SelectedServices.Count(); i++)
                    {
                        ServiceText[i] = DataClasses.GlobalData.FindServiceTypeNameByID(new Guid(thisAssist.SelectedServices[i].ToString()));
                    }
                }
                else
                {
                    ServiceText = new string[1];
                    ServiceText[0] = "No Services Rendered";
                }
                AllAssists.Add(new AssistScreenData { 
                    AssistID = thisAssist.AssistID,
                    DriverName = mySQL.FindDriverNameByID(thisAssist.DriverID),
                    DispatchNumber = DataClasses.GlobalData.FindIncidentNumberByID(thisAssist.IncidentID),
                    AssistNumber = thisAssist.AssistNumber,
                    IncidentNumber = DataClasses.GlobalData.FindIncidentNumberByID(thisAssist.IncidentID),
                    BeatNumber = DataClasses.GlobalData.FindBeatNumberByID(thisAssist.IncidentID),
                    VehicleNumber = DataClasses.GlobalData.FindTruckNumberByID(thisAssist.FleetVehicleID),
                    ContractorName = DataClasses.GlobalData.FindContractorNameByID(thisAssist.ContractorID),
                    x1097 = thisAssist.x1097,
                    OnSiteTime = thisAssist.OnSiteTime,
                    x0198 = thisAssist.x1098,
                    Comments = thisAssist.Comments,
                    Latitude = thisAssist.Lat,
                    Longitude = thisAssist.Lon,
                    CustomerWaitTime = thisAssist.CustomerWaitTime,
                    VehiclePosition = DataClasses.GlobalData.FindVehiclePositionNameByID(thisAssist.VehiclePositionID),
                    TrafficSpeed = DataClasses.GlobalData.FindTrafficSpeedNameByID(thisAssist.TrafficSpeedID),
                    DropZone = thisAssist.DropZone,
                    Make = thisAssist.Make,
                    VehicleType = DataClasses.GlobalData.FindVehicleTypeNameByID(thisAssist.VehicleTypeID),
                    Color = thisAssist.Color,
                    LicensePlate = thisAssist.LicensePlate,
                    State = DataClasses.GlobalData.FindTowTruckStatusByID(thisAssist.FleetVehicleID),
                    StartOD = thisAssist.StartOD,
                    EndOD = thisAssist.EndOD,
                    TowLocation = DataClasses.GlobalData.FindTowLocationNameByID(thisAssist.TowLocationID),
                    Tip = thisAssist.Tip,
                    TipDetail = thisAssist.TipDetail,
                    CustomerLastName = thisAssist.CustomerLastName,
                    SelectedServices = ServiceText,
                    AssistComplete = thisAssist.AssistComplete,
                    AssistAcked = thisAssist.Acked
                });
            }

            return AllAssists;
        }

        

        /*
        public void AddTruckAssistRequest(string IPAddress, AssistReq thisReq, Guid IncidentID)
        {
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(IPAddress);
            //TODO: Needs to be fixed
            if (thisTruck != null)
            {
                MiscData.Assist thisRequest = new MiscData.Assist();
                thisRequest.AssistID = thisReq.AssistID;
                thisRequest.IncidentID = thisReq.IncidentID;
                thisRequest.FleetVehicleID = thisReq.FleetVehicleID;
                thisRequest.DispatchTime = thisReq.DispatchTime;
                thisRequest.ServiceTypeID = thisReq.ServiceTypeID;
                thisRequest.DropZone = thisReq.DropZone;
                thisRequest.Make = thisReq.Make;
                thisRequest.VehicleTypeID = thisReq.VehicleTypeID;
                thisRequest.VehiclePositionID = thisReq.VehiclePositionID;
                thisRequest.Color = thisReq.Color;
                thisRequest.LicensePlate = thisReq.LicensePlate;
                thisRequest.State = thisReq.State;
                thisRequest.StartOD = thisReq.StartOD;
                thisRequest.EndOD = thisReq.EndOD;
                thisRequest.TowLocationID = thisReq.TowLocationID;
                thisRequest.Tip = thisReq.Tip;
                thisRequest.TipDetail = thisReq.TipDetail;
                thisRequest.CustomerLastName = thisReq.CustomerLastName;
                thisRequest.Comments = thisReq.Comments;
                thisRequest.IsMDC = thisReq.IsMDC;
                thisRequest.x1097 = thisReq.x1097;
                thisRequest.x1098 = thisReq.x1098;
                thisRequest.ContractorID = thisReq.ContractorID;
                thisRequest.LogNumber = thisReq.LogNumber;
                thisTruck.AddAssistRequest(thisRequest);

                DataClasses.GlobalData.AssignTruckToAssist(IncidentID, thisTruck);
            }
        }

        public void ClearTruckAssistRequest(string IPAddress, Guid AssistRequestID)
        {
            TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(IPAddress);
            if (thisTruck != null)
            {
                thisTruck.ClearAssistRequest(AssistRequestID);
            }
        }
        */
        #endregion

        public int GetUsedBreakTime(string DriverID, string Type)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            return mySQL.GetUsedBreakTime(DriverID, Type);
        }

        /* NOT USING THIS ANYMORE: THIS WAS OCTA INFORMATION
        public string[] GetPreloadedData(string Type)
        {
            string[] DataOut;
            int TypeCount = 0;
            switch (Type)
            {
                case "Code1098s":
                    TypeCount = DataClasses.GlobalData.Code1098s.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.Code1098s.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.Code1098s[i].CodeName.ToString();
                    }
                    break;
                case "Freeways":
                    TypeCount = DataClasses.GlobalData.Freeways.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.Freeways.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.Freeways[i].FreewayName.ToString();
                    }
                    break;
                case "IncidentTypes":
                    TypeCount = DataClasses.GlobalData.IncidentTypes.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.IncidentTypes.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.IncidentTypes[i].IncidentTypeCode.ToString();
                    }
                    break;
                case "LocationCodes":
                    TypeCount = DataClasses.GlobalData.LocationCodes.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.LocationCodes.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.LocationCodes[i].LocationCode.ToString();
                    }
                    break;
                case "ServiceTypes":
                    TypeCount = DataClasses.GlobalData.ServiceTypes.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.ServiceTypes.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.ServiceTypes[i].ServiceTypeCode.ToString();
                    }
                    break;
                case "TowLocations":
                    TypeCount = DataClasses.GlobalData.TowLocations.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.TowLocations.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.TowLocations[i].TowLocationCode.ToString();
                    }
                    break;
                case "TrafficSpeeds":
                    TypeCount = DataClasses.GlobalData.TrafficSpeeds.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.TrafficSpeeds.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.TrafficSpeeds[i].TrafficSpeedCode.ToString();
                    }
                    break;
                case "VehiclePositions":
                    TypeCount = DataClasses.GlobalData.VehiclePositions.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.VehiclePositions.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.VehiclePositions[i].VehiclePositionCode.ToString();
                    }
                    break;
                case "VehicleTypes":
                    TypeCount = DataClasses.GlobalData.VehicleTypes.Count;
                    DataOut = new string[TypeCount];
                    for (int i = 0; i < DataClasses.GlobalData.VehicleTypes.Count; i++)
                    {
                        DataOut[i] = DataClasses.GlobalData.VehicleTypes[i].VehicleTypeCode.ToString();
                    }
                    break;
                default:
                    DataOut = new string[1];
                    DataOut[0] = "NO DATA";
                    break;
            }
            return DataOut;
        }*/

        public List<AssistTruck> GetAssistTrucks()
        {
            List<AssistTruck> myTrucks = new List<AssistTruck>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                if (thisTruck.Driver.LastName != "Not Logged On")
                {
                    AssistTruck thisAssistTruck = new AssistTruck();
                    thisAssistTruck.TruckID = thisTruck.Extended.FleetVehicleID;
                    thisAssistTruck.TruckNumber = thisTruck.Extended.TruckNumber;
                    thisAssistTruck.ContractorID = thisTruck.Extended.ContractorID;
                    thisAssistTruck.ContractorName = thisTruck.Extended.ContractorName;
                    myTrucks.Add(thisAssistTruck);
                }
            }
            return myTrucks;
        }

        public List<beatData> GetBeatData()
        {
            return BeatData.Beats.beatData;
        }

        public List<beatData> GetBeatDataByBeat(string BeatNumber)
        {
            List<beatData> beats = new List<beatData>();
            var bList = from b in BeatData.Beats.beatData
                        where b.BeatNumber == BeatNumber
                        select b;
            foreach (beatData b in bList)
            {
                beats.Add(b);
            }
            return beats;
        }

        /* OLD OCTA INCIDENT CODE
        public List<IncidentDisplay> getIncidentData()
        {
            List<IncidentDisplay> idl = new List<IncidentDisplay>();
            SQL.SQLCode mySQL = new SQL.SQLCode();
            foreach (MiscData.Assist ta in DataClasses.GlobalData.Assists)
            {
                MiscData.Incident inc = DataClasses.GlobalData.FindIncidentByID(ta.IncidentID);
                TowTruck.TowTruck tt = DataClasses.GlobalData.FindTowTruckByTruckID(ta.FleetVehicleID);
                
                string State = "Not Connected";
                if (tt != null)
                {
                    State = tt.Status.VehicleStatus;
                }

                if (inc != null)
                {
                    IncidentDisplay id = new IncidentDisplay();
                    id.IncidentID = inc.IncidentID;
                    id.IncidentNumber = inc.IncidentNumber;
                    id.AssistNumber = ta.AssistNumber;
                    id.BeatNumber = inc.BeatNumber;
                    //id.TruckNumber = tt.TruckNumber;
                    //id.DriverName = tt.Driver.LastName + ", " + tt.Driver.FirstName;
                    id.TruckNumber = mySQL.GetTruckNumberByID(ta.FleetVehicleID);
                    id.DriverName = mySQL.FindDriverNameByID(ta.DriverID);
                    id.DispatchComments = inc.Description;
                    id.Timestamp = inc.TimeStamp;
                    id.DispatchNumber = inc.IncidentNumber;
                    //id.ContractorName = tt.Extended.ContractorName;
                    id.ContractorName = DataClasses.GlobalData.FindContractorNameByID(ta.ContractorID);
                    id.IsIncidentComplete = ta.AssistComplete;
                    id.State = State;
                    id.IsAcked = ta.Acked;
                    idl.Add(id);
                }
            }

            return idl;
        }
        */

        public List<TowTruckData> CurrentTrucks()
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

                myTrucks.Add(new TowTruckData {
                    TruckNumber = thisTruck.TruckNumber,
                    IPAddress = thisTruck.Identifier,
                    Heading = thisTruck.GPSPosition.Head,
                    Speed = thisTruck.GPSPosition.Speed,
                    Lat = thisTruck.GPSPosition.Lat,
                    Lon = thisTruck.GPSPosition.Lon,
                    Alarms = HasAlarms,
                    VehicleState = thisTruck.Status.VehicleStatus,
                    SpeedingAlarm = speedingAlarm,
                    SpeedingValue = speedingValue,
                    SpeedingTime = speedingTime,
                    OutOfBoundsAlarm = obAlarm,
                    OutOfBoundsMessage = obMessage,
                    OutOfBoundsTime = obTime,
                    CallSign = _CallSign,
                    LastMessage = thisTruck.LastMessage.LastMessageReceived,
                    ContractorName = thisTruck.Extended.ContractorName,
                    DriverName = _DriverName,
                    BeatNumber = thisTruck.beatNumber,
                    Location = thisTruck.location, //needs to show segment, description of segment
                    StatusStarted = thisTruck.Status.StatusStarted,
                    IsBackup = thisTruck.Extended.isBackup,
                    VehicleType = thisTruck.Extended.VehicleType
                });
            }
            /*
            var noDrv = from nd in DataClasses.GlobalData.currentTrucks
                        where nd.Driver.LastName == "Not Logged On"
                        select nd;
            foreach(TowTruck.TowTruck thisTruck in noDrv)
            {
                bool HasAlarms = false;
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }
                myTrucks.Add(new TowTruckData { 
                    TruckNumber = thisTruck.TruckNumber,
                    IPAddress = thisTruck.Identifier,
                    Direction = thisTruck.GPSPosition.Head.ToString(),
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
                    Heading = thisTruck.GPSPosition.Head,
                    LastMessage = thisTruck.LastMessage.LastMessageReceived,
                    ContractorName = thisTruck.Extended.ContractorName,
                    BeatNumber = new Guid("00000000-0000-0000-0000-000000000000")
                });
            }
            var Drv = from d in DataClasses.GlobalData.currentTrucks
                        where d.Driver.LastName != "Not Logged On"
                        select d;
            foreach (TowTruck.TowTruck thisTruck in Drv)
            {
                bool HasAlarms = false;
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }
                myTrucks.Add(new TowTruckData
                {
                    TruckNumber = thisTruck.TruckNumber,
                    IPAddress = thisTruck.Identifier,
                    Direction = thisTruck.GPSPosition.Head.ToString(),
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
                    Heading = thisTruck.GPSPosition.Head,
                    LastMessage = thisTruck.LastMessage.LastMessageReceived,
                    ContractorName = thisTruck.Extended.ContractorName,
                    BeatID = new Guid("00000000-0000-0000-0000-000000000000")
                });
            }
             * */
            return myTrucks;
        }

        public List<AlarmData> getAllAlarmData()
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            List<AlarmData> alarms = mySQL.getAllAlarms();
            return alarms;
        }

        public List<AlarmStatus> GetAllAlarms()
        {
            List<AlarmStatus> AllAlarms = new List<AlarmStatus>();
            try
            {
                var tList = from t in DataClasses.GlobalData.currentTrucks
                            where t.Status.VehicleStatus != "Waiting for Driver Login"
                            select t;

                foreach (TowTruck.TowTruck thisTruck in tList)
                {
                    //add long break, long lunch, overtime activity
                    TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                    TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });
                    TowTruck.AlarmTimer alarmOP = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOP) { return findOP.alarmName == "LateOnPatrol"; });
                    TowTruck.AlarmTimer alarmInc = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findInc) { return findInc.alarmName == "LongIncident"; });
                    TowTruck.AlarmTimer alarmSta = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSta) { return findSta.alarmName == "Stationary"; });
                    TowTruck.AlarmTimer alarmGPS = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findGPS) { return findGPS.alarmName == "GPSIssue"; });
                    TowTruck.AlarmTimer alarmEOS = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findEOS) { return findEOS.alarmName == "EarlyOutOfService"; });
                    TowTruck.AlarmTimer alarmLB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findLB) { return findLB.alarmName == "LongBreak"; });
                    TowTruck.AlarmTimer alarmLL = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findLL) { return findLL.alarmName == "LongLunch"; });
                    TowTruck.AlarmTimer alarmOA = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOA) { return findOA.alarmName == "OvertimeActivity"; });
                    
                    
                    bool _HasAlarms = false;
                    thisTruck.tta.checkAlarms();
                    if (thisTruck.tta.hasAlarms)
                    {
                        _HasAlarms = true;
                    }

                    #region " Speeding "

                    bool speedingAlarm = false;
                    Guid speedingAlarmID = new Guid();
                    string speedingValue = "NA";
                    DateTime speedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    int speedingDuration = 0;
                    if (alarmSpd != null)
                    {
                        speedingAlarm = alarmSpd.hasAlarm;
                        speedingAlarmID = alarmSpd._alarmID;
                        speedingValue = alarmSpd.alarmValue;
                        speedingTime = alarmSpd.alarmStart;
                        speedingDuration = alarmSpd.getStatusMinutes();
                    }

                    #endregion

                    #region " Off Beat "

                    bool obAlarm = false;
                    Guid obAlarmID = new Guid();
                    string obMessage = "NA";
                    DateTime obTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime obCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime obExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                    int obDuration = 0;
                    if (alarmOB != null)
                    {
                        obAlarm = alarmOB.hasAlarm;
                        obAlarmID = alarmOB._alarmID;
                        obMessage = alarmOB.alarmValue;
                        obTime = alarmOB.alarmStart;
                        obExcused = alarmOB.alarmExcused;
                        obCleared = alarmOB.alarmCleared;
                        obDuration = alarmOB.getStatusMinutes();
                    }

                    #endregion

                    #region " On Patrol "

                    bool opAlarm = false;
                    Guid opAlarmID = new Guid();
                    DateTime opTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime opCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime opExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                    string opComments = "NA";
                    int opDuration = 0; //this should always be 0, it just fires an alarm and quits

                    if (alarmOP != null)
                    {
                        opAlarm = alarmOP.hasAlarm;
                        opAlarmID = alarmOP._alarmID;
                        opTime = alarmOP.alarmStart;
                        opCleared = alarmOP.alarmCleared;
                        opExcused = alarmOP.alarmExcused;
                        opComments = alarmOP.alarmValue;
                        opDuration = alarmOP.getStatusMinutes();
                    }

                    #endregion

                    #region " Incident "

                    bool incAlarm = false;
                    Guid incAlarmID = new Guid();
                    DateTime incTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime incCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime incExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                    string incComments = "NA";
                    int incDuration = alarmInc.getStatusMinutes();

                    if (alarmInc != null)
                    {
                        incAlarm = alarmInc.hasAlarm;
                        incAlarmID = alarmInc._alarmID;
                        incTime = alarmInc.alarmStart;
                        incCleared = alarmInc.alarmCleared;
                        incExcused = alarmInc.alarmExcused;
                        incComments = alarmInc.alarmValue;
                        incDuration = alarmInc.getStatusMinutes();
                    }

                    #endregion

                    #region " GPS "

                    bool gpsAlarm = false;
                    Guid gpsAlarmID = new Guid();
                    DateTime gpsTime = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime gpsCleared = Convert.ToDateTime("01/01/2001 00:00:00");
                    DateTime gpsExcused = Convert.ToDateTime("01/01/2001 00:00:00");
                    string gpsComments = "NA";
                    int gpsDuration = 0;

                    if(alarmGPS != null)
                    {
                        gpsAlarm = alarmGPS.hasAlarm;
                        gpsAlarmID = alarmGPS._alarmID;
                        gpsTime = alarmGPS.alarmStart;
                        gpsCleared = alarmGPS.alarmCleared;
                        gpsExcused = alarmGPS.alarmExcused;
                        gpsComments = alarmGPS.alarmValue;
                        gpsDuration = alarmGPS.getStatusMinutes();
                    }

                    #endregion

                    #region " Stationary "

                    bool staAlarm = false;
                    Guid staAlarmID = new Guid();
                    DateTime staTime = alarmSta.GetStartDateTime();
                    DateTime staCleared = alarmSta.alarmCleared;
                    DateTime staExcused = alarmSta.alarmExcused;
                    string staComments = alarmSta.alarmValue;
                    int staDuration = alarmSta.getStatusMinutes();

                    if (alarmSta != null)
                    {
                        staAlarm = alarmSta.hasAlarm;
                        staAlarmID = alarmSta._alarmID;
                        staTime = alarmSta.alarmStart;
                        staCleared = alarmSta.alarmCleared;
                        staExcused = alarmSta.alarmExcused;
                        staComments = alarmSta.alarmValue;
                    }

                    #endregion

                    #region  " Early Roll In "

                    bool eosAlarm = alarmEOS.hasAlarm;
                        DateTime eosTime = alarmEOS.GetStartDateTime();
                        DateTime eosCleared = alarmEOS.alarmCleared;
                        DateTime eosExcused = alarmEOS.alarmExcused;
                        string eosComments = alarmEOS.alarmValue;
                        int eosDuration = 0; //this should also always be 0
                    
                    if (eosAlarm != null)
                    {
                        eosAlarm = alarmEOS.hasAlarm;
                        eosTime = alarmEOS.alarmStart;
                        eosCleared = alarmEOS.alarmCleared;
                        eosExcused = alarmEOS.alarmExcused;
                        eosComments = alarmEOS.alarmValue;
                        eosDuration = alarmEOS.getStatusMinutes();
                    }

                    #endregion

                    #region " Long Break "

                    bool lbAlarm = alarmLB.hasAlarm;
                    Guid lbAlarmID = alarmLB._alarmID;
                    DateTime lbTime = alarmLB.alarmStart;
                    DateTime lbCleared = alarmLB.alarmCleared;
                    DateTime lbExcused = alarmLB.alarmExcused;
                    string lbComments = alarmLB.alarmValue;
                    int lbDuration = alarmLB.getStatusMinutes();

                    #endregion

                    #region  " Long Lunch "

                    bool llAlarm = alarmLL.hasAlarm;
                    Guid llAlarmID = alarmLL._alarmID;
                    DateTime llTime = alarmLL.alarmStart;
                    DateTime llCleared = alarmLL.alarmCleared;
                    DateTime llExcused = alarmLL.alarmExcused;
                    string llComments = alarmLL.alarmValue;
                    int llDuration = alarmLL.getStatusMinutes();

                    #endregion

                    #region " Overtime Activity "

                    bool oaAlarm = alarmOA.hasAlarm;
                    Guid oaAlarmID = alarmOA._alarmID;
                    DateTime oaTime = alarmOA.alarmStart;
                    DateTime oaCleared = alarmOA.alarmCleared;
                    DateTime oaExcused = alarmOA.alarmExcused;
                    string oaComments = alarmOA.alarmValue;
                    int oaDuration = alarmOA.getStatusMinutes();

                    #endregion

                    AllAlarms.Add(new AlarmStatus
                    {
                        BeatNumber = thisTruck.beatNumber,
                        VehicleNumber = thisTruck.Extended.TruckNumber,
                        DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName,
                        CallSign = thisTruck.Driver.callSign,
                        ContractCompanyName = thisTruck.Extended.ContractorName,
                        HasAlarms = _HasAlarms,
                        SpeedingAlarm = speedingAlarm,
                        SpeedingAlarmID = speedingAlarmID,
                        SpeedingValue = speedingValue,
                        SpeedingTime = speedingTime,
                        SpeedingDuration = speedingDuration,
                        OutOfBoundsAlarm = obAlarm,
                        OutOfBoundsAlarmID = obAlarmID,
                        OutOfBoundsMessage = obMessage,
                        OutOfBoundsTime = obTime,
                        OutOfBoundsStartTime = obTime,
                        OutOfBoundsCleared = obCleared,
                        OutOfBoundsExcused = obExcused,
                        OutOfBoundsDuration = obDuration,
                        VehicleStatus = thisTruck.tts.currentStatus,
                        StatusStarted = thisTruck.tts.statusStarted,
                        /*
                        LogOnAlarm = thisTruck.Status.LogOnAlarm,
                        LogOnAlarmTime = thisTruck.Status.LogOnAlarmTime,
                        LogOnAlarmCleared = thisTruck.Status.LogOnAlarmCleared,
                        LogOnAlarmExcused = thisTruck.Status.LogOnAlarmCleared,
                        LogOnAlarmComments = thisTruck.Status.LogOnAlarmComments,
                        */
                        RollInAlarm = eosAlarm,
                        RollInAlarmTime = eosTime,
                        RollInAlarmCleared = eosCleared,
                        RollInAlarmExcused = eosExcused,
                        RollInAlarmComments = eosComments,
                        RollInAlarmDuration = eosDuration,
                        /*
                        RollOutAlarm = thisTruck.Status.RollOutAlarm,
                        RollOutAlarmTime = thisTruck.Status.RollOutAlarmTime,
                        RollOutAlarmCleared = thisTruck.Status.RollOutAlarmCleared,
                        RollOutAlarmExcused = thisTruck.Status.RollOutAlarmExcused,
                        RollOutAlarmComments = thisTruck.Status.RollOutAlarmComments,
                         *  * */
                        OnPatrolAlarm = opAlarm,
                        OnPatrolAlarmID = opAlarmID,
                        OnPatrolAlarmTime = opTime,
                        OnPatrolAlarmCleared = opCleared,
                        OnPatrolAlarmExcused = opExcused,
                        OnPatrolAlarmComments = opComments,
                        OnPatrolDuration = opDuration,
                        /*
                        LogOffAlarm = thisTruck.Status.LogOffAlarm,
                        LogOffAlarmTime = thisTruck.Status.LogOffAlarmTime,
                        LogOffAlarmCleared = thisTruck.Status.LogOffAlarmCleared,
                        LogOffAlarmExcused = thisTruck.Status.LogOffAlarmExcused,
                        LogOffAlarmComments = thisTruck.Status.LogOffAlarmComments,
                         * */
                        IncidentAlarm = incAlarm,
                        IncidentAlarmID = incAlarmID,
                        IncidentAlarmTime = incTime,
                        IncidentAlarmCleared = incCleared,
                        IncidentAlarmExcused = incExcused,
                        IncidentAlarmComments = incComments,
                        IncidentDuration = incDuration,
                        GPSIssueAlarm = gpsAlarm,
                        GPSIssueAlarmID = gpsAlarmID,
                        GPSIssueAlarmStart = gpsTime,
                        GPSIssueAlarmCleared = gpsCleared,
                        GPSIssueAlarmExcused = gpsExcused,
                        GPSIssueAlarmComments = gpsComments,
                        GPSIssueDuration = gpsDuration,
                        StationaryAlarm = staAlarm,
                        StationaryAlarmID = staAlarmID,
                        StationaryAlarmStart = staTime,
                        StationaryAlarmCleared = staCleared,
                        StationaryAlarmExcused = staExcused,
                        StationaryAlarmComments = staComments,
                        StationaryAlarmDuration = staDuration,
                        LongBreakAlarm = lbAlarm,
                        LongBreakAlarmID = lbAlarmID,
                        LongBreakAlarmStart = lbTime,
                        LongBreakAlarmCleared = lbCleared,
                        LongBreakAlarmExcused = lbExcused,
                        LongBreakAlarmComments = lbComments,
                        LongBreakDuration = lbDuration,
                        LongLunchAlarm = llAlarm,
                        LongLunchAlarmID = llAlarmID,
                        LongLunchAlarmStart = llTime,
                        LongLunchAlarmCleared = llCleared,
                        LongLunchAlarmExcused = llExcused,
                        LongLunchAlarmComments = llComments,
                        LongLunchDuration = llDuration,
                        OvertimeAlarm = oaAlarm,
                        OvertimeAlarmID = oaAlarmID,
                        OvertimeAlarmStart = oaTime,
                        OvertimeAlarmCleared = oaCleared,
                        OvertimeAlarmExcused = oaExcused,
                        OvertimeAlarmComments = oaComments,
                        OvertimeAlarmDuration = oaDuration
                    });
                }
                return AllAlarms;
            }
            catch(Exception ex)
            {
                string err = ex.ToString();
                return AllAlarms;
            }
        }

        public List<AlarmStatus> AlarmByTruck(string IPAddress)
        {
            try
            {

                List<AlarmStatus> thisAlarmStatus = new List<AlarmStatus>();
                TowTruck.TowTruck thisTruck = DataClasses.GlobalData.FindTowTruck(IPAddress);
                if (thisTruck != null)
                {
                    TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                    TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });
                    TowTruck.AlarmTimer alarmOP = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOP) { return findOP.alarmName == "LateOnPatrol"; });
                    TowTruck.AlarmTimer alarmInc = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findInc) { return findInc.alarmName == "Incident"; });
                    TowTruck.AlarmTimer alarmSta = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSta) { return findSta.alarmName == "OnPatrol"; });
                    TowTruck.AlarmTimer alarmGPS = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findGPS) { return findGPS.alarmName == "GPSIssue"; });
                    TowTruck.AlarmTimer alarmEOS = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findEOS) { return findEOS.alarmName == "EarlyOutOfService"; });

                    bool _HasAlarms = false;
                    thisTruck.tta.checkAlarms();
                    if (thisTruck.tta.hasAlarms)
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

                    #region " Roll In "

                    bool eosAlarm = false;
                    DateTime eosTime = alarmEOS.GetStartDateTime();
                    DateTime eosCleared = alarmEOS.alarmCleared;
                    DateTime eosExcused = alarmEOS.alarmExcused;
                    string eosComments = alarmEOS.alarmValue;

                    #endregion

                    thisAlarmStatus.Add(new AlarmStatus
                    {

                        BeatNumber = thisTruck.beatNumber.ToString(),
                        VehicleNumber = thisTruck.Extended.TruckNumber,
                        DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName,
                        ContractCompanyName = thisTruck.Extended.ContractorName,
                        CallSign = thisTruck.Driver.callSign,
                        HasAlarms = _HasAlarms,
                        SpeedingAlarm = speedingAlarm,
                        SpeedingValue = speedingValue,
                        SpeedingTime = speedingTime,
                        OutOfBoundsAlarm = obAlarm,
                        OutOfBoundsMessage = obMessage,
                        OutOfBoundsTime = obTime,
                        OutOfBoundsStartTime = obTime,
                        OutOfBoundsCleared = obCleared,
                        OutOfBoundsExcused = obExcused,
                        VehicleStatus = thisTruck.tts.currentStatus,
                        StatusStarted = thisTruck.tts.statusStarted,

                        OnPatrolAlarm = opAlarm,
                        OnPatrolAlarmTime = opTime,
                        OnPatrolAlarmCleared = opCleared,
                        OnPatrolAlarmExcused = opExcused,
                        OnPatrolAlarmComments = opComments,

                        RollInAlarm = eosAlarm,
                        RollInAlarmTime = eosTime,
                        RollInAlarmCleared = eosCleared,
                        RollInAlarmExcused = eosExcused,
                        RollInAlarmComments = eosComments,

                        IncidentAlarm = incAlarm,
                        IncidentAlarmTime = incTime,
                        IncidentAlarmCleared = incCleared,
                        IncidentAlarmExcused = incExcused,
                        IncidentAlarmComments = incComments,
                        GPSIssueAlarm = gpsAlarm,
                        GPSIssueAlarmStart = gpsTime,
                        GPSIssueAlarmCleared = gpsCleared,
                        GPSIssueAlarmExcused = gpsExcused,
                        GPSIssueAlarmComments = gpsComments,
                        StationaryAlarm = staAlarm,
                        StationaryAlarmStart = staTime,
                        StationaryAlarmCleared = staCleared,
                        StationaryAlarmExcused = staExcused,
                        StationaryAlarmComments = staComments

                        #region " Old Code "
                        
                        /*
                        BeatNumber = thisTruck.assignedBeat.BeatNumber.ToString(),
                        VehicleNumber = thisTruck.Extended.TruckNumber,
                        DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName,
                        ContractCompanyName = thisTruck.Extended.ContractorName,
                        SpeedingAlarm = thisTruck.Status.SpeedingAlarm,
                        SpeedingValue = thisTruck.Status.SpeedingValue,
                        SpeedingTime = thisTruck.Status.SpeedingTime,
                        OutOfBoundsAlarm = thisTruck.Status.OutOfBoundsAlarm,
                        OutOfBoundsMessage = thisTruck.Status.OutOfBoundsMessage,
                        OutOfBoundsTime = thisTruck.Status.OutOfBoundsTime,
                        OutOfBoundsStartTime = thisTruck.Status.OutOfBoundsStartTime,
                        VehicleStatus = thisTruck.Status.VehicleStatus,
                        StatusStarted = thisTruck.Status.StatusStarted,
                        LogOnAlarm = thisTruck.Status.LogOnAlarm,
                        LogOnAlarmTime = thisTruck.Status.LogOnAlarmTime,
                        LogOnAlarmCleared = thisTruck.Status.LogOnAlarmCleared,
                        LogOnAlarmExcused = thisTruck.Status.LogOnAlarmCleared,
                        LogOnAlarmComments = thisTruck.Status.LogOnAlarmComments,
                        RollInAlarm = thisTruck.Status.RollInAlarm,
                        RollInAlarmTime = thisTruck.Status.RollInAlarmTime,
                        RollInAlarmCleared = thisTruck.Status.RollInAlarmCleared,
                        RollInAlarmExcused = thisTruck.Status.RollInAlarmExcused,
                        RollInAlarmComments = thisTruck.Status.RollInAlarmComments,
                        RollOutAlarm = thisTruck.Status.RollOutAlarm,
                        RollOutAlarmTime = thisTruck.Status.RollOutAlarmTime,
                        RollOutAlarmCleared = thisTruck.Status.RollOutAlarmCleared,
                        RollOutAlarmExcused = thisTruck.Status.RollOutAlarmExcused,
                        RollOutAlarmComments = thisTruck.Status.RollOutAlarmComments,
                        OnPatrolAlarm = thisTruck.Status.OnPatrolAlarm,
                        OnPatrolAlarmTime = thisTruck.Status.OnPatrolAlarmTime,
                        OnPatrolAlarmCleared = thisTruck.Status.OnPatrolAlarmCleared,
                        OnPatrolAlarmExcused = thisTruck.Status.OnPatrolAlarmExcused,
                        OnPatrolAlarmComments = thisTruck.Status.OnPatrolAlarmComments,
                        LogOffAlarm = thisTruck.Status.LogOffAlarm,
                        LogOffAlarmTime = thisTruck.Status.LogOffAlarmTime,
                        LogOffAlarmCleared = thisTruck.Status.LogOffAlarmCleared,
                        LogOffAlarmExcused = thisTruck.Status.LogOffAlarmExcused,
                        LogOffAlarmComments = thisTruck.Status.LogOffAlarmComments,
                        IncidentAlarm = thisTruck.Status.IncidentAlarm,
                        IncidentAlarmTime = thisTruck.Status.IncidentAlarmTime,
                        IncidentAlarmCleared = thisTruck.Status.IncidentAlarmCleared,
                        IncidentAlarmExcused = thisTruck.Status.IncidentAlarmExcused,
                        IncidentAlarmComments = thisTruck.Status.IncidentAlarmComments,
                        GPSIssueAlarm = thisTruck.Status.GPSIssueAlarm,
                        GPSIssueAlarmStart = thisTruck.Status.GPSIssueAlarmStart,
                        GPSIssueAlarmCleared = thisTruck.Status.GPSIssueAlarmCleared,
                        GPSIssueAlarmExcused = thisTruck.Status.GPSIssueAlarmExcused,
                        GPSIssueAlarmComments = thisTruck.Status.GPSIssueAlarmComments,
                        StationaryAlarm = thisTruck.Status.StationaryAlarm,
                        StationaryAlarmStart = thisTruck.Status.StationaryAlarmStart,
                        StationaryAlarmCleared = thisTruck.Status.StationaryAlarmCleared,
                        StationaryAlarmExcused = thisTruck.Status.StationaryAlarmExcused,
                        StationaryAlarmComments = thisTruck.Status.StationaryAlarmComments
                        */
                        #endregion
                    });
                }
                return thisAlarmStatus;
            }
            catch
            {
                return null;
            }
        }

        public void UnexcuseAlarm(string _vehicleNumber, string _beatNumber, string _alarm, string _driverName, string _comments = "NO COMMENT")
        {
            try
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(_vehicleNumber);
                if (string.IsNullOrEmpty(_comments))
                {
                    _comments = "NO COMMENT";
                }
                if (t != null)
                {
                    switch (_alarm.ToUpper())
                    {
                        case "LOGON":
                            //t.Status.LogOnAlarm = false;
                            //t.Status.LogOnAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOnAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOnAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGON", _comments, _beatNumber);
                            break;
                        case "ROLLIN":
                            //t.Status.RollInAlarm = false;
                            //t.Status.RollInAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollInAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollInAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ROLL IN", _comments, _beatNumber);
                            break;
                        case "ROLLOUT":
                            //t.Status.RollOutAlarm = false;
                            //t.Status.RollOutAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollOutAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollOutAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ROLL OUT", _comments, _beatNumber);
                            break;
                        case "ONPATROL":
                            //t.Status.OnPatrolAlarm = false;
                            //t.Status.OnPatrolAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.OnPatrolAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.OnPatrolAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ON PATROL", _comments, _beatNumber);
                            break;
                        case "LOGOFF":
                            //t.Status.LogOffAlarm = false;
                            //t.Status.LogOffAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOffAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOffAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGOFF", _comments, _beatNumber);
                            break;
                        case "INCIDENT":
                            //t.Status.IncidentAlarm = false;
                            //t.Status.IncidentAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.IncidentAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.IncidentAlarmComments = _comments;
                            t.Status.StatusStarted = DateTime.Now;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "INCIDENT", _comments, _beatNumber);
                            break;
                        case "GPSISSUE": //handles NO GPS signal: 0,0
                            //t.Status.NoMotionAlarm = false;
                            //t.Status.NoMotionAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.GPSIssueAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.GPSIssueAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "GPS ISSUE", _comments, _beatNumber);
                            //t.Status.StatusStarted = DateTime.Now;
                            break;
                        case "STATIONARY": //handles NO MOVEMENT: speed = 0
                            //t.Status.StationaryAlarm = false;
                            //t.Status.StationaryAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.StationaryAlarmExcused = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.StationaryAlarmComments = _comments;
                            mySQL.UnExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "STATIONARY", _comments, _beatNumber);
                            break;
                    }
                }
                else
                {
                    //truck isn't currently in the system, so we need to do some lookups
                    Guid truckID = mySQL.GetTruckID(_vehicleNumber);
                    string[] splitter = _driverName.Split(',');
                    Guid driverID = mySQL.GetDriverID(splitter[0].ToString().Trim(), splitter[1].ToString().Trim());
                    if (truckID != new Guid("00000000-0000-0000-0000-000000000000") && driverID != new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        mySQL.UnExcuseAlarm(driverID, truckID, _alarm.ToUpper(), _comments, _beatNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void ExcuseAlarm(string _vehicleNumber, string _alarm, string _excuser, Guid _alertID, string _comments = "NO COMMENT")
        {
            try
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(_vehicleNumber);
                if (string.IsNullOrEmpty(_comments))
                {
                    _comments = "NO COMMENT";
                }
                if (t != null)
                {
                    switch (_alarm.ToUpper())
                    {
                        case "LONGBREAK":
                            TowTruck.AlarmTimer aLongBreak = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongBreak"; });
                            if (aLongBreak != null)
                            {
                                aLongBreak.alarmExcused = DateTime.Now;
                                aLongBreak.excusedBy = _excuser;
                                aLongBreak.comment = _comments;
                                aLongBreak.ExcuseAlarm(t.runID, aLongBreak._alarmID, _excuser, _comments);
                                aLongBreak.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "LongBreak", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "LONGLUNCH":
                            TowTruck.AlarmTimer aLongLunch = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongLunch"; });
                            if (aLongLunch != null)
                            {
                                aLongLunch.alarmExcused = DateTime.Now;
                                aLongLunch.excusedBy = _excuser;
                                aLongLunch.comment = _comments;
                                aLongLunch.ExcuseAlarm(t.runID, aLongLunch._alarmID, _excuser, _comments);
                                aLongLunch.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "LongLunch", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "STATIONARY": //handles NO MOVEMENT: speed = 0
                            TowTruck.AlarmTimer aStat = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Stationary"; });
                            if (aStat != null)
                            {
                                aStat.alarmExcused = DateTime.Now;
                                aStat.excusedBy = _excuser;
                                aStat.comment = _comments;
                                aStat.ExcuseAlarm(t.runID, aStat._alarmID, _excuser, _comments);
                                aStat.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "Stationary", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "OFFBEAT": //handles NO MOVEMENT: speed = 0
                            TowTruck.AlarmTimer aOB = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OffBeat"; });
                            if (aOB != null)
                            {
                                aOB.alarmExcused = DateTime.Now;
                                aOB.excusedBy = _excuser;
                                aOB.comment = _comments;
                                aOB.ExcuseAlarm(t.runID, aOB._alarmID, _excuser, _comments);
                                aOB.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "OffBeat", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "LATEONPATROL":
                            TowTruck.AlarmTimer aLatePatrol = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LateOnPatrol"; });
                            if (aLatePatrol != null)
                            {
                                aLatePatrol.alarmExcused = DateTime.Now;
                                aLatePatrol.excusedBy = _excuser;
                                aLatePatrol.comment = _comments;
                                aLatePatrol.ExcuseAlarm(t.runID, aLatePatrol._alarmID, _excuser, _comments);
                            }
                            break;
                        case "EARLYOUTOFSERVICE":
                            TowTruck.AlarmTimer aEarlyService = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "EarlyOutOfService"; });
                            if (aEarlyService != null)
                            {
                                aEarlyService.alarmExcused = DateTime.Now;
                                aEarlyService.excusedBy = _excuser;
                                aEarlyService.comment = _comments;
                                aEarlyService.ExcuseAlarm(t.runID, aEarlyService._alarmID, _excuser, _comments);
                            }
                            break;
                        case "SPEEDING": 
                            TowTruck.AlarmTimer aSpeed = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Speeding"; });
                            if (aSpeed != null)
                            {
                                aSpeed.alarmExcused = DateTime.Now;
                                aSpeed.excusedBy = _excuser;
                                aSpeed.comment = _comments;
                                aSpeed.ExcuseAlarm(t.runID, aSpeed._alarmID, _excuser, _comments);
                                aSpeed.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "Speeding", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "GPSISSUE": //handles NO GPS signal: 0,0
                            TowTruck.AlarmTimer aGPS = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "GPSIssue"; });
                            if (aGPS != null)
                            {
                                aGPS.alarmExcused = DateTime.Now;
                                aGPS.excusedBy = _excuser;
                                aGPS.comment = _comments;
                                aGPS.ExcuseAlarm(t.runID, aGPS._alarmID, _excuser, _comments);
                                aGPS.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "GPSIssue", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "LONGINCIDENT":
                            TowTruck.AlarmTimer aIncident = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongIncident"; });
                            if (aIncident != null)
                            {
                                aIncident.alarmExcused = DateTime.Now;
                                aIncident.excusedBy = _excuser;
                                aIncident.comment = _comments;
                                aIncident.ExcuseAlarm(t.runID, aIncident._alarmID, _excuser, _comments);
                                aIncident.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "LongIncident", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        case "OVERTIMEACTIVITY":
                            TowTruck.AlarmTimer aOvertime = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OvertimeActivity"; });
                            if (aOvertime != null)
                            {
                                aOvertime.alarmExcused = DateTime.Now;
                                aOvertime.excusedBy = _excuser;
                                aOvertime.comment = _comments;
                                aOvertime.ExcuseAlarm(t.runID, aOvertime._alarmID, _excuser, _comments);
                                aOvertime.StopAlarm(t.TruckNumber, t.Driver.FirstName + " " + t.Driver.LastName, t.Driver.TowTruckCompany, t.beatNumber, "LOvertimeActivity", t.GPSPosition.Lat,
                                    t.GPSPosition.Lon, t.runID, t.location, t.GPSPosition.Speed, t.GPSPosition.Head, t.Driver.callSign, t.Driver.schedule.scheduleID, t.Driver.schedule.ScheduleType);
                            }
                            break;
                        
                        #region " OCTA ALERTS NOT IN MTC "

                        case "LOGON": //not used in MTC
                            //t.Status.LogOnAlarm = false;
                            //t.Status.LogOnAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            //TowTruck.AlarmTimer a = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Logon"; });
                            //t.Status.LogOnAlarmExcused = DateTime.Now;
                            //t.Status.LogOnAlarmComments = _comments;
                            //mySQL.ExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGON", _comments, _beatNumber);
                            break;

                        case "LOGOFF": //not used in MTC
                            //t.Status.LogOffAlarm = false;
                            //t.Status.LogOffAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            //t.Status.LogOffAlarmExcused = DateTime.Now;
                            //t.Status.LogOffAlarmComments = _comments;
                            //mySQL.ExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGOFF", _comments, _beatNumber);
                            break;

                        case "ROLLOUT": //not in use in MTC
                            //t.Status.RollOutAlarm = false;
                            //t.Status.RollOutAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            //t.Status.RollOutAlarmExcused = DateTime.Now;
                            //t.Status.RollOutAlarmComments = _comments;
                            //mySQL.ExcuseAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ROLL OUT", _comments, _beatNumber);
                            break;

                        #endregion
                    }

                }
                else
                {
                    //truck isn't currently in the system, so we need to do some lookups
                    Guid runID = mySQL.GetRunID(_alertID);
                    if (runID != new Guid("00000000-0000-0000-0000-000000000000"))
                    {
                        mySQL.excuseAlarm(runID, _alertID, _excuser, _comments);
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void ClearAlarm(string _vehicleNumber, string _alarm, Guid _alertID)
        {
            try
            {
                SQL.SQLCode mySQL = new SQL.SQLCode();
                TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(_vehicleNumber);
                if (t != null)
                {
                    switch (_alarm.ToUpper())
                    {
                        case "LONGBREAK":
                            TowTruck.AlarmTimer aLongBreak = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongBreak"; });
                            if (aLongBreak != null)
                            {
                                aLongBreak.alarmCleared = DateTime.Now;
                                aLongBreak.clearAlarm(t.runID, aLongBreak._alarmID);
                            }
                            break;
                        case "LONGLUNCH":
                            TowTruck.AlarmTimer aLongLunch = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongLunch"; });
                            if (aLongLunch != null)
                            {
                                aLongLunch.alarmCleared = DateTime.Now;
                                aLongLunch.clearAlarm(t.runID, aLongLunch._alarmID);
                            }
                            break;
                        case "STATIONARY": //handles NO MOVEMENT: speed = 0
                            TowTruck.AlarmTimer aStat = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Stationary"; });
                            if (aStat != null)
                            {
                                aStat.alarmCleared = DateTime.Now;
                                aStat.clearAlarm(t.runID, aStat._alarmID);
                            }
                            break;
                        case "OFFBEAT": //handles NO MOVEMENT: speed = 0
                            TowTruck.AlarmTimer aOB = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OffBeat"; });
                            if (aOB != null)
                            {
                                aOB.alarmCleared = DateTime.Now;
                                aOB.clearAlarm(t.runID, aOB._alarmID);
                            }
                            break;
                        case "LATEONPATROL":
                            TowTruck.AlarmTimer aLatePatrol = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LateOnPatrol"; });
                            if (aLatePatrol != null)
                            {
                                aLatePatrol.alarmCleared = DateTime.Now;
                                aLatePatrol.clearAlarm(t.runID, aLatePatrol._alarmID);
                            }
                            break;
                        case "EARLYOUTOFSERVICE":
                            TowTruck.AlarmTimer aEarlyService = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "EarlyOutOfService"; });
                            if (aEarlyService != null)
                            {
                                aEarlyService.alarmCleared = DateTime.Now;
                                aEarlyService.clearAlarm(t.runID, aEarlyService._alarmID);
                            }
                            break;
                        case "SPEEDING":
                            TowTruck.AlarmTimer aSpeed = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "Speeding"; });
                            if (aSpeed != null)
                            {
                                aSpeed.alarmCleared = DateTime.Now;
                                aSpeed.clearAlarm(t.runID, aSpeed._alarmID);
                            }
                            break;
                        case "GPSISSUE": //handles NO GPS signal: 0,0
                            TowTruck.AlarmTimer aGPS = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "GPSIssue"; });
                            if (aGPS != null)
                            {
                                aGPS.alarmCleared = DateTime.Now;
                                aGPS.clearAlarm(t.runID, aGPS._alarmID);
                            }
                            break;
                        case "LONGINCIDENT":
                            TowTruck.AlarmTimer aIncident = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "LongIncident"; });
                            if (aIncident != null)
                            {
                                aIncident.alarmCleared = DateTime.Now;
                                aIncident.clearAlarm(t.runID, aIncident._alarmID);
                            }
                            break;
                        case "OVERTIMEACTIVITY":
                            TowTruck.AlarmTimer aOvertime = t.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer find) { return find.alarmName == "OvertimeActivity"; });
                            if (aOvertime != null)
                            {
                                aOvertime.alarmCleared = DateTime.Now;
                                aOvertime.clearAlarm(t.runID, aOvertime._alarmID);
                            }
                            break;
                        #region " OLD OCTA CODE "
                            /*
                        case "LOGON":
                            t.Status.LogOnAlarm = false;
                            t.Status.LogOnAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOnAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGON");
                            break;
                        case "ROLLIN":
                            t.Status.RollInAlarm = false;
                            t.Status.RollInAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollInAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ROLLIN");
                            break;
                        case "ROLLOUT":
                            t.Status.RollOutAlarm = false;
                            t.Status.RollOutAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.RollOutAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ROLLOUT");
                            break;
                        case "ONPATROL":
                            t.Status.OnPatrolAlarm = false;
                            t.Status.OnPatrolAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.OnPatrolAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "ONPATROL");
                            break;
                        case "LOGOFF":
                            t.Status.LogOffAlarm = false;
                            t.Status.LogOffAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.LogOffAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "LOGOFF");
                            break;
                        case "INCIDENT":
                            t.Status.IncidentAlarm = false;
                            t.Status.IncidentAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.IncidentAlarmCleared = DateTime.Now;
                            t.Status.StatusStarted = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "INCIDENT");
                            break;
                        case "GPSISSUE": //handles NO GPS signal: 0,0
                            t.Status.GPSIssueAlarm = false;
                            t.Status.GPSIssueAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.GPSIssueAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "GPSISSUE");
                            //t.Status.StatusStarted = DateTime.Now;
                            break;
                        case "STATIONARY": //handles NO MOVEMENT: speed = 0
                            t.Status.StationaryAlarm = false;
                            t.Status.StationaryAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.StationaryAlarmCleared = DateTime.Now;
                            mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "STATIONARY");
                            break;
                        case "OFFBEAT":
                            t.Status.OutOfBoundsAlarm = false;
                            t.Status.OutOfBoundsStartTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.OutOfBoundsTimeCleared = DateTime.Now;
                            //mySQL.ClearAlarm(t.Driver.DriverID, t.Extended.FleetVehicleID, "OFFBEAT");
                            break;
                             * */
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void ResetAlarm(string _vehicleNumber, string _alarm)
        {
            try
            {
                TowTruck.TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(_vehicleNumber);
                if (t != null)
                {
                    switch (_alarm.ToUpper())
                    {
                        case "LOGON":
                            t.Status.LogOnAlarm = false;
                            t.Status.LogOnAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "ROLLIN":
                            t.Status.RollInAlarm = false;
                            t.Status.RollInAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "ROLLOUT":
                            t.Status.RollOutAlarm = false;
                            t.Status.RollOutAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "ONPATROL":
                            t.Status.OnPatrolAlarm = false;
                            t.Status.OnPatrolAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "LOGOFF":
                            t.Status.LogOffAlarm = false;
                            t.Status.LogOffAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "INCIDENT":
                            t.Status.IncidentAlarm = false;
                            t.Status.IncidentAlarmTime = DateTime.Parse("01/01/2001 00:00:00");
                            t.Status.StatusStarted = DateTime.Now;
                            break;
                        case "GPSISSUE": //handles NO GPS signal: 0,0
                            t.Status.GPSIssueAlarm = false;
                            t.Status.GPSIssueAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            //t.Status.StatusStarted = DateTime.Now;
                            break;
                        case "STATIONARY": //handles NO MOVEMENT: speed = 0
                            t.Status.StationaryAlarm = false;
                            t.Status.StationaryAlarmStart = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                        case "OFFBEAT":
                            t.Status.OutOfBoundsAlarm = false;
                            t.Status.OutOfBoundsStartTime = DateTime.Parse("01/01/2001 00:00:00");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                string err = ex.ToString();
            }
        }

        public List<TruckMessage> GetAllMessages()
        {
            return DataClasses.GlobalData.theseMessages;
        }

        public void SendMessage(TruckMessage thisMessage)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            //string UserName = mySQL.FindUserNameByID(thisMessage.UserID);
            thisMessage.MessageText += Environment.NewLine + "FROM: " + thisMessage.UserEmail;
            string IPAddr = thisMessage.TruckIP;
            var tList = from t in DataClasses.GlobalData.currentTrucks
                        where t.Identifier == IPAddr
                        select t;
            foreach(TowTruck.TowTruck t in tList)
            {
                if (string.IsNullOrEmpty(thisMessage.Driver))
                {
                    thisMessage.Driver = t.Driver.LastName + ", " + t.Driver.FirstName;
                }
                if (string.IsNullOrEmpty(thisMessage.Beat))
                {
                    thisMessage.Beat = t.beatNumber;
                }
                if (string.IsNullOrEmpty(thisMessage.CallSign))
                {
                    thisMessage.CallSign = t.Driver.callSign;
                }
                if (string.IsNullOrEmpty(thisMessage.TruckNumber))
                {
                    thisMessage.TruckNumber = t.TruckNumber;
                }
            }
            //sanity check
            if (string.IsNullOrEmpty(thisMessage.Driver))
            {
                thisMessage.Driver = "NA";
            }
            if (string.IsNullOrEmpty(thisMessage.Beat))
            {
                thisMessage.Beat = "NA";
            }
            if (string.IsNullOrEmpty(thisMessage.CallSign))
            {
                thisMessage.CallSign = "NA";
            }
            if (string.IsNullOrEmpty(thisMessage.TruckNumber))
            {
                thisMessage.TruckNumber = "NA";
            }
            DataClasses.GlobalData.AddTruckMessage(thisMessage);
        }

        public List<ListDrivers> GetTruckDrivers()
        {
            List<ListDrivers> truckDrivers = new List<ListDrivers>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                truckDrivers.Add(new ListDrivers { 
                    TruckID = thisTruck.Extended.FleetVehicleID,
                    TruckNumber = thisTruck.TruckNumber,
                    DriverID = thisTruck.Driver.DriverID,
                    DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName,
                    ContractorName = thisTruck.Extended.ContractorName
                });
            }
            return truckDrivers;
        }

        public void LogOffDriver(Guid DriverID)
        {
            DataClasses.GlobalData.ForceDriverLogoff(DriverID);
        }
    }
}
