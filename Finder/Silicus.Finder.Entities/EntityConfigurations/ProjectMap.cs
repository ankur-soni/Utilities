using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;
using FluentValidation;


namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class ProjectMap :EntityTypeConfiguration<Project>
    {
            public ProjectMap()
        {
            HasKey(o => o.ProjectId);

            Property(p => p.ProjectId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

          // Property(p => p.StartDate < p.ExpectedEndDate);

            ToTable(TableSettings.Projects, TableSettings.DefaultSchema);
        }
    }
}