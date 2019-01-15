using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class HeliosUnitMap : EntityTypeConfiguration<HeliosUnit>
    {
        public HeliosUnitMap()
        {
            // Primary Key
            this.HasKey(t => t.HeliosUnitID);

            // Properties
            this.Property(t => t.HeliosID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IPAddress)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("HeliosUnits");
            this.Property(t => t.HeliosUnitID).HasColumnName("HeliosUnitID");
            this.Property(t => t.HeliosID).HasColumnName("HeliosID");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.DateAdded).HasColumnName("DateAdded");
            this.Property(t => t.DateModified).HasColumnName("DateModified");
        }
    }
}
