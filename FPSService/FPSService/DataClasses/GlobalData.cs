using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FPSService.TowTruck;
using System.Collections;

namespace FPSService.DataClasses
{
    public class ParamData
    {
        public string ParamName { get; set; }
        public string ParamVal { get; set; }
    }

    public static class GlobalData
    {
        public static List<MiscData.callSign> CallSigns = new List<MiscData.callSign>();
        public static List<TowTruck.TowTruck> currentTrucks = new List<TowTruck.TowTruck>();
        public static List<TowTruck.TowTruckExtended> currentTrucksExtended = new List<TowTruck.TowTruckExtended>();
        public static int CadSeqNum = 0;
        static Logging.EventLogger logger = new Logging.EventLogger();
        //Misc data, mostly static data in the database, loads at service startup
        //public static List<MiscData.Code1098> Code1098s = new List<MiscData.Code1098>();
        //public static List<MiscData.Freeway> Freeways = new List<MiscData.Freeway>();
        public static List<MiscData.IncidentType> IncidentTypes = new List<MiscData.IncidentType>();
        public static List<MiscData.LocationAbbreviation> LocationAbbreviations = new List<MiscData.LocationAbbreviation>();
        public static List<MiscData.Transportation> Transportations = new List<MiscData.Transportation>();
        //public static List<MiscData.LocationCoding> LocationCodes = new List<MiscData.LocationCoding>();
        //public static List<MiscData.ServiceType> ServiceTypes = new List<MiscData.ServiceType>();
        //public static List<MiscData.TowLocation> TowLocations = new List<MiscData.TowLocation>();
        //public static List<MiscData.TrafficSpeed> TrafficSpeeds = new List<MiscData.TrafficSpeed>();
        public static List<MiscData.VehiclePosition> VehiclePositions = new List<MiscData.VehiclePosition>();
        public static List<MiscData.VehicleType> VehicleTypes = new List<MiscData.VehicleType>();
        public static List<MiscData.Contractors> Contractors = new List<MiscData.Contractors>();
        public static List<MTCIncident> Incidents = new List<MTCIncident>();
        public static List<MiscData.HighwaysBeats> highwaysBeats = new List<MiscData.HighwaysBeats>();
        /* OLD OCTA Assist Information */
        //public static List<MiscData.Incident> Incidents = new List<MiscData.Incident>();
        //public static List<MiscData.Assist> Assists = new List<MiscData.Assist>();
        public static List<MiscData.DropZone> DropZones = new List<MiscData.DropZone>();
        public static List<TruckMessage> theseMessages = new List<TruckMessage>();
        public static List<string> cadReceived = new List<string>();
        public static List<string> cadSent = new List<string>();
        public static int RollOutLeeway;
        public static int StationaryLeeway;
        public static int LogOnLeeway;
        public static int OnPatrollLeeway;
        public static int RollInLeeway;
        public static int LogOffLeeway;
        public static int OffBeatLeeway;
        public static int ExtendedLeeway;
        public static int SpeedingLeeway;
        //public static int NoMotionLeeway;
        public static int GPSIssueLeeway;
        public static int ForceOff;
        public static List<MiscData.BeatSchedule> theseSchedules = new List<MiscData.BeatSchedule>();
        public static List<MiscData.CoveredBeat> CoveredBeats = new List<MiscData.CoveredBeat>();
        public static List<SurveyTruck> surveyTrucks = new List<SurveyTruck>();
        public static List<BeatData.BeatFreeway> beatsFreeways = new List<BeatData.BeatFreeway>();
        public static List<WAZE.WAZEClass> wazeFeed = new List<WAZE.WAZEClass>();

        #region " Truck Messages (Not Assists) "

        public static void AddTruckMessage(TruckMessage thisMessage)
        {
            SQL.SQLCode mySQL = new SQL.SQLCode();
            TruckMessage foundMessage = theseMessages.Find(delegate(TruckMessage myMessage) { return myMessage.MessageID == thisMessage.MessageID; });
            if (foundMessage == null)
            {
                theseMessages.Add(thisMessage);
                mySQL.LogTruckMessage(thisMessage);
            }
            else
            {
                foundMessage = thisMessage;
                mySQL.LogTruckMessage(thisMessage);
            }
        }

        public static List<TruckMessage> GetMessagesByTruck(string TruckIP)
        {
            List<TruckMessage> myMessages = new List<TruckMessage>();
            for (int i = 0; i < theseMessages.Count; i++)
            {
                if (theseMessages[i].TruckIP == TruckIP && theseMessages[i].Acked != true)
                {
                    myMessages.Add(theseMessages[i]);
                }
            }
            return myMessages;
        }

        public static void AckTruckMessage(Guid MessageID)
        {
            TruckMessage foundMessage = theseMessages.Find(delegate(TruckMessage myMessage) { return myMessage.MessageID == MessageID; });
            foundMessage.Acked = true;
            foundMessage.AckedTime = DateTime.Now;
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.AckTruckMessage(foundMessage);
        }

        public static void AckTruckMessageWithResponse(Guid MessageID, string Response)
        {
            TruckMessage foundMessage = theseMessages.Find(delegate(TruckMessage myMessage) { return myMessage.MessageID == MessageID; });
            foundMessage.Acked = true;
            foundMessage.AckedTime = DateTime.Now;
            foundMessage.MessageResponse = Response;
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.AckTruckMessage(foundMessage);
        }

        #endregion

        #region " Covered Beats "

        public static void RemoveCover(string BeatName)
        {
            MiscData.CoveredBeat foundBeat = CoveredBeats.Find(delegate(MiscData.CoveredBeat myBeat) { return myBeat.BeatNumber == BeatName; });
            if (foundBeat != null)
            {
                CoveredBeats.Remove(foundBeat);
            }
        }

