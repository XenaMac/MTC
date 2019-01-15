using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class CalendarMap : EntityTypeConfiguration<Calendar>
    {
        public CalendarMap()
        {
            // Primary Key
            this.HasKey(t => t.CalendarID);

            // Properties
            this.Property(t => t.CalendarName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Calendars");
            this.Property(t => t.CalendarID).HasColumnName("CalendarID");
            this.Property(t => t.CalendarName).HasColumnName("CalendarName");
            this.Property(t => t.CalendarDate).HasColumnName("CalendarDate");
            this.Property(t => t.BeatScheduleID).HasColumnName("BeatScheduleID");
        }
    }
}
