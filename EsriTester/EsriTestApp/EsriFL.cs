using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;
using System.Windows.Forms;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;

namespace EsriTestApp
{
    public class EsriFL
    {
        public void GetBeatSegs()
        {
            QueryTask qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/SegementsTest/FeatureServer/0");
            Query query = new Query();
            query.ReturnGeometry = true;
            //query.Geometry = "POLYGON";
            query.Where = "1=1";
            qt.Execute(query);
            foreach (Graphic resultFeature in qt.LastResult.Features)
            {
                
                string bsID = string.Empty;
                System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                foreach (string theKey in allAttributes.Keys)
                {
                    object theValue = allAttributes[theKey];
                    bsID = theValue.ToString();
                }

                esriData ed = new esriData();
                ed.BeatSegID = bsID;
                ed.segData = resultFeature;
                GeoClass.segPolygons.Add(ed);
                /*
                LatLonList lList = new LatLonList();
                lList.pairs = new List<List<LatLonPair>>();
                string s = resultFeature.Geometry.Extent.ToString();
                Polygon p = (Polygon)resultFeature.Geometry;
                PointCollection pcc = new PointCollection();
                MapPoint mp = new MapPoint(32.0, 54.35);
                pcc.Add(mp);
                if (p.Rings.Contains(pcc))
                {
                    string hi = "hi";
                }
                List<PointCollection> pcol = p.Rings.ToList<PointCollection>();
                foreach (PointCollection pc in pcol)
                {
                    List<LatLonPair> llpList = new List<LatLonPair>();
                    for (int i = 0; i < pc.Count(); i++)
                    {
                        LatLonPair llp = new LatLonPair();
                        llp.lat = pc[i].X;
                        llp.lon = pc[i].Y;
                        llpList.Add(llp);
                    }
                    lList.pairs.Add(llpList);
                }
                if (lList.pairs.Count == 1)
                {
                    if (lList.pairs[0].Count < 4) //make line
                    {
                        makeAndAddLine(bsID, lList);
                    }
                    else //make polygon
                    {
                        makeAndAddGeo(bsID, lList);
                    }
                }
                if (lList.pairs.Count > 1)
                {
                    makeAndAddMultiPolygon(bsID, lList);
                }
                 * */
            }
        }

