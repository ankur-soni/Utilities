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
        private readonly ILogger _logger;
        public ReviewNominationApiController(IReviewService reviewService,ILogger logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }
        //[HttpGet]
        //public bool lockreview()
        //{
        //    _logger.Log("ReviewnominationApi-lockreview");
        //    return _reviewService.LockReview();
        //}

        //[HttpGet]
        //public bool UnLockReview()
        //{
        //    _logger.Log("ReviewNominatioApi-UnLockReview");
        //    return _reviewService.UnLockReview();
        //}
    }
}
