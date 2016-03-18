
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ManagerDetailMap : EntityTypeConfiguration<Manager>
    {
        public ManagerDetailMap()
        {
            HasKey(o => o.ManagerId);

            Property(p => p.ManagerId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Managers, TableSettings.DefaultSchema);

        }
    }
}
