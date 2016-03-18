
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{
    internal class ProjectSummaryMap :EntityTypeConfiguration<ProjectSummary>
    {
        public ProjectSummaryMap()
        {
            HasKey(o => o.ProjectSummaryId);

            Property(p => p.ProjectSummaryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.FeedBack)
            .IsOptional()
            .HasColumnType(SqlTypes.DateTime2);
            
            Property(t => t.Remarks)
           .IsOptional()
           .HasColumnType(SqlTypes.Varchar)
           .HasMaxLength(500);

            Property(t => t.StartDate)
           .IsRequired()
           .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.EndDate)
            .IsRequired()
            .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.ReleaseDate)
           .IsRequired()
           .HasColumnType(SqlTypes.DateTime2);

            Property(t => t.ReleaseNumber)
           .HasColumnType(SqlTypes.Varchar)
           .HasMaxLength(20);
           
            ToTable(TableSettings.ProjectSummary, TableSettings.DefaultSchema);

            //Relationships
            //HasRequired(r => r.Project)
            //    .WithMany(a => a.ProjectSummaries)
            //    .HasForeignKey(b => b.ProjectId)
            //    .WillCascadeOnDelete(false);

            
        }

    }
}

