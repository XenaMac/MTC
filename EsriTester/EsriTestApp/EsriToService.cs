using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriTestApp
{
    public class Layer
    {
        public int id, parentLayerId;
        public string name;
        public bool defaultVisibility;
        public List<int> subLayerIds;
    }

    public struct Extent
    {
        public double XMin, YMin, XMax, YMax;

        public Extent(bool InitializeMaxValues)
        {
            if (InitializeMaxValues)
            {
                this.XMin = 999999999999999;
                this.XMax = -999999999999999;
                this.YMin = 999999999999999;
                this.YMax = -999999999999999;
            }
            else
            {
                this.XMin = 0;
                this.XMax = 0;
                this.YMin = 0;
                this.YMax = 0;
            }
        }

        public Extent(double XMin, double YMin, double XMax, double YMax)
        {
            this.XMin = XMin;
            this.XMax = XMax;
            this.YMin = YMin;
            this.YMax = YMax;
        }
    }

    public class DetailedLayer
    {
        public int? id { get; set; }
        public Layer parentLayer { get; set; }
        public string name, type, description, definitionExpression, geometryType, copyrightText, htmlPopupType, displayField, capabilities;
        public List<Layer> subLayers;
        public double minScale, maxScale;
        public bool defaultVisibility, hasAttachments;
        public Extent extent;
        public object drawingInfo, fields, typeIdField, types, relationships;

        public static DetailedLayer GetDetailedLayerInfo(string URL)
        {
            try
            {
                string url = URL.TrimEnd("/".ToCharArray()) + "?f=pjson";

                System.Net.WebResponse response = null;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "GET";
                response = request.GetResponse();
                if (response != null)
                {
                    System.IO.StreamReader myReader = new System.IO.StreamReader(response.GetResponseStream());
                    string ResponseString = myReader.ReadToEnd();
                    DetailedLayer newDetailedLayerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(ResponseString, typeof(DetailedLayer)) as DetailedLayer;
                    return newDetailedLayerInfo;
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message); 
            }
            return new DetailedLayer();
        }
    }

}
