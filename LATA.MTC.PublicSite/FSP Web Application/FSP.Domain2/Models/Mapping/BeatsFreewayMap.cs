using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class BeatsFreewayMap : EntityTypeConfiguration<BeatsFreeway>
    {
        public BeatsFreewayMap()
        {
            // Primary Key
            this.HasKey(t => t.BeatFreewayID);

            // Properties
            // Table & Column Mappings
            this.ToTable("BeatsFreeways");
            this.Property(t => t.BeatFreewayID).HasColumnName("BeatFreewayID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.FreewayID).HasColumnName("FreewayID");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.BeatsFreeways)
                .HasForeignKey(d => d.BeatID);
            this.HasRequired(t => t.Freeway)
                .WithMany(t => t.BeatsFreeways)
                .HasForeignKey(d => d.FreewayID);

        }
    }
}
