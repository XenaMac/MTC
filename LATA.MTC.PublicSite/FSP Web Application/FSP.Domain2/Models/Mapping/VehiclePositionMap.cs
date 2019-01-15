using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class VehiclePositionMap : EntityTypeConfiguration<VehiclePosition>
    {
        public VehiclePositionMap()
        {
            // Primary Key
            this.HasKey(t => t.VehiclePositionID);

            // Properties
            this.Property(t => t.VehiclePositionCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.VehiclePosition1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("VehiclePositions");
            this.Property(t => t.VehiclePositionID).HasColumnName("VehiclePositionID");
            this.Property(t => t.VehiclePositionCode).HasColumnName("VehiclePositionCode");
            this.Property(t => t.VehiclePosition1).HasColumnName("VehiclePosition");
        }
    }
}
