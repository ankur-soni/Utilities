using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Entities.EntityConfigurations
{
    internal class SkillMap : EntityTypeConfiguration<Skill>
    {
        public SkillMap()
        {
            HasKey(o => o.ID);

            Property(p => p.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Skills, TableSettings.DefaultSchema);

        }
    }
}
