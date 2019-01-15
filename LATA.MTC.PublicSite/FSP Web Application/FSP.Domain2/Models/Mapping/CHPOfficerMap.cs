using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class CHPOfficerMap : EntityTypeConfiguration<CHPOfficer>
    {
        public CHPOfficerMap()
        {
            // Primary Key
            this.HasKey(t => t.BadgeID);

            // Properties
            this.Property(t => t.BadgeID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OfficerLastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OfficerFirstName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CHPOfficer");
            this.Property(t => t.BadgeID).HasColumnName("BadgeID");
            this.Property(t => t.OfficerLastName).HasColumnName("OfficerLastName");
            this.Property(t => t.OfficerFirstName).HasColumnName("OfficerFirstName");
        }
    }
}
