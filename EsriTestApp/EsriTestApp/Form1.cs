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
        //QueryTask qt;
        public Form1()
        {
            InitializeComponent();
            InitMap();
            //qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/Beats/FeatureServer/0");
            //qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/Beat_Segment/FeatureServer/0");
            //qt = new QueryTask("http://38.124.164.214:6080/arcgis/rest/services/DropZones/FeatureServer/0");
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

            esriDropSite ed = GeoClass.segPolygons.Find(delegate(esriDropSite find) { return find.Name == bsID; });
            if (ed != null)
            {
                GMapOverlay polyOverlay = new GMapOverlay(gMapControl1, "polygons");
                List<PointLatLng> pointList = new List<PointLatLng>();
                Polygon p = (Polygon)ed.dropSiteData.Geometry;
                foreach (PointCollection points in p.Rings)
                {
                    for (int i = 1; i < points.Count; i++)
                    {
                        pointList.Add(new PointLatLng((double)points[i].Y, (double)points[i].X));
                    }
                    GMapPolygon polygon = new GMapPolygon(pointList, ed.Name);
                    polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                    polygon.Stroke = new Pen(Color.Red, 1);
                    polyOverlay.Polygons.Add(polygon);
                    polyOverlay.Id = bsID;
                }
                gMapControl1.Overlays.Add(polyOverlay);
                gMapControl1.ZoomAndCenterMarkers(bsID);
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
        

        public void QueryObjects()
        {

            EsriFL efl = new EsriFL();
            efl.GetBeatSegs();

            //EsriBeat.LoadBeats();

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            QueryObjects();
            InitMap();
            cboBeatSegs.Items.Clear();
            List<string> beatSegs = new List<string>();
            foreach (esriDropSite ed in GeoClass.segPolygons)
            {
                beatSegs.Add(ed.Name);
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
    }
}
