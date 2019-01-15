using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DropZoneMap : EntityTypeConfiguration<DropZone>
    {
        public DropZoneMap()
        {
            // Primary Key
            this.HasKey(t => t.DropZoneID);

            // Properties
            this.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Comments)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.Restrictions)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.DropZoneNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DropZoneDescription)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PDPhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("DropZones");
            this.Property(t => t.DropZoneID).HasColumnName("DropZoneID");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Restrictions).HasColumnName("Restrictions");
            this.Property(t => t.DropZoneNumber).HasColumnName("DropZoneNumber");
            this.Property(t => t.DropZoneDescription).HasColumnName("DropZoneDescription");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.PDPhoneNumber).HasColumnName("PDPhoneNumber");
            this.Property(t => t.Capacity).HasColumnName("Capacity");
            this.Property(t => t.Position).HasColumnName("Position");
        }
    }
}
