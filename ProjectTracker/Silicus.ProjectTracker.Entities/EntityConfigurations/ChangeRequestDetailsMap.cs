using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ChangeRequestDetailsMap : EntityTypeConfiguration<ChangeRequestDetails>
    {
        public ChangeRequestDetailsMap()
        {
            HasKey(o => o.ChangeRequestId);

            Property(p => p.ChangeRequestId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.ChangeRequestDetails, TableSettings.DefaultSchema);
        }
    }
}
