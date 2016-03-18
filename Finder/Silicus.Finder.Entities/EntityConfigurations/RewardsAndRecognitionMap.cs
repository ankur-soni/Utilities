using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.Finder.Models.DataObjects;

namespace Silicus.Finder.Entities.EntityConfigurations
{
    class RewardsAndRecognitionMap : EntityTypeConfiguration<RewardsAndRecognition>
    {
        public RewardsAndRecognitionMap()
        {
            HasKey(o => o.RewardsAndRecognitionId);

            Property(p => p.RewardsAndRecognitionId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

          // Property(p => p.StartDate < p.ExpectedEndDate);

            ToTable(TableSettings.RewardsAndRecognitions, TableSettings.DefaultSchema);
        }}
}