        public static void RemoveCoverByTruck(string TruckNumber)
        {
            MiscData.CoveredBeat foundBeat = CoveredBeats.Find(delegate(MiscData.CoveredBeat myBeat) { return myBeat.TruckNumber == TruckNumber; });
            if (foundBeat != null)
            {
                CoveredBeats.Remove(foundBeat);
            }
        }

        public static int GetCoverCount(string BeatName)
        {
            int count = 0;
            for (int i = 0; i < CoveredBeats.Count; i++)
            {
                if (CoveredBeats[i].BeatNumber == BeatName)
                {
                    count += 1;
                }
            }
            return count;
        }

        public static void AddCover(string TruckNumber, string BeatName)
        {
            MiscData.CoveredBeat foundBeat = CoveredBeats.Find(delegate(MiscData.CoveredBeat thisBeat) { return thisBeat.BeatNumber == BeatName; });
            if (foundBeat == null)
            {
                MiscData.CoveredBeat myBeat = new MiscData.CoveredBeat();
                myBeat.TruckNumber = TruckNumber;
                myBeat.BeatNumber = BeatName;
                CoveredBeats.Add(myBeat);
            }
        }

        #endregion

        #region " Beats Freeways "

        public static void UpdateBeatsFreeways(BeatData.BeatFreeway bf)
        {
            BeatData.BeatFreeway b = beatsFreeways.Find(delegate(BeatData.BeatFreeway find) { return find.BeatID == bf.BeatID; });
            if (b == null)
            {
                beatsFreeways.Add(bf);
            }
            else
            {
                for (int i = beatsFreeways.Count - 1; i >= 0; i--)
                {
                    if (beatsFreeways[i].BeatID == bf.BeatID)
                    {
                        beatsFreeways.RemoveAt(i);
                        beatsFreeways.Add(bf);
                    }
                }
            }
        }

        #endregion

        #region " find contractors and driver logon counts "

        public static Guid GetContractorID(string ContractorName)
        {
            Guid ContractorID = new Guid("00000000-0000-0000-0000-000000000000");
            foreach(MiscData.Contractors contractor in Contractors)
            {
                if (contractor.ContractCompanyName == ContractorName)
                {
                    ContractorID = contractor.ContractorID;
                }
            }
            return ContractorID;
        }

        public static int GetDriverLogonCount(string FSPID, string TruckNumber)
        {
            int iCount = 0;
            foreach (TowTruck.TowTruck thisTruck in currentTrucks)
            {
                if (thisTruck.Driver.FSPID == FSPID && thisTruck.TruckNumber != TruckNumber)
                {
                    iCount += 1;
                }
            }
            return iCount;
        }

        #endregion

        #region " Find, add, remove TowTrucks "

