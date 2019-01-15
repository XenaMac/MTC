using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap()
        {
            // Primary Key
            this.HasKey(t => t.LocationID);

            // Properties
            this.Property(t => t.LocationCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Location1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Locations");
            this.Property(t => t.LocationID).HasColumnName("LocationID");
            this.Property(t => t.LocationCode).HasColumnName("LocationCode");
            this.Property(t => t.Location1).HasColumnName("Location");
        }
    }
}
