﻿using Silicus.Encourage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.Services.Interface
{
    public interface IReviewService
    {
        IEnumerable<Review> GetReviewsForNomination(int nominationID);
        void UpdateReview(Review model);
        List<Review> GetAllReview();
        List<Award> LockReview(List<int> awardIds);
        List<Award> UnLockReview(List<int> awardIds);
        void DeletePrevoiusReviewerComments(int reviewerId, int nominationID);
        List<Award> GetReviewLockStatus();
        bool GetAwardReviewLockStatus(int awardId);
        List<Configuration> GetProcessesToLock(int awardId);
        List<Configuration> GetProcessesToUnlock(int awardId);
        Configuration GetConfigurationById(int id);
        Review GetSubmitedReviewByDate(int nominationId, int month, int year);
    }
}
