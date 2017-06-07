using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Silicus.FrameWorx.Utility;
using System.Data.Entity.Validation;

namespace Silicus.Ensure.Entities
{
    /// <summary>
    /// This class provides a generic repository to access 
    /// the data store.
    /// </summary>
    public class SilicusIpDataContext : DataContextBase, IDataContext
    {
        //public SilicusIpDataContext() : base(@"Data Source=.\SQLEXPRESS2014;Initial Catalog=UtilityContainer2;Integrated Security=true")
        //{
                
        //}
        public SilicusIpDataContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<SilicusIpDataContext>(null);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public T Add<T>(T item) where T : class
        {
            Guard.ArgumentNotNull(item, "item");

            var t = Set<T>().Add(item);

            SaveChanges();

            return t;
        }

        public void AddAll<T>(IEnumerable<T> items) where T : class
        {
            Guard.ArgumentNotNull(items, "items");

            foreach (var item in items)
            {
                Set<T>().Add(item);
            }

            SaveChanges();
        }

        public void BulkAddAll<T>(IEnumerable<T> items) where T : class
        {
            Guard.ArgumentNotNull(items, "items");

            using (var scope = new TransactionScope())
            {
                var autoDetectChangesEnabledBefore = Configuration.AutoDetectChangesEnabled;
                var validateOnSaveEnabled = Configuration.ValidateOnSaveEnabled;

                // It is said to make the performance better.
                Configuration.AutoDetectChangesEnabled = false;
                Configuration.ValidateOnSaveEnabled = false;

                int count = 0;
                foreach (var entityToInsert in items)
                {
                    ++count;
                    AddToContext(entityToInsert, count, 100);
                }

                SaveChanges();

                scope.Complete();

                Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabledBefore;
                Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            }
        }

        public int Update<T>(T item) where T : class
        {
            Guard.ArgumentNotNull(item, "item");

            Set<T>().Attach(item);

            // Calling State on an entity in the Detached state will call DetectChanges() 
            // which is required to force an update. 
            Entry(item).State = EntityState.Modified;

            return SaveChanges();
        }

        public void AttachAndMakeStateModified<T>(T item) where T : class
        {
            Guard.ArgumentNotNull(item, "item");

            Set<T>().Attach(item);

            Entry(item).State = EntityState.Modified;
        }

        public void UpdateAll<T>(IEnumerable<T> items) where T : class
        {
            Guard.ArgumentNotNull(items, "items");

            foreach (var item in items)
            {
                Set<T>().Attach(item);
                Entry(item).State = EntityState.Modified;
            }

            SaveChanges();
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void Delete<T>(T item) where T : class
        {
            Guard.ArgumentNotNull(item, "item");

            Set<T>().Attach(item);

            Set<T>().Remove(item);

            SaveChanges();
        }

        public void DeleteAll<T>(IEnumerable<T> items) where T : class
        {
            Guard.ArgumentNotNull(items, "items");

            var autoDetectChangesEnabledBefore = Configuration.AutoDetectChangesEnabled;
            var validateOnSaveEnabled = Configuration.ValidateOnSaveEnabled;

            // It is said to make the performance better.
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;

            foreach (var item in items)
            {
                Set<T>().Attach(item);
                Set<T>().Remove(item);
            }

            SaveChanges();

            Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabledBefore;
            Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;

        }

        public T TryAdd<T>(T item) where T : class
        {
            T addedItem = null;
            try
            {
                addedItem = Set<T>().Add(item);
                this.SaveChanges();
            }
            catch
            {
                // In case the item can not be added log here and do not throw
                // Only unique items will be added
            }

            return addedItem;
        }

        private void AddToContext<T>(T entity, int count, int commitCount) where T : class
        {
            Set<T>().Add(entity);

            if (count%commitCount == 0)
            {
                SaveChanges();
            }
        }
    }
}
