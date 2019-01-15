using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using MTC.FSP.Web.Common;
using MTC.FSP.Web.TowTruckServiceRef;
using Newtonsoft.Json;

namespace MTC.FSP.Web.Controllers.Monitoring
{
    [CustomAuthorize(Roles = "Admin,MTC,FSPPartner,CHPOfficer,TowContractor,InVehicleContractor,DataConsultant")]
    public class MapController : MtcBaseController
    {
        #region yards

        public ActionResult GetYardPolygons()
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    var yards = service.getAllYards().OrderBy(p => p.Contractor).ToList();
                    return Json(yards, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"GetYards Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveYardPolygon(yardPolygonData data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.updateYardData(data);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"SaveYard Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult AddYardPolygon(yardPolygonData data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    data.ID = Guid.NewGuid();
                    service.updateYardData(data);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception ex)
            {
                Utilities.LogError($"AddYard Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteYard(Guid id)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.deleteYard(id);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"DeleteYard Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region segments

        public ActionResult GetSegmentPolygons()
        {
            try
            {
                Utilities.LogInfo("segments requested");
                using (var service = new TowTruckServiceClient())
                {
                    var segments = service.getAllSegmentPolygons().OrderBy(p => p.segmentID).ToList();
                    var jsonResult = Json(segments, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    Utilities.LogInfo("segments returned");
                    return jsonResult;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"GetSegments Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveSegmentPolygon(beatSegmentPolygonData data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.updateBeatSegmentData(data);
                    return Json("true", JsonRequestBehavior.AllowGet);
                }              
            }
            catch (Exception ex)
            {
                Utilities.LogError($"SaveSegment Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult AddSegmentPolygon(beatSegmentPolygonData data)
        {
            try
            {
                var stringData = JsonConvert.SerializeObject(data);
                Debug.WriteLine(stringData);

                using (var service = new TowTruckServiceClient())
                {
                    data.ID = Guid.NewGuid();
                    service.updateBeatSegmentData(data);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }               
            }
            catch (Exception ex)
            {
                Utilities.LogError($"AddSegment Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteSegment(Guid id)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.deleteBeatSegment(id);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }              
            }
            catch (Exception ex)
            {
                Utilities.LogError($"DeleteSegment Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region beats

        public ActionResult GetBeats()
        {
            try
            {
                Utilities.LogInfo("beats requested");
                using (var service = new TowTruckServiceClient())
                {
                    var beats = service.getAllBeats().ToList().OrderBy(p => Convert.ToInt32(p.BeatID)).ToList();
                    Utilities.LogInfo("beats returned");
                    return Json(beats, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"GetBeats Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        //via list of segments
        public ActionResult GetBeatSegmentPolygons(string beatId)
        {
            try
            {
                if (string.IsNullOrEmpty(beatId)) return Json(false, JsonRequestBehavior.AllowGet);
                if (beatId == "undefined") return Json(false, JsonRequestBehavior.AllowGet);

                Utilities.LogInfo($"beat segment polygons for beat {beatId}");

                using (var service = new TowTruckServiceClient())
                {
                    var beat = service.getAllBeats().FirstOrDefault(p => p.BeatID == beatId);
                    if (beat?.beatSegmentList != null)
                    {
                        var beatSegmentIds = beat.beatSegmentList.ToList();
                        var beatSegmentPolygons = service.getAllSegmentPolygons().Where(p => beatSegmentIds.Contains(p.segmentID)).OrderBy(p => p.segmentID).ToList();
                        Utilities.LogInfo("beat segment polygons returned");
                        return Json(beatSegmentPolygons, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.LogError($"GetBeatSegmentPolygons Error: {ex.Message}, {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveBeat(beatInformation data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.updateBeatInfoData(data);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception ex)
            {
                Utilities.LogError($"SaveBeat Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddBeat(beatInformation data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    data.ID = Guid.NewGuid();
                    service.updateBeatInfoData(data);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }               
            }
            catch (Exception ex)
            {
                Utilities.LogError($"AddBeat Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteBeat(Guid id)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.deleteBeat(id);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.LogError($"DeleteBeat Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region dropsites

        public ActionResult GetDropSitePolygons()
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    var dropSites = service.getAllDropSites().Where(p => !string.IsNullOrEmpty(p.dropSiteID)).OrderBy(p => p.dropSiteID).ToList();
                    return Json(dropSites, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError($"GetDropSitePolygons Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public ActionResult SaveDropSitePolygon(dropSitePolygonData data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.updateDropSiteData(data);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.LogError($"SaveDropSitePolygon Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
          
        }

        [HttpPost]
        public ActionResult AddDropSitePolygon(dropSitePolygonData data)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    data.ID = Guid.NewGuid();
                    service.updateDropSiteData(data);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.LogError($"AddDropSitePolygon Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteDropSite(Guid id)
        {
            try
            {
                using (var service = new TowTruckServiceClient())
                {
                    service.deleteDropSite(id);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Utilities.LogError($"DeleteDropSite Error: {ex.Message}");
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion


        public ActionResult GetCallSignLocations()
        {
            var callSigns = Utilities.GetFromCache<List<esriCB>>("AllCallSignLocations");
            if (callSigns != null) return Json(callSigns, JsonRequestBehavior.AllowGet);
            using (var service = new TowTruckServiceClient())
            {
                callSigns = service.getAllCallBoxes().OrderBy(p => p.CallBoxNumber).ToList();
                Utilities.AddToCache("AllCallSignLocations", callSigns, DateTime.UtcNow.AddDays(1));
                return Json(callSigns, JsonRequestBehavior.AllowGet);
            }
        }


        //new
        public ActionResult Index()
        {
            return View();
        }

        //old
        public ActionResult IndexOld()
        {
            return View();
        }


        public ActionResult Test()
        {
            return View();
        }
    }
}