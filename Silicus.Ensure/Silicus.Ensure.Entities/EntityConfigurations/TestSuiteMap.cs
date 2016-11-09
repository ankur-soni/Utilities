using Silicus.Ensure.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class TestSuiteMap: EntityTypeConfiguration<TestSuite>
    {
        public TestSuiteMap()
        {
            HasKey(o => o.TestSuiteId);

            Property(p => p.TestSuiteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.TestSuite, TableSettings.DefaultSchema);
        }
    }
}