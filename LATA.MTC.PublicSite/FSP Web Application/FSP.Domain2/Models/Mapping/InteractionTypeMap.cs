using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class InteractionTypeMap : EntityTypeConfiguration<InteractionType>
    {
        public InteractionTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.InteractionTypeID);

            // Properties
            this.Property(t => t.InteractionType1)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("InteractionTypes");
            this.Property(t => t.InteractionTypeID).HasColumnName("InteractionTypeID");
            this.Property(t => t.InteractionType1).HasColumnName("InteractionType");
        }
    }
}
