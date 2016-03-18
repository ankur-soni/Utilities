using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class InfrastructureDetailsMap : EntityTypeConfiguration<InfrastructureDetails>
    {
        public InfrastructureDetailsMap()
        {
            HasKey(o => o.InfrastructureDetailId);

            Property(p => p.InfrastructureDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.InfrastructureDetails, TableSettings.DefaultSchema);
        }
    }
}

