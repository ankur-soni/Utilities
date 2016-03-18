using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Silicus.ProjectTracker.Logger
{
    [ExcludeFromCodeCoverage]
    public class LoggerContext : DbContext, IDataContext
    {
        public LoggerContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {       
        }

        public DbSet<LogMessage> LogMessages { get; set; }

        public IQueryable<T> Query<T>() where T : class
        {
            throw new NotImplementedException();
        }
                
        public T Add<T>(T item) where T : class
        {
            var t = Set<T>().Add(item);

            SaveChanges();

            return t;
        }

        public void AddAll<T>(IEnumerable<T> items) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
