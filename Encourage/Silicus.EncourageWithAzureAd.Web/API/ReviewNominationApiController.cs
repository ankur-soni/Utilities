using Silicus.Encourage.Services.Interface;
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
        public ReviewNominationApiController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost, AttributeRouting.Web.Mvc.Route("reviewlock")]
        public bool lockreview()
        {
            return _reviewService.LockReview();
        }
    }
}
