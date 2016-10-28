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
    internal class SkillMap : EntityTypeConfiguration<Skill>
    {
        public SkillMap()
        {
            HasKey(o => o.SkillId);

            Property(p => p.SkillId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Skill, TableSettings.DefaultSchema);
        }
    }
}
