using System.Collections.Generic;
using System.Linq;
using Silicus.Finder.Entities;
using Silicus.Finder.Models.DataObjects;
using Silicus.Finder.Services.Interfaces;

namespace Silicus.Finder.Services
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
