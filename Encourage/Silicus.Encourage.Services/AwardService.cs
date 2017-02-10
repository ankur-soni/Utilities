using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Silicus.Encourage.Services
{
    public class AwardService : IAwardService
    {
        private readonly IEncourageDatabaseContext _encourageDbcontext;
        private readonly INominationService _nominationService;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _CommonDbContext;
        private readonly ICustomDateService _customDateService;

        public AwardService(IDataContextFactory contextFactory, ICommonDbService commonDbService, INominationService nominationService, ICustomDateService customDateService)
        {
            _encourageDbcontext = contextFactory.CreateEncourageDbContext();
            _CommonDbContext = commonDbService.GetCommonDataBaseContext();
            _nominationService = nominationService;
            _customDateService = customDateService;
        }

        public bool AddNomination(Nomination nomination)
        {
            try
            {
                _encourageDbcontext.Add<Nomination>(nomination);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Award> GetAllAwards()
        {
            return _encourageDbcontext.Query<Award>().ToList();
        }

        public Award GetAwardFromNominationId(int nominationId)
        {
            var singleOrDefault = _encourageDbcontext.Query<Nomination>("Award").SingleOrDefault(nomination => nomination.Id == nominationId);
            if (singleOrDefault != null)
            {
                return singleOrDefault.Award;
            }

            return null;
        }

        public List<Engagement> GetProjectsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().SingleOrDefault(user => user.EmailAddress.Equals(email));
            var projectUnderCurrentUser = new List<Engagement>();
            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            var distinctClientIdsUnderCurrentManager = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID).GroupBy(engagement => engagement.ClientID).ToList();
            foreach (var clientid in distinctClientIdsUnderCurrentManager)
            {
                var data = _CommonDbContext.Query<Engagement>().FirstOrDefault(engagement => engagement.PrimaryProjectManagerID == currentUser.ID && engagement.ClientID == clientid.Key && engagement.Stage != closedProject);
                var firstOrDefault = _CommonDbContext.Query<Client>().FirstOrDefault(client => client.ID == clientid.Key);
                if (firstOrDefault != null && data != null)
                {
                    data.Name = firstOrDefault.Code;
                    projectUnderCurrentUser.Add(data);
                }
            }
            return projectUnderCurrentUser;
        }

        public List<Department> GetDepartmentsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().SingleOrDefault(user => user.EmailAddress == email);

            var resourcesInDepartment = from resource in _CommonDbContext.Query<Resource>()
                                        join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on resource.ID equals resourceHistory.ResourceID
                                        join title in _CommonDbContext.Query<Title>() on resourceHistory.TitleID equals title.ID
                                        join department in _CommonDbContext.Query<Department>() on title.DepartmentID equals department.ID
                                        where resource.DirectManager1ID == currentUser.ID
                                        select department;

            return resourcesInDepartment.Distinct().ToList();
        }

        public List<User> GetResourcesUnderDepartment(int DepartmentId, int userIdToExcept)
        {
            var resourcesUnderDept = (from user in _CommonDbContext.Query<User>()
                                     join resource in _CommonDbContext.Query<Resource>() on user.ID equals resource.UserID
                                     join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on resource.ID equals resourceHistory.ResourceID
                                     join title in _CommonDbContext.Query<Title>() on resourceHistory.TitleID equals title.ID
                                     join department in _CommonDbContext.Query<Department>() on title.DepartmentID equals department.ID
                                     where department.ID == DepartmentId && resource.DirectManager1ID == userIdToExcept
                                      select user).Distinct();
            int awardId = 1;
            var currentUserId = userIdToExcept;
            var recourcesInDepartmentUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.DepartmentId == DepartmentId && n.ManagerId == currentUserId && n.AwardId == awardId).ToList();
            resourcesUnderDept = resourcesUnderDept.Where(u => u.ID != userIdToExcept);
            var userList = resourcesUnderDept.ToList();
            foreach (var item in recourcesInDepartmentUnderCurrentManger)
            {
                userList.RemoveAll(u => u.ID == item.UserId);
            }
            var winners = _encourageDbcontext.Query<Shortlist>().Where(w => w.IsWinner == true).ToList();
            var winnersWithin12Months = new List<Shortlist>();
            var winnerNominationsWithin12Months = new List<Nomination>();
            foreach (var winner in winners)
            {
                var currentNomination = _nominationService.GetNomination(winner.NominationId);
                var customDate = _customDateService.GetCustomDate(currentNomination.AwardId);
                //var noOfMonthsFromLastWinningDate = (DateTime.Now.Year - winner.WinningDate.Value.Year) * 12 + (DateTime.Now.Month - winner.WinningDate.Value.Month);
                var noOfMonthsFromLastWinningDate = (customDate.Year - winner.WinningDate.Value.Year) * 12 + (customDate.Month - winner.WinningDate.Value.Month);
                if (noOfMonthsFromLastWinningDate <= 12)
                {
                    winnersWithin12Months.Add(winner);
                    winnerNominationsWithin12Months.Add(_nominationService.GetNomination(winner.NominationId));
                }

            }

            foreach (var winnerNomination in winnerNominationsWithin12Months)
            {
                userList.RemoveAll(user => user.ID == winnerNomination.UserId);
            }

            return userList;
        }

        public List<Criteria> GetCriteriasForAward(int awardId)
        {
            var criteriaList = _encourageDbcontext.Query<Criteria>().Where(criteria => criteria.AwardId == awardId).ToList();
            return criteriaList;
        }

        public List<User> GetResourcesInEngagement(int engagementId, int userIdToExcept, int awardId)
        {
            var userList = new List<User>();
            var customDate = _customDateService.GetCustomDate(awardId);
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            var firstOrDefault = currentUser.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var currentUserId = firstOrDefault.ID;
                var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
                var engagementForClient = _CommonDbContext.Query<Engagement>().FirstOrDefault(engagement => engagement.PrimaryProjectManagerID == currentUserId && engagement.ID == engagementId);
                var clientId = engagementForClient != null ? engagementForClient.ClientID : 0;
                var allEngagementIds = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUserId && engagement.ClientID == clientId && engagement.Stage != closedProject).Select(c => c.ID).ToList();

                var userInEngagement = from engagement in _CommonDbContext.Query<Engagement>()
                    join engagementRole in _CommonDbContext.Query<EngagementRole>() on engagement.ID equals engagementRole.EngagementID
                    join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                    join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                    join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                    where engagement.ClientID == clientId && engagement.Stage != closedProject && allEngagementIds.Contains(engagement.ID)
                    select user;

                var recourcesInEnggementUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.ProjectID == engagementId && n.ManagerId == currentUserId && n.AwardId == awardId).ToList();
                userInEngagement = userInEngagement.Except(currentUser);
                userList = userInEngagement.ToList();

                foreach (var item in recourcesInEnggementUnderCurrentManger)
                {
                    userList.RemoveAll(u => u.ID == item.UserId);
                }

                //Start-Winner can not be nominated for next one year.
                var winners = _encourageDbcontext.Query<Shortlist>().Where(w => w.IsWinner == true).ToList();
                var winnerNominationsWithin12Months = new List<Nomination>();
                foreach (var winner in winners)
                {
                    //var noOfMonthsFromLastWinningDate = (DateTime.Now.Year - winner.WinningDate.Value.Year) * 12 + (DateTime.Now.Month - winner.WinningDate.Value.Month);
                    var noOfMonthsFromLastWinningDate = (customDate.Year - winner.WinningDate.Value.Year) * 12 + (customDate.Month - winner.WinningDate.Value.Month);
                    var winnernomination = _nominationService.GetNomination(winner.NominationId);
                
                    var previousAwardId = winnernomination.AwardId;

                    if (noOfMonthsFromLastWinningDate <= 12 && previousAwardId == awardId)
                    {
                        winnerNominationsWithin12Months.Add(_nominationService.GetNomination(winner.NominationId));
                    }
                }

                foreach (var winnerNomination in winnerNominationsWithin12Months)
                {
                    userList.RemoveAll(user => user.ID == winnerNomination.UserId);
                }
                //End
            }

            return userList;
        }

        public List<User> GetResourcesForEditInEngagement(int engagementId, int userIdToExcept)
        {
            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            var engagementForClient = _CommonDbContext.Query<Engagement>().FirstOrDefault(engagement => engagement.PrimaryProjectManagerID == userIdToExcept && engagement.ID == engagementId);
            var clientId = engagementForClient != null ? engagementForClient.ClientID : 0;
            var allEngagementIds = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == userIdToExcept && engagement.ClientID == clientId && engagement.Stage != closedProject).Select(c => c.ID).ToList();

            var userInEngagement = from engagement in _CommonDbContext.Query<Engagement>()
                                   join engagementRole in _CommonDbContext.Query<EngagementRole>() on engagement.ID equals engagementRole.EngagementID
                                   join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagement.ClientID == clientId && engagement.Stage != closedProject && allEngagementIds.Contains(engagement.ID)
                                   select user;

            userInEngagement = userInEngagement.Where(u => u.ID != userIdToExcept);
            return userInEngagement.ToList();
        }

        public List<User> GetResourcesForEditInDepartment(int DepartmentId, int userIdToExcept)
        {
            var resourcesUnderDept = from user in _CommonDbContext.Query<User>()
                                     join resource in _CommonDbContext.Query<Resource>() on user.ID equals resource.UserID
                                     join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on resource.ID equals resourceHistory.ResourceID
                                     join title in _CommonDbContext.Query<Title>() on resourceHistory.TitleID equals title.ID
                                     join department in _CommonDbContext.Query<Department>() on title.DepartmentID equals department.ID
                                     where department.ID == DepartmentId
                                     select user;

            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            resourcesUnderDept = resourcesUnderDept.Except(currentUser);
            return resourcesUnderDept.ToList();
        }


        public int GetUserIdFromEmail(string email)
        {
            var firstOrDefault = _CommonDbContext.Query<User>().FirstOrDefault(user => user.EmailAddress == email);
            if (firstOrDefault != null)
            {
                return firstOrDefault.ID;
            }
            return 0;
        }

        public List<WinnerData> GetWinnerData()
        {
            var allWinnersWithoutDateFilter = _encourageDbcontext.Query<Shortlist>().Where(shortlist => shortlist.IsWinner == true ).ToList();
            var allWinners = new List<Shortlist>();
                
            foreach (var winner in allWinnersWithoutDateFilter)
            {
                var currentNomination = _nominationService.GetNomination(winner.NominationId);
                var customDate = _customDateService.GetCustomDate(currentNomination.AwardId);
                allWinners.Add(_encourageDbcontext.Query<Shortlist>().FirstOrDefault(shortlist => shortlist.IsWinner == true && shortlist.WinningDate.Value.Month == customDate.Month &&
                                                                                                  shortlist.WinningDate.Value.Year == customDate.Year));
            }
            //var allWinners = _encourageDbcontext.Query<Shortlist>().Where(shortlist => shortlist.IsWinner == true && shortlist.WinningDate.Value.Month == DateTime.Now.Month && shortlist.WinningDate.Value.Year == DateTime.Now.Year).ToList();
            
            var winnersList = new List<WinnerData>();

            foreach (var winner in allWinners)
            {
                string projectName = string.Empty;
                var nominationOfWinnner = _encourageDbcontext.Query<Nomination>().FirstOrDefault(nomination => nomination.Id == winner.NominationId);

                var user = _CommonDbContext.Query<User>().FirstOrDefault(u => u.ID == nominationOfWinnner.UserId);
                if (user != null)
                {
                    var userName = user.DisplayName;
                    var award = _encourageDbcontext.Query<Award>().FirstOrDefault(a => a.Id == nominationOfWinnner.AwardId);
                    if (award != null)
                    {
                        var awardName = award.Name;
                        var manager = _CommonDbContext.Query<User>().FirstOrDefault(u => u.ID == nominationOfWinnner.ManagerId);
                        if (manager != null)
                        {
                            var managerName = manager.DisplayName;

                            if (nominationOfWinnner != null && nominationOfWinnner.ProjectID != null)
                            {
                                var engagement = _CommonDbContext.Query<Engagement>().FirstOrDefault(enagegement => enagegement.ID == nominationOfWinnner.ProjectID);
                                if (engagement != null)
                                {
                                    projectName = engagement.Name;
                                }
                            }
                            else
                            {
                                var engagement = _CommonDbContext.Query<Department>().FirstOrDefault(enagegement => enagegement.ID == nominationOfWinnner.DepartmentId);
                                if (engagement != null)
                                {
                                    projectName = engagement.Name;
                                }
                            }

                            if (nominationOfWinnner != null)
                            {
                                if (nominationOfWinnner.NominationDate != null)
                                {
                                    var awardPeriod = nominationOfWinnner.NominationDate.Value.ToString("MMMM") + " - " + nominationOfWinnner.NominationDate.Value.Year.ToString();
                                    var winnerData = new WinnerData()
                                    {
                                        Name = userName,
                                        AwardName = awardName,
                                        AwardPeriod = awardPeriod,
                                        ManagerName = managerName,
                                        ProjectName = projectName
                                    };

                                    winnersList.Add(winnerData);
                                }
                            }
                        }
                    }
                }
            }
            return winnersList;
        }
        public List<string> GetEmailAddressOfManager(string name)
        {
            var emailaddress = _CommonDbContext.Query<User>().Where(user => user.DisplayName == name).Select(u => u.EmailAddress).ToList();
            return emailaddress;
        }

        public Award GetAwardById(int awardId)
        {
            return _encourageDbcontext.Query<Award>().FirstOrDefault(x => x.Id == awardId);
        }

        public Award GetAwardByCode(string awardName)
        {
            return _encourageDbcontext.Query<Award>().FirstOrDefault(x => x.Code == awardName);
        }

        public string GetAwardNameById(int awardId)
        {
            var firstOrDefault = _encourageDbcontext.Query<Award>().FirstOrDefault(x => x.Id == awardId);
            if (firstOrDefault != null)
            {
                return firstOrDefault.Name;
            }
            return null;
        }

        public User GetUserById(int userId)
        {
            return _CommonDbContext.Query<User>().FirstOrDefault(u => u.ID == userId);
        }
    }
}
