using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    using System.Data.Entity.ModelConfiguration;

    internal class BaseEntityMap : EntityTypeConfiguration<BaseEntity>
    {
        public BaseEntityMap()
        {
            Property(t => t.CreatedBy)
            .IsRequired();

            Property(t => t.ModifiedBy)
            .IsRequired();

            Property(t => t.CreatedDate)
           .IsRequired()
           .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.ModifiedDate)
           .IsRequired()
           .HasColumnType(SqlTypes.DateTime2);
              

            //ToTable(TableSettings.ProjectMapping, TableSettings.DefaultSchema);

        }
    }
}
