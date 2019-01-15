﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTC.FSP.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MTCDBEntities : DbContext
    {
        public MTCDBEntities()
            : base("name=MTCDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AEReportType> AEReportTypes { get; set; }
        public virtual DbSet<ContractorManager> ContractorManagers { get; set; }
        public virtual DbSet<Contractor> Contractors { get; set; }
        public virtual DbSet<ContractsBeat> ContractsBeats { get; set; }
        public virtual DbSet<DriverEvent> DriverEvents { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<FleetVehicle> FleetVehicles { get; set; }
        public virtual DbSet<Freeway> Freeways { get; set; }
        public virtual DbSet<GPSTrackingLog> GPSTrackingLogs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<HeliosUnit> HeliosUnits { get; set; }
        public virtual DbSet<InspectionType> InspectionTypes { get; set; }
        public virtual DbSet<InteractionType> InteractionTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<TowTruckYard> TowTruckYards { get; set; }
        public virtual DbSet<TruckMessage> TruckMessages { get; set; }
        public virtual DbSet<Var> Vars { get; set; }
        public virtual DbSet<YearlyCalendar> YearlyCalendars { get; set; }
        public virtual DbSet<AEFrequency> AEFrequencies { get; set; }
        public virtual DbSet<AERecipient> AERecipients { get; set; }
        public virtual DbSet<AEReport> AEReports { get; set; }
        public virtual DbSet<TowTruckSetup> TowTruckSetups { get; set; }
        public virtual DbSet<TruckState> TruckStates { get; set; }
        public virtual DbSet<ContractorType> ContractorTypes { get; set; }
        public virtual DbSet<MTCSchedule> MTCSchedules { get; set; }
        public virtual DbSet<DriverInteraction> DriverInteractions { get; set; }
        public virtual DbSet<InsuranceCarrier> InsuranceCarriers { get; set; }
        public virtual DbSet<TruckAlert> TruckAlerts { get; set; }
        public virtual DbSet<CHPOfficerBeat> CHPOfficerBeats { get; set; }
        public virtual DbSet<CHPInspection> CHPInspections { get; set; }
        public virtual DbSet<CHPOfficer> CHPOfficers { get; set; }
        public virtual DbSet<MTCBeatsCallSign> MTCBeatsCallSigns { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<VehicleTypeLU> VehicleTypeLUs { get; set; }
        public virtual DbSet<OvertimeActivity> OvertimeActivities { get; set; }
        public virtual DbSet<BeatBeatSchedule> BeatBeatSchedules { get; set; }
        public virtual DbSet<BeatSchedule> BeatSchedules { get; set; }
        public virtual DbSet<BeatBeatSegment> BeatBeatSegments { get; set; }
        public virtual DbSet<MTC_Invoice_Addition> MTC_Invoice_Addition { get; set; }
        public virtual DbSet<MTC_Invoice_Deductions> MTC_Invoice_Deductions { get; set; }
        public virtual DbSet<MTC_Invoice_Summary> MTC_Invoice_Summary { get; set; }
        public virtual DbSet<MTC_Invoice_Anomalies> MTC_Invoice_Anomalies { get; set; }
        public virtual DbSet<TruckStatu> TruckStatus { get; set; }
        public virtual DbSet<MTC_Invoice> MTC_Invoice { get; set; }
        public virtual DbSet<MTCActionTaken> MTCActionTakens { get; set; }
        public virtual DbSet<MTCAssist> MTCAssists { get; set; }
        public virtual DbSet<MTCIncident> MTCIncidents { get; set; }
        public virtual DbSet<MTCPreAssist> MTCPreAssists { get; set; }
        public virtual DbSet<Assist> Assists { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<BeatData> BeatDatas { get; set; }
        public virtual DbSet<BeatsFreeway> BeatsFreeways { get; set; }
        public virtual DbSet<WAZEData> WAZEDatas { get; set; }
        public virtual DbSet<WAZEIncoming> WAZEIncomings { get; set; }
        public virtual DbSet<Appeal> Appeals { get; set; }
        public virtual DbSet<AppealStatu> AppealStatus { get; set; }
        public virtual DbSet<MTCRateTable> MTCRateTables { get; set; }
    
        public virtual ObjectResult<GetBeatSchedules_Result> GetBeatSchedules()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetBeatSchedules_Result>("GetBeatSchedules");
        }
    
        public virtual ObjectResult<GetDailySchedules_Result> GetDailySchedules()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDailySchedules_Result>("GetDailySchedules");
        }
    
        public virtual ObjectResult<SchedulesSearch_Result> SchedulesSearch(Nullable<System.DateTime> dt, Nullable<System.Guid> beatid)
        {
            var dtParameter = dt.HasValue ?
                new ObjectParameter("dt", dt) :
                new ObjectParameter("dt", typeof(System.DateTime));
    
            var beatidParameter = beatid.HasValue ?
                new ObjectParameter("beatid", beatid) :
                new ObjectParameter("beatid", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SchedulesSearch_Result>("SchedulesSearch", dtParameter, beatidParameter);
        }
    }
}
