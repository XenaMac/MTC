using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class BeatSegments
    {
        public static List<beatSegmentPolygonData> bsPolyList = new List<beatSegmentPolygonData>();

        public static void LoadSegments() {
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.loadBeatSegmentData();
        }

        #region " Updater "

        public static void addBeatSegment(beatSegmentPolygonData b) {
            for (int i = bsPolyList.Count - 1; i >= 0; i--) {
                if (bsPolyList[i].ID == b.ID) {
                    bsPolyList.RemoveAt(i);
                }
            }
            //if (b.maxLat == 0 || b.maxLon == 0 || b.minLat == 0 || b.minLon == 0)
            //{
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
           // }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateBeatSegmentPolygon(b);
            bsPolyList.Add(b);
        }

        public static void deleteBeatSegmentPolygonData(Guid id)
        {
            for (int i = bsPolyList.Count - 1; i >= 0; i--)
            {
                if (bsPolyList[i].ID == id)
                {
                    bsPolyList.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteBeatSegmentData(id);
        }

        #endregion

        #region " intersects "

        public static bool checkBeatSegmentIntersect(double lat, double lon, string segID) {
            bool inPoly = false;

            var spList = from sp in bsPolyList
                         where sp.segmentID == segID
                         select sp;
            foreach (beatSegmentPolygonData b in spList) {
                if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                {
                    //possible match, check it
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
            }

            return inPoly;
        }

        public static string findBeatSegByBeat(string beatID, double lat, double lon) {
            string segName = "NOT FOUND";
            
            try {
                var biFound = from bi in Beats.beatInfos
                              where bi.BeatID == beatID
                              select bi;
                foreach (beatInformation bi in biFound) {
                    var spList = from sp in bsPolyList
                                 where bi.beatSegmentList.Contains(sp.segmentID)
                                 select sp;
                    foreach (beatSegmentPolygonData b in spList)
                    {
                        if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                        {
                            //possible match, check it
                            int i;
                            int j = b.geoFence.Count - 1;
                            for (i = 0; i < b.geoFence.Count; i++)
                            {
                                if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                    || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                                {
                                    if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                    {
                                        segName = b.segmentDescription;
                                    }
                                }
                                j = i;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }

            return segName;
        }

        public static string findBeatSeg(double lat, double lon)
        {
            string segName = "NOT FOUND";

            try {
                foreach (beatSegmentPolygonData b in bsPolyList) {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        int i;
                        int j = b.geoFence.Count - 1;
                        for (i = 0; i < b.geoFence.Count; i++)
                        {
                            if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                            {
                                if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                {
                                    segName = b.segmentID;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return segName;
        }

        #endregion
    }

    #region " object definition "

   //Moved to ITowTruckService.cs

    #endregion
}
