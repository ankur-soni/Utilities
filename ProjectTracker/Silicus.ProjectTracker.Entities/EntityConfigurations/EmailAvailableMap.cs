using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class EmailAvailableMap : EntityTypeConfiguration<EmailAvailable>
    {
        public EmailAvailableMap()
        {
            HasKey(o => o.Id);

            Property(p => p.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name)
                .IsRequired()
                //.HasColumnType(varchar)
                .HasMaxLength(100);
                

            // Table
            ToTable(TableSettings.EmailAvailable, TableSettings.DefaultSchema);
        }
    }
}
