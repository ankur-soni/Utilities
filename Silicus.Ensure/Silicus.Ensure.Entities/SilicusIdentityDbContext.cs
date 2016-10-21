using Microsoft.AspNet.Identity.EntityFramework;
using Silicus.Ensure.Entities.Initializer;
using System.Configuration;
using System.Data.Entity;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities
{
    public class SilicusIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SilicusIdentityDbContext(string connectionString) : base(connectionString, false)
        {

        }

        static SilicusIdentityDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<SilicusIdentityDbContext>(new IdentityDbInitializer());
        }

        public static SilicusIdentityDbContext Create()
        {
            return new SilicusIdentityDbContext(ConfigurationManager.ConnectionStrings["UserManagement_Connection"].ConnectionString);
        }
    }
}
