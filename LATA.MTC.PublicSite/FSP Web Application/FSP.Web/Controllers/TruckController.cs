using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using FSP.Web.Helpers;
using Microsoft.SqlServer.Types;
using FSP.Domain.Model;
using FSP.Web.Hubs;
using System.Timers;
using FSP.Web.Models;
using FSP.Web.TowTruckServiceRef;
using FSP.Web.Services;

namespace FSP.Web.Controllers
{
    [Authorize] //any logged in user role can see the map
    public class TruckController : Controller
    {
        private FSPDataContext dc = new FSPDataContext();
     
        public ActionResult Map()
        {          
            return View();
        }
        public ActionResult Grid()
        {
            return View();
        }
        
        #region Other Calls

        public ActionResult GetDropZones()
        {
            List<UIDropZone> returnList = new List<UIDropZone>();
            try
            {

                var dropZones = dc.vDropZones.ToList();

                foreach (var dropZone in dropZones)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(dropZone.Position);

                    List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                    for (int i = 1; i < geo.STNumPoints(); i++)
                    {
                        try
                        {
                            SqlGeography point = geo.STPointN(i);
                            Double lat = Convert.ToDouble(point.Lat.ToString());
                            Double lon = Convert.ToDouble(point.Long.ToString());
                            linePoints.Add(new UIMapPolygonLinePoint
                            {
                                Lat = lat,
                                Lon = lon
                            });
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                            Util.WriteToEventLog(ex.Message);
                        }

                    }                   

                    returnList.Add(new UIDropZone
                    {
                        DropZoneID = dropZone.DropZoneID,
                        DropZoneNumber = dropZone.DropZoneNumber,
                        Comments = dropZone.Comments,
                        DropZoneDescription = dropZone.DropZoneDescription,
                        PolygonPoints = linePoints               
                    });
                }

            }
            catch { }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCallBoxes()
        {
            List<UICallBox> returnList = new List<UICallBox>();
            try
            {

                var callBoxes = dc.vCallBoxes;

                foreach (var callBox in callBoxes)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(callBox.Position);

                    Double lat = Convert.ToDouble(geo.STPointN(1).Lat.ToString());
                    Double lon = Convert.ToDouble(geo.STPointN(1).Long.ToString());

                    returnList.Add(new UICallBox
                    {
                        CallBoxId = callBox.CallBoxID,
                        FreewayId = callBox.FreewayID,
                        Lat = lat,
                        Lon = lon,
                        LocationDescription = callBox.Location,
                        TelephoneNumber = callBox.TelephoneNumber
                    });
                }

            }
            catch { }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeats()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();


            try
            {
                var beatsQuery = (from q in dc.vBeats
                                  where q.Active == true
                                  select q).ToList();

                foreach (var beat in beatsQuery)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(beat.BeatExtent.ToString());


                    List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                    for (int i = 1; i < geo.STNumPoints(); i++)
                    {
                        try
                        {
                            SqlGeography point = geo.STPointN(i);
                            Double lat = Convert.ToDouble(point.Lat.ToString());
                            Double lon = Convert.ToDouble(point.Long.ToString());
                            linePoints.Add(new UIMapPolygonLinePoint
                            {
                                Lat = lat,
                                Lon = lon
                            });
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                            Util.WriteToEventLog(ex.Message);
                        }

                    }

                    returnList.Add(new UIMapPolygonLine
                    {
                        Number = beat.BeatNumber,
                        Description = beat.BeatNumber + ": " + beat.BeatDescription,
                        Points = linePoints,
                        Color = beat.BeatColor
                    });
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
                Util.WriteToEventLog(ex.Message);
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatNumbers(String name_startsWith)
        {
            var beatsQuery = from q in dc.vBeats
                             where q.Active == true && q.BeatNumber.Contains(name_startsWith)
                             orderby q.BeatNumber
                             select new
                             {
                                 Number = q.BeatNumber,
                                 Description = q.BeatDescription
                             };

            return Json(beatsQuery, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBeatSegments()
        {
            List<UIMapPolygonLine> returnList = new List<UIMapPolygonLine>();


            try
            {
                var beatsQuery = (from q in dc.vBeatSegments
                                  select q).ToList();

                foreach (var beat in beatsQuery)
                {
                    SqlGeography geo = new SqlGeography();
                    geo = SqlGeography.Parse(beat.BeatSegmentExtent.ToString());


                    List<UIMapPolygonLinePoint> linePoints = new List<UIMapPolygonLinePoint>();
                    for (int i = 1; i < geo.STNumPoints(); i++)
                    {
                        try
                        {
                            SqlGeography point = geo.STPointN(i);
                            Double lat = Convert.ToDouble(point.Lat.ToString());
                            Double lon = Convert.ToDouble(point.Long.ToString());
                            linePoints.Add(new UIMapPolygonLinePoint
                            {
                                Lat = lat,
                                Lon = lon
                            });
                        }
                        catch (Exception ex)
                        {
                            Util.WriteToLog(ex.Message);
                            Util.WriteToEventLog(ex.Message);
                        }

                    }

                    returnList.Add(new UIMapPolygonLine
                    {
                        Number = beat.BeatSegmentNumber,
                        Description = beat.BeatSegmentNumber + ": " + beat.BeatSegmentDescription,
                        Points = linePoints
                    });
                }
            }
            catch (Exception ex)
            {
                Util.WriteToLog(ex.Message);
                Util.WriteToEventLog(ex.Message);
            }

            return Json(returnList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfActiveBeats()
        {
            var beatsQuery = from q in dc.vBeats
                             where q.Active == true
                             select new
                             {
                                 BeatId = q.BeatID,
                                 BeatName = q.BeatDescription
                             };

            return Json(beatsQuery.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOfActiveTowTruckContractors()
        {
            var contractorQuery = from q in dc.Contractors
                                  select new
                                  {
                                      ContractorID = q.ContractorID,
                                      ContractCompanyName = q.ContractCompanyName
                                  };

            return Json(contractorQuery.ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (dc != null)
            {
                dc.Dispose();
            }

            base.Dispose(disposing);
        }
    }

}
