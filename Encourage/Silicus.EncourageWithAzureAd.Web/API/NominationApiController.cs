using System.Web.Http;
using AttributeRouting.Web.Mvc;
using Silicus.Encourage.Services;
using Silicus.Encourage.Services.Interface;
using Silicus.FrameWorx.Logger;
using System.Collections.Generic;

namespace Silicus.EncourageWithAzureAd.Web.API
{

    public class NominationApiController : ApiController
    {
        private INominationService _nominationService;
        private readonly ILogger _logger;
        private readonly IAwardService _awardService;
        public NominationApiController(INominationService nominationService, ILogger logger,IAwardService awardService)
        {
            _nominationService = nominationService;
            _logger = logger;
            _awardService = awardService;
        }
        [HttpGet]
        public bool LockNominations(string frequencyCode)
        {
            _logger.Log("NominationApi-LockNominations");
            var awards = _awardService.GetAllAwards();
            var awardIds = new List<int>();
            var awardFrequency = _nominationService.GetAwardFrequencyByFrequencyCode(frequencyCode);
            foreach (var award in awards)
            {
                if (award.FrequencyId == awardFrequency.Id)
                {
                    awardIds.Add(award.Id);
                }
            }
            if (awardIds.Count > 0)
            {
                _nominationService.LockNominations(awardIds);
            }
            return true;
        }
        [HttpGet]
        public bool UnLockNominations(string frequencyCode)
        {
            _logger.Log("NominationApi-UnLockNominations");
            var awards = _awardService.GetAllAwards();
            var awardIds = new List<int>();
            var awardFrequency = _nominationService.GetAwardFrequencyByFrequencyCode(frequencyCode);

            foreach (var award in awards)
            {
                if (award.FrequencyId == awardFrequency.Id)
                {
                    awardIds.Add(award.Id);
                }
            }
            if (awardIds.Count > 0)
            {
                _nominationService.UnLockNominations(awardIds);
            }
            return true;
        }
    }
}
