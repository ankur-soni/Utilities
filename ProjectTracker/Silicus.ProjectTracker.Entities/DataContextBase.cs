using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Silicus.ProjectTracker.Core;
using Silicus.ProjectTracker.Entities.EntityConfigurations;

namespace Silicus.ProjectTracker.Entities
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

            modelBuilder.Configurations.Add(new ProjectMappingMap());

            modelBuilder.Configurations.Add(new ManagerDetailMap());

            modelBuilder.Configurations.Add(new EmailAvailableMap());

            modelBuilder.Configurations.Add(new ProjectSummaryMap());

            modelBuilder.Configurations.Add(new ProjectStatusMap());

            modelBuilder.Configurations.Add(new WeekMap());                        

            modelBuilder.Configurations.Add(new ProjectComplaintMap());

            modelBuilder.Configurations.Add(new ProjectResouceMap());

            modelBuilder.Configurations.Add(new PaymentDetailsMap());

            modelBuilder.Configurations.Add(new InfrastructureDetailsMap());

            modelBuilder.Configurations.Add(new ChangeRequestDetailsMap());  
        }
    }
}