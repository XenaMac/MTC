using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class vGPSTrackingMap : EntityTypeConfiguration<vGPSTracking>
    {
        public vGPSTrackingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.GPSID, t.Direction, t.VehicleStatus, t.LastUpdate, t.timeStamp, t.BeatSegmentID, t.VehicleID, t.Speed, t.Alarms, t.DriverID, t.VehicleNumber, t.BeatID, t.SpeedingAlarm, t.OutOfBoundsAlarm });

            // Properties
            this.Property(t => t.Direction)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.VehicleStatus)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VehicleID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VehicleNumber)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OutOfBoundsMessage)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("vGPSTracking");
            this.Property(t => t.GPSID).HasColumnName("GPSID");
            this.Property(t => t.Direction).HasColumnName("Direction");
            this.Property(t => t.VehicleStatus).HasColumnName("VehicleStatus");
            this.Property(t => t.LastUpdate).HasColumnName("LastUpdate");
            this.Property(t => t.timeStamp).HasColumnName("timeStamp");
            this.Property(t => t.BeatSegmentID).HasColumnName("BeatSegmentID");
            this.Property(t => t.VehicleID).HasColumnName("VehicleID");
            this.Property(t => t.Speed).HasColumnName("Speed");
            this.Property(t => t.Alarms).HasColumnName("Alarms");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.VehicleNumber).HasColumnName("VehicleNumber");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.SpeedingAlarm).HasColumnName("SpeedingAlarm");
            this.Property(t => t.SpeedingValue).HasColumnName("SpeedingValue");
            this.Property(t => t.SpeedingTime).HasColumnName("SpeedingTime");
            this.Property(t => t.SpeedingLocation).HasColumnName("SpeedingLocation");
            this.Property(t => t.OutOfBoundsAlarm).HasColumnName("OutOfBoundsAlarm");
            this.Property(t => t.OutOfBoundsMessage).HasColumnName("OutOfBoundsMessage");
        }
    }
}
