﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silicus.Encourage.Services
{
    public class AwardService : IAwardService
    {
        private readonly IDataContextFactory _contextFactory;
        private readonly IEncourageDatabaseContext _encourageDbcontext;
        private readonly ICommonDbService _commonDbService;
        private readonly INominationService _nominationService;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _CommonDbContext;


        public AwardService(IDataContextFactory contextFactory, ICommonDbService commonDbService,INominationService nominationService)
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

            var currentUser = _CommonDbContext.Query<User>().Where(user => user.ID == userIdToExcept);
            resourcesUnderDept = resourcesUnderDept.Except(currentUser);
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
                resourcesUnderDept.ToList().RemoveAll(user => user.ID == winnerNomination.UserId);
            }

            return resourcesUnderDept.ToList();
        }

        public List<Criteria> GetCriteriasForAward(int awardId)
        {
            var criteriaList = _encourageDbcontext.Query<Criteria>().Where(criteria => criteria.AwardId == awardId).ToList();
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
            var currentUserId = currentUser.FirstOrDefault().ID;
            var recourcesInEnggementUnderCurrentManger = _encourageDbcontext.Query<Nomination>().Where(n => n.ProjectID == engagementId && n.ManagerId == currentUserId );
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
            //End
            

            return userList;
        }


        public List<User> GetResourcesForEditInEngagement(int engagementId, int userIdToExcept)
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
