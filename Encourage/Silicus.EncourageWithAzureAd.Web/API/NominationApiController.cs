using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Silicus.Encourage.Services.Interface;

namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        INominationService _nominationService;
        public NominationApiController(INominationService nominationService)
        {
            _nominationService = nominationService;
        }

        [HttpPost, AttributeRouting.Web.Mvc.Route("lock")]
        public bool LockNominations()
        {
            return _nominationService.LockNominations();
        }
    }
}
