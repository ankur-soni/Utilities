using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class WeekMap : EntityTypeConfiguration<Week>
    {
        public WeekMap()
        {
            HasKey(o => o.WeekId);

            Property(p => p.WeekId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Week, TableSettings.DefaultSchema);
        }       
    }
}
