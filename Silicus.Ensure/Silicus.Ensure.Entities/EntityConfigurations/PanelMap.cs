using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class PanelMap : EntityTypeConfiguration<Panel>
    {
        public PanelMap()
        {
            HasKey(o => o.PanelId);

            Property(p => p.PanelId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Panel, TableSettings.DefaultSchema);

        }
    }
}