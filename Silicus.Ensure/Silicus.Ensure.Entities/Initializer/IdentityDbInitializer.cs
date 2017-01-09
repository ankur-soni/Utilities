using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Silicus.Ensure.Entities.Identity;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities.Initializer
{
    public class IdentityDbInitializer : CreateDatabaseIfNotExists<SilicusIdentityDbContext>
    {
        protected override void Seed(SilicusIdentityDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(SilicusIdentityDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Candidate";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            const string nameCandidate = "lorem@gmail.com";
            const string passwordCandidate = "Candidate@123";
            const string roleNameCandidate = "Candidate";

            //Create Role Candidate if it does not exist
            var role1 = roleManager.FindByName(roleNameCandidate);
            if (role1 == null)
            {
                role1 = new IdentityRole(roleNameCandidate);
                var roleresult = roleManager.Create(role1);
            }

            var user1 = userManager.FindByName(nameCandidate);
            if (user1 == null)
            {
                user1 = new ApplicationUser { UserName = nameCandidate, Email = nameCandidate };
                var result = userManager.Create(user1, passwordCandidate);
                result = userManager.SetLockoutEnabled(user1.Id, false);
            }

            // Add user Candidate to Role Candidate if not already added
            var rolesForUser1 = userManager.GetRoles(user1.Id);
            if (!rolesForUser1.Contains(role1.Name))
            {
                var result = userManager.AddToRole(user1.Id, role1.Name);
            }

            const string namePanel = "nilkanth@gmail.com";
            const string passwordPanel = "Panel@123";
            const string roleNamePanel = "Panel";

            //Create Role Panel if it does not exist
            var rolePanle = roleManager.FindByName(roleNamePanel);
            if (rolePanle == null)
            {
                rolePanle = new IdentityRole(roleNamePanel);
                var roleresult = roleManager.Create(rolePanle);
            }

            var userPanel = userManager.FindByName(namePanel);
            if (userPanel == null)
            {
                userPanel = new ApplicationUser { UserName = namePanel, Email = namePanel };
                var result = userManager.Create(userPanel, passwordPanel);
                result = userManager.SetLockoutEnabled(userPanel.Id, false);
            }

            // Add user Panel to Role Panel if not already added
            var rolesForUserPanel = userManager.GetRoles(userPanel.Id);
            if (!rolesForUserPanel.Contains(rolePanle.Name))
            {
                var result = userManager.AddToRole(userPanel.Id, rolePanle.Name);
            }
        }
    }
}
