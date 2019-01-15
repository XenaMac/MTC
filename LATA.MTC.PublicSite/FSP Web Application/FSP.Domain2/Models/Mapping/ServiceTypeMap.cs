using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ServiceTypeMap : EntityTypeConfiguration<ServiceType>
    {
        public ServiceTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceTypeID);

            // Properties
            this.Property(t => t.ServiceTypeCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ServiceType1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ServiceTypes");
            this.Property(t => t.ServiceTypeID).HasColumnName("ServiceTypeID");
            this.Property(t => t.ServiceTypeCode).HasColumnName("ServiceTypeCode");
            this.Property(t => t.ServiceType1).HasColumnName("ServiceType");
        }
    }
}
