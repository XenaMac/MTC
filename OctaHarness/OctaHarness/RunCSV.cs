using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OctaHarness
{
    public static class RunCSV
    {
        public static List<CSVTruck> playbackTrucks = new List<CSVTruck>();

        public static void ClearPlayBackTrucks()
        {
            playbackTrucks.Clear();
        }

        public static void AddPlaybackTruck(CSVTruck thisTruck)
        {
            playbackTrucks.Add(thisTruck);
        }
    }
}
