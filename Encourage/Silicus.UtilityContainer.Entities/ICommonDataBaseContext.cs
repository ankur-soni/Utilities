using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Entities
{
     public interface ICommonDataBaseContext : IDisposable
     {
         T Add<T>(T item) where T : class;
         int Update<T>(T item) where T : class;
         IQueryable<T> Query<T>() where T : class;
         void Delete<T>(T item) where T : class;
         int SaveChanges();
     }
    
}
