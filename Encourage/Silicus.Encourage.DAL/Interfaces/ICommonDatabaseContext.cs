using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.Interfaces
{
    public interface ICommonDatabaseContext : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
    }
}
