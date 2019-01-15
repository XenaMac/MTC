using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;

namespace EsriTestApp
{
    public static class EsriBeat
    {
        private static string url = "http://38.124.164.214:6080/arcgis/rest/services/Beats/FeatureServer/0";
        private static Query query;
        private static QueryTask qt;
        public static List<esriBeat> beatPolygons = new List<esriBeat>();

        public static void LoadBeats() //Load all the beat data in the feature layer
        {
            qt = new QueryTask(url);
            query = new Query();
            query.ReturnGeometry = true;
            query.OutFields.AddRange(new string[] { "BeatDescription", "BeatID" });
            query.Where = "1=1";
            qt.Execute(query);
            foreach (Graphic resultFeature in qt.LastResult.Features)
            {
                string bID = string.Empty;
                string bDesc = string.Empty;
                System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                foreach (string theKey in allAttributes.Keys)
                {
                    if (theKey == "BeatID")
                    {
                        object theValue = allAttributes[theKey];
                        bID = theValue.ToString();
                    }
                    if (theKey == "BeatDescription")
                    {
                        object theValue = allAttributes[theKey];
                        bDesc = theValue.ToString();
                    }
                }

                esriBeat eb = new esriBeat();
                eb.BeatID = bID;
                eb.BeatDescription = bDesc;
                eb.beatData = resultFeature;
                beatPolygons.Add(eb);
            }
        }

        public static bool checkEsriIntersect(double lat, double lon, string beatID)
        {
            bool inPoly = false;
            MapPoint point = new MapPoint(lon, lat);
            var spList = from sp in beatPolygons
                         where sp.BeatID == beatID
                         select sp;
            foreach (esriBeat eb in spList)
            {
                Polygon p = (Polygon)eb.beatData.Geometry;

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
    }

    public class esriBeat
    {
        public string BeatID { get; set; }
        public string BeatDescription { get; set; }
        public Graphic beatData { get; set; }
    }
}
