
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/07/2017 07:59:44
-- Generated from EDMX file: C:\TortoiseSVN\MTC\LATA.MTC.PublicSite\FSP MTC Web Application\MTC.FSP.Web\Models\MTCDatabase.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MTCDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Appeals_AppealStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appeals] DROP CONSTRAINT [FK_Appeals_AppealStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_Appeals_Beats]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appeals] DROP CONSTRAINT [FK_Appeals_Beats];
GO
IF OBJECT_ID(N'[dbo].[FK_Appeals_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appeals] DROP CONSTRAINT [FK_Appeals_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_Appeals_Drivers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Appeals] DROP CONSTRAINT [FK_Appeals_Drivers];
GO
IF OBJECT_ID(N'[dbo].[FK_BeatsFreeways_Beats]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BeatsFreeways] DROP CONSTRAINT [FK_BeatsFreeways_Beats];
GO
IF OBJECT_ID(N'[dbo].[FK_BeatsFreeways_Freeways]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BeatsFreeways] DROP CONSTRAINT [FK_BeatsFreeways_Freeways];
GO
IF OBJECT_ID(N'[dbo].[FK_CHPInspections_CHPOfficer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHPInspections] DROP CONSTRAINT [FK_CHPInspections_CHPOfficer];
GO
IF OBJECT_ID(N'[dbo].[FK_CHPInspections_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHPInspections] DROP CONSTRAINT [FK_CHPInspections_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_CHPInspections_FleetVehicles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHPInspections] DROP CONSTRAINT [FK_CHPInspections_FleetVehicles];
GO
IF OBJECT_ID(N'[dbo].[FK_CHPInspections_InspectionTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CHPInspections] DROP CONSTRAINT [FK_CHPInspections_InspectionTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_ContractorManagers_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContractorManagers] DROP CONSTRAINT [FK_ContractorManagers_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_Contractors_ContractorTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contractors] DROP CONSTRAINT [FK_Contractors_ContractorTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_DriverInteractions_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DriverInteractions] DROP CONSTRAINT [FK_DriverInteractions_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_DriverInteractions_Drivers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DriverInteractions] DROP CONSTRAINT [FK_DriverInteractions_Drivers];
GO
IF OBJECT_ID(N'[dbo].[FK_DriverInteractions_InteractionTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DriverInteractions] DROP CONSTRAINT [FK_DriverInteractions_InteractionTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_Drivers_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Drivers] DROP CONSTRAINT [FK_Drivers_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_FleetVehicles_Contractors]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FleetVehicles] DROP CONSTRAINT [FK_FleetVehicles_Contractors];
GO
IF OBJECT_ID(N'[dbo].[FK_MTC_Invoice_Addition_MTC_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MTC_Invoice_Addition] DROP CONSTRAINT [FK_MTC_Invoice_Addition_MTC_Invoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MTC_Invoice_Anomalies_MTC_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MTC_Invoice_Anomalies] DROP CONSTRAINT [FK_MTC_Invoice_Anomalies_MTC_Invoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MTC_Invoice_Deductions_MTC_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MTC_Invoice_Deductions] DROP CONSTRAINT [FK_MTC_Invoice_Deductions_MTC_Invoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MTC_Invoice_Summary_MTC_Invoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MTC_Invoice_Summary] DROP CONSTRAINT [FK_MTC_Invoice_Summary_MTC_Invoice];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AEReportTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AEReportTypes];
GO
IF OBJECT_ID(N'[dbo].[Appeals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Appeals];
GO
IF OBJECT_ID(N'[dbo].[AppealStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AppealStatus];
GO
IF OBJECT_ID(N'[dbo].[Assists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Assists];
GO
IF OBJECT_ID(N'[dbo].[BeatBeatSchedules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BeatBeatSchedules];
GO
IF OBJECT_ID(N'[dbo].[BeatBeatSegments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BeatBeatSegments];
GO
IF OBJECT_ID(N'[dbo].[Beats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Beats];
GO
IF OBJECT_ID(N'[dbo].[BeatSchedules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BeatSchedules];
GO
IF OBJECT_ID(N'[dbo].[BeatsFreeways]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BeatsFreeways];
GO
IF OBJECT_ID(N'[dbo].[CHPInspections]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CHPInspections];
GO
IF OBJECT_ID(N'[dbo].[CHPOfficer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CHPOfficer];
GO
IF OBJECT_ID(N'[dbo].[CHPOfficerBeats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CHPOfficerBeats];
GO
IF OBJECT_ID(N'[dbo].[ContractorManagers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractorManagers];
GO
IF OBJECT_ID(N'[dbo].[Contractors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contractors];
GO
IF OBJECT_ID(N'[dbo].[ContractorTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractorTypes];
GO
IF OBJECT_ID(N'[dbo].[Contracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contracts];
GO
IF OBJECT_ID(N'[dbo].[ContractsBeats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContractsBeats];
GO
IF OBJECT_ID(N'[dbo].[DriverEvents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DriverEvents];
GO
IF OBJECT_ID(N'[dbo].[DriverInteractions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DriverInteractions];
GO
IF OBJECT_ID(N'[dbo].[Drivers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Drivers];
GO
IF OBJECT_ID(N'[dbo].[FleetVehicles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FleetVehicles];
GO
IF OBJECT_ID(N'[dbo].[Freeways]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Freeways];
GO
IF OBJECT_ID(N'[dbo].[GPSTrackingLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GPSTrackingLog];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[HeliosUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HeliosUnits];
GO
IF OBJECT_ID(N'[dbo].[Incidents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Incidents];
GO
IF OBJECT_ID(N'[dbo].[InspectionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InspectionTypes];
GO
IF OBJECT_ID(N'[dbo].[InsuranceCarriers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InsuranceCarriers];
GO
IF OBJECT_ID(N'[dbo].[InteractionTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InteractionTypes];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[MTC_Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTC_Invoice];
GO
IF OBJECT_ID(N'[dbo].[MTC_Invoice_Addition]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTC_Invoice_Addition];
GO
IF OBJECT_ID(N'[dbo].[MTC_Invoice_Anomalies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTC_Invoice_Anomalies];
GO
IF OBJECT_ID(N'[dbo].[MTC_Invoice_Deductions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTC_Invoice_Deductions];
GO
IF OBJECT_ID(N'[dbo].[MTC_Invoice_Summary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTC_Invoice_Summary];
GO
IF OBJECT_ID(N'[dbo].[MTCActionTaken]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCActionTaken];
GO
IF OBJECT_ID(N'[dbo].[MTCAssists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCAssists];
GO
IF OBJECT_ID(N'[dbo].[MTCBeatsCallSigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCBeatsCallSigns];
GO
IF OBJECT_ID(N'[dbo].[MTCIncidents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCIncidents];
GO
IF OBJECT_ID(N'[dbo].[MTCPreAssists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCPreAssists];
GO
IF OBJECT_ID(N'[dbo].[MTCRateTable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCRateTable];
GO
IF OBJECT_ID(N'[dbo].[MTCSchedules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MTCSchedules];
GO
IF OBJECT_ID(N'[dbo].[OvertimeActivity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OvertimeActivity];
GO
IF OBJECT_ID(N'[dbo].[Reports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reports];
GO
IF OBJECT_ID(N'[dbo].[TowTruckYard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TowTruckYard];
GO
IF OBJECT_ID(N'[dbo].[TruckAlerts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TruckAlerts];
GO
IF OBJECT_ID(N'[dbo].[TruckMessages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TruckMessages];
GO
IF OBJECT_ID(N'[dbo].[TruckStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TruckStatus];
GO
IF OBJECT_ID(N'[dbo].[Vars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vars];
GO
IF OBJECT_ID(N'[dbo].[VehicleTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleTypes];
GO
IF OBJECT_ID(N'[dbo].[YearlyCalendar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[YearlyCalendar];
GO
IF OBJECT_ID(N'[MTCDBModelStoreContainer].[AEFrequencies]', 'U') IS NOT NULL
    DROP TABLE [MTCDBModelStoreContainer].[AEFrequencies];
GO
IF OBJECT_ID(N'[MTCDBModelStoreContainer].[AERecipients]', 'U') IS NOT NULL
    DROP TABLE [MTCDBModelStoreContainer].[AERecipients];
GO
IF OBJECT_ID(N'[MTCDBModelStoreContainer].[AEReports]', 'U') IS NOT NULL
    DROP TABLE [MTCDBModelStoreContainer].[AEReports];
GO
IF OBJECT_ID(N'[MTCDBModelStoreContainer].[TowTruckSetup]', 'U') IS NOT NULL
    DROP TABLE [MTCDBModelStoreContainer].[TowTruckSetup];
GO
IF OBJECT_ID(N'[MTCDBModelStoreContainer].[TruckStates]', 'U') IS NOT NULL
    DROP TABLE [MTCDBModelStoreContainer].[TruckStates];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AEReportTypes'
CREATE TABLE [dbo].[AEReportTypes] (
    [AEReportTypeID] uniqueidentifier  NOT NULL,
    [AEReportTypeName] varchar(20)  NOT NULL
);
GO

-- Creating table 'CallBoxes'
CREATE TABLE [dbo].[CallBoxes] (
    [CallBoxID] uniqueidentifier  NOT NULL,
    [TelephoneNumber] varchar(20)  NOT NULL,
    [Location] varchar(100)  NOT NULL,
    [FreewayID] int  NOT NULL,
    [SiteType] varchar(50)  NOT NULL,
    [Comments] varchar(500)  NOT NULL,
    [Position] geography  NOT NULL,
    [SignNumber] varchar(50)  NOT NULL
);
GO

-- Creating table 'ContractorManagers'
CREATE TABLE [dbo].[ContractorManagers] (
    [ContractorManagerID] uniqueidentifier  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [PhoneNumber] varchar(20)  NOT NULL,
    [CellPhone] varchar(20)  NULL,
    [Email] varchar(75)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NULL,
    [ContractorID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Contractors'
CREATE TABLE [dbo].[Contractors] (
    [ContractorID] uniqueidentifier  NOT NULL,
    [Address] varchar(200)  NOT NULL,
    [OfficeTelephone] varchar(20)  NOT NULL,
    [MCPNumber] varchar(50)  NOT NULL,
    [MCPExpiration] datetime  NOT NULL,
    [Comments] varchar(500)  NULL,
    [ContractCompanyName] varchar(100)  NULL,
    [City] varchar(50)  NULL,
    [State] varchar(2)  NULL,
    [Zip] varchar(10)  NULL,
    [Email] varchar(150)  NULL,
    [ContractorTypeId] int  NULL,
    [ContactFirstName] varchar(150)  NULL,
    [ContactLastName] varchar(150)  NULL
);
GO

-- Creating table 'ContractsBeats'
CREATE TABLE [dbo].[ContractsBeats] (
    [ContractID] uniqueidentifier  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'DailyBeatSchedules'
CREATE TABLE [dbo].[DailyBeatSchedules] (
    [ID] uniqueidentifier  NOT NULL,
    [RunDate] datetime  NOT NULL,
    [BeatNumber] varchar(20)  NOT NULL,
    [ScheduleName] varchar(100)  NOT NULL,
    [Supervisor] varchar(100)  NOT NULL,
    [OfficePhone] varchar(100)  NOT NULL,
    [CellPhone] varchar(100)  NOT NULL,
    [ContractorCompany] varchar(50)  NOT NULL,
    [Weekday] bit  NULL
);
GO

-- Creating table 'DriverEvents'
CREATE TABLE [dbo].[DriverEvents] (
    [ID] uniqueidentifier  NOT NULL,
    [DriverID] uniqueidentifier  NOT NULL,
    [EventType] varchar(100)  NOT NULL,
    [TimeStamp] datetime  NOT NULL
);
GO

-- Creating table 'Drivers'
CREATE TABLE [dbo].[Drivers] (
    [DriverID] uniqueidentifier  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [FirstName] varchar(50)  NOT NULL,
    [FSPIDNumber] varchar(50)  NOT NULL,
    [ProgramStartDate] datetime  NOT NULL,
    [TrainingCompletionDate] datetime  NULL,
    [DOB] datetime  NOT NULL,
    [LicenseExpirationDate] datetime  NOT NULL,
    [DL64ExpirationDate] datetime  NOT NULL,
    [MedicalCardExpirationDate] datetime  NOT NULL,
    [LastPullNoticeDate] datetime  NULL,
    [DateAdded] datetime  NOT NULL,
    [UDF] varchar(500)  NULL,
    [Comments] varchar(500)  NULL,
    [ContractorEndDate] datetime  NULL,
    [ProgramEndDate] datetime  NULL,
    [ContractorStartDate] datetime  NULL,
    [BeatID] uniqueidentifier  NULL,
    [Password] varchar(50)  NOT NULL,
    [DL64Number] nvarchar(50)  NULL,
    [DriversLicenseNumber] nvarchar(50)  NULL,
    [AddedtoC3Database] datetime  NULL
);
GO

-- Creating table 'FleetVehicles'
CREATE TABLE [dbo].[FleetVehicles] (
    [FleetVehicleID] uniqueidentifier  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [ProgramStartDate] datetime  NOT NULL,
    [FleetNumber] varchar(50)  NOT NULL,
    [VehicleType] varchar(50)  NOT NULL,
    [VehicleYear] int  NOT NULL,
    [VehicleMake] varchar(50)  NOT NULL,
    [VehicleModel] varchar(50)  NOT NULL,
    [VIN] varchar(50)  NOT NULL,
    [LicensePlate] varchar(10)  NOT NULL,
    [RegistrationExpireDate] datetime  NOT NULL,
    [InsuranceExpireDate] datetime  NOT NULL,
    [LastCHPInspection] datetime  NOT NULL,
    [Comments] varchar(500)  NULL,
    [ProgramEndDate] datetime  NOT NULL,
    [FAW] int  NOT NULL,
    [RAW] int  NOT NULL,
    [RAWR] int  NOT NULL,
    [GVW] int  NOT NULL,
    [GVWR] int  NOT NULL,
    [Wheelbase] int  NOT NULL,
    [Overhang] int  NOT NULL,
    [MAXTW] int  NOT NULL,
    [MAXTWCALCDATE] datetime  NOT NULL,
    [FuelType] varchar(20)  NOT NULL,
    [VehicleNumber] varchar(50)  NOT NULL,
    [IPAddress] varchar(20)  NOT NULL,
    [AgreementNumber] varchar(50)  NULL,
    [IsBackup] bit  NOT NULL
);
GO

-- Creating table 'Freeways'
CREATE TABLE [dbo].[Freeways] (
    [FreewayID] int  NOT NULL,
    [FreewayName] varchar(50)  NOT NULL
);
GO

-- Creating table 'GPSTrackingLogs'
CREATE TABLE [dbo].[GPSTrackingLogs] (
    [GPSID] uniqueidentifier  NOT NULL,
    [Direction] varchar(10)  NOT NULL,
    [VehicleStatus] varchar(50)  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [BeatSegmentID] uniqueidentifier  NOT NULL,
    [VehicleID] varchar(50)  NOT NULL,
    [Speed] float  NOT NULL,
    [Alarms] bit  NOT NULL,
    [DriverID] uniqueidentifier  NOT NULL,
    [VehicleNumber] varchar(50)  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [Position] geography  NOT NULL,
    [SpeedingAlarm] bit  NOT NULL,
    [SpeedingValue] float  NULL,
    [SpeedingTime] datetime  NULL,
    [SpeedingLocation] geography  NULL,
    [OutOfBoundsAlarm] bit  NOT NULL,
    [OutOfBoundsMessage] varchar(50)  NULL,
    [Cell] varchar(100)  NULL,
    [runID] uniqueidentifier  NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [GroupID] uniqueidentifier  NOT NULL,
    [GroupName] varchar(50)  NOT NULL,
    [GroupCity] varchar(50)  NULL,
    [GroupState] char(2)  NULL,
    [GroupAddress] varchar(100)  NULL,
    [GroupZip] varchar(10)  NULL,
    [GroupPhone] varchar(20)  NULL,
    [GroupContactName] varchar(50)  NULL
);
GO

-- Creating table 'HeliosUnits'
CREATE TABLE [dbo].[HeliosUnits] (
    [HeliosUnitID] uniqueidentifier  NOT NULL,
    [HeliosID] varchar(50)  NOT NULL,
    [IPAddress] varchar(50)  NOT NULL,
    [PhoneNumber] varchar(50)  NULL,
    [DateAdded] datetime  NOT NULL,
    [DateModified] datetime  NULL
);
GO

-- Creating table 'InspectionTypes'
CREATE TABLE [dbo].[InspectionTypes] (
    [InspectionTypeID] uniqueidentifier  NOT NULL,
    [InspectionType1] varchar(50)  NOT NULL,
    [InspectionTypeCode] varchar(10)  NOT NULL
);
GO

-- Creating table 'InteractionTypes'
CREATE TABLE [dbo].[InteractionTypes] (
    [InteractionTypeID] uniqueidentifier  NOT NULL,
    [InteractionType1] varchar(500)  NOT NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [LocationID] uniqueidentifier  NOT NULL,
    [LocationCode] varchar(10)  NOT NULL,
    [Location1] varchar(100)  NOT NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [ReportID] uniqueidentifier  NOT NULL,
    [ReportName] varchar(100)  NOT NULL,
    [ConnString] varchar(10)  NOT NULL,
    [SQL] varchar(max)  NOT NULL,
    [cmdType] varchar(50)  NOT NULL,
    [parameters] varchar(500)  NULL
);
GO

-- Creating table 'TowTruckYards'
CREATE TABLE [dbo].[TowTruckYards] (
    [TowTruckYardID] uniqueidentifier  NOT NULL,
    [Location] varchar(100)  NOT NULL,
    [Comments] varchar(500)  NULL,
    [TowTruckYardNumber] varchar(50)  NOT NULL,
    [TowTruckYardDescription] varchar(500)  NULL,
    [TowTruckCompanyName] varchar(100)  NOT NULL,
    [TowTruckCompanyPhoneNumber] varchar(20)  NOT NULL,
    [Position] geography  NOT NULL
);
GO

-- Creating table 'TruckMessages'
CREATE TABLE [dbo].[TruckMessages] (
    [TruckMessageID] uniqueidentifier  NOT NULL,
    [TruckIP] varchar(50)  NOT NULL,
    [MessageText] varchar(2000)  NOT NULL,
    [SentTime] datetime  NOT NULL,
    [Acked] bit  NOT NULL,
    [AckedTime] datetime  NULL,
    [UserEmail] varchar(200)  NOT NULL,
    [TruckNumber] varchar(50)  NULL,
    [CallSign] varchar(50)  NULL,
    [DriverName] varchar(100)  NULL,
    [BeatNumber] varchar(20)  NULL,
    [Response] varchar(10)  NULL
);
GO

-- Creating table 'Vars'
CREATE TABLE [dbo].[Vars] (
    [VarID] uniqueidentifier  NOT NULL,
    [VarName] varchar(50)  NOT NULL,
    [VarValue] varchar(200)  NOT NULL,
    [Description] varchar(max)  NULL,
    [Units] varchar(50)  NULL,
    [IsMTCAlarm] bit  NULL,
    [FriendlyVarName] varchar(150)  NULL
);
GO

-- Creating table 'YearlyCalendars'
CREATE TABLE [dbo].[YearlyCalendars] (
    [DateID] uniqueidentifier  NOT NULL,
    [dayName] varchar(100)  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'AEFrequencies'
CREATE TABLE [dbo].[AEFrequencies] (
    [AEFrequencyID] uniqueidentifier  NOT NULL,
    [AEFrequencyName] varchar(50)  NOT NULL,
    [AEFrequencyDescription] varchar(200)  NULL
);
GO

-- Creating table 'AERecipients'
CREATE TABLE [dbo].[AERecipients] (
    [AERecipientID] uniqueidentifier  NOT NULL,
    [AERecipientEmail] varchar(200)  NOT NULL,
    [AEIsContractor] bit  NOT NULL,
    [AEReportID] uniqueidentifier  NOT NULL,
    [AEFrequencyID] uniqueidentifier  NOT NULL,
    [AEContractorID] uniqueidentifier  NULL,
    [AEReportTypeID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'AEReports'
CREATE TABLE [dbo].[AEReports] (
    [AEReportID] uniqueidentifier  NOT NULL,
    [AEReportName] varchar(50)  NOT NULL,
    [AEReportDescription] varchar(250)  NULL
);
GO

-- Creating table 'TowTruckSetups'
CREATE TABLE [dbo].[TowTruckSetups] (
    [MountedSecurely] varchar(5)  NOT NULL,
    [ConnectedToCell] varchar(5)  NOT NULL,
    [DCPowerConnected] varchar(5)  NOT NULL,
    [RouterUnitMountedSecurely] varchar(5)  NOT NULL,
    [MoistureFree] varchar(5)  NOT NULL,
    [Speedtest] varchar(5)  NOT NULL,
    [GPSSentProperly] varchar(5)  NOT NULL,
    [InstallerName] varchar(100)  NOT NULL,
    [InstallationDate] datetime  NOT NULL,
    [TowTruckCompany] varchar(100)  NOT NULL,
    [VehicleID] varchar(100)  NOT NULL,
    [SystemSerialNumber] varchar(100)  NOT NULL,
    [IPAddress] varchar(50)  NOT NULL,
    [UploadSpeed] varchar(10)  NULL,
    [DownloadSpeed] varchar(10)  NULL
);
GO

-- Creating table 'TruckStates'
CREATE TABLE [dbo].[TruckStates] (
    [TruckStateID] uniqueidentifier  NOT NULL,
    [TruckState1] varchar(100)  NOT NULL,
    [TruckIcon] varchar(100)  NULL
);
GO

-- Creating table 'ContractorTypes'
CREATE TABLE [dbo].[ContractorTypes] (
    [ContractorTypeId] int IDENTITY(1,1) NOT NULL,
    [ContractorTypeName] varchar(50)  NOT NULL
);
GO

-- Creating table 'Beats'
CREATE TABLE [dbo].[Beats] (
    [BeatID] uniqueidentifier  NOT NULL,
    [Active] bit  NOT NULL,
    [BeatDescription] varchar(500)  NULL,
    [BeatNumber] varchar(50)  NULL,
    [LastUpdate] datetime  NULL,
    [LastUpdateBy] varchar(50)  NULL,
    [IsTemporary] bit  NULL,
    [BeatColor] varchar(50)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [OnCallAreas] varchar(250)  NULL,
    [Freq] varchar(250)  NULL,
    [CHPArea] varchar(250)  NULL
);
GO

-- Creating table 'MTCSchedules'
CREATE TABLE [dbo].[MTCSchedules] (
    [ScheduleID] uniqueidentifier  NOT NULL,
    [ScheduleName] varchar(50)  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [StartTime] time  NOT NULL,
    [EndTime] time  NOT NULL
);
GO

-- Creating table 'DriverInteractions'
CREATE TABLE [dbo].[DriverInteractions] (
    [InteractionID] uniqueidentifier  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [DriverID] uniqueidentifier  NOT NULL,
    [InteractionTypeID] uniqueidentifier  NOT NULL,
    [InteractionArea] varchar(100)  NOT NULL,
    [InteractionDescription] varchar(500)  NOT NULL,
    [InspectionPassFail] bit  NOT NULL,
    [AccidentPreventable] bit  NOT NULL,
    [FollowupRequired] bit  NOT NULL,
    [FollowupDescription] varchar(500)  NULL,
    [FollowupDate] datetime  NULL,
    [FollowupCompletionDate] datetime  NULL,
    [FollowupComments] varchar(500)  NULL,
    [CloseDate] datetime  NULL,
    [BadgeID] varchar(50)  NULL,
    [InteractionDate] datetime  NULL,
    [VehicleNumber] varchar(50)  NULL,
    [BeatNumber] varchar(50)  NULL
);
GO

-- Creating table 'InsuranceCarriers'
CREATE TABLE [dbo].[InsuranceCarriers] (
    [InsuranceID] uniqueidentifier  NOT NULL,
    [CarrierName] varchar(100)  NOT NULL,
    [InsurancePolicyNumber] varchar(100)  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [PolicyName] varchar(100)  NOT NULL,
    [PhoneNumber] varchar(20)  NOT NULL,
    [Fax] varchar(20)  NOT NULL,
    [Email] varchar(50)  NOT NULL
);
GO

-- Creating table 'TruckAlerts'
CREATE TABLE [dbo].[TruckAlerts] (
    [AlertID] uniqueidentifier  NOT NULL,
    [DriverName] varchar(100)  NOT NULL,
    [ContractorCompany] varchar(100)  NOT NULL,
    [Beat] varchar(50)  NOT NULL,
    [AlertName] varchar(50)  NOT NULL,
    [AlertStart] datetime  NOT NULL,
    [AlertEnd] datetime  NULL,
    [AlertMins] int  NULL,
    [TruckNumber] varchar(100)  NOT NULL,
    [lat] float  NULL,
    [lon] float  NULL,
    [runID] uniqueidentifier  NULL,
    [ExcuseTime] datetime  NULL,
    [DismissTime] datetime  NULL,
    [ExcusedBy] varchar(100)  NULL,
    [Comment] varchar(1000)  NULL,
    [location] varchar(50)  NULL,
    [Speed] float  NULL,
    [Heading] char(1)  NULL,
    [CallSign] varchar(50)  NULL,
    [ScheduleID] uniqueidentifier  NULL,
    [ScheduleType] int  NULL
);
GO

-- Creating table 'CHPOfficerBeats'
CREATE TABLE [dbo].[CHPOfficerBeats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CHPOfficerId] int  NOT NULL,
    [BeatId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'CHPInspections'
CREATE TABLE [dbo].[CHPInspections] (
    [InspectionID] uniqueidentifier  NOT NULL,
    [FleetVehicleID] uniqueidentifier  NOT NULL,
    [InspectionDate] datetime  NOT NULL,
    [InspectionTypeID] uniqueidentifier  NOT NULL,
    [InspectionNotes] varchar(500)  NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [CHPOfficerId] int  NOT NULL
);
GO

-- Creating table 'CHPOfficers'
CREATE TABLE [dbo].[CHPOfficers] (
    [BadgeID] varchar(50)  NOT NULL,
    [OfficerLastName] varchar(50)  NOT NULL,
    [OfficerFirstName] varchar(50)  NOT NULL,
    [Email] varchar(50)  NULL,
    [Phone] varchar(50)  NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'MTCBeatsCallSigns'
CREATE TABLE [dbo].[MTCBeatsCallSigns] (
    [BeatID] int  NOT NULL,
    [CallSign] varchar(20)  NOT NULL
);
GO

-- Creating table 'Contracts'
CREATE TABLE [dbo].[Contracts] (
    [ContractID] uniqueidentifier  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [MaxObligation] decimal(19,4)  NOT NULL,
    [AgreementNumber] varchar(50)  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [TruckCount] int  NULL,
    [BackupTruckCount] int  NULL,
    [BeatId] uniqueidentifier  NULL
);
GO

-- Creating table 'VehicleTypeLUs'
CREATE TABLE [dbo].[VehicleTypeLUs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Code] nvarchar(150)  NULL,
    [CreatedOn] datetime  NOT NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL
);
GO

-- Creating table 'BeatsFreeways'
CREATE TABLE [dbo].[BeatsFreeways] (
    [BeatFreewayID] int IDENTITY(1,1) NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [FreewayID] int  NOT NULL,
    [Active] bit  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL
);
GO

-- Creating table 'OvertimeActivities'
CREATE TABLE [dbo].[OvertimeActivities] (
    [ID] uniqueidentifier  NOT NULL,
    [timeStamp] datetime  NOT NULL,
    [Shift] varchar(50)  NOT NULL,
    [CallSign] varchar(20)  NOT NULL,
    [OverTimeCode] varchar(50)  NULL,
    [BlocksApproved] int  NULL,
    [Beat] varchar(20)  NOT NULL,
    [Contractor] varchar(50)  NOT NULL,
    [Confirmed] bit  NULL
);
GO

-- Creating table 'BeatBeatSchedules'
CREATE TABLE [dbo].[BeatBeatSchedules] (
    [BeatBeatScheduleID] uniqueidentifier  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [BeatScheduleID] uniqueidentifier  NOT NULL,
    [NumberOfTrucks] int  NOT NULL
);
GO

-- Creating table 'BeatSchedules'
CREATE TABLE [dbo].[BeatSchedules] (
    [BeatScheduleID] uniqueidentifier  NOT NULL,
    [ScheduleName] varchar(50)  NOT NULL,
    [Sunday] bit  NULL,
    [Monday] bit  NULL,
    [Tuesday] bit  NULL,
    [Wednesday] bit  NULL,
    [Thursday] bit  NULL,
    [Friday] bit  NULL,
    [Saturday] bit  NULL,
    [StartTime] time  NULL,
    [EndTime] time  NULL,
    [NumberOfTrucks] int  NULL
);
GO

-- Creating table 'Appeals'
CREATE TABLE [dbo].[Appeals] (
    [AppealID] uniqueidentifier  NOT NULL,
    [AppealType] varchar(50)  NOT NULL,
    [AppealStatusID] uniqueidentifier  NOT NULL,
    [ContactName] varchar(50)  NOT NULL,
    [ContactPhone] varchar(50)  NOT NULL,
    [ContractorId] uniqueidentifier  NOT NULL,
    [Beatid] uniqueidentifier  NOT NULL,
    [DriverId] uniqueidentifier  NOT NULL,
    [V_ViolationId] int  NULL,
    [V_ReasonForAppeal] varchar(500)  NULL,
    [V_AppropriateCharge] decimal(18,2)  NULL,
    [O_Datetime] datetime  NULL,
    [O_CallSign] varchar(10)  NULL,
    [O_NumOfBlocks] int  NULL,
    [O_BlocksInitGranted] int  NULL,
    [O_Detail] varchar(500)  NULL,
    [I_EventDate] datetime  NULL,
    [I_InvoiceReason] varchar(500)  NULL,
    [I_AppealReason] varchar(500)  NULL,
    [I_InvoiceDeduction] decimal(18,0)  NULL,
    [I_AppealDeduction] decimal(18,0)  NULL,
    [MTCNote] varchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] varchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] varchar(50)  NOT NULL,
    [O_CHPOTNumber] int  NULL,
    [V_BlocksGranted] int  NULL,
    [V_BlocksClaimed] int  NULL
);
GO

-- Creating table 'AppealStatus'
CREATE TABLE [dbo].[AppealStatus] (
    [AppealStatusID] uniqueidentifier  NOT NULL,
    [AppealStatus] varchar(50)  NOT NULL
);
GO

-- Creating table 'BeatBeatSegments'
CREATE TABLE [dbo].[BeatBeatSegments] (
    [BeatBeatSegmentID] uniqueidentifier  NOT NULL,
    [BeatNumber] varchar(20)  NOT NULL,
    [BeatSegmentNumber] varchar(20)  NOT NULL,
    [ObjectID] varchar(10)  NULL
);
GO

-- Creating table 'MTC_Invoice_Addition'
CREATE TABLE [dbo].[MTC_Invoice_Addition] (
    [AdditionID] uniqueidentifier  NOT NULL,
    [InvoiceID] nvarchar(50)  NOT NULL,
    [Category] nvarchar(100)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [TimeAdded] int  NOT NULL,
    [Rate] float  NOT NULL,
    [Cost] float  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'MTC_Invoice_Deductions'
CREATE TABLE [dbo].[MTC_Invoice_Deductions] (
    [DeductionID] uniqueidentifier  NOT NULL,
    [InvoiceID] nvarchar(50)  NOT NULL,
    [Category] nvarchar(100)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Description] nvarchar(500)  NOT NULL,
    [TimeAdded] int  NOT NULL,
    [Rate] float  NOT NULL,
    [Cost] float  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'MTC_Invoice_Summary'
CREATE TABLE [dbo].[MTC_Invoice_Summary] (
    [SummaryID] uniqueidentifier  NOT NULL,
    [InvoiceID] nvarchar(50)  NOT NULL,
    [Row] nvarchar(50)  NOT NULL,
    [Days] float  NOT NULL,
    [Shifts] float  NOT NULL,
    [ContractHours] float  NOT NULL,
    [OnPatrolHours] float  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'MTC_Invoice_Anomalies'
CREATE TABLE [dbo].[MTC_Invoice_Anomalies] (
    [AnomalyID] uniqueidentifier  NOT NULL,
    [InvoiceID] nvarchar(50)  NOT NULL,
    [Description] nvarchar(150)  NOT NULL,
    [Date] datetime  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'TruckStatus'
CREATE TABLE [dbo].[TruckStatus] (
    [StatusID] uniqueidentifier  NOT NULL,
    [DriverName] varchar(100)  NOT NULL,
    [ContractorCompany] varchar(100)  NOT NULL,
    [Beat] varchar(50)  NOT NULL,
    [StatusName] varchar(50)  NOT NULL,
    [StatusStart] datetime  NOT NULL,
    [StatusEnd] datetime  NULL,
    [StatusMins] int  NULL,
    [TruckNumber] varchar(50)  NOT NULL,
    [lat] float  NULL,
    [lon] float  NULL,
    [runID] uniqueidentifier  NULL,
    [location] varchar(50)  NULL,
    [Speed] float  NULL,
    [Heading] char(1)  NULL,
    [ScheduleID] uniqueidentifier  NULL
);
GO

-- Creating table 'MTCRateTables'
CREATE TABLE [dbo].[MTCRateTables] (
    [RateTableID] uniqueidentifier  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [CurrentMonthRate] decimal(19,4)  NOT NULL,
    [p100] decimal(19,4)  NULL,
    [p150] decimal(19,4)  NULL,
    [p200] decimal(19,4)  NULL,
    [p250] decimal(19,4)  NULL,
    [p300] decimal(19,4)  NOT NULL,
    [p350] decimal(19,4)  NOT NULL,
    [p400] decimal(19,4)  NOT NULL,
    [p450] decimal(19,4)  NOT NULL,
    [p500] decimal(19,4)  NOT NULL,
    [p550] decimal(19,4)  NOT NULL,
    [p600] decimal(19,4)  NULL,
    [p650] decimal(19,4)  NULL,
    [p700] decimal(19,4)  NULL,
    [p750] decimal(19,4)  NULL,
    [p800] decimal(19,4)  NULL
);
GO

-- Creating table 'MTC_Invoice'
CREATE TABLE [dbo].[MTC_Invoice] (
    [InvoiceID] nvarchar(50)  NOT NULL,
    [ContractorID] uniqueidentifier  NOT NULL,
    [Month] int  NOT NULL,
    [BeatID] uniqueidentifier  NOT NULL,
    [FuelRate] float  NOT NULL,
    [BaseRate] float  NOT NULL,
    [Notes] nvarchar(500)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] nvarchar(50)  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [ModifiedBy] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'MTCActionTakens'
CREATE TABLE [dbo].[MTCActionTakens] (
    [MTCActionTakenID] uniqueidentifier  NOT NULL,
    [MTCAssistID] uniqueidentifier  NOT NULL,
    [ActionTaken] varchar(50)  NOT NULL
);
GO

-- Creating table 'MTCAssists'
CREATE TABLE [dbo].[MTCAssists] (
    [MTCAssistID] uniqueidentifier  NOT NULL,
    [IncidentID] uniqueidentifier  NOT NULL,
    [TrafficCollision] varchar(50)  NULL,
    [DebrisOnly] varchar(50)  NULL,
    [Breakdown] varchar(50)  NULL,
    [Other] varchar(50)  NULL,
    [ProblemNote] varchar(500)  NULL,
    [OtherNote] varchar(500)  NULL,
    [TransportType] varchar(500)  NULL,
    [StartODO] float  NULL,
    [EndODO] float  NULL,
    [DropSiteBeat] varchar(50)  NULL,
    [DropSite] varchar(50)  NULL,
    [State] varchar(10)  NULL,
    [LicensePlateNumber] varchar(20)  NULL,
    [VehicleType] varchar(20)  NULL,
    [OTAuthorizationNumber] varchar(50)  NULL,
    [DetailNote] varchar(500)  NULL,
    [Lat] float  NULL,
    [Lon] float  NULL,
    [DropSiteOther] varchar(100)  NULL,
    [datePosted] datetime  NULL,
    [CallSign] varchar(100)  NULL,
    [TimeOnInc] varchar(50)  NULL,
    [TimeOffInc] varchar(50)  NULL
);
GO

-- Creating table 'MTCIncidents'
CREATE TABLE [dbo].[MTCIncidents] (
    [IncidentID] uniqueidentifier  NOT NULL,
    [DatePosted] datetime  NOT NULL,
    [UserPosted] varchar(50)  NOT NULL,
    [fromTruck] int  NOT NULL,
    [Lat] float  NOT NULL,
    [Lon] float  NOT NULL,
    [Canceled] bit  NOT NULL,
    [Reason] varchar(50)  NULL,
    [Beat] varchar(50)  NULL,
    [TruckNumber] varchar(50)  NULL
);
GO

-- Creating table 'MTCPreAssists'
CREATE TABLE [dbo].[MTCPreAssists] (
    [PreAssistID] uniqueidentifier  NOT NULL,
    [IncidentID] uniqueidentifier  NOT NULL,
    [Direction] char(2)  NOT NULL,
    [FSPLocation] varchar(100)  NULL,
    [DispatchLocation] varchar(100)  NULL,
    [Position] varchar(10)  NOT NULL,
    [LaneNumber] varchar(50)  NULL,
    [CHPIncidentType] varchar(50)  NOT NULL,
    [CHPLogNumber] varchar(50)  NULL,
    [IncidentSurveyNumber] varchar(50)  NULL,
    [Lat] float  NULL,
    [Lon] float  NULL,
    [datePosted] datetime  NULL,
    [DispatchCode] varchar(50)  NULL,
    [Comments] varchar(500)  NULL,
    [CrossStreet] varchar(100)  NULL,
    [Freeway] varchar(50)  NULL,
    [DriverLastName] varchar(100)  NULL,
    [DriverFirstName] varchar(50)  NULL,
    [DriverID] uniqueidentifier  NULL,
    [RunID] uniqueidentifier  NULL
);
GO

-- Creating table 'Assists'
CREATE TABLE [dbo].[Assists] (
    [AssistID] uniqueidentifier  NOT NULL,
    [IncidentID] uniqueidentifier  NOT NULL,
    [AssistDatePosted] datetime  NOT NULL,
    [LastAssistInIncidentReport] bit  NOT NULL,
    [ProblemType] varchar(50)  NULL,
    [ProblemDetail] varchar(50)  NULL,
    [ProblemNote] varchar(500)  NULL,
    [OtherNote] varchar(500)  NULL,
    [TransportType] varchar(50)  NULL,
    [StartODO] float  NULL,
    [EndODO] float  NULL,
    [DropSite] varchar(50)  NULL,
    [State] varchar(10)  NULL,
    [LicensePlate] varchar(10)  NULL,
    [VehicleType] varchar(50)  NULL,
    [OTAuthorizationNumber] varchar(20)  NULL,
    [DetailNote] varchar(500)  NULL,
    [AssistLat] float  NULL,
    [AssistLon] float  NULL,
    [DropSiteOther] varchar(50)  NULL,
    [CallSign] varchar(20)  NULL,
    [TimeOnAssist] datetime  NULL,
    [TimeOffAssist] datetime  NULL,
    [ActionTaken] varchar(100)  NULL,
    [DropSiteBeat] varchar(50)  NULL,
    [PTN] varchar(10)  NULL
);
GO

-- Creating table 'Incidents'
CREATE TABLE [dbo].[Incidents] (
    [IncidentID] uniqueidentifier  NOT NULL,
    [IncidentDatePosted] datetime  NOT NULL,
    [UserPosted] varchar(50)  NOT NULL,
    [CallSign] varchar(20)  NOT NULL,
    [FromTruck] bit  NOT NULL,
    [Lat] float  NOT NULL,
    [Lon] float  NOT NULL,
    [Canceled] bit  NOT NULL,
    [ReasonCanceled] varchar(50)  NULL,
    [Beat] varchar(20)  NOT NULL,
    [TruckNumber] varchar(20)  NOT NULL,
    [LogID] uniqueidentifier  NULL,
    [WazeID] uniqueidentifier  NULL,
    [TruckStatusID] uniqueidentifier  NULL,
    [FSPLocation] varchar(200)  NOT NULL,
    [DispatchLocation] varchar(200)  NULL,
    [Direction] varchar(10)  NULL,
    [PositionIncident] varchar(10)  NULL,
    [LaneNumber] varchar(10)  NULL,
    [CHPIncidentType] varchar(50)  NULL,
    [briefUpdateLat] float  NULL,
    [briefUpdateLon] float  NULL,
    [Freeway] varchar(20)  NULL,
    [BriefUpdatePosted] bit  NOT NULL,
    [TimeofBriefUpdate] datetime  NULL,
    [CHPLogNumber] varchar(20)  NULL,
    [IncidentSurveyNumber] varchar(20)  NULL,
    [DriverLastName] varchar(50)  NULL,
    [DriverFIrstName] varchar(50)  NULL,
    [DriverID] uniqueidentifier  NULL,
    [RunID] uniqueidentifier  NULL,
    [TimeOnIncident] datetime  NULL,
    [TimeOffIncident] datetime  NULL,
    [Comment] varchar(500)  NULL,
    [Acked] bit  NULL,
    [CrossStreet] varchar(100)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AEReportTypeID] in table 'AEReportTypes'
ALTER TABLE [dbo].[AEReportTypes]
ADD CONSTRAINT [PK_AEReportTypes]
    PRIMARY KEY CLUSTERED ([AEReportTypeID] ASC);
GO

-- Creating primary key on [CallBoxID] in table 'CallBoxes'
ALTER TABLE [dbo].[CallBoxes]
ADD CONSTRAINT [PK_CallBoxes]
    PRIMARY KEY CLUSTERED ([CallBoxID] ASC);
GO

-- Creating primary key on [ContractorManagerID] in table 'ContractorManagers'
ALTER TABLE [dbo].[ContractorManagers]
ADD CONSTRAINT [PK_ContractorManagers]
    PRIMARY KEY CLUSTERED ([ContractorManagerID] ASC);
GO

-- Creating primary key on [ContractorID] in table 'Contractors'
ALTER TABLE [dbo].[Contractors]
ADD CONSTRAINT [PK_Contractors]
    PRIMARY KEY CLUSTERED ([ContractorID] ASC);
GO

-- Creating primary key on [ContractID], [BeatID] in table 'ContractsBeats'
ALTER TABLE [dbo].[ContractsBeats]
ADD CONSTRAINT [PK_ContractsBeats]
    PRIMARY KEY CLUSTERED ([ContractID], [BeatID] ASC);
GO

-- Creating primary key on [ID] in table 'DailyBeatSchedules'
ALTER TABLE [dbo].[DailyBeatSchedules]
ADD CONSTRAINT [PK_DailyBeatSchedules]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DriverEvents'
ALTER TABLE [dbo].[DriverEvents]
ADD CONSTRAINT [PK_DriverEvents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [DriverID] in table 'Drivers'
ALTER TABLE [dbo].[Drivers]
ADD CONSTRAINT [PK_Drivers]
    PRIMARY KEY CLUSTERED ([DriverID] ASC);
GO

-- Creating primary key on [FleetVehicleID] in table 'FleetVehicles'
ALTER TABLE [dbo].[FleetVehicles]
ADD CONSTRAINT [PK_FleetVehicles]
    PRIMARY KEY CLUSTERED ([FleetVehicleID] ASC);
GO

-- Creating primary key on [FreewayID] in table 'Freeways'
ALTER TABLE [dbo].[Freeways]
ADD CONSTRAINT [PK_Freeways]
    PRIMARY KEY CLUSTERED ([FreewayID] ASC);
GO

-- Creating primary key on [GPSID] in table 'GPSTrackingLogs'
ALTER TABLE [dbo].[GPSTrackingLogs]
ADD CONSTRAINT [PK_GPSTrackingLogs]
    PRIMARY KEY CLUSTERED ([GPSID] ASC);
GO

-- Creating primary key on [GroupID] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([GroupID] ASC);
GO

-- Creating primary key on [HeliosUnitID] in table 'HeliosUnits'
ALTER TABLE [dbo].[HeliosUnits]
ADD CONSTRAINT [PK_HeliosUnits]
    PRIMARY KEY CLUSTERED ([HeliosUnitID] ASC);
GO

-- Creating primary key on [InspectionTypeID] in table 'InspectionTypes'
ALTER TABLE [dbo].[InspectionTypes]
ADD CONSTRAINT [PK_InspectionTypes]
    PRIMARY KEY CLUSTERED ([InspectionTypeID] ASC);
GO

-- Creating primary key on [InteractionTypeID] in table 'InteractionTypes'
ALTER TABLE [dbo].[InteractionTypes]
ADD CONSTRAINT [PK_InteractionTypes]
    PRIMARY KEY CLUSTERED ([InteractionTypeID] ASC);
GO

-- Creating primary key on [LocationID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([LocationID] ASC);
GO

-- Creating primary key on [ReportID] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([ReportID] ASC);
GO

-- Creating primary key on [TowTruckYardID] in table 'TowTruckYards'
ALTER TABLE [dbo].[TowTruckYards]
ADD CONSTRAINT [PK_TowTruckYards]
    PRIMARY KEY CLUSTERED ([TowTruckYardID] ASC);
GO

-- Creating primary key on [TruckMessageID] in table 'TruckMessages'
ALTER TABLE [dbo].[TruckMessages]
ADD CONSTRAINT [PK_TruckMessages]
    PRIMARY KEY CLUSTERED ([TruckMessageID] ASC);
GO

-- Creating primary key on [VarID] in table 'Vars'
ALTER TABLE [dbo].[Vars]
ADD CONSTRAINT [PK_Vars]
    PRIMARY KEY CLUSTERED ([VarID] ASC);
GO

-- Creating primary key on [DateID] in table 'YearlyCalendars'
ALTER TABLE [dbo].[YearlyCalendars]
ADD CONSTRAINT [PK_YearlyCalendars]
    PRIMARY KEY CLUSTERED ([DateID] ASC);
GO

-- Creating primary key on [AEFrequencyID], [AEFrequencyName] in table 'AEFrequencies'
ALTER TABLE [dbo].[AEFrequencies]
ADD CONSTRAINT [PK_AEFrequencies]
    PRIMARY KEY CLUSTERED ([AEFrequencyID], [AEFrequencyName] ASC);
GO

-- Creating primary key on [AERecipientID], [AERecipientEmail], [AEIsContractor], [AEReportID], [AEFrequencyID], [AEReportTypeID] in table 'AERecipients'
ALTER TABLE [dbo].[AERecipients]
ADD CONSTRAINT [PK_AERecipients]
    PRIMARY KEY CLUSTERED ([AERecipientID], [AERecipientEmail], [AEIsContractor], [AEReportID], [AEFrequencyID], [AEReportTypeID] ASC);
GO

-- Creating primary key on [AEReportID], [AEReportName] in table 'AEReports'
ALTER TABLE [dbo].[AEReports]
ADD CONSTRAINT [PK_AEReports]
    PRIMARY KEY CLUSTERED ([AEReportID], [AEReportName] ASC);
GO

-- Creating primary key on [MountedSecurely], [ConnectedToCell], [DCPowerConnected], [RouterUnitMountedSecurely], [MoistureFree], [Speedtest], [GPSSentProperly], [InstallerName], [InstallationDate], [TowTruckCompany], [VehicleID], [SystemSerialNumber], [IPAddress] in table 'TowTruckSetups'
ALTER TABLE [dbo].[TowTruckSetups]
ADD CONSTRAINT [PK_TowTruckSetups]
    PRIMARY KEY CLUSTERED ([MountedSecurely], [ConnectedToCell], [DCPowerConnected], [RouterUnitMountedSecurely], [MoistureFree], [Speedtest], [GPSSentProperly], [InstallerName], [InstallationDate], [TowTruckCompany], [VehicleID], [SystemSerialNumber], [IPAddress] ASC);
GO

-- Creating primary key on [TruckStateID], [TruckState1] in table 'TruckStates'
ALTER TABLE [dbo].[TruckStates]
ADD CONSTRAINT [PK_TruckStates]
    PRIMARY KEY CLUSTERED ([TruckStateID], [TruckState1] ASC);
GO

-- Creating primary key on [ContractorTypeId] in table 'ContractorTypes'
ALTER TABLE [dbo].[ContractorTypes]
ADD CONSTRAINT [PK_ContractorTypes]
    PRIMARY KEY CLUSTERED ([ContractorTypeId] ASC);
GO

-- Creating primary key on [BeatID] in table 'Beats'
ALTER TABLE [dbo].[Beats]
ADD CONSTRAINT [PK_Beats]
    PRIMARY KEY CLUSTERED ([BeatID] ASC);
GO

-- Creating primary key on [ScheduleID] in table 'MTCSchedules'
ALTER TABLE [dbo].[MTCSchedules]
ADD CONSTRAINT [PK_MTCSchedules]
    PRIMARY KEY CLUSTERED ([ScheduleID] ASC);
GO

-- Creating primary key on [InteractionID] in table 'DriverInteractions'
ALTER TABLE [dbo].[DriverInteractions]
ADD CONSTRAINT [PK_DriverInteractions]
    PRIMARY KEY CLUSTERED ([InteractionID] ASC);
GO

-- Creating primary key on [InsuranceID] in table 'InsuranceCarriers'
ALTER TABLE [dbo].[InsuranceCarriers]
ADD CONSTRAINT [PK_InsuranceCarriers]
    PRIMARY KEY CLUSTERED ([InsuranceID] ASC);
GO

-- Creating primary key on [AlertID] in table 'TruckAlerts'
ALTER TABLE [dbo].[TruckAlerts]
ADD CONSTRAINT [PK_TruckAlerts]
    PRIMARY KEY CLUSTERED ([AlertID] ASC);
GO

-- Creating primary key on [Id] in table 'CHPOfficerBeats'
ALTER TABLE [dbo].[CHPOfficerBeats]
ADD CONSTRAINT [PK_CHPOfficerBeats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [InspectionID] in table 'CHPInspections'
ALTER TABLE [dbo].[CHPInspections]
ADD CONSTRAINT [PK_CHPInspections]
    PRIMARY KEY CLUSTERED ([InspectionID] ASC);
GO

-- Creating primary key on [Id] in table 'CHPOfficers'
ALTER TABLE [dbo].[CHPOfficers]
ADD CONSTRAINT [PK_CHPOfficers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [BeatID], [CallSign] in table 'MTCBeatsCallSigns'
ALTER TABLE [dbo].[MTCBeatsCallSigns]
ADD CONSTRAINT [PK_MTCBeatsCallSigns]
    PRIMARY KEY CLUSTERED ([BeatID], [CallSign] ASC);
GO

-- Creating primary key on [ContractID] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [PK_Contracts]
    PRIMARY KEY CLUSTERED ([ContractID] ASC);
GO

-- Creating primary key on [Id] in table 'VehicleTypeLUs'
ALTER TABLE [dbo].[VehicleTypeLUs]
ADD CONSTRAINT [PK_VehicleTypeLUs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [BeatFreewayID] in table 'BeatsFreeways'
ALTER TABLE [dbo].[BeatsFreeways]
ADD CONSTRAINT [PK_BeatsFreeways]
    PRIMARY KEY CLUSTERED ([BeatFreewayID] ASC);
GO

-- Creating primary key on [ID] in table 'OvertimeActivities'
ALTER TABLE [dbo].[OvertimeActivities]
ADD CONSTRAINT [PK_OvertimeActivities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [BeatBeatScheduleID] in table 'BeatBeatSchedules'
ALTER TABLE [dbo].[BeatBeatSchedules]
ADD CONSTRAINT [PK_BeatBeatSchedules]
    PRIMARY KEY CLUSTERED ([BeatBeatScheduleID] ASC);
GO

-- Creating primary key on [BeatScheduleID] in table 'BeatSchedules'
ALTER TABLE [dbo].[BeatSchedules]
ADD CONSTRAINT [PK_BeatSchedules]
    PRIMARY KEY CLUSTERED ([BeatScheduleID] ASC);
GO

-- Creating primary key on [AppealID] in table 'Appeals'
ALTER TABLE [dbo].[Appeals]
ADD CONSTRAINT [PK_Appeals]
    PRIMARY KEY CLUSTERED ([AppealID] ASC);
GO

-- Creating primary key on [AppealStatusID] in table 'AppealStatus'
ALTER TABLE [dbo].[AppealStatus]
ADD CONSTRAINT [PK_AppealStatus]
    PRIMARY KEY CLUSTERED ([AppealStatusID] ASC);
GO

-- Creating primary key on [BeatBeatSegmentID] in table 'BeatBeatSegments'
ALTER TABLE [dbo].[BeatBeatSegments]
ADD CONSTRAINT [PK_BeatBeatSegments]
    PRIMARY KEY CLUSTERED ([BeatBeatSegmentID] ASC);
GO

-- Creating primary key on [AdditionID] in table 'MTC_Invoice_Addition'
ALTER TABLE [dbo].[MTC_Invoice_Addition]
ADD CONSTRAINT [PK_MTC_Invoice_Addition]
    PRIMARY KEY CLUSTERED ([AdditionID] ASC);
GO

-- Creating primary key on [DeductionID] in table 'MTC_Invoice_Deductions'
ALTER TABLE [dbo].[MTC_Invoice_Deductions]
ADD CONSTRAINT [PK_MTC_Invoice_Deductions]
    PRIMARY KEY CLUSTERED ([DeductionID] ASC);
GO

-- Creating primary key on [SummaryID] in table 'MTC_Invoice_Summary'
ALTER TABLE [dbo].[MTC_Invoice_Summary]
ADD CONSTRAINT [PK_MTC_Invoice_Summary]
    PRIMARY KEY CLUSTERED ([SummaryID] ASC);
GO

-- Creating primary key on [AnomalyID] in table 'MTC_Invoice_Anomalies'
ALTER TABLE [dbo].[MTC_Invoice_Anomalies]
ADD CONSTRAINT [PK_MTC_Invoice_Anomalies]
    PRIMARY KEY CLUSTERED ([AnomalyID] ASC);
GO

-- Creating primary key on [StatusID] in table 'TruckStatus'
ALTER TABLE [dbo].[TruckStatus]
ADD CONSTRAINT [PK_TruckStatus]
    PRIMARY KEY CLUSTERED ([StatusID] ASC);
GO

-- Creating primary key on [RateTableID] in table 'MTCRateTables'
ALTER TABLE [dbo].[MTCRateTables]
ADD CONSTRAINT [PK_MTCRateTables]
    PRIMARY KEY CLUSTERED ([RateTableID] ASC);
GO

-- Creating primary key on [InvoiceID] in table 'MTC_Invoice'
ALTER TABLE [dbo].[MTC_Invoice]
ADD CONSTRAINT [PK_MTC_Invoice]
    PRIMARY KEY CLUSTERED ([InvoiceID] ASC);
GO

-- Creating primary key on [MTCActionTakenID] in table 'MTCActionTakens'
ALTER TABLE [dbo].[MTCActionTakens]
ADD CONSTRAINT [PK_MTCActionTakens]
    PRIMARY KEY CLUSTERED ([MTCActionTakenID] ASC);
GO

-- Creating primary key on [MTCAssistID] in table 'MTCAssists'
ALTER TABLE [dbo].[MTCAssists]
ADD CONSTRAINT [PK_MTCAssists]
    PRIMARY KEY CLUSTERED ([MTCAssistID] ASC);
GO

-- Creating primary key on [IncidentID] in table 'MTCIncidents'
ALTER TABLE [dbo].[MTCIncidents]
ADD CONSTRAINT [PK_MTCIncidents]
    PRIMARY KEY CLUSTERED ([IncidentID] ASC);
GO

-- Creating primary key on [PreAssistID] in table 'MTCPreAssists'
ALTER TABLE [dbo].[MTCPreAssists]
ADD CONSTRAINT [PK_MTCPreAssists]
    PRIMARY KEY CLUSTERED ([PreAssistID] ASC);
GO

-- Creating primary key on [AssistID] in table 'Assists'
ALTER TABLE [dbo].[Assists]
ADD CONSTRAINT [PK_Assists]
    PRIMARY KEY CLUSTERED ([AssistID] ASC);
GO

-- Creating primary key on [IncidentID] in table 'Incidents'
ALTER TABLE [dbo].[Incidents]
ADD CONSTRAINT [PK_Incidents]
    PRIMARY KEY CLUSTERED ([IncidentID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ContractorID] in table 'ContractorManagers'
ALTER TABLE [dbo].[ContractorManagers]
ADD CONSTRAINT [FK_ContractorManagers_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContractorManagers_Contractors'
CREATE INDEX [IX_FK_ContractorManagers_Contractors]
ON [dbo].[ContractorManagers]
    ([ContractorID]);
GO

-- Creating foreign key on [ContractorID] in table 'Drivers'
ALTER TABLE [dbo].[Drivers]
ADD CONSTRAINT [FK_Drivers_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Drivers_Contractors'
CREATE INDEX [IX_FK_Drivers_Contractors]
ON [dbo].[Drivers]
    ([ContractorID]);
GO

-- Creating foreign key on [ContractorID] in table 'FleetVehicles'
ALTER TABLE [dbo].[FleetVehicles]
ADD CONSTRAINT [FK_FleetVehicles_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FleetVehicles_Contractors'
CREATE INDEX [IX_FK_FleetVehicles_Contractors]
ON [dbo].[FleetVehicles]
    ([ContractorID]);
GO

-- Creating foreign key on [ContractorTypeId] in table 'Contractors'
ALTER TABLE [dbo].[Contractors]
ADD CONSTRAINT [FK_Contractors_ContractorTypes]
    FOREIGN KEY ([ContractorTypeId])
    REFERENCES [dbo].[ContractorTypes]
        ([ContractorTypeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contractors_ContractorTypes'
CREATE INDEX [IX_FK_Contractors_ContractorTypes]
ON [dbo].[Contractors]
    ([ContractorTypeId]);
GO

-- Creating foreign key on [ContractorID] in table 'DriverInteractions'
ALTER TABLE [dbo].[DriverInteractions]
ADD CONSTRAINT [FK_DriverInteractions_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DriverInteractions_Contractors'
CREATE INDEX [IX_FK_DriverInteractions_Contractors]
ON [dbo].[DriverInteractions]
    ([ContractorID]);
GO

-- Creating foreign key on [DriverID] in table 'DriverInteractions'
ALTER TABLE [dbo].[DriverInteractions]
ADD CONSTRAINT [FK_DriverInteractions_Drivers]
    FOREIGN KEY ([DriverID])
    REFERENCES [dbo].[Drivers]
        ([DriverID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DriverInteractions_Drivers'
CREATE INDEX [IX_FK_DriverInteractions_Drivers]
ON [dbo].[DriverInteractions]
    ([DriverID]);
GO

-- Creating foreign key on [InteractionTypeID] in table 'DriverInteractions'
ALTER TABLE [dbo].[DriverInteractions]
ADD CONSTRAINT [FK_DriverInteractions_InteractionTypes]
    FOREIGN KEY ([InteractionTypeID])
    REFERENCES [dbo].[InteractionTypes]
        ([InteractionTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DriverInteractions_InteractionTypes'
CREATE INDEX [IX_FK_DriverInteractions_InteractionTypes]
ON [dbo].[DriverInteractions]
    ([InteractionTypeID]);
GO

-- Creating foreign key on [CHPOfficerId] in table 'CHPInspections'
ALTER TABLE [dbo].[CHPInspections]
ADD CONSTRAINT [FK_CHPInspections_CHPOfficer]
    FOREIGN KEY ([CHPOfficerId])
    REFERENCES [dbo].[CHPOfficers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CHPInspections_CHPOfficer'
CREATE INDEX [IX_FK_CHPInspections_CHPOfficer]
ON [dbo].[CHPInspections]
    ([CHPOfficerId]);
GO

-- Creating foreign key on [ContractorID] in table 'CHPInspections'
ALTER TABLE [dbo].[CHPInspections]
ADD CONSTRAINT [FK_CHPInspections_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CHPInspections_Contractors'
CREATE INDEX [IX_FK_CHPInspections_Contractors]
ON [dbo].[CHPInspections]
    ([ContractorID]);
GO

-- Creating foreign key on [FleetVehicleID] in table 'CHPInspections'
ALTER TABLE [dbo].[CHPInspections]
ADD CONSTRAINT [FK_CHPInspections_FleetVehicles]
    FOREIGN KEY ([FleetVehicleID])
    REFERENCES [dbo].[FleetVehicles]
        ([FleetVehicleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CHPInspections_FleetVehicles'
CREATE INDEX [IX_FK_CHPInspections_FleetVehicles]
ON [dbo].[CHPInspections]
    ([FleetVehicleID]);
GO

-- Creating foreign key on [InspectionTypeID] in table 'CHPInspections'
ALTER TABLE [dbo].[CHPInspections]
ADD CONSTRAINT [FK_CHPInspections_InspectionTypes]
    FOREIGN KEY ([InspectionTypeID])
    REFERENCES [dbo].[InspectionTypes]
        ([InspectionTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CHPInspections_InspectionTypes'
CREATE INDEX [IX_FK_CHPInspections_InspectionTypes]
ON [dbo].[CHPInspections]
    ([InspectionTypeID]);
GO

-- Creating foreign key on [ContractorID] in table 'Contracts'
ALTER TABLE [dbo].[Contracts]
ADD CONSTRAINT [FK_Contracts_Contractors]
    FOREIGN KEY ([ContractorID])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contracts_Contractors'
CREATE INDEX [IX_FK_Contracts_Contractors]
ON [dbo].[Contracts]
    ([ContractorID]);
GO

-- Creating foreign key on [BeatID] in table 'BeatsFreeways'
ALTER TABLE [dbo].[BeatsFreeways]
ADD CONSTRAINT [FK_BeatsFreeways_Beats]
    FOREIGN KEY ([BeatID])
    REFERENCES [dbo].[Beats]
        ([BeatID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BeatsFreeways_Beats'
CREATE INDEX [IX_FK_BeatsFreeways_Beats]
ON [dbo].[BeatsFreeways]
    ([BeatID]);
GO

-- Creating foreign key on [FreewayID] in table 'BeatsFreeways'
ALTER TABLE [dbo].[BeatsFreeways]
ADD CONSTRAINT [FK_BeatsFreeways_Freeways]
    FOREIGN KEY ([FreewayID])
    REFERENCES [dbo].[Freeways]
        ([FreewayID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BeatsFreeways_Freeways'
CREATE INDEX [IX_FK_BeatsFreeways_Freeways]
ON [dbo].[BeatsFreeways]
    ([FreewayID]);
GO

-- Creating foreign key on [AppealStatusID] in table 'Appeals'
ALTER TABLE [dbo].[Appeals]
ADD CONSTRAINT [FK_Appeals_AppealStatus]
    FOREIGN KEY ([AppealStatusID])
    REFERENCES [dbo].[AppealStatus]
        ([AppealStatusID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Appeals_AppealStatus'
CREATE INDEX [IX_FK_Appeals_AppealStatus]
ON [dbo].[Appeals]
    ([AppealStatusID]);
GO

-- Creating foreign key on [Beatid] in table 'Appeals'
ALTER TABLE [dbo].[Appeals]
ADD CONSTRAINT [FK_Appeals_Beats]
    FOREIGN KEY ([Beatid])
    REFERENCES [dbo].[Beats]
        ([BeatID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Appeals_Beats'
CREATE INDEX [IX_FK_Appeals_Beats]
ON [dbo].[Appeals]
    ([Beatid]);
GO

-- Creating foreign key on [ContractorId] in table 'Appeals'
ALTER TABLE [dbo].[Appeals]
ADD CONSTRAINT [FK_Appeals_Contractors]
    FOREIGN KEY ([ContractorId])
    REFERENCES [dbo].[Contractors]
        ([ContractorID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Appeals_Contractors'
CREATE INDEX [IX_FK_Appeals_Contractors]
ON [dbo].[Appeals]
    ([ContractorId]);
GO

-- Creating foreign key on [DriverId] in table 'Appeals'
ALTER TABLE [dbo].[Appeals]
ADD CONSTRAINT [FK_Appeals_Drivers]
    FOREIGN KEY ([DriverId])
    REFERENCES [dbo].[Drivers]
        ([DriverID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Appeals_Drivers'
CREATE INDEX [IX_FK_Appeals_Drivers]
ON [dbo].[Appeals]
    ([DriverId]);
GO

-- Creating foreign key on [RateTableID] in table 'MTCRateTables'
ALTER TABLE [dbo].[MTCRateTables]
ADD CONSTRAINT [FK_MTCRateTable_MTCRateTable]
    FOREIGN KEY ([RateTableID])
    REFERENCES [dbo].[MTCRateTables]
        ([RateTableID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [InvoiceID] in table 'MTC_Invoice_Addition'
ALTER TABLE [dbo].[MTC_Invoice_Addition]
ADD CONSTRAINT [FK_MTC_Invoice_Addition_MTC_Invoice]
    FOREIGN KEY ([InvoiceID])
    REFERENCES [dbo].[MTC_Invoice]
        ([InvoiceID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTC_Invoice_Addition_MTC_Invoice'
CREATE INDEX [IX_FK_MTC_Invoice_Addition_MTC_Invoice]
ON [dbo].[MTC_Invoice_Addition]
    ([InvoiceID]);
GO

-- Creating foreign key on [InvoiceID] in table 'MTC_Invoice_Anomalies'
ALTER TABLE [dbo].[MTC_Invoice_Anomalies]
ADD CONSTRAINT [FK_MTC_Invoice_Anomalies_MTC_Invoice]
    FOREIGN KEY ([InvoiceID])
    REFERENCES [dbo].[MTC_Invoice]
        ([InvoiceID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTC_Invoice_Anomalies_MTC_Invoice'
CREATE INDEX [IX_FK_MTC_Invoice_Anomalies_MTC_Invoice]
ON [dbo].[MTC_Invoice_Anomalies]
    ([InvoiceID]);
GO

-- Creating foreign key on [InvoiceID] in table 'MTC_Invoice_Deductions'
ALTER TABLE [dbo].[MTC_Invoice_Deductions]
ADD CONSTRAINT [FK_MTC_Invoice_Deductions_MTC_Invoice]
    FOREIGN KEY ([InvoiceID])
    REFERENCES [dbo].[MTC_Invoice]
        ([InvoiceID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTC_Invoice_Deductions_MTC_Invoice'
CREATE INDEX [IX_FK_MTC_Invoice_Deductions_MTC_Invoice]
ON [dbo].[MTC_Invoice_Deductions]
    ([InvoiceID]);
GO

-- Creating foreign key on [InvoiceID] in table 'MTC_Invoice_Summary'
ALTER TABLE [dbo].[MTC_Invoice_Summary]
ADD CONSTRAINT [FK_MTC_Invoice_Summary_MTC_Invoice]
    FOREIGN KEY ([InvoiceID])
    REFERENCES [dbo].[MTC_Invoice]
        ([InvoiceID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTC_Invoice_Summary_MTC_Invoice'
CREATE INDEX [IX_FK_MTC_Invoice_Summary_MTC_Invoice]
ON [dbo].[MTC_Invoice_Summary]
    ([InvoiceID]);
GO

-- Creating foreign key on [MTCAssistID] in table 'MTCActionTakens'
ALTER TABLE [dbo].[MTCActionTakens]
ADD CONSTRAINT [FK_MTCActionTaken_MTCAssists]
    FOREIGN KEY ([MTCAssistID])
    REFERENCES [dbo].[MTCAssists]
        ([MTCAssistID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTCActionTaken_MTCAssists'
CREATE INDEX [IX_FK_MTCActionTaken_MTCAssists]
ON [dbo].[MTCActionTakens]
    ([MTCAssistID]);
GO

-- Creating foreign key on [IncidentID] in table 'MTCAssists'
ALTER TABLE [dbo].[MTCAssists]
ADD CONSTRAINT [FK_MTCAssists_MTCIncidents]
    FOREIGN KEY ([IncidentID])
    REFERENCES [dbo].[MTCIncidents]
        ([IncidentID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTCAssists_MTCIncidents'
CREATE INDEX [IX_FK_MTCAssists_MTCIncidents]
ON [dbo].[MTCAssists]
    ([IncidentID]);
GO

-- Creating foreign key on [DriverID] in table 'MTCPreAssists'
ALTER TABLE [dbo].[MTCPreAssists]
ADD CONSTRAINT [FK_MTCPreAssists_Drivers]
    FOREIGN KEY ([DriverID])
    REFERENCES [dbo].[Drivers]
        ([DriverID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTCPreAssists_Drivers'
CREATE INDEX [IX_FK_MTCPreAssists_Drivers]
ON [dbo].[MTCPreAssists]
    ([DriverID]);
GO

-- Creating foreign key on [IncidentID] in table 'MTCPreAssists'
ALTER TABLE [dbo].[MTCPreAssists]
ADD CONSTRAINT [FK_MTCPreAssists_MTCIncidents]
    FOREIGN KEY ([IncidentID])
    REFERENCES [dbo].[MTCIncidents]
        ([IncidentID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MTCPreAssists_MTCIncidents'
CREATE INDEX [IX_FK_MTCPreAssists_MTCIncidents]
ON [dbo].[MTCPreAssists]
    ([IncidentID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------