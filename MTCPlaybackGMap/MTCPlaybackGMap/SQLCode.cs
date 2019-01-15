using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using GMap.NET.WindowsForms;
using GMap.NET;
using GMap;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace MTCPlaybackGMap
{
    public class SQLCode
    {
        private string connStr = string.Empty;
        private string connMain = string.Empty;
        private string connAcc = string.Empty;
        public SQLCode()
        {
            connStr = ConfigurationManager.AppSettings["db"].ToString();
            connMain = ConfigurationManager.AppSettings["mainDb"].ToString();
            connAcc = ConfigurationManager.AppSettings["accDb"].ToString();
        }

        #region  " Get the status data "

        public List<statusData> getStatusData(string TruckNumber, DateTime dtStart, DateTime dtEnd)
        {
            List<statusData> status = new List<statusData>();
            List<Guid> runIDs = new List<Guid>();
            foreach (playBackRow pbr in globalData.playbackData)
            {
                if (!runIDs.Contains(pbr.runID))
                {
                    runIDs.Add(pbr.runID);
                }
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connMain))
                {
                    conn.Open();

                    string SQL = "SELECT StatusName, SUM(StatusMins) as 'Time' FROM TruckStatus WHERE RunID IN (";
                    string where = string.Empty;
                    foreach (Guid g in runIDs)
                    {
                        where += "'" + g.ToString() + "',";
                    }
                    where = where.Substring(0, where.Length - 1);
                    SQL += where + ") AND StatusName NOT IN ('Logoff', 'LogOn') GROUP BY StatusName";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    //c.monday = rdr.IsDBNull(1) ? 0 : Convert.ToInt32(rdr["monday"]);
                    while (rdr.Read())
                    {
                        statusData sd = new statusData();
                        sd.Status = rdr.IsDBNull(0) ? "ON PATROL" : rdr[0].ToString();
                        sd.Minutes = rdr.IsDBNull(1) ? 0 : Convert.ToInt32(rdr[1]);
                        status.Add(sd);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                double totalTime = 0;
                foreach (statusData sd in status)
                {
                    totalTime += (double)sd.Minutes;
                }
                foreach (statusData sd in status)
                {
                    double pct = (sd.Minutes / totalTime) * 100;
                    sd.Percentage = Math.Round(pct);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return status;
        }

        #endregion

        #region " Get lists "

        private string getSQL(string thing, DateTime dtStart, DateTime dtEnd)
        {

                switch (thing.ToUpper())
                {
                    case "TRUCKS":
                        return "SELECT DISTINCT TruckNumber FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                            dtStart.ToString() + "' AND '" + dtEnd.ToString() + "' | ORDER BY TruckNumber";
                    case "DRIVERS":
                        return "SELECT DISTINCT Driver FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                            dtStart.ToString() + "' and '" + dtEnd.ToString() + "' | ORDER BY Driver";
                    case "CALLSIGNS":
                        return "SELECT DISTINCT CallSign FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                            dtStart.ToString() + "' and '" + dtEnd.ToString() + "' | ORDER BY CallSign";
                    case "CONTRACTORS":
                        return "SELECT DISTINCT Contractor FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                            dtStart.ToString() + "' and '" + dtEnd.ToString() + "' | ORDER BY Contractor";
                    case "BEATS":
                        return "SELECT DISTINCT Beat FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                            dtStart.ToString() + "' and '" + dtEnd.ToString() + "' | ORDER BY Beat";
                    default:
                        return string.Empty;
                }

        }

        public List<string> getData(DateTime dtStart, DateTime dtEnd, string thing, string extraWhere = "NA")
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            List<string> things = new List<string>();
            things.Add("SELECT");
            try
            {
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();

                    string SQL = getSQL(thing, dtStart, dtEnd);
                    if (extraWhere == "NA")
                    {
                        SQL = SQL.Replace(" |", "");
                    }
                    else
                    {
                        SQL = SQL.Replace(" |", extraWhere);
                    }
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandTimeout = 0;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        things.Add(rdr[0].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return things;
        }

        

        #endregion

        #region " Get Playback Data "

        public void loadTruckPlaybackData(string TruckNumber, DateTime dtStart, DateTime dtEnd)
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();
                    //List<playBackRow> thisTruck = new List<playBackRow>();
                    string SQL = "select TimeStamp, TruckNumber, Driver, CallSign, Contractor, Status, Schedule, Beat, BeatSegment, Speed, Lat, Lon," +
                        "Heading, HasAlarm, AlarmInfo, RunID from playbackdata WHERE TruckNumber = '" + TruckNumber + "'" +
                        " AND TimeStamp BETWEEN '" + dtStart + "' AND '" + dtEnd + "'" +
                        " ORDER BY TimeStamp";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        playBackRow row = new playBackRow();

                        row.timeStamp = rdr.IsDBNull(0) ? Convert.ToDateTime("01/01/2001 00:00:00") : Convert.ToDateTime(rdr["TimeStamp"].ToString());
                        row.TruckNumber = rdr["TruckNumber"].ToString();
                        row.Driver = rdr.IsDBNull(2) ? "NO DRIVER" : rdr["Driver"].ToString();
                        row.CallSign = rdr.IsDBNull(3) ? "NO CS" : rdr["CallSign"].ToString();
                        row.Contractor = rdr.IsDBNull(4) ? "NO CO" : rdr["Contractor"].ToString();
                        row.Status = rdr.IsDBNull(5) ? "NO ST" : rdr["Status"].ToString();
                        row.Schedule = rdr.IsDBNull(6) ? "NO SC" : rdr["Schedule"].ToString();
                        row.Beat = rdr.IsDBNull(7) ? "NO BT" : rdr["Beat"].ToString();
                        row.BeatSegment = rdr.IsDBNull(8) ? "NO BS" : rdr["BeatSegment"].ToString();
                        row.Speed = rdr.IsDBNull(9) ? 999 : Convert.ToInt32(rdr["Speed"]);
                        row.Lat = rdr.IsDBNull(10) ? 0.0 : Convert.ToDouble(rdr["Lat"]);
                        row.Lon = rdr.IsDBNull(11) ? 0.0 : Convert.ToDouble(rdr["Lon"]);
                        row.Heading = rdr.IsDBNull(12) ? 361 : Convert.ToInt32(rdr["Heading"]);
                        int hasAlarms = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr["HasAlarm"]);
                        if (hasAlarms == 0)
                        {
                            row.HasAlarms = false;
                        }
                        else
                        {
                            row.HasAlarms = true;
                        }
                        row.AlarmInfo = rdr.IsDBNull(14) ? "NA" : rdr["AlarmInfo"].ToString();
                        row.runID = rdr.IsDBNull(15) ? new Guid("") : new Guid(rdr["RunID"].ToString());
                        globalData.playbackData.Add(row);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadCallSignPlayback(string CallSign, DateTime dtStart, DateTime dtEnd)
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();
                    //List<playBackRow> thisTruck = new List<playBackRow>();
                    string SQL = "select TimeStamp, TruckNumber, Driver, CallSign, Contractor, Status, Schedule, Beat, BeatSegment, Speed, Lat, Lon," +
                        "Heading, HasAlarm, AlarmInfo, RunID from playbackdata WHERE CallSign = '" + CallSign + "'" +
                        " AND TimeStamp BETWEEN '" + dtStart + "' AND '" + dtEnd + "'" +
                        " ORDER BY TimeStamp";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        playBackRow row = new playBackRow();

                        row.timeStamp = rdr.IsDBNull(0) ? Convert.ToDateTime("01/01/2001 00:00:00") : Convert.ToDateTime(rdr["TimeStamp"].ToString());
                        row.TruckNumber = rdr["TruckNumber"].ToString();
                        row.Driver = rdr.IsDBNull(2) ? "NO DRIVER" : rdr["Driver"].ToString();
                        row.CallSign = rdr.IsDBNull(3) ? "NO CS" : rdr["CallSign"].ToString();
                        row.Contractor = rdr.IsDBNull(4) ? "NO CO" : rdr["Contractor"].ToString();
                        row.Status = rdr.IsDBNull(5) ? "NO ST" : rdr["Status"].ToString();
                        row.Schedule = rdr.IsDBNull(6) ? "NO SC" : rdr["Schedule"].ToString();
                        row.Beat = rdr.IsDBNull(7) ? "NO BT" : rdr["Beat"].ToString();
                        row.BeatSegment = rdr.IsDBNull(8) ? "NO BS" : rdr["BeatSegment"].ToString();
                        row.Speed = rdr.IsDBNull(9) ? 999 : Convert.ToInt32(rdr["Speed"]);
                        row.Lat = rdr.IsDBNull(10) ? 0.0 : Convert.ToDouble(rdr["Lat"]);
                        row.Lon = rdr.IsDBNull(11) ? 0.0 : Convert.ToDouble(rdr["Lon"]);
                        row.Heading = rdr.IsDBNull(12) ? 361 : Convert.ToInt32(rdr["Heading"]);
                        int hasAlarms = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr["HasAlarm"]);
                        if (hasAlarms == 0)
                        {
                            row.HasAlarms = false;
                        }
                        else
                        {
                            row.HasAlarms = true;
                        }
                        row.AlarmInfo = rdr.IsDBNull(14) ? "NA" : rdr["AlarmInfo"].ToString();
                        row.runID = rdr.IsDBNull(15) ? new Guid("") : new Guid(rdr["RunID"].ToString());
                        globalData.playbackData.Add(row);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadDriverPlayback(string DriverName, DateTime dtStart, DateTime dtEnd)
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();
                    //List<playBackRow> thisTruck = new List<playBackRow>();
                    string SQL = "select TimeStamp, TruckNumber, Driver, CallSign, Contractor, Status, Schedule, Beat, BeatSegment, Speed, Lat, Lon," +
                        "Heading, HasAlarm, AlarmInfo, RunID from playbackdata WHERE Driver = '" + DriverName + "'" +
                        " AND TimeStamp BETWEEN '" + dtStart + "' AND '" + dtEnd + "'" +
                        " ORDER BY TimeStamp";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        playBackRow row = new playBackRow();

                        row.timeStamp = rdr.IsDBNull(0) ? Convert.ToDateTime("01/01/2001 00:00:00") : Convert.ToDateTime(rdr["TimeStamp"].ToString());
                        row.TruckNumber = rdr["TruckNumber"].ToString();
                        row.Driver = rdr.IsDBNull(2) ? "NO DRIVER" : rdr["Driver"].ToString();
                        row.CallSign = rdr.IsDBNull(3) ? "NO CS" : rdr["CallSign"].ToString();
                        row.Contractor = rdr.IsDBNull(4) ? "NO CO" : rdr["Contractor"].ToString();
                        row.Status = rdr.IsDBNull(5) ? "NO ST" : rdr["Status"].ToString();
                        row.Schedule = rdr.IsDBNull(6) ? "NO SC" : rdr["Schedule"].ToString();
                        row.Beat = rdr.IsDBNull(7) ? "NO BT" : rdr["Beat"].ToString();
                        row.BeatSegment = rdr.IsDBNull(8) ? "NO BS" : rdr["BeatSegment"].ToString();
                        row.Speed = rdr.IsDBNull(9) ? 999 : Convert.ToInt32(rdr["Speed"]);
                        row.Lat = rdr.IsDBNull(10) ? 0.0 : Convert.ToDouble(rdr["Lat"]);
                        row.Lon = rdr.IsDBNull(11) ? 0.0 : Convert.ToDouble(rdr["Lon"]);
                        row.Heading = rdr.IsDBNull(12) ? 361 : Convert.ToInt32(rdr["Heading"]);
                        int hasAlarms = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr["HasAlarm"]);
                        if (hasAlarms == 0)
                        {
                            row.HasAlarms = false;
                        }
                        else
                        {
                            row.HasAlarms = true;
                        }
                        row.AlarmInfo = rdr.IsDBNull(14) ? "NA" : rdr["AlarmInfo"].ToString();
                        row.runID = rdr.IsDBNull(15) ? new Guid("") : new Guid(rdr["RunID"].ToString());
                        globalData.playbackData.Add(row);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadContractorPlayback(string Contractor, DateTime dtStart, DateTime dtEnd)
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();
                    //List<playBackRow> thisTruck = new List<playBackRow>();
                    string SQL = "select TimeStamp, TruckNumber, Driver, CallSign, Contractor, Status, Schedule, Beat, BeatSegment, Speed, Lat, Lon," +
                        "Heading, HasAlarm, AlarmInfo, RunID from playbackdata WHERE Contractor = '" + Contractor + "'" +
                        " AND TimeStamp BETWEEN '" + dtStart + "' AND '" + dtEnd + "'" +
                        " ORDER BY TimeStamp";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        playBackRow row = new playBackRow();

                        row.timeStamp = rdr.IsDBNull(0) ? Convert.ToDateTime("01/01/2001 00:00:00") : Convert.ToDateTime(rdr["TimeStamp"].ToString());
                        row.TruckNumber = rdr["TruckNumber"].ToString();
                        row.Driver = rdr.IsDBNull(2) ? "NO DRIVER" : rdr["Driver"].ToString();
                        row.CallSign = rdr.IsDBNull(3) ? "NO CS" : rdr["CallSign"].ToString();
                        row.Contractor = rdr.IsDBNull(4) ? "NO CO" : rdr["Contractor"].ToString();
                        row.Status = rdr.IsDBNull(5) ? "NO ST" : rdr["Status"].ToString();
                        row.Schedule = rdr.IsDBNull(6) ? "NO SC" : rdr["Schedule"].ToString();
                        row.Beat = rdr.IsDBNull(7) ? "NO BT" : rdr["Beat"].ToString();
                        row.BeatSegment = rdr.IsDBNull(8) ? "NO BS" : rdr["BeatSegment"].ToString();
                        row.Speed = rdr.IsDBNull(9) ? 999 : Convert.ToInt32(rdr["Speed"]);
                        row.Lat = rdr.IsDBNull(10) ? 0.0 : Convert.ToDouble(rdr["Lat"]);
                        row.Lon = rdr.IsDBNull(11) ? 0.0 : Convert.ToDouble(rdr["Lon"]);
                        row.Heading = rdr.IsDBNull(12) ? 361 : Convert.ToInt32(rdr["Heading"]);
                        int hasAlarms = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr["HasAlarm"]);
                        if (hasAlarms == 0)
                        {
                            row.HasAlarms = false;
                        }
                        else
                        {
                            row.HasAlarms = true;
                        }
                        row.AlarmInfo = rdr.IsDBNull(14) ? "NA" : rdr["AlarmInfo"].ToString();
                        row.runID = rdr.IsDBNull(15) ? new Guid("") : new Guid(rdr["RunID"].ToString());
                        globalData.playbackData.Add(row);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void loadBeatPlayback(string Beat, DateTime dtStart, DateTime dtEnd)
        {
            string connS = string.Empty;
            TimeSpan ts = dtStart - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                connS = connAcc;
            }
            else
            {
                connS = connStr;
            }
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connS))
                {
                    conn.Open();
                    //List<playBackRow> thisTruck = new List<playBackRow>();
                    string SQL = "select TimeStamp, TruckNumber, Driver, CallSign, Contractor, Status, Schedule, Beat, BeatSegment, Speed, Lat, Lon," +
                        "Heading, HasAlarm, AlarmInfo, RunID from playbackdata WHERE Beat = '" + Beat + "'" +
                        " AND TimeStamp BETWEEN '" + dtStart + "' AND '" + dtEnd + "'" +
                        " ORDER BY TimeStamp";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        playBackRow row = new playBackRow();

                        row.timeStamp = rdr.IsDBNull(0) ? Convert.ToDateTime("01/01/2001 00:00:00") : Convert.ToDateTime(rdr["TimeStamp"].ToString());
                        row.TruckNumber = rdr["TruckNumber"].ToString();
                        row.Driver = rdr.IsDBNull(2) ? "NO DRIVER" : rdr["Driver"].ToString();
                        row.CallSign = rdr.IsDBNull(3) ? "NO CS" : rdr["CallSign"].ToString();
                        row.Contractor = rdr.IsDBNull(4) ? "NO CO" : rdr["Contractor"].ToString();
                        row.Status = rdr.IsDBNull(5) ? "NO ST" : rdr["Status"].ToString();
                        row.Schedule = rdr.IsDBNull(6) ? "NO SC" : rdr["Schedule"].ToString();
                        row.Beat = rdr.IsDBNull(7) ? "NO BT" : rdr["Beat"].ToString();
                        row.BeatSegment = rdr.IsDBNull(8) ? "NO BS" : rdr["BeatSegment"].ToString();
                        row.Speed = rdr.IsDBNull(9) ? 999 : Convert.ToInt32(rdr["Speed"]);
                        row.Lat = rdr.IsDBNull(10) ? 0.0 : Convert.ToDouble(rdr["Lat"]);
                        row.Lon = rdr.IsDBNull(11) ? 0.0 : Convert.ToDouble(rdr["Lon"]);
                        row.Heading = rdr.IsDBNull(12) ? 361 : Convert.ToInt32(rdr["Heading"]);
                        int hasAlarms = rdr.IsDBNull(13) ? 0 : Convert.ToInt32(rdr["HasAlarm"]);
                        if (hasAlarms == 0)
                        {
                            row.HasAlarms = false;
                        }
                        else
                        {
                            row.HasAlarms = true;
                        }
                        row.AlarmInfo = rdr.IsDBNull(14) ? "NA" : rdr["AlarmInfo"].ToString();
                        row.runID = rdr.IsDBNull(15) ? new Guid("") : new Guid(rdr["RunID"].ToString());
                        globalData.playbackData.Add(row);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        #endregion
    }

    public class GMapCustomImageMarker : GMapMarker
    {
        private Image _image;
        public GMapCustomImageMarker(Image Image, PointLatLng p)
            : base(p)
        {
            _image = Image;
        }

        public override void OnRender(Graphics g)
        {
            g.DrawImage(_image, new Point(LocalPosition.X - _image.Width / 2, LocalPosition.Y - _image.Height / 2));
        }
    }

    public static class globalData
    {
        public static List<playBackRow> playbackData = new List<playBackRow>();
    }

    public class statusData
    {
        public string Status { get; set; }
        public int Minutes { get; set; }
        public double Percentage { get; set; }
    }

    public class playBackRow
    {
        public DateTime timeStamp { get; set; }
        public string TruckNumber { get; set; }
        public string Driver { get; set; }
        public string CallSign { get; set; }
        public string Contractor { get; set; }
        public string Status { get; set; }
        public string Schedule { get; set; }
        public string Beat { get; set; }
        public string BeatSegment { get; set; }
        public int Speed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Heading { get; set; }
        public bool HasAlarms { get; set; }
        public string AlarmInfo { get; set; }
        public Guid runID { get; set; }
    }
}
