using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Web.Http;

namespace Silicus.EncourageWithAzureAd.Web.API
{
    public class ReviewerApiController : ApiController
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerApiController(ILogger logger, IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [HttpGet]
        public bool AddReviewer(int userId)
        {
            return _reviewerService.AddReviewer(userId);
        }

    }
}