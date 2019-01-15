using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TrafficSpeedMap : EntityTypeConfiguration<TrafficSpeed>
    {
        public TrafficSpeedMap()
        {
            // Primary Key
            this.HasKey(t => t.TrafficSpeedID);

            // Properties
            this.Property(t => t.TrafficSpeedCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.TrafficSpeed1)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TrafficSpeeds");
            this.Property(t => t.TrafficSpeedID).HasColumnName("TrafficSpeedID");
            this.Property(t => t.TrafficSpeedCode).HasColumnName("TrafficSpeedCode");
            this.Property(t => t.TrafficSpeed1).HasColumnName("TrafficSpeed");
        }
    }
}
