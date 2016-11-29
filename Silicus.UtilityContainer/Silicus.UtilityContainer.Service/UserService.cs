using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainer.Entities;
using Silicus.UtilityContainer.Models.ViewModels;

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

        public void AddRolesToUserForAUtility(UtilityUserRoleViewModel newUserRole)
        {
            var myNewUserRole = new UtilityUserRoles();
            foreach (var item in newUserRole.UserId)
            {

                var userRole = _commonDBContext.Query<UtilityUserRoles>().Where(x => x.UserId == item && x.UtilityId == newUserRole.UtilityId).ToList();
                //if (userRole != null)
                //{
                //    _commonDBContext.Delete(userRole);
                //}

                myNewUserRole.UtilityId = newUserRole.UtilityId;
                myNewUserRole.RoleId = newUserRole.RoleId;
                myNewUserRole.UserId = item;
                _commonDBContext.Add(myNewUserRole);

            }


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
