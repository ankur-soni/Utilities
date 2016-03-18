using System.Collections.Generic;
using System.Linq;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Services.Interfaces;

namespace Silicus.ProjectTracker.Services
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IDataContext context;
        private IGenericService _genericService;

        public UserDashboardService(IDataContextFactory dataContextFactory, IGenericService genericService)
        {
            this.context = dataContextFactory.Create(ConnectionType.Ip);
            _genericService = genericService;
        }

        public IEnumerable<ProjectStatus> GetUserProjectStatus(string userName)
        {
            var userProject = context.Query<ProjectMapping>().Where(um => um.UserName == userName);

            var projectStatusList = context.Query<ProjectStatus>()
                .Where(p => p.ProjectId == (userProject.Where(uu => uu.ProjectId == p.ProjectId)).FirstOrDefault().ProjectId);

            return projectStatusList;
        }
    }
}
