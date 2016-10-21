using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    /// <summary>
    /// This class gives more control over mapping POCO with database table. This uses Fluent API to configure
    /// the table column. A configuration required only for column that needs extra setup e.g. length 100.
    /// </summary>
    internal class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            HasKey(o => o.OrganizationId);

            Property(p => p.OrganizationId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            // Table
            ToTable(TableSettings.Organizations, TableSettings.DefaultSchema);
        }
    }
}
