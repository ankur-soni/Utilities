using Silicus.Ensure.Entities;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
{
    public class ProjectService :IProjectService
    {
        private readonly IDataContext context;

        public ProjectService(IDataContextFactory dataContextFactory)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
        }
    }
}


