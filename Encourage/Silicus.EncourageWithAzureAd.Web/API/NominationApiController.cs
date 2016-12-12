using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;

namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        private INominationService _nominationService;
        //private IReviewService _reviewService;
        private readonly ILogger _logger;
        public NominationApiController(INominationService nominationService,ILogger logger)
        {
            _nominationService = nominationService;
            _logger = logger;
            //_reviewService = reviewService;
        }
       //[HttpGet]
       // public bool LockNominations()
       // {
       //     _logger.Log("NominationApi-LockNominations");
       //     return _nominationService.LockNominations();
        //}
        [HttpGet]
        public bool UnLockNominations()
        {
            _logger.Log("NominationApi-UnLockNominations");
            return _nominationService.UnLockNominations();
        }
    }
}
