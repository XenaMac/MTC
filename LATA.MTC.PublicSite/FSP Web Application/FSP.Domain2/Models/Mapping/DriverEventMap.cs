using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverEventMap : EntityTypeConfiguration<DriverEvent>
    {
        public DriverEventMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EventType)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("DriverEvents");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
        }
    }
}
