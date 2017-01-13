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
        //private readonly SilicusUtilityContext _context;
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
            var utilityUserRoles = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UtilityId == utilityId && x.RoleId == roleId).ToList();
            if (utilityUserRoles.Count > 0)
            {
                foreach (var utilityUserRole in utilityUserRoles)
                {
                    users.Add(_commonDBContext.Query<User>().Where(x => x.ID == utilityUserRole.UserId).FirstOrDefault());
                }
            }
            return users;
        }
        public List<User> GetAllUsers()
        {
            return _commonDBContext.Query<User>().OrderBy(user => user.DisplayName).ToList();
        }

        public void AddRolesToUserForAUtility(UtilityUserRoles newUserRole)
        {
            var userRole = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UserId == newUserRole.UserId && x.UtilityId == newUserRole.UtilityId).FirstOrDefault();
            //if (userRole != null)
            //{
            //    _commonDBContext.Delete(userRole);
            //}
            _commonDBContext.Add(newUserRole);
        }

        public User FindUserByEmail(string email)
        {
            return _commonDBContext.Query<User>().Where(user=>user.EmailAddress==email).First();
        }

        public void AddRoleToUserForAllUtility(User user)
        {
            var allUtilities = _utilityService.GetAllUtilities();
            var userRole=_roleService.GetRoleByRoleName("User");
            foreach(var utility in allUtilities)
            {
               _commonDBContext.Add(new UtilityUserRoles { UtilityId=utility.Id, UserId=user.ID, RoleId=userRole.ID});
            }
            
        }

        private void RemoveRolesOfUserFromUtility(UtilityUserRoleViewModel utilitiUserRoles)
        {
            var existingUtilityUserRoles = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UtilityId == utilitiUserRoles.UtilityId && x.RoleId == utilitiUserRoles.RoleId).ToList();

            foreach (var existingUtilityUserRole in existingUtilityUserRoles )
            {
              var result =  utilitiUserRoles.UserId.Find(x => x.Equals(existingUtilityUserRole.UserId));

                if (result == 0)
                {
                    var recordToDelete = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UserId == existingUtilityUserRole.UserId && x.UtilityId == utilitiUserRoles.UtilityId && x.RoleId == utilitiUserRoles.RoleId).FirstOrDefault();
                    _commonDBContext.Delete(recordToDelete);
                }
            }

        }

        public void AddRolesToUserForAUtility(UtilityUserRoleViewModel newUserRole)
        {
            var newUserRoles = new UtilityUserRoles();
            var existingUtilityUserRoles = _commonDBContext.Query<UtilityUserRoles>().Where( x => x.UtilityId == newUserRole.UtilityId && x.RoleId == newUserRole.RoleId ).ToList();

            foreach (var item in newUserRole.UserId)
            {

                var userRole = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UserId == item && x.UtilityId == newUserRole.UtilityId).ToList();
              
                 if ( existingUtilityUserRoles.Count() > 0 )
                {
                    var roleToUserAlreadyExists = false;

                    if (existingUtilityUserRoles.Find( x => x.UtilityId == newUserRole.UtilityId && x.RoleId == newUserRole.RoleId && x.UserId == item) != null)
                    {
                        roleToUserAlreadyExists = true;
                    }

                    if ( !roleToUserAlreadyExists )
                    {
                        if (newUserRole.RoleId == Convert.ToInt32(ConfigurationManager.AppSettings["Reviewer"]))
                        {
                            foreach (var userid in newUserRole.UserId)
                            {
                                string url = ConfigurationManager.AppSettings["Addreviewer"] + userid;
                                HttpClient client = new HttpClient();
                                client.BaseAddress = new Uri(url);
                                var response = client.GetAsync(url).Result;
                            }

                        }
                        else
                        {
                            newUserRoles.UtilityId = newUserRole.UtilityId;
                            newUserRoles.RoleId = newUserRole.RoleId;
                            newUserRoles.UserId = item;
                            _commonDBContext.Add(newUserRoles);

                        }
                       
                    }
                   
                }
                else
                {
                    newUserRoles.UtilityId = newUserRole.UtilityId;
                    newUserRoles.RoleId = newUserRole.RoleId;
                    newUserRoles.UserId = item;
                    _commonDBContext.Add(newUserRoles);
                }

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
            return _commonDBContext.Query<User>().Where(user => user.ID == ID).FirstOrDefault();
        }


       public string FindDisplayNameFromEmail(string email)
        {
            var userDisplayName = _commonDBContext.Query<User>().Where(user => user.EmailAddress == email).FirstOrDefault().DisplayName;
            return userDisplayName;
        }

       public List<string> GetAllManagersEmailAddresses()
        {
            var allManagerUseId = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.RoleId == 364).Select(x => x.UserId).ToList();
            var manageremailAdddresses = new List<string>();

            foreach(var id in allManagerUseId)
            {
                manageremailAdddresses.Add(_commonDBContext.Query<User>().Where(user => user.ID == id).SingleOrDefault().EmailAddress);
            }

            return manageremailAdddresses;
        }

    }
}
