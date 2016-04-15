using System.Linq;
using System.Data.Entity;

namespace Silicus.UtilityContainerr.Entities
{
    public class CommonDataBaseContext : DataContextBase, ICommonDataBaseContext
    {
        public CommonDataBaseContext(string connectionString)
            : base(connectionString)
        {
            
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }
    }
}
