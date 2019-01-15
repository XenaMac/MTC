using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Charts;
using DevExpress.Xpf.Charts;


namespace MTCPlaybackRev2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime startDateTime;
        DateTime endDateTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            SQLCode sql = new SQLCode();
            //{1/1/0001 12:00:00 AM
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
            
            sql.loadPlaybackData(cboTrucks.Text, startDateTime, endDateTime);
            if (globalData.playbackData.Count > 0)
            {

                foreach (List<playBackRow> list in globalData.playbackData)
                {
                    //draw data on the map
                    var graphicsLayer = new Esri.ArcGISRuntime.Layers.GraphicsLayer();
                    graphicsLayer.ID = "PointPlots";
                    Esri.ArcGISRuntime.Geometry.SpatialReference aSpacRef = new Esri.ArcGISRuntime.Geometry.SpatialReference(4326);
                    MyMap.Layers.Add(graphicsLayer);
                    
                    LineSeries2D series = new LineSeries2D();
                    //series.ArgumentScaleType = ScaleType.DateTime;

                    int iCount = 0;
                    foreach (playBackRow row in list)
                    {

                        //Add Data to the chart
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
                        series.Points.Add(new SeriesPoint(st, row.Speed));
                        

                        //Add Data to the map
                        var markerSym = new Esri.ArcGISRuntime.Symbology.SimpleMarkerSymbol();
                        switch (row.Status.ToUpper())
                        {
                            case "ONPATROL":
                                markerSym.Color = ConvertStringToColor("#219846");
                                break;
                            case "ONBREAK":
                                markerSym.Color = ConvertStringToColor("#5ef2fb");
                                break;
                            case "ONLUNCH":
                                markerSym.Color = ConvertStringToColor("#5ef2fb");
                                break;
                            case "FORCEDBREAK":
                                markerSym.Color = ConvertStringToColor("#5ef2fb");
                                break;
                            case "ROLLOUT":
                                markerSym.Color = ConvertStringToColor("#5ef2fb");
                                break;
                            case "ONINCIDENT":
                                markerSym.Color = ConvertStringToColor("#265cff");
                                break;
                            case "ONTOW":
                                markerSym.Color = ConvertStringToColor("#dee742");
                                break;
                            case "ROLLIN":
                                markerSym.Color = ConvertStringToColor("#5ef2fb");
                                break;
                            case "ENROUTE":
                                markerSym.Color = ConvertStringToColor("#f37330");
                                break;
                            default:
                                markerSym.Color = Colors.Red;
                                break;
                        }
                        //markerSym.Color = Colors.Red;
                        markerSym.Style = Esri.ArcGISRuntime.Symbology.SimpleMarkerStyle.Circle;
                        markerSym.Size = 5;

                        Esri.ArcGISRuntime.Geometry.MapPoint aPoint = new Esri.ArcGISRuntime.Geometry.MapPoint();
                        aPoint.SpatialReference = aSpacRef;
                        aPoint.X = row.Lon;
                        aPoint.Y = row.Lat;

                        var pointGraphic = new Esri.ArcGISRuntime.Layers.Graphic();
                        pointGraphic.Geometry = aPoint;
                        pointGraphic.Symbol = markerSym;

                        graphicsLayer.Graphics.Add(pointGraphic);
                        iCount += 1;
                    }
                    //series.ArgumentScaleType = ScaleType.DateTime;
                    
                    chart.Series.Add(series);
                    
                }
                
                myGrid.ItemsSource = globalData.playbackData[0];

                List<statusData> status = sql.getStatusData(cboTrucks.Text, startDateTime, endDateTime);
                spStatus.Children.Clear();
                foreach (statusData sd in status)
                {
                    Label l = new Label();
                    l.Content = sd.statusName + " " + sd.statusMins.ToString();
                    spStatus.Children.Add(l);
                }
            }
        }

        public System.Windows.Media.Color ConvertStringToColor(String hex)
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

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        private void btnPreload_Click(object sender, RoutedEventArgs e)
        {
            string startTime = string.Empty;
            string endTime = string.Empty;
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (txtEndTime.Text != "hh:mm:ss" && txtStartTime.Text != "hh:mm:ss")
            {
                startTime = txtStartTime.Text;
                endTime = txtEndTime.Text;
            }
            else
            {
                MessageBox.Show("Invalid time");
                return;
            }
            if (string.IsNullOrEmpty(dtStart.Text) || string.IsNullOrEmpty(dtEnd.Text))
            {
                MessageBox.Show("Invalid date");
                return;
            }
            try
            {
                startDateTime = Convert.ToDateTime(dtStart.Text + " " + startTime);
                endDateTime = Convert.ToDateTime(dtEnd.Text + " " + endTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            SQLCode sql = new SQLCode();
            List<string> trucks = sql.getTrucks(startDateTime, endDateTime);
            cboTrucks.ItemsSource = trucks;
        }
    }
}
