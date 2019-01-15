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
    public class EsriYards
    {
        //DEPRECATED - REPLACED WITH Yards.cs

        private static string url = ConfigurationManager.AppSettings["EsriYards"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriYard> yardPolygons = new List<esriYard>();

        public static void LoadYards() //Load the segment data into memory for faster querying
        {
            try
            {
                qt = new QueryTask(url);
                Query query = new Query();
                query.ReturnGeometry = true;
                query.OutFields.Add("*");
                query.Where = "1=1";
                qt.Execute(query);
                yardPolygons.Clear();
                foreach (Graphic resultFeature in qt.LastResult.Features)
                {
                    string BeatID = string.Empty;
                    string Contractor = string.Empty;
                    string Desc = string.Empty;
                    string Address = string.Empty;
                    string City = string.Empty;
                    string Zip = string.Empty;
                    string Phone = string.Empty;
                    string ObjectID = string.Empty;
                    System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                    foreach (string theKey in allAttributes.Keys)
                    {
                        if (theKey == "ID_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { BeatID = theValue.ToString(); }
                        }
                        if (theKey == "Contract_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Contractor = theValue.ToString(); }
                        }
                        if (theKey == "Address_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Address = theValue.ToString(); }
                        }
                        if (theKey == "City_1_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { City = theValue.ToString(); }
                        }
                        if (theKey == "Zip_1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Zip = theValue.ToString(); }
                        }
                        if (theKey == "Phone1")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { Phone = theValue.ToString(); }
                        }
                        if (theKey == "OBJECTID")
                        {
                            object theValue = allAttributes[theKey];
                            if (theValue != null)
                            { ObjectID = theValue.ToString(); }
                        }
                    }

                    esriYard ey = new esriYard();
                    ey.YardID = ObjectID;
                    ey.BeatID = BeatID;
                    ey.Contractor = Contractor;
                    ey.yardData = resultFeature;
                    ey.Address = Address;
                    ey.City = City;
                    ey.Zip = Zip;
                    ey.Phone = Phone;
                    ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)ey.yardData.Geometry;
                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {
                        ey.maxLat = points[0].Y;
                        ey.maxLon = points[0].X;
                        ey.minLat = points[0].Y;
                        ey.minLon = points[0].X;
                    }

                    foreach (ESRI.ArcGIS.Client.Geometry.PointCollection points in p.Rings)
                    {

                        for (int i = 0; i < points.Count(); i++)
                        {
                            //start setting min/max points
                            if (points[i].X > ey.maxLon)
                            {
                                ey.maxLon = points[i].X;
                            }
                            if (points[i].Y > ey.maxLat)
                            {
                                ey.maxLat = points[i].Y;
                            }
                            if (points[i].X < ey.minLon)
                            {
                                ey.minLon = points[i].X;
                            }
                            if (points[i].Y < ey.minLat)
                            {
                                ey.minLat = points[i].Y;
                            }
                        }
                    }
                    yardPolygons.Add(ey);
                }
            }
            catch (Exception ex)
            {
                Logging.EventLogger log = new Logging.EventLogger();
                log.LogEvent(ex.ToString(), true);
            }
        }

        public static bool checkEsriIntersect(double lat, double lon, string contractor)
        {
            bool inPoly = false;
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in yardPolygons
                         where sp.Contractor == contractor
                         select sp;
            foreach (esriYard ey in spList)
            {
                if (lat <= ey.maxLat && lat >= ey.minLat && lon <= ey.maxLon && lon >= ey.minLon) {
                    Polygon p = (Polygon)ey.yardData.Geometry;

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

        public static string findYardByContractor(double lat, double lon, string contractor)
        {
            string yardName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in yardPolygons
                         where sp.Contractor == contractor
                         select sp;
            foreach (esriYard ey in spList)
            {
                if (lat <= ey.maxLat && lat >= ey.minLat && lon <= ey.maxLon && lon >= ey.minLon) {
                    Polygon p = (Polygon)ey.yardData.Geometry;
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
                                    return ey.Contractor;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            return yardName;
        }

        public static string findYard(double lat, double lon)
        {
            string segName = "NOT FOUND";
            MapPoint point = new MapPoint(lon, lat);
            foreach (esriYard ey in yardPolygons)
            {
                Polygon p = (Polygon)ey.yardData.Geometry;
                foreach (PointCollection points in p.Rings)
                {
                    if (lat <= p.Extent.YMax && lat >= p.Extent.YMin && lon <= p.Extent.XMax && lon >= p.Extent.XMin)
                    {
                        return ey.Contractor;
                    }
                }
            }
            return segName;
        }
    }

    public class esriYard
    {
        public string YardID { get; set; }
        public string BeatID { get; set; }
        public string Contractor { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public Graphic yardData { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }
}