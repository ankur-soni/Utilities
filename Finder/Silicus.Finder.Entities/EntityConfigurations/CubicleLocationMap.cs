using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;


namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class CubicleLocationMap : EntityTypeConfiguration<CubicleLocation>
    {
        public CubicleLocationMap()
        {
            HasKey(o => o.CubicleLocationId);

            Property(p => p.CubicleLocationId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.CubicleLocations, TableSettings.DefaultSchema);

        }
    }
}