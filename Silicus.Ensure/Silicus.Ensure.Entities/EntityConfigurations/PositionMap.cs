using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class PositionMap : EntityTypeConfiguration<Position>
    {
        public PositionMap()
        {
            HasKey(o => o.PositionId);

            Property(p => p.PositionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Position, TableSettings.DefaultSchema);

        }
    }
}