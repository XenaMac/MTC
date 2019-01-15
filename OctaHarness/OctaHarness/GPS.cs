using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OctaHarness
{
    public class GPS
    {
        public int Id { get; set; } //Message ID, sent from truck
        public double Speed { get; set; } //Current speed
        public double Lat { get; set; } //Current Lat
        public double Lon { get; set; } //Current Lon
        public double MaxSpd { get; set; } //Max Speed
        public double MLat { get; set; } //Lat for Max Speed
        public double MLon { get; set; } //Lon for Max Speed
        public DateTime Time { get; set; } //GPS Time
        public string Status { get; set; } //GPS Status (Valid, No Signal, Error)
        public double DOP { get; set; } //Dilution of precision x<6 = low accuracy
        public int Alt { get; set; }
        public int Head { get; set; } //compass heading
        public string Cell { get; set; }
    }
}
