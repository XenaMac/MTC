using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class VarMap : EntityTypeConfiguration<Var>
    {
        public VarMap()
        {
            // Primary Key
            this.HasKey(t => t.VarID);

            // Properties
            this.Property(t => t.VarName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VarValue)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Vars");
            this.Property(t => t.VarID).HasColumnName("VarID");
            this.Property(t => t.VarName).HasColumnName("VarName");
            this.Property(t => t.VarValue).HasColumnName("VarValue");
        }
    }
}
