using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FSP.Domain2.Models.Mapping;

namespace FSP.Domain2.Models
{
    public partial class fspContext : DbContext
    {
        static fspContext()
        {
            Database.SetInitializer<fspContext>(null);
        }

        public fspContext()
            : base("Name=fspContext")
        {
        }

        public DbSet<C1098Codes> C1098Codes { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Assist> Assists { get; set; }
        public DbSet<BeatAlarm> BeatAlarms { get; set; }
        public DbSet<BeatBeatSchedule> BeatBeatSchedules { get; set; }
        public DbSet<BeatBeatSegment> BeatBeatSegments { get; set; }
        public DbSet<Beat> Beats { get; set; }
        public DbSet<BeatSchedule> BeatSchedules { get; set; }
        public DbSet<BeatSegment> BeatSegments { get; set; }
        public DbSet<BeatsFreeway> BeatsFreeways { get; set; }
        public DbSet<buBeatBeatSegment> buBeatBeatSegments { get; set; }
        public DbSet<buBeat> buBeats { get; set; }
        public DbSet<buBeatSegment> buBeatSegments { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CallBox> CallBoxes { get; set; }
        public DbSet<CHPInspection> CHPInspections { get; set; }
        public DbSet<CHPOfficer> CHPOfficers { get; set; }
        public DbSet<ContractorManager> ContractorManagers { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractsBeat> ContractsBeats { get; set; }
        public DbSet<DriverBreak> DriverBreaks { get; set; }
        public DbSet<DriverDailySchedule> DriverDailySchedules { get; set; }
        public DbSet<DriverEvent> DriverEvents { get; set; }
        public DbSet<DriverHistory> DriverHistories { get; set; }
        public DbSet<DriverInteraction> DriverInteractions { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DropZone> DropZones { get; set; }
        public DbSet<FleetVehicle> FleetVehicles { get; set; }
        public DbSet<FleetVehiclesBeat> FleetVehiclesBeats { get; set; }
        public DbSet<Freeway> Freeways { get; set; }
        public DbSet<GPSTracking> GPSTrackings { get; set; }
        public DbSet<GPSTrackingLog> GPSTrackingLogs { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<HeliosUnit> HeliosUnits { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<InspectionType> InspectionTypes { get; set; }
        public DbSet<InsuranceCarrier> InsuranceCarriers { get; set; }
        public DbSet<InteractionType> InteractionTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OffBeatAlert> OffBeatAlerts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServicesPerformed> ServicesPerformeds { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ShiftSchedule> ShiftSchedules { get; set; }
        public DbSet<SpeedingAlert> SpeedingAlerts { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TowLocation> TowLocations { get; set; }
        public DbSet<TowTruckSetup> TowTruckSetups { get; set; }
        public DbSet<TowTruckYard> TowTruckYards { get; set; }
        public DbSet<TrafficSpeed> TrafficSpeeds { get; set; }
        public DbSet<TruckMessage> TruckMessages { get; set; }
        public DbSet<TruckState> TruckStates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Var> Vars { get; set; }
        public DbSet<VehiclePosition> VehiclePositions { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Weblink> Weblinks { get; set; }
        public DbSet<YearlyCalendar> YearlyCalendars { get; set; }
        public DbSet<vBeat> vBeats { get; set; }
        public DbSet<vBeatSegment> vBeatSegments { get; set; }
        public DbSet<vCallBox> vCallBoxes { get; set; }
        public DbSet<vDropZone> vDropZones { get; set; }
        public DbSet<vGPSTracking> vGPSTrackings { get; set; }
        public DbSet<vGPSTrackingLog> vGPSTrackingLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new C1098CodesMap());
            modelBuilder.Configurations.Add(new AlarmMap());
            modelBuilder.Configurations.Add(new AssistMap());
            modelBuilder.Configurations.Add(new BeatAlarmMap());
            modelBuilder.Configurations.Add(new BeatBeatScheduleMap());
            modelBuilder.Configurations.Add(new BeatBeatSegmentMap());
            modelBuilder.Configurations.Add(new BeatMap());
            modelBuilder.Configurations.Add(new BeatScheduleMap());
            modelBuilder.Configurations.Add(new BeatSegmentMap());
            modelBuilder.Configurations.Add(new BeatsFreewayMap());
            modelBuilder.Configurations.Add(new buBeatBeatSegmentMap());
            modelBuilder.Configurations.Add(new buBeatMap());
            modelBuilder.Configurations.Add(new buBeatSegmentMap());
            modelBuilder.Configurations.Add(new CalendarMap());
            modelBuilder.Configurations.Add(new CallBoxMap());
            modelBuilder.Configurations.Add(new CHPInspectionMap());
            modelBuilder.Configurations.Add(new CHPOfficerMap());
            modelBuilder.Configurations.Add(new ContractorManagerMap());
            modelBuilder.Configurations.Add(new ContractorMap());
            modelBuilder.Configurations.Add(new ContractMap());
            modelBuilder.Configurations.Add(new ContractsBeatMap());
            modelBuilder.Configurations.Add(new DriverBreakMap());
            modelBuilder.Configurations.Add(new DriverDailyScheduleMap());
            modelBuilder.Configurations.Add(new DriverEventMap());
            modelBuilder.Configurations.Add(new DriverHistoryMap());
            modelBuilder.Configurations.Add(new DriverInteractionMap());
            modelBuilder.Configurations.Add(new DriverMap());
            modelBuilder.Configurations.Add(new DropZoneMap());
            modelBuilder.Configurations.Add(new FleetVehicleMap());
            modelBuilder.Configurations.Add(new FleetVehiclesBeatMap());
            modelBuilder.Configurations.Add(new FreewayMap());
            modelBuilder.Configurations.Add(new GPSTrackingMap());
            modelBuilder.Configurations.Add(new GPSTrackingLogMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new HeliosUnitMap());
            modelBuilder.Configurations.Add(new IncidentMap());
            modelBuilder.Configurations.Add(new IncidentTypeMap());
            modelBuilder.Configurations.Add(new InspectionTypeMap());
            modelBuilder.Configurations.Add(new InsuranceCarrierMap());
            modelBuilder.Configurations.Add(new InteractionTypeMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new OffBeatAlertMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new ServicesPerformedMap());
            modelBuilder.Configurations.Add(new ServiceTypeMap());
            modelBuilder.Configurations.Add(new ShiftScheduleMap());
            modelBuilder.Configurations.Add(new SpeedingAlertMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TowLocationMap());
            modelBuilder.Configurations.Add(new TowTruckSetupMap());
            modelBuilder.Configurations.Add(new TowTruckYardMap());
            modelBuilder.Configurations.Add(new TrafficSpeedMap());
            modelBuilder.Configurations.Add(new TruckMessageMap());
            modelBuilder.Configurations.Add(new TruckStateMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VarMap());
            modelBuilder.Configurations.Add(new VehiclePositionMap());
            modelBuilder.Configurations.Add(new VehicleTypeMap());
            modelBuilder.Configurations.Add(new WeblinkMap());
            modelBuilder.Configurations.Add(new YearlyCalendarMap());
            modelBuilder.Configurations.Add(new vBeatMap());
            modelBuilder.Configurations.Add(new vBeatSegmentMap());
            modelBuilder.Configurations.Add(new vCallBoxMap());
            modelBuilder.Configurations.Add(new vDropZoneMap());
            modelBuilder.Configurations.Add(new vGPSTrackingMap());
            modelBuilder.Configurations.Add(new vGPSTrackingLogMap());
        }
    }
}
