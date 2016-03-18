using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations

{
    using System.Data.Entity.ModelConfiguration;

    internal class ProjectMap :EntityTypeConfiguration<Project>
    {
        public ProjectMap()
        {
            HasKey(o => o.ProjectId);

            Property(p => p.ProjectId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.ProjectName)
               .IsRequired()
               .HasColumnType(SqlTypes.Varchar)
               .HasMaxLength(50);

            Property(t => t.StartDate)
              .IsRequired()
              .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.PlannedEndDate)
              .IsRequired()
              .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.ProjectDescription)
            .HasColumnType(SqlTypes.Varchar)
            .HasMaxLength(500);
            
            ToTable(TableSettings.Projects, TableSettings.DefaultSchema);
        }
    }
}



