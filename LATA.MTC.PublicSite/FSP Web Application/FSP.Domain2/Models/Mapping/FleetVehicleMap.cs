using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class FleetVehicleMap : EntityTypeConfiguration<FleetVehicle>
    {
        public FleetVehicleMap()
        {
            // Primary Key
            this.HasKey(t => t.FleetVehicleID);

            // Properties
            this.Property(t => t.FleetNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VehicleType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VehicleMake)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VehicleModel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VIN)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LicensePlate)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.FuelType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.VehicleNumber)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.IPAddress)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AgreementNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FleetVehicles");
            this.Property(t => t.FleetVehicleID).HasColumnName("FleetVehicleID");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.ProgramStartDate).HasColumnName("ProgramStartDate");
            this.Property(t => t.FleetNumber).HasColumnName("FleetNumber");
            this.Property(t => t.VehicleType).HasColumnName("VehicleType");
            this.Property(t => t.VehicleYear).HasColumnName("VehicleYear");
            this.Property(t => t.VehicleMake).HasColumnName("VehicleMake");
            this.Property(t => t.VehicleModel).HasColumnName("VehicleModel");
            this.Property(t => t.VIN).HasColumnName("VIN");
            this.Property(t => t.LicensePlate).HasColumnName("LicensePlate");
            this.Property(t => t.RegistrationExpireDate).HasColumnName("RegistrationExpireDate");
            this.Property(t => t.InsuranceExpireDate).HasColumnName("InsuranceExpireDate");
            this.Property(t => t.LastCHPInspection).HasColumnName("LastCHPInspection");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ProgramEndDate).HasColumnName("ProgramEndDate");
            this.Property(t => t.FAW).HasColumnName("FAW");
            this.Property(t => t.RAW).HasColumnName("RAW");
            this.Property(t => t.RAWR).HasColumnName("RAWR");
            this.Property(t => t.GVW).HasColumnName("GVW");
            this.Property(t => t.GVWR).HasColumnName("GVWR");
            this.Property(t => t.Wheelbase).HasColumnName("Wheelbase");
            this.Property(t => t.Overhang).HasColumnName("Overhang");
            this.Property(t => t.MAXTW).HasColumnName("MAXTW");
            this.Property(t => t.MAXTWCALCDATE).HasColumnName("MAXTWCALCDATE");
            this.Property(t => t.FuelType).HasColumnName("FuelType");
            this.Property(t => t.VehicleNumber).HasColumnName("VehicleNumber");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.AgreementNumber).HasColumnName("AgreementNumber");

            // Relationships
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.FleetVehicles)
                .HasForeignKey(d => d.ContractorID);

        }
    }
}
