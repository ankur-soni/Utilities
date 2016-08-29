using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Entities.EntityConfigurations
{
    internal class CreditsMap : EntityTypeConfiguration<Credits>
    {
        public CreditsMap()
        {

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Credits, TableSettings.DefaultSchema);
        }
    }
}
