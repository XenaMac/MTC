using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class FleetVehiclesBeatMap : EntityTypeConfiguration<FleetVehiclesBeat>
    {
        public FleetVehiclesBeatMap()
        {
            // Primary Key
            this.HasKey(t => t.FleetVehicleBeat);

            // Properties
            // Table & Column Mappings
            this.ToTable("FleetVehiclesBeats");
            this.Property(t => t.FleetVehicleBeat).HasColumnName("FleetVehicleBeat");
            this.Property(t => t.FleetVehicleID).HasColumnName("FleetVehicleID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");

            // Relationships
            this.HasRequired(t => t.Beat)
                .WithMany(t => t.FleetVehiclesBeats)
                .HasForeignKey(d => d.BeatID);
            this.HasRequired(t => t.FleetVehicle)
                .WithMany(t => t.FleetVehiclesBeats)
                .HasForeignKey(d => d.FleetVehicleID);

        }
    }
}
