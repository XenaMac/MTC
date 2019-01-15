using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class TowTruckSetupMap : EntityTypeConfiguration<TowTruckSetup>
    {
        public TowTruckSetupMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MountedSecurely, t.ConnectedToCell, t.DCPowerConnected, t.RouterUnitMountedSecurely, t.MoistureFree, t.Speedtest, t.GPSSentProperly, t.InstallerName, t.InstallationDate, t.TowTruckCompany, t.VehicleID, t.SystemSerialNumber, t.IPAddress });

            // Properties
            this.Property(t => t.MountedSecurely)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.ConnectedToCell)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.DCPowerConnected)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.RouterUnitMountedSecurely)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.MoistureFree)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.Speedtest)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.GPSSentProperly)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.InstallerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TowTruckCompany)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.VehicleID)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SystemSerialNumber)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.IPAddress)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UploadSpeed)
                .HasMaxLength(10);

            this.Property(t => t.DownloadSpeed)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TowTruckSetup");
            this.Property(t => t.MountedSecurely).HasColumnName("MountedSecurely");
            this.Property(t => t.ConnectedToCell).HasColumnName("ConnectedToCell");
            this.Property(t => t.DCPowerConnected).HasColumnName("DCPowerConnected");
            this.Property(t => t.RouterUnitMountedSecurely).HasColumnName("RouterUnitMountedSecurely");
            this.Property(t => t.MoistureFree).HasColumnName("MoistureFree");
            this.Property(t => t.Speedtest).HasColumnName("Speedtest");
            this.Property(t => t.GPSSentProperly).HasColumnName("GPSSentProperly");
            this.Property(t => t.InstallerName).HasColumnName("InstallerName");
            this.Property(t => t.InstallationDate).HasColumnName("InstallationDate");
            this.Property(t => t.TowTruckCompany).HasColumnName("TowTruckCompany");
            this.Property(t => t.VehicleID).HasColumnName("VehicleID");
            this.Property(t => t.SystemSerialNumber).HasColumnName("SystemSerialNumber");
            this.Property(t => t.IPAddress).HasColumnName("IPAddress");
            this.Property(t => t.UploadSpeed).HasColumnName("UploadSpeed");
            this.Property(t => t.DownloadSpeed).HasColumnName("DownloadSpeed");
        }
    }
}
