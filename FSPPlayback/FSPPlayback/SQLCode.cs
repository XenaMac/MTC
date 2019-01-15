using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace FSPPlayback
{
    class SQLCode
    {
        string strConn = "Initial Catalog=fsp;Data Source=38.124.164.211\\OCTA,5815;User Id=sa;Password=C@pt@1n@mer1c@";

        public List<GPSTrack> GetTracking(string VehicleID, DateTime dtStart, DateTime dtEnd, bool OnlyLoggedOn)
        {
            List<GPSTrack> myGPS = new List<GPSTrack>();
            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPlaybackByVehicle", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int LogonVal = 0;
                    if (OnlyLoggedOn == true)
                    {
                        LogonVal = 1;
                    }
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                    cmd.Parameters.AddWithValue("@dtStart", dtStart.ToString());
                    cmd.Parameters.AddWithValue("@dtEnd", dtEnd.ToString());
                    cmd.Parameters.AddWithValue("@logonOnly", LogonVal.ToString());
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int i = 0;
                    while (rdr.Read())
                    {
                        GPSTrack thisGPS = new GPSTrack();
                        thisGPS.ID = i;
                        thisGPS.Direction = double.Parse(rdr["Direction"].ToString());
                        thisGPS.VehicleStatus = rdr["VehicleStatus"].ToString();
                        thisGPS.timeStamp = Convert.ToDateTime(rdr["timeStamp"].ToString());
                        thisGPS.VehicleID = rdr["VehicleID"].ToString();
                        thisGPS.Speed = double.Parse(rdr["Speed"].ToString());
                        thisGPS.DriverName = rdr["Driver Name"].ToString();
                        thisGPS.ContractCompanyName = rdr["ContractCompanyName"].ToString();
                        thisGPS.IPAddress = rdr["IPAddress"].ToString();
                        thisGPS.Lat = double.Parse(rdr["Lat"].ToString());
                        thisGPS.Lon = double.Parse(rdr["Lon"].ToString());
                        thisGPS.SpeedingValue = double.Parse(rdr["SpeedingValue"].ToString());
                        thisGPS.SpeedingTime = Convert.ToDateTime(rdr["SpeedingTime"].ToString());
                        thisGPS.OutOfBoundsMessage = rdr["OutofBoundsMessage"].ToString();
                        thisGPS.BeatNumber = rdr["Beat"].ToString();
                        i += 1;
                        myGPS.Add(thisGPS);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                return myGPS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void LoadTrackingDataByBeat(string BeatNumber, DateTime dtStart, DateTime dtEnd, bool OnlyLoggedOn)
        {
            try
            {
                GlobalData.allTrack.Clear();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPlaybackByBeat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int LogonVal = 0;
                    if (OnlyLoggedOn == true)
                    {
                        LogonVal = 1;
                    }

                    cmd.Parameters.AddWithValue("@BeatNumber", BeatNumber);
                    cmd.Parameters.AddWithValue("@dtStart", dtStart.ToString());
                    cmd.Parameters.AddWithValue("@dtEnd", dtEnd.ToString());
                    cmd.Parameters.AddWithValue("@logonOnly", LogonVal.ToString());
                    cmd.CommandTimeout = 0;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int i = 0;
                    while (rdr.Read())
                    {
                        GPSTrack thisGPS = new GPSTrack();
                        thisGPS.ID = i;
                        thisGPS.Direction = double.Parse(rdr["Direction"].ToString());
                        thisGPS.VehicleStatus = rdr["VehicleStatus"].ToString();
                        thisGPS.timeStamp = Convert.ToDateTime(rdr["timeStamp"].ToString());
                        thisGPS.VehicleID = rdr["VehicleID"].ToString();
                        thisGPS.Speed = double.Parse(rdr["Speed"].ToString());
                        thisGPS.DriverName = rdr["Driver Name"].ToString();
                        thisGPS.ContractCompanyName = rdr["ContractCompanyName"].ToString();
                        thisGPS.IPAddress = rdr["IPAddress"].ToString();
                        thisGPS.Lat = double.Parse(rdr["Lat"].ToString());
                        thisGPS.Lon = double.Parse(rdr["Lon"].ToString());
                        thisGPS.SpeedingValue = double.Parse(rdr["SpeedingValue"].ToString());
                        thisGPS.SpeedingTime = Convert.ToDateTime(rdr["SpeedingTime"].ToString());
                        thisGPS.OutOfBoundsMessage = rdr["OutofBoundsMessage"].ToString();
                        thisGPS.BeatNumber = rdr["Beat"].ToString();
                        i += 1;
                        GlobalData.allTrack.Add(thisGPS);
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

        public void LoadTrackingData(string VehicleID, DateTime dtStart, DateTime dtEnd, bool OnlyLoggedOn)
        {
            try
            {
                GlobalData.allTrack.Clear();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPlaybackByVehicle", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int LogonVal = 0;
                    if (OnlyLoggedOn == true)
                    {
                        LogonVal = 1;
                    }
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                    cmd.Parameters.AddWithValue("@dtStart", dtStart.ToString());
                    cmd.Parameters.AddWithValue("@dtEnd", dtEnd.ToString());
                    cmd.Parameters.AddWithValue("@logonOnly", LogonVal.ToString());
                    cmd.CommandTimeout = 0;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int i = 0;
                    while (rdr.Read())
                    {
                        GPSTrack thisGPS = new GPSTrack();
                        thisGPS.ID = i;
                        thisGPS.Direction = double.Parse(rdr["Direction"].ToString());
                        thisGPS.VehicleStatus = rdr["VehicleStatus"].ToString();
                        thisGPS.timeStamp = Convert.ToDateTime(rdr["timeStamp"].ToString());
                        thisGPS.VehicleID = rdr["VehicleID"].ToString();
                        thisGPS.Speed = double.Parse(rdr["Speed"].ToString());
                        thisGPS.DriverName = rdr["Driver Name"].ToString();
                        thisGPS.ContractCompanyName = rdr["ContractCompanyName"].ToString();
                        thisGPS.IPAddress = rdr["IPAddress"].ToString();
                        thisGPS.Lat = double.Parse(rdr["Lat"].ToString());
                        thisGPS.Lon = double.Parse(rdr["Lon"].ToString());
                        thisGPS.SpeedingValue = double.Parse(rdr["SpeedingValue"].ToString());
                        thisGPS.SpeedingTime = Convert.ToDateTime(rdr["SpeedingTime"].ToString());
                        thisGPS.OutOfBoundsMessage = rdr["OutofBoundsMessage"].ToString();
                        thisGPS.BeatNumber = rdr["Beat"].ToString();
                        i += 1;
                        GlobalData.allTrack.Add(thisGPS);
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

        public List<string> GetBeats(DateTime dtStart, DateTime dtEnd)
        {
            List<string> myBeats = new List<string>();
            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPlaybackBeats", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dtStart", dtStart.ToString());
                    cmd.Parameters.AddWithValue("@dtEnd", dtEnd.ToString());
                    cmd.CommandTimeout = 0;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        myBeats.Add(rdr[0].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            return myBeats;
        }

        public List<string> GetTrucks(DateTime dtStart, DateTime dtEnd)
        {
            List<string> myTrucks = new List<string>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("GetPlaybackTrucks", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dtStart", dtStart.ToString());
                    cmd.Parameters.AddWithValue("@dtEnd", dtEnd.ToString());
                    cmd.CommandTimeout = 0;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        myTrucks.Add(rdr[0].ToString());
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            return myTrucks;
        }
    }
}
