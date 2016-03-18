using Silicus.Finder.IdentityWrapper.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data.Entity;

namespace Silicus.Finder.IdentityWrapper.Entities
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext(string connectionString)
            : base(connectionString, false)
        {

        }

        static IdentityDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<IdentityDbContext>(new IdentityDbInitializer());
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext(ConfigurationManager.ConnectionStrings["UserManagement_Connection"].ConnectionString);
        }
    }
}
