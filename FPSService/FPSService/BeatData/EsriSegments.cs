using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;
using System.Configuration;

namespace FPSService.BeatData
{
    public static class EsriSegments
    {
        //DEPRECATED - REPLACED WITH BeatSegments
        private static string url = ConfigurationManager.AppSettings["EsriSegments"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriSegment> segPolygons = new List<esriSegment>();

        public static void LoadSegments() //Load the segment data into memory for faster querying
        {
            try
            {
                segPolygons.Clear();
                qt = new QueryTask(url);
                Query query = new Query();
                query.ReturnGeometry = true;
                //query.OutFields.AddRange(new string[] { "SegmentID", "BeatID", "SegmentDescription" }); //for beats
                query.OutFields.Add("*");
                query.Where = "1=1";
                qt.Execute(query);
                foreach (Graphic resultFeature in qt.LastResult.Features)
                {

                    string BeatID = string.Empty;
                    string SegmentID = string.Empty;
                    string SegDesc = string.Empty;
                    System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                    foreach (string theKey in allAttributes.Keys)
                    {
                        if (theKey == "BeatID")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { BeatID = theValue.ToString(); }

                        }
                        if (theKey == "BEAT_ID_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { BeatID = theValue.ToString(); }
                        }
                        if (theKey == "Beatsegmen")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { SegmentID = theValue.ToString(); }
                        }

                        if (theKey == "BeatSegmen")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { SegDesc = theValue.ToString(); }
                        }

                        if (theKey == "BeatSegeme")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            {
                                SegDesc = theValue.ToString();
                            }
                        }

                    }

                    esriSegment es = new esriSegment();
                    es.BeatID = BeatID;
                    es.SegmentID = SegmentID;
                    es.SegmentDescription = SegDesc;
                    es.segmentData = resultFeature;

                    ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)es.segmentData.Geometry;
                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {
                        es.maxLat = points[0].Y;
                        es.maxLon = points[0].X;
                        es.minLat = points[0].Y;
                        es.minLon = points[0].X;
                    }

                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {

                        for (int i = 0; i < points.Count(); i++)
                        {
                            //start setting min/max points
                            if (points[i].X > es.maxLon)
                            {
                                es.maxLon = points[i].X;
                            }
                            if (points[i].Y > es.maxLat)
                            {
                                es.maxLat = points[i].Y;
                            }
                            if (points[i].X < es.minLon)
                            {
                                es.minLon = points[i].X;
                            }
                            if (points[i].Y < es.minLat)
                            {
                                es.minLat = points[i].Y;
                            }
                        }
                    }
                    segPolygons.Add(es);
                }
            }
            catch (Exception ex)
            {
                Logging.EventLogger log = new Logging.EventLogger();
                log.LogEvent(ex.ToString(), true);
            }
        }

        public static bool checkEsriIntersect(double lat, double lon, string segID)
        {
            bool inPoly = false;
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in segPolygons
                         where sp.SegmentID == segID
                         select sp;
            foreach (esriSegment es in spList)
            {
                Polygon p = (Polygon)es.segmentData.Geometry;
                if (lat <= es.maxLat && lat >= es.minLat && lon <= es.maxLon && lon >= es.minLon) {
                    foreach (PointCollection points in p.Rings)
                    {
                        int i;
                        int j = points.Count - 1;
                        for (i = 0; i < points.Count; i++)
                        {
                            if (points[i].X < point.X && points[j].X >= point.X
                              || points[j].X < point.X && points[i].X >= point.X)
                            {
                                if (points[i].Y + (point.X - points[i].X) / (points[j].X - points[i].X) * (points[j].Y - points[i].Y) < point.Y)
                                {
                                    inPoly = !inPoly;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            return inPoly;
        }

        public static string findBeatSegByBeat(string beatID, double lat, double lon)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in segPolygons
                         where sp.BeatID == beatID
                         select sp;
            foreach (esriSegment es in spList)
            {
                Polygon p = (Polygon)es.segmentData.Geometry;
                if (lat <= es.maxLat && lat >= es.minLat && lon <= es.maxLon && lon >= es.minLon) {
                    foreach (PointCollection points in p.Rings)
                    {
                        for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
                        {
                            if (((points[i].Y > point.Y) != (points[j].Y > point.Y)) &&
                                (point.X < (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                            {
                                if (!string.IsNullOrEmpty(es.SegmentDescription))
                                {
                                    return es.SegmentDescription;
                                }
                                else
                                {
                                    return es.SegmentID;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            return segName;
        }

        public static string findBeatSeg(double lat, double lon)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            foreach (esriSegment es in segPolygons)
            {
                if (lat <= es.maxLat && lat >= es.minLat && lon <= es.maxLon && lon >= es.minLon) {
                    Polygon p = (Polygon)es.segmentData.Geometry;
                    foreach (PointCollection points in p.Rings)
                    {
                        int i;
                        int j = points.Count - 1;
                        for (i = 0; i < points.Count; i++)
                        {
                            if (points[i].X < point.X && points[j].X >= point.X
                              || points[j].X < point.X && points[i].X >= point.X)
                            {
                                if (points[i].Y + (point.X - points[i].X) / (points[j].X - points[i].X) * (points[j].Y - points[i].Y) < point.Y)
                                {
                                    if (!string.IsNullOrEmpty(es.SegmentDescription))
                                    {
                                        return es.SegmentDescription;
                                    }
                                    else
                                    {
                                        return es.SegmentID;
                                    }
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            return segName;
        }
    }

    public class esriSegment
    {
        public string BeatID { get; set; }
        public string SegmentID { get; set; }
        public string SegmentDescription { get; set; }
        public Graphic segmentData { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }
}