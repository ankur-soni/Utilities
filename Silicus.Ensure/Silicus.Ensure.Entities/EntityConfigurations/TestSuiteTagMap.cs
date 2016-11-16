using Silicus.Ensure.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class TestSuiteTagMap : EntityTypeConfiguration<TestSuiteTag>
    {
        public TestSuiteTagMap()
        {
            HasKey(o => o.TestSuiteId);

            Property(p => p.TestSuiteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.TestSuite, TableSettings.DefaultSchema);
        }
    }
}
