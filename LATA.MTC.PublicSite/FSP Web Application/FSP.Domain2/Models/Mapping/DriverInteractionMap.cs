using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class DriverInteractionMap : EntityTypeConfiguration<DriverInteraction>
    {
        public DriverInteractionMap()
        {
            // Primary Key
            this.HasKey(t => t.InteractionID);

            // Properties
            this.Property(t => t.InteractionArea)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.InteractionDescription)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.FollowupDescription)
                .HasMaxLength(500);

            this.Property(t => t.FollowupComments)
                .HasMaxLength(500);

            this.Property(t => t.BadgeID)
                .HasMaxLength(50);

            this.Property(t => t.VehicleNumber)
                .HasMaxLength(50);

            this.Property(t => t.BeatNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DriverInteractions");
            this.Property(t => t.InteractionID).HasColumnName("InteractionID");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");
            this.Property(t => t.DriverID).HasColumnName("DriverID");
            this.Property(t => t.InteractionTypeID).HasColumnName("InteractionTypeID");
            this.Property(t => t.InteractionArea).HasColumnName("InteractionArea");
            this.Property(t => t.InteractionDescription).HasColumnName("InteractionDescription");
            this.Property(t => t.InspectionPassFail).HasColumnName("InspectionPassFail");
            this.Property(t => t.AccidentPreventable).HasColumnName("AccidentPreventable");
            this.Property(t => t.FollowupRequired).HasColumnName("FollowupRequired");
            this.Property(t => t.FollowupDescription).HasColumnName("FollowupDescription");
            this.Property(t => t.FollowupDate).HasColumnName("FollowupDate");
            this.Property(t => t.FollowupCompletionDate).HasColumnName("FollowupCompletionDate");
            this.Property(t => t.FollowupComments).HasColumnName("FollowupComments");
            this.Property(t => t.CloseDate).HasColumnName("CloseDate");
            this.Property(t => t.BadgeID).HasColumnName("BadgeID");
            this.Property(t => t.InteractionDate).HasColumnName("InteractionDate");
            this.Property(t => t.VehicleNumber).HasColumnName("VehicleNumber");
            this.Property(t => t.BeatNumber).HasColumnName("BeatNumber");

            // Relationships
            this.HasRequired(t => t.Contractor)
                .WithMany(t => t.DriverInteractions)
                .HasForeignKey(d => d.ContractorID);
            this.HasRequired(t => t.Driver)
                .WithMany(t => t.DriverInteractions)
                .HasForeignKey(d => d.DriverID);
            this.HasRequired(t => t.InteractionType)
                .WithMany(t => t.DriverInteractions)
                .HasForeignKey(d => d.InteractionTypeID);

        }
    }
}
