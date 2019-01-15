using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSPPlayback
{
    public class GPSTrack
    {
        /*
        gps.Direction, gps.VehicleStatus, gps.[timeStamp], gps.VehicleID, gps.Speed, d.LastName + ', ' + d.FirstName as 'Driver Name', c.ContractCompanyName,
	gps.VehicleNumber, gps.Position.Lat, gps.Position.Long, gps.SpeedingValue, gps.SpeedingTime,
	gps.OutofBoundsMessage
         * */
        public int ID { get; set; }
        public double Direction { get; set; }
        public string VehicleStatus { get; set; }
        public DateTime timeStamp { get; set; }
        public string VehicleID { get; set; }
        public double Speed { get; set; }
        public string DriverName { get; set; }
        public string ContractCompanyName { get; set; }
        public string IPAddress { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double SpeedingValue { get; set; }
        public DateTime SpeedingTime { get; set; }
        public string OutOfBoundsMessage { get; set; }
        public string BeatNumber { get; set; }
    }
}
