using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class Beats2
    {
        public static List<beatPolygonData> beatList = new List<beatPolygonData>();

        #region " updater "

        public static void addBeatPolygonData(beatPolygonData b)
        {
            for (int i = beatList.Count - 1; i >= 0; i--)
            {
                if (beatList[i].ID == b.ID)
                {
                    beatList.RemoveAt(i);
                }
            }
            beatList.Add(b);
        }

        #endregion

        #region " intersects "

        public static bool checkBeatIntersect(double lat, double lon, string beatID)
        {
            bool inPoly = false;

            try
            {
                //limit beats searched to specific beat
                var spList = from sp in beatList
                             where sp.BeatID == beatID
                             select sp;
                foreach (beatPolygonData b in spList)
                {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        if (b.geoType.ToUpper() == "POLYGON")
                        {
                            int i;
                            int j = b.geoFence.Count - 1;
                            for (i = 0; i < b.geoFence.Count; i++)
                            {
                                if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                    || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                                {
                                    if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                    {
                                        inPoly = !inPoly;
                                    }
                                }
                                j = i;
                            }
                        }
                        else if (b.geoType.ToUpper() == "CIRCLE")
                        {
                            Position center = new Position();
                            center.Latitude = b.geoFence[0].lat;
                            center.Longitude = b.geoFence[0].lon;
                            Position truck = new Position();
                            Haversine h = new Haversine();
                            double distance = h.Distance(center, truck, DistanceType.Kilometers);
                            if (distance < (b.radius / 1000)) //radii are in meters, divide by 1k to get kilometers. Yay metric system!
                            {
                                inPoly = !inPoly;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return inPoly;

        }

        public static string findBeat(double lat, double lon)
        {
            string beatName = "NOT FOUND";

            try
            {
                foreach (beatPolygonData b in beatList)
                {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        if (b.geoType.ToUpper() == "POLYGON")
                        {
                            int i;
                            int j = b.geoFence.Count - 1;
                            for (i = 0; i < b.geoFence.Count; i++)
                            {
                                if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                    || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                                {
                                    if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                    {
                                        beatName = b.BeatID;
                                    }
                                }
                                j = i;
                            }
                        }
                        else if (b.geoType.ToUpper() == "CIRCLE")
                        {
                            Position center = new Position();
                            center.Latitude = b.geoFence[0].lat;
                            center.Longitude = b.geoFence[0].lon;
                            Position truck = new Position();
                            Haversine h = new Haversine();
                            double distance = h.Distance(center, truck, DistanceType.Kilometers);
                            if (distance < (b.radius / 1000)) //radii are in meters, divide by 1k to get kilometers. Yay metric system!
                            {
                                beatName = b.BeatID;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return beatName;
        }

        #endregion
    }

    #region " object definition "

    public class beatPolygonData
    {
        public Guid ID { get; set; }
        public string BeatID { get; set; }
        public string notes { get; set; }
        public List<latLon> geoFence { get; set; }
        public string geoType { get; set; }
        public double radius { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }

    #endregion
}