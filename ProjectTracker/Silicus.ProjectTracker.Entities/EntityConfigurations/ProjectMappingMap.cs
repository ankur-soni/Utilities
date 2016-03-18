using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ProjectMappingMap : EntityTypeConfiguration<ProjectMapping>
    {
        public ProjectMappingMap()
        {
            HasKey(o => o.MappingId);

            Property(p => p.MappingId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.ProjectMapping, TableSettings.DefaultSchema);

        }
    }
}
