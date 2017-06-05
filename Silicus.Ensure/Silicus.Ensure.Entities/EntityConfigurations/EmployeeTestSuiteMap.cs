using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class EmployeeTestSuiteMap : EntityTypeConfiguration<EmployeeTestSuite>
    {
        public EmployeeTestSuiteMap()
        {
            HasKey(o => o.EmployeeTestSuiteId);

            Property(p => p.EmployeeTestSuiteId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.EmployeeTestSuite, TableSettings.DefaultSchema);

        }
    }
}
