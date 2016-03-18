using System.Configuration;

namespace Silicus.ProjectTracker.Entities
{
    public class DataContextFactory : IDataContextFactory
    {
        public IDataContext Create(ConnectionType connetionType)
        {
            IDataContext dataContext = null;

            if (connetionType == ConnectionType.Ip)
            {
                dataContext = new ProjectTrackerIpDataContext(ConfigurationManager.ConnectionStrings["ProjectTrackerIpDataContext"].ConnectionString);
            }

            return dataContext;
        }
    }
}