using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Services.Models;
using Silicus.EncourageWithAzureAd.Web.Models;
using Silicus.FrameWorx.Logger;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    public class UserController : Controller
    {

        private readonly IWinnerUserService _winnerUserService;
        private readonly IAwardService _awardService;
        private readonly ILogger _logger;
        public UserController(IWinnerUserService winnerUserService,IAwardService awardService, ILogger logger)
        {
            _winnerUserService = winnerUserService;
            _awardService = awardService;
            _logger = logger;

        }
        // GET: User
        public ActionResult History(int awardId = 0)
        {
            _logger.Log("User-History");
            var userId = _awardService.GetUserIdFromEmail(User.Identity.Name);
             return View(_winnerUserService.GetMyWinningHistory(userId, awardId));
            
        }
    }
}