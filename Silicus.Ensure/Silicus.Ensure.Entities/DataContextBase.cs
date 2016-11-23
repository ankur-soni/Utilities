//using Eda.RDBI.Models.DataObjects;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Silicus.FrameWorx.Utility;
using Silicus.Ensure.Entities.EntityConfigurations;
using Silicus.Ensure.Models.DataObjects;

namespace Silicus.Ensure.Entities
{
    /// <summary>
    /// This class registers the entities that are comprised
    /// in the model.
    /// </summary>
    public abstract class DataContextBase : DbContext
    {
        /// <summary>
        /// Calls the base class with the given connection string.
        /// </summary>
        protected DataContextBase(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  Disable the default PluralizingTableNameConvention 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 

            // Register Entities.
            Guard.ArgumentNotNull(modelBuilder, "modelBuilder");

            modelBuilder.Configurations.Add(new OrganizationMap());

            modelBuilder.Configurations.Add(new ProjectMap());

            modelBuilder.Configurations.Add(new ProjectDetailMap());

            modelBuilder.Configurations.Add(new ManagerDetailMap());

            modelBuilder.Configurations.Add(new EmailAvailableMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RolesMap());
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new TagsMap());
            modelBuilder.Configurations.Add(new TestSuiteMap());
            modelBuilder.Configurations.Add(new UserTestSuiteMap());
            modelBuilder.Configurations.Add(new UserTestDetailsMap());
            modelBuilder.Configurations.Add(new PositionMap());

            // Many-to-many example - can be moved to Map file as well.
            modelBuilder.Entity<Asset>()
            .HasMany<Category>(s => s.Categories)
            .WithMany(c => c.Assets)
            .Map(cs =>
            {
                cs.MapLeftKey("AssetId");
                cs.MapRightKey("CategoryId");
                cs.ToTable("AssetCategory");
            });

            modelBuilder.Entity<UserTestSuite>()
                .HasMany<UserTestDetails>(u => u.UserTestDetails)
                .WithRequired(x => x.UserTestSuite).Map(x => x.MapKey("UserTestSuiteId"));

            modelBuilder.Entity<TestSuite>()
                .HasMany<TestSuiteTag>(u => u.TestSuiteTags)
                .WithRequired(x => x.TestSuite).Map(x => x.MapKey("UserTestSuiteId"));
        }
    }
}