using Silicus.UtilityContainer.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.UtilityContainer.Entities.EntityConfigurations
{
    internal class UtilityUserRolesMap : EntityTypeConfiguration<UtilityUserRoles>
    {
        public UtilityUserRolesMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.UtilityUserRoles, TableSettings.DefaultSchema);
        }
    }
}

