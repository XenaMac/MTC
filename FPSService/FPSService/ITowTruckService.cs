using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.SqlServer.Types;
using System.IO;

namespace FPSService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITowTruckService" in both code and config file together.
    #region  " Contract "
    [ServiceContract]
    public interface ITowTruckService
    {
        [OperationContract]
        void reloadBeats();

        [OperationContract]
        void reloadBeatSegments();

        [OperationContract]
        void reloadYards();

        [OperationContract]
        void reloadDrops();
        
        [OperationContract]
        List<TowTruckData> CurrentTrucks();

        [OperationContract]
        void UpdateVar(string varName, string varValue);

        [OperationContract]
        void SingleTruckDump(CopyTruck t);

        [OperationContract]
        void UnexcuseAlarm(string _vehicleNumber, string _beatNumber, string _alarm, string _driverName, string _comments = "NO COMMENT");

        [OperationContract]
        string findSeg(double lat, double lon);

        [OperationContract]
        string findBeat(double lat, double lon);

        //[OperationContract]
        //void AddTruckAssistRequest(string IPAddress, AssistReq thisReq, Guid IncidentID);

        //[OperationContract]
        //void ClearTruckAssistRequest(string IPAddress, Guid AssistRequestID);
        [OperationContract]
        void TruckDump(List<CopyTruck> trucks);

        [OperationContract]
        void ClearAlarm(string _vehicleNumber, string _alarm, Guid _alertID);

        [OperationContract]
        void ExcuseAlarm(string _vehicleNumber, string _alarm, string _excuser, Guid _alertID, string _comments = "NO COMMENT");

        /* DEPRECATED : OCTA DATA NOT IN USE IN MTA
        [OperationContract]
        string[] GetPreloadedData(string Type);
         * */

        [OperationContract]
        void addAssist(string UserName, string IPAddress, MTCPreAssistData data);

        [OperationContract]
        List<MTCIncidentScreenData> GetAllIncidents();

        /* OCTA Incidents - DEPRECATED AND COMMENTED OUT */
        //[OperationContract]
        //void AddIncident(IncidentIn thisIncidentIn);

        //[OperationContract]
        //List<IncidentScreenData> GetAllIncidents();

        //[OperationContract]
        //IncidentIn FindIncidentByID(Guid IncidentID);

        //[OperationContract]
        //void ClearIncident(Guid IncidentID);

        //[OperationContract]
        //void AddAssist(AssistReq thisReq);

        //[OperationContract]
        //List<AssistScreenData> GetAllAssists();

        [OperationContract]
        List<AssistTruck> GetAssistTrucks();

        [OperationContract]
        int GetUsedBreakTime(string DriverID, string Type);

        [OperationContract]
        List<ListDrivers> GetTruckDrivers();

        [OperationContract]
        void LogOffDriver(Guid DriverID);

        [OperationContract]
        List<AlarmStatus> GetAllAlarms();

        [OperationContract]
        List<AlarmStatus> AlarmByTruck(string IPAddress);

        [OperationContract]
        void SendMessage(TruckMessage thisMessage);

        //[OperationContract]
        //List<IncidentDisplay> getIncidentData();

        [OperationContract]
        void ResetAlarm(string _vehicleNumber, string _alarm);

        [OperationContract]
        List<beatData> GetBeatData();

        [OperationContract]
        List<beatData> GetBeatDataByBeat(string BeatNumber);

        [OperationContract]
        List<RunStatus> getTruckRunStatus(string truckNumber);

        [OperationContract]
        void rebootTruck(string IPAddress);

        [OperationContract]
        string getTruck(string truckNumber);

        #region  " Get Truck Messages "

        [OperationContract]
        List<TruckMessage> getMessagesBySender(string senderEmail);

        [OperationContract]
        List<TruckMessage> getAllMessages();

        [OperationContract]
        List<TruckMessage> getMessagesByDriverID(string driverID);

        [OperationContract]
        List<TruckMessage> getMessagesByCallSign(string CallSign);

        [OperationContract]
        List<TruckMessage> getMessagesByBeat(string beatNumber);

        #endregion
    }

    #endregion

    #region " Incidents and Assists - NEW VERSIONS "

    [DataContract]
    public class Incident {
        [DataMember]
        public Guid? incidentID { get; set; }
        [DataMember]
        public DateTime? incidentDatePosted { get; set; }
        [DataMember]
        public string userPosted { get; set; }
        [DataMember]
        public string callSign { get; set; }
        [DataMember]
        public bool fromTruck { get; set; }
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lon { get; set; }
        [DataMember]
        public bool canceled { get; set; }
        [DataMember]
        public string reasonCanceled { get; set; }
        [DataMember]
        public string beat { get; set; }
        [DataMember]
        public string truckNumber { get; set; }
        [DataMember]
        public Guid? logID { get; set; }
        [DataMember]
        public Guid? wazeID { get; set; }
        [DataMember]
        public Guid? truckStatusID { get; set; }
        [DataMember]
        public string FSPLocation { get; set; }
        [DataMember]
        public string dispatchLocation { get; set; }
        [DataMember]
        public string direction { get; set; }
        [DataMember]
        public string positionIncident { get; set; }
        [DataMember]
        public string laneNumber { get; set; }
        [DataMember]
        public string chpIncidentType { get; set; }
        [DataMember]
        public double? briefUpdateLat { get; set; }
        [DataMember]
        public double? briefUpdateLon { get; set; }
        [DataMember]
        public string freeway { get; set; }
        [DataMember]
        public bool briefUpdatePosted { get; set; }
        [DataMember]
        public DateTime? timeOfBriefUpdate { get; set; }
        [DataMember]
        public string CHPLogNumber { get; set; }
        [DataMember]
        public string incidentSurveyNumber { get; set; }
        [DataMember]
        public string driverLastName { get; set; }
        [DataMember]
        public string driverFirstName { get; set; }
        [DataMember]
        public Guid? driverID { get; set; }
        [DataMember]
        public Guid? runID { get; set; }
        [DataMember]
        public DateTime? timeOnIncident { get; set; }
        [DataMember]
        public DateTime? timeOffIncident { get; set; }
        [DataMember]
        public bool sentToTruck { get; set; }
        [DataMember]
        public bool acked { get; set; }
        [DataMember]
        public string comment { get; set; }
        [DataMember]
        public string crossStreet { get; set; }
        [DataMember]
        public bool requestSent { get; set; }
    }

    [DataContract]
    public class Assist {
        [DataMember]
        public Guid? assistID { get; set; }
        [DataMember]
        public Guid incidentID { get; set; }
        [DataMember]
        public DateTime? assistDatePosted { get; set; }
        [DataMember]
        public bool lastAssistInIncidentReport { get; set; }
        [DataMember]
        public string problemType { get; set; }
        [DataMember]
        public string problemDetail { get; set; }
        [DataMember]
        public string problemNote { get; set; }
        [DataMember]
        public string otherNote { get; set; }
        [DataMember]
        public string transportType { get; set; }
        [DataMember]
        public double? StartODO { get; set; }
        [DataMember]
        public double? EndODO { get; set; }
        [DataMember]
        public string dropSite { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string licensePlate { get; set; }
        [DataMember]
        public string vehicleType { get; set; }
        [DataMember]
        public string OTAuthorizationNumber { get; set; }
        [DataMember]
        public string detailNote { get; set; }
        [DataMember]
        public double? assistLat { get; set; }
        [DataMember]
        public double? assistLon { get; set; }
        [DataMember]
        public string dropSiteOther { get; set; }
        [DataMember]
        public string callSign { get; set; }
        [DataMember]
        public DateTime? timeOnAssist { get; set; }
        [DataMember]
        public DateTime? timeOffAssist { get; set; }
        [DataMember]
        public string actionTaken { get; set; }
        [DataMember]
        public string dropSiteBeat { get; set; }
        [DataMember]
        public string PTN { get; set; }
    }

    #endregion

    #region " MTC Assist and Incident Classes - DEPRECATED"
    //deprecated
    [DataContract]
    public class MTCPreAssistData
    {
        [DataMember]
        public string Direction { get; set; }
        [DataMember]
        public string Freeway { get; set; }
        [DataMember]
        public string Beat { get; set; }
        [DataMember]
        public string DispatchCode { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string CrossStreet { get; set; }
        [DataMember]
        public string FSPLocation { get; set; }
        [DataMember]
        public string LocationofInitialDispatch { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string LaneNumber { get; set; }
        [DataMember]
        public string CHPIncidentType { get; set; }
        [DataMember]
        public string CHPLogNumber { get; set; } //this is set from the II or IS packet from CAD.
        [DataMember]
        public string IncidentSurveyNumber { get; set; }
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }
        [DataMember]
        public string DriverLastName { get; set; }
        [DataMember]
        public string DriverFirstName { get; set; }
        [DataMember]
        public Guid DriverID { get; set; }
        [DataMember]
        public Guid RunID { get; set; }
    }

    [DataContract]
    public class MTCAssist
    {
        //ProblemType
        [DataMember]
        public string TrafficCollision { get; set; }
        [DataMember]
        public string Breakdown { get; set; }
        [DataMember]
        public string DebrisOnly { get; set; }
        [DataMember]
        public string Other { get; set; }
        [DataMember]
        public string ProblemNote { get; set; }

        //ActionTaken
        [DataMember]
        public List<string> ActionTaken { get; set; }
        [DataMember]
        public string OtherNote { get; set; }

        //Transport
        [DataMember]
        public List<string> TransportType { get; set; }
        [DataMember]
        public double PStartODO { get; set; }
        [DataMember]
        public double PEndODO { get; set; }
        [DataMember]
        public string DropSiteBeat { get; set; }
        [DataMember]
        public string DropSite { get; set; }
        [DataMember]
        public string DropSiteOther { get; set; }

        //Details
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string LicPlateNum { get; set; }
        [DataMember]
        public string VehicleType { get; set; }
        [DataMember]
        public string OTAuthNum { get; set; }
        [DataMember]
        public string DetailNote { get; set; }

        //PhysicalLocation
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }

        //CallSign
        [DataMember]
        public string CallSign { get; set; }
    }

    [DataContract]
    public class MTCIncident
    {
        [DataMember]
        public Guid IncidentID { get; set; }
        [DataMember]
        public string IPAddr { get; set; }
        [DataMember]
        public DateTime DatePosted { get; set; }
        [DataMember]
        public string UserPosted { get; set; }
        [DataMember]
        public string Beat { get; set; }
        [DataMember]
        public int fromTruck { get; set; }
        [DataMember]
        public MTCPreAssistData preAssist { get; set; }
        [DataMember]
        public List<MTCAssist> assistList { get; set; }
        [DataMember]
        public bool sentToTruck { get; set; }
        [DataMember]
        public bool Acked { get; set; }
        [DataMember]
        public bool Canceled { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public bool incidentComplete { get; set; }
    }

    [DataContract]
    public class MTCIncidentScreenData
    {
        [DataMember]
        public string Beat { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public string Driver { get; set; }
        [DataMember]
        public string DispatchSummaryMessage { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public string DispatchNumber { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string IsIncidentComplete { get; set; }
        [DataMember]
        public string isAcked { get; set; }
        [DataMember]
        public string IncidentID { get; set; }
        [DataMember]
        public string ContractorName { get; set; }
        [DataMember]
        public string IncidentType { get; set; }
        [DataMember]
        public string Location { get; set; }
    }

    #endregion

    #region " Classes "

    [DataContract]
    public class beatData
    {
        [DataMember]
        public string BeatNumber { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public int TruckCount { get; set; }
        [DataMember]
        public int BackupTruckCount { get; set; }
        [DataMember]
        public string ContractCompanyName { get; set; }
        [DataMember]
        public string ScheduleName { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
    }

    [DataContract]
    public class RunStatus
    {
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string StatusChange { get; set; }
        [DataMember]
        public string Alert { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public int Speed { get; set; }
        [DataMember]
        public string Heading { get; set; }
    }

    [DataContract]
    public class ListDrivers
    {
        [DataMember]
        public Guid TruckID { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public Guid DriverID { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string ContractorName { get; set; }
    }

    [DataContract]
    public class AssistTruck
    {
        [DataMember]
        public Guid TruckID { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public string ContractorName { get; set; }
        [DataMember]
        public Guid ContractorID { get; set; }
    }

    [DataContract]
    public class TowTruckData
    {
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public int Heading { get; set; }
        [DataMember]
        public double Speed { get; set; }
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }
        [DataMember]
        public string VehicleState { get; set; }
        [DataMember]
        public bool Alarms { get; set; }
        [DataMember]
        public bool SpeedingAlarm { get; set; }
        [DataMember]
        public string SpeedingValue { get; set; }
        [DataMember]
        public DateTime SpeedingTime { get; set; }
        [DataMember]
        public bool OutOfBoundsAlarm { get; set; }
        [DataMember]
        public string OutOfBoundsMessage { get; set; }
        [DataMember]
        public DateTime OutOfBoundsTime { get; set; }
        [DataMember]
        public string BeatNumber { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public DateTime LastMessage { get; set; }
        [DataMember]
        public string ContractorName { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public DateTime StatusStarted { get; set; }
        [DataMember]
        public string Cell { get; set; }
        [DataMember]
        public bool IsBackup { get; set; }
        [DataMember]
        public string VehicleType { get; set; }

    }

    [DataContract]
    public class AlarmStatus
    {
        [DataMember]
        public string BeatNumber { get; set; }
        [DataMember]
        public string VehicleNumber { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public bool HasAlarms { get; set; }
        [DataMember]
        public string ContractCompanyName { get; set; }
        [DataMember]
        public Guid SpeedingAlarmID { get; set; }
        [DataMember]
        public bool SpeedingAlarm { get; set; }
        [DataMember]
        public string SpeedingValue { get; set; }
        [DataMember]
        public DateTime SpeedingTime { get; set; }
        [DataMember]
        public int SpeedingDuration { get; set; }
        [DataMember]
        public Guid OutOfBoundsAlarmID { get; set; }
        [DataMember]
        public bool OutOfBoundsAlarm { get; set; }
        [DataMember]
        public string OutOfBoundsMessage { get; set; }
        [DataMember]
        public DateTime OutOfBoundsTime { get; set; }
        [DataMember]
        public DateTime OutOfBoundsStartTime { get; set; }
        [DataMember]
        public DateTime OutOfBoundsExcused { get; set; }
        [DataMember]
        public DateTime OutOfBoundsCleared { get; set; }
        [DataMember]
        public int OutOfBoundsDuration { get; set; }
        [DataMember]
        public string VehicleStatus { get; set; }
        [DataMember]
        public DateTime StatusStarted { get; set; }
        [DataMember]
        public Guid OnPatrolAlarmID { get; set; }
        [DataMember]
        public bool OnPatrolAlarm { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmTime { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmCleared { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmExcused { get; set; }
        [DataMember]
        public string OnPatrolAlarmComments { get; set; }
        [DataMember]
        public int OnPatrolDuration { get; set; }
        [DataMember]
        public Guid RollInAlarmID { get; set; }
        [DataMember]
        public bool RollInAlarm { get; set; }
        [DataMember]
        public DateTime RollInAlarmTime { get; set; }
        [DataMember]
        public DateTime RollInAlarmCleared { get; set; }
        [DataMember]
        public DateTime RollInAlarmExcused { get; set; }
        [DataMember]
        public string RollInAlarmComments { get; set; }
        [DataMember]
        public int RollInAlarmDuration { get; set; }
        [DataMember]
        public Guid IncidentAlarmID { get; set; }
        [DataMember]
        public bool IncidentAlarm { get; set; }
        [DataMember]
        public DateTime IncidentAlarmTime { get; set; }
        [DataMember]
        public DateTime IncidentAlarmCleared { get; set; }
        [DataMember]
        public DateTime IncidentAlarmExcused { get; set; }
        [DataMember]
        public string IncidentAlarmComments { get; set; }
        [DataMember]
        public int IncidentDuration { get; set; }
        [DataMember]
        public Guid GPSIssueAlarmID { get; set; }
        [DataMember]
        public bool GPSIssueAlarm { get; set; }
        [DataMember]
        public DateTime GPSIssueAlarmStart { get; set; }
        [DataMember]
        public DateTime GPSIssueAlarmCleared { get; set; }
        [DataMember]
        public DateTime GPSIssueAlarmExcused { get; set; }
        [DataMember]
        public string GPSIssueAlarmComments { get; set; }
        [DataMember]
        public int GPSIssueDuration { get; set; }
        [DataMember]
        public Guid StationaryAlarmID { get; set; }
        [DataMember]
        public bool StationaryAlarm { get; set; }
        [DataMember]
        public DateTime StationaryAlarmStart { get; set; }
        [DataMember]
        public DateTime StationaryAlarmCleared { get; set; }
        [DataMember]
        public DateTime StationaryAlarmExcused { get; set; }
        [DataMember]
        public string StationaryAlarmComments { get; set; }
        [DataMember]
        public int StationaryAlarmDuration { get; set; }
        //Long Break
        [DataMember]
        public bool LongBreakAlarm { get; set; }
        [DataMember]
        public DateTime LongBreakAlarmStart { get; set; }
        [DataMember]
        public Guid LongBreakAlarmID { get; set; }
        [DataMember]
        public DateTime LongBreakAlarmCleared { get; set; }
        [DataMember]
        public DateTime LongBreakAlarmExcused { get; set; }
        [DataMember]
        public string LongBreakAlarmComments { get; set; }
        [DataMember]
        public int LongBreakDuration { get; set; }
        //Long Lunch
        [DataMember]
        public bool LongLunchAlarm { get; set; }
        [DataMember]
        public Guid LongLunchAlarmID { get; set; }
        [DataMember]
        public DateTime LongLunchAlarmStart { get; set; }
        [DataMember]
        public DateTime LongLunchAlarmCleared { get; set; }
        [DataMember]
        public DateTime LongLunchAlarmExcused { get; set; }
        [DataMember]
        public string LongLunchAlarmComments { get; set; }
        [DataMember]
        public int LongLunchDuration { get; set; }
        //Overtime
        [DataMember]
        public bool OvertimeAlarm { get; set; }
        [DataMember]
        public Guid OvertimeAlarmID { get; set; }
        [DataMember]
        public DateTime OvertimeAlarmStart { get; set; }
        [DataMember]
        public DateTime OvertimeAlarmCleared { get; set; }
        [DataMember]
        public DateTime OvertimeAlarmExcused { get; set; }
        [DataMember]
        public string OvertimeAlarmComments { get; set; }
        [DataMember]
        public int OvertimeAlarmDuration { get; set; }
    }

    [DataContract]
    public class TruckMessage
    {
        [DataMember]
        public Guid MessageID { get; set; }
        [DataMember]
        public string TruckIP { get; set; }
        [DataMember]
        public string MessageText { get; set; }
        [DataMember]
        public string Driver { get; set; }
        [DataMember]
        public string Beat { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public DateTime SentTime { get; set; }
        [DataMember]
        public string UserEmail { get; set; } //email address
        [DataMember]
        public bool Acked { get; set; }
        [DataMember]
        public DateTime AckedTime { get; set; }
        [DataMember]
        public int MessageType { get; set; }
        [DataMember]
        public string MessageResponse { get; set; }
    }

    #endregion

    #region  " Alarm Data "

    [DataContract]
    public class AlarmData
    {
        [DataMember]
        public string Beat { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string CallSign { get; set; }
        [DataMember]
        public string ContractorCompany { get; set; }
        [DataMember]
        public string alarmName { get; set; }
        [DataMember]
        public bool hasAlarm { get; set; }
        [DataMember]
        public Guid alarmID { get; set; }
        [DataMember]
        public DateTime alarmStart { get; set; }
        [DataMember]
        public DateTime? alarmEnd { get; set; }
        [DataMember]
        public DateTime? alarmCleared { get; set; }
        [DataMember]
        public DateTime? alarmExcused { get; set; }
        [DataMember]
        public string comments { get; set; }
        [DataMember]
        public int? alarmDuration { get; set; }
    }


    #endregion

    #region  " Other Server Classes "

    [DataContract]
    public class CopyTruck
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public string BeatNumber { get; set; }
        [DataMember]
        public Guid BeatID { get; set; }
        [DataMember]
        public CopyStatus Status { get; set; }
        [DataMember]
        public CopyDriver Driver { get; set; }
        [DataMember]
        public CopyExtended Extended { get; set; }

    }

    [DataContract]
    public class CopyStatus
    {
        [DataMember]
        public bool SpeedingAlarm { get; set; }
        [DataMember]
        public double SpeedingValue { get; set; }
        [DataMember]
        public DateTime SpeedingTime { get; set; }
        //[DataMember]
        //public SqlGeography SpeedingLocation { get; set; }
        [DataMember]
        public bool OutOfBoundsAlarm { get; set; }
        [DataMember]
        public string OutOfBoundsMessage { get; set; }
        [DataMember]
        public DateTime OutOfBoundsTime { get; set; }
        [DataMember]
        public DateTime OutOfBoundsStartTime { get; set; }
        [DataMember]
        public string VehicleStatus { get; set; }
        [DataMember]
        public DateTime StatusStarted { get; set; }
        [DataMember]
        public bool LogOnAlarm { get; set; }
        [DataMember]
        public DateTime LogOnAlarmTime { get; set; }
        [DataMember]
        public DateTime LogOnAlarmCleared { get; set; }
        [DataMember]
        public DateTime LogOnAlarmExcused { get; set; }
        [DataMember]
        public string LogOnAlarmComments { get; set; }
        [DataMember]
        public bool RollOutAlarm { get; set; }
        [DataMember]
        public DateTime RollOutAlarmTime { get; set; }
        [DataMember]
        public DateTime RollOutAlarmCleared { get; set; }
        [DataMember]
        public DateTime RollOutAlarmExcused { get; set; }
        [DataMember]
        public string RollOutAlarmComments { get; set; }
        [DataMember]
        public bool OnPatrolAlarm { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmTime { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmCleared { get; set; }
        [DataMember]
        public DateTime OnPatrolAlarmExcused { get; set; }
        [DataMember]
        public string OnPatrolAlarmComments { get; set; }
        [DataMember]
        public bool RollInAlarm { get; set; }
        [DataMember]
        public DateTime RollInAlarmTime { get; set; }
        [DataMember]
        public DateTime RollInAlarmCleared { get; set; }
        [DataMember]
        public DateTime RollInAlarmExcused { get; set; }
        [DataMember]
        public string RollInAlarmComments { get; set; }
        [DataMember]
        public bool LogOffAlarm { get; set; }
        [DataMember]
        public DateTime LogOffAlarmTime { get; set; }
        [DataMember]
        public DateTime LogOffAlarmCleared { get; set; }
        [DataMember]
        public DateTime LogOffAlarmExcused { get; set; }
        [DataMember]
        public string LogOffAlarmComments { get; set; }
        [DataMember]
        public bool IncidentAlarm { get; set; }
        [DataMember]
        public DateTime IncidentAlarmTime { get; set; }
        [DataMember]
        public DateTime IncidentAlarmCleared { get; set; }
        [DataMember]
        public DateTime IncidentAlarmExcused { get; set; }
        [DataMember]
        public string IncidentAlarmComments { get; set; }
        [DataMember]
        public bool GPSIssueAlarm { get; set; } //handles NO GPS
        [DataMember]
        public DateTime GPSIssueAlarmStart { get; set; } //handles NO GPS
        [DataMember]
        public DateTime GPSIssueAlarmCleared { get; set; }
        [DataMember]
        public DateTime GPSIssueAlarmExcused { get; set; }
        [DataMember]
        public string GPSIssueAlarmComments { get; set; }
        [DataMember]
        public bool StationaryAlarm { get; set; } //handles no movement, speed = 0
        [DataMember]
        public DateTime StationaryAlarmStart { get; set; } //handles no movement, speed = 0
        [DataMember]
        public DateTime StationaryAlarmCleared { get; set; }
        [DataMember]
        public DateTime StationaryAlarmExcused { get; set; }
        [DataMember]
        public string StationaryAlarmComments { get; set; }
    }

    [DataContract]
    public class CopyDriver
    {
        [DataMember]
        public Guid DriverID { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string TowTruckCompany { get; set; }
        [DataMember]
        public string FSPID { get; set; }
        [DataMember]
        public Guid AssignedBeat { get; set; }
        [DataMember]
        public Guid BeatScheduleID { get; set; }
        [DataMember]
        public DateTime BreakStarted { get; set; }
        [DataMember]
        public DateTime LunchStarted { get; set; }
    }

    [DataContract]
    public class CopyExtended
    {
        [DataMember]
        public string ContractorName { get; set; }
        [DataMember]
        public Guid ContractorID { get; set; }
        [DataMember]
        public string TruckNumber { get; set; }
        [DataMember]
        public string FleetNumber { get; set; }
        [DataMember]
        public DateTime ProgramStartDate { get; set; }
        [DataMember]
        public string VehicleType { get; set; }
        [DataMember]
        public int VehicleYear { get; set; }
        [DataMember]
        public string VehicleMake { get; set; }
        [DataMember]
        public string VehicleModel { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public DateTime RegistrationExpireDate { get; set; }
        [DataMember]
        public DateTime InsuranceExpireDate { get; set; }
        [DataMember]
        public DateTime LastCHPInspection { get; set; }
        [DataMember]
        public DateTime ProgramEndDate { get; set; }
        [DataMember]
        public int FAW { get; set; }
        [DataMember]
        public int RAW { get; set; }
        [DataMember]
        public int RAWR { get; set; }
        [DataMember]
        public int GVW { get; set; }
        [DataMember]
        public int GVWR { get; set; }
        [DataMember]
        public int Wheelbase { get; set; }
        [DataMember]
        public int Overhang { get; set; }
        [DataMember]
        public int MAXTW { get; set; }
        [DataMember]
        public DateTime MAXTWCALCDATE { get; set; }
        [DataMember]
        public string FuelType { get; set; }
        [DataMember]
        public Guid FleetVehicleID { get; set; }
    }
#endregion
}
