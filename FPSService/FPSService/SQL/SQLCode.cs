using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.SqlServer.Types;

namespace FPSService.SQL
{
    public class SQLCode
    {
        string ConnStr;
        string ConnBeat;
        string ConnPlayback;
        Logging.EventLogger logger;

        public SQLCode()
        {
            ConnStr = ConfigurationManager.AppSettings["FSPdb"].ToString();
            ConnBeat = ConfigurationManager.AppSettings["BeatDB"].ToString();
            ConnPlayback = ConfigurationManager.AppSettings["PlaybackDB"].ToString();
        }

        #region " Playback Logging "

        public void dumpPlaybackData(List<MiscData.dataDump> dump)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnPlayback))
                {
                    conn.Open();

                    string SQL = "LogPlaybackData";
                    foreach (MiscData.dataDump d in dump)
                    {
                        SqlCommand cmd = new SqlCommand(SQL, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TimeStamp", d.timeStamp);
                        cmd.Parameters.AddWithValue("@TruckNumber", d.TruckNumber);
                        cmd.Parameters.AddWithValue("@Driver", d.Driver);
                        cmd.Parameters.AddWithValue("@CallSign", d.CallSign);
                        cmd.Parameters.AddWithValue("@Contractor", d.Contractor);
                        cmd.Parameters.AddWithValue("@Status", d.Status);
                        cmd.Parameters.AddWithValue("@Schedule", d.Schedule);
                        cmd.Parameters.AddWithValue("@Beat", d.Beat);
                        cmd.Parameters.AddWithValue("@BeatSegment", d.BeatSegment);
                        cmd.Parameters.AddWithValue("@Speed", d.Speed);
                        cmd.Parameters.AddWithValue("@Lat", d.Lat);
                        cmd.Parameters.AddWithValue("@Lon", d.Lon);
                        cmd.Parameters.AddWithValue("@Heading", d.Heading);
                        cmd.Parameters.AddWithValue("@HasAlarm", d.HasAlarm);
                        cmd.Parameters.AddWithValue("@AlarmInfo", d.AlarmInfo);
                        cmd.Parameters.AddWithValue("@RunID", d.RunID);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error Dumping Playback Data " + Environment.NewLine + ex.ToString(), true);
            }
        }

        #endregion

        #region " SQL Reads "

        #region " One-time loads, execute at start of service"

        public void loadBeatData()
        { //new beats loaded from db
            try
            {
                BeatData.Beats2.beatList.Clear();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT ID, BeatID, Notes, BoundaryData, PointData, GeoType, Radius FROM BeatData ORDER BY BeatID";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.beatPolygonData b = new BeatData.beatPolygonData();
                        b.ID = new Guid(rdr["ID"].ToString());
                        b.BeatID = rdr["BeatID"].ToString();
                        b.notes = rdr["Notes"].ToString();
                        string[] splitBoundary = rdr["BoundaryData"].ToString().Split(',');
                        if (splitBoundary.Length > 0)
                        {
                            b.maxLat = Convert.ToDouble(splitBoundary[0]);
                            b.maxLon = Convert.ToDouble(splitBoundary[1]);
                            b.minLat = Convert.ToDouble(splitBoundary[2]);
                            b.minLon = Convert.ToDouble(splitBoundary[3]);
                        }
                        else
                        {

                            b.maxLat = 0.0;
                            b.maxLon = 0.0;
                            b.minLat = 0.0;
                            b.minLon = 0.0;
                        }
                        string[] splitPointData = rdr["PointData"].ToString().Split('|');
                        b.geoFence = new List<BeatData.latLon>();
                        for (int i = 0; i < splitPointData.Count(); i++)
                        {
                            string[] splitLatLon = splitPointData[i].Split(',');
                            BeatData.latLon ll = new BeatData.latLon();
                            ll.lat = Convert.ToDouble(splitLatLon[0]);
                            ll.lon = Convert.ToDouble(splitLatLon[1]);
                            b.geoFence.Add(ll);
                        }
                        b.geoType = rdr["GeoType"].ToString();
                        b.radius = Convert.ToDouble(rdr["Radius"]);

                        BeatData.Beats2.addBeatPolygonData(b);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadBeatSegmentData()
        {
            try
            {
                BeatData.BeatSegments.bsPolyList.Clear();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT ID, BeatID, SegmentID, SegmentDescription, BoundaryData, PointData FROM BeatSegmentData ORDER BY BeatID, SegmentID";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.beatSegmentPolygonData b = new BeatData.beatSegmentPolygonData();
                        b.ID = new Guid(rdr["ID"].ToString());
                        b.beatID = rdr["BeatID"].ToString();
                        b.segmentID = rdr["SegmentID"].ToString();
                        b.segmentDescription = rdr["SegmentDescription"].ToString();
                        string[] splitBoundary = rdr["BoundaryData"].ToString().Split(',');
                        if (splitBoundary.Length > 0)
                        {
                            b.maxLat = Convert.ToDouble(splitBoundary[0]);
                            b.maxLon = Convert.ToDouble(splitBoundary[1]);
                            b.minLat = Convert.ToDouble(splitBoundary[2]);
                            b.minLon = Convert.ToDouble(splitBoundary[3]);
                        }
                        else
                        {

                            b.maxLat = 0.0;
                            b.maxLon = 0.0;
                            b.minLat = 0.0;
                            b.minLon = 0.0;
                        }
                        string[] splitPointData = rdr["PointData"].ToString().Split('|');
                        b.geoFence = new List<BeatData.latLon>();
                        for (int i = 0; i < splitPointData.Count(); i++)
                        {
                            string[] splitLatLon = splitPointData[i].Split(',');
                            BeatData.latLon ll = new BeatData.latLon();
                            ll.lat = Convert.ToDouble(splitLatLon[0]);
                            ll.lon = Convert.ToDouble(splitLatLon[1]);
                            b.geoFence.Add(ll);
                        }
                        BeatData.BeatSegments.bsPolyList.Add(b);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadDropSiteData()
        {
            Guid dsID = new Guid();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT ID, BeatID, DropSiteID, DropSiteDescription, BoundaryData, PointData FROM DropSiteData ORDER BY BeatID, DropSiteID";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.dropSitePolygonData d = new BeatData.dropSitePolygonData();
                        d.ID = new Guid(rdr["ID"].ToString());
                        dsID = d.ID;
                        d.dropSiteID = rdr["DropSiteID"].ToString();
                        d.dropSiteDescription = rdr["DropSiteDescription"].ToString();
                        string[] splitBoundary = rdr["BoundaryData"].ToString().Split(',');
                        if (splitBoundary.Length > 0)
                        {
                            d.maxLat = Convert.ToDouble(splitBoundary[0]);
                            d.maxLon = Convert.ToDouble(splitBoundary[1]);
                            d.minLat = Convert.ToDouble(splitBoundary[2]);
                            d.minLon = Convert.ToDouble(splitBoundary[3]);
                        }
                        else
                        {

                            d.maxLat = 0.0;
                            d.maxLon = 0.0;
                            d.minLat = 0.0;
                            d.minLon = 0.0;
                        }
                        string[] splitPointData = rdr["PointData"].ToString().Split('|');
                        d.geoFence = new List<BeatData.latLon>();
                        for (int i = 0; i < splitPointData.Count(); i++)
                        {
                            string[] splitLatLon = splitPointData[i].Split(',');
                            BeatData.latLon ll = new BeatData.latLon();
                            ll.lat = Convert.ToDouble(splitLatLon[0]);
                            ll.lon = Convert.ToDouble(splitLatLon[1]);
                            d.geoFence.Add(ll);
                        }
                        BeatData.DropSites.addDropSite(d);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = dsID.ToString();
                throw new Exception(ex.ToString());
            }
        }

        public void loadYardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT ID, YardID, BeatID, Contractor, Address, City, Zip, Phone, BoundaryData, PointData FROM YardData ORDER BY BeatID, YardID";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.yardPolygonData y = new BeatData.yardPolygonData();
                        y.ID = new Guid(rdr["ID"].ToString());
                        y.YardID = rdr["YardID"].ToString();
                        y.BeatID = rdr["BeatID"].ToString();
                        y.Contractor = rdr["Contractor"].ToString();
                        y.Address = rdr["Address"].ToString();
                        y.City = rdr["City"].ToString();
                        y.Phone = rdr["Phone"].ToString();
                        string[] splitBoundary = rdr["BoundaryData"].ToString().Split(',');
                        if (splitBoundary.Length > 0)
                        {
                            y.maxLat = Convert.ToDouble(splitBoundary[0]);
                            y.maxLon = Convert.ToDouble(splitBoundary[1]);
                            y.minLat = Convert.ToDouble(splitBoundary[2]);
                            y.minLon = Convert.ToDouble(splitBoundary[3]);
                        }
                        else
                        {

                            y.maxLat = 0.0;
                            y.maxLon = 0.0;
                            y.minLat = 0.0;
                            y.minLon = 0.0;
                        }

                        string[] splitPointData = rdr["PointData"].ToString().Split('|');
                        y.geoFence = new List<BeatData.latLon>();
                        for (int i = 0; i < splitPointData.Count(); i++)
                        {
                            string[] splitLatLon = splitPointData[i].Split(',');
                            BeatData.latLon ll = new BeatData.latLon();
                            ll.lat = Convert.ToDouble(splitLatLon[0]);
                            ll.lon = Convert.ToDouble(splitLatLon[1]);
                            y.geoFence.Add(ll);
                        }
                        BeatData.Yards.addYard(y);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void LoadBeatsFreeways()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.beatsFreeways.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT DISTINCT BeatID, BeatNumber, BeatDescription, Active FROM Beats";

                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.BeatFreeway bf = new BeatData.BeatFreeway();
                        bf.BeatID = new Guid(rdr["BeatID"].ToString());
                        bf.BeatNumber = rdr["BeatNumber"].ToString();
                        bf.BeatDescription = rdr["BeatDescription"].ToString();
                        bf.Active = (bool)rdr["Active"];
                        bf.Freeways = new List<string>();
                        DataClasses.GlobalData.beatsFreeways.Add(bf);
                    }
                    rdr.Close();

                    SQL = "getBeatsFreeways";
                    cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        List<string> freewayList = new List<string>();
                        
                        Guid _BeatID = new Guid(rdr["BeatID"].ToString());
                        string _freeway = rdr["FreewayName"].ToString();
                        BeatData.BeatFreeway bf = DataClasses.GlobalData.beatsFreeways.Find(delegate(BeatData.BeatFreeway find) { return find.BeatID == _BeatID; });
                        if (bf != null)
                        {
                            bf.Freeways.Add(_freeway);
                        }

                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("ERROR Loading Beats/Freeways:" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void LoadHighwaysBeats()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.highwaysBeats.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "GetHighwaysBeats";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.HighwaysBeats h = new MiscData.HighwaysBeats();
                        h.BeatNumber = rdr["BeatNumber"].ToString();
                        h.HighwayNumber = rdr["FreewayID"].ToString();
                        DataClasses.GlobalData.highwaysBeats.Add(h);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("ERROR Loading HighwaysBeats" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void LoadIncidentTypes()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.IncidentTypes.Clear();
            string SQL = "GetIncidentTypes";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.IncidentType thisIncidentType = new MiscData.IncidentType();
                        thisIncidentType.CHPIncidentTypeID = new Guid(rdr["CHPIncidentTypeID"].ToString());
                        thisIncidentType.CHPIncidentType = rdr["CHPIncidentType"].ToString();
                        DataClasses.GlobalData.IncidentTypes.Add(thisIncidentType);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Incident Types" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadTransportations()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.Transportations.Clear();
            string SQL = "GetTransportations";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.Transportation transportation = new MiscData.Transportation();
                        transportation.ID = Convert.ToInt32(rdr["ID"]);
                        transportation.Code = rdr["Code"].ToString();
                        DataClasses.GlobalData.Transportations.Add(transportation);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Transportations" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadLocationAbbreviations()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetLocationAbbreviations";
            DataClasses.GlobalData.LocationAbbreviations.Clear();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.LocationAbbreviation location = new MiscData.LocationAbbreviation();
                        location.ID = Convert.ToInt32(rdr["ID"]);
                        location.position = rdr["Position"].ToString();
                        location.abbreviation = rdr["Abbreviation"].ToString();
                        DataClasses.GlobalData.LocationAbbreviations.Add(location);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Incident Types" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadVehiclePositions()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.VehiclePositions.Clear();
            string SQL = "GetVehiclePositions";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.VehiclePosition thisVehiclePosition = new MiscData.VehiclePosition();
                        thisVehiclePosition.ID = Convert.ToInt32(rdr["ID"]);
                        thisVehiclePosition.Code = rdr["Code"].ToString();
                        DataClasses.GlobalData.VehiclePositions.Add(thisVehiclePosition);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Vehicle Positions" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadVehicleTypes()
        {
            logger = new Logging.EventLogger();
            DataClasses.GlobalData.VehicleTypes.Clear();
            string SQL = "GetVehicleTypes";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.VehicleType thisVehicleType = new MiscData.VehicleType();
                        thisVehicleType.ID = Convert.ToInt32(rdr["ID"]);
                        thisVehicleType.Code = rdr["Code"].ToString();
                        DataClasses.GlobalData.VehicleTypes.Add(thisVehicleType);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Vehicle Types" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadSchedules()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetSchedules";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    string today = DateTime.Now.ToShortDateString();
                    while (rdr.Read())
                    {
                        MiscData.BeatSchedule bs = new MiscData.BeatSchedule();
                        bs.BeatID = new Guid(rdr["beatid"].ToString());
                        bs.BeatNumber = rdr["beatnumber"].ToString();
                        bs.Contractor = rdr["ContractCompanyName"].ToString();
                        bs.ScheduleName = rdr["schedulename"].ToString();
                        bs.start = Convert.ToDateTime(today + " " + rdr["starttime"].ToString());
                        bs.end = Convert.ToDateTime(today + " " + rdr["endtime"].ToString());
                        bs.scheduleID = new Guid(rdr["scheduleID"].ToString());
                        bs.ScheduleType = Convert.ToInt32(rdr["ScheduleType"]);
                        DataClasses.GlobalData.theseSchedules.Add(bs);
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR Loading Schedules" + Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadCallSigns() //handles callsign and beat association
        {
            logger = new Logging.EventLogger();
            string SQL = "GetCallSigns";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    DataClasses.GlobalData.CallSigns.Clear();
                    while (rdr.Read())
                    {
                        MiscData.callSign cs = new MiscData.callSign();
                        cs.CallSign = rdr[0].ToString();
                        cs.ScheduleName = rdr[1].ToString();
                        cs.Beat = rdr[2].ToString();
                        DataClasses.GlobalData.CallSigns.Add(cs);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR Loading Call Signs" + Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadContractors() //handles contractor information for drivers
        {
            logger = new Logging.EventLogger();
            string SQL = "SELECT ContractorID, ContractCompanyName FROM Contractors ORDER BY ContractCompanyName";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.Contractors thisContractor = new MiscData.Contractors();
                        thisContractor.ContractorID = new Guid(rdr["ContractorID"].ToString());
                        thisContractor.ContractCompanyName = rdr["ContractCompanyName"].ToString();
                        DataClasses.GlobalData.Contractors.Add(thisContractor);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Contractors" + Environment.NewLine + ex.ToString(), true);
                }
            }
        }
       

        public void LoadLeeways()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT VarName, VarValue FROM Vars WHERE VarName LIKE '%Leeway'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        int LeewayVal = Convert.ToInt32(rdr["VarValue"]);
                        switch(rdr["VarName"].ToString())
                        {
                            case "RollOutLeeway":
                                DataClasses.GlobalData.RollOutLeeway = LeewayVal;
                                break;
                            case "StationaryLeeway":
                                DataClasses.GlobalData.StationaryLeeway = LeewayVal;
                                break;
                            case "LogOnLeeway":
                                DataClasses.GlobalData.LogOnLeeway = LeewayVal;
                                break;
                            case "OnPatrolLeeway":
                                DataClasses.GlobalData.OnPatrollLeeway = LeewayVal;
                                break;
                            case "RollInLeeway":
                                DataClasses.GlobalData.RollInLeeway = LeewayVal;
                                break;
                            case "LogOffLeeway":
                                DataClasses.GlobalData.LogOffLeeway = LeewayVal;
                                break;
                            case "SpeedingLeeway":
                                DataClasses.GlobalData.SpeedingLeeway = LeewayVal;
                                break;
                            case "OffBeatLeeway":
                                DataClasses.GlobalData.OffBeatLeeway = LeewayVal;
                                break;
                            case "ExtendedLeeway":
                                DataClasses.GlobalData.ExtendedLeeway = LeewayVal;
                                break;
                            case "GPSIssueLeeway":
                                DataClasses.GlobalData.GPSIssueLeeway = LeewayVal;
                                break;
                            case "ForcedOffLeeway":
                                DataClasses.GlobalData.ForceOff = LeewayVal;
                                break;
                        }
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Leeways" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void LoadBeatSchedules()
        {
            //TODO: THIS NEEDS TO BE REBUILT
            try
            {
                /*
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    DataClasses.GlobalData.theseSchedules.Clear();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetBeatSchedules", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    string cDate = DateTime.Now.ToShortDateString();
                    while (rdr.Read())
                    {
                        MiscData.BeatSchedule thisSchedule = new MiscData.BeatSchedule();
                        thisSchedule.BeatID = new Guid(rdr["BeatID"].ToString());
                        thisSchedule.BeatScheduleID = new Guid(rdr["BeatScheduleID"].ToString());
                        thisSchedule.BeatNumber = rdr["BeatNumber"].ToString();
                        thisSchedule.ScheduleName = rdr["ScheduleName"].ToString();
                        bool Weekday = false;
                        string wdtest = rdr["Weekday"].ToString();
                        if(rdr["Weekday"].ToString() == "True")
                        {
                            Weekday = true;
                        }
                        thisSchedule.Weekday = Weekday;
                        thisSchedule.Logon = Convert.ToDateTime(cDate + " " + rdr["Logon"]);
                        thisSchedule.RollOut = Convert.ToDateTime(cDate + " " + rdr["RollOut"]);
                        thisSchedule.OnPatrol = Convert.ToDateTime(cDate + " " + rdr["OnPatrol"]);
                        thisSchedule.RollIn = Convert.ToDateTime(cDate + " " + rdr["RollIn"]);
                        thisSchedule.LogOff = Convert.ToDateTime(cDate + " " + rdr["LogOff"]);
                        DataClasses.GlobalData.AddBeatSchedule(thisSchedule);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                 * */
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR Loading Beat Schedules" + Environment.NewLine + ex.ToString(), true);
            }
        }

        

        #endregion

        public List<AlarmData> getAllAlarms()
        {
            logger = new Logging.EventLogger();
            try
            {
                List<AlarmData> allAlarms = new List<AlarmData>();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "getAlarts";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AlarmData ad = new AlarmData();
                        ad.Beat = rdr["Beat"].ToString();
                        ad.TruckNumber = rdr["TruckNumber"].ToString();
                        ad.DriverName = rdr["DriverName"].ToString();
                        ad.CallSign = rdr["CallSign"].ToString();
                        ad.ContractorCompany = rdr["ContractorCompany"].ToString();
                        ad.alarmName = rdr["AlertName"].ToString();
                        if (rdr["HasAlarm"].ToString() == "0")
                        {
                            ad.hasAlarm = false;
                        }
                        else
                        {
                            ad.hasAlarm = true;
                        }
                        ad.alarmID = new Guid(rdr["alarmID"].ToString());
                        ad.alarmStart = Convert.ToDateTime(rdr["alarmStart"]);
                        if(!rdr.IsDBNull(9)) //AlertEnd
                        {
                            ad.alarmEnd = Convert.ToDateTime(rdr["alarmEnd"]);
                        }
                        if (!rdr.IsDBNull(10)) //DismissTime
                        {
                            ad.alarmCleared = Convert.ToDateTime(rdr["DismissTime"]);
                        }
                        if (!rdr.IsDBNull(11)) //Excuse Time
                        {
                            ad.alarmExcused = Convert.ToDateTime(rdr["ExcuseTime"]);
                        }
                        if (!rdr.IsDBNull(12)) //comment
                        {
                            ad.comments = rdr["comments"].ToString();
                        }
                        if (!rdr.IsDBNull(13)) //duration
                        {
                            ad.alarmDuration = Convert.ToInt32(rdr["AlertMins"]);
                        }
                        allAlarms.Add(ad);
                    }

                    conn.Close();
                }

                List<AlarmStatus> asList = new List<AlarmStatus>();
                foreach (AlarmData ad in allAlarms)
                {
 
                }

                return allAlarms;
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() +  Environment.NewLine + "Error getting alarms" + Environment.NewLine + ex.ToString(), true);
                return null;
            }
        }

        public string findBeatID(string beatNumber, string beatdescription)
        {
            logger = new Logging.EventLogger();
            string beat = "NOT FOUND";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT BeatID FROM Beats WHERE BeatNumber = '" + beatNumber + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        beat = rdr.IsDBNull(0) ? "NOT FOUND" : rdr["BeatID"].ToString();
                    }
                    cmd = null;
                    rdr.Close();
                    rdr = null;
                    if (beat != "NOT FOUND")
                    {
                        //groovy, return it.
                    }
                    else
                    {
                        //no beat, gotta create it and check for the ID again
                        SQL = "INSERT INTO Beats (Active, BeatDescription, BeatNumber, LastUpdate, LastUpdateBy)" +
                            " VALUES(1,'" + beatdescription + "','" + beatNumber + "','" + DateTime.Now + "','System')";
                        cmd = new SqlCommand(SQL, conn);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                        SQL = "SELECT BeatID FROM Beats WHERE BeatNumber = '" + beatNumber + "'";
                        cmd = new SqlCommand(SQL, conn);
                        rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            beat = rdr.IsDBNull(0) ? "NOT FOUND" : rdr["BeatID"].ToString();
                        }
                        rdr.Close();
                        rdr = null;
                        cmd = null;
                    }

                    conn.Close();
                }
                
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error loading beat number" + Environment.NewLine + ex.ToString(), true);
            }
            return beat;
        }

        public List<string> getContractors()
        {
            logger = new Logging.EventLogger();
            List<string> contractors = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT ContractCompanyName FROM dbo.Contractors ORDER BY ContractCompanyName";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        contractors.Add(rdr[0].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("ERROR GETTING CONTRACTORS: " + Environment.NewLine + ex.ToString(), true);
            }
            return contractors;
        }
 
        public List<string> getBeatNumbers()
        {
            List<string> BeatNumbers = new List<string>();
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT DISTINCT BeatNumber FROM Beats ORDER BY BeatNumber";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatNumbers.Add(rdr["BeatNumber"].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                return BeatNumbers;
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error getting beat list: " + Environment.NewLine + ex.ToString(), true);
                return null;
            }
        }

        public List<bbs> getSegmentsByBeat(string beatNumber)
        {
            List<bbs> BeatSegments = new List<bbs>();
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT BeatNumber, BeatSegmentNumber, ObjectID FROM BeatBeatSegments WHERE BeatNumber = '" + beatNumber + "' ORDER BY BeatSegmentNumber";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        bbs b = new bbs();
                        b.beatNumber = rdr["BeatNumber"].ToString();
                        b.beatSegmentNumber = rdr["BeatSegmentNumber"].ToString();
                        b.objectID = rdr["ObjectID"].ToString();
                        BeatSegments.Add(b);
                        //BeatSegments.Add(rdr["BeatSegmentNumber"].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                return BeatSegments;
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error getting beat list: " + Environment.NewLine + ex.ToString(), true);
                return null;
            }
        }

        public Guid GetDriverID(string _lastName, string _firstName)
        {
            Guid gDriver = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT DriverID FROM Drivers WHERE LastName = '" + _lastName + "' AND FirstName = '" + _firstName + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    object driverIDObj = cmd.ExecuteScalar();
                    string driverIDString = "00000000-0000-0000-0000-000000000000";
                    if (driverIDObj != null)
                    {
                        driverIDString = driverIDObj.ToString();
                    }
                    //string driverIDString = cmd.ExecuteScalar().ToString();
                    gDriver = new Guid(driverIDString);
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting DriverID for " + _firstName + " " + _lastName  +
                Environment.NewLine + ex.ToString(), true);
            }
            return gDriver;
        }

        public Guid GetTruckID(string truckNumber)
        {
            Guid gTruck = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT ISNULL(FleetVehicleID, '00000000-0000-0000-0000-000000000000') FROM FleetVehicles WHERE VehicleNumber = '" + truckNumber + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    object truckIDString = cmd.ExecuteScalar();
                    cmd = null;
                    if (truckIDString != null)
                    {
                        gTruck = new Guid(truckIDString.ToString());
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting TruckID for " + truckNumber +
                               Environment.NewLine + ex.ToString(), true);
            }
            return gTruck;
        }

        public Guid GetRunID(Guid _alertID)
        {
            Guid runID = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT TOP 1 RunID FROM TruckAlerts WHERE AlertID = '" + _alertID.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    object runIDString = cmd.ExecuteScalar();
                    cmd = null;
                    if (runIDString != null)
                    {
                        runID = new Guid(runIDString.ToString());
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting RunID for " + _alertID.ToString() +
                               Environment.NewLine + ex.ToString(), true);
            }
            return runID;
        }

        public string GetTruckNumberByID(Guid FleetVehicleID)
        {
            string TruckNumber = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetVehicleNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FleetVehicleID", FleetVehicleID);
                    TruckNumber = cmd.ExecuteScalar().ToString();
                    cmd = null;
                    conn.Close();
                }
               
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Next Survey Number " +
                Environment.NewLine + ex.ToString(), true);
            }
            return TruckNumber;
        }

        public int GetUsedBreakTime(string DriverID, string Type)
        {
            int UsedBreakTime = 0;
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("FindUsedBreakTime", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BreakType", Type);
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    var returnVal = cmd.ExecuteScalar();
                    if (returnVal != null)
                    {
                        UsedBreakTime = Convert.ToInt32(returnVal);
                        if (UsedBreakTime < 0)
                        {
                            UsedBreakTime = 0;
                        }
                    }
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Used BreakTime: " +
                 Environment.NewLine + ex.ToString(), true);
            }
            return UsedBreakTime;
        }

        public string GetVarValue(string VarName)
        {
            string VarValue;
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "SELECT VarValue FROM Vars WHERE VarName = '" + VarName + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    VarValue = cmd.ExecuteScalar().ToString();
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Next Incident Number " +
                  Environment.NewLine + ex.ToString(), true);
                VarValue = "error";
            }
            return VarValue;
        }

        public string GetSurveyNum(string beatNumber)
        {
            logger = new Logging.EventLogger();
            string SurveyNum = "NA";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    int TodayNum = 0;
                    int DBNum = 0;
                    int SurveyNumVal = 0;
                    string sMonth = DateTime.Now.Month.ToString();
                    string sDay = DateTime.Now.Day.ToString();
                    string sYear = DateTime.Now.Year.ToString();
                    while (sMonth.Length < 2)
                    {
                        sMonth = "0" + sMonth;
                    }
                    while (sDay.Length < 2)
                    {
                        sDay = "0" + sDay;
                    }
                    while (sYear.Length < 4)
                    {
                        sYear = "0" + sYear;
                    }
                    //SurveyNum = sMonth + sDay + sYear + "SN";
                    SurveyNum = sYear + sMonth + sDay + "SN" + beatNumber;
                    string SQL = "SELECT DATEPART(dy,GETDATE())"; //get TodayNum
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    TodayNum = Convert.ToInt32(cmd.ExecuteScalar());

                    SQL = "SELECT VarValue FROM Vars WHERE VarName = 'SurveyNumLastUpdate'"; //get DBNum
                    cmd = new SqlCommand(SQL, conn);
                    DBNum = Convert.ToInt32(cmd.ExecuteScalar());

                    if (TodayNum != DBNum)
                    {
                        cmd = new SqlCommand("ResetSurveyNums", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    cmd = new SqlCommand("GetNextSurveyNum", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    string NextSNum = Convert.ToString(cmd.ExecuteScalar());
                    SurveyNum += NextSNum;

                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Next Survey Number " +
                      Environment.NewLine + ex.ToString(), true);
                    if (conn.State == ConnectionState.Open)
                    { conn.Close(); }
                }
            }
            return SurveyNum;
        }

        public int GetNextIncidentNumber()
        {
            logger = new Logging.EventLogger();
            int NextNumber = 2147483647;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    string SQL = "GetIncidentCountForDay";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    NextNumber = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Next Incident Number " + 
                       Environment.NewLine + ex.ToString(), true);
                    if (conn.State == ConnectionState.Open)
                    { conn.Close(); }
                }
                return NextNumber;
            }
        }

        public Guid GetBeatIDByBeatNumber(string _BeatNumber)
        {
            logger = new Logging.EventLogger();
            try
            {
                Guid BeatID = new Guid();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT BeatID FROM Beats WHERE BeatNumber = '" + _BeatNumber + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    string val = cmd.ExecuteScalar().ToString();
                    if (!string.IsNullOrEmpty(val))
                    {
                        BeatID = new Guid(val.ToString());
                    }
                    cmd = null;
                    conn.Close();
                    return BeatID;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error getting BeatID for Beat " + _BeatNumber + Environment.NewLine + ex.ToString(), true);
                return new Guid("00000000-0000-0000-0000-000000000000");
            }
        }

        public TowTruck.TowTruckExtended GetExtendedData(string IPAddress)
        {
            logger = new Logging.EventLogger();
            TowTruck.TowTruckExtended thisTruckExtended = null;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    string SQL = "GetExtendedData";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    thisTruckExtended = new TowTruck.TowTruckExtended();

                    while (rdr.Read())
                    {

                        thisTruckExtended.ContractorName = rdr["ContractCompanyName"].ToString();
                        thisTruckExtended.FleetNumber = rdr["FleetNumber"].ToString();
                        thisTruckExtended.ProgramStartDate = Convert.ToDateTime(rdr["ProgramStartDate"]);
                        thisTruckExtended.VehicleType = rdr["VehicleType"].ToString();
                        thisTruckExtended.VehicleYear = Convert.ToInt32(rdr["VehicleYear"]);
                        thisTruckExtended.VehicleMake = rdr["VehicleMake"].ToString();
                        thisTruckExtended.VehicleModel = rdr["VehicleModel"].ToString();
                        thisTruckExtended.LicensePlate = rdr["LicensePlate"].ToString();
                        thisTruckExtended.RegistrationExpireDate = Convert.ToDateTime(rdr["RegistrationExpireDate"]);
                        thisTruckExtended.InsuranceExpireDate = Convert.ToDateTime(rdr["InsuranceExpireDate"]);
                        thisTruckExtended.LastCHPInspection = Convert.ToDateTime(rdr["LastCHPInspection"]);
                        thisTruckExtended.ProgramEndDate = Convert.ToDateTime(rdr["ProgramEndDate"]);
                        thisTruckExtended.FAW = Convert.ToInt32(rdr["FAW"]);
                        thisTruckExtended.RAW = Convert.ToInt32(rdr["RAW"]);
                        thisTruckExtended.RAWR = Convert.ToInt32(rdr["RAWR"]);
                        thisTruckExtended.GVW = Convert.ToInt32(rdr["GVW"]);
                        thisTruckExtended.GVWR = Convert.ToInt32(rdr["GVWR"]);
                        thisTruckExtended.Wheelbase = Convert.ToInt32(rdr["Wheelbase"]);
                        thisTruckExtended.Overhang = Convert.ToInt32(rdr["Overhang"]);
                        thisTruckExtended.MAXTW = Convert.ToInt32(rdr["MAXTW"]);
                        thisTruckExtended.MAXTWCALCDATE = Convert.ToDateTime(rdr["MAXTWCALCDATE"]);
                        thisTruckExtended.FuelType = rdr["FuelType"].ToString();
                        thisTruckExtended.TruckNumber = rdr["VehicleNumber"].ToString();
                        thisTruckExtended.FleetVehicleID = new Guid(rdr["FleetVehicleID"].ToString());
                        thisTruckExtended.ContractorID = new Guid(rdr["ContractorID"].ToString());
                        thisTruckExtended.isBackup = (bool)rdr["IsBackup"];
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Extended Data for Tow Truck " + IPAddress +
                        Environment.NewLine + ex.ToString(), true);
                    if (conn.State == ConnectionState.Open)
                    { conn.Close(); }
                }
            }
            return thisTruckExtended;
        }

        /*
        public List<BeatData.Yard> LoadYards()
        {
            logger = new Logging.EventLogger();
            List<BeatData.Yard> theseYards = new List<BeatData.Yard>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnBeat))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetAllYards", conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.Yard thisYard = new BeatData.Yard();
                        thisYard.YardID = new Guid(rdr["TowTruckYardID"].ToString());
                        thisYard.Location = rdr["Location"].ToString();
                        thisYard.TowTruckCompanyName = rdr["TowTruckCompanyName"].ToString();
                        thisYard.Position = SqlGeography.Deserialize(rdr.GetSqlBytes(3));
                        thisYard.YardDescription = rdr["TowTruckYardDescription"].ToString();
                        theseYards.Add(thisYard);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error loading yards" + Environment.NewLine + ex.ToString(), true);
            }
            return theseYards;
        }
        */
        public List<BeatData.Beat> LoadBeatsOnly()
        {
            logger = new Logging.EventLogger();
            List<BeatData.Beat> theseBeats = new List<BeatData.Beat>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnBeat))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LoadBeatsOnly", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.Beat thisBeat = new BeatData.Beat();
                        thisBeat.BeatID = new Guid(rdr["BeatID"].ToString());
                        thisBeat.BeatDescription = rdr["BeatDescription"].ToString();
                        thisBeat.BeatExtent = SqlGeography.Deserialize(rdr.GetSqlBytes(2));
                        thisBeat.FreewayID = Convert.ToInt32(rdr["FreewayID"]);
                        thisBeat.BeatNumber = rdr["BeatNumber"].ToString();
                        thisBeat.IsTemporary = Convert.ToBoolean(rdr["IsTemporary"]);
                        theseBeats.Add(thisBeat);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Beats Only" + Environment.NewLine + ex.ToString(), true);
            }
            if (theseBeats.Count > 0)
            { return theseBeats; }
            else
            { return null; }
        }

        public void LoadFreeways()
        {
            logger = new Logging.EventLogger();
            try
            {
                BeatData.EsriBeats.freeways.Clear();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT FreewayName FROM Freeways ORDER BY FreewayName", conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.EsriBeats.freeways.Add(rdr[0].ToString());
                    }

                    rdr.Close();
                    rdr = null;
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Freeways" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public List<beatData> getBeatData()
        {
            logger = new Logging.EventLogger();
            try
            {
                List<beatData> BeatData = new List<beatData>();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetBeatData", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        beatData bd = new beatData();
                        bd.BeatNumber = rdr["BeatNumber"].ToString();
                        bd.CallSign = rdr["CallSign"].ToString();
                        bd.TruckCount = Convert.ToInt32(rdr["TruckCount"]);
                        bd.BackupTruckCount = Convert.ToInt32(rdr["BackupTruckCount"]);
                        bd.ContractCompanyName = rdr["ContractCompanyName"].ToString();
                        bd.ScheduleName = rdr["ScheduleName"].ToString();
                        bd.StartTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + rdr["StartTime"]);
                        bd.EndTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + rdr["EndTime"]);
                        BeatData.Add(bd);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                return BeatData;
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Beat Data" + Environment.NewLine + ex.ToString(), true);
                return null;
            }
        }

        public List<BeatData.BeatSegment> LoadSegmentsOnly()
        {
            logger = new Logging.EventLogger();
            List<BeatData.BeatSegment> theseSegments = new List<BeatData.BeatSegment>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnBeat))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LoadBeatSegmentsOnly", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        BeatData.BeatSegment thisSegment = new BeatData.BeatSegment();
                        thisSegment.BeatSegmentID = new Guid(rdr["BeatSegmentID"].ToString());
                        thisSegment.CHPDescription = rdr["CHPDescription"].ToString();
                        thisSegment.BeatSegmentExtent = SqlGeography.Deserialize(rdr.GetSqlBytes(2));
                        thisSegment.BeatSegmentNumber = rdr["BeatSegmentNumber"].ToString();
                        thisSegment.BeatSegmentDescription = rdr["BeatSegmentDescription"].ToString();
                        thisSegment.BeatID = new Guid(rdr["BeatID"].ToString());
                        theseSegments.Add(thisSegment);
                    }
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Beat Segments Only" + Environment.NewLine + ex.ToString(), true);
            }
            if (theseSegments.Count > 0)
            { return theseSegments; }
            else
            { return null; }
        }

        public string FindUserNameByID(Guid ID)
        {
            logger = new Logging.EventLogger();
            string UserName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("FindUserName", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    UserName = Convert.ToString(cmd.ExecuteScalar());
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Finding User Name by ID" + Environment.NewLine + ex.ToString(), true);
            }
            if (string.IsNullOrEmpty(UserName))
            {
                return "Unknown";
            }
            else
            {
                return UserName;
            }
        }

        public string FindDriverNameByID(Guid ID)
        {
            logger = new Logging.EventLogger();
            string DriverName = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("FindDriverNameByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", ID.ToString());
                    DriverName = Convert.ToString(cmd.ExecuteScalar());
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Finding Driver Name by ID" + Environment.NewLine + ex.ToString(), true);
            }
            if (string.IsNullOrEmpty(DriverName))
            {
                return "Unknown";
            }
            else
            {
                return DriverName;
            }
        }

        /*
        public List<BeatData.BeatClass> LoadBeats()
        {
            List<BeatData.BeatClass> theseBeats = new List<BeatData.BeatClass>();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("LoadBeats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BeatData.BeatClass thisBeat = new BeatData.BeatClass();
                    thisBeat.BeatID = new Guid(rdr["BeatID"].ToString());
                    thisBeat.BeatDescription = rdr["BeatDescription"].ToString();
                    thisBeat.Active = Convert.ToBoolean(rdr["Active"]);
                    //thisBeat.BeatExtent = rdr.GetValue(3) as SqlGeography;
                    thisBeat.BeatExtent = SqlGeography.Deserialize(rdr.GetSqlBytes(3));
                    thisBeat.FreewayName = rdr["FreewayName"].ToString();
                    thisBeat.BeatSegmentID = new Guid(rdr["BeatSegmentID"].ToString());
                    thisBeat.BeatSegmentDescription = rdr["BeatSegmentDescription"].ToString();
                    thisBeat.BeatSegmentExtent = rdr.GetValue(7) as SqlGeography;
                    thisBeat.CHPDescription = rdr["CHPDescription"].ToString();
                    thisBeat.PIMSID = rdr["PIMSID"].ToString();
                    theseBeats.Add(thisBeat);
                }
                conn.Close();
            }
            if (theseBeats.Count > 0)
            { return theseBeats; }
            else
            { return null; }
        }
        */
        #endregion

        public List<RunStatus> getRunStatus(string truckNumber)
        {
            
            List<RunStatus> runs = new List<RunStatus>();
            try
            {
                Guid runID = new Guid("00000000-0000-0000-0000-000000000000");
                var tList = from t in DataClasses.GlobalData.currentTrucks
                            where t.TruckNumber == truckNumber
                            select t;
                foreach (TowTruck.TowTruck t in tList)
                {
                    runID = t.runID;
                }

                if (runID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    return null;
                }

                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("AlertByRunID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@runID", runID.ToString());
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        RunStatus rs = new RunStatus();
                        rs.Time = rdr["Time"].ToString();
                        rs.StatusChange = rdr.IsDBNull(1) ? "NA" : rdr["StatusChange"].ToString();
                        rs.Alert = rdr.IsDBNull(2) ? "NA" : rdr["Alert"].ToString();
                        rs.Location = rdr.IsDBNull(3) ? "NA" : rdr["Location"].ToString();
                        rs.Speed = rdr.IsDBNull(4) ? 0 : Convert.ToInt32(rdr["Speed"]);
                        rs.Heading = rdr.IsDBNull(5) ? "U" : rdr["Heading"].ToString();
                        runs.Add(rs);
                    }

                    conn.Close();
                }

                return runs;
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Truck " + truckNumber + " Run Information"  + Environment.NewLine + ex.ToString(), true);
            }

            return runs;
        }

        #region " SQL Writes "

        public void logNewAssist(Assist a) {
            logger = new Logging.EventLogger();
            try {
                using (SqlConnection conn = new SqlConnection(ConnStr)) {
                    conn.Open();

                    string SQL = "logNewAssist";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AssistID", a.assistID);
                    cmd.Parameters.AddWithValue("@IncidentID", a.incidentID);
                    cmd.Parameters.AddWithValue("@AssistDatePosted", a.assistDatePosted);
                    cmd.Parameters.AddWithValue("@LastAssistInIncidentReport", a.lastAssistInIncidentReport);
                    cmd.Parameters.AddWithValue("@ProblemType", a.problemType);
                    cmd.Parameters.AddWithValue("@ProblemDetail", a.problemDetail);
                    cmd.Parameters.AddWithValue("@ProblemNote", a.problemNote);
                    cmd.Parameters.AddWithValue("@OtherNote", a.otherNote);
                    cmd.Parameters.AddWithValue("@TransportType", a.transportType);
                    cmd.Parameters.AddWithValue("@StartODO", a.StartODO);
                    cmd.Parameters.AddWithValue("@EndODO", a.EndODO);
                    cmd.Parameters.AddWithValue("@DropSite", a.dropSite);
                    cmd.Parameters.AddWithValue("@State", a.state);
                    cmd.Parameters.AddWithValue("@LicensePlate", a.licensePlate);
                    cmd.Parameters.AddWithValue("@VehicleType", a.vehicleType);
                    cmd.Parameters.AddWithValue("@OTAuthorizationNumber", a.OTAuthorizationNumber);
                    cmd.Parameters.AddWithValue("@DetailNote", a.detailNote);
                    cmd.Parameters.AddWithValue("@AssistLat", a.assistLat);
                    cmd.Parameters.AddWithValue("@AssistLon", a.assistLon);
                    cmd.Parameters.AddWithValue("@DropSiteOther", a.dropSiteOther);
                    cmd.Parameters.AddWithValue("@CallSign", a.callSign);
                    cmd.Parameters.AddWithValue("@TimeOnAssist", a.timeOnAssist);
                    cmd.Parameters.AddWithValue("@TimeOffAssist", a.timeOffAssist);
                    cmd.Parameters.AddWithValue("@ActionTaken", a.actionTaken);
                    cmd.Parameters.AddWithValue("@DropSiteBeat", a.dropSiteBeat);
                    cmd.Parameters.AddWithValue("@PTN", a.PTN);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex) {
                logger.LogEvent("ERROR Logging Assist : " + ex.ToString(), true);
            }
        }

        public void logNewIncident(Incident i) {
            logger = new Logging.EventLogger();
            try {
                using (SqlConnection conn = new SqlConnection(ConnStr)) {
                    conn.Open();
                    string SQL = "logNewIncident";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncidentID", i.incidentID);
                    cmd.Parameters.AddWithValue("@IncidentDatePosted", i.incidentDatePosted);
                    cmd.Parameters.AddWithValue("@UserPosted", i.userPosted);
                    cmd.Parameters.AddWithValue("@CallSign", i.callSign);
                    cmd.Parameters.AddWithValue("@FromTruck", i.fromTruck);
                    cmd.Parameters.AddWithValue("@Lat", i.lat);
                    cmd.Parameters.AddWithValue("@Lon", i.lon);
                    cmd.Parameters.AddWithValue("@Canceled", i.canceled);
                    cmd.Parameters.AddWithValue("@ReasonCanceled", i.reasonCanceled);
                    cmd.Parameters.AddWithValue("@Beat", i.beat);
                    cmd.Parameters.AddWithValue("@TruckNumber", i.truckNumber);
                    cmd.Parameters.AddWithValue("@LogID", i.logID);
                    cmd.Parameters.AddWithValue("@WazeID", i.wazeID);
                    cmd.Parameters.AddWithValue("@TruckStatusID", i.truckStatusID);
                    cmd.Parameters.AddWithValue("@FSPLocation", i.FSPLocation);
                    cmd.Parameters.AddWithValue("@DispatchLocation", i.dispatchLocation);
                    cmd.Parameters.AddWithValue("@Direction", i.direction);
                    cmd.Parameters.AddWithValue("@PositionIncident", i.positionIncident);
                    cmd.Parameters.AddWithValue("@LaneNumber", i.laneNumber);
                    cmd.Parameters.AddWithValue("@CHPIncidentType", i.chpIncidentType);
                    cmd.Parameters.AddWithValue("@briefUpdateLat", i.briefUpdateLat);
                    cmd.Parameters.AddWithValue("@briefUpdateLon", i.briefUpdateLon);
                    cmd.Parameters.AddWithValue("@Freeway", i.freeway);
                    cmd.Parameters.AddWithValue("@BriefUpdatePosted", i.briefUpdatePosted);
                    cmd.Parameters.AddWithValue("@TimeOfBriefUpdate", i.timeOfBriefUpdate);
                    cmd.Parameters.AddWithValue("@CHPLogNumber", i.CHPLogNumber);
                    cmd.Parameters.AddWithValue("@IncidentSurveyNumber", i.incidentSurveyNumber);
                    cmd.Parameters.AddWithValue("@DriverLastName", i.driverLastName);
                    cmd.Parameters.AddWithValue("@DriverFirstName", i.driverFirstName);
                    cmd.Parameters.AddWithValue("@DriverID", i.driverID);
                    cmd.Parameters.AddWithValue("@TimeOnIncident", i.timeOnIncident);
                    cmd.Parameters.AddWithValue("@TimeOffIncident", i.timeOffIncident);
                    cmd.Parameters.AddWithValue("@RunID", i.runID);
                    cmd.Parameters.AddWithValue("@Comment", i.comment);
                    cmd.Parameters.AddWithValue("@Acked", i.acked);
                    cmd.Parameters.AddWithValue("@CrossStreet", i.crossStreet);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = "Error logging Incident Data: " + i.truckNumber + " " + i.userPosted + " " + DateTime.Now.ToString() + Environment.NewLine +
                    ex.ToString();
                logger.LogEvent(err, true);
            }
        }

        public List<MTCIncidentScreenData> getDailyIncidents() {
            Logging.EventLogger logger = new Logging.EventLogger();
            try {
                List<MTCIncidentScreenData> dList = new List<MTCIncidentScreenData>();
                using (SqlConnection conn = new SqlConnection(ConnStr)) {

                    conn.Open();
                    string SQL = "GetDailyIncidents";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        MTCIncidentScreenData d = new MTCIncidentScreenData();
                        d.IncidentID = rdr["IncidentID"].ToString();
                        d.Beat = rdr["Beat"].ToString();
                        d.CallSign = rdr["CallSign"].ToString();
                        d.TruckNumber = rdr["TruckNumber"].ToString();
                        d.Driver = rdr["Driver"].ToString();
                        d.DispatchSummaryMessage = rdr.IsDBNull(5) ? "NA" : rdr["Comment"].ToString();
                        d.ContractorName = rdr["ContractCompanyName"].ToString();
                        d.Time = Convert.ToDateTime(rdr["IncidentDatePosted"]);
                        d.DispatchNumber = rdr.IsDBNull(8) ? "NA" : rdr["CHPLogNumber"].ToString();
                        d.State = rdr["StatusName"].ToString();
                        d.IsIncidentComplete = "Yes";
                        d.isAcked = rdr["Acked"].ToString();
                        d.IncidentType = rdr["CHPIncidentType"].ToString();
                        d.Location = rdr.IsDBNull(12) ? "NA" : rdr["FSPLocation"].ToString();
                        dList.Add(d);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                return dList;
            }
            catch (Exception ex) {
                logger.LogEvent(ex.ToString(), true);
                return null;
            }
        }

        public void logDataUse(TowTruck.TowTruck t)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    if (string.IsNullOrEmpty(t.Driver.FirstName))
                    {
                        t.Driver.FirstName = "No Driver";
                    }
                    if (string.IsNullOrEmpty(t.State.BillStartDay))
                    {
                        t.State.BillStartDay = "0";
                    }
                    if (string.IsNullOrEmpty(t.State.LastBillReset))
                    {
                        t.State.LastBillReset = "01/01/2001 00:00:00";
                    }
                    if (string.IsNullOrEmpty(t.State.DataUsed))
                    {
                        t.State.DataUsed = "0.0";
                    }
                    if (string.IsNullOrEmpty(t.State.IgnTimeoutSecs))
                    {
                        t.State.IgnTimeoutSecs = "0";
                    }
                    if (string.IsNullOrEmpty(t.State.IPList))
                    {
                        t.State.IPList = "NA";
                    }
                    string SQL = "LogDataUse";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TruckIP", t.Identifier);
                    cmd.Parameters.AddWithValue("@DriverLastName", t.Driver.LastName);
                    cmd.Parameters.AddWithValue("@DriverFirstName", t.Driver.FirstName);
                    cmd.Parameters.AddWithValue("@IPList", t.State.IPList);
                    cmd.Parameters.AddWithValue("@BillStartDay", t.State.BillStartDay);
                    cmd.Parameters.AddWithValue("@LastBillReset", t.State.LastBillReset);
                    cmd.Parameters.AddWithValue("@DataUsed", t.State.DataUsed);
                    cmd.Parameters.AddWithValue("@IgnTimeoutSecs", t.State.IgnTimeoutSecs);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("ERROR Logging Data Use for Truck: " + t.TruckNumber + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void logIncomingWAZE(string wazeData, string callSign) {
            Logging.EventLogger logger = new Logging.EventLogger();
            try {
                using (SqlConnection conn = new SqlConnection(ConnStr)) {
                    conn.Open();

                    string SQL = "LogWAZEInbound";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CallSign", callSign);
                    cmd.Parameters.AddWithValue("@wazeData", wazeData);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex) {
                logger.LogEvent(ex.ToString(), true);
            }
        }

        public void logOutgoingWAZE(string wazeData, string callSign, string driverName)
        {
            Logging.EventLogger logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "LogWAZEOutbound";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CallSign", callSign);
                    cmd.Parameters.AddWithValue("@wazeData", wazeData);
                    cmd.Parameters.AddWithValue("@DriverName", driverName);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(ex.ToString(), true);
            }
        }

        public void updateBeatBeatSegments(string beatNumber, List<string> beatSegments)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "DELETE BeatBeatSegments WHERE BeatNumber = '" + beatNumber + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    foreach (string s in beatSegments)
                    {
                        SQL = "INSERT INTO BeatBeatSegments(BeatNumber,BeatSegmentNumber) VALUES ('" + beatNumber + "','" + s + "')";
                        cmd = new SqlCommand(SQL, conn);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error updating beatBeatSegments:" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void UpdateBeatFreeway(BeatData.BeatFreeway bf)
        {
            logger = new Logging.EventLogger();
            try
            {
                if (bf.BeatID == null || bf.BeatID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    bf.BeatID = Guid.NewGuid();
                }
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "UpdateBeat";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BeatID", bf.BeatID);
                    cmd.Parameters.AddWithValue("@BeatNumber", bf.BeatNumber);
                    cmd.Parameters.AddWithValue("@BeatDescription", bf.BeatDescription);
                    cmd.Parameters.AddWithValue("@Active", bf.Active);
                    cmd.ExecuteNonQuery();

                    cmd = null;

                    foreach (string s in bf.Freeways)
                    {
                        SQL = "LinkBeatsFreeways";
                        cmd = new SqlCommand(SQL, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BeatID", bf.BeatID);
                        cmd.Parameters.AddWithValue("@FreewayID", s);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Empty;
                msg += "BeatID: " + bf.BeatID.ToString() + Environment.NewLine;
                msg += "BeatNumber: " + bf.BeatNumber + Environment.NewLine;
                msg += "BeatDescription: " + bf.BeatDescription + Environment.NewLine;
                msg += "Active: " + bf.Active.ToString() + Environment.NewLine;
                msg += "Freeways: " + Environment.NewLine;
                foreach(string s in bf.Freeways)
                {
                    msg += s + Environment.NewLine;
                }
                logger.LogEvent("ERROR Updating BeatsFreeways" + Environment.NewLine + msg + Environment.NewLine + ex.ToString(), true);
            }
            
        }

        public void addTruck(string TruckNumber, string IPAddress, string contractor)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "AddTruck";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TruckNumber", TruckNumber);
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    cmd.Parameters.AddWithValue("@contractor", contractor);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("ERROR ADDING TRUCK: " + TruckNumber + " " + IPAddress + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void logOverTime(string shift, string callSign, string beat, string contractor)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "LogOvertimeActivity";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Shift", shift);
                    cmd.Parameters.AddWithValue("@CallSign", callSign);
                    cmd.Parameters.AddWithValue("@beat", beat);
                    cmd.Parameters.AddWithValue("@Contractor", contractor);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error logging overtime activity" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void logViolation(TowTruck.AlarmLog log)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "logViolation";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ViolationTypeId", log.ViolationTypeId);
                    cmd.Parameters.AddWithValue("@CreatedOn", log.CreatedOn);
                    cmd.Parameters.AddWithValue("@CreatedBy", log.CreatedBy);
                    cmd.Parameters.AddWithValue("@ContractorID", log.ContractorID);
                    cmd.Parameters.AddWithValue("@DateTimeOfViolation", log.DateTimeOfViolation);
                    cmd.Parameters.AddWithValue("@BeatID", log.BeatID);
                    cmd.Parameters.AddWithValue("@DriverID", log.DriverID);
                    cmd.Parameters.AddWithValue("@FleetVehicleID", log.FleetVehicleId);
                    cmd.Parameters.AddWithValue("@CallSign", log.CallSign);
                    cmd.Parameters.AddWithValue("@AlarmName", log.AlarmName);
                    cmd.Parameters.AddWithValue("@AlertTime", log.AlertTime);
                    cmd.Parameters.AddWithValue("@LengthOfViolation", log.LengthOfViolation);
                    cmd.Parameters.AddWithValue("@Lat", log.Lat);
                    cmd.Parameters.AddWithValue("@Lon", log.Lon);
                    cmd.Parameters.AddWithValue("@RunID", log.runID);
                    cmd.Parameters.AddWithValue("@AlarmID", log.AlarmID);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error logging violation: " + Environment.NewLine + ex.ToString() + Environment.NewLine, true);
            }
        }

        public void closeOutTruck(Guid runID)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "UPDATE TruckAlerts SET AlertEnd = GETDATE(), AlertMins = (SELECT DATEDIFF(mi, AlertEnd, GETDATE()))" +
                        " WHERE AlertEnd IS NULL AND RunID = '" + runID.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    SQL = "UPDATE TruckAlerts SET AlertMins = DATEDIFF(mi, AlertStart, AlertEnd) WHERE AlertMins IS NULL AND runID = '" + runID.ToString() + "'";
                    cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error closing out truck" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void excuseAlarm(Guid _runID, Guid _alertID, string excusedBy, string comment)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "ExcuseAlert";
                    SqlCommand cmd = new SqlCommand(SQL,conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@runID", _runID.ToString());
                    cmd.Parameters.AddWithValue("@alertID", _alertID.ToString());
                    cmd.Parameters.AddWithValue("@excusedBy", excusedBy);
                    cmd.Parameters.AddWithValue("@comment", comment);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Excusing Alarm" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void clearAlarm(Guid _runID, Guid _alertID)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "DismissAlert";
                    SqlCommand cmd = new SqlCommand(SQL,conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@runID", _runID);
                    cmd.Parameters.AddWithValue("@alertID", _alertID);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Excusing Alarm" + Environment.NewLine + ex.ToString(), true);
            }
        }

        private string checkHead(int heading)
        {
            string head = "U";

            if (heading >= 338 && heading <= 23)
            {
                head = "N";
            }
            if (heading > 23 && heading <= 68)
            {
                head = "NE";
            }
            if (heading > 68 && heading <= 113)
            {
                head = "E";
            }
            if (heading > 113 && heading <= 158)
            {
                head = "SE";
            }
            if (heading > 158 && heading <= 203)
            {
                head = "S";
            }
            if (heading > 203 && heading <= 248)
            {
                head = "SW";
            }
            if (heading > 248 && heading <= 293)
            {
                head = "W";
            }
            if (heading > 293 && heading < 338)
            {
                head = "NW";
            }
            return head;
        }

        public void logStatus(Guid StatusID, string statusName, string truckNumber, string DriverName, string ContractorCompany, string Beat, DateTime statusStart, DateTime statusEnd,
            double lat, double lon, Guid RunID, string location, double speed, int heading, Guid ScheduleID)
        {
            logger = new Logging.EventLogger();
            try
            {
                string head = checkHead(heading);
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("logStatus", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StatusID", StatusID.ToString());
                    cmd.Parameters.AddWithValue("@TruckNumber", truckNumber);
                    cmd.Parameters.AddWithValue("@DriverName", DriverName);
                    cmd.Parameters.AddWithValue("@ContractorCompany", ContractorCompany);
                    cmd.Parameters.AddWithValue("@Beat", Beat);
                    cmd.Parameters.AddWithValue("@StatusName", statusName);
                    cmd.Parameters.AddWithValue("@StatusStart", statusStart);
                    cmd.Parameters.AddWithValue("@StatusEnd", statusEnd);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon);
                    cmd.Parameters.AddWithValue("@runID", RunID);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@heading", head);
                    cmd.Parameters.AddWithValue("@speed", speed);
                    cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR LOGGING STATUS" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void LogAlert(Guid AlertID, string truckNumber, string DriverName, string ContractorCompany, string Beat, string AlertName, DateTime alertStart, DateTime alertEnd,
            double lat, double lon, Guid runID, string location, double speed, int heading, string CallSign, Guid ScheduleID, int ScheduleType)
        {
            logger = new Logging.EventLogger();
            try
            {
                string head = checkHead(heading);
                if (ScheduleID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    ScheduleType = 0;
                }
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("logAlert", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AlertID", AlertID.ToString());
                    cmd.Parameters.AddWithValue("@TruckNumber", truckNumber);
                    cmd.Parameters.AddWithValue("@DriverName", DriverName);
                    cmd.Parameters.AddWithValue("@ContractorCompany", ContractorCompany);
                    cmd.Parameters.AddWithValue("@Beat", Beat);
                    cmd.Parameters.AddWithValue("@AlertName", AlertName);
                    cmd.Parameters.AddWithValue("@AlertStart", alertStart);
                    cmd.Parameters.AddWithValue("@AlertEnd", alertEnd);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lon", lon);
                    cmd.Parameters.AddWithValue("@runID", runID);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@heading", head);
                    cmd.Parameters.AddWithValue("@speed", speed);
                    cmd.Parameters.AddWithValue("@CallSign", CallSign);
                    cmd.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    cmd.Parameters.AddWithValue("@ScheduleType", ScheduleType);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR LOGGING ALARM" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void logMileage(Guid RunID, double startODO, double endODO)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("LogTruckMileage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@runId", RunID.ToString());
                    cmd.Parameters.AddWithValue("@startODO", startODO);
                    cmd.Parameters.AddWithValue("@endODO", endODO);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR LOGGING MILEIAGE" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void ClearAlarm(Guid DriverID, Guid VehicleID, string AlarmType)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("ClearAlarm", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID.ToString());
                    cmd.Parameters.AddWithValue("@AlarmType", AlarmType);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR CLEARING ALARM: " + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        public string GetUserName(Guid UserID)
        {
            logger = new Logging.EventLogger();
            string UserName = "Unknown User";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "SELECT FirstName FROM Users WHERE UserID = '" + UserID + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    object retVal = cmd.ExecuteScalar();
                    if (retVal != DBNull.Value && retVal != null)
                    {
                        UserName = cmd.ExecuteScalar().ToString();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR FINDING UserName: " + Environment.NewLine +
                                   ex.ToString(), true);
            }
            return UserName;
        }

        public void UnExcuseAlarm(Guid DriverID, Guid VehicleID, string AlarmType, string Comments, string BeatNumber)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UnExcuseAlarm", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID.ToString());
                    cmd.Parameters.AddWithValue("@AlarmType", AlarmType);
                    cmd.Parameters.AddWithValue("@Comments", Comments);
                    cmd.Parameters.AddWithValue("@BeatNumber", BeatNumber);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR UNEXCUSING ALARM: " + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        public void ExcuseAlarm(Guid DriverID, Guid VehicleID, string AlarmType, string Comments, string BeatNumber)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("ExcuseAlarm", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID.ToString());
                    cmd.Parameters.AddWithValue("@AlarmType", AlarmType);
                    cmd.Parameters.AddWithValue("@Comments", Comments);
                    cmd.Parameters.AddWithValue("@BeatNumber", BeatNumber);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR EXCUSING ALARM: " + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        public void LogTruckMessage(TruckMessage thisMessage)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogTruckMessage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MessageID", thisMessage.MessageID.ToString());
                    cmd.Parameters.AddWithValue("@TruckIP", thisMessage.TruckIP.ToString());
                    cmd.Parameters.AddWithValue("@MessageText", thisMessage.MessageText);
                    cmd.Parameters.AddWithValue("@SentTime", thisMessage.SentTime.ToString());
                    cmd.Parameters.AddWithValue("@UserEmail", thisMessage.UserEmail.ToString());
                    cmd.Parameters.AddWithValue("@TruckNumber", thisMessage.TruckNumber.ToString());
                    cmd.Parameters.AddWithValue("@CallSign", thisMessage.CallSign.ToString());
                    cmd.Parameters.AddWithValue("@DriverName", thisMessage.Driver.ToString());
                    cmd.Parameters.AddWithValue("@BeatNumber", thisMessage.Beat.ToString());
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR LOGGING TRUCK MESSAGE: " + thisMessage.MessageText + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        public void AckTruckMessage(TruckMessage thisMessage)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("AckTruckMessage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MessageID", thisMessage.MessageID.ToString());
                    cmd.Parameters.AddWithValue("@AckTime", thisMessage.AckedTime.ToString());
                    cmd.Parameters.AddWithValue("@Response", thisMessage.MessageResponse);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR ACKING TRUCK MESSAGE: " + thisMessage.MessageID.ToString() + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        public void LogAlarm(string AlarmType, DateTime AlarmTime, Guid DriverID, Guid VehicleID, Guid BeatID)
        {
            logger = new Logging.EventLogger();
            try
            {
                using(SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogAlarm", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AlarmType", AlarmType);
                    cmd.Parameters.AddWithValue("@AlarmTime", AlarmTime.ToString());
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID.ToString());
                    cmd.Parameters.AddWithValue("@BeatID", BeatID.ToString());
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + "Error logging alarm" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void SetBreakTime(Guid DriverID, string Type, int TotalMinutes)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UpdateBreaks", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BreakType", Type);
                    cmd.Parameters.AddWithValue("@additionalMinutes", TotalMinutes);
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + "Error writing break time" + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void SetVarValue(string VarName, string VarValue)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "UPDATE Vars SET VarValue = '" + VarValue + "' WHERE VarName = '" + VarName + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + "Error writing " + VarName + " value to " + VarValue + Environment.NewLine + ex.ToString(), true);
            }
        }

        public void LogGPS(string SQL, ArrayList arrParams)
        {
            logger = new Logging.EventLogger();
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(SQL, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        for (int i = 0; i < arrParams.Count; i++)
                        {
                            string[] splitter = arrParams[i].ToString().Split('^');
                            cmd.Parameters.AddWithValue(splitter[0].ToString(), splitter[1].ToString());
                        }
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        string paramList = string.Empty;
                        for (int i = 0; i < arrParams.Count; i++)
                        {
                            paramList += arrParams[i].ToString() + Environment.NewLine;
                        }
                       logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + ex.ToString() + Environment.NewLine + paramList, true);
                       if (conn.State == ConnectionState.Open)
                       {conn.Close();}
                    }
                }
            }

        public void LogOffBeat(Guid DriverID, string VehicleNumber, DateTime OffBeatTime, SqlGeography location)
        {
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogOffBeatAlert", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                    cmd.Parameters.AddWithValue("@OffBeatTime", OffBeatTime);
                    cmd.Parameters.AddWithValue("@Location", location.ToString());
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = null;
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Logging Off Beat " + VehicleNumber +
                       Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LogSpeeding(Guid DriverID, string VehicleNumber, double LoggedSpeed, double MaxSpeed, DateTime speedingTime, SqlGeography location)
        {
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogSpeedingAlert", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                    cmd.Parameters.AddWithValue("@LoggedSpeed", LoggedSpeed);
                    cmd.Parameters.AddWithValue("@MaxSpeed", MaxSpeed);
                    cmd.Parameters.AddWithValue("@SpeedingTime", speedingTime);
                    cmd.Parameters.AddWithValue("@Location", location.ToString());
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    cmd = null;
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Logging Speeding " + VehicleNumber + 
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void FixTruckNumber(string IPAddress, string TruckNumber, Guid ContractorID)
        {
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("FixTruckNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
                    cmd.Parameters.AddWithValue("@VehicleNumber", TruckNumber);
                    cmd.Parameters.AddWithValue("@ContractorID", ContractorID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Fixing Truck Number " + TruckNumber + " IP Address: " + IPAddress +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void CancelIncident(MTCIncident mi)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CancelIncident", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncidentID", mi.IncidentID);
                    cmd.Parameters.AddWithValue("@Reason", mi.Reason);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "ERROR LOGGING INCIDENT CANCEL: " + mi.IncidentID + Environment.NewLine +
                    ex.ToString(), true);
            }
        }

        #endregion

        #region " SQL Stored Procs "

        public void RunProc(string ProcName, ArrayList arrParams)
        {
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(ProcName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (arrParams.Count > 0)
                    {
                        for (int i = 0; i < arrParams.Count; i++)
                        {
                            string[] splitter = arrParams[i].ToString().Split('~');
                            cmd.Parameters.AddWithValue(splitter[0].ToString(), splitter[1].ToString());
                        }
                    }
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    string Params = "";
                    if(arrParams.Count > 0)
                    {
                        for(int i = 0;i<arrParams.Count;i++)
                        {
                            Params += arrParams[i].ToString() + Environment.NewLine;
                        }
                    }
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error running proc: " + ProcName + " Params:" + Params +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public Guid? logCADMessage(string direction, string message)
        {
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    //string SQL = "INSERT INTO CADLog (Direction, Message) VALUES ('" + direction + "','" + message + "')";
                    string SQL = "LogCADMessage";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Direction", direction);
                    cmd.Parameters.AddWithValue("@Message", message);
                    Guid id = new Guid(cmd.ExecuteScalar().ToString());
                    cmd = null;

                    conn.Close();
                    return id;
                }
            }
            catch (Exception ex)
            {
                logger.LogEvent("Error logging CAD Message " + DateTime.Now.ToString() + Environment.NewLine + ex.ToString(), true);
                return null;
            }
        }

        public void LogEarlyRollIn(Guid DriverID, string _reason, Guid BeatID, string _vehicleID, DateTime timeStamp, string CHPLogNumber, string _exceptionType)
        {
            //this was originally specced to handle only early roll ins and was later adapted to handle mistimed events in general
            //we support late log on, roll out, on patrol, early roll in, and early log off.  All functionality works essentially the
            //same and gets logged into the EarlyRollIns table in the fsp database.
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogEarlyRollIn", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    cmd.Parameters.AddWithValue("@Reason", _reason);
                    cmd.Parameters.AddWithValue("@BeatID", BeatID.ToString());
                    cmd.Parameters.AddWithValue("@VehicleID", _vehicleID);
                    cmd.Parameters.AddWithValue("@timeStamp", timeStamp);
                    cmd.Parameters.AddWithValue("@CHPLogNumber", CHPLogNumber);
                    cmd.Parameters.AddWithValue("@ExceptionType", _exceptionType);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error logging Early Login for Driver: " + DriverID.ToString() + Environment.NewLine +
                        "Reason: " + _reason + Environment.NewLine + Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LogEvent(Guid DriverID, string EventType)
        {
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try 
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("LogDriverEvent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    cmd.Parameters.AddWithValue("@EventType", EventType);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "LogEvent Error" + Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public DateTime GetLastEventTime(string DriverID, string EventType)
        {
            logger = new Logging.EventLogger();
            DateTime lastTime = Convert.ToDateTime("01/01/2001 00:00:00");

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetEventTime", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EventType", EventType);
                    cmd.Parameters.AddWithValue("@DriverID", DriverID);
                    lastTime = Convert.ToDateTime(cmd.ExecuteScalar());
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "GetLastEventTime Error" + Environment.NewLine + ex.ToString(), true);
                }
            }

            return lastTime;
        }

        public int GetBreakDuration(string DriverID)
        {
            logger = new Logging.EventLogger();
            int BreakDuration = 0;
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try 
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetBreakDuration", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DriverID", DriverID.ToString());
                    BreakDuration = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "GetBreakDuration Error" + Environment.NewLine + ex.ToString(), true);
                    return 0;
                }
            }
            return BreakDuration;
        }

        public bool CheckLogon(string IPAddr, string FSPID, string Password)
        {
            bool valid = false;
            logger = new Logging.EventLogger();
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("CheckLogon", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FSPID", FSPID);
                    cmd.Parameters.AddWithValue("@password", Password);
                    string Output = Convert.ToString(cmd.ExecuteScalar());
                    if (Output == "1")
                    { valid = true; }
                    else
                    { 
                        valid = false;
                        logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "An invalid logon was attempted from " + IPAddr, true);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "CheckLogon Error" + Environment.NewLine + ex.ToString(), true);
                }

            }
            return valid;
        }

        public TowTruck.AssignedBeat GetAssignedBeat(Guid VehicleID)
        {
            TowTruck.AssignedBeat thisAssignedBeat = new TowTruck.AssignedBeat();
            logger = new Logging.EventLogger();
            thisAssignedBeat.BeatID = new Guid("00000000-0000-0000-0000-000000000000");
            thisAssignedBeat.Loaded = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetVehicleAssignedBeat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FleetVehicleID", VehicleID.ToString());
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        thisAssignedBeat.BeatID = new Guid(rdr["BeatID"].ToString());
                        thisAssignedBeat.BeatExtent = SqlGeography.Deserialize(rdr.GetSqlBytes(1));
                        thisAssignedBeat.BeatNumber = rdr["BeatNumber"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            { logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Assigned Beat" + Environment.NewLine + ex.ToString(), true); }
            return thisAssignedBeat;
        }

        public TowTruck.TowTruckDriver GetDriver(string FSPID)
        {
            TowTruck.TowTruckDriver thisDriver = new TowTruck.TowTruckDriver();
            logger = new Logging.EventLogger();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetDriver", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FSPIDNumber", FSPID);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        thisDriver.DriverID = new Guid(rdr["DriverID"].ToString());
                        thisDriver.LastName = rdr["LastName"].ToString();
                        thisDriver.FirstName = rdr["FirstName"].ToString();
                        thisDriver.TowTruckCompany = rdr["ContractCompanyName"].ToString();
                        //thisDriver.AssignedBeat = new Guid(rdr["AssignedBeat"].ToString());
                        //thisDriver.BeatScheduleID = new Guid(rdr["BeatScheduleID"].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            { logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Getting Driver" + Environment.NewLine + ex.ToString(), true); }
            return thisDriver;
        }

        #endregion

        #region " MTC Assist Data "

        public void UpdateMTCIncident(Guid IncidentID, bool Cancelled, string Reason)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "UpdateMTCIncident";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncidentID", IncidentID.ToString());
                    cmd.Parameters.AddWithValue("@Canceled", Cancelled);
                    cmd.Parameters.AddWithValue("@Reason", Reason);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Updating MTC Incident" +
                        Environment.NewLine + ex.ToString(), true);
            }
        }

        public Guid PostMTCIncident(string UserPosted, string TruckNumber, string Beat, int fromTruck, double lat, double lon)
        {
            Guid IncidentID = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();

                    string SQL = "MakeMTCIncident";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserPosted", UserPosted);
                    cmd.Parameters.AddWithValue("@Beat", Beat);
                    cmd.Parameters.AddWithValue("@fromTruck", fromTruck);
                    cmd.Parameters.AddWithValue("@Lat", lat);
                    cmd.Parameters.AddWithValue("@Lon", lon);
                    cmd.Parameters.AddWithValue("@TruckNumber", TruckNumber);

                    IncidentID = new Guid(cmd.ExecuteScalar().ToString());

                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error posting MTC Incident" +
                        Environment.NewLine + ex.ToString(), true);
            }
            return IncidentID;
        }

        public void PostMTCPreAssist(Guid IncidentID, MTCPreAssistData data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    string SQL = "MakeMTCPreAssist";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncidentID", IncidentID);
                    cmd.Parameters.AddWithValue("@Direction", data.Direction);
                    cmd.Parameters.AddWithValue("@FSPLocation", data.FSPLocation);
                    cmd.Parameters.AddWithValue("@DispatchLocation", data.LocationofInitialDispatch);
                    cmd.Parameters.AddWithValue("@Comment", data.Comment);
                    cmd.Parameters.AddWithValue("@CrossStreet", data.CrossStreet);
                    cmd.Parameters.AddWithValue("@DispatchCode", data.DispatchCode);
                    cmd.Parameters.AddWithValue("@Position", data.Position);
                    cmd.Parameters.AddWithValue("@LaneNumber", data.LaneNumber);
                    cmd.Parameters.AddWithValue("@CHPLogNumber", data.CHPLogNumber);
                    cmd.Parameters.AddWithValue("@IncidentSurveyNumber", data.IncidentSurveyNumber);
                    cmd.Parameters.AddWithValue("@Lat", data.Lat);
                    cmd.Parameters.AddWithValue("@Lon", data.Lon);
                    cmd.Parameters.AddWithValue("@CHPIncidentType", data.CHPIncidentType);
                    cmd.Parameters.AddWithValue("@Freeway", data.Freeway);
                    cmd.Parameters.AddWithValue("@DriverLastName", data.DriverLastName);
                    cmd.Parameters.AddWithValue("@DriverFirstName", data.DriverFirstName);
                    cmd.Parameters.AddWithValue("@DriverID", data.DriverID);
                    cmd.Parameters.AddWithValue("@RunID", data.RunID);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error posting MTC PreAssist" +
                    Environment.NewLine + ex.ToString(), true);
            }
        }

        public void PostMTCAssist(Guid IncidentID, MTCAssist data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    //Handle Transport Type and Action Taken (lists of strings)
                    string transportTypes = "";
                    foreach (string s in data.TransportType)
                    {
                        transportTypes += s + "|";
                    }
                    if (transportTypes.Length > 1)
                    {
                        transportTypes = transportTypes.Substring(0, transportTypes.Length - 1);
                    }
                    else
                    {
                        transportTypes = "NA";
                    }

                    string actionsTaken = "";
                    foreach (string s in data.ActionTaken)
                    {
                        actionsTaken += s + "|";
                    }
                    if (actionsTaken.Length > 1)
                    {
                        actionsTaken = actionsTaken.Substring(0, actionsTaken.Length - 1);
                    }
                    else
                    {
                        actionsTaken = "NA";
                    }
                    
                    conn.Open();
                    string SQL = "MakeMTCAssist";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncidentID", IncidentID);
                    cmd.Parameters.AddWithValue("@TrafficCollision", data.TrafficCollision);
                    cmd.Parameters.AddWithValue("@DebrisOnly", data.DebrisOnly);
                    cmd.Parameters.AddWithValue("@Breakdown", data.Breakdown);
                    cmd.Parameters.AddWithValue("@Other", data.Other);
                    cmd.Parameters.AddWithValue("@ProblemNote", data.ProblemNote);
                    cmd.Parameters.AddWithValue("@OtherNote", data.OtherNote);
                    cmd.Parameters.AddWithValue("@TransportType", transportTypes);
                    //cmd.Parameters.AddWithValue("@ActionsTaken", actionsTaken);
                    cmd.Parameters.AddWithValue("@StartODO", data.PStartODO);
                    cmd.Parameters.AddWithValue("@EndODO", data.PEndODO);
                    cmd.Parameters.AddWithValue("@DropSiteBeat", data.DropSiteBeat);
                    cmd.Parameters.AddWithValue("@DropSite", data.DropSite);
                    cmd.Parameters.AddWithValue("@DropSiteOther", data.DropSiteOther);
                    cmd.Parameters.AddWithValue("@State", data.State);
                    cmd.Parameters.AddWithValue("@LicensePlateNumber", data.LicPlateNum);
                    cmd.Parameters.AddWithValue("@VehicleType", data.VehicleType);
                    cmd.Parameters.AddWithValue("@OTAuthorizationNumber", data.OTAuthNum);
                    cmd.Parameters.AddWithValue("@DetailNote", data.DetailNote);
                    cmd.Parameters.AddWithValue("@Lat", data.Lat);
                    cmd.Parameters.AddWithValue("@Lon", data.Lon);
                    cmd.Parameters.AddWithValue("@CallSign", data.CallSign);
                    Guid AssistID = new Guid(cmd.ExecuteScalar().ToString());
                    cmd = null;
                    foreach (string s in data.ActionTaken)
                    {
                        SQL = "MakeMTCActionsTaken";
                        cmd = new SqlCommand(SQL, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MTCAssistID", AssistID);
                        cmd.Parameters.AddWithValue("@ActionTaken", s);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                logger = new Logging.EventLogger();
                logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error posting MTC Assist" +
                        Environment.NewLine + ex.ToString(), true);
            }
        }

        #endregion

        #region  " Deprecated Code "

        /*  Deprecated data - OCTA used this information, MTC does not
        public void LoadCode1098s()
        {
            logger = new Logging.EventLogger();
            string SQL = "Get1098Codes";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        MiscData.Code1098 thisCode = new MiscData.Code1098();
                        thisCode.CodeID = new Guid(rdr["CodeID"].ToString());
                        thisCode.Code = rdr["Code"].ToString();
                        thisCode.CodeName = rdr["CodeName"].ToString();
                        thisCode.CodeDescription = rdr["CodeDescription"].ToString();
                        thisCode.CodeCall = rdr["CodeCall"].ToString();
                        DataClasses.GlobalData.Code1098s.Add(thisCode);
                    }

                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading 1098 Codes" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadFreeways()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetFreeways";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.Freeway thisFreeway = new MiscData.Freeway();
                        thisFreeway.FreewayID = Convert.ToInt32(rdr["FreewayID"]);
                        thisFreeway.FreewayName = rdr["FreewayName"].ToString();
                        DataClasses.GlobalData.Freeways.Add(thisFreeway);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Freeways" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        

        public void LoadLocationCoding()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetLocationCoding";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.LocationCoding thisLocationCoding = new MiscData.LocationCoding();
                        thisLocationCoding.LocationID = new Guid(rdr["LocationID"].ToString());
                        thisLocationCoding.LocationCode = rdr["LocationCode"].ToString();
                        thisLocationCoding.Location = rdr["Location"].ToString();
                        DataClasses.GlobalData.LocationCodes.Add(thisLocationCoding);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Location Coding" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadServiceTypes()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetServiceTypes";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.ServiceType thisServiceType = new MiscData.ServiceType();
                        thisServiceType.ServiceTypeID = new Guid(rdr["ServiceTypeID"].ToString());
                        thisServiceType.ServiceTypeCode = rdr["ServiceTypeCode"].ToString();
                        thisServiceType.ServiceTypeName = rdr["ServiceType"].ToString();
                        DataClasses.GlobalData.ServiceTypes.Add(thisServiceType);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Service Types" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadTowLocations()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetTowLocations";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.TowLocation thisTowLocation = new MiscData.TowLocation();
                        thisTowLocation.TowLocationID = new Guid(rdr["TowLocationID"].ToString());
                        thisTowLocation.TowLocationCode = rdr["TowLocationCode"].ToString();
                        thisTowLocation.TowLocationName = rdr["TowLocation"].ToString();
                        DataClasses.GlobalData.TowLocations.Add(thisTowLocation);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Tow Locations" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadTrafficSpeeds()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetTrafficSpeeds";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.TrafficSpeed thisTrafficSpeed = new MiscData.TrafficSpeed();
                        thisTrafficSpeed.TrafficSpeedID = new Guid(rdr["TrafficSpeedID"].ToString());
                        thisTrafficSpeed.TrafficSpeedCode = rdr["TrafficSpeedCode"].ToString();
                        thisTrafficSpeed.TrafficSpeedName = rdr["TrafficSpeed"].ToString();
                        DataClasses.GlobalData.TrafficSpeeds.Add(thisTrafficSpeed);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Traffic Speeds" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        public void LoadVehiclePositions()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetVehiclePositions";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.VehiclePosition thisVehiclePosition = new MiscData.VehiclePosition();
                        thisVehiclePosition.VehiclePositionID = new Guid(rdr["VehiclePositionID"].ToString());
                        thisVehiclePosition.VehiclePositionCode = rdr["VehiclePositionCode"].ToString();
                        thisVehiclePosition.VehiclePositionName = rdr["VehiclePosition"].ToString();
                        DataClasses.GlobalData.VehiclePositions.Add(thisVehiclePosition);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Vehicle Positions" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }

        

        public void LoadDropZones()
        {
            logger = new Logging.EventLogger();
            string SQL = "GetDropZones";
            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MiscData.DropZone thisDropZone = new MiscData.DropZone();
                        thisDropZone.DropZoneID = new Guid(rdr["DropZoneID"].ToString());
                        thisDropZone.Location = rdr["Location"].ToString();
                        thisDropZone.Comments = rdr["Comments"].ToString();
                        thisDropZone.Restrictions = rdr["Restrictions"].ToString();
                        thisDropZone.DropZoneNumber = rdr["DropZoneNumber"].ToString();
                        thisDropZone.DropZoneDescription = rdr["DropZoneDescription"].ToString();
                        thisDropZone.City = rdr["City"].ToString();
                        thisDropZone.PDPhoneNumber = rdr["PDPhoneNumber"].ToString();
                        thisDropZone.Capacity = Convert.ToInt32(rdr["Capacity"]);
                        thisDropZone.Position = SqlGeography.Deserialize(rdr.GetSqlBytes(9));
                        DataClasses.GlobalData.DropZones.Add(thisDropZone);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    logger.LogEvent(DateTime.Now.ToString() + Environment.NewLine + "Error Loading Drop Zones" +
                        Environment.NewLine + ex.ToString(), true);
                }
            }
        }
         * */

        #endregion
    }
}