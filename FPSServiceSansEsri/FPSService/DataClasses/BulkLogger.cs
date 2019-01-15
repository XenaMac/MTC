using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Collections;

namespace FPSService.DataClasses
{
    public class BulkLogger
    {
        public BulkLogger()
        {
            Timer tmrLogTrucks = new Timer(30000);
            tmrLogTrucks.Elapsed += new ElapsedEventHandler(tmrLogTrucks_Elapsed);
            tmrLogTrucks.Enabled = true;
        }

        void tmrLogTrucks_Elapsed(object sender, ElapsedEventArgs e)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            foreach (TowTruck.TowTruck thisTruck in DataClasses.GlobalData.currentTrucks.ToList())
            {
                bool Alarms = false;
                TowTruck.AlarmTimer alarmSpd = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findSpd) { return findSpd.alarmName == "Speeding"; });
                TowTruck.AlarmTimer alarmOB = thisTruck.tta.truckAlarms.Find(delegate(TowTruck.AlarmTimer findOB) { return findOB.alarmName == "OffBeat"; });
                thisTruck.tta.checkAlarms();
                if (thisTruck.tta.hasAlarms == true)
                {
                    Alarms = true;
                }

                ArrayList arrParams = new ArrayList();
                arrParams.Add("@Direction^" + thisTruck.GPSPosition.Head);
                arrParams.Add("@VehicleStatus^" + thisTruck.Status.VehicleStatus);
                arrParams.Add("@LastUpdate^" + thisTruck.GPSPosition.Time.ToString());
                arrParams.Add("@VehicleID^" + thisTruck.TruckNumber);
                arrParams.Add("@Speed^" + thisTruck.GPSPosition.Speed.ToString());
                arrParams.Add("@Alarms^" + Alarms);
                arrParams.Add("@DriverID^" + thisTruck.Driver.DriverID.ToString());
                arrParams.Add("@CallSign^" + thisTruck.Driver.callSign);
                arrParams.Add("@AssignedShift^" + thisTruck.Driver.AssignedShift);
                arrParams.Add("@VehicleNumber^" + thisTruck.Identifier.ToString());
                arrParams.Add("@SpeedingAlarm^" + alarmSpd.hasAlarm.ToString());
                arrParams.Add("@SpeedingValue^" + alarmSpd.alarmValue);
                arrParams.Add("@SpeedingTime^" + alarmSpd.alarmStart.ToString());
                arrParams.Add("@OutOfBoundsAlarm^" + alarmOB.hasAlarm.ToString());
                arrParams.Add("@OutOfBoundsMessage^" + alarmOB.alarmValue);
                string CellVal;
                if (string.IsNullOrEmpty(thisTruck.GPSPosition.Cell))
                {
                    CellVal = "NA";
                }
                else
                {
                    CellVal = thisTruck.GPSPosition.Cell;
                }
                arrParams.Add("@Cell^" + CellVal);
                arrParams.Add("@runID^" + thisTruck.runID);
                arrParams.Add("@AssignedBeat^" + thisTruck.beatNumber);
                arrParams.Add("@Beat^" + thisTruck.beatNumber);
                arrParams.Add("@BeatSegment^" + thisTruck.location);
                arrParams.Add("@Lat^" + thisTruck.GPSPosition.Lat);
                arrParams.Add("@Lon^" + thisTruck.GPSPosition.Lon);
                if (alarmSpd.hasAlarm)
                {
                    arrParams.Add("@SLat^" + thisTruck.GPSPosition.Lat);
                    arrParams.Add("@SLon^" + thisTruck.GPSPosition.Lon);
                }
                else
                {
                    arrParams.Add("@SLat^0.0");
                    arrParams.Add("@SLon^0.0");
                }
                mySQL.LogGPS("SetGPS", arrParams);
            }
        }
    }
}