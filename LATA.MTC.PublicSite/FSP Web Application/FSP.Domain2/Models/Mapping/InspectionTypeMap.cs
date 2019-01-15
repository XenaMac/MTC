using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class InspectionTypeMap : EntityTypeConfiguration<InspectionType>
    {
        public InspectionTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.InspectionTypeID);

            // Properties
            this.Property(t => t.InspectionType1)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InspectionTypeCode)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("InspectionTypes");
            this.Property(t => t.InspectionTypeID).HasColumnName("InspectionTypeID");
            this.Property(t => t.InspectionType1).HasColumnName("InspectionType");
            this.Property(t => t.InspectionTypeCode).HasColumnName("InspectionTypeCode");
        }
    }
}
