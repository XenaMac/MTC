using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserID);

            // Properties
            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.LastName)
                .HasMaxLength(50);

            this.Property(t => t.FirstName)
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(100);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.State)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Zip)
                .HasMaxLength(10);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Zip).HasColumnName("Zip");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.WantsNotification).HasColumnName("WantsNotification");
            this.Property(t => t.HourlyCost).HasColumnName("HourlyCost");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.ContractorID).HasColumnName("ContractorID");

            // Relationships
            this.HasRequired(t => t.Role)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
