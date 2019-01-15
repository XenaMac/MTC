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
            //QueryTask qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/MTCSegementsFinal/FeatureServer/0");
            //QueryTask qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/Beats/FeatureServer/0");
            QueryTask qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer/0");
            //QueryTask qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/TowTruckSites/FeatureServer/0");
            Query query = new Query();
            query.ReturnGeometry = true;
            //query.OutFields.AddRange(new string[] { "SegmentID", "BeatID", "SegmentDescription" }); //for beats
            query.OutFields.Add("*");
            query.Where = "1=1";
            qt.Execute(query);
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
                        if(theValue != null)
                        { BeatID = theValue.ToString(); }
                        
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
                GeoClass.segPolygons.Add(ed);
            }
        }
    }

    public class esriDropSite
    {
        public string BeatID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public Graphic dropSiteData { get; set; }
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
}
