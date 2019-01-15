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
    public static class EsriBeats
    {
        private static string url = ConfigurationManager.AppSettings["EsriBeats"].ToString();
        private static Query query;
        private static QueryTask qt;
        public static List<esriBeat> beatPolygons = new List<esriBeat>();
        public static List<string> freeways = new List<string>();

        public static void LoadBeats() //Load all the beat data in the feature layer
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
                beatPolygons.Add(eb);
            }
        }
    }

    public class esriBeat
    {
        public string BeatID { get; set; }
        public string BeatDescription { get; set; }
        public Graphic beatData { get; set; }
    }
}
