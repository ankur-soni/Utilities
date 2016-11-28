using System.Configuration;

namespace Silicus.Ensure.Entities
{
    public class DataContextFactory : IDataContextFactory
    {
        public IDataContext Create(ConnectionType connetionType)
        {
            IDataContext dataContext = null;

            if (connetionType == ConnectionType.Ip)
            {
                dataContext = new SilicusIpDataContext(ConfigurationManager.ConnectionStrings["SilicusIpDataContext"].ConnectionString);
            }

            return dataContext;
        }
    }
}