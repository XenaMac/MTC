using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class CallBoxMap : EntityTypeConfiguration<CallBox>
    {
        public CallBoxMap()
        {
            // Primary Key
            this.HasKey(t => t.CallBoxID);

            // Properties
            this.Property(t => t.TelephoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SiteType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.SignNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CallBoxes");
            this.Property(t => t.CallBoxID).HasColumnName("CallBoxID");
            this.Property(t => t.TelephoneNumber).HasColumnName("TelephoneNumber");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.FreewayID).HasColumnName("FreewayID");
            this.Property(t => t.SiteType).HasColumnName("SiteType");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.SignNumber).HasColumnName("SignNumber");
        }
    }
}
