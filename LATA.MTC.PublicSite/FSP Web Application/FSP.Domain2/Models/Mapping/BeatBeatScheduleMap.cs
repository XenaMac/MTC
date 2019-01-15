using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatBeatScheduleMap : EntityTypeConfiguration<BeatBeatSchedule>
    {
        public BeatBeatScheduleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BeatID, t.BeatScheduleID });

            // Properties
            // Table & Column Mappings
            this.ToTable("BeatBeatSchedules");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
        }
    }
}
