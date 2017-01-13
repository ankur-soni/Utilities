﻿using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System;
using System.Configuration;

namespace Silicus.Encourage.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IEncourageDatabaseContext _encourageDatabaseContext;
        private readonly ILogger _logger;

        public ReviewService(Silicus.Encourage.DAL.Interfaces.IDataContextFactory dataContextFactory, ICommonDbService commonDbService, INominationService nominationService, ILogger logger)
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

        public List<Award> LockReview(List<int> awardIds)
        {
            _logger.Log("ReviewService-LockReview");
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var lockedAwards = new List<Award>();
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == lockKey && x.AwardId == awardId).FirstOrDefault();
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

        public List<Award> UnLockReview(List<int> awardIds)
        {
            _logger.Log("reviewService-unLockReview");
            var lockKey = WebConfigurationManager.AppSettings["ReviewLockKey"];
            var unlockedAwards = new List<Award>();
            if (awardIds.Count > 0)
            {
                foreach (var awardId in awardIds)
                {
                    var data = _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.configurationKey == lockKey && x.value == true && x.AwardId == awardId).FirstOrDefault();
                    if (data != null)
                    {
                        data.value = false;
                        _encourageDatabaseContext.Update<Models.Configuration>(data);
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
                var result = _encourageDatabaseContext.Query<Models.Configuration>().FirstOrDefault(x => x.configurationKey == lockKey && x.AwardId == award.Id && x.value == true);
                if (result != null)
                {
                    lockedAwards.Add(award);
                }
            }

            return lockedAwards;

        }

        public List<Models.Configuration> GetProcessesToLock(int awardId)
        {

            return _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.AwardId == awardId && x.value == false).ToList();

        }

        public List<Models.Configuration> GetProcessesToUnlock(int awardId)
        {

            return _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.AwardId == awardId && x.value == true).ToList();

        }

        public Models.Configuration GetConfigurationById(int id)
        {
            return _encourageDatabaseContext.Query<Models.Configuration>().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}