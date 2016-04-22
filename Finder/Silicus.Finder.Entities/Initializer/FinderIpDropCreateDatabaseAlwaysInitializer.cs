using System.Data.Entity;

namespace Silicus.Finder.Entities.Initializer
{
    public class FinderIpDropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<FinderIpDataContext>
    {
        protected override void Seed(FinderIpDataContext context)
        {
           // new BaseDatabaseInitializer().Seed(context);
        }
    }
}
