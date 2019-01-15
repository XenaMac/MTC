using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ReportMap : EntityTypeConfiguration<Report>
    {
        public ReportMap()
        {
            // Primary Key
            this.HasKey(t => t.ReportID);

            // Properties
            this.Property(t => t.ReportName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ConnString)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.SQL)
                .IsRequired();

            this.Property(t => t.cmdType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.parameters)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Reports");
            this.Property(t => t.ReportID).HasColumnName("ReportID");
            this.Property(t => t.ReportName).HasColumnName("ReportName");
            this.Property(t => t.ConnString).HasColumnName("ConnString");
            this.Property(t => t.SQL).HasColumnName("SQL");
            this.Property(t => t.cmdType).HasColumnName("cmdType");
            this.Property(t => t.parameters).HasColumnName("parameters");
        }
    }
}
