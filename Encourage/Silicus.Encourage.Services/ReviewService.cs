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
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly ILogger _logger;

        public ReviewService(IDataContextFactory dataContextFactory, ICommonDbService commonDbService, INominationService nominationService, ILogger logger)
        {
            _encourageDatabaseContext = dataContextFactory.CreateEncourageDbContext();
            _logger = logger;
        }

        public IEnumerable<Review> GetReviewsForNomination(int nominationID)
        {
            var reviews = _encourageDatabaseContext.Query<Review>("ReviewerComments").Where(review => review.NominationId == nominationID && review.IsSubmited == true).ToList();
            return reviews;
        }

        public void UpdateReview(Review model)
        {
            _encourageDatabaseContext.Update(model);
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
                _encourageDatabaseContext.Delete(previousComment);
            }
        }

        public List<Award> LockReview(List<int> awardIds)
        {
            _logger.Log("ReviewService-LockReview");
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var lockedAwards = new List<Award>();
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == awardId);
                    if (data != null)
                    {
                        data.value = true;
                        _encourageDatabaseContext.Update(data);
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

        public List<Award> UnLockReview(List<int> awardIds)
        {
            _logger.Log("reviewService-unLockReview");
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var unlockedAwards = new List<Award>();
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.value == true && x.AwardId == awardId);
                    if (data != null)
                    {
                        data.value = false;
                        _encourageDatabaseContext.Update(data);
                        unlockedAwards.Add(_encourageDatabaseContext.Query<Award>().FirstOrDefault(a => a.Id == awardId));
                    }
                }
                return unlockedAwards;
            }
            else
            {
                return new List<Award>();
            }
        }

        public List<Award> GetReviewLockStatus()
        {
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var allAwards = _encourageDatabaseContext.Query<Award>().ToList();
            var lockedAwards = new List<Award>();
            foreach (var award in allAwards)
            {
                var result = _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == award.Id && x.value == true);
                if (result != null)
                {
                    lockedAwards.Add(award);
                }
            }

            return lockedAwards;
        }

        public bool GetAwardReviewLockStatus(int awardId)
        {
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var lockStatus = _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == awardId).value.Value;
            return lockStatus;
        }

        public List<Configuration> GetProcessesToLock(int awardId)
        {
            return _encourageDatabaseContext.Query<Configuration>().Where(x => x.AwardId == awardId && x.value == false).ToList();
        }

        public List<Configuration> GetProcessesToUnlock(int awardId)
        {
            return _encourageDatabaseContext.Query<Configuration>().Where(x => x.AwardId == awardId && x.value == true).ToList();
        }

        public Configuration GetConfigurationById(int id)
        {
            return _encourageDatabaseContext.Query<Configuration>().FirstOrDefault(x => x.Id == id);
        }
    }
}