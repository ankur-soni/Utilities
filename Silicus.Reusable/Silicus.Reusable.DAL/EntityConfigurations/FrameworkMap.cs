using Silicus.Reusable.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Reusable.DAL.EntityConfigurations
{
    internal class FrameworkMap : EntityTypeConfiguration<Framework>
    {
        public FrameworkMap()
        {
            HasKey(o => o.Id);
        }
    }
}
