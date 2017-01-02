using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Finder.Entities
{
    class CommonDataBaseContext : DataContextBase, ICommonDataBaseContext
    {
        public CommonDataBaseContext(string connectionString)
            : base(connectionString)
        {
            
        }

           public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }
        
    }
}
