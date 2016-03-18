using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Silicus.ProjectTracker.Models.DataObjects;

namespace Silicus.ProjectTracker.Entities.EntityConfigurations
{ 

    internal class ProjectComplaintMap : EntityTypeConfiguration<ProjectComplaint>
    {
        public ProjectComplaintMap()
        {
            HasKey(o => o.ComplaintId);

            Property(p => p.ComplaintId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Description)
            .IsOptional()
            .HasMaxLength(500)
            .HasColumnType(SqlTypes.Varchar);

            Property(t => t.ProjectId)
            .IsRequired();

            Property(t => t.WeekId)
           .IsRequired();
            
            Property(t => t.Description)
            .IsOptional()
            .HasMaxLength(500)
            .HasColumnType(SqlTypes.Varchar);

            Property(t => t.PlannedAction)
           .IsOptional()
           .HasColumnType(SqlTypes.Varchar)
           .HasMaxLength(500);

            Property(t => t.Remarks)
           .IsOptional()
           .HasColumnType(SqlTypes.Varchar)
           .HasMaxLength(500);
                        
            ToTable(TableSettings.ProjectComplaint, TableSettings.DefaultSchema);
        }
    }
}
