using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class Yards
    {
        public static List<yardPolygonData> yards = new List<yardPolygonData>();

        public static void LoadYards() {
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.loadYardData();
        }

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

                y.maxLat = y.geoFence[0].lat;
                y.maxLon = y.geoFence[0].lon;
                y.minLat = y.geoFence[0].lat;
                y.minLon = y.geoFence[0].lon;
                foreach (latLon ll in y.geoFence)
                {
                    if (ll.lat < y.minLat)
                    {
                        y.minLat = ll.lat;
                    }
                    if (ll.lat > y.maxLat)
                    {
                        y.maxLat = ll.lat;
                    }
                    if (ll.lon < y.minLon)
                    {
                        y.minLon = ll.lon;
                    }
                    if (ll.lon > y.maxLon)
                    {
                        y.maxLon = ll.lon;
                    }
                }

            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateYardPolygon(y);
            yards.Add(y);
        }

        public static void deleteYard(Guid id)
        {
            for (int i = yards.Count - 1; i >= 0; i--)
            {
                if (yards[i].ID == id)
                {
                    yards.RemoveAt(i);
                }
            }

            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteYardData(id);
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

    //moved to ITowTruckService.cs

    #endregion
}