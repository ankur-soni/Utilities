
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web.Mvc;

namespace Silicus.EncourageWithAzureAd.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult BadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View();
        }

        public ActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return View();
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

        public ActionResult PageNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        [ExcludeFromCodeCoverage]
        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("CustomError");
        }

        [ExcludeFromCodeCoverage]
        public ActionResult CustomError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }
    }
}
