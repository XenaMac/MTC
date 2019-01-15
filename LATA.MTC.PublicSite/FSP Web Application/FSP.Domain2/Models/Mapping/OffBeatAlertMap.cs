using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class OffBeatAlertMap : EntityTypeConfiguration<OffBeatAlert>
    {
        public OffBeatAlertMap()
        {
            // Primary Key
            this.HasKey(t => t.AlertID);

            // Properties
            this.Property(t => t.VehicleNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OffBeatAlerts");
            this.Property(t => t.AlertID).HasColumnName("AlertID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.VehicleNumber).HasColumnName("VehicleNumber");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.OffBeatTime).HasColumnName("OffBeatTime");
        }
    }
}
