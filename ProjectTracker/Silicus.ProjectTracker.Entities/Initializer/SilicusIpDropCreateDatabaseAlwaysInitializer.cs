using System.Data.Entity;

namespace Silicus.ProjectTracker.Entities.Initializer
{
    public class SilicusIpDropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<ProjectTrackerIpDataContext>
    {
        protected override void Seed(ProjectTrackerIpDataContext context)
        {
            BaseDatabaseInitializer.Seed(context);
        }
    }
}
