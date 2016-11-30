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
        private readonly IDataContextFactory _contextFactory;
        private readonly IEncourageDatabaseContext _encourageDbcontext;
        private readonly ICommonDbService _commonDbService;
        private readonly INominationService _nominationService;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _CommonDbContext;

        public AwardService(IDataContextFactory contextFactory, ICommonDbService commonDbService, INominationService nominationService)
        {
            _contextFactory = contextFactory;
            _encourageDbcontext = _contextFactory.CreateEncourageDbContext();
            _commonDbService = commonDbService;
            _CommonDbContext = _commonDbService.GetCommonDataBaseContext();
            _nominationService = nominationService;
        }

        public bool AddNomination(Nomination nomination)
        {
            try
            {
                _encourageDbcontext.Add<Nomination>(nomination);
                return true;
            }
            catch (Exception Ex)
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
            return _encourageDbcontext.Query<Nomination>("Award").Where(nomination => nomination.Id == nominationId).SingleOrDefault().Award;
        }

        public List<Engagement> GetProjectsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.EmailAddress.Equals(email)).SingleOrDefault();
            // var projectUnderCurrentUser = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID).ToList()
            //  return projectUnderCurrentUser;
            var projectUnderCurrentUser = new List<Engagement>();
            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            var distinctClientIdsUnderCurrentManager = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID).GroupBy(engagement => engagement.ClientID).ToList();
            foreach (var clientid in distinctClientIdsUnderCurrentManager)
            {
                var data = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUser.ID && engagement.ClientID == clientid.Key && engagement.Stage != closedProject).FirstOrDefault();
                data.Name = _CommonDbContext.Query<Client>().Where(client => client.ID == clientid.Key).FirstOrDefault().Code;
                projectUnderCurrentUser.Add(data);

            }
            return projectUnderCurrentUser;
        }

        public List<Department> GetDepartmentsUnderCurrentUserAsManager(string email)
        {
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.EmailAddress == email).SingleOrDefault();

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
            var resourcesUnderDept = from user in _CommonDbContext.Query<User>()
                                     join resource in _CommonDbContext.Query<Resource>() on user.ID equals resource.UserID
                                     join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on resource.ID equals resourceHistory.ResourceID
                                     join title in _CommonDbContext.Query<Title>() on resourceHistory.TitleID equals title.ID
                                     join department in _CommonDbContext.Query<Department>() on title.DepartmentID equals department.ID
                                     where department.ID == DepartmentId
                                     select user;
            int awardId = 1;
            //
            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            var currentUserId = currentUser.FirstOrDefault().ID;
            var recourcesInDepartmentUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.DepartmentId == DepartmentId && n.ManagerId == currentUserId && n.AwardId == awardId).ToList();
            resourcesUnderDept = resourcesUnderDept.Except(currentUser);
            var userList = resourcesUnderDept.ToList();
            foreach (var item in recourcesInDepartmentUnderCurrentManger)
            {
                userList.RemoveAll(u => u.ID == item.UserId);
            }
            ////
            var winners = _encourageDbcontext.Query<Shortlist>().Where(w => w.IsWinner == true).ToList();
            var winnersWithin12Months = new List<Shortlist>();
            var winnerNominationsWithin12Months = new List<Nomination>();
            foreach (var winner in winners)
            {
                var noOfMonthsFromLastWinningDate = (DateTime.Now.Year - winner.WinningDate.Value.Year) * 12 + (DateTime.Now.Month - winner.WinningDate.Value.Month);
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
            var currentAwardName = _encourageDbcontext.Query<Award>().Where(a => a.Id == awardId).FirstOrDefault().Name;

            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            var currentUserId = currentUser.FirstOrDefault().ID;
            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            var clientId = _CommonDbContext.Query<Engagement>().Where( engagement => engagement.PrimaryProjectManagerID == currentUserId && engagement.ID == engagementId).FirstOrDefault().ClientID;

            var userInEngagement = from engagement in _CommonDbContext.Query<Engagement>()
                                   join engagementRole in _CommonDbContext.Query<EngagementRole>() on engagement.ID equals engagementRole.EngagementID
                                   join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagement.ClientID == clientId && engagement.Stage != closedProject && engagement.ID == engagementId
                                   select user;
           

            var recourcesInEnggementUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.ProjectID == engagementId && n.ManagerId == currentUserId && n.AwardId == awardId).ToList();
            userInEngagement = userInEngagement.Except(currentUser);
            var userList = userInEngagement.ToList();

            foreach (var item in recourcesInEnggementUnderCurrentManger)
            {
                userList.RemoveAll(u => u.ID == item.UserId);
            }

            //Start-Winner can not be nominated for next one year.
            var winners = _encourageDbcontext.Query<Shortlist>().Where(w => w.IsWinner == true).ToList();
            var winnersWithin12Months = new List<Shortlist>();
            var winnerNominationsWithin12Months = new List<Nomination>();
            foreach (var winner in winners)
            {
                var noOfMonthsFromLastWinningDate = (DateTime.Now.Year - winner.WinningDate.Value.Year) * 12 + (DateTime.Now.Month - winner.WinningDate.Value.Month);
                var winnernomination = _nominationService.GetNomination(winner.NominationId);

                var previousAwardName = _encourageDbcontext.Query<Award>().Where(a => a.Id == winnernomination.AwardId).FirstOrDefault().Name;

                var previousAwardId = winnernomination.AwardId;

                if (noOfMonthsFromLastWinningDate <= 12)
                {
                    if (previousAwardId == awardId)
                    {

                        winnerNominationsWithin12Months.Add(_nominationService.GetNomination(winner.NominationId));
                    }
                }

            }

            foreach (var winnerNomination in winnerNominationsWithin12Months)
            {
                userList.RemoveAll(user => user.ID == winnerNomination.UserId);
            }

            
            //End


            return userList;
        }

        //public List<User> GetResourcesInEngagement(int engagementId, int userIdToExcept, int awardId)
        //{
        //    var userInEngagement = from engagementRole in _CommonDbContext.Query<EngagementRole>()
        //                           join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
        //                           join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
        //                           join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
        //                           where engagementRole.EngagementID == engagementId
        //                           select user;

        //    var currentAwardName = _encourageDbcontext.Query<Award>().Where(a => a.Id == awardId).FirstOrDefault().Name;

        //    var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
        //    var currentUserId = currentUser.FirstOrDefault().ID;


        //    var recourcesInEnggementUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.ProjectID == engagementId && n.ManagerId == currentUserId && n.AwardId == awardId).ToList();

        //    userInEngagement = userInEngagement.Except(currentUser);
        //    var userList = userInEngagement.ToList();
        //    foreach (var item in recourcesInEnggementUnderCurrentManger)
        //    {
        //        userList.RemoveAll(u => u.ID == item.UserId);
        //    }
        //    //Start-Winner can not be nominated for next one year.
        //    var winners = _encourageDbcontext.Query<Shortlist>().Where(w => w.IsWinner == true).ToList();
        //    var winnersWithin12Months = new List<Shortlist>();
        //    var winnerNominationsWithin12Months = new List<Nomination>();
        //    foreach (var winner in winners)
        //    {
        //        var noOfMonthsFromLastWinningDate = (DateTime.Now.Year - winner.WinningDate.Value.Year) * 12 + (DateTime.Now.Month - winner.WinningDate.Value.Month);
        //        var winnernomination = _nominationService.GetNomination(winner.NominationId);

        //        var previousAwardName = _encourageDbcontext.Query<Award>().Where(a => a.Id == winnernomination.AwardId).FirstOrDefault().Name;

        //        var previousAwardId = winnernomination.AwardId;

        //        if (noOfMonthsFromLastWinningDate <= 12)
        //        {
        //            if (previousAwardId == awardId)
        //            {

        //                winnerNominationsWithin12Months.Add(_nominationService.GetNomination(winner.NominationId));
        //            }
        //        }

        //    }

        //    foreach (var winnerNomination in winnerNominationsWithin12Months)
        //    {
        //        userList.RemoveAll(user => user.ID == winnerNomination.UserId);
        //    }
        //    //End


        //    return userList;
        //}


        public List<User> GetResourcesForEditInEngagement(int engagementId, int userIdToExcept)
        {

            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            var currentUserId = currentUser.FirstOrDefault().ID;
            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            var clientId = _CommonDbContext.Query<Engagement>().Where(engagement => engagement.PrimaryProjectManagerID == currentUserId && engagement.ID == engagementId).FirstOrDefault().ClientID;

            var userInEngagement = from engagement in _CommonDbContext.Query<Engagement>()
                                   join engagementRole in _CommonDbContext.Query<EngagementRole>() on engagement.ID equals engagementRole.EngagementID
                                   join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
                                   join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
                                   join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
                                   where engagement.ClientID == clientId && engagement.Stage != closedProject && engagement.ID == engagementId
                                   select user;
            //var userInEngagement = from engagementRole in _CommonDbContext.Query<EngagementRole>()
            //                       join resourceHistory in _CommonDbContext.Query<ResourceHistory>() on engagementRole.ResourceHistoryID equals resourceHistory.ID
            //                       join resource in _CommonDbContext.Query<Resource>() on resourceHistory.ResourceID equals resource.ID
            //                       join user in _CommonDbContext.Query<User>() on resource.UserID equals user.ID
            //                       where engagementRole.EngagementID == engagementId
            //                       select user;

            userInEngagement = userInEngagement.Except(currentUser);
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
            return _CommonDbContext.Query<User>().Where(user => user.EmailAddress == email).FirstOrDefault().ID;
        }

        public List<WinnerData> GetWinnerData()
        {

            var allWinners = _encourageDbcontext.Query<Shortlist>().Where(shortlist => shortlist.IsWinner == true && shortlist.WinningDate.Value.Month == DateTime.Now.Month && shortlist.WinningDate.Value.Year == DateTime.Now.Year).ToList();
            var winnersList = new List<WinnerData>();
            //using (Silicus.Encourage.DAL.Interfaces.ICommonDatabaseContext _commonDbContext = new Silicus.Encourage.DAL.DataContextFactory().CreateCommonDbContext())
            //{
            foreach (var winner in allWinners)
            {
                string projectName = string.Empty;
                var nominationOfWinnner = _encourageDbcontext.Query<Nomination>().Where(nomination => nomination.Id == winner.NominationId).FirstOrDefault();

                var userName = _CommonDbContext.Query<User>().Where(user => user.ID == nominationOfWinnner.UserId).FirstOrDefault().DisplayName;
                var awardName = _encourageDbcontext.Query<Award>().Where(award => award.Id == nominationOfWinnner.AwardId).FirstOrDefault().Name;
                var managerName = _CommonDbContext.Query<User>().Where(user => user.ID == nominationOfWinnner.ManagerId).FirstOrDefault().DisplayName;

                if (nominationOfWinnner.ProjectID != null)
                {
                    var engagement = _CommonDbContext.Query<Engagement>().Where(enagegement => enagegement.ID == nominationOfWinnner.ProjectID).FirstOrDefault();
                    if (engagement != null)
                    {
                        projectName = engagement.Name;
                    }
                }
                else
                {
                    var engagement  = _CommonDbContext.Query<Department>().Where(enagegement => enagegement.ID == nominationOfWinnner.DepartmentId).FirstOrDefault();
                    if(engagement != null)
                    {
                        projectName = engagement.Name;
                    }
                }

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
            // }

            return winnersList;
        }
        public List<string> GetEmailAddressOfManager(string name)
        {
            var emailaddress = _CommonDbContext.Query<User>().Where(user => user.DisplayName == name).Select(u => u.EmailAddress).ToList();

            //#region Getting ManagerId
            //var lstManagerIds = (from nomination in _encourageDbcontext.Query<Nomination>()
            //                join shortlist in _encourageDbcontext.Query<Shortlist>()
            //                on nomination.Id equals shortlist.NominationId
            //                where shortlist.IsWinner == true && nomination.IsSubmitted == true
            //                && shortlist.WinningDate.Value.Month == DateTime.Now.Month
            //                && shortlist.WinningDate.Value.Year == DateTime.Now.Year
            //                select nomination.ManagerId).ToList();

            //if (lstManagerIds.Any())
            //{
            //    var lstManagerEmails = (from user in _CommonDbContext.Query<User>()
            //                        where lstManagerIds.Contains(user.ID)
            //                        select user.EmailAddress).ToList();
            return emailaddress;
            //}
            //#endregion
            //return null;
        }
    }
}
