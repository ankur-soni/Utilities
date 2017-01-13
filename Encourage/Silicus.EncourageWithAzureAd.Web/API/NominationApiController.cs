using System.Web.Http;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;

namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        private readonly INominationService _nominationService;
        private readonly ILogger _logger;
        private readonly IAwardService _awardService;
        public NominationApiController(INominationService nominationService, ILogger logger, IAwardService awardService)
        {
            _nominationService = nominationService;
            _logger = logger;
            _awardService = awardService;
        }

        [HttpGet]
        public bool LockNominations(string awardName)
        {
            _logger.Log("NominationApi-LockNominations");
            var awardToLock = _awardService.GetAwardByCode(awardName);
            if (awardToLock != null)
            {
                _nominationService.LockNominations(new List<int> { awardToLock.Id });

            }

            return true;
        }

        [HttpGet]
        public bool UnLockNominations(string awardName)
        {
            _logger.Log("NominationApi-UnLockNominations");
            var awardToLock = _awardService.GetAwardByCode(awardName);
            if (awardToLock != null)
            {
                _nominationService.UnLockNominations(new List<int> { awardToLock.Id });
            }
            return true;
        }
    }
}
