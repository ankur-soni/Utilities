using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silicus.UtilityContainerr.Entities;

namespace Silicus.UtilityContainer.Services
{
    public class UserService : IUserService
    {
        private readonly SilicusUtilityContext _context;
        private readonly IUtilityService _utilityService;

        public UserService(IUtilityService utilityService)
        {
            _context = new SilicusUtilityContext();
            _utilityService = utilityService;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void AddRolesToUserForAUtility(UserRole newUserRole)
        {
            var userRole = _context.UserRoles.Where(x => x.UserId == newUserRole.UserId && x.UtilityId == newUserRole.UtilityId).FirstOrDefault();
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
            }
            _context.UserRoles.Add(newUserRole);
            _context.SaveChanges();
        }

        public void CreateUserIfNotExists(User newUser)
        {
            if (_context.Users.Where(x => x.Name == newUser.Name).FirstOrDefault() == null)
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                var allUtilities = _utilityService.GetAllUtilities();
                foreach (var utility in allUtilities)
                {
                    AddRolesToUserForAUtility(new UserRole() { RoleId = 1, UserId = newUser.Id, UtilityId = utility.Id });
                }

            }
        }
    }
}
