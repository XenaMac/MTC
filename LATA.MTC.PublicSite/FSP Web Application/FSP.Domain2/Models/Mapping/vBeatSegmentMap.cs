using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class vBeatSegmentMap : EntityTypeConfiguration<vBeatSegment>
    {
        public vBeatSegmentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BeatSegmentID, t.BeatSegmentDescription, t.CHPDescription });

            // Properties
            this.Property(t => t.BeatSegmentDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CHPDescription)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.PIMSID)
                .HasMaxLength(50);

            this.Property(t => t.BeatSegmentNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("vBeatSegments");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.BeatSegmentDescription).HasColumnName("BeatSegmentDescription");
            this.Property(t => t.CHPDescription).HasColumnName("CHPDescription");
            this.Property(t => t.PIMSID).HasColumnName("PIMSID");
            this.Property(t => t.BeatSegmentExtent).HasColumnName("BeatSegmentExtent");
            this.Property(t => t.BeatSegmentNumber).HasColumnName("BeatSegmentNumber");
        }
    }
}
