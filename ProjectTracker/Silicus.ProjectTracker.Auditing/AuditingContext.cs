using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Silicus.ProjectTracker.Auditing
{
    [ExcludeFromCodeCoverage]
    public class AuditingContext : DbContext, IDataContext
    {
        public AuditingContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {       
        }

        // To instruct EF about AuditMessage table in a simple way.
        public DbSet<AuditMessage> AuditMessages { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }
                
        public T Add<T>(T item) where T : class
        {
            var t = Set<T>().Add(item);

            SaveChanges();

            return t;
        }
    }
}
