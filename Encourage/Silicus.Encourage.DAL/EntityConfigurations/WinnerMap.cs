using Silicus.Encourage.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.EntityConfigurations
{
        internal class WinnerMap : EntityTypeConfiguration<Winner>
        {
            public WinnerMap()
            {
                HasKey(o => o.Id);

                Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                ToTable(TableSettings.Winners, TableSettings.DefaultSchema);
            }
        }
  
}