        public static void AddTowTruck(TowTruck.TowTruck thisTruck)
        {
            UDP.SendMessage msgAck = new UDP.SendMessage();
            try
            {
                TowTruck.TowTruck foundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck myTruck)
                {
                    return myTruck.Identifier == thisTruck.Identifier;
                });
                lock (currentTrucks)
                {
                    if (foundTruck == null)
                    {
                        currentTrucks.Add(thisTruck);
                    }
                }
                //ack moved to TowTruck object's update functions
                //msgAck.SendMyMessage("MsgID: " + thisTruck.GPSPosition.MessageID.ToString() + " Received");
            }
            catch (Exception ex)
            {
                string msg = "IP Address: " + thisTruck.Identifier + Environment.NewLine;
                msg += "VehicleNumber: " + thisTruck.Identifier + Environment.NewLine;
                msg += "Contractor: " + thisTruck.Extended.ContractorName + Environment.NewLine;
                msg += "VehicleStatus: " + thisTruck.Status.VehicleStatus + Environment.NewLine;
                msg += "Direction: " + thisTruck.GPSPosition.Head + Environment.NewLine;
                msg += "LastUpdate: " + thisTruck.GPSPosition.Time.ToString() + Environment.NewLine;
                msg += "BeatSegmentID: " + thisTruck.GPSPosition.BeatSegmentID + Environment.NewLine;
                msg += "Speed: " + thisTruck.GPSPosition.Speed + Environment.NewLine;
                msg += "Alarms: " + thisTruck.Status.SpeedingAlarm + Environment.NewLine;
                msg += "Lat: " + thisTruck.GPSPosition.Lat.ToString() + Environment.NewLine;
                msg += "Lon: " + thisTruck.GPSPosition.Lon.ToString() + Environment.NewLine;
                logger.LogEvent(DateTime.Now.ToString() + " Error adding/updating tow truck" + Environment.NewLine + msg + Environment.NewLine
                    + ex.ToString() + Environment.NewLine, true);
                msgAck.SendMyMessage("MsgID: " + thisTruck.GPSPosition.Id.ToString() + " Failed To Process");
            }
        }

        public static TowTruck.TowTruck FindTowTruck(string _ipaddress)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Identifier == _ipaddress; });
            return thisFoundTruck;
        }

        public static TowTruck.TowTruck FindTowTruckByVehicleNumber(string _vehicleNumber)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.TruckNumber == _vehicleNumber; });
            if (thisFoundTruck != null)
            {
                return thisFoundTruck;
            }
            else
            {
                return null;
            }
        }

        public static TowTruck.TowTruck FindTowTruckByTruckID(Guid TruckID)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Extended.FleetVehicleID == TruckID; });
            return thisFoundTruck;
        }

        public static string FindTowTruckStatusByID(Guid TruckID)
        {
            string CurrentStatus = "UNKNOWN";
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t){return t.Extended.FleetVehicleID == TruckID;});
            if (thisFoundTruck != null)
            {
                CurrentStatus = thisFoundTruck.Status.VehicleStatus;
            }
            return CurrentStatus;
        }

        public static void RemoveTowTruck(string _ipaddress)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Identifier == _ipaddress; });
            if (thisFoundTruck != null)
            {
                currentTrucks.Remove(thisFoundTruck);
            }
        }

        public static void RemoveTowTruckByTruckID(string TruckNumber)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.TruckNumber == TruckNumber; });
            if (thisFoundTruck != null)
            { currentTrucks.Remove(thisFoundTruck); }
        }

        public static void UpdateTowTruck(string _ipdress, TowTruck.TowTruck thisTruck)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Identifier == _ipdress; });
            thisFoundTruck = thisTruck;
        }

        public static string FindContractorByTruckNumber(string TruckNumber)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.TruckNumber == TruckNumber; });
            if (thisFoundTruck != null)
            {
                return thisFoundTruck.Extended.ContractorName;
            }
            else
            {
                return "Truck Not Found";
            }
        }

        #endregion

        #region " assign and remove trucks to/from incidents "
        /*
        public static void AssignTruckToAssist(Guid AssistID, TowTruck.TowTruck thisTruck)
        {
            MiscData.Assist thisFoundAssist = Assists.Find(delegate(MiscData.Assist a) { return a.AssistID == AssistID; });
            if (thisFoundAssist != null)
            {
                thisFoundAssist.FleetVehicleID = thisTruck.Extended.FleetVehicleID;
            }
        }

        public static void RemoveTruckFromIncident(Guid AssistID)
        {
            MiscData.Assist thisFoundAssist = Assists.Find(delegate(MiscData.Assist a) { return a.AssistID == AssistID; });
            if (thisFoundAssist != null)
            {
                thisFoundAssist.FleetVehicleID = new Guid("00000000-0000-0000-0000-000000000000");
            }
        }
        */


        #endregion

        #region " Locate data by ID "
        /* DEPRECATED: OCTA used this data MTC does not
        public static string FindFreewayNameByID(int FreewayID)
        {
            MiscData.Freeway thisFoundFreeway = Freeways.Find(delegate(MiscData.Freeway f) { return f.FreewayID == FreewayID; });
            if (thisFoundFreeway != null)
            {
                return thisFoundFreeway.FreewayName;
            }
            else
            {
                return "Unknown";
            }
        }

        public static string FindLocationNameByID(Guid _LocationID)
        {
            MiscData.LocationCoding thisFoundLocation = LocationCodes.Find(delegate(MiscData.LocationCoding l) { return l.LocationID == _LocationID; });
            if (thisFoundLocation != null)
            {
                return thisFoundLocation.Location;
            }
            else
            {
                return "Unknown";
            }
        }

        public static string FindVehiclePositionNameByID(Guid _VehiclePositionID)
        {
            MiscData.VehiclePosition thisFoundPosition = VehiclePositions.Find(delegate(MiscData.VehiclePosition vp) { return vp.VehiclePositionID == _VehiclePositionID; });
            if (thisFoundPosition != null)
            {
                return thisFoundPosition.VehiclePositionName;
            }
            else
            {
                return "Unknown";
            }
        }

        public static string FindIncidentTypeNameByID(Guid _IncidentTypeID)
        {
            MiscData.IncidentType thisFoundType = IncidentTypes.Find(delegate(MiscData.IncidentType it) { return it.IncidentTypeID == _IncidentTypeID; });
            if (thisFoundType != null)
            { return thisFoundType.IncidentTypeName; }
            else
            { return "Unknown"; }
        }*/

        /* OLD OCTA INCIDENT CODE
        public static string FindIncidentNumberByID(Guid _IncidentID)
        {
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentID; });
            if(thisFoundIncident != null)
            {
                return thisFoundIncident.IncidentNumber;
            }
            else
            {return "Unknown";}
        }

        public static string FindBeatNumberByID(Guid _IncidentID)
        {
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentID; });
            if (thisFoundIncident != null)
            {
                return thisFoundIncident.BeatNumber;
            }
            else
            { return "Unknown"; }
        }
        
        public static string FindTrafficSpeedNameByID(Guid _TrafficSpeedID)
        {
            MiscData.TrafficSpeed thisFoundSpeed = TrafficSpeeds.Find(delegate(MiscData.TrafficSpeed ts) { return ts.TrafficSpeedID == _TrafficSpeedID; });
            if (thisFoundSpeed != null)
            { return thisFoundSpeed.TrafficSpeedName; }
            else
            { return "Unknown"; }
        }

        public static string FindServiceTypeNameByID(Guid _ServiceTypeID)
        {
            MiscData.ServiceType thisFoundType = ServiceTypes.Find(delegate(MiscData.ServiceType st) { return st.ServiceTypeID == _ServiceTypeID; });
            if (thisFoundType != null)
            { return thisFoundType.ServiceTypeName; }
            else
            { return "Unknown"; }
        }

        public static string FindVehicleTypeNameByID(Guid _VehicleTypeID)
        {
            MiscData.VehicleType thisFoundType = VehicleTypes.Find(delegate(MiscData.VehicleType vt) { return vt.VehicleTypeID == _VehicleTypeID; });
            if (thisFoundType != null)
            { return thisFoundType.VehicleTypeName; }
            else
            { return "Unknown"; }
        }

        public static string FindTowLocationNameByID(Guid _TowLocationID)
        {
            MiscData.TowLocation thisFoundLocation = TowLocations.Find(delegate(MiscData.TowLocation tl) { return tl.TowLocationID == _TowLocationID; });
            if (thisFoundLocation != null)
            { return thisFoundLocation.TowLocationName; }
            else
            { return "Unknown"; }
        }
        */
        public static string FindContractorNameByID(Guid _ContractorID)
        {
            MiscData.Contractors thisFoundContractor = Contractors.Find(delegate(MiscData.Contractors c) { return c.ContractorID == _ContractorID; });
            if (thisFoundContractor != null)
            { return thisFoundContractor.ContractCompanyName; }
            else
            { return "Unknown"; }
        }

        public static string FindTruckNumberByID(Guid _FleetVehicleID)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Extended.FleetVehicleID == _FleetVehicleID; });
            if (thisFoundTruck != null)
            { return thisFoundTruck.TruckNumber; }
            else
            { return "Unknown"; }
        }

        #endregion

        #region " Force Driver Logoff "

        public static void ForceDriverLogoff(Guid _DriverID)
        {
            TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Driver.DriverID == _DriverID; });
            if (thisFoundTruck != null)
            {
                thisFoundTruck.Status.VehicleStatus = "Waiting for Driver Login";
                thisFoundTruck.Driver.DriverID = new Guid("00000000-0000-0000-0000-000000000000");
                thisFoundTruck.Driver.FSPID = "";
                thisFoundTruck.Driver.FirstName = "No Driver";
                thisFoundTruck.Driver.LastName = "No Driver";
                thisFoundTruck.Driver.TowTruckCompany = "No Driver";
                thisFoundTruck.Driver.callSign = "NA";
                thisFoundTruck.Status.SpeedingTime = Convert.ToDateTime("01/01/2001 00:00:00");
                thisFoundTruck.Status.OutOfBoundsTime = Convert.ToDateTime("01/01/2001 00:00:00");
                thisFoundTruck.Status.SpeedingValue = "0.0";
                thisFoundTruck.Status.OutOfBoundsMessage = "Not logged on";
                thisFoundTruck.Status.OutOfBoundsAlarm = false;
                thisFoundTruck.Status.SpeedingAlarm = false;
                thisFoundTruck.Status.StatusStarted = DateTime.Now;
                //thisFoundTruck.thisSchedule = null;
                thisFoundTruck.runID = new Guid("00000000-0000-0000-0000-000000000000");
                //reset status and alarms for truck
                thisFoundTruck.tts = null;
                thisFoundTruck.tta = null;
                thisFoundTruck.tts = new TowTruck.TruckStatus();
                thisFoundTruck.tta = new TowTruck.TowTruckAlarms();
                thisFoundTruck.wentOnPatrol = false;
                thisFoundTruck.rolledIn = false;
                thisFoundTruck.Driver.schedule = null;
                thisFoundTruck.beatNumber = "NOBEAT";
                thisFoundTruck.Extended.ContractorName = "No Driver";
                thisFoundTruck.Extended.ContractorID = new Guid("00000000-0000-0000-0000-000000000000");
                SQL.SQLCode mySQL = new SQL.SQLCode();
                mySQL.closeOutTruck(thisFoundTruck.runID);
            }
        }

        #endregion

        #region " Beat Schedules "

        public static MiscData.BeatSchedule findScheduleTimes(string Contractor, string beatNumber, string scheduleName)
        {
            MiscData.BeatSchedule bs = new MiscData.BeatSchedule();
            var schList = from s in theseSchedules
                          where s.Contractor == Contractor && s.BeatNumber == beatNumber && s.ScheduleName == scheduleName
                          select s;
            if (schList.Count() > 0)
            {
                foreach (MiscData.BeatSchedule b in schList)
                {
                    bs = b;
                }
            }
            else
            {
                var fbList = from s in theseSchedules
                             where s.BeatNumber == beatNumber && s.ScheduleName == scheduleName
                             select s;
                foreach (MiscData.BeatSchedule b in fbList)
                {
                    bs = b;
                    bs.scheduleID = new Guid("00000000-0000-0000-0000-000000000000");
                }
            }
            return bs;
        }

        /* DEPRECATED: THIS IS BASED ON OCTA REQUIREMENTS
        public static MiscData.BeatSchedule FindAppropriateSchedule(Guid BeatID)
        {
            string dtToday = DateTime.Now.DayOfWeek.ToString();
            MiscData.BeatSchedule foundSchedule;
            if (dtToday == "Saturday" || dtToday == "Sunday")
            {
                foundSchedule = theseSchedules.Find(delegate(MiscData.BeatSchedule mySchedule)
                {
                    return mySchedule.BeatID == BeatID && mySchedule.Logon <= DateTime.Now && mySchedule.LogOff >= DateTime.Now && mySchedule.Weekday == false;
                });
            }
            else
            {
                foundSchedule = theseSchedules.Find(delegate(MiscData.BeatSchedule mySchedule)
                {
                    return mySchedule.BeatID == BeatID && mySchedule.Logon <= DateTime.Now && mySchedule.LogOff >= DateTime.Now && mySchedule.Weekday == true;
                });
            }
            return foundSchedule;
        }
        */
        #endregion

        #region  " MTC Incidents "

        public static MTCIncident findIncidentByID(Guid _incidentID)
        {
            MTCIncident found = Incidents.Find(delegate(MTCIncident find) { return find.IncidentID == _incidentID; });
            if (found == null)
            {
                return null;
            }
            else
            {
                return found;
            }
        }

        public static void addIncident(MTCIncident _data)
        {
            MTCIncident found = Incidents.Find(i => i.IncidentID == _data.IncidentID);
            if (found == null)
            {
                Incidents.Add(_data);
            }
            else
            {
                found.assistList = _data.assistList;
                found.DatePosted = _data.DatePosted;
                found.fromTruck = _data.fromTruck;
                found.preAssist = _data.preAssist;
                found.UserPosted = _data.UserPosted;
            }
        }

        public static void AddPreAssist(Guid _incidentID, MTCPreAssistData _data)
        {
            try
            {
                MTCIncident found = Incidents.Find(i => i.IncidentID == _incidentID);
                if (found == null)
                {
                    throw new Exception("BAD INCIDENT");
                }
                else
                {
                    found.preAssist = _data;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("BAD INCIDENT");
            }
        }

        public static void AddAssist(Guid _incidentID, MTCAssist _data)
        {
            try
            {
                MTCIncident found = Incidents.Find(i => i.IncidentID == _incidentID);
                if (found == null)
                {
                    throw new Exception("BAD INCIDENT");
                }
                else
                {
                    found.assistList.Add(_data);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("BAD ASSIST");
            }
        }

        #endregion

        #region " OCTA Add, remove, return incidents - DEPRECATED AND COMMENTED OUT "

        /*
        public static void UpdateIncidentBeat(Guid _IncidentiD, Guid _FleetVehicleID)
        {
            string BeatNumber = "";
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentiD; });
            if (thisFoundIncident != null)
            {
                TowTruck.TowTruck thisFoundTruck = currentTrucks.Find(delegate(TowTruck.TowTruck t) { return t.Extended.FleetVehicleID == _FleetVehicleID; });
                if (thisFoundTruck != null)
                {
                    BeatNumber = thisFoundTruck.assignedBeat.BeatNumber;
                }
                thisFoundIncident.BeatNumber = BeatNumber;
            }
        }

        public static void UpdateIncident(MiscData.Incident thisIncident)
        {
            try
            {
                MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == thisIncident.IncidentID; });
                if (thisFoundIncident != null)
                {
                    var _incident = Incidents.Find(i => i.IncidentID == thisIncident.IncidentID);
                    _incident.Location = thisIncident.Location;
                    _incident.FreewayID = thisIncident.FreewayID;
                    _incident.LocationID = thisIncident.LocationID;
                    _incident.BeatSegmentID = thisIncident.BeatSegmentID;
                    //_incident.TimeStamp = thisIncident.TimeStamp;
                    _incident.CreatedBy = thisIncident.CreatedBy;
                    _incident.Description = thisIncident.Description;
                    //_incident.IncidentNumber = thisIncident.IncidentNumber;
                    _incident.CrossStreet1 = thisIncident.CrossStreet1;
                    _incident.CrossStreet2 = thisIncident.CrossStreet2;
                    _incident.Direction = thisIncident.Direction;
                    _incident.BeatNumber = thisIncident.BeatNumber;
                }
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now.ToString() + Environment.NewLine;
                msg += "Error Logging Incident" + Environment.NewLine;
                msg += Environment.NewLine;
                msg += "Incident ID: " + thisIncident.IncidentID.ToString() + Environment.NewLine;
                msg += "FreeWay ID: " + thisIncident.FreewayID.ToString() + Environment.NewLine;
                msg += "Location: " + thisIncident.Location + Environment.NewLine;
                msg += "Location ID: " + thisIncident.LocationID.ToString() + Environment.NewLine;
                msg += "Beat Segment ID: " + thisIncident.BeatSegmentID.ToString() + Environment.NewLine;
                msg += "Time Stamp: " + thisIncident.TimeStamp.ToString() + Environment.NewLine;
                msg += "Created By: " + thisIncident.CreatedBy.ToString() + Environment.NewLine;
                msg += "Description: " + thisIncident.Description + Environment.NewLine;
                msg += "Incident Number: " + thisIncident.IncidentNumber + Environment.NewLine;
                msg += "Cross Street 1: " + thisIncident.CrossStreet1 + Environment.NewLine;
                msg += "Cross Street 2: " + thisIncident.CrossStreet2 + Environment.NewLine;
                msg += "Direction: " + thisIncident.Direction + Environment.NewLine;
                msg += Environment.NewLine;
                msg += ex.ToString();
                logger.LogEvent(msg, true);
            }
        }

        public static void AddIncident(MiscData.Incident thisIncident)
        {
            try
            {
                MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == thisIncident.IncidentID; });
                if (thisFoundIncident == null)
                {
                    Incidents.Add(thisIncident);
                }
                else
                {

                }
                //Log to database, proc will update or insert based on existing data
                SQL.SQLCode mySQL = new SQL.SQLCode();

                mySQL.RunProc("LogIncident", GetIncidentArray(thisIncident));
            }
            catch (Exception ex)
            {
                string msg = DateTime.Now.ToString() + Environment.NewLine;
                msg += "Error Logging Incident" + Environment.NewLine;
                msg += Environment.NewLine;
                msg += "Incident ID: " + thisIncident.IncidentID.ToString() + Environment.NewLine;
                msg += "FreeWay ID: " + thisIncident.FreewayID.ToString() + Environment.NewLine;
                msg += "Location: " + thisIncident.Location + Environment.NewLine;
                msg += "Location ID: " + thisIncident.LocationID.ToString() + Environment.NewLine;
                msg += "Beat Segment ID: " + thisIncident.BeatSegmentID.ToString() + Environment.NewLine;
                msg += "Time Stamp: " + thisIncident.TimeStamp.ToString() + Environment.NewLine;
                msg += "Created By: " + thisIncident.CreatedBy.ToString() + Environment.NewLine;
                msg += "Description: " + thisIncident.Description + Environment.NewLine;
                msg += "Incident Number: " + thisIncident.IncidentNumber + Environment.NewLine;
                msg += "Cross Street 1: " + thisIncident.CrossStreet1 + Environment.NewLine;
                msg += "Cross Street 2: " + thisIncident.CrossStreet2 + Environment.NewLine;
                msg += "Direction: " + thisIncident.Direction + Environment.NewLine;
                msg += "Beat Number: " + thisIncident.BeatNumber + Environment.NewLine;
                msg += "DispatchNumber: " + thisIncident.IncidentNumber;
                msg += Environment.NewLine;
                msg += ex.ToString();
                logger.LogEvent(msg, true);

            }
        }

        private static ArrayList GetIncidentArray(MiscData.Incident thisIncident)
        {
            //to verify that we can translate everything into an arraylist correctly, we need to parse the elements in the object
            //into strings, filling default values if necessary
            ArrayList arrParams = new ArrayList();
            string IncidentID = thisIncident.IncidentID.ToString();
            string Direction;
            if (string.IsNullOrEmpty(thisIncident.Direction))
            {
                Direction = "NA";
            }
            else
            {
                Direction = thisIncident.Direction;
            }
            string FreewayID = thisIncident.FreewayID.ToString();
            string Location = "NA";
            if (!string.IsNullOrEmpty(thisIncident.Location))
            {
                Location = thisIncident.Location.ToString();
            }
            string LocationID = thisIncident.LocationID.ToString();
            string BeatSegmentID = thisIncident.BeatSegmentID.ToString();
            string TimeStamp = thisIncident.TimeStamp.ToString();
            string CrossStreet1 = "NA";
            if (!string.IsNullOrEmpty(thisIncident.CrossStreet1))
            { CrossStreet1 = thisIncident.CrossStreet1; }
            string CrossStreet2 = "NA";
            if (!string.IsNullOrEmpty(thisIncident.CrossStreet2))
            {
                CrossStreet2 = thisIncident.CrossStreet2;
            }
            string CreatedBy;
            if (!string.IsNullOrEmpty(thisIncident.CreatedBy))
            {
                CreatedBy = thisIncident.CreatedBy.ToString();
            }
            else
            {
                CreatedBy = "UNKNOWN USER";
            }
            
            string Description;
            if (string.IsNullOrEmpty(thisIncident.Description))
            {
                Description = "NA";
            }
            else
            {
                Description = thisIncident.Description;
            }
            string IncidentNumber;
            if (string.IsNullOrEmpty(thisIncident.IncidentNumber))
            {
                IncidentNumber = "NA";
            }
            else
            {
                IncidentNumber = thisIncident.IncidentNumber;
            }
            string BeatNumber;
            if (string.IsNullOrEmpty(thisIncident.BeatNumber))
            {
                BeatNumber = "NA";
            }
            else
            {
                BeatNumber = thisIncident.BeatNumber;
            }

            arrParams.Add("@Direction~" + Direction);
            arrParams.Add("@IncidentID~" + IncidentID);
            arrParams.Add("@ApproximateLocation~" + Direction);
            arrParams.Add("@FreewayID~" + FreewayID);
            arrParams.Add("@Location~" + Location);
            arrParams.Add("@LocationID~" + LocationID);
            arrParams.Add("@BeatSegmentID~" + BeatSegmentID);
            arrParams.Add("@TimeStamp~" + TimeStamp);
            arrParams.Add("@CreatedBy~" + CreatedBy);
            arrParams.Add("@Description~" + Description);
            arrParams.Add("@IncidentNumber~" + IncidentNumber);
            arrParams.Add("@CrossStreet1~" + CrossStreet1);
            arrParams.Add("@CrossStreet2~" + CrossStreet2);
            arrParams.Add("@BeatNumber~" + BeatNumber);
            return arrParams;
        }

        public static void ClearIndicent(Guid _IncidentID)
        {
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentID; });
            if (thisFoundIncident == null)
            {
                return;
            }
            else
            {
                Incidents.Remove(thisFoundIncident);
                for (int i = Assists.Count - 1; i >= 0; i--)
                {
                    if (Assists[i].IncidentID == _IncidentID)
                    {
                        Assists.Remove(Assists[i]);
                    }
                }
            }
        }

        public static string FindIncidentCreatorFromAssist(Guid _IncidentID)
        {
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentID; });
            if (thisFoundIncident == null)
            {
                //Guid newGuid = new Guid("00000000-0000-0000-0000-000000000000");
                //return newGuid;
                return "Not Found";
            }
            else
            {
                return thisFoundIncident.CreatedBy;
            }
        }

        public static MiscData.Incident FindIncidentByID(Guid _IncidentID)
        {
            MiscData.Incident thisFoundIncident = Incidents.Find(delegate(MiscData.Incident i) { return i.IncidentID == _IncidentID; });
            if (thisFoundIncident == null)
            {
                return null;
            }
            else
            {
                return thisFoundIncident;
            }
        }
        */
        #endregion

        #region " OCTA add, remove, assign assists - DEPRECATED AND COMMENTED OUT "

        /*

        public static List<MiscData.Assist> GetAssistsByIncident(Guid IncidentID)
        {
            List<MiscData.Assist> theseAssists = new List<MiscData.Assist>();
            foreach (MiscData.Assist thisAssist in Assists)
            {
                if (thisAssist.IncidentID == IncidentID)
                {
                    theseAssists.Add(thisAssist);
                }
            }
            return theseAssists;
        }

        public static void AckAssistRequest(Guid _assistid)
        {
            var _assist = Assists.Find(a => a.AssistID == _assistid);
            if (_assist != null)
            {
                _assist.Acked = true;
            }
        }

        public static void AddAssist(MiscData.Assist thisAssist)
        {
            if (thisAssist.IncidentID == null || thisAssist.IncidentID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                return; //assists must be assigned to incidents
            }
            MiscData.Assist thisFoundAssist = Assists.Find(delegate(MiscData.Assist a) { return a.AssistID == thisAssist.AssistID; });
            if (thisFoundAssist == null)
            {
                Assists.Add(thisAssist);
            }
            else
            {
                var _assist = Assists.Find(a => a.AssistID == thisAssist.AssistID);
                _assist.IncidentID = thisAssist.IncidentID;
                _assist.DriverID = thisAssist.DriverID;
                _assist.FleetVehicleID = thisAssist.FleetVehicleID;
                _assist.DispatchTime = thisAssist.DispatchTime;
                _assist.CustomerWaitTime = thisAssist.CustomerWaitTime;
                _assist.VehiclePositionID = thisAssist.VehiclePositionID;
                _assist.IncidentTypeID = thisAssist.IncidentTypeID;
                _assist.TrafficSpeedID = thisAssist.TrafficSpeedID;
                _assist.DropZone = thisAssist.DropZone;
                _assist.Make = thisAssist.Make;
                _assist.VehicleTypeID = thisAssist.VehicleTypeID;
                _assist.Color = thisAssist.Color;
                _assist.LicensePlate = thisAssist.LicensePlate;
                _assist.State = thisAssist.State;
                _assist.StartOD = thisAssist.StartOD;
                _assist.EndOD = thisAssist.EndOD;
                _assist.TowLocationID = thisAssist.TowLocationID;
                _assist.Tip = thisAssist.Tip;
                _assist.TipDetail = thisAssist.TipDetail;
                _assist.CustomerLastName = thisAssist.CustomerLastName;
                _assist.Comments = thisAssist.Comments;
                _assist.IsMDC = thisAssist.IsMDC;
                _assist.x1097 = thisAssist.x1097;
                _assist.x1098 = thisAssist.x1098;
                _assist.ContractorID = thisAssist.ContractorID;
                _assist.LogNumber = thisAssist.LogNumber;
                _assist.SelectedServices = thisAssist.SelectedServices;
                _assist.OnSiteTime = thisAssist.OnSiteTime;
                _assist.AssistComplete = thisAssist.AssistComplete;
                _assist.SurveyNum = thisAssist.SurveyNum;
                _assist.AssistNumber = thisAssist.AssistNumber;
                _assist.Lat = thisAssist.Lat;
                _assist.Lon = thisAssist.Lon;
            }
            ArrayList arrParams = new ArrayList();
            SQL.SQLCode mySQL = new SQL.SQLCode();
            mySQL.RunProc("LogAssist", GetAssistParams(thisAssist)); //first shot updates assist data
            ArrayList arrSTCleanse = new ArrayList();
            arrSTCleanse.Add("@AssistID~" + thisAssist.AssistID); //clear old service type data before adding new.  This makes sure we maintain most accurate data
            mySQL.RunProc("CleanseServiceTypes", arrSTCleanse);
            if (thisAssist.SelectedServices != null)
            {
                for (int i = 0; i < thisAssist.SelectedServices.Count(); i++)
                {
                    ArrayList arrSrvTypes = new ArrayList();
                    arrSrvTypes.Add("@AssistID~" + thisAssist.AssistID);
                    arrSrvTypes.Add("@ServiceTypeID~" + thisAssist.SelectedServices[i].ToString());
                    mySQL.RunProc("LogServiceTypes", arrSrvTypes);
                }
            }
        }

        private static ArrayList GetAssistParams(MiscData.Assist thisAssist)
        {
            ArrayList arrParams = new ArrayList();
            string AssistID = thisAssist.AssistID.ToString();
            string IncidentID = thisAssist.IncidentID.ToString();
            string DriverID;
            if (thisAssist.DriverID == null)
            {
                DriverID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                DriverID = thisAssist.DriverID.ToString();
            }
            string FleetVehicleID;
            if (thisAssist.FleetVehicleID == null)
            {
                FleetVehicleID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                FleetVehicleID = thisAssist.FleetVehicleID.ToString();
            }
            string DispatchTime;
            if (thisAssist.DispatchTime == null)
            {
                DispatchTime = DateTime.Now.ToString();
            }
            else
            {
                DispatchTime = thisAssist.DispatchTime.ToString();
            }
            string CustomerWaitTime = thisAssist.CustomerWaitTime.ToString();
            string VehiclePositionID;
            if (thisAssist.VehiclePositionID == null)
            {
                VehiclePositionID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                VehiclePositionID = thisAssist.VehiclePositionID.ToString();
            }
            string IncidentTypeID;
            if (thisAssist.IncidentTypeID == null)
            {
                IncidentTypeID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                IncidentTypeID = thisAssist.IncidentTypeID.ToString();
            }
            string TrafficSpeedID;
            if (thisAssist.TrafficSpeedID == null)
            {
                TrafficSpeedID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                TrafficSpeedID = thisAssist.TrafficSpeedID.ToString();
            }

            string DropZone;
            if (string.IsNullOrEmpty(thisAssist.DropZone))
            {
                DropZone = "NA";
            }
            else
            {
                DropZone = thisAssist.DropZone;
            }
            string Make;
            if (string.IsNullOrEmpty(thisAssist.Make))
            { Make = "NA"; }
            else
            { Make = thisAssist.Make; }
            string VehicleTypeID;
            if (thisAssist.VehicleTypeID == null)
            {
                VehicleTypeID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                VehicleTypeID = thisAssist.VehicleTypeID.ToString();
            }
            string Color;
            if (string.IsNullOrEmpty(thisAssist.Color))
            { Color = "NA"; }
            else
            { Color = thisAssist.Color; }
            string LicensePlate;
            if (string.IsNullOrEmpty(thisAssist.LicensePlate))
            { LicensePlate = "NA"; }
            else
            { LicensePlate = thisAssist.LicensePlate; }
            string State;
            if (string.IsNullOrEmpty(thisAssist.State))
            { State = "NA"; }
            else
            { State = thisAssist.State; }
            string TowLocationID;
            if (thisAssist.TowLocationID == null)
            {
                TowLocationID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                TowLocationID = thisAssist.TowLocationID.ToString();
            }
            string TipDetail;
            if (string.IsNullOrEmpty(thisAssist.TipDetail))
            { TipDetail = "NA"; }
            else
            { TipDetail = thisAssist.TipDetail; }
            string CustomerLastName;
            if (string.IsNullOrEmpty(thisAssist.CustomerLastName))
            { CustomerLastName = "NA"; }
            else
            { CustomerLastName = thisAssist.CustomerLastName; }
            string Comments;
            if (string.IsNullOrEmpty(thisAssist.Comments))
            { Comments = "NA"; }
            else
            { Comments = thisAssist.Comments; }
            string x1097;
            if (thisAssist.x1097 == null)
            {
                x1097 = DateTime.Now.ToString();
            }
            else
            {
                x1097 = thisAssist.x1097.ToString();
            }
            string x1098;
            if (thisAssist.x1098 == null || thisAssist.x1098 == DateTime.Parse("01/01/0001 00:00:00"))
            {
                x1098 = "01/01/2001 00:00:00";
            }
            else
            {
                x1098 = thisAssist.x1098.ToString();
            }
            string OnSiteTime;
            if (thisAssist.OnSiteTime == null || thisAssist.OnSiteTime == DateTime.Parse("01/01/0001 00:00:00"))
            {
                OnSiteTime = "01/01/2001 00:00:00";
            }
            else
            {
                OnSiteTime = thisAssist.OnSiteTime.ToString();
            }
            string ContractorID;
            if (thisAssist.ContractorID == null)
            {
                ContractorID = new Guid("00000000-0000-0000-0000-000000000000").ToString();
            }
            else
            {
                ContractorID = thisAssist.ContractorID.ToString();
            }
            string LogNumber;
            if (string.IsNullOrEmpty(thisAssist.LogNumber))
            { LogNumber = "NA"; }
            else
            { LogNumber = thisAssist.LogNumber; }

            string SurveyNum;
            if (string.IsNullOrEmpty(thisAssist.SurveyNum))
            { SurveyNum = "NA"; }
            else
            { SurveyNum = thisAssist.SurveyNum; }

            string AssistNumber;
            if (string.IsNullOrEmpty(thisAssist.AssistNumber))
            {
                AssistNumber = "NA";
            }
            else
            {
                AssistNumber = thisAssist.AssistNumber;
            }

            arrParams.Add("@AssistID~" + AssistID);
            arrParams.Add("@IncidentID~" + IncidentID);
            arrParams.Add("@DriverID~" + DriverID);
            arrParams.Add("@FleetVehicleID~" + FleetVehicleID);
            arrParams.Add("@DispatchTime~" + DispatchTime);
            arrParams.Add("@CustomerWaitTime~" + CustomerWaitTime);
            arrParams.Add("@VehiclePositionID~" + VehiclePositionID);
            arrParams.Add("@IncidentTypeID~" + IncidentTypeID);
            arrParams.Add("@TrafficSpeedID~" + TrafficSpeedID);
            //arrParams.Add("@ServiceTypeID~" + ServiceTypeID);
            arrParams.Add("@DropZone~" + DropZone);
            arrParams.Add("@Make~" + Make);
            arrParams.Add("@VehicleTypeID~" + VehicleTypeID);
            arrParams.Add("@Color~" + Color);
            arrParams.Add("@LicensePlate~" + LicensePlate);
            arrParams.Add("@State~" + State);
            arrParams.Add("@StartOD~" + thisAssist.StartOD);
            arrParams.Add("@EndOD~" + thisAssist.EndOD);
            arrParams.Add("@TowLocationID~" + TowLocationID);
            arrParams.Add("@Tip~" + thisAssist.Tip);
            arrParams.Add("@TipDetail~" + TipDetail);
            arrParams.Add("@CustomerLastName~" + CustomerLastName);
            arrParams.Add("@Comments~" + Comments);
            arrParams.Add("@IsMDC~" + thisAssist.IsMDC);
            arrParams.Add("@x1097~" + x1097);
            arrParams.Add("@x1098~" + x1098);
            arrParams.Add("@ContractorID~" + ContractorID);
            arrParams.Add("@LogNumber~" + LogNumber);
            arrParams.Add("@Lat~" + thisAssist.Lat.ToString());
            arrParams.Add("@Lon~" + thisAssist.Lon.ToString());
            arrParams.Add("@OnSiteTime~" + OnSiteTime);
            arrParams.Add("@SurveyNum~" + SurveyNum);
            arrParams.Add("@AssistNumber~" + AssistNumber);
            return arrParams;
        }

        public static void RemoveAssist(Guid _AssistID)
        {
            MiscData.Assist thisFoundAssist = Assists.Find(delegate(MiscData.Assist a) { return a.AssistID == _AssistID; });
            if (thisFoundAssist != null)
            {
                Assists.Remove(thisFoundAssist);
            }
        }

        public static MiscData.Assist FindAssistByID(Guid _AssistID)
        {
            MiscData.Assist thisFoundAssist = Assists.Find(delegate(MiscData.Assist a) { return a.AssistID == _AssistID; });
            return thisFoundAssist;
        }

        public static List<MiscData.Assist> GetAssistsByTruck(Guid _truckID)
        {
            List<MiscData.Assist> TruckAssists = new List<MiscData.Assist>();

            foreach (MiscData.Assist thisAssist in Assists)
            {
                if (thisAssist.FleetVehicleID == _truckID)
                {
                    TruckAssists.Add(thisAssist);
                }
            }

            return TruckAssists;
        }

         * */
        #endregion
    }
}