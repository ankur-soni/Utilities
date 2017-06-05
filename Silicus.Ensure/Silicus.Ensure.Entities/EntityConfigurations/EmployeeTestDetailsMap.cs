using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.EntityConfigurations
{
    internal class EmployeeTestDetailsMap : EntityTypeConfiguration<EmployeeTestDetails>
    {
        public EmployeeTestDetailsMap()
        {
            HasKey(o => o.TestDetailId);

            Property(p => p.TestDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.EmployeeTestDetails, TableSettings.DefaultSchema);
        }
    }
}
