using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace OctaHarness
{
    public class ParseCSV
    {
        private string _FileName;
        List<CSVTruck> playbackTruck = new List<CSVTruck>();
        public ParseCSV()
        {
            OpenFileDialog myOpen = new OpenFileDialog();
            myOpen.ShowDialog();
            _FileName = myOpen.FileName;
        }

        public void ProcessData()
        {
            if (string.IsNullOrEmpty(_FileName))
            {
                MessageBox.Show("No file selected");
                return;
            }
            string[] FileData = File.ReadAllLines(_FileName);
            for (int i = 0; i < FileData.Length; i++)
            {
                string[] splitter = FileData[i].ToString().Split(',');
                CSVTruck thisTruck = new CSVTruck();
                thisTruck.TruckNumber = splitter[0].ToString();
                thisTruck.Direction = Convert.ToInt32(splitter[1].ToString());
                thisTruck.VehicleStatus = splitter[2].ToString();
                thisTruck.timeStamp = DateTime.Parse(splitter[3].ToString());
                thisTruck.Speed = Convert.ToInt32(splitter[4].ToString());
                thisTruck.MaxSpeed = Convert.ToInt32(splitter[7].ToString());
                string LatLon = splitter[8].ToString();
                LatLon = LatLon.Replace("POINT (", "");
                LatLon = LatLon.Replace(")", "");
                string[] splitLatLon = LatLon.Split(' ');
                thisTruck.Lon = Convert.ToDouble(splitLatLon[0]);
                thisTruck.Lat = Convert.ToDouble(splitLatLon[1]);
                RunCSV.AddPlaybackTruck(thisTruck);
            }
        }
    }
}
