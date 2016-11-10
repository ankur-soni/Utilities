using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
   internal class UserTestDetailsMap : EntityTypeConfiguration<UserTestDetails>
    {
       public UserTestDetailsMap()
       {
              HasKey(o => o.TestDetailId);

              Property(p => p.TestDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.UserTestDetails, TableSettings.DefaultSchema);
       }
    }
}
