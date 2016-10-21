using System.Collections.Generic;
using System.Linq;
using Silicus.Ensure.Entities;
using Silicus.Ensure.Models.DataObjects;
using Silicus.Ensure.Services.Interfaces;

namespace Silicus.Ensure.Services
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
