
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web.Mvc;
using Silicus.FrameWorx.Logger;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;
        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }
        public ActionResult BadRequest()
        {
            _logger.Log("Error - BadRequest");
            return View();
        }

        public ActionResult Unauthorized()
        {
            _logger.Log("Error - Unauthorized");
            return View();
        }

        public ActionResult Forbidden()
        {
            _logger.Log("Error - Forbidden");
            return View();
        }

        public ActionResult PageNotFound()
        {
            _logger.Log("Error - PageNotFound");
            return View();
        }

        public ActionResult ServerError()
        {
            _logger.Log("Error - ServerError");
            return View("CustomError");
        }

       
        public ActionResult CustomError()
        {
            _logger.Log("Error - CustomError");
            return View();
        }
    }
}
