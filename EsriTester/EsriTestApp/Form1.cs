using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client.Geometry;

namespace EsriTestApp
{
    public partial class Form1 : Form
    {
        FeatureLayer fs;
        double lat = 0.0;
        double lon = 0.0;
        QueryTask qt;
        public Form1()
        {
            InitializeComponent();
            InitMap();
            //qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/SegementsTest/FeatureServer/0");
            qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/Beat_Segment/FeatureServer/0");
            txtBeatSeg.KeyPress += txtBeatSeg_KeyPress;
        }

        void txtBeatSeg_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (e.KeyChar == 13)
            {
                if (GeoClass.segPolygons.Count < 1)
                {
                    MessageBox.Show("No data, try loading first");
                    return;
                }
                DrawBeatSegment(txtBeatSeg.Text);
            }
            
        }

        private void DrawData(string bsID, double lat = 0.0, double lon = 0.0)
        {
            gMapControl1.Overlays.Clear();
            DrawBeatSegment(bsID);
            if (lat != 0.0 && lon != 0.0)
            {
                PushPoint(lat, lon, "test point");
            }
        }

        private void DrawBeatSegment(string bsID)
        {
            /*
            BeatSeg bs = GeoClass.beatsegs.Find(delegate(BeatSeg find) { return find.BeatSegID == bsID; });
            if (bs != null)
            {
                
                GMapOverlay polyOverlay = new GMapOverlay(gMapControl1, "polygons");
                List<PointLatLng> points = new List<PointLatLng>();
                for (int i = 1; i < bs.geo.STNumPoints(); i++)
                {
                    SqlGeography point = bs.geo.STPointN(i);
                    points.Add(new PointLatLng((double)point.Lat, (double)point.Long));
                }
                GMapPolygon polygon = new GMapPolygon(points, bs.BeatSegID);
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                polygon.Stroke = new Pen(Color.Red, 1);
                polyOverlay.Polygons.Add(polygon);
                gMapControl1.Overlays.Add(polyOverlay);
            }
            else
            {
                MessageBox.Show("Couldn't find beat segment " + txtBeatSeg.Text);
            }*/
            esriData ed = GeoClass.segPolygons.Find(delegate(esriData find) { return find.BeatSegID == bsID; });
            if (ed != null)
            {
                GMapOverlay polyOverlay = new GMapOverlay(gMapControl1, "polygons");
                List<PointLatLng> pointList = new List<PointLatLng>();
                Polygon p = (Polygon)ed.segData.Geometry;
                foreach (PointCollection points in p.Rings)
                {
                    for (int i = 1; i < points.Count; i++)
                    {
                        pointList.Add(new PointLatLng((double)points[i].Y, (double)points[i].X));
                    }
                    GMapPolygon polygon = new GMapPolygon(pointList, ed.BeatSegID);
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                    polygon.Stroke = new Pen(Color.Red, 1);
                    polyOverlay.Polygons.Add(polygon);
                }
                gMapControl1.Overlays.Add(polyOverlay);
            }
            else
            {
                MessageBox.Show("Couldn't find beat segment " + bsID);
            }
        }

        private void PushPoint(double lat, double lon, string id)
        {
            GMapOverlay pointOverlay = new GMapOverlay(gMapControl1, "point");
            GMapMarkerGoogleGreen marker = new GMapMarkerGoogleGreen(new PointLatLng(lat, lon));
            pointOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(pointOverlay);

        }

        private void InitMap()
        {
            lat = Convert.ToDouble(txtLat.Text);
            lon = Convert.ToDouble(txtLon.Text);
            gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl1.Position = new PointLatLng(lat, lon);
            GMapOverlay overlay = new GMapOverlay(gMapControl1, "base");
            GMapMarkerGoogleGreen home = new GMapMarkerGoogleGreen(new PointLatLng(lat, lon));
            overlay.Markers.Add(home);
            gMapControl1.Overlays.Add(overlay);
        }
        /*
        private void DrawPolygon()
        {
            /*
            GMapOverlay polyOverlay = new GMapOverlay(gMapControl1, "polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(fs.initialExtent.ymax, fs.initialExtent.xmin));
            points.Add(new PointLatLng(fs.initialExtent.ymin, fs.initialExtent.xmin));
            points.Add(new PointLatLng(fs.initialExtent.ymin, fs.initialExtent.xmax));
            points.Add(new PointLatLng(fs.initialExtent.ymax, fs.initialExtent.xmax));
            GMapPolygon polygon = new GMapPolygon(points, "myPolygon");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 1);
            polyOverlay.Polygons.Add(polygon);
            gMapControl1.Overlays.Add(polyOverlay);
             
            
            List<string> segIDs = new List<string>();
            foreach (BeatSeg bs in GeoClass.beatsegs)
            {
                segIDs.Add(bs.BeatSegID);
            }
            segIDs = segIDs.OrderBy(s => s).ToList<string>();
            cboBeatSegs.DataSource = segIDs;

            int count = GeoClass.beatsegs.Count();
            MessageBox.Show("Loaded " + count + " segments", "Data Loaded");
        }* */

