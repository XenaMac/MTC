using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverDailyScheduleMap : EntityTypeConfiguration<DriverDailySchedule>
    {
        public DriverDailyScheduleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DriverID, t.BeatScheduleID });

            // Properties
            // Table & Column Mappings
            this.ToTable("DriverDailySchedule");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.DriverDailySchedules)
                .HasForeignKey(d => d.BeatID);
            this.HasRequired(t => t.Driver)
                .WithMany(t => t.DriverDailySchedules)
                .HasForeignKey(d => d.DriverID);

        }
    }
}
