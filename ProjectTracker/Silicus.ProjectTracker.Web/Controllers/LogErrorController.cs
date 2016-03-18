using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Silicus.ProjectTracker.Logger;

namespace Silicus.ProjectTracker.Web.Controllers
{
    [AllowAnonymous]
    public class LogErrorController : Controller
    {
        private readonly ILogger _logger;

	    public LogErrorController(ILogger logger)
	    {
		    _logger = logger;
	    }

	    [ExcludeFromCodeCoverage]
	    [AllowAnonymous]
	    public ActionResult LogData(string errorData)
	    {
		    try
		    {
			    var userName = string.Empty;

			    userName = (Request.IsAuthenticated)
				    ? HttpContext.User.Identity.Name
				    : Request.Params["userName"];

			    var sessionId = Request.Params["ASP.NET_SessionId"] + "-" + userName;

			    _logger.Log(string.Format("Ajax Error for : {0} and Error details are: {1}", userName, errorData),
				    LogCategory.Error,
				    sessionId);
		    }
		    catch
		    {
			    // Skip exception in Logger
		    }

		    return Json(true);
	    }
    }
}
