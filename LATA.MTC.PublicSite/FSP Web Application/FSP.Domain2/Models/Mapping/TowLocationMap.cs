using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TowLocationMap : EntityTypeConfiguration<TowLocation>
    {
        public TowLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.TowLocationID);

            // Properties
            this.Property(t => t.TowLocationCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.TowLocation1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TowLocations");
            this.Property(t => t.TowLocationID).HasColumnName("TowLocationID");
            this.Property(t => t.TowLocationCode).HasColumnName("TowLocationCode");
            this.Property(t => t.TowLocation1).HasColumnName("TowLocation");
        }
    }
}
