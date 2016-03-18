using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ProjectResouceMap : EntityTypeConfiguration<ProjectResourceUtilization>
    {
        public ProjectResouceMap()
        {
            HasKey(o => o.ProjectResourceId);

            Property(p => p.ProjectResourceId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.RoleName)
               .IsRequired()
               .HasColumnType(SqlTypes.Varchar)
               .HasMaxLength(100);

            Property(t => t.ResourceName)
               .IsRequired()
               .HasColumnType(SqlTypes.Varchar)
               .HasMaxLength(100);

            Property(t => t.Status)
               .HasColumnType(SqlTypes.Varchar)
               .HasMaxLength(500);
                        
            ToTable(TableSettings.ProjectResouceUtilization, TableSettings.DefaultSchema);
        }
    }
}
