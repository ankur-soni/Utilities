using Silicus.Ensure.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class TestSuiteTagMap : EntityTypeConfiguration<TestSuiteTag>
    {
        public TestSuiteTagMap()
        {
            HasKey(o => o.TestSuiteTagId);

            Property(p => p.TestSuiteTagId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.TestSuite, TableSettings.DefaultSchema);
        }
    }
}
