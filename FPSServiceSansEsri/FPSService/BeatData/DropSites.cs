﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.BeatData
{
    public static class DropSites
    {
        public static List<dropSitePolygonData> dropSites = new List<dropSitePolygonData>();

        public static void LoadDropSites() {
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.loadDropSiteData();
        }

        #region " updates "

        public static void addDropSite(dropSitePolygonData d)
        {
            for (int i = dropSites.Count - 1; i >= 0; i--)
            {
                if (dropSites[i].ID == d.ID)
                {
                    dropSites.RemoveAt(i);
                }
            }
            
                d.maxLat = d.geoFence[0].lat;
                d.maxLon = d.geoFence[0].lon;
                d.minLat = d.geoFence[0].lat;
                d.minLon = d.geoFence[0].lon;
                foreach (latLon ll in d.geoFence) {
                    if (ll.lat < d.minLat) {
                        d.minLat = ll.lat;
                    }
                    if (ll.lat > d.maxLat) {
                        d.maxLat = ll.lat;
                    }
                    if (ll.lon < d.minLon) {
                        d.minLon = ll.lon;
                    }
                    if (ll.lon > d.maxLon) {
                        d.maxLon = ll.lon;
                    }
                }    
            

            SQL.SQLCode sql = new SQL.SQLCode();
            sql.updateDropSitePolygon(d);
            dropSites.Add(d);
        }

        public static void deleteDropSite(Guid id)
        {
            for (int i = dropSites.Count - 1; i >= 0; i--)
            {
                if (dropSites[i].ID == id)
                {
                    dropSites.RemoveAt(i);
                }
            }
            SQL.SQLCode sql = new SQL.SQLCode();
            sql.deleteDropSiteData(id);
        }

        #endregion

        #region " intersections "

        public static bool checkDropSiteIntersect(double lat, double lon, string name)
        {
            bool inPoly = false;
            try
            {
                var spList = from sp in dropSites
                             where sp.dropSiteID == name
                             select sp;
                foreach (dropSitePolygonData b in dropSites)
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

        public static string findDropSiteByBeat(double lat, double lon, string beatID)
        {
            string dsName = "NOT FOUND";
            try
            {
                var spList = from sp in dropSites
                             where sp.beatID == beatID
                             select sp;
                foreach (dropSitePolygonData b in spList)
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
                                    dsName = b.dropSiteID;
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
            return dsName;
        }

        public static string findDropSite(double lat, double lon)
        {
            string dsName = "NOT FOUND";
            try
            {
                foreach (dropSitePolygonData b in dropSites)
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
                                    dsName = b.dropSiteID;
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
            return dsName;
        }

        #endregion

        #region " Helper list "

        public static List<dropSitePolygonData> sitesByBeat(string beatID)
        {
            List<dropSitePolygonData> sites = new List<dropSitePolygonData>();
            try
            {
                var spList = from sp in dropSites
                             where sp.beatID == beatID
                             select sp;
                foreach (dropSitePolygonData b in spList)
                {
                    sites.Add(b);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sites;
        }

        #endregion
    }

    #region " Object definition "

    //moved to ITowTruckService.cs

    #endregion
}