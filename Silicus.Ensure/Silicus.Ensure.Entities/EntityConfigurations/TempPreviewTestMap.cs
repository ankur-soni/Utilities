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
    internal class TempPreviewTestMap : EntityTypeConfiguration<TempPreviewTest>
    {
        public TempPreviewTestMap()
        {
            HasKey(o => o.TempPreviewTestId);

            Property(p => p.TempPreviewTestId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.TempPreviewTest, TableSettings.DefaultSchema);

        }
    }
}
