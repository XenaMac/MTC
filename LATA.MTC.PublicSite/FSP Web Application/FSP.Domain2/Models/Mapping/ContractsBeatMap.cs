using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ContractsBeatMap : EntityTypeConfiguration<ContractsBeat>
    {
        public ContractsBeatMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ContractID, t.BeatID });

            // Properties
            // Table & Column Mappings
            this.ToTable("ContractsBeats");
            this.Property(t => t.ContractID).HasColumnName("ContractID");
            this.Property(t => t.BeatID).HasColumnName("BeatID");
        }
    }
}
