using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class AlarmMap : EntityTypeConfiguration<Alarm>
    {
        public AlarmMap()
        {
            // Primary Key
            this.HasKey(t => t.AlarmID);

            // Properties
            this.Property(t => t.AlarmType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Alarms");
            this.Property(t => t.AlarmID).HasColumnName("AlarmID");
            this.Property(t => t.AlarmType).HasColumnName("AlarmType");
            this.Property(t => t.AlarmTime).HasColumnName("AlarmTime");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.VehicleID).HasColumnName("VehicleID");
        }
    }
}
