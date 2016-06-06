using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace Eda.RDBI.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
		
		[ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult BadRequest()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return View();
        }

        [ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			return View();
        }
		
		[ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult Forbidden()
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;

		    var model = (ViewData.Model as HandleErrorInfo);
		    string returnUrl = string.Empty;

		    if (model != null)
		    {
		        returnUrl = Url.Action(model.ActionName, model.ControllerName);
		    }

		    return RedirectToAction("Login", "Account", new { returnUrl = returnUrl, name = "" });
        }
		
		[ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
			return View();
        }
		
		[ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult ServerError()
        {
			Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return View("CustomError");
        }
		
		[ExcludeFromCodeCoverage]
		[AllowAnonymous]
        public ActionResult CustomError()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			return View();
        }

        [ExcludeFromCodeCoverage]
        [AllowAnonymous]
        public ActionResult MobileUnauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return View();
        }
    }
}
