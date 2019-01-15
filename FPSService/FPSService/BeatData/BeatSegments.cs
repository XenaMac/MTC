using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class BeatSegments
    {
        public static List<beatSegmentPolygonData> bsPolyList = new List<beatSegmentPolygonData>();

        #region " Updater "

        public static void addBeatSegment(beatSegmentPolygonData b) {
            for (int i = bsPolyList.Count - 1; i >= 0; i--) {
                if (bsPolyList[i].ID == b.ID) {
                    bsPolyList.RemoveAt(i);
                }
                bsPolyList.Add(b);
            }
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
                var spList = from sp in bsPolyList
                             where sp.beatID == beatID
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

    public class beatSegmentPolygonData {
        public Guid ID { get; set; }
        public string beatID { get; set; }
        public string segmentID { get; set; }
        public string segmentDescription { get; set; }
        public List<latLon> geoFence { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }

    #endregion
}
