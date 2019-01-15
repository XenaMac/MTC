using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.MiscData
{
    public class DataDumper
    {
        Timer dumpTimer;

        public DataDumper()
        {
            dumpTimer = new Timer(30000);
            dumpTimer.Elapsed += new ElapsedEventHandler(dumpTimer_Elapsed);
            dumpTimer.Start();
        }

        void dumpTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<dataDump> dumpData = new List<dataDump>();
            foreach (TowTruck.TowTruck t in DataClasses.GlobalData.currentTrucks)
            {
                //if (t.Status.VehicleStatus != "Waiting for Driver Login")
                //{
                    dataDump d = new dataDump();
                    d.TruckNumber = t.TruckNumber;
                    d.timeStamp = t.LastMessage.LastMessageReceived;
                    d.Driver = t.Driver.LastName + ", " + t.Driver.FirstName;
                    d.CallSign = t.Driver.callSign;
                    d.Contractor = t.Extended.ContractorName;
                    d.Status = t.tts.currentStatus;
                    d.Schedule = t.Driver.AssignedShift;
                    d.Beat = t.beatNumber;
                    d.BeatSegment = t.location;
                    d.Speed = t.GPSPosition.Speed;
                    d.Lat = t.GPSPosition.Lat;
                    d.Lon = t.GPSPosition.Lon;
                    d.Heading = t.GPSPosition.Head;
                    if (t.tta.hasAlarms == true)
                    {
                        d.HasAlarm = 1;
                        string alarmData = string.Empty;
                        foreach (TowTruck.AlarmTimer at in t.tta.truckAlarms)
                        {
                            if (at.hasAlarm == true)
                            {
                                alarmData += at.alarmName + " " + at.alarmValue + ":";
                            }
                        }
                        d.AlarmInfo = alarmData;
                    }
                    else
                    {
                        d.HasAlarm = 0;
                        d.AlarmInfo = "NO ALARMS";
                    }
                    d.RunID = t.runID;
                    dumpData.Add(d);
                //}
            }
            if (dumpData.Count > 0)
            {
                SQL.SQLCode mysQL = new SQL.SQLCode();
                mysQL.dumpPlaybackData(dumpData);
            }
        }
    }

    public class dataDump
    {
        public DateTime timeStamp { get; set; }
        public string TruckNumber { get; set; }
        public string Driver { get; set; }
        public string CallSign { get; set; }
        public string Contractor { get; set; }
        public string Status { get; set; }
        public string Schedule { get; set; }
        public string Beat { get; set; }
        public string BeatSegment { get; set; }
        public double Speed { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Heading { get; set; }
        public int HasAlarm { get; set; }
        public string AlarmInfo { get; set; }
        public Guid RunID { get; set; }
    }
}