        /*
        private void makeAndAddGeo(string bsID, LatLonList pairList)
        {
            BeatSeg bs = new BeatSeg();
            bs.BeatSegID = bsID;
            string polyString = "POLYGON((";
            foreach (LatLonPair llp in pairList.pairs[0])
            {
                polyString += llp.lat.ToString() + " " + llp.lon.ToString() + ",";
            }
            polyString = polyString.Substring(0, polyString.Length - 1);
            polyString += "))";
            SqlChars polyText = new SqlChars(polyString);
            SqlGeography polygon = SqlGeography.STPolyFromText(polyText, 4326);
            polygon.MakeValid();
            var inverted = polygon.ReorientObject();
            if (polygon.STArea() > inverted.STArea())
            {
                polygon = inverted;
            }
            bs.geo = polygon;
            GeoClass.addSegment(bs);
        }
        */
        /*
        private void makeAndAddLine(string bsID, LatLonList pairList)
        {
            BeatSeg bs = new BeatSeg();
            bs.BeatSegID = bsID;
            string polyString = "LINESTRING(";
            foreach (LatLonPair llp in pairList.pairs[0])
            {
                polyString += llp.lat.ToString() + " " + llp.lon.ToString() + ",";
            }
            polyString = polyString.Substring(0, polyString.Length - 1);
            polyString += ")";
            SqlChars polyText = new SqlChars(polyString);
            SqlGeography polygon = SqlGeography.STLineFromText(polyText, 4326);
            polygon.MakeValid();
            var inverted = polygon.ReorientObject();
            if (polygon.STArea() > inverted.STArea())
            {
                polygon = inverted;
            }
            bs.geo = polygon;
            GeoClass.addSegment(bs);
        }
        */
        /*
        private void makeAndAddMultiPolygon(string bsID, LatLonList pairList)
        {
            BeatSeg bs = new BeatSeg();
            bs.BeatSegID = bsID;
            SqlGeographyBuilder sqlbuilder = new SqlGeographyBuilder();
            sqlbuilder.SetSrid(4326);
            List<SqlGeography> areaPolygons = new List<SqlGeography>();
            sqlbuilder.BeginGeography(OpenGisGeographyType.MultiPolygon);
            for (int i = 0; i < pairList.pairs.Count(); i++)
            {
                if (pairList.pairs[i].Count() < 4)
                {
                    string polyString = "LINESTRING(";
                    foreach (LatLonPair llp in pairList.pairs[0])
                    {
                        polyString += llp.lat.ToString() + " " + llp.lon.ToString() + ",";
                    }
                    polyString = polyString.Substring(0, polyString.Length - 1);
                    polyString += ")";
                    SqlChars polyText = new SqlChars(polyString);
                    SqlGeography polygon = SqlGeography.STLineFromText(polyText, 4326);
                    polygon.MakeValid();
                    var inverted = polygon.ReorientObject();
                    if (polygon.STArea() > inverted.STArea())
                    {
                        polygon = inverted;
                    }
                    //areaPolygons.Add(polygon);
                }
                else
                {
                    //make polygon
                    string polyString = "POLYGON((";
                    foreach (LatLonPair llp in pairList.pairs[0])
                    {
                        polyString += llp.lat.ToString() + " " + llp.lon.ToString() + ",";
                    }
                    polyString = polyString.Substring(0, polyString.Length - 1);
                    polyString += "))";
                    SqlChars polyText = new SqlChars(polyString);
                    SqlGeography polygon = SqlGeography.STPolyFromText(polyText, 4326);
                    try
                    {
                        polygon.MakeValid();
                        var inverted = polygon.ReorientObject();
                        if (polygon.STArea() > inverted.STArea())
                        {
                            polygon = inverted;
                        }
                    }
                    catch
                    {
                        string err = "err";
                    }
                    areaPolygons.Add(polygon);
                }
            }
            try
            {
                foreach (SqlGeography geog in areaPolygons)
                {
                    sqlbuilder.BeginGeography(OpenGisGeographyType.Polygon);
                    for (int i = 1; i <= geog.STNumPoints(); i++)
                    {
                        if (i == 1)
                        {
                            sqlbuilder.BeginFigure((double)geog.STPointN(i).Lat, (double)geog.STPointN(i).Long);
                        }
                        else
                        {
                            sqlbuilder.AddLine((double)geog.STPointN(i).Lat, (double)geog.STPointN(i).Long);
                        }
                        sqlbuilder.EndFigure();
                        sqlbuilder.EndGeography();
                    }
                }
                sqlbuilder.EndGeography();
                bs.geo = sqlbuilder.ConstructedGeography;
            }
            catch (Exception ex)
            {
                string boom = "Boom";
            }
            
        
                /*
                string polyString = "MULTIPOLYGON(";
                for (int i = 0; i < pairList.pairs.Count(); i++)
                {
                    if (pairList.pairs[i].Count() < 4)
                    {
                        string bad = "bad count";
                    }
                    else
                    {
                        polyString += "((";
                        foreach (LatLonPair llp in pairList.pairs[i])
                        {
                            polyString += llp.lat.ToString() + " " + llp.lon.ToString() + ",";
                        }
                        polyString = polyString.Substring(0, polyString.Length - 1);
                        polyString += ")),";
                    }
                }
                polyString = polyString.Substring(0, polyString.Length - 1);
                polyString += ")";
                SqlChars polyText = new SqlChars(polyString);
                SqlGeography polygon = SqlGeography.STMPolyFromText(polyText, 4326);
                polygon.MakeValid();
                /*
                var inverted = polygon.ReorientObject();
                if (polygon.STArea() > inverted.STArea())
                {
                    polygon = inverted;
                }
                
                //bs.geo = polygon;
            GeoClass.addSegment(bs);
        }
    */
    }

    public class LatLonPair
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class LatLonList
    {
        public List<List<LatLonPair>> pairs { get; set; }
    }

    /*
     * string tLeft = fs.initialExtent.xmin.ToString() + " " + fs.initialExtent.ymax.ToString();
            string bLeft = fs.initialExtent.xmin.ToString() + " " + fs.initialExtent.ymin.ToString();
            string bRight = fs.initialExtent.xmax.ToString() + " " + fs.initialExtent.ymin.ToString();
            string tRight = fs.initialExtent.xmax.ToString() + " " + fs.initialExtent.ymax.ToString();
            string wktString = "POLYGON ((" + tLeft + ", " + bLeft + ", " + bRight + ", " + tRight + ", " + tLeft + "))";
     */
}
