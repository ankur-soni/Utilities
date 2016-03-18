using Silicus.Finder.IdentityWrapper.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Silicus.Finder.IdentityWrapper.Entities
{
    public class IdentityDbInitializer : CreateDatabaseIfNotExists<IdentityDbContext>
    {
        protected override void Seed(IdentityDbContext context)
        {
            //InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(IdentityDbContext db)
        {
            var userManager =  HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            var adminRole = CreateIdentityRoleIfNotExist(roleManager, roleName);
            var adminUser = CreateUserIfNotExist(userManager, name, password);
            AssignRoleToUser(userManager, adminUser, adminRole);

            var roleUser = CreateIdentityRoleIfNotExist(roleManager, "User");
            var userApplicationUser = CreateUserIfNotExist(userManager, "testuser@test.com", "Testuser@123");
            AssignRoleToUser(userManager, userApplicationUser, roleUser);
        }

        private static void AssignRoleToUser(ApplicationUserManager userManager, ApplicationUser user, IdentityRole role)
        {
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }

        private static ApplicationUser CreateUserIfNotExist(ApplicationUserManager userManager, string name, string password)
        {
            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser {UserName = name, Email = name};
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }
            return user;
        }

        private static IdentityRole CreateIdentityRoleIfNotExist(ApplicationRoleManager roleManager, string roleName)
        {
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
            return role;
        }
    }
}
