﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Silicus.Reusable.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
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
    }
}