        public void QueryObjects()
        {
            EsriFL efl = new EsriFL();
            efl.GetBeatSegs();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            QueryObjects();
            InitMap();
            cboBeatSegs.Items.Clear();
            List<string> beatSegs = new List<string>();
            foreach (esriData ed in GeoClass.segPolygons)
            {
                beatSegs.Add(ed.BeatSegID);
            }
            beatSegs = beatSegs.OrderBy(s => s).ToList<string>();
            cboBeatSegs.DataSource = beatSegs;
            //DrawPolygon();
        }

        private void btnCheckIntersection_Click(object sender, EventArgs e)
        {
            lat = Convert.ToDouble(txtLat.Text);
            lon = Convert.ToDouble(txtLon.Text);
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(4326);
            builder.BeginGeography(OpenGisGeographyType.Point);
            builder.BeginFigure(lat, lon);
            builder.EndFigure();
            builder.EndGeography();

            DrawData(cboBeatSegs.Text, lat, lon);

            //bool tf = GeoClass.checkSegIntersection(cboBeatSegs.Text, builder.ConstructedGeography);
            bool tf = GeoClass.checkEsriIntersect(lat, lon, cboBeatSegs.Text);
            
            //bool tf = GeoClass.checkIntersects(builder.ConstructedGeography);
            if (tf == true)
            {
                lblInside.Text = "Point is inside figure";
            }
            else
            {
                lblInside.Text = "Point is outside figure";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            double lat = Convert.ToDouble(txtLat.Text);
            double lon = Convert.ToDouble(txtLon.Text);
            string val = GeoClass.findBeatSeg(lat, lon);
            lblSegID.Text = val;
        }

        #region " archived "
        /*
        private void button1_Click_1(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.Client.FeatureLayer fl = getFL();
            QueryObjects();
            /*var request = WebRequest.Create("http://174.47.234.62:6080/arcgis/rest/services/Polygontest/FeatureServer/?f=pjson");
            var request = WebRequest.Create("http://38.124.164.214:6080/arcgis/rest/services/SegementsTest/FeatureServer/0?f=pjson");
            using (var resp = (HttpWebResponse)request.GetResponse())
            {
                using (var rdr = new StreamReader(resp.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string objText = rdr.ReadToEnd();
                    string done = objText;
                    fs = (FeatureLayer)js.Deserialize(objText, typeof(FeatureLayer));
                }
            }

            if (fs != null)
            {
                txtXmin.Text = fs.initialExtent.xmin.ToString();
                txtYmin.Text = fs.initialExtent.ymin.ToString();
                txtXmax.Text = fs.initialExtent.xmax.ToString();
                txtYmax.Text = fs.initialExtent.ymax.ToString();
                txtWkid.Text = fs.initialExtent.spatialReference.wkid.ToString();
                txtLwkid.Text = fs.initialExtent.spatialReference.latestWkid.ToString();
            }

            string tLeft = fs.initialExtent.xmin.ToString() + " " + fs.initialExtent.ymax.ToString();
            string bLeft = fs.initialExtent.xmin.ToString() + " " + fs.initialExtent.ymin.ToString();
            string bRight = fs.initialExtent.xmax.ToString() + " " + fs.initialExtent.ymin.ToString();
            string tRight = fs.initialExtent.xmax.ToString() + " " + fs.initialExtent.ymax.ToString();
            string wktString = "POLYGON ((" + tLeft + ", " + bLeft + ", " + bRight + ", " + tRight + ", " + tLeft + "))";
            //string wktString = "POLYGON ((-74.00751113891601 40.73412061435751, -74.02193069458008 40.705758069466754, -73.96940231323242 40.72891738296314, -74.00442123413086 40.70107318388675, -74.00751113891601 40.73412061435751))";
            SqlChars polyText = new SqlChars(wktString);
            SqlGeography polygon = SqlGeography.STPolyFromText(polyText, 4326);
            GeoClass.myGeo = polygon;
            InitMap();
            DrawPolygon();
        }*/
        #endregion
    }
}
