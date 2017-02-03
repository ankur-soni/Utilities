using Silicus.Encourage.DAL.Interfaces;
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
using Configuration = Silicus.Encourage.Models.Configuration;

namespace Silicus.Encourage.Services
{
    public class NominationService : INominationService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ILogger _logger;
        private readonly ICustomDateService _customDateService;

        public NominationService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, ILogger logger, ICustomDateService customDateService)
        {
            _commonDataBaseContext = commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _logger = logger;
            _customDateService = customDateService;
        }

        public List<Nomination> GetAllNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").ToList();
        }

        public List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUser(int managerID)
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(model => model.ManagerId == managerID).ToList();
        }

        public List<Nomination> GetAllSubmitedNonreviewedNominations(int reviewerId)
        {
            var allNominations = new List<Nomination>();

            var awardsList = _encourageDatabaseContext.Query<Award>().ToList();

            var alreadyReviewedNominationIds = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == reviewerId && r.IsSubmited == true).Select(r => r.NominationId);

            foreach (var award in awardsList)
            {
                var customDate = _customDateService.GetCustomDate(award.Id);
                switch (award.Code)
                {
                    default:
                    case "SOM":
                        var prevMonth = customDate;
                        var somNominations = _encourageDatabaseContext.Query<Nomination>().Where(N =>
                            N.AwardId == award.Id &&
                            N.IsSubmitted == true &&
                            (!alreadyReviewedNominationIds.Contains(N.Id)) &&
                            N.NominationDate.Value.Month == prevMonth.Month &&
                            N.NominationDate.Value.Year == prevMonth.Year).ToList();
                        allNominations.AddRange(somNominations);
                        break;

                    case "PINNACLE":
                        var pinnacleNominations = _encourageDatabaseContext.Query<Nomination>().Where(N =>
                            N.AwardId == award.Id &&
                            N.IsSubmitted == true &&
                            (!alreadyReviewedNominationIds.Contains(N.Id)) &&
                            N.NominationDate.Value.Year == (customDate.Year)).ToList();
                        allNominations.AddRange(pinnacleNominations);
                        break;
                }
            }
            return allNominations;
        }

        public Nomination GetReviewNomination(int nominationId)
        {
            var nomination = _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(n => n.Id == nominationId) == null ? new Nomination() :
                _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(n => n.Id == nominationId);
            return nomination;
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

        public List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId, bool forCurrentMonth, int awardId)
        {
            var toBeComparedDate = _customDateService.GetCustomDate(awardId);
            var award = _encourageDatabaseContext.Query<Award>().Where(x => x.Id == awardId).FirstOrDefault();

            List<Review> alreadyReviewedRecords;
            var finalNominations = new List<Nomination>();
            if (award != null)
            {
                switch (award.Code)
                {
                    default:
                    case "SOM":
                        alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>()
                                .Where(r =>
                                r.Nomination.AwardId == awardId &&
                                r.ReviewerId == reviewerId &&
                                r.IsSubmited == true &&
                                (forCurrentMonth ? (r.Nomination.NominationDate.Value.Month == toBeComparedDate.Month && r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate < toBeComparedDate))).ToList();
                        break;

                    case "PINNACLE":
                        alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>()
                                .Where(r =>
                                r.Nomination.AwardId == awardId &&
                                r.ReviewerId == reviewerId &&
                                r.IsSubmited == true &&
                                (forCurrentMonth ? (r.Nomination.NominationDate.Value.Year == toBeComparedDate.Year) : (r.Nomination.NominationDate.Value.Year < toBeComparedDate.Year))).ToList();
                        break;
                }

                foreach (var item in alreadyReviewedRecords)
                {
                    var nomination = _encourageDatabaseContext.Query<Nomination>().FirstOrDefault(N => N.IsSubmitted == true && N.Id == item.NominationId);
                    if (nomination != null)
                    {
                        nomination.IsSubmitted = item.IsSubmited;
                        finalNominations.Add(nomination);
                    }
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

        public bool CheckReviewIsDrafted(int nominationId)
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
                        var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId);
                        if (award != null)
                        {
                            lockedAwards.Add(award);
                        }
                        
                    }
                }
                return lockedAwards;
            }
            else
            {
                return new List<Award>();
            }
        }

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
                        var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId);
                        if (award != null)
                        {
                            unLockedAwards.Add(award);
                        }
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
            List<Configuration> data;
            if (status == ConfigurationManager.AppSettings["Lock"])
            {
                data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.value == false).ToList();
            }
            else
            {
                data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.value == true).ToList();
            }
            var awardsToUnlock = new List<Award>();
            foreach (var awardconfiguration in data)
            {
                var award = _encourageDatabaseContext.Query<Award>().FirstOrDefault(x => x.Id == awardconfiguration.AwardId);
                if (award != null)
                {
                    award.Configurations.Add(awardconfiguration);
                    awardsToUnlock.Add(award);
                }
                
            }
            if (awardsToUnlock.Any())
            {
                return awardsToUnlock.GroupBy(x => x.Name).Select(group => group.FirstOrDefault()).ToList();
            }
            return new List<Award>();
        }

        public int GetNominationCountByManagerIdForSom(int managerId, DateTime startDate, DateTime endDate, int awardId)
        {
            return _encourageDatabaseContext.Query<Nomination>().Count(x => x.ManagerId == managerId && (x.NominationDate >= startDate && x.NominationDate <= endDate && x.AwardId == awardId));
        }

        public int GetNominationCountByManagerIdForPinnacle(int managerId, DateTime startDate, int awardId)
        {
            return _encourageDatabaseContext.Query<Nomination>().Count(x => x.ManagerId == managerId && (x.NominationDate.Value.Year == startDate.Year && x.AwardId == awardId));
        }

        public List<Award> GetNominationLockStatus()
        {
            var lockKey = WebConfigurationManager.AppSettings["NominationLockKey"];
            var allAwards = _encourageDatabaseContext.Query<Award>().ToList();
            var lockedAwards = new List<Award>();
            foreach (var award in allAwards)
            {
                var result = _encourageDatabaseContext.Query<Encourage.Models.Configuration>().Where(x => x.configurationKey == lockKey && x.AwardId == award.Id && x.value == true).FirstOrDefault();
                if (result != null)
                {
                    lockedAwards.Add(award);
                }
            }

            return lockedAwards;
        }

        public bool GetAwardNominationLockStatus(int awardId)
        {
            var lockKey = WebConfigurationManager.AppSettings["NominationLockKey"];
            var lockData = _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == awardId);
            var lockStatus = false;
            if (lockData != null)
            {
                lockStatus = lockData.value.Value;
            }
            return lockStatus;
        }

        public List<FrequencyMaster> GetAllAwardFrequencies()
        {
            return _encourageDatabaseContext.Query<FrequencyMaster>().ToList();
        }

        public List<User> GetAllResources()
        {
            return _commonDataBaseContext.Query<User>().ToList();
        }

        public List<User> GetAllResourcesForOtherReason(int awardType, int managerId)
        {
            var customDate = _customDateService.GetCustomDate(awardType);
            var toBeExcludedUserIds = new List<int>();

            var prevWinnersUserIds = _encourageDatabaseContext.Query<Shortlist>()
                                        .Where(s =>
                                            s.IsWinner == true &&
                                            //s.WinningDate.Value.Year == DateTime.Now.Year &&
                                            s.WinningDate.Value.Year == customDate.Year &&
                                            s.Nomination.AwardId == awardType &&
                                            s.Nomination.ManagerId == managerId)
                                        .Select(n => n.Nomination.UserId).ToList();
            toBeExcludedUserIds.AddRange(prevWinnersUserIds);

            toBeExcludedUserIds.Add(managerId);

            return _commonDataBaseContext.Query<User>().Where(u => !toBeExcludedUserIds.Contains(u.ID)).ToList();
        }

        #region Get Saved Nominations Details

        public string GetAwardNameByAwardId(int awardId)
        {
            var award =  _encourageDatabaseContext.Query<Award>().Where(a => a.Id == awardId).FirstOrDefault();
            if (award != null)
            {
                return award.Code;
            }
            return string.Empty;
        }

        public User GetNomineeDetails(int userId)
        {
            var user = _commonDataBaseContext.Query<User>().Where(u => u.ID == userId).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return new User();
        }

        public List<Nomination> GetAllSubmittedAndSavedNominationsByCurrentUserAndMonth(int managerID, bool forCurrentMonth, int awardId)
        {
            DateTime toBeComparedDate = _customDateService.GetCustomDate(awardId);
            var award = _encourageDatabaseContext.Query<Award>().Where(x => x.Id == awardId).FirstOrDefault();

            var allNominations = new List<Nomination>();
            if (award != null)
            {
                switch (award.Code)
                {
                    default:
                    case "SOM":
                        //SOM
                        allNominations = _encourageDatabaseContext.Query<Nomination>().Where(N =>
                                N.ManagerId == managerID &&
                                N.AwardId == awardId &&
                                (forCurrentMonth ? (N.NominationDate.Value.Month == toBeComparedDate.Month && N.NominationDate.Value.Year <= toBeComparedDate.Year) : (N.NominationDate < toBeComparedDate))).ToList();
                        break;

                    case "PINNACLE":
                        //Pinnacle
                        allNominations = _encourageDatabaseContext.Query<Nomination>().Where(N =>
                                N.ManagerId == managerID &&
                                N.AwardId == awardId &&
                                (forCurrentMonth ? (N.NominationDate.Value.Year == toBeComparedDate.Year) : (N.NominationDate.Value.Year < toBeComparedDate.Year))).ToList();
                        break;
                }
            }
            return allNominations;
        }

        public void UpdateFinalScore(int nominationId)
        {
            var managerComments = _encourageDatabaseContext.Query<ManagerComment>().Where(m => m.NominationId == nominationId).ToList();

            foreach (var managerComment in managerComments)
            {
                var reviewersComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(r => r.CriteriaId == managerComment.CriteriaId && r.NominationId == managerComment.NominationId).ToList();
                if (reviewersComments.Any())
                {
                    managerComment.FinalScore = (Convert.ToDecimal(reviewersComments.Average(r => r.Credit)) * managerComment.Weightage) / 100m;
                }

                _encourageDatabaseContext.Update(managerComment);
            }
        }

        public FrequencyMaster GetAwardFrequencyByFrequencyCode(string frequencyCode)
        {
            var frequency = _encourageDatabaseContext.Query<FrequencyMaster>().Where(x => x.Code == frequencyCode).FirstOrDefault();
            if (frequency != null)
            {
                return frequency;
            }
            return new FrequencyMaster();
        }

        public FrequencyMaster GetAwardFrequencyById(int id)
        {
            var frequency = _encourageDatabaseContext.Query<FrequencyMaster>().Where(x => x.Id == id).FirstOrDefault();
            if (frequency != null)
            {
                return frequency;
            }
            return new FrequencyMaster();
        }

        #endregion Get Saved Nominations Details
    }
}