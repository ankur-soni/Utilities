using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;

namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        private  INominationService _nominationService;
        public NominationApiController(INominationService nominationService)
        {
           this._nominationService = nominationService;
        }

        //[HttpGet, AttributeRouting.Web.Mvc.Route("lock")]
        //public bool LockNominations()
        //{            
        //    return _nominationService.LockNominations();
        //}

        [HttpPost, AttributeRouting.Web.Mvc.Route("lock")]
        public bool LockNominations()
        {
            return _nominationService.LockNominations();
        }
    }
}
