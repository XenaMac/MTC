using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class AssistMap : EntityTypeConfiguration<Assist>
    {
        public AssistMap()
        {
            // Primary Key
            this.HasKey(t => t.AssistID);

            // Properties
            this.Property(t => t.DropZone)
                .HasMaxLength(50);

            this.Property(t => t.Make)
                .HasMaxLength(50);

            this.Property(t => t.Color)
                .HasMaxLength(20);

            this.Property(t => t.LicensePlate)
                .HasMaxLength(20);

            this.Property(t => t.State)
                .HasMaxLength(20);

            this.Property(t => t.Tip)
                .HasMaxLength(50);

            this.Property(t => t.TipDetail)
                .HasMaxLength(50);

            this.Property(t => t.CustomerLastName)
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.LogNumber)
                .HasMaxLength(50);

            this.Property(t => t.SurveyNum)
                .HasMaxLength(100);

            this.Property(t => t.AssistNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Assists");
            this.Property(t => t.AssistID).HasColumnName("AssistID");
            this.Property(t => t.IncidentID).HasColumnName("IncidentID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.FleetVehicleID).HasColumnName("FleetVehicleID");
            this.Property(t => t.DispatchTime).HasColumnName("DispatchTime");
            this.Property(t => t.CustomerWaitTime).HasColumnName("CustomerWaitTime");
            this.Property(t => t.VehiclePositionID).HasColumnName("VehiclePositionID");
            this.Property(t => t.IncidentTypeID).HasColumnName("IncidentTypeID");
            this.Property(t => t.TrafficSpeedID).HasColumnName("TrafficSpeedID");
            this.Property(t => t.ServiceTypeID).HasColumnName("ServiceTypeID");
            this.Property(t => t.DropZone).HasColumnName("DropZone");
            this.Property(t => t.Make).HasColumnName("Make");
            this.Property(t => t.VehicleTypeID).HasColumnName("VehicleTypeID");
            this.Property(t => t.Color).HasColumnName("Color");
            this.Property(t => t.LicensePlate).HasColumnName("LicensePlate");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.StartOD).HasColumnName("StartOD");
            this.Property(t => t.EndOD).HasColumnName("EndOD");
            this.Property(t => t.TowLocationID).HasColumnName("TowLocationID");
            this.Property(t => t.Tip).HasColumnName("Tip");
            this.Property(t => t.TipDetail).HasColumnName("TipDetail");
            this.Property(t => t.CustomerLastName).HasColumnName("CustomerLastName");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.IsMDC).HasColumnName("IsMDC");
            this.Property(t => t.x1097).HasColumnName("x1097");
            this.Property(t => t.x1098).HasColumnName("x1098");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.LogNumber).HasColumnName("LogNumber");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Lon).HasColumnName("Lon");
            this.Property(t => t.OnSiteTime).HasColumnName("OnSiteTime");
            this.Property(t => t.SurveyNum).HasColumnName("SurveyNum");
            this.Property(t => t.AssistNumber).HasColumnName("AssistNumber");

            // Relationships
            this.HasRequired(t => t.Incident)
                .WithMany(t => t.Assists)
                .HasForeignKey(d => d.IncidentID);

        }
    }
}
