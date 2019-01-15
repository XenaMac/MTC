using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ContractMap : EntityTypeConfiguration<Contract>
    {
        public ContractMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractID);

            // Properties
            this.Property(t => t.AgreementNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Contracts");
            this.Property(t => t.ContractID).HasColumnName("ContractID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.MaxObligation).HasColumnName("MaxObligation");
            this.Property(t => t.AgreementNumber).HasColumnName("AgreementNumber");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.Contracts)
                .HasForeignKey(d => d.BeatID);
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.Contracts)
                .HasForeignKey(d => d.ContractorID);

        }
    }
}
