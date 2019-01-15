using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class WeblinkMap : EntityTypeConfiguration<Weblink>
    {
        public WeblinkMap()
        {
            // Primary Key
            this.HasKey(t => t.WeblinkID);

            // Properties
            this.Property(t => t.URL)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Weblinks");
            this.Property(t => t.WeblinkID).HasColumnName("WeblinkID");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
