using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ShiftScheduleMap : EntityTypeConfiguration<ShiftSchedule>
    {
        public ShiftScheduleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BeatScheduleID, t.BeatID });

            // Properties
            // Table & Column Mappings
            this.ToTable("ShiftSchedules");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.ShiftSchedules)
                .HasForeignKey(d => d.BeatID);

        }
    }
}
