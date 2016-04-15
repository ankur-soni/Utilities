using Silicus.UtilityContainer.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.UtilityContainerr.Entities.EntityConfigurations
{
    internal class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Roles, TableSettings.DefaultSchema);
        }
    }
}

