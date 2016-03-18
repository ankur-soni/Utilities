using System.Data.Entity;

namespace Silicus.ProjectTracker.Entities.Initializer
{
    public class SilicusIpDropCreateDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<ProjectTrackerIpDataContext>
    {
        protected override void Seed(ProjectTrackerIpDataContext context)
        {
            BaseDatabaseInitializer.Seed(context);
        }
    }
}
