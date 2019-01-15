using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class IncidentTypeMap : EntityTypeConfiguration<IncidentType>
    {
        public IncidentTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.IncidentTypeID);

            // Properties
            this.Property(t => t.IncidentTypeCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.IncidentType1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("IncidentTypes");
            this.Property(t => t.IncidentTypeID).HasColumnName("IncidentTypeID");
            this.Property(t => t.IncidentTypeCode).HasColumnName("IncidentTypeCode");
            this.Property(t => t.IncidentType1).HasColumnName("IncidentType");
        }
    }
}
