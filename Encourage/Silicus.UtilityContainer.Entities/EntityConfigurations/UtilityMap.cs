using Silicus.UtilityContainer.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.UtilityContainer.Entities.EntityConfigurations
{
    internal class UtilityMap : EntityTypeConfiguration<Utility>
    {
        public UtilityMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Utilities, TableSettings.DefaultSchema);
        }
    }
}

