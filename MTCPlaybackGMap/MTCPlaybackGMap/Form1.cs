using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
//using DevExpress.Xpf;
using DevExpress.Charts;
using System.IO;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;

namespace MTCPlaybackGMap
{
    public partial class Form1 : Form
    {
        DateTime startDateTime;
        DateTime endDateTime;
        string currentDirectory;
        int iCounter = 0;
        bool mustStop = false;
        SplashScreen ss;

        public Form1()
        {
            InitializeComponent();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
            ss = new SplashScreen();
            ss.ShowDialog();
            tStartTime.ShowUpDown = true;
            tStartTime.CustomFormat = "HH:mm";
            tStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            tEndTime.ShowUpDown = true;
            tEndTime.CustomFormat = "HH:mm";
            tEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            initMap();
            currentDirectory = Directory.GetCurrentDirectory() + "\\images\\";
            tmrPlayback.Tick += tmrPlayback_Tick;
            tmrPlayback.Stop();
            
            chartControl1.CustomDrawSeriesPoint += chartControl1_CustomDrawSeriesPoint;
            chartControl1.CustomDrawSeries += chartControl1_CustomDrawSeries;
            gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;
            gMapControl1.DragButton = MouseButtons.Left;
            btnPreLoad.BackColor = System.Drawing.Color.LightGreen;
            btnLoadData.BackColor = System.Drawing.Color.Red;
            btnPlayback.BackColor = System.Drawing.Color.Red;
            btnStopPlayback.BackColor = System.Drawing.Color.Red;
            btnExportData.BackColor = System.Drawing.Color.Red;
            btnLoadData.Enabled = false;
            btnPlayback.Enabled = false;
            btnStopPlayback.Enabled = false;
            btnExportData.Enabled = false;
            //ss.Close();
        }

        void chartControl1_CustomDrawSeries(object sender, DevExpress.XtraCharts.CustomDrawSeriesEventArgs e)
        {
            e.SeriesDrawOptions.Color = Color.Black;
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ss.Close();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            loadBeats();
            loadDrops();
        }

        private void loadBeats()
        {
            EsriBeats.LoadBeats();
        }

        private void loadDrops()
        {
            EsriDrops.LoadDrops();
        }

        #region " Custom Events : Chart Control and Grid Control "

        void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            playBackRow selRow = (playBackRow)gridView1.GetRow(e.FocusedRowHandle);
            gMapControl1.Position = new PointLatLng(selRow.Lat, selRow.Lon);
            gridView1.Appearance.FocusedRow.BackColor = Color.Red;
        }

