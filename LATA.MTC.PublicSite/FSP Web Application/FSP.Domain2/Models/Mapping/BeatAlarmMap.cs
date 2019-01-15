using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatAlarmMap : EntityTypeConfiguration<BeatAlarm>
    {
        public BeatAlarmMap()
        {
            // Primary Key
            this.HasKey(t => t.BeatAlarmsID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BeatAlarms");
            this.Property(t => t.BeatAlarmsID).HasColumnName("BeatAlarmsID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
            this.Property(t => t.MaxDistanceFromBeat).HasColumnName("MaxDistanceFromBeat");
            this.Property(t => t.MaxTimeOffBeat).HasColumnName("MaxTimeOffBeat");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.BeatAlarms)
                .HasForeignKey(d => d.BeatID);

        }
    }
}
