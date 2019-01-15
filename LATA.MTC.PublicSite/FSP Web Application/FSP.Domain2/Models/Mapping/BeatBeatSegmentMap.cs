using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatBeatSegmentMap : EntityTypeConfiguration<BeatBeatSegment>
    {
        public BeatBeatSegmentMap()
        {
            // Primary Key
            this.HasKey(t => t.BeatBeatSegmentID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BeatBeatSegments");
            this.Property(t => t.BeatBeatSegmentID).HasColumnName("BeatBeatSegmentID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.Active).HasColumnName("Active");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.BeatBeatSegments)
                .HasForeignKey(d => d.BeatID);

        }
    }
}
