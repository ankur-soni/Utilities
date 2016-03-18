using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ProjectStatusMap : EntityTypeConfiguration<ProjectStatus>
    {
        public ProjectStatusMap()
        {
            HasKey(o => o.ProjectStatusId);

            Property(p => p.ProjectStatusId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.StatusId)
            .IsRequired();

            Property(t => t.ProjectSummary)
            .HasColumnType(SqlTypes.Varchar)
            .HasMaxLength(500);
            
            ToTable(TableSettings.ProjectStatus, TableSettings.DefaultSchema);
        }
    }
}
