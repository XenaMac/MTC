using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class Beats
    {
        public static List<beatPolygonData> beatList = new List<beatPolygonData>();
        public static List<beatData> beatData = new List<beatData>();
        public static List<beatInformation> beatInfos = new List<beatInformation>();
        public static List<string> freeways = new List<string>();

        /*
        public static void LoadBeats() {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            beatList.Clear();
            mySQL.loadBeatData();
            beatData.Clear();
            beatData = mySQL.getBeatData();
        }
        */
        public static void LoadBeatInfo() {
            SQL.SQLCode sql = new SQL.SQLCode();
            beatInfos.Clear();
            sql.loadBeatInformation();
            beatData.Clear();
            beatData = sql.getBeatData();
        }

        public static bool checkBeatIntersect(double lat, double lon, string beatID) {
            bool inPoly = false;
            try {
                var found = from bi in beatInfos
                                        where bi.BeatID == beatID
                                        select bi;
                foreach (beatInformation bi in found) {
                    var bsFound = from bs in BeatSegments.bsPolyList
                             where bi.beatSegmentList.Contains(bs.segmentID)
                             select bs;
                    foreach (beatSegmentPolygonData bsd in bsFound) {
                        if (lat >= bsd.minLat && lon <= bsd.maxLat && lon >= bsd.minLon && lon <= bsd.maxLat) {
                            int i;
                            int j = bsd.geoFence.Count - 1;
                            for (i = 0; i < bsd.geoFence.Count; i++)
                            {
                                if (bsd.geoFence[i].lon < lon && bsd.geoFence[j].lon >= lon
                                    || bsd.geoFence[j].lon < lon && bsd.geoFence[i].lon >= lon)
                                {
                                    if (bsd.geoFence[i].lat + (lon - bsd.geoFence[i].lon) / (bsd.geoFence[j].lon - bsd.geoFence[i].lon) * (bsd.geoFence[j].lat - bsd.geoFence[i].lat) < lat)
                                    {
                                        inPoly = !inPoly;
                                    }
                                }
                                j = i;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logging.EventLogger logger = new Logging.EventLogger();
                logger.LogEvent(ex.ToString(), true);
            }
            return inPoly;
        }

        public static string findBeat(double lat, double lon)
        {
            string beatName = "NOT FOUND";
            try
            {
                foreach (beatInformation bi in beatInfos)
                {
                    var bsFound = from bs in BeatSegments.bsPolyList
                                  where bi.beatSegmentList.Contains(bs.segmentID)
                                  select bs;
                    foreach (beatSegmentPolygonData bsd in bsFound)
                    {
                        if (lat >= bsd.minLat && lon <= bsd.maxLat && lon >= bsd.minLon && lon <= bsd.maxLat)
                        {
                            int i;
                            int j = bsd.geoFence.Count - 1;
                            for (i = 0; i < bsd.geoFence.Count; i++)
                            {
                                if (bsd.geoFence[i].lon < lon && bsd.geoFence[j].lon >= lon
                                    || bsd.geoFence[j].lon < lon && bsd.geoFence[i].lon >= lon)
                                {
                                    if (bsd.geoFence[i].lat + (lon - bsd.geoFence[i].lon) / (bsd.geoFence[j].lon - bsd.geoFence[i].lon) * (bsd.geoFence[j].lat - bsd.geoFence[i].lat) < lat)
                                    {
                                        beatInformation biRet = (beatInformation)bsFound;
                                        beatName = biRet.BeatName;
                                        return beatName;
                                    }
                                }
                                j = i;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.EventLogger logger = new Logging.EventLogger();
                logger.LogEvent(ex.ToString(), true);
            }
            return beatName;
        }

        #region " updater "

        public static void addBeatInfoData(beatInformation b) {
            for (int i = beatInfos.Count - 1; i >= 0; i--) {
                if (beatInfos[i].ID == b.ID) {
                    beatInfos.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateBeatInfos(b);
            beatInfos.Add(b);
        }

        public static void deleteBeatInfoData(Guid id)
        {
            for (int i = beatInfos.Count - 1; i >= 0; i--)
            {
                if (beatInfos[i].ID == id)
                {
                    beatInfos.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteBeatData(id);
        }

        //old pre modification when beats still held poly data
        //deprecated, use addBeatInfoData instead)
        /*
        public static void addBeatPolygonData(beatPolygonData b)
        {
            for (int i = beatList.Count - 1; i >= 0; i--)
            {
                if (beatList[i].ID == b.ID)
                {
                    beatList.RemoveAt(i);
                }
            }
            if (b.maxLat == 0 || b.maxLon == 0 || b.minLat == 0 || b.minLon == 0)
            {
                b.maxLat = b.geoFence[0].lat;
                b.maxLon = b.geoFence[0].lon;
                b.minLat = b.geoFence[0].lat;
                b.minLon = b.geoFence[0].lon;
                foreach (latLon ll in b.geoFence)
                {
                    if (ll.lat < b.minLat)
                    {
                        b.minLat = ll.lat;
                    }
                    if (ll.lat > b.maxLat)
                    {
                        b.maxLat = ll.lat;
                    }
                    if (ll.lon < b.minLon)
                    {
                        b.minLon = ll.lon;
                    }
                    if (ll.lon > b.maxLon)
                    {
                        b.maxLon = ll.lon;
                    }
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateBeatPolygon(b);
            beatList.Add(b);
        }

        public static void deleteBeatPolygonData(Guid id) {
            for (int i = beatList.Count - 1; i >= 0; i--) {
                if (beatList[i].ID == id) {
                    beatList.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteBeatData(id);
        }
        */
        #endregion

        #region " intersects "

        public static bool checkBeatIntersectOld(double lat, double lon, string beatID)
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

        public static string findBeatOld(double lat, double lon)
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

    //Moved to ITowTruckService.cs

    #endregion
}