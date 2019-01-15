using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class FreewayMap : EntityTypeConfiguration<Freeway>
    {
        public FreewayMap()
        {
            // Primary Key
            this.HasKey(t => t.FreewayID);

            // Properties
            this.Property(t => t.FreewayID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FreewayName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Freeways");
            this.Property(t => t.FreewayID).HasColumnName("FreewayID");
            this.Property(t => t.FreewayName).HasColumnName("FreewayName");
        }
    }
}
