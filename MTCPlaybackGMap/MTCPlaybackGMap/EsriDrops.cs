using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;
using System.Configuration;

namespace MTCPlaybackGMap
{
    public static class EsriDrops
    {
        private static string url = ConfigurationManager.AppSettings["EsriDrops"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriDrop> dropPolygons = new List<esriDrop>();

        public static void LoadDrops()
        {
            qt = new QueryTask(url);
            query = new Query();
            query.ReturnGeometry = true;
            query.OutFields.Add("*");
            query.Where = "1=1";
            qt.Execute(query);
            dropPolygons.Clear();
            foreach (Graphic resultFeature in qt.LastResult.Features)
            {
                string bID = string.Empty;
                string bDesc = string.Empty;
                System.Collections.Generic.IDictionary<string, object> allAttributes = resultFeature.Attributes;
                foreach (string theKey in allAttributes.Keys)
                {
                    if (theKey == "Beat")
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
                    if (theKey == "DPSDSC")
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

                esriDrop ed = new esriDrop();
                ed.BeatID = bID;
                ed.DropDescription = bDesc;
                ed.dropData = resultFeature;
                dropPolygons.Add(ed);
            }
        }

        public class esriDrop
        {
            public string BeatID { get; set; }
            public string DropDescription { get; set; }
            public Graphic dropData { get; set; }
        }
    }
}
