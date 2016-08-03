using Silicus.UtilityContainer.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.UtilityContainer.Entities.EntityConfigurations
{
    internal class HolidayMap : EntityTypeConfiguration<Holiday>
    {
        public HolidayMap()
        {
            HasKey(o => o.ID);

            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Holidays, TableSettings.DefaultSchema);
        }
    }
}
