using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatScheduleMap : EntityTypeConfiguration<BeatSchedule>
    {
        public BeatScheduleMap()
        {
            // Primary Key
            this.HasKey(t => t.BeatScheduleID);

            // Properties
            this.Property(t => t.ScheduleName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BeatSchedules");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
            this.Property(t => t.ScheduleName).HasColumnName("ScheduleName");
            this.Property(t => t.Weekday).HasColumnName("Weekday");
            this.Property(t => t.Logon).HasColumnName("Logon");
            this.Property(t => t.RollOut).HasColumnName("RollOut");
            this.Property(t => t.OnPatrol).HasColumnName("OnPatrol");
            this.Property(t => t.RollIn).HasColumnName("RollIn");
            this.Property(t => t.LogOff).HasColumnName("LogOff");
        }
    }
}
