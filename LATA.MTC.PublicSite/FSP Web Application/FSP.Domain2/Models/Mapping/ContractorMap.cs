using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ContractorMap : EntityTypeConfiguration<Contractor>
    {
        public ContractorMap()
        {
            // Primary Key
            this.HasKey(t => t.ContractorID);

            // Properties
            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.OfficeTelephone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.MCPNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.ContractCompanyName)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.State)
                .HasMaxLength(2);

            this.Property(t => t.Zip)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Contractors");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.OfficeTelephone).HasColumnName("OfficeTelephone");
            this.Property(t => t.MCPNumber).HasColumnName("MCPNumber");
            this.Property(t => t.MCPExpiration).HasColumnName("MCPExpiration");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ContractCompanyName).HasColumnName("ContractCompanyName");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Zip).HasColumnName("Zip");

            // Relationships
            this.HasMany(t => t.InsuranceCarriers)
                .WithMany(t => t.Contractors)
                .Map(m =>
                    {
                        m.ToTable("ContractorInsurance");
                        m.MapLeftKey("ContractorID");
                        m.MapRightKey("InsuranceID");
                    });


        }
    }
}
