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
    internal class QuestionMap: EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            HasKey(o => o.QuestionId);

            Property(p => p.QuestionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.Question, TableSettings.DefaultSchema);
        }
    }
}
