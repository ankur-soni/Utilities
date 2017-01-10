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
   internal class SuperUserMap: EntityTypeConfiguration<SuperUser>
    {
        public SuperUserMap()
        {
            HasKey( o => o.Id);
            Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            ToTable(TableSettings.SuperUsers, TableSettings.DefaultSchema);
        }
    }
}
