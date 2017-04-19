using Silicus.Ensure.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class TechnologyMap : EntityTypeConfiguration<Technology>
    {
        public TechnologyMap()
        {
            HasKey(o => o.TechnologyId);

            Property(p => p.TechnologyId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Technology, TableSettings.DefaultSchema);
        }
    }
}
