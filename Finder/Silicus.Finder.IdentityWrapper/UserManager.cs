using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Finder.IdentityWrapper.Models;

namespace Silicus.Finder.IdentityWrapper
{
    public class UserManager : IUserManager
    {
        public void AssignRoleToUser(string userId, string roleName)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var rolesForUser = userManager.GetRoles(userId);
            if (!rolesForUser.Contains(roleName))
            {
                userManager.AddToRole(userId, roleName);
            }
        }

        public string CreateUserIfNotExist(string name, string password)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                userManager.SetLockoutEnabled(user.Id, false);
            }
            return user.Id;
        }

        public void DeleteUser(string name)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(name);
            
            if (user != null)
            {
                userManager.Delete(user);
            }
            
        }
        public void CreateIdentityRoleIfNotExist(string roleName)
        {
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
        }
        public IEnumerable<IdentityRole> GetAllRoles()
        {
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var roles = roleManager.Roles;
            return roles;
        }

        public void ChangeUserNameOfUser(string newUserName, string oldUserName)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(oldUserName);
            user.UserName = newUserName;
            user.Email = newUserName;
            userManager.Update(user);
            
        }
    }
}
