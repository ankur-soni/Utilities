using Silicus.Encourage.DAL.Interfaces;
using Silicus.Encourage.Services.Interface;
using Silicus.Encourage.Web.Models;
using Silicus.UtilityContainer.Security;
using Silicus.UtilityContainer.Security.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Silicus.Encourage.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthentication _userAuthentication;
        private readonly IDataContextFactory _context;
        private readonly IUserSecurityService _securityService;
        private readonly ICommonDbService _commonDbService;

        public AccountController(IAuthentication userAuthentication, IDataContextFactory context, IUserSecurityService securityService, ICommonDbService commonDbService)
        {
            _userAuthentication = userAuthentication;
            _context = context;
            _securityService = securityService;
            _commonDbService = commonDbService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Login(string returnUrl, int utilityID = 0)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = Request.Cookies[".ADAuthCookie"];

            if (authCookie == null)
            {
                // cookie to check if user logins directly in finder
                HttpCookie DirectLoginInFinderCookie = new HttpCookie("DirectLoginInFinderCookie");
                DirectLoginInFinderCookie.Value = "abcd";
                Response.Cookies.Add(DirectLoginInFinderCookie);
                return Redirect(ConfigurationManager.AppSettings["utilityContainer"] + returnUrl);
            }

            if (authCookie.Value != null)
            {
                var model = new LoginModel();
                var authenticationTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var username = authenticationTicket.Name;
                var password = authenticationTicket.UserData;

                if (ModelState.IsValid)
                {
                    var userFromAd = Membership.GetUser(username);

                    var encourageDbContext = _context.CreateEncourageDbContext();
                    var commonDbContext = _context.CreateCommonDbContext();

                    string utility = WebConfigurationManager.AppSettings["ProductName"];

                    var authorizationService = new Authorization(_commonDbService.GetCommonDataBaseContext());
                    var user = Membership.GetUser(username);


                    Session["UserEmailAddress"] = user.Email;

                    var commonRoles = authorizationService.GetRoleForUtility(user.Email, utility);
                    
                        if ((commonRoles.Count>0))
                            HttpContext.Session["Role"] = commonRoles;
                    else
                    {
                        return Redirect("http://localhost:52250/Home/Index?data=" + "No role Assigned for " + username + " in " + utility + " uitility!");
                    }


                   

                    // HttpContext.Session["Role"] = commonRole;
                    var loginResult = _securityService.PasswordSignInAsync(username, password);

                    switch (loginResult)
                    {
                        case "Success":
                            FormsAuthentication.SetAuthCookie(username, model.RememberMe);
                            var result = Request.IsAuthenticated;
                            Session["CurrentUser"] = username.ToUpper();
                            var isAdmin = false;

                            if (commonRoles.Contains( "Admin"))
                            {
                                isAdmin = true;
                            }
                            return RedirectToLocal(returnUrl, username, isAdmin);
                        case "Failure":
                            if (username != null)
                            {
                                string message = "The password does not matches with your username, please enter the correct one.";
                                ModelState.AddModelError("", message);
                                return Redirect(ConfigurationManager.AppSettings["utilityContainer"]);
                            }
                            ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                            return Redirect(ConfigurationManager.AppSettings["utilityContainer"]);
                        default:
                            ModelState.AddModelError("", "User Name and Password does not matches! Please provide your correct login credentials.");
                            return Redirect(ConfigurationManager.AppSettings["utilityContainer"]);
                    }
                }
                return View(model);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        private ActionResult RedirectToLocal(string returnUrl, string userName = "", bool isAdmin = false)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Dashboard", new { });
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
