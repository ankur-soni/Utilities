using System.Linq;
using System.Collections.Generic;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class ManagerService : IManager
    {
        private readonly IDataContext context;

        public ManagerService(IDataContextFactory dataContextFactory)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
        }

        public IList<Manager> GetManagers()
        {
            var managerList = this.context.Query<Manager>().ToList();
            return managerList;

        }
        

    }
}
