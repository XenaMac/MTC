using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class Yards
    {
        public static List<yardPolygonData> yards = new List<yardPolygonData>();

        #region " Updates "

        public static void addYard(yardPolygonData y)
        {
            for (int i = yards.Count - 1; i >= 0; i--)
            {
                if (yards[i].ID == y.ID)
                {
                    yards.RemoveAt(i);
                }
            }
            yards.Add(y);
        }

        #endregion

        #region " Intersections "

        public static bool checkYardIntersect(double lat, double lon, string contractor)
        {
            bool inPoly = false;
            try
            {
                var spList = from sp in yards
                             where sp.Contractor.ToUpper() == contractor
                             select sp;
                foreach (yardPolygonData b in spList)
                {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        int i;
                        int j = b.geoFence.Count - 1;
                        for (i = 0; i < b.geoFence.Count; i++)
                        {
                            if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                            {
                                if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                {
                                    inPoly = !inPoly;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return inPoly;
        }

        public static string findYardByContractor(double lat, double lon, string contractor)
        {
            string yardName = "NOT FOUND";
            try
            {
                var spList = from sp in yards
                             where sp.Contractor.ToUpper() == contractor
                             select sp;
                foreach (yardPolygonData b in spList)
                {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        int i;
                        int j = b.geoFence.Count - 1;
                        for (i = 0; i < b.geoFence.Count; i++)
                        {
                            if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                            {
                                if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                {
                                    yardName = b.YardID;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return yardName;
        }

        public static string findYard(double lat, double lon)
        {
            string yardName = "NOT FOUND";
            try
            {
                foreach (yardPolygonData b in yards)
                {
                    if (lat >= b.minLat && lat <= b.maxLat && lon >= b.minLon && lon <= b.maxLat)
                    {
                        //possible match, check it
                        int i;
                        int j = b.geoFence.Count - 1;
                        for (i = 0; i < b.geoFence.Count; i++)
                        {
                            if (b.geoFence[i].lon < lon && b.geoFence[j].lon >= lon
                                || b.geoFence[j].lon < lon && b.geoFence[i].lon >= lon)
                            {
                                if (b.geoFence[i].lat + (lon - b.geoFence[i].lon) / (b.geoFence[j].lon - b.geoFence[i].lon) * (b.geoFence[j].lat - b.geoFence[i].lat) < lat)
                                {
                                    yardName = b.YardID;
                                }
                            }
                            j = i;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return yardName;
        }

        #endregion
    }

    #region " object definition "

    public class yardPolygonData
    {
        public Guid ID { get; set; }
        public string YardID { get; set; }
        public string BeatID { get; set; }
        public string Contractor { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public List<latLon> geoFence { get; set; }
        public double minLat { get; set; }
        public double minLon { get; set; }
        public double maxLat { get; set; }
        public double maxLon { get; set; }
    }

    #endregion
}