using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.Interfaces
{
   public interface IDataContextFactory
    {
       ICommonDatabaseContext CreateCommonDbContext();
       IEncourageDatabaseContext CreateEncourageDbContext();
    }
}
