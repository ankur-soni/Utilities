using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;


namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class SkillSetMap : EntityTypeConfiguration<SkillSet>
    {
        public SkillSetMap()
        {
            HasKey(o => o.SkillSetId);

            Property(p => p.SkillSetId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.SkillSets, TableSettings.DefaultSchema);

        }
    }
}