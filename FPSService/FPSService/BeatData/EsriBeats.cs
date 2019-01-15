using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;

namespace FPSService.BeatData
{
    public static class EsriBeats
    {
        //DEPRECATED - REPLACED WITH Beats.cs
        private static string url = ConfigurationManager.AppSettings["EsriBeats"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriBeat> beatPolygons = new List<esriBeat>();
        public static List<beatData> beatData = new List<beatData>();
        public static List<string> freeways = new List<string>();


        public static void LoadBeatData() //beat information for the front-end client
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            beatData.Clear();
            beatData = mySQL.getBeatData();
        }

        public static void LoadBeats() //Load all the beat data in the feature layer
        {
            try
            {
                qt = new QueryTask(url);
                query = new Query();
                query.ReturnGeometry = true;
                query.OutFields.Add("*");
                query.Where = "1=1";
                qt.Execute(query);
                beatPolygons.Clear();
                foreach (Graphic resultFeature in qt.LastResult.Features)
                {
                    string bID = string.Empty;
                    string bDesc = string.Empty;
                    System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                    foreach (string theKey in allAttributes.Keys)
                    {
                        if (theKey == "BEAT_ID_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            {
                                bID = theValue.ToString();
                            }
                            else
                            {
                                bID = "NO BEAT ID";
                            }
                        }
                        if (theKey == "BeatDescript")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            {
                                bDesc = theValue.ToString();
                            }
                            else
                            {
                                bDesc = "NO DESCRIPTION";
                            }
                        }
                    }

                    esriBeat eb = new esriBeat();
                    eb.BeatID = bID;
                    eb.BeatDescription = bDesc;
                    eb.beatData = resultFeature;
                    //check for min/max lat/lon and log it
                    //init vars so we can test each point in the polygon against something other than 0.0

                    ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)eb.beatData.Geometry;
                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings) {
                        eb.maxLat = points[0].Y;
                        eb.maxLon = points[0].X;
                        eb.minLat = points[0].Y;
                        eb.minLon = points[0].X;
                    }

                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings) {

                        for (int i = 0; i < points.Count(); i++) {
                            //start setting min/max points
                            if (points[i].X > eb.maxLon) {
                                eb.maxLon = points[i].X;
                            }
                            if (points[i].Y > eb.maxLat) {
                                eb.maxLat = points[i].Y;
                            }
                            if (points[i].X < eb.minLon) {
                                eb.minLon = points[i].X;
                            }
                            if (points[i].Y < eb.minLat) {
                                eb.minLat = points[i].Y;
                            }
                        }
                    }
                    beatPolygons.Add(eb);
                }
            }
            catch (Exception ex)
            {
                Logging.EventLogger log = new Logging.EventLogger();
                log.LogEvent(ex.ToString(), true);
            }
        }

        public static bool checkEsriIntersect(double lat, double lon, string beatID)
        {
            bool inPoly = false;
            bool isInside = false;
            ESRI.ArcGIS.Client.Geometry.MapPoint point = new ESRI.ArcGIS.Client.Geometry.MapPoint(lon, lat);
            var spList = from sp in beatPolygons
                         where sp.BeatID == beatID
                         select sp;
            foreach (esriBeat eb in spList)
            {
                ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)eb.beatData.Geometry;
                if (lat <= eb.maxLat && lat >= eb.minLat && lon <= eb.maxLon && lon >= eb.minLon) {
                    /* OLD CODE WORKED MOST OF THE TIME BUT RETURNED OFF BEAT SOMETIMES WHEN A TRUCK WAS ON BEAT */
                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
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


                    foreach (PointCollection points in p.Rings)
                    {
                        for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
                        {
                            if (((points[i].Y > point.Y) != (points[j].Y > point.Y)) &&
                                    (point.X < (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                            {
                                isInside = true;
                            }
                        }
                    }
                }
            }
            return isInside;
        }

        public static string findBeat(double lat, double lon)
        {
            string beatName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            foreach (esriBeat es in beatPolygons)
            {
                if (lat >= es.minLat && lat <= es.maxLat && lon >= es.minLon && lon <= es.maxLon) {
                    Polygon p = (Polygon)es.beatData.Geometry;
                    foreach (PointCollection points in p.Rings)
                    {
                        /*
                        int i;
                        int j = points.Count - 1;
                        for (i = 0; i < points.Count; i++)
                        {
                            if (points[i].X < point.X && points[j].X >= point.X
                              || points[j].X < point.X && points[i].X >= point.X)
                            {
                                if (points[i].Y + (point.X - points[i].X) / (points[j].X - points[i].X) * (points[j].Y - points[i].Y) < point.Y)
                                {
                                    return es.BeatID;
                                }
                            }
                            j = i;
                        }
                         * */
                        for (int i = 0, j = points.Count - 1; i < points.Count; j = i++)
                        {
                            if (((points[i].Y > point.Y) != (points[j].Y > point.Y)) &&
                                    (point.X < (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                            {
                                return es.BeatID;
                            }
                        }
                    }
                }
                
            }
            return beatName;
        }
    }

    public class esriBeat
    {
        public string BeatID { get; set; }
        public string BeatDescription { get; set; }
        public Graphic beatData { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }
}