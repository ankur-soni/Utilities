using System.Configuration;

namespace Silicus.Finder.Entities
{
    public class DataContextFactory : IDataContextFactory
    {
        public IDataContext Create(ConnectionType connetionType)
        {
            IDataContext dataContext = null;

            if (connetionType == ConnectionType.Ip)
            {
                dataContext = new FinderIpDataContext(ConfigurationManager.ConnectionStrings["FinderIpDataContext"].ConnectionString);
            }

            return dataContext;
        }
        public ICommonDataBaseContext CreateCommonDataBaseContext()
        {
            ICommonDataBaseContext dataContext = null;


            dataContext = new CommonDataBaseContext(ConfigurationManager.ConnectionStrings["CommonDataBaseConnection"].ConnectionString);
           
            return dataContext;
        }
    }
}