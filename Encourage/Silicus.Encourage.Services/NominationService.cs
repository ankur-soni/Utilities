﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Silicus.Encourage.Services
{
    public class NominationService : INominationService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICommonDbService _commonDbService;
        private readonly ILogger _logger;

        public NominationService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, ILogger logger)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _logger = logger;
        }

        public List<Nomination> GetAllNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").ToList();
        }

        private List<Nomination> GetCurrentNominations()
        {
            _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)
                                                                                      && (x.NominationDate.Value.Year.Equals(DateTime.Now.Month > 1 ? DateTime.Now.Year : DateTime.Now.Year - 1))).ToList();
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)
            && (x.NominationDate.Value.Year.Equals(DateTime.Now.Month > 1 ? DateTime.Now.Year : DateTime.Now.Year - 1))).ToList();
        }

        private List<Nomination> GetCurrentLockNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(x => x.NominationDate.Value.Month.Equals(DateTime.Now.Month - 1)
            && (x.NominationDate.Value.Year.Equals(DateTime.Now.Month > 1 ? DateTime.Now.Year : DateTime.Now.Year - 1)) && x.IsLocked == true).ToList();
        }

        public List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUser(int managerID)
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(model => model.ManagerId == managerID).ToList();
        }

        public List<Nomination> GetAllSubmitedNonreviewedNominations(int reviewerId)
        {
            var alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == reviewerId && r.IsSubmited == true).ToList();
            var finalNomination = new List<Nomination>();
            var allNominations = _encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true && N.NominationDate.Value.Month == (DateTime.Now.Month - 1) && N.NominationDate.Value.Year == DateTime.Now.Year).ToList();

            foreach (var item in alreadyReviewedRecords)
            {
                finalNomination.Add(_encourageDatabaseContext.Query<Nomination>().FirstOrDefault(N => N.IsSubmitted == true && N.Id == item.NominationId));
            }
            foreach (var item in finalNomination)
            {
                allNominations.RemoveAll(r => r.Id == item.Id);
            }

            return allNominations;
        }

        public Nomination GetReviewNomination(int nominationId)
        {
            return _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(n => n.Id == nominationId);
        }

        public List<ManagerComment> GetManagerCommentsForNomination(int nominationId)
        {
            return _encourageDatabaseContext.Query<ManagerComment>().Where(c => c.NominationId == nominationId).ToList();
        }

        public void AddReviewForCurrentNomination(Review review)
        {
            _encourageDatabaseContext.Add(review);
            _encourageDatabaseContext.SaveChanges();
        }

        public void AddReviewerCommentsForCurrentNomination(ReviewerComment revrComment)
        {
            _encourageDatabaseContext.Add<ReviewerComment>(revrComment);
            _encourageDatabaseContext.SaveChanges();
        }

        public List<Review> GetAllSubmitedReviewsForCurrentNomination(int nominationId)
        {
            var data = _encourageDatabaseContext.Query<Review>().Where(r => r.NominationId == nominationId && r.IsSubmited == true).ToList();

            return data;
        }

        public List<Criteria> GetCriteriaForNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            return _encourageDatabaseContext.Query<Criteria>().Where(c => c.AwardId == nomination.AwardId).ToList();
        }

        public string GetNomineeNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            var user = _commonDataBaseContext.Query<User>().FirstOrDefault(u => u.ID == nomination.UserId);
            if (user != null)
            {
                return user.DisplayName;
            }

            return string.Empty;
        }

        public string GetAwardName(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);

            var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == nomination.AwardId);
            if (award != null)
            {
                return award.Code;
            }

            return string.Empty;
        }

        public string GetAwardMonthAndYear(int nominationId)
        {
            var nomination = _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(n => n.Id == nominationId);
            if (nomination != null)
            {
                var nominationTime = nomination.NominationDate;

                return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
            }
            return string.Empty;
        }

        public string GetManagerNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            var user = _commonDataBaseContext.Query<User>().FirstOrDefault(u => u.ID == nomination.ManagerId);
            if (user != null)
            {
                return user.DisplayName;
            }

            return string.Empty;
        }

        public string GetProjectNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            string projectName = string.Empty;
            string departmentName = string.Empty;

            var closedProject = ConfigurationManager.AppSettings["ClosedEngagementStage"];
            if (nomination.ProjectID != null)
            {
                var engagement = _commonDataBaseContext.Query<Engagement>().FirstOrDefault(e => e.PrimaryProjectManagerID == nomination.ManagerId && e.ID == nomination.ProjectID);
                if (engagement != null)
                {
                    var clientId = engagement.ClientID;
                    var client = _commonDataBaseContext.Query<Client>().FirstOrDefault(c => c.ID == clientId);
                    if (client != null)
                    {
                        projectName = client.Code;
                    }
                }
            }

            if (nomination.DepartmentId != null)
            {
                var department = _commonDataBaseContext.Query<Department>().FirstOrDefault(e => e.ID == nomination.DepartmentId);
                if (department != null)
                {
                    departmentName = department.Name;
                }
                return departmentName;
            }

            return projectName;
        }

        public string GetDeptNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            string deptName = string.Empty;
            if (nomination.DepartmentId != null)
            {
                var department = _commonDataBaseContext.Query<Department>().FirstOrDefault(e => e.ID == nomination.DepartmentId);
                if (department != null)
                {
                    deptName = department.Name;
                }
            }

            return deptName;
        }

        public int GetReviewerIdOfCurrentNomination(string email)
        {
            var reviewerId = 0;

            var user = _commonDataBaseContext.Query<User>().FirstOrDefault(u => u.EmailAddress == email);
            if (user != null)
            {
                var reviewersUserId = user.ID;
                var reviewer = _encourageDatabaseContext.Query<Reviewer>().FirstOrDefault(r => r.UserId == reviewersUserId);
                if (reviewer != null)
                {
                    reviewerId = reviewer.Id;
                    return reviewerId;
                }
            }

            return reviewerId;
        }

        public List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId, bool forCurrentMonth)
        {
            var today = DateTime.Today;
            var prevMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);

            var alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == reviewerId && r.IsSubmited == true && (forCurrentMonth ? (r.Nomination.NominationDate >= prevMonth) : (r.Nomination.NominationDate < prevMonth))).ToList();
            var finalNominations = new List<Nomination>();

            foreach (var item in alreadyReviewedRecords)
            {
                var nomination = _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(N => N.IsSubmitted == true && N.Id == item.NominationId);
                if (nomination != null)
                {
                    nomination.IsSubmitted = item.IsSubmited;
                    finalNominations.Add(nomination);
                }
            }

            return finalNominations;
        }

        public List<Nomination> GetAllSavedNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(nomination => nomination.IsSubmitted == false).ToList();
        }

        public Nomination GetNomination(int nominationId)
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").SingleOrDefault(nomination => nomination.Id == nominationId);
        }

        public void UpdateNomination(Nomination model)
        {
            _encourageDatabaseContext.Update<Nomination>(model);
        }

        public void DeletePrevoiusManagerComments(int nominationId)
        {
            var managerCommentsToDelete = _encourageDatabaseContext.Query<ManagerComment>().Where(comment => comment.NominationId == nominationId).ToList();

            foreach (var managerComments in managerCommentsToDelete)
                _encourageDatabaseContext.Delete<ManagerComment>(managerComments);
        }

        public void DiscardNomination(int nominationId)
        {
            DeletePrevoiusManagerComments(nominationId);
            var nominationToDelete = _encourageDatabaseContext.Query<Nomination>().SingleOrDefault(nomination => nomination.Id == nominationId);
            _encourageDatabaseContext.Delete<Nomination>(nominationToDelete);
        }

        public bool checkReviewIsDrafted(int nominationId)
        {
            var reviewerId = GetReviewerIdOfCurrentNomination(HttpContext.Current.User.Identity.Name);
            var data = _encourageDatabaseContext.Query<Review>().Where(review => review.NominationId == nominationId && review.ReviewerId == reviewerId).ToList();

            return data.Count == 1;
        }

        public List<Award> LockNominations(List<int> awardIds)
        {
            _logger.Log("NominationService-LockNominations");
            var lockedAwards = new List<Award>();
            var lockKey = WebConfigurationManager.AppSettings["NominationLockKey"];
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Models.Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == awardId);
                    if (data != null)
                    {
                        data.value = true;
                        _encourageDatabaseContext.Update<Models.Configuration>(data);
                        lockedAwards.Add(_encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId));
                    }
                }
                return lockedAwards;
            }
            else
            {
                return new List<Award>();
            }

        }

        public bool IsNominationLocked()
        {
            _logger.Log("NominationService-IsNominationLocked");
            return GetNominationLockStatus();
        }

        //public bool UnLockNominations()
        //{
        //    _logger.Log("NominationService-UnLockNominations");
        //    var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == "NominationLock").SingleOrDefault();
        //    data.value = false;
        //    _encourageDatabaseContext.Update<Models.Configuration>(data);
        //    return true;
        //}

        public List<Award> UnLockNominations(List<int> awardIds)
        {
            _logger.Log("NominationService-UnLockNominations");

            var unLockedAwards = new List<Award>();
            var lockKey = WebConfigurationManager.AppSettings["NominationLockKey"];
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Models.Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == awardId && x.value == true);
                    if (data != null)
                    {
                        data.value = false;
                        _encourageDatabaseContext.Update<Models.Configuration>(data);
                        unLockedAwards.Add(_encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId));
                    }
                }
                return unLockedAwards;
            }
            else
            {
                return new List<Award>();
            }
        }

        public List<Award> GetAwardstoUnLockOrUnlock(string status)
        {
            _logger.Log("NominationService-GetAwardstoUnLock");
            var lockKey = WebConfigurationManager.AppSettings["NominationLockKey"];
            var data = new List<Models.Configuration>();
            if (status == ConfigurationManager.AppSettings["Lock"])
            {
                 data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == lockKey && x.value == false ).ToList();
            }
            else
            {
                data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == lockKey && x.value == true ).ToList();

            }
            var awardsToUnlock = new List<Award>();
            foreach (var awardconfiguration in data)
            {
                awardsToUnlock.Add(_encourageDatabaseContext.Query<Award>().FirstOrDefault(x => x.Id == awardconfiguration.AwardId));
            }
            if (awardsToUnlock.Any())
            {
                return awardsToUnlock;
            }
            return new List<Award>();
        }

       
        public int GetNominationCountByManagerId(int managerId, DateTime startDate, DateTime endDate)
        {
            return _encourageDatabaseContext.Query<Nomination>().Count(x => x.ManagerId == managerId && (x.NominationDate >= startDate && x.NominationDate <= endDate));
        }

        public bool GetNominationLockStatus()
        {
            var config = _encourageDatabaseContext.Query<Encourage.Models.Configuration>().FirstOrDefault(x => x.configurationKey == "NominationLock");
            if (config != null)
            {
                var data = config.value;
                return data == true;
            }
            return false;
        }

        #region Get Saved Nominations Details

        public string GetAwardNameByAwardId(int awardId)
        {
            return _encourageDatabaseContext.Query<Award>().Where(a => a.Id == awardId).FirstOrDefault().Code;
        }

        public User GetNomineeDetails(int userId)
        {
            return _commonDataBaseContext.Query<User>().Where(u => u.ID == userId).FirstOrDefault();
        }

        public List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(int managerID, bool forCurrentMonth)
        {
            var today = DateTime.Today;
            var prevMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);

            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(model => model.ManagerId == managerID && (forCurrentMonth ? (model.NominationDate >= prevMonth) : (model.NominationDate < prevMonth))).ToList();
        }

        public FrequencyMaster GetAwardFrequencyByFrequencyCode(string frequencyCode)
        {
            return _encourageDatabaseContext.Query<FrequencyMaster>().Where(x => x.Code == frequencyCode).FirstOrDefault();
        }

        #endregion Get Saved Nominations Details
    }
}