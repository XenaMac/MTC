using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class buBeatSegmentMap : EntityTypeConfiguration<buBeatSegment>
    {
        public buBeatSegmentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BeatSegmentID, t.CHPDescription, t.BeatSegmentExtent, t.Active });

            // Properties
            this.Property(t => t.CHPDescription)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.PIMSID)
                .HasMaxLength(50);

            this.Property(t => t.BeatSegmentNumber)
                .HasMaxLength(50);

            this.Property(t => t.BeatSegmentDescription)
                .HasMaxLength(500);

            this.Property(t => t.CHPDescription2)
                .HasMaxLength(500);

            this.Property(t => t.LastUpdateBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("buBeatSegments");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.CHPDescription).HasColumnName("CHPDescription");
            this.Property(t => t.PIMSID).HasColumnName("PIMSID");
            this.Property(t => t.BeatSegmentExtent).HasColumnName("BeatSegmentExtent");
            this.Property(t => t.BeatSegmentNumber).HasColumnName("BeatSegmentNumber");
            this.Property(t => t.BeatSegmentDescription).HasColumnName("BeatSegmentDescription");
            this.Property(t => t.CHPDescription2).HasColumnName("CHPDescription2");
            this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
            this.Property(t => t.LastUpdateBy).HasColumnName("LastUpdateBy");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
