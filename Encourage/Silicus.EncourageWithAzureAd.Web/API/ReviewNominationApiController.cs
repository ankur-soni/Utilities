using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;
using System.Web.Http;

namespace Silicus.EncourageWithAzureAd.Web.API
{
    public class ReviewNominationApiController : ApiController
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger _logger;
        private readonly IAwardService _awardService;
        public ReviewNominationApiController(IReviewService reviewService, ILogger logger, INominationService nominationService, IAwardService awardService)
        {
            _reviewService = reviewService;
            _logger = logger;
            _awardService = awardService;
        }
        [HttpGet]
        public bool Lockreview(string awardName)
        {
            _logger.Log("ReviewnominationApi-lockreview");
            var awarToLock = _awardService.GetAwardByCode(awardName);
            if (awarToLock != null)
            {
                _reviewService.LockReview(new List<int> { awarToLock.Id });
            }
            return true;
        }

        [HttpGet]
        public bool UnLockReview(string awardCode)
        {
            _logger.Log("ReviewNominatioApi-UnLockReview");
            var awarToLock = _awardService.GetAwardByCode(awardCode);
            if (awarToLock != null)
            {
                _reviewService.UnLockReview(new List<int> { awarToLock.Id });
            }
            return true;
        }
    }
}
