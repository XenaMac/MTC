using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverBreakMap : EntityTypeConfiguration<DriverBreak>
    {
        public DriverBreakMap()
        {
            // Primary Key
            this.HasKey(t => t.BreakID);

            // Properties
            this.Property(t => t.BreakType)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("DriverBreaks");
            this.Property(t => t.BreakID).HasColumnName("BreakID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.BreakType).HasColumnName("BreakType");
            this.Property(t => t.DateOfEvent).HasColumnName("DateOfEvent");
            this.Property(t => t.Duration).HasColumnName("Duration");
        }
    }
}
