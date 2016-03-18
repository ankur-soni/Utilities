using Silicus.Finder.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class TitleMap : EntityTypeConfiguration<Title>
    {
        public TitleMap()
        {
            HasKey(o => o.TitleId);

            Property(p => p.TitleId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Titles, TableSettings.DefaultSchema);

        }
    }
}
