using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.TowTruck
{
    public class TruckStatus
    {
        public List<StatusTimer> statusTimers = new List<StatusTimer>();
        public Guid statusID = new Guid();
        public string currentStatus = "NA";
        public DateTime statusStarted;

        public TruckStatus()
        {
            StatusTimer logOn = new StatusTimer("LogOn");
            StatusTimer rollOut = new StatusTimer("RollOut");
            StatusTimer onPatrol = new StatusTimer("OnPatrol");
            StatusTimer rollIn = new StatusTimer("RollIn");
            StatusTimer onIncident = new StatusTimer("OnIncident");
            StatusTimer onTow = new StatusTimer("OnTow");
            StatusTimer logOff = new StatusTimer("LogOff");
            StatusTimer onBreak = new StatusTimer("OnBreak");
            StatusTimer onLunch = new StatusTimer("OnLunch");
            StatusTimer enRoute = new StatusTimer("EnRoute");
            StatusTimer forcedBreak = new StatusTimer("ForcedBreak");
            StatusTimer busy = new StatusTimer("Busy");
            statusTimers.Add(logOn);
            statusTimers.Add(rollOut);
            statusTimers.Add(onPatrol);
            statusTimers.Add(rollIn);
            statusTimers.Add(onIncident);
            statusTimers.Add(onTow);
            statusTimers.Add(logOff);
            statusTimers.Add(onBreak);
            statusTimers.Add(onLunch);
            statusTimers.Add(enRoute);
            statusTimers.Add(forcedBreak);
            statusTimers.Add(busy);
            statusStarted = Convert.ToDateTime("01/01/2001 00:00:00");
        }

        private string lookupStatusName(string statusName)
        {
            switch (statusName)
            {
                case "ROLL OUT":
                    return "RollOut";
                case "ENROUTE":
                    return "EnRoute";
                case "ON PATROL":
                    return "OnPatrol";
                case "ON BREAK":
                    return "OnBreak";
                case "ON LUNCH":
                    return "OnLunch";
                case "ON INCIDENT":
                    return "OnIncident";
                case "ON TOW":
                    return "OnTow";
                case "ROLL IN":
                    return "RollIn";
                case "LogOn": //sent from service, not from truck
                    return "LogOn";
                case "LogOff": //setn from service, not from truck
                    return "LogOff";
                case "Driver Logged On":
                    return "LogOn";
                case "Waiting for Driver Login":
                    return string.Empty; //don't start a timer for this
                case "FORCED BREAK":
                    return "ForcedBreak";
                case "BUSY":
                    return "Busy";
                case "NA":
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        public void startStatus(string _statusName, string truckNumber, string driverName, string contractorCompany, string Beat, double lat, double lon, Guid runID, 
            string location, double speed, int heading, Guid ScheduleID)
        {

            string statusMod = lookupStatusName(_statusName);
            statusStarted = DateTime.Now;
            if(!string.IsNullOrEmpty(statusMod))
            {

                if (statusMod != currentStatus) //we've got a status shift somewhere, shut down the old timer and start the new one
                {
                    stopStatus(currentStatus, truckNumber, driverName, contractorCompany, Beat, lat, lon, runID, location, speed, heading, ScheduleID);
                }

                StatusTimer st = statusTimers.Find(delegate(StatusTimer find) {
                    return find.statusName == statusMod;
                });

                if (st != null)
                {
                    statusID = st.StartTimer(statusMod, truckNumber, driverName, contractorCompany, Beat, lat, lon, runID, location, speed, heading, ScheduleID);
                }
                currentStatus = statusMod;

                if (statusMod == "LogOn")
                {
                    stopStatus(statusMod, truckNumber, driverName, contractorCompany, Beat, lat, lon, runID, location, speed, heading, ScheduleID);
                    currentStatus = "LOGGED ON";
                }

                if (statusMod == "LogOff")
                {
                    stopStatus(statusMod, truckNumber, driverName, contractorCompany, Beat, lat, lon, runID, location,speed,heading, ScheduleID);
                    currentStatus = "Waiting for Driver Login";
                }
                
            }
        }

        public void stopStatus(string _statusName, string truckNumber, string driverName, string contractorCompany, string Beat, double lat, double lon, Guid runID, 
            string location, double speed, int heading, Guid ScheduleID)
        {
            StatusTimer st = statusTimers.Find(delegate(StatusTimer find) {
                return find.statusName == _statusName;
            });

            if (st != null)
            {
                if(st.isRunning() == true)
                {
                    st.StopTimer(statusID, _statusName, truckNumber, driverName, contractorCompany, Beat, lat, lon, runID, location, speed, heading, ScheduleID);
                }
            }
        }
    }
}