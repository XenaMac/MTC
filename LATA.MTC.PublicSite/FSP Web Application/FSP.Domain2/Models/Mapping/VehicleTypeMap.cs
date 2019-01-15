using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class VehicleTypeMap : EntityTypeConfiguration<VehicleType>
    {
        public VehicleTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.VehicleTypeID);

            // Properties
            this.Property(t => t.VehicleTypeCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.VehicleType1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("VehicleTypes");
            this.Property(t => t.VehicleTypeID).HasColumnName("VehicleTypeID");
            this.Property(t => t.VehicleTypeCode).HasColumnName("VehicleTypeCode");
            this.Property(t => t.VehicleType1).HasColumnName("VehicleType");
        }
    }
}
