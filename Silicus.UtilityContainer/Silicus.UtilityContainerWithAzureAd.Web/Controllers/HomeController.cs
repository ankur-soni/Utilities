using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Silicus.UtilityContainer.Services.Interfaces;
using Silicus.UtilityContainer.Models.ViewModels;
using Silicus.UtilityContainer.Models.DataObjects;

namespace Test7.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
        private const string LoginUrl = "https://login.windows.net/{0}";
        private const string GraphUrl = "https://graph.windows.net";
        private const string GraphUserUrl = "https://graph.windows.net/{0}/users/{1}?api-version=2013-04-05";
        private static readonly string AppPrincipalId = ConfigurationManager.AppSettings["ida:ClientID"];
        private static readonly string AppKey = ConfigurationManager.AppSettings["ida:Password"];




        private readonly IUtilityService _utilityService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public HomeController(IUtilityService utilityService, IRoleService roleService, IUserService userService)
        {
            _utilityService = utilityService;
            _roleService = roleService;
            _userService = userService;
        }

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(string data)
        {
            var allUtilities = _utilityService.GetAllUtilities();
            ViewBag.noRoleForCUrrentUser = data;
            return View("Dashboard", allUtilities);
        }


        public FileContentResult GetImg(int id)
        {
            byte[] byteArray = _utilityService.FindUtility(id).UtilityIcon;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }

        public ActionResult AddRolesToUserForAUtility()
        {

            var newUserRole = new UtilityUserRoleViewModel();
            // ViewData["User"] = new SelectList(_userService.GetAllUsers(), "ID", "DisplayName", "Select");

            var selectListItems = _userService.GetAllUsers().Select(u => new SelectListItem() { Text = u.ID.ToString(), Value = u.DisplayName }).ToList();

            ViewData["User"] = selectListItems;

            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name", "Select");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name", "Select");
            //var role = _roleService.GetAllRoles();
            return View(newUserRole);
        }

        [HttpPost]
        public ActionResult AddRolesToUserForAUtility(UtilityUserRoleViewModel newUserRole)
        {

            if (newUserRole.RoleId != 0)
            {
                _userService.AddRolesToUserForAUtility(new UtilityUserRoleViewModel { UtilityId = newUserRole.UtilityId, RoleId = newUserRole.RoleId, UserId = newUserRole.UserId });
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddRoleToUtility()
        {
            var newUtilityRole = new UtilityRole();
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name");
            return View(newUtilityRole);
        }

        [HttpPost]
        public ActionResult AddRoleToUtility(UtilityRole newUtilityRole)
        {
            ViewData["Utilities"] = new SelectList(_utilityService.GetAllUtilities(), "Id", "Name");
            ViewData["Roles"] = new SelectList(_roleService.GetAllRoles(), "ID", "Name");
            _utilityService.SaveUtilityRole(newUtilityRole);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult FillRoles(int utilityId)
        {

            var roles = _utilityService.GetAllRolesForAnUtility(utilityId);
            //SelectList obgroles = new SelectList(roles, "Id", "RoleName", 0);
            var roleData = roles.Select(m => new SelectListItem()
            {
                Text = _roleService.GetRoleName(m.RoleID),
                Value = m.RoleID.ToString(),
            });
            return Json(roleData, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public string GetUserByID(string userId)
        {


            var user = _userService.GetUserByID(Convert.ToInt32(userId));



            return user.DisplayName;
        }
    


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public async Task<ActionResult> UserProfile()
        {
            string tenantId = ClaimsPrincipal.Current.FindFirst(TenantIdClaimType).Value;

            // Get a token for calling the Windows Azure Active Directory Graph
            AuthenticationContext authContext = new AuthenticationContext(String.Format(CultureInfo.InvariantCulture, LoginUrl, tenantId));
            ClientCredential credential = new ClientCredential(AppPrincipalId, AppKey);
            AuthenticationResult assertionCredential = authContext.AcquireToken(GraphUrl, credential);
            string authHeader = assertionCredential.CreateAuthorizationHeader();
            string requestUrl = String.Format(
                CultureInfo.InvariantCulture,
                GraphUserUrl,
                HttpUtility.UrlEncode(tenantId),
                HttpUtility.UrlEncode(User.Identity.Name));

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.TryAddWithoutValidation("Authorization", authHeader);
            HttpResponseMessage response = await client.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();
             //UserProfile profile = JsonConvert.DeserializeObject<UserProfile>(responseString);

           // return View(profile);
            return View();
        }
    }
}