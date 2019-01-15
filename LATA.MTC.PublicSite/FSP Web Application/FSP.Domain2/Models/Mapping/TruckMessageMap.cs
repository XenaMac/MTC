using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TruckMessageMap : EntityTypeConfiguration<TruckMessage>
    {
        public TruckMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.TruckMessageID);

            // Properties
            this.Property(t => t.TruckIP)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MessageText)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("TruckMessages");
            this.Property(t => t.TruckMessageID).HasColumnName("TruckMessageID");
            this.Property(t => t.TruckIP).HasColumnName("TruckIP");
            this.Property(t => t.MessageText).HasColumnName("MessageText");
            this.Property(t => t.SentTime).HasColumnName("SentTime");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Acked).HasColumnName("Acked");
            this.Property(t => t.AckedTime).HasColumnName("AckedTime");
        }
    }
}
