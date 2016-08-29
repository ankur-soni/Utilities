using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        private INominationService _nominationService;
        //private IReviewService _reviewService;
        public NominationApiController(INominationService nominationService )
        {
            _nominationService = nominationService;
            //_reviewService = reviewService;
        }

        [HttpPost, AttributeRouting.Web.Mvc.Route("lock")]
        public bool LockNominations()
        {
            return _nominationService.LockNominations();
        }

        //[HttpPost, AttributeRouting.Web.Mvc.Route("unlock")]
        //public bool UnLockReview()
        //{
        //    return _nominationService.UnLockNominations();
        //}
     }
}
