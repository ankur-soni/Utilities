using System.Data.Entity;

namespace Silicus.ProjectTracker.Entities.Initializer
{
    public class SilicusIpCreateDatabaseIfNotExistsInitializer : CreateDatabaseIfNotExists<ProjectTrackerIpDataContext>
    {
        protected override void Seed(ProjectTrackerIpDataContext context)
        {
            BaseDatabaseInitializer.Seed(context);
        }
    }
}
