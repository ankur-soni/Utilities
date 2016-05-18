using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Encourage.Services
{
    public class AwardService : IAwardService
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

        public IEnumerable<Award> GetAllAwards()
        {
            return _encourageDbcontext.Query<Award>().ToList();
        }

        public List<Engagement> GetProjectsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.EmailAddress.Equals(email)).SingleOrDefault();
            var projectUnderCurrentUser = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID).ToList();
            return projectUnderCurrentUser;
        }


        public List<Department> GetDepartmentsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.EmailAddress == email).SingleOrDefault();

            var resourcesInDepartment = from resource in _CommonDbContext.Query<Resource>()
                                        join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on resource.ID equals resourceHistory.ResourceID
                                        join title in _CommonDbContext.Query<Title>() on resourceHistory.TitleID equals title.ID
                                        join department in _CommonDbContext.Query<Department>() on title.DepartmentID equals department.ID
                                        where resource.DirectManager1ID == 3779
                                        select department;

            return resourcesInDepartment.Distinct().ToList();
        }

        public List<Criterion> GetCriteriasForAward(int awardId)
        {
            var criteriaList = _encourageDbcontext.Query<Criterion>().Where(criteria => criteria.AwardId == awardId).ToList();
            return criteriaList;
        }

        public List<User> GetResourcesInEngagement(int engagementId, int userIdToExcept)
        {
            var userInEngagement = from engagementRole in _CommonDbContext.Query<EngagementRole>()
                                   join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagementRole.EngagementID == engagementId
                                   select user;

            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            userInEngagement = userInEngagement.Except(currentUser);

            return userInEngagement.ToList();
        }


        public int GetUserIdFromEmail(string email)
        {
            return _CommonDbContext.Query<User>().Where(user => user.EmailAddress == email).FirstOrDefault().ID;
        }
    }
}
