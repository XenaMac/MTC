using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace MTCPlaybackRev2
{
    public class SQLCode
    {
        private string connStr = string.Empty;
        private string connMain = string.Empty;
        public SQLCode()
        {
            connStr = ConfigurationManager.AppSettings["db"].ToString();
            connMain = ConfigurationManager.AppSettings["mainDb"].ToString();
        }

        public List<statusData> getStatusData(string TruckNumber, DateTime dtStart, DateTime dtEnd)
        {
            List<statusData> status = new List<statusData>();
            List<Guid> runIDs = new List<Guid>();
            foreach (playBackRow pbr in globalData.playbackData[0])
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
                    while (rdr.Read())
                    {
                        statusData sd = new statusData();
                        sd.statusName = rdr[0].ToString();
                        sd.statusMins = Convert.ToInt32(rdr[1]);
                        status.Add(sd);
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

            return status;
        }

        public List<string> getTrucks(DateTime dtStart, DateTime dtEnd)
        {
            List<string> trucks = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string SQL = "SELECT DISTINCT TruckNumber FROM PlaybackData WHERE [TimeStamp] BETWEEN '" +
                        dtStart.ToString() + "' AND '" + dtEnd.ToString() + "' ORDER BY TruckNumber";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        trucks.Add(rdr["TruckNumber"].ToString());
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
            return trucks;
        }

        public void loadPlaybackData(string TruckNumber, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                globalData.playbackData.Clear();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    List<playBackRow> thisTruck = new List<playBackRow>();
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
                        thisTruck.Add(row);
                    }
                    conn.Close();
                    globalData.playbackData.Add(thisTruck);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }

    public static class globalData
    {
        public static List<List<playBackRow>> playbackData = new List<List<playBackRow>>();
    }

    public class statusData
    {
        public string statusName { get; set; }
        public int statusMins { get; set; }
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
