using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace OctaHarness
{
    class ParseKML
    {
        string KMLData;
        string _fileName;
        string coordinatedataraw;
        public List<LatLon> _latlon = new List<LatLon>();

        public void OpenKMLFile()
        {
            OpenFileDialog thisOpen = new OpenFileDialog();
            thisOpen.Title = "Find txt File";
            thisOpen.ShowDialog();
            _fileName = thisOpen.FileName;

            string coordinatedataraw = File.ReadAllText(_fileName);
            if (coordinatedataraw.Contains(",0 "))
            {
                coordinatedataraw = coordinatedataraw.Replace(",0 ", "|");
                string[] splitter = coordinatedataraw.Split('|');
                for (int i = 0; i < splitter.Length; i++)
                {
                    string[] splitLatLon = splitter[i].ToString().Split(',');
                    LatLon thisLatLon = new LatLon();
                    thisLatLon.lat = Convert.ToDouble(splitLatLon[1]);
                    thisLatLon.lon = Convert.ToDouble(splitLatLon[0]);
                    RunKML.AddCoord(thisLatLon);
                }
            }
            else
            {
                string[] splitter = coordinatedataraw.Split(',');
                for (int i = 0; i < splitter.Length; i++)
                {
                    string pairString = splitter[i].ToString().Trim();
                    string[] splitLatLon = pairString.Split(' ');
                    LatLon thisLatLon = new LatLon();
                    thisLatLon.lat = Convert.ToDouble(splitLatLon[1]);
                    thisLatLon.lon = Convert.ToDouble(splitLatLon[0]);
                    RunKML.AddCoord(thisLatLon);
                }
            }
        }
    }

    public class LatLon
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
