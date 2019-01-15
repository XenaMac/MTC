using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatMap : EntityTypeConfiguration<Beat>
    {
        public BeatMap()
        {
            // Primary Key
            this.HasKey(t => t.BeatID);

            // Properties
            this.Property(t => t.BeatDescription)
                .HasMaxLength(500);

            this.Property(t => t.BeatNumber)
                .HasMaxLength(50);

            this.Property(t => t.LastUpdateBy)
                .HasMaxLength(50);

            this.Property(t => t.BeatColor)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Beats");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.BeatExtent).HasColumnName("BeatExtent");
            this.Property(t => t.FreewayID).HasColumnName("FreewayID");
            this.Property(t => t.BeatDescription).HasColumnName("BeatDescription");
            this.Property(t => t.BeatNumber).HasColumnName("BeatNumber");
            this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
            this.Property(t => t.LastUpdateBy).HasColumnName("LastUpdateBy");
            this.Property(t => t.IsTemporary).HasColumnName("IsTemporary");
            this.Property(t => t.BeatColor).HasColumnName("BeatColor");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
