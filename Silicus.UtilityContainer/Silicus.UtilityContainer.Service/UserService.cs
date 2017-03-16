using System.Net;
using System.Web;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.ViewModels;
using System.Net.Http;
using System.Configuration;

namespace Silicus.UtilityContainer.Services
{
    public class UserService : IUserService
    {
        private readonly IUtilityService _utilityService;
        private readonly IUtilityUserRoleService _utilityUserRoleService;
        private readonly ICommonDataBaseContext _commonDBContext;
        private readonly IRoleService _roleService;

        public UserService(IUtilityService utilityService, IDataContextFactory dataContextFactory, IUtilityUserRoleService utilityUserRoleService, IRoleService roleService)
        {
            _commonDBContext = dataContextFactory.CreateCommonDBContext();
            _utilityService = utilityService;
            _utilityUserRoleService = utilityUserRoleService;
            _roleService = roleService;
        }

        public List<User> GetAllUsersByRoleInUtility(int utilityId, int roleId)
        {
            var users = new List<User>();
            var utilityUserRoles = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UtilityId == utilityId && x.RoleId == roleId && x.IsActive).ToList();
            if (utilityUserRoles.Count > 0)
            {
                foreach (var utilityUserRole in utilityUserRoles)
                {
                    users.Add(_commonDBContext.Query<User>().FirstOrDefault(x => x.ID == utilityUserRole.UserId));
                }
            }
            return users;
        }

        public List<User> GetAllUsers()
        {
            return _commonDBContext.Query<User>().Where(u => u.InactiveDate == null).OrderBy(user => user.DisplayName).ToList();
        }
        
        public User FindUserByEmail(string email)
        {
            return _commonDBContext.Query<User>().First(user => user.EmailAddress == email);
        }

        public void AddRoleToUserForAllUtility(User user)
        {
            var allUtilities = _utilityService.GetAllUtilities();
            var userRole = _roleService.GetRoleByRoleName("User");
            foreach (var utility in allUtilities)
            {
                _commonDBContext.Add(new UtilityUserRoles { UtilityId = utility.Id, UserId = user.ID, RoleId = userRole.ID });
            }

        }

        private void RemoveRolesOfUserFromUtility(UtilityUserRoleViewModel utilitiUserRoles)
        {
            var existingUtilityUserRoles = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UtilityId == utilitiUserRoles.UtilityId && x.RoleId == utilitiUserRoles.RoleId).ToList();

            foreach (var existingUtilityUserRole in existingUtilityUserRoles)
            {
                if (utilitiUserRoles.UserId.All(x => x != existingUtilityUserRole.UserId))
                {
                    var recordToDelete = _commonDBContext.Query<UtilityUserRoles>().FirstOrDefault(x => x.UserId == existingUtilityUserRole.UserId && x.UtilityId == utilitiUserRoles.UtilityId && x.RoleId == utilitiUserRoles.RoleId);
                    if (recordToDelete != null)
                    {
                        recordToDelete.IsActive = false;
                        _commonDBContext.Update(recordToDelete);
                    }
                }
            }
        }

        public void AddRolesToUserForAUtility(UtilityUserRoleViewModel newUserRole)
        {
            var newUserRoles = new UtilityUserRoles();
            
            foreach (var userId in newUserRole.UserId)
            {
                var userRole = _commonDBContext.Query<UtilityUserRoles>().FirstOrDefault(x => x.UserId == userId && x.UtilityId == newUserRole.UtilityId && x.RoleId == newUserRole.RoleId);
                if (userRole != null)
                {
                    userRole.IsActive = true;
                    _commonDBContext.Update(userRole);
                }
                else
                {
                    newUserRoles.UtilityId = newUserRole.UtilityId;
                    newUserRoles.RoleId = newUserRole.RoleId;
                    newUserRoles.UserId = userId;
                    newUserRoles.IsActive = true;
                    _commonDBContext.Add(newUserRoles);
                }

                if (HttpContext.Current.Request.IsLocal)
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }

                string url = ConfigurationManager.AppSettings["Addreviewer"] + userId;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync(url).Result;
            }

            RemoveRolesOfUserFromUtility(newUserRole);
        }

        public bool CheckForFirstLoginByEmail(string email)
        {
            bool status = false;
            var user = FindUserByEmail(email);
            var utilityRegisteredCount = _utilityUserRoleService.GetAllRolesForUser(user.UserName).Count;
            if (utilityRegisteredCount == 0)
                status = true;
            return status;
        }

        public User GetUserByID(int ID)
        {
            return _commonDBContext.Query<User>().FirstOrDefault(user => user.ID == ID);
        }


        public string FindDisplayNameFromEmail(string email)
        {
            var userDisplayName = string.Empty;
            var firstOrDefault = _commonDBContext.Query<User>().FirstOrDefault(user => user.EmailAddress == email);
            if (firstOrDefault != null)
            {
                userDisplayName = firstOrDefault.DisplayName;
            }

            return userDisplayName;
        }

        public List<string> GetAllManagersEmailAddresses()
        {
            var allManagerUseId = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.RoleId == 364).Select(x => x.UserId).ToList();
            var manageremailAdddresses = new List<string>();

            foreach (var id in allManagerUseId)
            {
                var singleOrDefault = _commonDBContext.Query<User>().SingleOrDefault(user => user.ID == id);
                if (singleOrDefault != null)
                {
                    manageremailAdddresses.Add(singleOrDefault.EmailAddress);
                }
            }

            return manageremailAdddresses;
        }

    }
}
