using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class RolesMap :EntityTypeConfiguration<Role>
    {
        public RolesMap()
        {
            HasKey(o => o.RoleId);

            Property(p => p.RoleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Roles, TableSettings.DefaultSchema);

        }
    }
}
