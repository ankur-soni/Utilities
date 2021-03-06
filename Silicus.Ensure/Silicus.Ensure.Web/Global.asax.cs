﻿using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Silicus.FrameWorx.Logger;
using Silicus.Ensure.Web.Controllers;
using Silicus.Ensure.Web.Mappings;
using System.Collections.Generic;
using System.Web.Configuration;
using Silicus.UtilityContainer.Security;
using System.Web.Helpers;
using System.IdentityModel.Claims;
using System.Text.RegularExpressions;
using RazorEngine;
using System.IO;
using System.Linq;

namespace Silicus.Ensure.Web
{
    //using Eda.RDBI.Entities.Initializer;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    [ExcludeFromCodeCoverage]
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Session_Start()
        {
        }
        protected void Application_Start()
        {
            var logger = new DatabaseLogger(
               ConfigurationManager.ConnectionStrings["SilicusLoggerDataContext"].ToString(),
               this.GetType());
            logger.Log("Application_Start");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            // For email template caching
            foreach (string templateName in Directory.EnumerateFiles(Path.Combine(HttpRuntime.AppDomainAppPath, "EmailTemplates"), "*.cshtml"))
            {
                var template = System.IO.File.ReadAllText(templateName);
                RazorEngine.Razor.Compile(template, Path.GetFileNameWithoutExtension(templateName));
            }
           
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Enter - Application_Error");

            var logger = new DatabaseLogger(
                ConfigurationManager.ConnectionStrings["SilicusLoggerDataContext"].ToString(),
                this.GetType());

            var httpContext = ((MvcApplication)sender).Context;

            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var currentController = " ";
            var currentAction = " ";

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();

            if (ex != null)
            {
                logger.Log(string.Format("Server_Exception - {0}", ex.Message), LogCategory.Error);
                logger.Log(string.Format("Server_Exception - Stack Trace - {0}", ex.StackTrace), LogCategory.Error);
                System.Diagnostics.Trace.WriteLine(ex);

                if (ex.InnerException != null)
                {
                    logger.Log(string.Format("Server_InnerException - {0}", ex.InnerException), LogCategory.Error);
                    logger.Log(string.Format("Server_InnerException - Stack Trace - {0}", ex.StackTrace), LogCategory.Error);
                    System.Diagnostics.Trace.WriteLine(ex.InnerException);
                    System.Diagnostics.Trace.WriteLine(ex.InnerException.Message);
                }
            }

            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "CustomError";
            var statusCode = 500;

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;
                statusCode = httpEx.GetHttpCode();

                switch (httpEx.GetHttpCode())
                {
                    case 400:
                        action = "BadRequest";
                        break;

                    case 401:
                        action = "Unauthorized";
                        break;

                    case 403:
                        action = "Forbidden";
                        break;

                    case 404:
                        action = "PageNotFound";
                        break;

                    case 500:
                        action = "CustomError";
                        break;

                    default:
                        action = "CustomError";
                        break;
                }
            }
            else if (ex is AuthenticationException)
            {
                action = "Forbidden";
                statusCode = 403;
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.TrySkipIisCustomErrors = true;
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;

            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }

        public string getCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        public static List<string> getCurrentUserRoles()
        {
            var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));
            string utility = WebConfigurationManager.AppSettings["ProductName"];
            var commonRoles = authorizationService.GetRoleForUtility(new MvcApplication().getCurrentUserName(), utility);
            return commonRoles;
        }

        //public static List<string> getUtilityRoles()
        //{
        //    var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));
        //    string utility = WebConfigurationManager.AppSettings["ProductName"];
        //    var commonRoles = authorizationService.GetRoleForUtility(new MvcApplication().getCurrentUserName(), utility);
        //    return commonRoles;
        //}

        public static List<string> getDevelopersName()
        {
            var authorizationService = new Authorization(new Silicus.UtilityContainer.Entities.CommonDataBaseContext("DefaultConnection"));
            var productId = ConfigurationManager.AppSettings["ProductId"];
            var utilityId = 0;
            if (!string.IsNullOrEmpty(productId))
            {
                utilityId = Convert.ToInt32(productId);
            }
            var developers = authorizationService.GetNameOfContributors(utilityId);
            return developers;
        }


        public static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}