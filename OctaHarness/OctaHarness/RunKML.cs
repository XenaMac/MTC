using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OctaHarness
{
    public static class RunKML
    {
        public static List<LatLon> Coords = new List<LatLon>();

        public static void clearCoords()
        {
            Coords.Clear();
        }

        public static void AddCoord(LatLon thisLatLon)
        {
            Coords.Add(thisLatLon);
        }

        
    }
}
