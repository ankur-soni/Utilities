
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using Silicus.ProjectTracker.Core;
using System.Web.Configuration;


namespace Silicus.ProjectTracker.Web.Filters
{   
    public class AuthorizeADAttribute : AuthorizeAttribute
    {
        public string Groups { get; set; }
        public string UserId { get; set; }

        private bool _isAuthorized = false;
        private bool _isInAuthorizedRule = false;
        private readonly ICookieHelper _cookieHelper;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            string username = string.Empty;
            string adEmail = WebConfigurationManager.AppSettings["ADDefaultUserEmail"];
            string adPassword = WebConfigurationManager.AppSettings["ADDefaultUserPassword"];

            /* Return true immediately if the authorization is not
             * locked down to any particular AD group */
            if (String.IsNullOrEmpty(Groups))
            {
                _isInAuthorizedRule = false;
                return _isAuthorized = false;
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies["userid"];

            if (String.IsNullOrEmpty(cookie.ToString()))
            {
                username = HttpContext.Current.User.Identity.Name;
            }
            else if (String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                username = cookie.Value;
            }
            else
            {
                username = string.Empty;
            }

            if (string.IsNullOrEmpty(username))
            {

                _isInAuthorizedRule = false;
                return _isAuthorized = false;
            }

            // Get the AD groups
            var groups = Groups.Split(',').ToList<string>();
                      

            // set up domain context

            PrincipalContext context = new PrincipalContext(ContextType.Domain, Silicus.ProjectTracker.Core.Constants.DomainName, null, ContextOptions.SimpleBind, adEmail, adPassword);

            // find a user
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, username);

            foreach (var group in groups)
            {
                if (userPrincipal.IsMemberOf(context, IdentityType.Name, group))
                {
                    _isInAuthorizedRule = true;
                    return _isAuthorized = true;
                }
                            
              
            }

            _isInAuthorizedRule = false;
            return _isAuthorized = false;
                    

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
                      
            if (!_isInAuthorizedRule)
            {
                var result = new ViewResult();
                result.ViewName = "~/Views/Error/Unauthorized.cshtml";     //this can be a property you don't have to hard code it
                result.MasterName = "~/Views/Shared/_Layout.cshtml";    //this can also be a property
                result.ViewBag.Message = "You do not have permissions to view this page.";
                filterContext.Result = result;
            }
          
        }

    }


    
}