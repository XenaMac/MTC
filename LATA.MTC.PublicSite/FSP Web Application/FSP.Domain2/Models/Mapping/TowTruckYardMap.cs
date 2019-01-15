using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TowTruckYardMap : EntityTypeConfiguration<TowTruckYard>
    {
        public TowTruckYardMap()
        {
            // Primary Key
            this.HasKey(t => t.TowTruckYardID);

            // Properties
            this.Property(t => t.Location)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Comments)
                .HasMaxLength(500);

            this.Property(t => t.TowTruckYardNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TowTruckYardDescription)
                .HasMaxLength(500);

            this.Property(t => t.TowTruckCompanyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TowTruckCompanyPhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TowTruckYard");
            this.Property(t => t.TowTruckYardID).HasColumnName("TowTruckYardID");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.TowTruckYardNumber).HasColumnName("TowTruckYardNumber");
            this.Property(t => t.TowTruckYardDescription).HasColumnName("TowTruckYardDescription");
            this.Property(t => t.TowTruckCompanyName).HasColumnName("TowTruckCompanyName");
            this.Property(t => t.TowTruckCompanyPhoneNumber).HasColumnName("TowTruckCompanyPhoneNumber");
            this.Property(t => t.Position).HasColumnName("Position");
        }
    }
}
