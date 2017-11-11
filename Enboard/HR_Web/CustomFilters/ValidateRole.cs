using HR_Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR_Web.CustomFilters
{
    public class ValidateRole : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if normal user try to access Admi Functionality
            try
            {
                if (SessionManager.RoleId == 0 || SessionManager.RoleId == -1)
                {
                    Controller controller = filterContext.Controller as Controller;
                    filterContext.Result = new RedirectResult(controller.Url.Content("~/") + "User/LogOut");
                    return;

                }
                else
                {
                    base.OnActionExecuting(filterContext);
                }
            }
            catch 
            {
                base.OnActionExecuting(filterContext);
            
            }
        }
    }
}