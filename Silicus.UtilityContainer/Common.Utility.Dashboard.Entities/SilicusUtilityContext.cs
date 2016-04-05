using System.Data.Entity;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities.DatabaseInitializer;

namespace Silicus.UtilityContainerr.Entities
{
    public class SilicusUtilityContext : DbContext
    {
        public SilicusUtilityContext()
            : base("SilicusUtilities_commonDB")
        {
            Database.SetInitializer<SilicusUtilityContext>(new BaseDatabaseInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
    }
}
