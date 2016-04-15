using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainerr.Entities
{
    public class DataContextFactory : IDataContextFactory
    {
        public ILocalDataBaseContext CreateLocalDBContext()
        {
            ILocalDataBaseContext dataContext = null;

                dataContext =
                    new LocalDataBaseContext(
                        ConfigurationManager.ConnectionStrings["localDataBaseConnection"].ConnectionString);
            

            return dataContext;
        }

        public ICommonDataBaseContext CreateCommonDBContext()
        {
            ICommonDataBaseContext dataContext = null;

                dataContext =
                    new CommonDataBaseContext(
                        ConfigurationManager.ConnectionStrings["CommonDataBaseConnection"].ConnectionString);
            

            return dataContext;
        }
    }
}
