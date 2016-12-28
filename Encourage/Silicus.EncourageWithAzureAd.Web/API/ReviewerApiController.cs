using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Silicus.EncourageWithAzureAd.Web.API
{
    public class ReviewerApiController : ApiController
    {
        private readonly IReviewerService _reviewerService;
        private readonly ILogger _logger;
        public ReviewerApiController(ILogger logger, IReviewerService reviewerService)
        {
            _logger = logger;
            _reviewerService = reviewerService;
        }

        [HttpGet]
        public bool AddReviewer(int userId)
        {
            return _reviewerService.addReviewer(userId);
        }

    }
}