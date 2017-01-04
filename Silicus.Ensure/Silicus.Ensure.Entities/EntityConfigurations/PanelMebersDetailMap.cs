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
    internal class PanelMebersDetailMap : EntityTypeConfiguration<PanelMemberDetail>
    {
        public PanelMebersDetailMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.PanelMemberDetail, TableSettings.DefaultSchema);

        }
    }
}
