using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class C1098CodesMap : EntityTypeConfiguration<C1098Codes>
    {
        public C1098CodesMap()
        {
            // Primary Key
            this.HasKey(t => t.CodeID);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.CodeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CodeDescription)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.CodeCall)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("1098Codes");
            this.Property(t => t.CodeID).HasColumnName("CodeID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CodeName).HasColumnName("CodeName");
            this.Property(t => t.CodeDescription).HasColumnName("CodeDescription");
            this.Property(t => t.CodeCall).HasColumnName("CodeCall");
        }
    }
}
