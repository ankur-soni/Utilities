using Silicus.Encourage.Models;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Silicus.EncourageWithAzureAd.Web.API
{
    public class ReviewNominationApiController : ApiController
    {
        private IReviewService _reviewService;
        private INominationService _nominationService;
        private readonly ILogger _logger;
        private readonly IAwardService _awardService;
        public ReviewNominationApiController(IReviewService reviewService,ILogger logger,INominationService nominationService,IAwardService awardService)
        {
            _reviewService = reviewService;
            _logger = logger;
            _nominationService = nominationService;
            _awardService = awardService;
        }
        [HttpGet]
        public bool lockreview(string frequencyCode)
        {
            var awards = _awardService.GetAllAwards().ToList();
            _logger.Log("ReviewnominationApi-lockreview");
            return _reviewService.LockReview(GetAwardsToLock(frequencyCode,awards));
        }

        private List<int> GetAwardsToLock(string frequencyCode,List<Award> awards)
        {
            var awardFrequency = _nominationService.GetAwardFrequencyByFrequencyCode(frequencyCode);
            var awardIds = new List<int>();
            foreach (var award in awards)
            {
                if (award.FrequencyId == awardFrequency.Id)
                {
                    awardIds.Add(award.Id);

                }
            }
            return awardIds;
        }

        [HttpGet]
        public bool UnLockReview(string frequencyCode)
        {
            var awards = _awardService.GetAllAwards().ToList();
            _logger.Log("ReviewNominatioApi-UnLockReview");
            return _reviewService.UnLockReview(GetAwardsToLock(frequencyCode,awards));
        }
    }
}
