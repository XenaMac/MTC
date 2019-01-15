using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class CHPInspectionMap : EntityTypeConfiguration<CHPInspection>
    {
        public CHPInspectionMap()
        {
            // Primary Key
            this.HasKey(t => t.InspectionID);

            // Properties
            this.Property(t => t.BadgeID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InspectionNotes)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("CHPInspections");
            this.Property(t => t.InspectionID).HasColumnName("InspectionID");
            this.Property(t => t.FleetVehicleID).HasColumnName("FleetVehicleID");
            this.Property(t => t.BadgeID).HasColumnName("BadgeID");
            this.Property(t => t.InspectionDate).HasColumnName("InspectionDate");
            this.Property(t => t.InspectionTypeID).HasColumnName("InspectionTypeID");
            this.Property(t => t.InspectionNotes).HasColumnName("InspectionNotes");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");

            // Relationships
            this.HasRequired(t => t.CHPOfficer)
                .WithMany(t => t.CHPInspections)
                .HasForeignKey(d => d.BadgeID);
            this.HasRequired(t => t.CHPOfficer1)
                .WithMany(t => t.CHPInspections1)
                .HasForeignKey(d => d.BadgeID);
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.CHPInspections)
                .HasForeignKey(d => d.ContractorID);
            this.HasRequired(t => t.FleetVehicle)
                .WithMany(t => t.CHPInspections)
                .HasForeignKey(d => d.FleetVehicleID);
            this.HasRequired(t => t.InspectionType)
                .WithMany(t => t.CHPInspections)
                .HasForeignKey(d => d.InspectionTypeID);

        }
    }
}
