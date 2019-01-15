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
    public class EsriDropSites
    {
        //DEPRECATED - REPLACED WITH DropSites.cs

        private static string url = ConfigurationManager.AppSettings["EsriDropSites"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriDropSite> dropPolygons = new List<esriDropSite>();

        public static void LoadDropSites() //Load the segment data into memory for faster querying
        {
            try
            {
                qt = new QueryTask(url);
                Query query = new Query();
                query.ReturnGeometry = true;
                query.OutFields.Add("*");
                query.Where = "1=1";
                qt.Execute(query);
                dropPolygons.Clear();
                foreach (Graphic resultFeature in qt.LastResult.Features)
                {

                    string BeatID = string.Empty;
                    string Name = string.Empty;
                    string Desc = string.Empty;
                    System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                    foreach (string theKey in allAttributes.Keys)
                    {
                        if (theKey == "BeatID")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { BeatID = theValue.ToString(); }

                        }
                        if (theKey == "Beat")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            {
                                BeatID = theValue.ToString();
                            }
                        }
                        if (theKey == "Name")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Name = theValue.ToString(); }
                        }
                        if (theKey == "DPSDSC")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Desc = theValue.ToString(); }
                        }
                    }

                    esriDropSite ed = new esriDropSite();
                    ed.BeatID = BeatID;
                    ed.Desc = Desc;
                    ed.Name = Name;
                    ed.dropSiteData = resultFeature;

                    ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)ed.dropSiteData.Geometry;
                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {
                        ed.maxLat = points[0].Y;
                        ed.maxLon = points[0].X;
                        ed.minLat = points[0].Y;
                        ed.minLon = points[0].X;
                    }


                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {

                        for (int i = 0; i < points.Count(); i++)
                        {
                            //start setting min/max points
                            if (points[i].X > ed.maxLon)
                            {
                                ed.maxLon = points[i].X;
                            }
                            if (points[i].Y > ed.maxLat)
                            {
                                ed.maxLat = points[i].Y;
                            }
                            if (points[i].X < ed.minLon)
                            {
                                ed.minLon = points[i].X;
                            }
                            if (points[i].Y < ed.minLat)
                            {
                                ed.minLat = points[i].Y;
                            }
                        }
                    }

                    dropPolygons.Add(ed);
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public static bool checkEsriIntersect(double lat, double lon, string Name)
        {
            bool inPoly = false;
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in dropPolygons
                         where sp.Name == Name
                         select sp;
            foreach (esriDropSite es in spList)
            {
                Polygon p = (Polygon)es.dropSiteData.Geometry;
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

        public static List<esriDropSite> sitesByBeat(string beatID)
        {
            List<esriDropSite> sites = new List<esriDropSite>();
            var spList = from sp in dropPolygons
                         where sp.BeatID == beatID
                         select sp;
            foreach (esriDropSite es in spList)
            {
                sites.Add(es);
            }
            return sites;
        }

        public static string findDropSiteByBeat(double lat, double lon, string beatID)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in dropPolygons
                         where sp.BeatID == beatID
                         select sp;
            foreach (esriDropSite es in spList)
            {
                Polygon p = (Polygon)es.dropSiteData.Geometry;
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
                                    return es.Name;
                                }
                            }
                            j = i;
                        }
                    }
                }

            }
            return segName;
        }

        public static string findDropSite(double lat, double lon)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            foreach (esriDropSite es in dropPolygons)
            {
                Polygon p = (Polygon)es.dropSiteData.Geometry;
                foreach (PointCollection points in p.Rings)
                {
                    if (lat <= p.Extent.YMax && lat >= p.Extent.YMin && lon <= p.Extent.XMax && lon >= p.Extent.XMin)
                    {
                        return es.Name;
                    }
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
                                return es.Name;
                            }
                        }
                        j = i;
                    }
                     * */
                }
            }
            return segName;
        }
    }

    public class esriDropSite
    {
        public string BeatID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public Graphic dropSiteData { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }
}