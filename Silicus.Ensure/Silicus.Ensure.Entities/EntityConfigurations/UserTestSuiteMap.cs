using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class UserTestSuiteMap : EntityTypeConfiguration<UserTestSuite>
    {
        public UserTestSuiteMap()
        {
            HasKey(o => o.UserTestSuiteId);

            Property(p => p.UserTestSuiteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.UserTestSuite, TableSettings.DefaultSchema);

        }
    }
}
