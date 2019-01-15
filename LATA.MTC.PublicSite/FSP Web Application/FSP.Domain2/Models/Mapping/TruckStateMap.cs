using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TruckStateMap : EntityTypeConfiguration<TruckState>
    {
        public TruckStateMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TruckStateID, t.TruckState1 });

            // Properties
            this.Property(t => t.TruckState1)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TruckIcon)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TruckStates");
            this.Property(t => t.TruckStateID).HasColumnName("TruckStateID");
            this.Property(t => t.TruckState1).HasColumnName("TruckState");
            this.Property(t => t.TruckIcon).HasColumnName("TruckIcon");
        }
    }
}
