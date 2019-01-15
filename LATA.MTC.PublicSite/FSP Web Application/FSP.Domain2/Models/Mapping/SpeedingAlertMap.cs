using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class SpeedingAlertMap : EntityTypeConfiguration<SpeedingAlert>
    {
        public SpeedingAlertMap()
        {
            // Primary Key
            this.HasKey(t => t.AlarmID);

            // Properties
            this.Property(t => t.VehicleNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SpeedingAlerts");
            this.Property(t => t.AlarmID).HasColumnName("AlarmID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.VehicleNumber).HasColumnName("VehicleNumber");
            this.Property(t => t.LoggedSpeed).HasColumnName("LoggedSpeed");
            this.Property(t => t.MaxSpeed).HasColumnName("MaxSpeed");
            this.Property(t => t.SpeedingTime).HasColumnName("SpeedingTime");
            this.Property(t => t.Location).HasColumnName("Location");
        }
    }
}
