using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Entities
{
    public interface ICommonDataBaseContext : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        int SaveChanges();
    }
}
