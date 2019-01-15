using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Timers;

namespace FPSService.WAZE
{
    public static class GetWAZE
    {
        #region pros and vars

        //expose feed
        //turn beats on and off for Waze - waze active beats List<string> beat numbers
        public static List<wazeXML> wazes = new List<wazeXML>();
        public static List<string> activeBeats = new List<string>();
        public static int minConfidence = 4;
        public static int minReliability = 1;
        public static int minNumThumbsUp = 1;
        private static string URIXML = "https://na-georss.waze.com/rtserver/web/TGeoRSS?tk=ccp_partner&ccp_partner_name=MTCCalifornia&format=XML&types=alerts,irregularities&polygon=-123.513794,38.753226;-121.788940,38.633178;-120.552979,36.755610;-121.865845,36.861164;-123.261108,37.155063;-123.354492,37.735100;-123.513794,38.753226;-123.513794,38.753226";
        private static List<string> valids = new List<string>();
        private static List<int> validRoads = new List<int>();

        public static void setMinConfidence(int val) {
            minConfidence = val;
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.SetVarValue("MinConfidence", val.ToString());
        }

        public static void setMinReliability(int val) {
            minReliability = val;
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.SetVarValue("MinReliability", val.ToString());
        }

        public static void setMinNumThumbsUp(int val) {
            minNumThumbsUp = val;
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.SetVarValue("MinThumbsUp", val.ToString());
        }

        private static void getMinReliability() {
            SQL.SQLCode sql = new SQL.SQLCode();
            minReliability = Convert.ToInt32(sql.GetVarValue("MinReliability"));
        }

        private static void getMinConfidence() {
            SQL.SQLCode sql = new SQL.SQLCode();
            minConfidence = Convert.ToInt32(sql.GetVarValue("MinConfidence"));
        }

        private static void getMinThumbsUp() {
            SQL.SQLCode sql = new SQL.SQLCode();
            minNumThumbsUp = Convert.ToInt32(sql.GetVarValue("MinThumbsUp"));
        }

        public static string getActiveBeats() {
            string beats = string.Empty;
            foreach (string s in activeBeats) {
                beats += s + ",";
            }
            beats = beats.Substring(0, beats.Length - 1);
            return beats;
        }

        public static void setActiveBeats(string beats) {
            string[] splitter = beats.Split(',');
            List<string> beatList = new List<string>();
            for (int i = 0; i < splitter.Count(); i++) {
                beatList.Add(splitter[i].ToString());
            }
            activeBeats = beatList;
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.setActiveBeats(beats);
        }

        #endregion

        //add timer

        private static void initWaze()
        {
            valids.Clear();
            validRoads.Clear();
            valids.Add("ACCIDENT_MAJOR");
            valids.Add("ACCIDENT_MINOR");
            valids.Add("HAZARD_ON_ROAD_CAR_STOPPED");
            valids.Add("HAZARD_ON_ROAD_OBJECT");
            valids.Add("HAZARD_ON_SHOULDER");
            valids.Add("HAZARD_ON_SHOULDER_CAR_STOPPED");
            validRoads.Add(1);
            validRoads.Add(2);
            validRoads.Add(3);
            validRoads.Add(4);
            validRoads.Add(6);
            validRoads.Add(7);
        }

        public static void startLooping() {
            if (valids.Count == 0 || validRoads.Count == 0) {
                initWaze();
            }
            Timer tmrLogTrucks = new Timer(60000);
            tmrLogTrucks.Elapsed += tmrLogTrucks_Elapsed;
            tmrLogTrucks.Enabled = true;
        }

        static void tmrLogTrucks_Elapsed(object sender, ElapsedEventArgs e)
        {
            readWazes();
        }

