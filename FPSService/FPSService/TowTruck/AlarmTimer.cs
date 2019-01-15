using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace FPSService.TowTruck
{
    public class AlarmTimer
    {
        public DateTime alarmStart { get; set; }
        public DateTime alarmEnd { get; set; }
        public int alarmMinutes { get; set; }
        public int currentMinutes { get; set; }
        private Timer tmrStatus;
        private bool tmrRunning = false;
        SQL.SQLCode sql;
        public string alarmName;
        public string alarmValue;
        public bool hasAlarm;
        public Guid _alarmID = new Guid();
        public DateTime alarmCleared;
        public DateTime alarmExcused;
        public DateTime statusStarted;
        public string comment;
        public string excusedBy;
        public bool alarmLogged;

        public AlarmTimer(string _alarmName)
        {
            alarmName = _alarmName;
            hasAlarm = false;
            tmrStatus = new Timer();
            tmrStatus.Interval = 1000;
            tmrStatus.Elapsed += tmrStatus_Elapsed;
            alarmStart = Convert.ToDateTime("01/01/2001 00:00:00");
            alarmEnd = Convert.ToDateTime("01/01/2001 00:00:00");
            _alarmID = new Guid("00000000-0000-0000-0000-000000000000");
            alarmCleared = Convert.ToDateTime("01/01/2001 00:00:00");
            alarmExcused = Convert.ToDateTime("01/01/2001 00:00:00");
            statusStarted = Convert.ToDateTime("01/01/2001 00:00:00");
            alarmLogged = false;
        }

        private void tmrStatus_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentMinutes += 1;
        }

        public bool isRunning()
        {
            return tmrRunning;
        }

        public void ExcuseAlarm(Guid _runID, Guid _alertID, string _excusedBy, string _comment)
        {
            alarmExcused = DateTime.Now;
            excusedBy = _excusedBy;
            if (isRunning() || this.alarmName.ToUpper() == "LATEONPATROL" || this.alarmName.ToUpper() == "EARLYOUTOFSERVICE")
            {
                alarmEnd = DateTime.Now;
                tmrStatus.Stop();
                TimeSpan ts = alarmEnd - alarmStart;
                alarmMinutes += Convert.ToInt32(ts.TotalMinutes);
                tmrRunning = false;
                hasAlarm = false;
                //_alertID = this._alarmID;
                sql = new SQL.SQLCode();
                sql.excuseAlarm(_runID, _alertID, _excusedBy, _comment);
                if (this.alarmName.ToUpper() != "LATEONPATROL" && this.alarmName.ToUpper() != "EARLYOUTOFSERVICE")
                {
                    _alarmID = new Guid("00000000-0000-0000-0000-000000000000");
                }

            }
        }

        public void clearAlarm(Guid _runID, Guid _alertID)
        {
            alarmCleared = DateTime.Now;
            if (isRunning() || this.alarmName.ToUpper() == "LATEONPATROL" || this.alarmName.ToUpper() == "EARLYOUTOFSERVICE")
            {
                alarmEnd = DateTime.Now;
                tmrStatus.Stop();
                TimeSpan ts = alarmEnd - alarmStart;
                alarmMinutes += Convert.ToInt32(ts.TotalMinutes);
                tmrRunning = false;
                hasAlarm = false;
                sql = new SQL.SQLCode();
                sql.clearAlarm(_runID, _alertID);
                if (this.alarmName.ToUpper() != "LATEONPATROL" && this.alarmName.ToUpper() != "EARLYOUTOFSERVICE")
                {
                    _alarmID = new Guid("00000000-0000-0000-0000-000000000000");
                }
            }
            hasAlarm = false; //fix for Onpatrol and roll in alarms that start and stop but keep the alarm status = true
        }
        

        public void StartAlarm(string truckNumber, string driverName, string contractorCompany, string Beat, string AlertName, double lat, double lon, 
            Guid runID, string location, double speed, int heading)
        {
            Guid g = new Guid();
            
            if (tmrRunning == false)
            {
                //alarmStart = DateTime.Now;
                //alarmEnd = Convert.ToDateTime("01/01/2001 00:00:00");
                statusStarted = DateTime.Now;
                alarmLogged = false;
                alarmEnd = Convert.ToDateTime("01/01/2001 00:00:00");
                currentMinutes = 0;
                g = Guid.NewGuid();
                tmrStatus.Start();
                tmrRunning = true;
                //hasAlarm = true;
                _alarmID = g;
                
            }
        }

        public void logAlarm(string truckNumber, string driverName, string contractorCompany, string Beat, string AlertName, double lat, double lon,
            Guid runID, string location, double speed, int heading, string CallSign, Guid ScheduleID, int ScheduleType)
        {
            if (tmrRunning == true && alarmLogged == false)
            {
                sql = new SQL.SQLCode();
                alarmStart = DateTime.Now;
                hasAlarm = true;
                sql.LogAlert(_alarmID, truckNumber, driverName, contractorCompany, Beat, AlertName, alarmStart, alarmEnd, lat, lon, runID, location, speed, heading, CallSign, ScheduleID, ScheduleType);
                if (AlertName.ToUpper() == "OVERTIMEACTIVITY")
                {
                    TowTruck t = DataClasses.GlobalData.currentTrucks.Find(delegate(TowTruck find) { return find.TruckNumber == truckNumber; });
                    if (t != null)
                    {
                        sql.logOverTime(t.Driver.schedule.ScheduleName, t.Driver.callSign, t.beatNumber, t.Extended.ContractorName);
                    }
                }
                alarmLogged = true;
            }
        }

        public string getAlarmStart() //this is the time statusStarted + leeway when the alarm started
        {
            return alarmStart.ToString("HH:mm:ss");
        }

        public string GetStartTime() //as soon as a truck enters a potential alarm state this should set.
        {
            //return alarmStart.ToString("HH:mm:ss");
            return statusStarted.ToString("HH:mm:ss");
        }

        public DateTime GetStartDateTime()
        {
            return statusStarted;
        }

        public DateTime GetEndDateTime()
        {
            return alarmEnd;
        }

        public string GetEndTime()
        {
            return alarmEnd.ToString("HH:mm:ss");
        }

        public int getStatusMinutes()
        {
            int totalMins = 0;

            if (alarmStart != Convert.ToDateTime("01/01/2001 00:00:00"))
            {
                if (alarmEnd != Convert.ToDateTime("01/01/2001 00:00:00"))
                {
                    TimeSpan ts = alarmEnd - alarmStart;
                    totalMins = (int)ts.TotalMinutes;
                }
                else
                {
                    TimeSpan ts = DateTime.Now - alarmStart;
                    totalMins = (int)ts.TotalMinutes;
                }
            }

            return totalMins;
        }

        public void StopAlarm(string truckNumber, string driverName, string contractorCompany, string Beat, string AlertName, double lat, double lon,
            Guid runID, string location, double speed, int heading, string CallSign, Guid ScheduleID, int ScheduleType)
        {
            if (tmrRunning == true)
            {
                alarmEnd = DateTime.Now;
                tmrStatus.Stop();
                TimeSpan ts;
                if (alarmStart != Convert.ToDateTime("01/01/2001 00:00:00"))
                {
                    ts = alarmEnd - alarmStart;
                }
                else
                {
                    alarmStart = statusStarted;
                    ts = alarmEnd - alarmStart;
                }
                
                alarmMinutes = Convert.ToInt32(ts.TotalMinutes);
                tmrRunning = false;
                
                alarmLogged = false;
                if (hasAlarm == true || (AlertName == "LateOnPatrol" || AlertName == "EarlyOutOfService"))
                {
                    sql = new SQL.SQLCode();
                    sql.LogAlert(_alarmID, truckNumber, driverName, contractorCompany, Beat, AlertName, alarmStart, alarmEnd, lat, lon, runID, location, speed, heading, CallSign, ScheduleID, ScheduleType);
                }
                if ((AlertName == "LongBreak" || AlertName == "LongLunch" || AlertName == "Stationary" ||
                    AlertName == "OffBeat" || AlertName == "LateOnPatrol" || AlertName == "EarlyOutOfService") && hasAlarm == true)
                {
                    try
                    {
                        AlarmLog al = new AlarmLog();
                        TowTruck t = DataClasses.GlobalData.FindTowTruckByVehicleNumber(truckNumber);
                        if (t == null)
                        {
                            return;
                        }
                        Guid BeatID = sql.GetBeatIDByBeatNumber(Beat);
                        al.CreatedBy = "FSP Service";
                        al.CreatedOn = DateTime.Now;
                        al.ContractorID = t.Extended.ContractorID;
                        al.DateTimeOfViolation = DateTime.Now;
                        al.BeatID = BeatID;
                        al.DriverID = t.Driver.DriverID;
                        al.FleetVehicleId = t.Extended.FleetVehicleID;
                        al.CallSign = t.Driver.callSign;
                        al.AlarmName = AlertName;
                        al.AlertTime = alarmStart;
                        al.LengthOfViolation = alarmMinutes;
                        //sub breakout
                        if (AlertName == "LongBreak")
                        {
                            if (alarmMinutes < 60)
                            {
                                //this is a breaks violation (#49)
                                al.ViolationTypeId = 49;
                            }
                            if (alarmMinutes > 60)
                            {
                                //this is a service failure (#46)
                                al.ViolationTypeId = 46;
                            }
                        }
                        if (AlertName == "LongLunch")
                        {
                            if (alarmMinutes <= 75)
                            {
                                //this is a breaks violation (#49)
                                al.ViolationTypeId = 49;
                            }
                            if (alarmMinutes > 75)
                            {
                                //this is a service failure (#46)
                                al.ViolationTypeId = 46;
                            }
                        }
                        if (AlertName == "Stationary")
                        {
                            if (alarmMinutes <= 60)
                            {
                                //this is a breaks violation (#49)
                                al.ViolationTypeId = 49;
                            }
                            if (alarmMinutes > 60)
                            {
                                //this is a service failure (#46)
                                al.ViolationTypeId = 46;
                            }
                        }
                        if (AlertName == "OffBeat")
                        {
                            if (alarmMinutes <= 60)
                            {
                                //this is a breaks violation (#49)
                                al.ViolationTypeId = 49;
                            }
                            if (alarmMinutes > 60)
                            {
                                //this is a service failure (#46)
                                al.ViolationTypeId = 46;
                            }
                        }
                        if (AlertName == "LateOnPatrol")
                        {
                            TimeSpan lop = DateTime.Now - t.Driver.schedule.start;
                            if (lop.TotalMinutes <= 60)
                            {
                                al.ViolationTypeId = 49;
                            }
                            if (lop.TotalMinutes > 60)
                            {
                                al.ViolationTypeId = 46;
                            }
                            al.LengthOfViolation = (int)lop.TotalMinutes;
                        }
                        if (AlertName == "EarlyOutOfService")
                        {
                            TimeSpan lop = t.Driver.schedule.end - DateTime.Now;
                            if (lop.TotalMinutes <= 60)
                            {
                                al.ViolationTypeId = 49;
                            }
                            if (lop.TotalMinutes > 60)
                            {
                                al.ViolationTypeId = 46;
                            }
                            al.LengthOfViolation = (int)lop.TotalMinutes;
                        }
                        //log it to the violations table.
                        al.Lat = t.GPSPosition.Lat;
                        al.Lon = t.GPSPosition.Lon;
                        al.runID = t.runID;
                        al.AlarmID = _alarmID;
                        if (al.ContractorID != new Guid("00000000-0000-0000-0000-000000000000") && al.DriverID != new Guid("00000000-0000-0000-0000-000000000000") &&
                            al.FleetVehicleId != new Guid("00000000-0000-0000-0000-000000000000"))
                        {
                            sql.logViolation(al);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                if (AlertName != "LateOnPatrol" && AlertName != "EarlyOutOfService")
                {
                    hasAlarm = false;
                    _alarmID = new Guid("00000000-0000-0000-0000-000000000000");
                    alarmMinutes = 0;
                    alarmStart = Convert.ToDateTime("01/01/2001 00:00:00");
                }

                //check for possible violation-type Alarms

            }
        }
    }

    public class AlarmLog
    {
        public int ViolationTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Guid ContractorID { get; set; }
        public DateTime DateTimeOfViolation { get; set; }
        public Guid BeatID { get; set; }
        public Guid DriverID { get; set; }
        public Guid FleetVehicleId { get; set; }
        public string CallSign { get; set; }
        public string AlarmName { get; set; }
        public DateTime AlertTime { get; set; }
        public int LengthOfViolation { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public Guid runID { get; set; }
        public Guid AlarmID { get; set; }
    }
}