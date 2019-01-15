using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace FPSService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EsriService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EsriService.svc or EsriService.svc.cs at the Solution Explorer and start debugging.
        [AspNetCompatibilityRequirements(
            RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EsriService : IEsriService
    {
        public void DoWork()
        {

        }

        public List<EsriTruck> GetEsriTrucks()
        {
            List<EsriTruck> allTrucks = new List<EsriTruck>();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks)
            {
                bool HasAlarms = false;
                string _DriverName = "";
                if (thisTruck.Driver.LastName != "Not Logged On")
                {
                    _DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                }
                else
                {
                    _DriverName = "Not Logged On";
                }
                /*
                if (thisTruck.Status.OutOfBoundsAlarm == true || thisTruck.Status.SpeedingAlarm == true)
                { HasAlarms = true; }*/

                TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });

                thisTruck.tta.checkAlarms();
                if (thisTruck.tta.hasAlarms == true)
                {
                    HasAlarms = true;
                }

                bool speedingAlarm = false;
                string speedingValue = "NA";
                DateTime speedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmSpd != null)
                {
                    speedingAlarm = alarmSpd.hasAlarm;
                    speedingValue = alarmSpd.alarmValue;
                    speedingTime = alarmSpd.alarmStart;
                }

                bool obAlarm = false;
                string obMessage = "NA";
                DateTime obTime = Convert.ToDateTime("01/01/2001 00:00:00");
                if (alarmOB != null)
                {
                    obAlarm = alarmOB.hasAlarm;
                    obMessage = alarmOB.alarmValue;
                    obTime = alarmOB.alarmStart;
                }

                EsriTruck et = new EsriTruck();
                et.TruckNumber = thisTruck.TruckNumber;
                et.CallSign = thisTruck.Driver.callSign;
                et.Direction = thisTruck.GPSPosition.Head;
                et.Speed = thisTruck.GPSPosition.Speed;
                et.VehicleState = thisTruck.tts.currentStatus;
                et.Alarms = HasAlarms.ToString().ToLower();
                et.SpeedingAlarm = speedingAlarm.ToString().ToLower();
                et.SpeedingTime = DataClasses.MakeUnixTime.ConvertToUnixTime(speedingTime);
                et.OutOfBoundsAlarms = obAlarm.ToString().ToLower();
                et.OutOfBoundsMessage = obMessage;
                et.OutOfBoundsTime = DataClasses.MakeUnixTime.ConvertToUnixTime(obTime);
                et.Heading = thisTruck.GPSPosition.Head;
                et.BeatNumber = thisTruck.beatNumber;
                et.IPAddress = thisTruck.Identifier;
                et.LastMessage = DataClasses.MakeUnixTime.ConvertToUnixTime(thisTruck.LastMessage.LastMessageReceived);
                et.ContractorName = thisTruck.Extended.ContractorName;
                et.DriverName = thisTruck.Driver.LastName + ", " + thisTruck.Driver.FirstName;
                et.StatusStarted = DataClasses.MakeUnixTime.ConvertToUnixTime(thisTruck.Status.StatusStarted);
                SpatialReference sr = new SpatialReference();
                sr.wkid = 4326;
                EsriGeometry Geometry = new EsriGeometry();
                Geometry.x = thisTruck.GPSPosition.Lon;
                Geometry.y = thisTruck.GPSPosition.Lat;
                Geometry.z = 0;
                Geometry.spatialReference = sr;
                et.Geometry = Geometry;
                allTrucks.Add(et);
            }
            return allTrucks;
        }
    }
}