        void chartControl1_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e)
        {
            try {
                DevExpress.XtraCharts.BarDrawOptions drawOptions = e.SeriesDrawOptions as DevExpress.XtraCharts.BarDrawOptions;
                if (drawOptions == null)
                {
                    return;
                }
                string type = string.Empty;
                try
                {
                    type = (string)e.SeriesPoint.Tag;
                }
                catch (Exception ex) {
                    string err = ex.ToString();
                }
                if (!string.IsNullOrEmpty(type))
                {
                    //analyze the type;
                    switch (type.ToUpper())
                    {
                        case "ONINCIDENT":
                            drawOptions.Color = Color.FromArgb(50, 0, 87, 255);
                            break;
                        case "ONPATROL":
                            drawOptions.Color = Color.FromArgb(50, 0, 151, 38);
                            break;
                        case "ONBREAK":
                            drawOptions.Color = Color.FromArgb(50, 8, 253, 244);
                            break;
                        case "ONLUNCH":
                            drawOptions.Color = Color.FromArgb(50, 8, 253, 244);
                            break;
                        case "ROLLOUT":
                            drawOptions.Color = Color.FromArgb(50, 8, 253, 244);
                            break;
                        case "ROLLIN":
                            drawOptions.Color = Color.FromArgb(50, 8, 253, 244);
                            break;
                        case "ENROUTE":
                            drawOptions.Color = Color.FromArgb(50, 253, 158, 8);
                            break;
                        case "LOGGEDON":
                            drawOptions.Color = Color.FromArgb(50, 100, 8, 253);
                            break;
                        case "ONTOW":
                            drawOptions.Color = Color.FromArgb(50, 253, 250, 8);
                            break;
                        default:
                            drawOptions.Color = Color.FromArgb(36, 172, 179, 172);
                            break;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region  " helpers "

        private void drawBeats(string beatNumber)
        {
            GMapOverlay polyOverlay = new GMapOverlay("beats");
            List<string> beats = new List<string>();
            foreach (esriBeat b in EsriBeats.beatPolygons)
            {
                
                if (!beats.Contains(b.BeatID) && b.BeatID != "NO BEAT ID")
                {
                    beats.Add(b.BeatID);
                }
            }

            List<PointLatLng> points = new List<PointLatLng>();
                var bList = from b in EsriBeats.beatPolygons
                            where b.BeatID == beatNumber
                            select b;
                foreach (esriBeat eb in bList)
                {
                    ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)eb.beatData.Geometry;
                    foreach (PointCollection pc in p.Rings)
                    {
                        for (int i = 0; i < pc.Count; i++)
                        {
                            points.Add(new PointLatLng(pc[i].Y, pc[i].X));
                        }
                        points.Add(new PointLatLng(pc[0].Y, pc[0].X));
                    }
                    GMapPolygon poly = new GMapPolygon(points, "Beat " + beatNumber);
                    poly.Fill = new SolidBrush(Color.FromArgb(50, Color.Blue));
                    poly.Stroke = new Pen(Color.Red, 1);
                    polyOverlay.Polygons.Add(poly);

                    gMapControl1.Overlays.Add(polyOverlay);
                }
        }

        private void drawDrops(string beatNumber)
        {
            GMapOverlay polyOverlay = new GMapOverlay("drops");
            List<string> drops = new List<string>();
            foreach (EsriDrops.esriDrop d in EsriDrops.dropPolygons)
            {
                if (!drops.Contains(d.BeatID) && d.BeatID != "NO BEAT ID")
                {
                    drops.Add(d.BeatID);
                }
            }
            
            var dList = from d in EsriDrops.dropPolygons
                        where d.BeatID == beatNumber
                        select d;
            foreach (EsriDrops.esriDrop ed in dList)
            {
                ESRI.ArcGIS.Client.Geometry.Polygon p = (ESRI.ArcGIS.Client.Geometry.Polygon)ed.dropData.Geometry;
                List<PointLatLng> points = new List<PointLatLng>();
                foreach (PointCollection pc in p.Rings)
                {
                    for (int i = 0; i < pc.Count; i++)
                    {
                        points.Add(new PointLatLng(pc[i].Y, pc[i].X));
                    }
                    points.Add(new PointLatLng(pc[0].Y, pc[0].X));
                }
                GMapPolygon poly = new GMapPolygon(points, "Drop " + beatNumber);
                poly.Fill = new SolidBrush(Color.FromArgb(50, Color.Orange));
                poly.Stroke = new Pen(Color.Orange, 1);
                polyOverlay.Polygons.Add(poly);

                gMapControl1.Overlays.Add(polyOverlay);
            }
        }

        private Image getImage(string status)
        {
            switch (status.ToUpper())
            {
                case "ONPATROL":
                    return Properties.Resources.mtc_icons_v2_green;
                case "ONBREAK":
                    return Properties.Resources.mtc_icons_v2_lt_blue;
                case "ONLUNCH":
                    return Properties.Resources.mtc_icons_v2_lt_blue;
                case "ENROUTE":
                    return Properties.Resources.mtc_icons_v2_orange;
                case "ONINCIDENT":
                    return Properties.Resources.mtc_icons_v2_blue;
                case "LOGGEDON":
                    return Properties.Resources.mtc_icons_v2_purple;
                case "ROLLOUT":
                    return Properties.Resources.mtc_icons_v2_lt_blue;
                case "ROLLIN":
                    return Properties.Resources.mtc_icons_v2_lt_blue;
                case "ONTOW":
                    return Properties.Resources.mtc_icons_v2_yellow;
                default:
                    return Properties.Resources.mtc_icons_v2_gray;
            }
        }

        private string geticonName(string status)
        {
            switch (status.ToUpper())
            {
                case "ONPATROL":
                    return "mtc_icons_v2_green.png";
                case "ONBREAK":
                    return "mtc_icons_v2_lt_blue.png";
                case "ONLUNCH":
                    return "mtc_icons_v2_lt_blue.png";
                case "ENROUTE":
                    return "mtc_icons_v2_orange.png";
                case "ONINCIDENT":
                    return "mtc_icons_v2_blue.png";
                case "LOGGEDON":
                    return "mtc_icons_v2_purple.png";
                case "ROLLOUT":
                    return "mtc_icons_v2_lt_blue.png";
                case "ROLLIN":
                    return "mtc_icons_v2_lt_blue.png";
                case "ONTOW":
                    return "mtc_icons_v2_yellow.png";
                default:
                    return "mtc_icons_v2_gray.png";
            }
        }

        private Image rotateImageByAngle(Image oldBitmap, int angle)
        {
            angle = angle - 90;

            var newBitmap = new Bitmap(oldBitmap.Width + 4, oldBitmap.Height + 4);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.TranslateTransform((float)oldBitmap.Width / 2, (float)oldBitmap.Height / 2);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-(float)oldBitmap.Width / 2, -(float)oldBitmap.Height / 2);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(oldBitmap, new Point(-4, -4));
            graphics.Dispose();
            return newBitmap;
        }

        public System.Drawing.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        #endregion

        #region " Map Init and Events"

        private void initMap()
        {
            //gMapControl1.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl1.Position = new PointLatLng(37.3653964900001, -121.918542853);
            GMapOverlay overlay = new GMapOverlay("markers");
            //GMapMarkerGoogleGreen home = new GMapMarkerGoogleGreen(new PointLatLng(35.0844, -106.6506));
            GMarkerGoogle home = new GMarkerGoogle(new PointLatLng(35.0844, -106.6506), GMarkerGoogleType.green);
            home.Size = Size.Add(new System.Drawing.Size(20, 20), new System.Drawing.Size(20, 20));
            home.ToolTipText = "This is my home locaiton";
            gMapControl1.OnMarkerClick += new MarkerClick(gMapControl1_OnMarkerClick);
            overlay.Markers.Add(home);
            gMapControl1.Overlays.Add(overlay);
            
        }

        void gMapControl1_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            //MessageBox.Show(item.ToolTipText);
            string data = item.ToolTipText.Replace("Click for more data", "");
            if (!data.Contains("|"))
            {
                return;
            }
            string[] splitter = data.Split('|');
            string truckNumber = splitter[0].Replace("Truck Number: ", "");
            DateTime ts = Convert.ToDateTime(splitter[1].Replace("TimeStamp: ", "").Replace("\\r\\n", ""));
            playBackRow found = globalData.playbackData.Find(delegate(playBackRow find)
            {
                return find.TruckNumber == truckNumber && find.timeStamp == ts;
            });

            int rowHandle = gridView1.LocateByValue("timeStamp", found.timeStamp);
            if (rowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
            {
                gridView1.FocusedRowHandle = rowHandle;

                gridView1.Appearance.FocusedRow.BackColor = Color.Red;
            }
        }

        #endregion

        #region  " Button Clicks and Other Events "

        private void btnExportData_Click(object sender, EventArgs e)
        {
            if (globalData.playbackData.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text Files | *.txt";
                sfd.DefaultExt = "txt";
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    List<string> fileData = new List<string>();
                    foreach(playBackRow pbr in globalData.playbackData)
                    {
                        string rowData = string.Empty;
                        rowData += pbr.timeStamp.ToShortTimeString() + "|";
                        rowData += pbr.TruckNumber + "|";
                        rowData += pbr.Driver + "|";
                        rowData += pbr.CallSign + "|";
                        rowData += pbr.Contractor + "|";
                        rowData += pbr.Status + "|";
                        rowData += pbr.Schedule + "|";
                        rowData += pbr.Beat + "|";
                        rowData += pbr.BeatSegment + "|";
                        rowData += pbr.Speed.ToString() + "|";
                        rowData += pbr.Lat.ToString() + "|";
                        rowData += pbr.Lon.ToString() + "|";
                        rowData += pbr.Heading.ToString() + "|";
                        rowData += pbr.HasAlarms.ToString() + "|";
                        rowData += pbr.AlarmInfo;
                        fileData.Add(rowData);
                    }
                    File.AppendAllLines(fileName, fileData);
                    MessageBox.Show("Finished exporting data to " + fileName);
                }
            }
            else
            {
                MessageBox.Show("No data has been loaded for export");
            }
        }

        private bool checkTimeRange()
        {
            string startTime = string.Empty;
            string endTime = string.Empty;
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (tEndTime.Text != "hh:mm:ss" && tEndTime.Text != "hh:mm:ss")
            {
                startTime = tStartTime.Text;
                endTime = tEndTime.Text;
            }
            else
            {
                MessageBox.Show("Invalid time");
                return false;
            }
            if (string.IsNullOrEmpty(dStartDate.Text) || string.IsNullOrEmpty(dEndDate.Text))
            {
                MessageBox.Show("Invalid date");
                return false;
            }
            try
            {
                startDateTime = Convert.ToDateTime(dStartDate.Text + " " + startTime);
                endDateTime = Convert.ToDateTime(dEndDate.Text + " " + endTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            TimeSpan ts = startDateTime - DateTime.Now;
            int startDay = ts.Days;
            if (startDay < 0)
            {
                return true;
            }
            else
            {
                //Current day, limit to two hour interval
                ts = endDateTime - startDateTime;
                if (ts.Hours > 2)
                {
                    MessageBox.Show("You can only access two hours' worth of data at a time for the current day", "Aborting");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private void btnPreLoad_Click(object sender, EventArgs e)
        {
            /*
            string startTime = string.Empty;
            string endTime = string.Empty;
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (tEndTime.Text != "hh:mm:ss" && tEndTime.Text != "hh:mm:ss")
            {
                startTime = tStartTime.Text;
                endTime = tEndTime.Text;
            }
            else
            {
                MessageBox.Show("Invalid time");
                return;
            }
            if (string.IsNullOrEmpty(dStartDate.Text) || string.IsNullOrEmpty(dEndDate.Text))
            {
                MessageBox.Show("Invalid date");
                return;
            }
            try
            {
                startDateTime = Convert.ToDateTime(dStartDate.Text + " " + startTime);
                endDateTime = Convert.ToDateTime(dEndDate.Text + " " + endTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            TimeSpan ts = endDateTime - startDateTime;
            if (ts.TotalHours > 12)
            {
                MessageBox.Show("Time range must not be more than 12 hours");
                return;
            }
            */

            bool chkTime = checkTimeRange();
            if (!chkTime) {
                return;
            }
            Cursor = Cursors.WaitCursor;
            btnPreLoad.Enabled = false;
            try
            {
                SQLCode sql = new SQLCode();

                List<string> trucks = sql.getData(startDateTime, endDateTime, "TRUCKS");
                List<string> drivers = sql.getData(startDateTime, endDateTime, "DRIVERS");
                List<string> callsigns = sql.getData(startDateTime, endDateTime, "CALLSIGNS");
                //List<string> beats = sql.getData(startDateTime, endDateTime, "BEATS");
                //List<string> contractors = sql.getData(startDateTime, endDateTime, "CONTRACTORS");

                //cboContractors.Items.Clear();
                //cboBeats.Items.Clear();
                cboCallsigns.Items.Clear();
                cboTrucks.Items.Clear();
                cboDrivers.Items.Clear();
                /*
                foreach (string s in contractors)
                { cboContractors.Items.Add(s); }
                foreach (string s in beats)
                { cboBeats.Items.Add(s); }
                 * */
                foreach (string s in callsigns)
                { cboCallsigns.Items.Add(s); }
                foreach (string s in trucks)
                { cboTrucks.Items.Add(s); }
                foreach (string s in drivers)
                { cboDrivers.Items.Add(s); }
                //cboBeats.SelectedIndex = 0;
                cboCallsigns.SelectedIndex = 0;
                cboTrucks.SelectedIndex = 0;
                cboDrivers.SelectedIndex = 0;
                //cboContractors.SelectedIndex = 0;
            }
            catch (TimeoutException tx)
            {
                MessageBox.Show("The System has timed out trying to load your requested date, please select a smaller date/time range", "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor = Cursors.Arrow;
            btnPreLoad.Enabled = true;
            btnLoadData.Enabled = true;
            btnPlayback.BackColor = System.Drawing.Color.Red;
            btnStopPlayback.BackColor = System.Drawing.Color.Red;
            btnPlayback.Enabled = false;
            btnStopPlayback.Enabled = false;
            btnLoadData.BackColor = System.Drawing.Color.LightGreen;
            btnExportData.BackColor = System.Drawing.Color.Red;
            btnExportData.Enabled = false;
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            btnLoadData.Enabled = false;
            btnLoadData.BackColor = System.Drawing.Color.Red;
            btnPlayback.BackColor = System.Drawing.Color.Red;
            btnStopPlayback.BackColor = System.Drawing.Color.Red;
            btnPlayback.Enabled = false;
            btnStopPlayback.Enabled = false;
            Cursor = Cursors.WaitCursor;
            SQLCode sql = new SQLCode();
            bool chkTime = checkTimeRange();
            if (!chkTime)
            {
                return;
            }
            try { 
            if (startDateTime == Convert.ToDateTime("1/1/0001") || endDateTime == Convert.ToDateTime("1/1/0001"))
            {
                MessageBox.Show("Missing Date/Time information, preload data first");
                return;
            }

            if (string.IsNullOrEmpty(cboTrucks.Text))
            {
                MessageBox.Show("Please select a truck first");
                return;
            }
            bool check = true;
            if (cboTrucks.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadTruckPlaybackData(cboTrucks.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboCallsigns.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadCallSignPlayback(cboCallsigns.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboDrivers.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadDriverPlayback(cboDrivers.Text, startDateTime, endDateTime);
                check = false;
            }
            /*  Can't run a query by contractor or beat since that would return multiple trucks
             * Multiple truck completely bone up the graph */
            if (cboContractors.Text.ToUpper() != "SELECT" && check == true)
            {
                /*
                sql.loadContractorPlayback(cboContractors.Text, startDateTime, endDateTime);
                check = false;
                 * */
                MessageBox.Show("This functionality has been disabled");
                return;
            }

            if (cboBeats.Text.ToUpper() != "SELECT" && check == true)
            {
                /*
                sql.loadBeatPlayback(cboBeats.Text, startDateTime, endDateTime);
                check = false;
                 * */
                MessageBox.Show("This functionality has been disabled");
                return;
            }

            gMapControl1.Overlays.Clear();
            if (globalData.playbackData.Count > 0)
            {
                string beatNumber = "NOBEAT";
                foreach (playBackRow row in globalData.playbackData)
                {
                    if (row.Beat != "NOBEAT")
                    {
                        beatNumber = row.Beat;
                        break;
                    }
                }
                drawBeats(beatNumber);
                drawDrops(beatNumber);
                gMapControl1.Position = new PointLatLng(globalData.playbackData[0].Lat, globalData.playbackData[0].Lon);
                //chart data
                chartControl1.Series.Clear();
                //find max speed in range
                int maxSpeed = 0;
                foreach (playBackRow row in globalData.playbackData)
                {
                    if (row.Speed > maxSpeed)
                    {
                        maxSpeed = row.Speed;
                    }
                }
                if (maxSpeed == 0) { maxSpeed = 1; }
                DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series("Speed Over Time", DevExpress.XtraCharts.ViewType.Line);
                DevExpress.XtraCharts.Series barSeries = new DevExpress.XtraCharts.Series("Max Speed", DevExpress.XtraCharts.ViewType.Bar);
                //Add Data to the chart
                foreach (playBackRow row in globalData.playbackData)
                {
                    string st = string.Empty;
                    string hour = row.timeStamp.Hour.ToString();
                    string minute = row.timeStamp.Minute.ToString();
                    string second = row.timeStamp.Second.ToString();
                    while (hour.Length < 2)
                    {
                        hour = "0" + hour;
                    }
                    while (minute.Length < 2)
                    {
                        minute = "0" + minute;
                    }
                    while (second.Length < 2)
                    {
                        second = "0" + second;
                    }
                    st = hour + "." + minute + "." + second;
                    series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(st, row.Speed));
                    DevExpress.XtraCharts.SeriesPoint barPoint = new DevExpress.XtraCharts.SeriesPoint(st, maxSpeed);
                    barPoint.Tag = row.Status;
                    
                    //barPoint.DateTimeArgument = row.timeStamp;
                    barPoint.ToolTipHint = row.Status;
                    barSeries.Points.Add(barPoint);

                    //add map data
                    GMapOverlay overlay = new GMapOverlay(row.timeStamp.ToString());
                    //Image markerImage = Image.FromFile(currentDirectory + geticonName(row.Status));
                    Image markerImage = getImage(row.Status);
                    markerImage = rotateImageByAngle(markerImage, row.Heading);
                    GMapCustomImageMarker marker = new GMapCustomImageMarker(markerImage, new PointLatLng(row.Lat, row.Lon));

                    marker.Size = Size.Add(new System.Drawing.Size(markerImage.Height - 4, markerImage.Width - 4), new System.Drawing.Size(10, 10));
                    marker.ToolTipText = "Truck Number: " + row.TruckNumber + "|TimeStamp: " + row.timeStamp.ToString() + Environment.NewLine + "Click for more data";

                    overlay.Markers.Add(marker);
                    gMapControl1.Overlays.Add(overlay);
                }
                gMapControl1.Refresh();
                //barSeries.ToolTipHintDataMember = barSeries.Tag.ToString();
                chartControl1.Series.Add(barSeries);

                chartControl1.Series.Add(series);


                //map data
                gvData.DataSource = globalData.playbackData;
                gvData.RefreshDataSource();
            }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            

                try
                {
                    List<statusData> status = sql.getStatusData(cboTrucks.Text, startDateTime, endDateTime);
                    gvStatusData.DataSource = status;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                btnLoadData.Enabled = true;
                btnLoadData.BackColor = System.Drawing.Color.LightGreen;
                btnPlayback.BackColor = System.Drawing.Color.LightGreen;
                btnStopPlayback.BackColor = System.Drawing.Color.Red;
                btnPlayback.Enabled = true;
                btnStopPlayback.Enabled = false;
                btnExportData.BackColor = System.Drawing.Color.LightGreen;
                btnExportData.Enabled = true;
                Cursor = Cursors.Arrow;
        }

        private void btnPlayback_Click(object sender, EventArgs e)
        {
            btnPlayback.BackColor = System.Drawing.Color.Red;
            btnPlayback.Enabled = false;
            btnStopPlayback.BackColor = System.Drawing.Color.LightGreen;
            btnStopPlayback.Enabled = true;
            SQLCode sql = new SQLCode();
            mustStop = false;
            gMapControl1.Overlays.Clear();
            if (startDateTime == Convert.ToDateTime("1/1/0001") || endDateTime == Convert.ToDateTime("1/1/0001"))
            {
                MessageBox.Show("Missing Date/Time information, preload data first");
                return;
            }

            if (string.IsNullOrEmpty(cboTrucks.Text))
            {
                MessageBox.Show("Please select a truck first");
                return;
            }
            bool check = true;
            if (cboTrucks.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadTruckPlaybackData(cboTrucks.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboCallsigns.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadCallSignPlayback(cboCallsigns.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboDrivers.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadDriverPlayback(cboDrivers.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboContractors.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadContractorPlayback(cboContractors.Text, startDateTime, endDateTime);
                check = false;
            }

            if (cboBeats.Text.ToUpper() != "SELECT" && check == true)
            {
                sql.loadBeatPlayback(cboBeats.Text, startDateTime, endDateTime);
                check = false;
            }

            //find max speed in range
            int maxSpeed = 0;
            foreach (playBackRow row in globalData.playbackData)
            {
                if (row.Speed > maxSpeed)
                {
                    maxSpeed = row.Speed;
                }
            }
            chartControl1.Series.Clear();
            DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series("Speed Over Time", DevExpress.XtraCharts.ViewType.Line);
            DevExpress.XtraCharts.Series barSeries = new DevExpress.XtraCharts.Series("Max Speed", DevExpress.XtraCharts.ViewType.Bar);

            string beatNumber = "NOBEAT";
            foreach (playBackRow row in globalData.playbackData)
            {
                if (row.Beat != "NOBEAT")
                {
                    beatNumber = row.Beat;
                    break;
                }
            }
            drawBeats(beatNumber);
            drawDrops(beatNumber);
            //Add Data to the chart
            foreach (playBackRow row in globalData.playbackData)
            {
                string st = string.Empty;
                string hour = row.timeStamp.Hour.ToString();
                string minute = row.timeStamp.Minute.ToString();
                string second = row.timeStamp.Second.ToString();
                while (hour.Length < 2)
                {
                    hour = "0" + hour;
                }
                while (minute.Length < 2)
                {
                    minute = "0" + minute;
                }
                while (second.Length < 2)
                {
                    second = "0" + second;
                }
                st = hour + "." + minute + "." + second;
                series.Points.Add(new DevExpress.XtraCharts.SeriesPoint(st, row.Speed));
                DevExpress.XtraCharts.SeriesPoint barPoint = new DevExpress.XtraCharts.SeriesPoint(st, maxSpeed);
                barPoint.Tag = row.Status;
                barSeries.Points.Add(barPoint);

            }
            chartControl1.Series.Add(barSeries);
            chartControl1.Series.Add(series);

            gvData.DataSource = globalData.playbackData;
            gvData.RefreshDataSource();
            try
            {
                List<statusData> status = sql.getStatusData(cboTrucks.Text, startDateTime, endDateTime);
                gvStatusData.DataSource = status;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            int pbSpeed = 1;
            if (!string.IsNullOrEmpty(cboPlaybackSpeed.Text))
            {
                string selSpeed = cboPlaybackSpeed.Text;
                pbSpeed = Convert.ToInt32(selSpeed.Replace("x", ""));
            }
            
            tmrPlayback.Interval = 10000 / pbSpeed; //defaults at updating every 10 seconds
            tmrPlayback.Start();
        }

        #endregion

        #region " Timer and Playback Control "

        void tmrPlayback_Tick(object sender, EventArgs e)
        {
            if (mustStop == false)
            {
                PlaybackData(false);
            }
            else
            {
                tmrPlayback.Stop();
            }
        }

        private void PlaybackData(bool addCrumbs)
        {
            
            if (iCounter >= globalData.playbackData.Count)
            {
                mustStop = true;
            }
            else
            {
                playBackRow row = globalData.playbackData[iCounter];
                putTruckOnMap(row);
                iCounter += 1;
            }
        }

        private void putTruckOnMap(playBackRow row)
        {
            gMapControl1.Position = new PointLatLng(row.Lat, row.Lon);
            GMapOverlay overlay = new GMapOverlay(row.timeStamp.ToString());
            //Image markerImage = Image.FromFile(currentDirectory + geticonName(row.Status));
            Image markerImage = getImage(row.Status);
            markerImage = rotateImageByAngle(markerImage, row.Heading);
            GMapCustomImageMarker marker = new GMapCustomImageMarker(markerImage, new PointLatLng(row.Lat, row.Lon));

            marker.Size = Size.Add(new System.Drawing.Size(markerImage.Height - 4, markerImage.Width - 4), new System.Drawing.Size(10, 10));
            marker.ToolTipText = "Truck Number: " + row.TruckNumber + "|TimeStamp: " + row.timeStamp.ToString() + Environment.NewLine + "Click for more data";

            overlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(overlay);
        }

        private void btnStopPlayback_Click(object sender, EventArgs e)
        {
            btnStopPlayback.BackColor = System.Drawing.Color.Red;
            btnPlayback.BackColor = System.Drawing.Color.LightGreen;
            btnStopPlayback.Enabled = false;
            btnPlayback.Enabled = true;
            tmrPlayback.Stop();
        }
        
        #endregion

        private void cboContractors_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLCode sql = new SQLCode();
            
            if (cboContractors.Text.ToUpper() != "SELECT")
            {
                string where = "AND Contractor = '" + cboContractors.Text.Replace("'", "''") + "'";
                cboBeats.Items.Clear();
                cboCallsigns.Items.Clear();
                cboTrucks.Items.Clear();
                cboDrivers.Items.Clear();
                List<string> beats = sql.getData(startDateTime, endDateTime, "BEATS", where);
                List<string> callSigns = sql.getData(startDateTime, endDateTime, "CALLSIGNS", where);
                List<string> trucks = sql.getData(startDateTime, endDateTime, "TRUCKS", where);
                List<string> drivers = sql.getData(startDateTime, endDateTime, "DRIVERS", where);
                foreach (string s in beats)
                { cboBeats.Items.Add(s); }
                foreach (string s in callSigns)
                { cboCallsigns.Items.Add(s); }
                foreach (string s in trucks)
                { cboTrucks.Items.Add(s); }
                foreach (string s in drivers)
                { cboDrivers.Items.Add(s); }
                cboBeats.SelectedIndex = 0;
                cboCallsigns.SelectedIndex = 0;
                cboTrucks.SelectedIndex = 0;
                cboDrivers.SelectedIndex = 0;
            }
            else
            {
                string where = "NA";
                cboBeats.Items.Clear();
                cboCallsigns.Items.Clear();
                cboTrucks.Items.Clear();
                cboDrivers.Items.Clear();
                List<string> beats = sql.getData(startDateTime, endDateTime, "BEATS", where);
                List<string> callSigns = sql.getData(startDateTime, endDateTime, "CALLSIGNS", where);
                List<string> trucks = sql.getData(startDateTime, endDateTime, "TRUCKS", where);
                List<string> drivers = sql.getData(startDateTime, endDateTime, "DRIVERS", where);
                foreach (string s in beats)
                { cboBeats.Items.Add(s); }
                foreach (string s in callSigns)
                { cboCallsigns.Items.Add(s); }
                foreach (string s in trucks)
                { cboTrucks.Items.Add(s); }
                foreach (string s in drivers)
                { cboDrivers.Items.Add(s); }
                cboBeats.SelectedIndex = 0;
                cboCallsigns.SelectedIndex = 0;
                cboTrucks.SelectedIndex = 0;
                cboDrivers.SelectedIndex = 0;
            }
        }

        private void cboBeats_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLCode sql = new SQLCode();

            if (cboBeats.Text.ToUpper() != "SELECT")
            {
                string where = "AND Beat = '" + cboBeats.Text + "'";
                cboCallsigns.Items.Clear();
                cboTrucks.Items.Clear();
                cboDrivers.Items.Clear();
                List<string> callSigns = sql.getData(startDateTime, endDateTime, "CALLSIGNS", where);
                List<string> trucks = sql.getData(startDateTime, endDateTime, "TRUCKS", where);
                List<string> drivers = sql.getData(startDateTime, endDateTime, "DRIVERS", where);
                foreach (string s in callSigns)
                { cboCallsigns.Items.Add(s); }
                foreach (string s in trucks)
                { cboTrucks.Items.Add(s); }
                foreach (string s in drivers)
                { cboDrivers.Items.Add(s); }
                cboCallsigns.SelectedIndex = 0;
                cboTrucks.SelectedIndex = 0;
                cboDrivers.SelectedIndex = 0;
            }
            else
            {
                string where = "NA";
                cboCallsigns.Items.Clear();
                cboTrucks.Items.Clear();
                cboDrivers.Items.Clear();
                List<string> callSigns = sql.getData(startDateTime, endDateTime, "CALLSIGNS", where);
                List<string> trucks = sql.getData(startDateTime, endDateTime, "TRUCKS", where);
                List<string> drivers = sql.getData(startDateTime, endDateTime, "DRIVERS", where);
                foreach (string s in callSigns)
                { cboCallsigns.Items.Add(s); }
                foreach (string s in trucks)
                { cboTrucks.Items.Add(s); }
                foreach (string s in drivers)
                { cboDrivers.Items.Add(s); }
                cboCallsigns.SelectedIndex = 0;
                cboTrucks.SelectedIndex = 0;
                cboDrivers.SelectedIndex = 0;
            }
        }


    }
}
