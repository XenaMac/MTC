using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class IncidentMap : EntityTypeConfiguration<Incident>
    {
        public IncidentMap()
        {
            // Primary Key
            this.HasKey(t => t.IncidentID);

            // Properties
            this.Property(t => t.ApproximateLocation)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.IncidentNumber)
                .HasMaxLength(50);

            this.Property(t => t.Location)
                .HasMaxLength(100);

            this.Property(t => t.CrossStreet1)
                .HasMaxLength(100);

            this.Property(t => t.CrossStreet2)
                .HasMaxLength(100);

            this.Property(t => t.Direction)
                .HasMaxLength(50);

            this.Property(t => t.BeatNumber)
                .HasMaxLength(50);

            this.Property(t => t.DispatchNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Incidents");
            this.Property(t => t.IncidentID).HasColumnName("IncidentID");
            this.Property(t => t.ApproximateLocation).HasColumnName("ApproximateLocation");
            this.Property(t => t.FreewayID).HasColumnName("FreewayID");
            this.Property(t => t.LocationID).HasColumnName("LocationID");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.DateStamp).HasColumnName("DateStamp");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IncidentNumber).HasColumnName("IncidentNumber");
            this.Property(t => t.LastModified).HasColumnName("LastModified");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.CrossStreet1).HasColumnName("CrossStreet1");
            this.Property(t => t.CrossStreet2).HasColumnName("CrossStreet2");
            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.BeatNumber).HasColumnName("BeatNumber");
            this.Property(t => t.DispatchNumber).HasColumnName("DispatchNumber");

            // Relationships
            this.HasRequired(t => t.Freeway)
                .WithMany(t => t.Incidents)
                .HasForeignKey(d => d.FreewayID);
            this.HasOptional(t => t.Location1)
                .WithMany(t => t.Incidents)
                .HasForeignKey(d => d.LocationID);

        }
    }
}
