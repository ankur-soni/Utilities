using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Silicus.Ensure.Web.Mappings;
using Silicus.Ensure.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Silicus.Ensure.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Silicus.UtilityContainer.Services.Interfaces.IUserService _containerUserService;
        private readonly IMappingService _mappingService;
        private Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService _utilityUserRoleService;

        public EmployeeController(MappingService mappingService, UtilityContainer.Services.Interfaces.IUserService containerUserService, Silicus.UtilityContainer.Services.Interfaces.IUtilityUserRoleService utilityUserRoleService)
        {
            //_positionService = positionService;
            //_userService = userService;
            _mappingService = mappingService;
            //_testSuiteService = testSuiteService;
            _containerUserService = containerUserService;
            //_utilityService = utilityService;
            _utilityUserRoleService = utilityUserRoleService;
            //_panelMemberService = panelMemberService;
            //_emailService = emailService;
            //_commonController = commonController;
            //_tagsService = tagService;
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View("EmployeeList");
        }

        private int GetUtilityId()
        {
            var utilityProductId = WebConfigurationManager.AppSettings["ProductId"];
            if (string.IsNullOrWhiteSpace(utilityProductId))
            {
                throw new ArgumentNullException();
            }

            return Convert.ToInt32(utilityProductId);
        }

        public ActionResult GetUserDetails([DataSourceRequest] DataSourceRequest request)
        {
            var userlist = _containerUserService.GetAllUsers();
            var userlistViewModel = _mappingService.Map<List<Silicus.UtilityContainer.Models.DataObjects.User>, List<UserDetailViewModel>>(userlist);
            var UtilityId = GetUtilityId();
            var userRoles = _utilityUserRoleService.GetAllUserRolesForUtility(UtilityId);

            var userWithRoles = (from userinRoles in userRoles
                                 join allUsers in userlistViewModel
                                 on userinRoles.UserId equals allUsers.UserId
                                 where userinRoles.IsActive && userinRoles.RoleId == 5
                                 select new UserDetailViewModel
                                 {
                                     RoleName = userinRoles?.Role?.Name,
                                     UserName = allUsers.UserName,
                                     Department = allUsers.Department,
                                     Designation = allUsers.Designation,
                                     Email = allUsers.Email,
                                     FirstName = allUsers.FirstName,
                                     FullName = allUsers.FullName,
                                     LastName = allUsers.LastName,
                                     MiddleName = allUsers.MiddleName,
                                     EmployeeId = allUsers.EmployeeId,
                                     RoleId = userinRoles.Role.ID,
                                     UserId = allUsers.UserId
                                 }).ToList();

            DataSourceResult result = userWithRoles.ToDataSourceResult(request);
            return Json(result);
        }
    }
}