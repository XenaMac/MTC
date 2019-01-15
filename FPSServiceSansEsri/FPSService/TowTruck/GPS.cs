using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Types;

namespace FPSService.TowTruck
{
    public class GPS : TowTruckMessage
    {
        public int Id { get; set; } //Message ID, sent from truck
        public double Speed { get; set; } //Current speed
        public double Lat { get; set; } //Current Lat
        public double Lon { get; set; } //Current Lon
        public double MaxSpd { get; set; } //Max Speed
        public double MLat { get; set; } //Lat for Max Speed
        public double MLon { get; set; } //Lon for Max Speed
        public string Time { get; set; } //GPS Time
        public string Status { get; set; } //GPS Status (Valid, No Signal, Error)
        public double DOP { get; set; } //Dilution of precision x<6 = low accuracy
        public int Alt { get; set; }
        public int Head { get; set; } //compass heading
        public string Cell { get; set; }
        public Guid BeatSegmentID { get; set; } //current beat segment, calculated on arrival by service

        public SqlGeography Position { get; set; } //SQL GPS data, this is how the database sees the data
        public Guid BeatID { get; set; } //current beat, calculated on arrival by service
    }
}