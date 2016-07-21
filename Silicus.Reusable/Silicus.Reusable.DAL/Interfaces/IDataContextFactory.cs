using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Reusable.DAL.Interfaces
{
    public interface IDataContextFactory
    {
        ICommonDatabaseContext CreateCommonDbContext();
        IReusableDatabaseContext CreateReusableDbContext();
    }
}
