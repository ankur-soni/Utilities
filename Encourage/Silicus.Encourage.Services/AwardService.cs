using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models.DataObjects;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Encourage.Services
{
    public class AwardService:IAwardService
    { 
        private readonly IDataContextFactory _contextFactory;
        private readonly IEncourageDatabaseContext _encourageDbcontext;
        private readonly ICommonDbService _commonDbService;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _CommonDbContext;


        public AwardService(IDataContextFactory contextFactory, ICommonDbService commonDbService)
        {
            _contextFactory = contextFactory;
            _encourageDbcontext = _contextFactory.CreateEncourageDbContext();
            _commonDbService = commonDbService;
            _CommonDbContext = _commonDbService.GetCommonDataBaseContext();
       }

        public IEnumerable<Models.DataObjects.Award> GetAllAwards()
        {
            return _encourageDbcontext.Query<Award>().ToList();
        }

        public List<Engagement> GetProjectsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.EmailAddress.Equals(email)).SingleOrDefault();
            var projectUnderCurrentUser = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID).ToList();
            return projectUnderCurrentUser;
        }

        public List<Criteria> GetCriteriasForAward(int awardId)
        {
            var criteriaList = _encourageDbcontext.Query<Criteria>().Where(criteria => criteria.AwardId == awardId).ToList();
            return criteriaList;
        }

        public List<User> GetResourcesInEngagement(int engagementId)
        {
            var userInEngagement = from engagementRole in _CommonDbContext.Query<EngagementRole>()
                                   join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagementRole.EngagementID == engagementId
                                   select user;
           
            return userInEngagement.ToList();
        }

    }
}
