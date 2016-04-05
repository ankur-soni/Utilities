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

        public UserService()
        {
            _context = new SilicusUtilityContext();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void AddRolesToUserForAUtility(UserRole newUserRole)
        {
            _context.UserRoles.Add(newUserRole);
            _context.SaveChanges();
        }
    }
}
