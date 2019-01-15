using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ContractorManagerMap : EntityTypeConfiguration<ContractorManager>
    {
        public ContractorManagerMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractorManagerID);

            // Properties
            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CellPhone)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(75);

            // Table & Column Mappings
            this.ToTable("ContractorManagers");
            this.Property(t => t.ContractorManagerID).HasColumnName("ContractorManagerID");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.CellPhone).HasColumnName("CellPhone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");

            // Relationships
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.ContractorManagers)
                .HasForeignKey(d => d.ContractorID);

        }
    }
}
