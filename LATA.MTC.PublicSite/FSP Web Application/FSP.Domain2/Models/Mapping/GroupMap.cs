using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class GroupMap : EntityTypeConfiguration<Group>
    {
        public GroupMap()
        {
            // Primary Key
            this.HasKey(t => t.GroupID);

            // Properties
            this.Property(t => t.GroupName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.GroupCity)
                .HasMaxLength(50);

            this.Property(t => t.GroupState)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.GroupAddress)
                .HasMaxLength(100);

            this.Property(t => t.GroupZip)
                .HasMaxLength(10);

            this.Property(t => t.GroupPhone)
                .HasMaxLength(20);

            this.Property(t => t.GroupContactName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Groups");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.GroupCity).HasColumnName("GroupCity");
            this.Property(t => t.GroupState).HasColumnName("GroupState");
            this.Property(t => t.GroupAddress).HasColumnName("GroupAddress");
            this.Property(t => t.GroupZip).HasColumnName("GroupZip");
            this.Property(t => t.GroupPhone).HasColumnName("GroupPhone");
            this.Property(t => t.GroupContactName).HasColumnName("GroupContactName");
        }
    }
}
