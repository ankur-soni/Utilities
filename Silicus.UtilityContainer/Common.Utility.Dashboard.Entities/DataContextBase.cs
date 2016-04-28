using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Silicus.UtilityContainer.Models.DataObjects;
using Silicus.UtilityContainerr.Entities.EntityConfigurations;

namespace Silicus.UtilityContainerr.Entities
{
    /// <summary>
    ///     This class registers the entities that are comprised
    ///     in the model.
    /// </summary>
    public abstract class DataContextBase : DbContext
    {
        /// <summary>
        ///     Calls the base class with the given connection string.
        /// </summary>
        protected DataContextBase(string connectionString)
            : base(connectionString)
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  Disable the default PluralizingTableNameConvention 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UtilityMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UtilityUserRolesMap());
            modelBuilder.Configurations.Add(new EngagementMap());
            modelBuilder.Configurations.Add(new EngagementUserPermissionsMap());
            modelBuilder.Configurations.Add(new EngagementRoleMap());
            modelBuilder.Configurations.Add(new ResourceMap());
            modelBuilder.Configurations.Add(new ResourceHistoryMap());
            modelBuilder.Configurations.Add(new EngagementTypeMap());
            modelBuilder.Configurations.Add(new ResourceSkillLevelMap());
            modelBuilder.Configurations.Add(new TitleMap());
            modelBuilder.Configurations.Add(new ResourceTypeMap());
            modelBuilder.Configurations.Add(new UtilityRoleMap());

            // Many-to-many example - can be moved to Map file as well.
            modelBuilder.Entity<Asset>()
                .HasMany(s => s.Categories)
                .WithMany(c => c.Assets)
                .Map(cs =>
                {
                    cs.MapLeftKey("AssetId");
                    cs.MapRightKey("CategoryId");
                    cs.ToTable("AssetCategory");
                });
        }
    }
}
