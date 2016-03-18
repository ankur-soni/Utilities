using System;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Silicus.ProjectTracker.Entities;
using Silicus.ProjectTracker.Logger;
using Silicus.ProjectTracker.Models.DataObjects;
using Silicus.ProjectTracker.Web.Filters;
using Silicus.ProjectTracker.Web.Models;
using Silicus.ProjectTracker.Web.UserMembership;
using System.DirectoryServices.AccountManagement;
using Silicus.ProjectTracker.Services.Interfaces;
using Silicus.ProjectTracker.Core;

namespace Silicus.ProjectTracker.Web.Controllers
{  
    public class AccountController : Controller
    {
        //GET: /Account/Login
        private readonly IMembershipService _membershipService;
        private readonly ICookieHelper _cookieHelper;
        private readonly ILogger _logger;
        private readonly IDataContextFactory _dataContextFactory;
        private readonly IActiveDirectoryService _activeDirectoryService;
           
        public AccountController(IMembershipService membershipService,
            ICookieHelper cookieHelper, ILogger logger,IActiveDirectoryService activeDirectoryService,
            IDataContextFactory dataContextFactory)
        {
            this._membershipService = membershipService;
            this._cookieHelper = cookieHelper;
            this._logger = logger;
            this._activeDirectoryService = activeDirectoryService;
            this._dataContextFactory = dataContextFactory;
        }
              
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            using (var context = _dataContextFactory.Create(ConnectionType.Ip))
            {
                context.Query<Organization>().Count();
            }


            string name = string.Empty;
            name = _cookieHelper.GetCookie("userid");

            if (name != null && name != string.Empty)
            {
                string role = this._activeDirectoryService.VerifyGroupPolicy(name);

                if (role == Constants.AdminRole)
                {
                    return RedirectToLocal(returnUrl, Constants.AdminRole);
                }
                // check if user is member of that user group
                else if (role == Constants.UserRole)
                {
                    return RedirectToLocal(returnUrl, Constants.UserRole);
                }
                else
                {
                    // user does not belong to active directory
                    _logger.Log(string.Format("User is not found in the active directory: {0}", name), LogCategory.Warning, GetUserIdentifiableString(name));
                }
            }
            else
            {
                _logger.Log(string.Format("Failed login attempt for user : {0}", name), LogCategory.Information, GetUserIdentifiableString(name));
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Audit]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            _logger.Log(string.Format("Login request received for user : {0}", model.UserName),
                LogCategory.Information, GetUserIdentifiableString(model.UserName));
            
            if (ModelState.IsValid)
            {
                bool result = this._activeDirectoryService.VerifyLoggedInUser(model.UserName, model.Password);
                if (result)
                {
                    string role = this._activeDirectoryService.VerifyGroupPolicy(model.UserName);
                           

                    if (role == Constants.AdminRole)
                    {
                        _cookieHelper.SetCookie("userid", model.UserName, new TimeSpan(30, 0, 0));
                        _cookieHelper.SetCookie("adminMenuVisibility", "YES", new TimeSpan(30, 0, 0));
                        return RedirectToLocal(returnUrl, Constants.AdminRole);
                    }
                    // check if user is member of that user group
                    else if (role == Constants.UserRole)
                    {
                        _cookieHelper.SetCookie("userid", model.UserName, new TimeSpan(30, 0, 0));
                        _cookieHelper.SetCookie("adminMenuVisibility", "NO", new TimeSpan(30, 0, 0));
                        return RedirectToLocal(returnUrl, Constants.UserRole);

                    }
                    else
                    {
                        _cookieHelper.SetCookie("userid", string.Empty, new TimeSpan(30, 0, 0));
                        // user does not belong to active directory
                        _logger.Log(string.Format("User is not found in the active directory: {0}", model.UserName), LogCategory.Warning, GetUserIdentifiableString(model.UserName));

                    }
                }
                else
                {
                    _logger.Log(string.Format("Failed login attempt for user : {0}", model.UserName), LogCategory.Information, GetUserIdentifiableString(model.UserName));
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");

            return View(model);

        }
               
        [AllowAnonymous]
        [Audit]
        public ActionResult LogOff()
        {
            var userName = "Unknown";

            if (HttpContext.User != null && !string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                userName = HttpContext.User.Identity.Name;
            }

            _logger.Log(string.Format("LogOff request received for user : {0}", userName),
                LogCategory.Information, GetUserIdentifiableString(userName));

            return LogUserOut();
        }
                
        private ActionResult LogUserOut()
        {
            Session.Abandon();
            _cookieHelper.ClearAllCookies();

            FormsAuthentication.SignOut();
                                        
            return RedirectToAction("Login", "Account");
        }

        private string GetUserIdentifiableString(string userName)
        {
            return Session.SessionID + "-" + userName;
        }

        private ActionResult RedirectToLocal(string returnUrl, string role = "")
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            ViewBag.AdminMenuVisibilty = true;

            //////If the user is having admin roles 
            if (role == Constants.AdminRole)
            {
                return RedirectToAction("Dashboard", Constants.AdminRole);
            }
            else if (role == Constants.UserRole)
            {
                return RedirectToAction("Dashboard", Constants.UserRole);
            }
            else
            {
                return RedirectToAction("CustomError", "Error");
            }
        }
    }
}
