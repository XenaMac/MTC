using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class buBeatBeatSegmentMap : EntityTypeConfiguration<buBeatBeatSegment>
    {
        public buBeatBeatSegmentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BeatBeatSegmentID, t.BeatID, t.BeatSegmentID });

            // Properties
            // Table & Column Mappings
            this.ToTable("buBeatBeatSegments");
            this.Property(t => t.BeatBeatSegmentID).HasColumnName("BeatBeatSegmentID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
