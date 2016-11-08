using Silicus.Ensure.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class TagsMap : EntityTypeConfiguration<Tags>
    {
        public TagsMap()
        {
            HasKey(o => o.TagId);

            Property(p => p.TagId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Tags, TableSettings.DefaultSchema);
        }
    }
}
