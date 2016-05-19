using Silicus.Encourage.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.Encourage.DAL.EntityConfigurations
{
    internal class CriteriaMap : EntityTypeConfiguration<Criteria>
    {
        public CriteriaMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Criterias, TableSettings.DefaultSchema);
        }
    }
}
