using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class YearlyCalendarMap : EntityTypeConfiguration<YearlyCalendar>
    {
        public YearlyCalendarMap()
        {
            // Primary Key
            this.HasKey(t => t.DateID);

            // Properties
            this.Property(t => t.dayName)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("YearlyCalendar");
            this.Property(t => t.DateID).HasColumnName("DateID");
            this.Property(t => t.dayName).HasColumnName("dayName");
            this.Property(t => t.Date).HasColumnName("Date");
        }
    }
}