        public static void readWazes() {
            XDocument doc = XDocument.Load(URIXML);
            var matches = doc.Descendants("item");
            List<wazeXML> xList = new List<wazeXML>();
            foreach (var x in matches)
            {
                wazeXML w = new wazeXML();
                //int orderID = Convert.ToInt32(orderXML.Element("ID").Value);
                foreach (XElement xl in x.Elements())
                {
                    string name = xl.Name.ToString();
                    if (name.Contains("title"))
                    {
                        w.title = xl.Value;
                    }
                    if (name.Contains("type"))
                    {
                        w.type = xl.Value;
                    }
                    if (name.Contains("subtype"))
                    {
                        w.subtype = xl.Value;
                    }
                    if (name.Contains("pubDate"))
                    {
                        w.pubDate = makeDT(xl.Value);
                    }
                    if (name.Contains("uuid"))
                    {
                        w.uuid = xl.Value;
                    }
                    if (name.Contains("confidence"))
                    {
                        w.confidence = Convert.ToInt32(xl.Value);
                    }
                    if (name.Contains("point"))
                    {
                        w.lat = getLat(xl.Value);
                        w.lon = getLon(xl.Value);
                        string segment = BeatData.BeatSegments.findBeatSeg(w.lat, w.lon);
                        if (segment != "NOT FOUND")
                        {
                            w.segment = segment;
                        }
                    }
                    if (name.Contains("nThumbsUp"))
                    {
                        w.nThumbsUp = Convert.ToInt32(xl.Value);
                    }
                    if (name.Contains("reliability"))
                    {
                        w.reliability = Convert.ToInt32(xl.Value);
                    }
                    if (name.Contains("street"))
                    {
                        w.street = xl.Value;
                    }
                    if (name.Contains("city"))
                    {
                        w.city = xl.Value;
                    }
                    if (name.Contains("country"))
                    {
                        w.country = xl.Value;
                    }
                    if (name.Contains("roadType"))
                    {
                        w.roadType = Convert.ToInt32(xl.Value);
                    }
                    if (name.Contains("reportRating"))
                    {
                        w.reportRating = Convert.ToInt32(xl.Value);
                    }
                }
                if (valids.Contains(w.type) && DateTime.Now.AddMinutes(-90) < w.pubDate && validRoads.Contains(w.roadType)
                    && w.confidence >= minConfidence && w.reliability >= minReliability && !String.IsNullOrEmpty(w.segment))
                {
                    addWazeData(w);
                }

            }
            //checkSegs();
            clearBadWazes();
            checkForDispatches();
        }

        public static void addWazeData(wazeXML x)
        {
            wazeXML found = wazes.Find(delegate(wazeXML find) { return find.uuid == x.uuid; });
            if (found == null) //only add if we don't already have one with the same uuid
            {
                x.dispatched = false;
                x.acked = false;
                x.accepted = false;
                wazes.Add(x);
                SQL.SQLCode sql = new SQL.SQLCode();
                sql.logIncomingWaze(x);
            }
            else
            {
                
                found.city = x.city;
                found.confidence = x.confidence;
                found.country = x.country;
                found.lat = x.lat;
                found.lon = x.lon;
                found.nThumbsUp = x.nThumbsUp;
                found.pubDate = x.pubDate;
                found.reliability = x.reliability;
                found.reportRating = x.reportRating;
                found.roadType = x.roadType;
                found.segment = x.segment;
                found.street = x.street;
                found.subtype = x.subtype;
                found.title = x.title;
                found.type = x.type;
                found.uuid = x.uuid;
                
            }
        }

        private static void clearBadWazes()
        {
            for (int i = wazes.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(wazes[i].segment) || wazes[i].pubDate < DateTime.Now.AddMinutes(-90))
                {
                    wazes.RemoveAt(i);
                }
            }
        }

