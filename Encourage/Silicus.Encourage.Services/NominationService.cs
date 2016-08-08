using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Comparer;
using Silicus.Encourage.Services.Interface;
using Silicus.UtilityContainer.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Silicus.Encourage.Services
{
    public class NominationService : INominationService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICommonDbService _commonDbService;

        public NominationService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();

        }

        public List<Nomination> GetAllNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").ToList();
        }

        private List<Nomination> GetCurrentNominations()
        {
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
                finalNomination.Add(_encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true && N.Id == item.NominationId).FirstOrDefault());

            }
            foreach (var item in finalNomination)
            {
                allNominations.RemoveAll(r => r.Id == item.Id);
            }


            return allNominations;
        }

        public Nomination GetReviewNomination(int nominationId)
        {

            return _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nominationId).FirstOrDefault();
        }

        public List<ManagerComment> GetManagerCommentsForNomination(int nominationId)
        {

            return _encourageDatabaseContext.Query<ManagerComment>().Where(c => c.NominationId == nominationId).ToList();
        }

        public void AddReviewForCurrentNomination(Review review)
        {
            _encourageDatabaseContext.Add<Review>(review);
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
            return _commonDataBaseContext.Query<User>().Where(u => u.ID == nomination.UserId).FirstOrDefault().DisplayName;
        }

        public string GetAwardName(int nominationId)
        {

            var nomination = GetReviewNomination(nominationId);

            return _encourageDatabaseContext.Query<Award>().Where(a => a.Id == nomination.AwardId).FirstOrDefault().Code;
        }

        public string GetAwardMonthAndYear(int nominationId)
        {
            var nominationTime = _encourageDatabaseContext.Query<Nomination>().Where(n => n.Id == nominationId).FirstOrDefault().NominationDate;

            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(nominationTime.Value.Month) + "-" + nominationTime.Value.Year.ToString();
        }

        public string GetManagerNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            return _commonDataBaseContext.Query<User>().Where(u => u.ID == nomination.ManagerId).FirstOrDefault().DisplayName;
        }

        public string GetProjectNameOfCurrentNomination(int nominationId)
        {
            var nomination = GetReviewNomination(nominationId);
            string projectName = string.Empty;
            string departmentName = string.Empty;
            if (nomination.ProjectID != null)
            {
                projectName = _commonDataBaseContext.Query<Engagement>().Where(e => e.ID == nomination.ProjectID).FirstOrDefault().Name;
            }

            if (nomination.DepartmentId != null)
            {
                departmentName = _commonDataBaseContext.Query<Department>().Where(e => e.ID == nomination.DepartmentId).FirstOrDefault().Name;
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
                deptName = _commonDataBaseContext.Query<Department>().Where(e => e.ID == nomination.DepartmentId).FirstOrDefault().Name;
            }

            return deptName;
        }



        public int GetReviewerIdOfCurrentNomination(string email)
        {

            var reviewersUserId = _commonDataBaseContext.Query<User>().Where(u => u.EmailAddress == email).FirstOrDefault().ID;
            var reviewer = _encourageDatabaseContext.Query<Reviewer>().Where(r => r.UserId == reviewersUserId).FirstOrDefault();
            var reviewerId = 0;
            if (reviewer != null)
            {
                reviewerId = reviewer.Id;
                return reviewerId;
            }

            return reviewerId;
        }

        public List<Nomination> GetAllSubmitedReviewedNominations(int reviewerId)
        {
            var alreadyReviewedRecords = _encourageDatabaseContext.Query<Review>().Where(r => r.ReviewerId == reviewerId && r.IsSubmited == true).ToList();
            var finalNominations = new List<Nomination>();


            foreach (var item in alreadyReviewedRecords)
            {
                var nomination = _encourageDatabaseContext.Query<Nomination>().Where(N => N.IsSubmitted == true && N.Id == item.NominationId).FirstOrDefault();
                nomination.IsSubmitted = item.IsSubmited;
                finalNominations.Add(nomination);

            }

            return finalNominations;
        }

        public List<Nomination> GetAllSavedNominations()
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(nomination => nomination.IsSubmitted == false).ToList();
        }

        public Nomination GetNomination(int nominationId)
        {
            return _encourageDatabaseContext.Query<Nomination>("ManagerComments").Where(nomination => nomination.Id == nominationId).SingleOrDefault();
        }

        public void UpdateNomination(Nomination model)
        {
            _encourageDatabaseContext.Update<Nomination>(model);
        }

        public void DeletePrevoiusManagerComments(int nominationID)
        {
            var managerCommentsToDelete = _encourageDatabaseContext.Query<ManagerComment>().Where(comment => comment.NominationId == nominationID).ToList();

            foreach (var managerComments in managerCommentsToDelete)
                _encourageDatabaseContext.Delete<ManagerComment>(managerComments);
        }

        public void DiscardNomination(int nominationId)
        {
            DeletePrevoiusManagerComments(nominationId);
            var nominationToDelete = _encourageDatabaseContext.Query<Nomination>().Where(nomination => nomination.Id == nominationId).SingleOrDefault();
            _encourageDatabaseContext.Delete<Nomination>(nominationToDelete);
        }

        public bool checkReviewIsDrafted(int nominationId)
        {
            var reviewerId = GetReviewerIdOfCurrentNomination(HttpContext.Current.User.Identity.Name);
            var data = _encourageDatabaseContext.Query<Review>().Where(review => review.NominationId == nominationId && review.ReviewerId == reviewerId).ToList();

            return data.Count == 1;
        }

        public bool LockNominations()
        {
            var currentNominations = GetCurrentNominations().Where(n => n.IsLocked == false);

            foreach (var nomination in currentNominations)
            {
                nomination.IsLocked = true;
                UpdateNomination(nomination);
            }
            return true;

        }

        public bool IsNominationLocked()
        {
            var currentNominations = GetCurrentNominations();
            return currentNominations.Count > 0 && currentNominations.TrueForAll(cn => Convert.ToBoolean(cn.IsLocked));
            //return true;

        }
            
        
        public bool UnLockNominations()
        {
            var currentNominations = GetCurrentLockNominations();

            foreach (var nomination in currentNominations)
            {
                nomination.IsLocked = false;
                UpdateNomination(nomination);
               // return true;

            }
            return false;

        }

        public int GetNominationCountByManagerId(int managerId, DateTime startDate, DateTime endDate)
        {
            return _encourageDatabaseContext.Query<Nomination>().Where(x => x.ManagerId == managerId && (x.NominationDate >= startDate && x.NominationDate <= endDate)).Count();
        }
    }
}
