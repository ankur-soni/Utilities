using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainer.Entities
{
    public class DataContextFactory : IDataContextFactory
    {
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
