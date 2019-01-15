using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.SqlServer.Types;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;


namespace EsriTestApp
{
    public static class GeoClass
    {
        public static List<esriDropSite> segPolygons = new List<esriDropSite>();

        public static bool checkEsriIntersect(double lat, double lon, string Name)
        {
            bool inPoly = false;
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in segPolygons
                         where sp.Name == Name
                         select sp;
            foreach (esriDropSite es in spList)
            {
                Polygon p = (Polygon)es.dropSiteData.Geometry;

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
            return inPoly;
        }

        public static string findBeatSeg(double lat, double lon)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            foreach (esriDropSite es in GeoClass.segPolygons)
            {
                Polygon p = (Polygon)es.dropSiteData.Geometry;
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
            return segName;
        }
    }

    /*
    public class BeatSeg 
    {
        public string BeatSegID { get; set; }
        public SqlGeography geo { get; set; }
    }*/

    public class esriData
    {
        public string BeatSegID { get; set; }
        public Graphic segData { get; set; }
    }

    public class Extent2
    {
        public double xmin { get; set; }
        public double ymin { get; set; }
        public double xmax { get; set; }
        public double ymax { get; set; }
        public SpatialReference spatialReference { get; set; }
    }

    public class SpatialReference
    {
        public int wkid { get; set; }
        public int latestWkid { get; set; }
    }

    public class DocumentInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Comments { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Keywords { get; set; }
    }

    public class Layer2 
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class FeatureLayer
    {
        public string currentVersion { get; set; }
        public string serviceDescription { get; set; }
        public bool hasVersionedData { get; set; }
        public bool supportsDisconnectedEditing { get; set; }
        public bool syncEnabled { get; set; }
        public string supportedQueryFormats { get; set; }
        public int maxRecordCount { get; set; }
        public string capabilities { get; set; }
        public string description { get; set; }
        public string copyrightText { get; set; }
        public SpatialReference spatialReference { get; set; }
        public Extent2 initialExtent { get; set; }
        public Extent2 fullExtent { get; set; }
        public bool allowGeometryUpdates { get; set; }
        public string units { get; set; }
        public DocumentInfo documentInfo { get; set; }
        public List<Layer2> layers{get;set;}
        public List<string> tables { get; set; }
        public bool enableZDefaults { get; set; }
    }
}
