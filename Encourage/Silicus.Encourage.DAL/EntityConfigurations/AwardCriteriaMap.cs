using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.EntityConfigurations
{
    internal class AwardCriteriaMap : EntityTypeConfiguration<AwardCriteria>
    {
        public AwardCriteriaMap()
        {
            ToTable(TableSettings.AwardCriterias, TableSettings.DefaultSchema);
        }
    }
}
