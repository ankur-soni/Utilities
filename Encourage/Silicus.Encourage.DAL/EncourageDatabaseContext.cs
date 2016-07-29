using Silicus.Encourage.DAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Silicus.Encourage.DAL
{
    public class EncourageDatabaseContext : DataContextBase, IEncourageDatabaseContext
    {
        public EncourageDatabaseContext(string connectionString)
            : base(connectionString)
        {
           Database.SetInitializer<EncourageDatabaseContext>(null);
        }

        public int Update<T>(T item) where T : class
        {
            //if (Entry(item).State == EntityState.Detached)
            //    Set<T>().Add(item);

            Set<T>().Attach(item);

            //Entry(item).CurrentValues.SetValues(item);
            // Calling State on an entity in the Detached state will call DetectChanges() 
            // which is required to force an update. 
            Entry(item).State = EntityState.Modified;

            return SaveChanges();
        }

        public T Add<T>(T item) where T : class
        {
            var t = Set<T>().Add(item);

            SaveChanges();

            return t;
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public IQueryable<T> Query<T>(string property) where T : class
        {
            return Set<T>().Include(property).AsNoTracking();
        }

        public IQueryable<T> Query<T>(string property1, string property2) where T : class
        {
            return Set<T>().Include(property1).Include(property2).AsNoTracking();
        }

        public void Delete<T>(T item) where T : class
        {
            Set<T>().Attach(item);

            Set<T>().Remove(item);

            SaveChanges();
        }

    }
}
