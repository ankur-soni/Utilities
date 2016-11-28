using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class ProjectDetailMap :EntityTypeConfiguration<ProjectDetail>
    {
        public ProjectDetailMap()
        {
            HasKey(o => o.ProjectDetailId);

            Property(p => p.ProjectDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.ProjectDetails, TableSettings.DefaultSchema);

        }
    }
}
