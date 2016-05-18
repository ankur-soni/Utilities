using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.EntityConfigurations
{
    internal class NominationMap : EntityTypeConfiguration<Nomination>
    {
        public NominationMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Nominations, TableSettings.DefaultSchema);
        }
    }
}
