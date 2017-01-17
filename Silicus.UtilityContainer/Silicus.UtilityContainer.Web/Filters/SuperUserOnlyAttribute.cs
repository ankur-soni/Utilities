using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.DataObjects;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace Silicus.UtilityContainer.Web.Filters
{
    public class SuperUserOnlyAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/UnAuthorizedError.cshtml"
                    };
               
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var commondbContext = new CommonDataBaseContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var superUsers = commondbContext.Query<SuperUser>().ToList();
            var loggedInUser = HttpContext.Current.User.Identity.Name; ;
            var status = superUsers.Find(x => x.Email.ToLower() == loggedInUser.ToLower()) != null;
            return status;
        }
    }
}