        private static void checkForDispatches() {
            foreach (wazeXML x in wazes) {
                if (x.dispatched == false) { 
                    //look for the beat by segment
                    beatInformation bi = (from b in BeatData.Beats.beatInfos
                                          where b.beatSegmentList.Contains(x.segment)
                                          select b).FirstOrDefault();
                    if (bi != null) { //we found a beat, find out if we have a vehicle
                        //check to see if beat is active. Inactive beats don't get messages
                        if (activeBeats.Contains(bi.BeatID)) {
                            List<truckDistance> tdList = new List<truckDistance>();
                            var tList = from trucks in DataClasses.GlobalData.currentTrucks
                                        where trucks.beatNumber == bi.BeatID && trucks.Status.VehicleStatus != "ON INCIDENT"
                                        select trucks;
                            foreach (TowTruck.TowTruck t in tList)
                            {
                                BeatData.Haversine h = new BeatData.Haversine();
                                BeatData.Position pW = new BeatData.Position();
                                pW.Latitude = x.lat;
                                pW.Longitude = x.lon;
                                BeatData.Position tW = new BeatData.Position();
                                tW.Latitude = t.GPSPosition.Lat;
                                tW.Longitude = t.GPSPosition.Lon;
                                double dist = h.Distance(pW, tW, BeatData.DistanceType.Miles);
                                truckDistance td = new truckDistance();
                                td.t = t;
                                td.distance = dist;
                                tdList.Add(td);
                            }

                            TowTruck.TowTruck ft = null;
                            if (tdList.Count > 0) {
                                tdList = tdList.OrderBy(t => t.distance).ToList<truckDistance>();
                                ft = tdList[0].t;
                            }

                            if (ft != null)
                            {
                                TruckMessage tm = new TruckMessage();
                                tm.Acked = false;
                                tm.Beat = bi.BeatID;
                                tm.CallSign = ft.Driver.callSign;
                                tm.Driver = ft.Driver.LastName + ", " + ft.Driver.FirstName;
                                tm.MessageID = Guid.NewGuid();
                                tm.MessageText = "You have a WAZE request for an assist. Please respond.";
                                tm.streetInformation = x.street;
                                tm.MessageType = 4;
                                tm.SentTime = DateTime.Now;
                                tm.TruckIP = ft.Identifier;
                                tm.TruckNumber = ft.TruckNumber;
                                tm.UserEmail = "WAZE DISPATCH";
                                tm.lat = x.lat;
                                tm.lon = x.lon;
                                tm.WazeUUID = x.uuid;
                                tm.WazeType = x.type;
                                tm.WazeSubType = x.subtype;
                                DataClasses.GlobalData.theseMessages.Add(tm);
                                x.dispatched = true;
                                x.dispatchVehicle = ft.TruckNumber;
                                x.dispatchedTime = DateTime.Now;

                                SQL.SQLCode sql = new SQL.SQLCode();
                                sql.dispatchWaze(x);
                            }
                        }
                    }
                }
            }
        }

        private static void sortWazes()
        {
            wazes = wazes.OrderBy(s => s.segment).ThenBy(s => s.pubDate).ThenBy(s => s.type).ToList<wazeXML>();
        }

        private static double getLat(string point)
        {
            string[] splitter = point.Split(' ');
            return Convert.ToDouble(splitter[0]);
        }

        private static double getLon(string point)
        {
            string[] splitter = point.Split(' ');
            return Convert.ToDouble(splitter[1]);
        }

        private static DateTime makeDT(string val)
        {
            string[] splitter = val.Split(' ');
            string mo = makeMonth(splitter[1]).ToString();
            string day = splitter[2];
            string year = splitter[5];
            string time = splitter[3];
            return Convert.ToDateTime(mo + "/" + day + "/" + year + " " + time).AddHours(-7);
        }

        private static int makeMonth(string mo)
        {
            switch (mo.ToUpper())
            {
                case "JAN":
                    return 1;
                case "FEB":
                    return 2;
                case "MAR":
                    return 3;
                case "APR":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                    return 6;
                case "JUL":
                    return 7;
                case "AUG":
                    return 8;
                case "SEP":
                    return 9;
                case "OCT":
                    return 10;
                case "NOV":
                    return 11;
                case "DEC":
                    return 12;
                default:
                    return 0;
            }
        }
    }

    public class truckDistance {
        public TowTruck.TowTruck t { get; set; }
        public double distance { get; set; }
    }
}