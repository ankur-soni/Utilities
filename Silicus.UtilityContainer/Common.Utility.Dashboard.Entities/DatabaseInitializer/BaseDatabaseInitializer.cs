using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Security;
using Silicus.UtilityContainer.Models.DataObjects;
using System.Web;

namespace Silicus.UtilityContainerr.Entities.DatabaseInitializer
{
    class BaseDatabaseInitializer : DropCreateDatabaseIfModelChanges<SilicusUtilityContext>
    {
        protected override void Seed(SilicusUtilityContext context)
        {
            //get all members frm active directory.
            var allMembers = Membership.GetAllUsers();

            //Insert roles into database
            IList<Role> defaultRoles = new List<Role>();
            context.Roles.Add(new Role { RoleName = "User" });
            context.Roles.Add(new Role { RoleName = "Admin" });
            context.Roles.Add(new Role { RoleName = "Manager" });
            foreach (var role in defaultRoles)
            context.Roles.Add(role);
            context.SaveChanges();

            //Insert utilities into database
            Image img = Image.FromFile(HttpContext.Current.Server.MapPath("~\\Images\\loginbackground.jpg"));
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }

            context.Utilities.Add(new Utility { Name = "Finder", Description = "Find Employee", Url = "http://localhost:53393", UtilityIcon = arr });
            img = Image.FromFile(HttpContext.Current.Server.MapPath("~\\Images\\Enable Logo.PNG"));
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            context.Utilities.Add(new Utility { Name = "Enable", Description = "Time Sheets", Url = "https://entime.silicus.com/", UtilityIcon = arr });
            img = Image.FromFile(HttpContext.Current.Server.MapPath("~\\Images\\PROVARE-logo.jpg"));
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            context.Utilities.Add(new Utility { Name = "Provare", Description = "test", Url = "#", UtilityIcon = arr });
            context.SaveChanges();

            //IList<User> defaultUsers = new List<User>();
            var allUtilities = context.Utilities.ToList();
           //Adding AD users to DB
            foreach (var member in allMembers)
            {
                MembershipUser currentUser = (MembershipUser)member;
                var newUser = new User() { Name = currentUser.UserName };
                context.Users.Add(newUser);
                context.SaveChanges();

                foreach (var utility in allUtilities)
                {
                    var newUserRole = new UserRole() { UserId = newUser.Id, UtilityId = utility.Id, RoleId = 1 };
                    context.UserRoles.Add(newUserRole);
                    context.SaveChanges();
                }
            }

            //defaultUsers.Add(new User() { Name = "Yeshwant", Designation = "trainee" });
            //defaultUsers.Add(new User() { Name = "Abhishek", Designation = "trainee" });
            //foreach (var user in defaultUsers)
            //    context.Users.Add(user);

            base.Seed(context);
        }
    }
}
