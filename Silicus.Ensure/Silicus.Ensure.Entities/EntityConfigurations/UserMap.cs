using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class UserMap :EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(o => o.UserId);

            Property(p => p.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.User, TableSettings.DefaultSchema);

        }
    }
}
