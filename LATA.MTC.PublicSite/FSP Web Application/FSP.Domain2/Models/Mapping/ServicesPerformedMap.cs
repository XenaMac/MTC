using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FSP.Domain2.Models.Mapping
{
    public class ServicesPerformedMap : EntityTypeConfiguration<ServicesPerformed>
    {
        public ServicesPerformedMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AssistID, t.ServiceTypeID });

            // Properties
            // Table & Column Mappings
            this.ToTable("ServicesPerformed");
            this.Property(t => t.AssistID).HasColumnName("AssistID");
            this.Property(t => t.ServiceTypeID).HasColumnName("ServiceTypeID");
        }
    }
}
