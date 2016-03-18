using Silicus.ProjectTracker.Models.DataObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class PaymentDetailsMap : EntityTypeConfiguration<PaymentDetails>
    {
        public PaymentDetailsMap()
        {
            HasKey(o => o.PaymentDetailId);

            Property(p => p.PaymentDetailId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable(TableSettings.PaymentDetails, TableSettings.DefaultSchema);
        }
    }
}
