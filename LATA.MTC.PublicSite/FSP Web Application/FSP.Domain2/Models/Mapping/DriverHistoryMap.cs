using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverHistoryMap : EntityTypeConfiguration<DriverHistory>
    {
        public DriverHistoryMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DriverID, t.TimeStamp });

            // Properties
            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FSPIDNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LicenseNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UDF)
                .HasMaxLength(500);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("DriverHistory");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.FSPIDNumber).HasColumnName("FSPIDNumber");
            this.Property(t => t.ProgramStartDate).HasColumnName("ProgramStartDate");
            this.Property(t => t.TrainingCompletionDate).HasColumnName("TrainingCompletionDate");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.LicenseNumber).HasColumnName("LicenseNumber");
            this.Property(t => t.LicenseExpirationDate).HasColumnName("LicenseExpirationDate");
            this.Property(t => t.DL64ExpirationDate).HasColumnName("DL64ExpirationDate");
            this.Property(t => t.MedicalCardExpirationDate).HasColumnName("MedicalCardExpirationDate");
            this.Property(t => t.LassPullNoticeDate).HasColumnName("LassPullNoticeDate");
            this.Property(t => t.DateAdded).HasColumnName("DateAdded");
            this.Property(t => t.UDF).HasColumnName("UDF");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ContractorEndDate).HasColumnName("ContractorEndDate");
            this.Property(t => t.ProgramEndDate).HasColumnName("ProgramEndDate");
            this.Property(t => t.ScheduleID).HasColumnName("ScheduleID");
            this.Property(t => t.DriverStartDate).HasColumnName("DriverStartDate");
        }
    }
}
