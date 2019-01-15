using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class GPSMessage : TowTruckMessage
    {
        GPS gps = new GPS();
        public GPSMessage()
        { }
        public GPSMessage(GPS _gps)
        {
            gps = _gps;
        }
        public override string XmlMessage
        {
            get
            {
                TowTruckXMLWriter writer = new TowTruckXMLWriter();

                // put together a start packet command
                writer.WriteStartElement("GPS");
                writer.WriteElementString("Id", ID.ToString());
                writer.WriteElementString("Latitude", gps.Lat.ToString());
                writer.WriteElementString("Longitude", gps.Lon.ToString());
                writer.WriteElementString("Speed", gps.Speed.ToString());
                writer.WriteElementString("DOP", gps.DOP.ToString());
                writer.WriteElementString("Time", String.Format("{0:MM/dd/yyyy HH:mm:ss}", gps.Time));
                writer.WriteElementString("Status", gps.Status);
                writer.WriteElementString("Head", gps.Head.ToString());
                writer.WriteElementString("BeatSegment", gps.BeatSegmentID.ToString());
                writer.WriteEndElement();

                return writer.XmlString;
            }
        }

        public override void AddAttribute(string attribute, string value)
        {
            try
            {
                if (attribute == "Latitude")
                {
                    gps.Lat = Convert.ToDouble(value);
                }
                else if (attribute == "Longitude")
                {
                    gps.Lon = Convert.ToDouble(value);
                }
                else if (attribute == "Speed")
                {
                    gps.Speed = Convert.ToDouble(value);
                }
                else if (attribute == "DOP")
                {
                    gps.DOP = Convert.ToDouble(value);
                }
                else if (attribute == "Time")
                {
                    gps.Time = value;
                }
                else if (attribute == "Status")
                {
                    gps.Status = value;
                }
                else if (attribute == "Head")
                {
                    gps.Head = Convert.ToInt32(value);
                }
                else if (attribute == "BeatSegmentID")
                {
                    gps.BeatSegmentID = new Guid(value);
                }
                else if (attribute == "BeatID")
                {
                    gps.BeatID = new Guid(value);
                }
            }
            catch
            { }
        }

        public override string AckString
        {
            get
            {
                TowTruckXMLWriter writer = new TowTruckXMLWriter();
                writer.WriteStartElement("GPSAck");
                writer.WriteElementString("Id", messageNumber);
                writer.WriteEndElement();
                return writer.XmlString;
            }
        }
    }
}