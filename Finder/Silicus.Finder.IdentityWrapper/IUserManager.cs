using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Silicus.Finder.IdentityWrapper.Models;

namespace Silicus.Finder.IdentityWrapper
{
    public interface IUserManager
    {
        void AssignRoleToUser(string userId, string roleName);
        string CreateUserIfNotExist(string name, string password);
        void CreateIdentityRoleIfNotExist(string roleName);
        IEnumerable<IdentityRole> GetAllRoles();
        void ChangeUserNameOfUser(string newUserName, string oldUserName);
        void DeleteUser(string name);
    }
}
