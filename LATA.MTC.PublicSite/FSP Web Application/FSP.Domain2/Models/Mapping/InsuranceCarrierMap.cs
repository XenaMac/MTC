using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class InsuranceCarrierMap : EntityTypeConfiguration<InsuranceCarrier>
    {
        public InsuranceCarrierMap()
        {
            // Primary Key
            this.HasKey(t => t.InsuranceID);

            // Properties
            this.Property(t => t.CarrierName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.InsurancePolicyNumber)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PolicyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Fax)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("InsuranceCarriers");
            this.Property(t => t.InsuranceID).HasColumnName("InsuranceID");
            this.Property(t => t.CarrierName).HasColumnName("CarrierName");
            this.Property(t => t.InsurancePolicyNumber).HasColumnName("InsurancePolicyNumber");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.PolicyName).HasColumnName("PolicyName");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Fax).HasColumnName("Fax");
            this.Property(t => t.Email).HasColumnName("Email");
        }
    }
}
