using Silicus.UtilityContainer.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.UtilityContainerr.Entities.EntityConfigurations
{
    internal class UtilityRoleMap : EntityTypeConfiguration<UtilityRole>
    {
        public UtilityRoleMap()
        {
            HasKey(o => o.ID);

            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.UtilityRoles, TableSettings.DefaultSchema);
        }
    }
}