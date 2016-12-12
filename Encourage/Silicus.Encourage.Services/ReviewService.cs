using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace Silicus.Encourage.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly Silicus.UtilityContainer.Entities.ICommonDataBaseContext _commonDataBaseContext;
        private readonly ICommonDbService _commonDbService;
        private readonly INominationService _nominationService;
        private readonly ILogger _logger;

        public ReviewService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, INominationService nominationService, ILogger logger)
        {
            _dataContextFactory = dataContextFactory;
            _commonDbService = commonDbService;
            _commonDataBaseContext = _commonDbService.GetCommonDataBaseContext();
            _encourageDatabaseContext = _dataContextFactory.CreateEncourageDbContext();
            _nominationService = nominationService;
            _logger = logger;
        }

        public IEnumerable<Review> GetReviewsForNomination(int nominationID)
        {
            var reviews = _encourageDatabaseContext.Query<Review>("ReviewerComments").Where(review => review.NominationId == nominationID && review.IsSubmited == true).ToList();
            return reviews;
        }

        public void UpdateReview(Review model)
        {
            _encourageDatabaseContext.Update<Review>(model);
        }

        public List<Review> GetAllReview()
        {
            return _encourageDatabaseContext.Query<Review>("ReviewerComments").ToList();
        }

        public void DeletePrevoiusReviewerComments(int reviewerId, int nominationID)
        {
            var previousComments = _encourageDatabaseContext.Query<ReviewerComment>().Where(r => r.ReviewerId == reviewerId && r.NominationId == nominationID).ToList();
            foreach (var previousComment in previousComments)
            {
                _encourageDatabaseContext.Delete<ReviewerComment>(previousComment);
            }
        }

        public bool LockReview(List<int> awardIds)
        {
            _logger.Log("ReviewService-LockReview");
            //var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == "ReviewLock").SingleOrDefault();
            //data.value = true;
            //_encourageDatabaseContext.Update<Models.Configuration>(data);

            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == lockKey && x.AwardId == awardId).FirstOrDefault();
                    if (data != null)
                    {
                        data.value = true;
                        _encourageDatabaseContext.Update<Models.Configuration>(data);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UnLockReview()
        {
            _logger.Log("reviewService-unLockReview");
            var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == "ReviewLock").SingleOrDefault();
            data.value = false;
            _encourageDatabaseContext.Update<Models.Configuration>(data);
            return true;
        }

        public bool GetReviewLockStatus()
        {
            var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == "ReviewLock").SingleOrDefault().value;
            return data == true ? true : false;
        }

        //public string GetHrAdminsCommentForEmployee(int loggedInAdminsId, int nominatedEmployeeId)
        //{
        //    var data = _encourageDatabaseContext.Query<Shortlist>().Where( s => s.)
        //}
    }
}