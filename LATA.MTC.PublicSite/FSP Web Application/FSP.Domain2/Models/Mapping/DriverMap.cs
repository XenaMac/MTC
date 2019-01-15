using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverMap : EntityTypeConfiguration<Driver>
    {
        public DriverMap()
        {
            // Primary Key
            this.HasKey(t => t.DriverID);

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

            this.Property(t => t.UDF)
                .HasMaxLength(500);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DL64Number)
                .HasMaxLength(50);

            this.Property(t => t.DriversLicenseNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Drivers");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.FSPIDNumber).HasColumnName("FSPIDNumber");
            this.Property(t => t.ProgramStartDate).HasColumnName("ProgramStartDate");
            this.Property(t => t.TrainingCompletionDate).HasColumnName("TrainingCompletionDate");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.LicenseExpirationDate).HasColumnName("LicenseExpirationDate");
            this.Property(t => t.DL64ExpirationDate).HasColumnName("DL64ExpirationDate");
            this.Property(t => t.MedicalCardExpirationDate).HasColumnName("MedicalCardExpirationDate");
            this.Property(t => t.LastPullNoticeDate).HasColumnName("LastPullNoticeDate");
            this.Property(t => t.DateAdded).HasColumnName("DateAdded");
            this.Property(t => t.UDF).HasColumnName("UDF");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ContractorEndDate).HasColumnName("ContractorEndDate");
            this.Property(t => t.ProgramEndDate).HasColumnName("ProgramEndDate");
            this.Property(t => t.ContractorStartDate).HasColumnName("ContractorStartDate");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.DL64Number).HasColumnName("DL64Number");
            this.Property(t => t.DriversLicenseNumber).HasColumnName("DriversLicenseNumber");
            this.Property(t => t.AddedtoC3Database).HasColumnName("AddedtoC3Database");

            // Relationships
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.Drivers)
                .HasForeignKey(d => d.ContractorID);

        }
    }
}
