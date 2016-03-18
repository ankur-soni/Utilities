using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Entities.EntityConfigurations
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
