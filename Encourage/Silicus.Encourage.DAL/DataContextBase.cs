using Silicus.Encourage.DAL.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL
{
    public class DataContextBase : DbContext
    {
        /// <summary>
        ///     Calls the base class with the given connection string.
        /// </summary>
        protected DataContextBase(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new AwardMap());
            modelBuilder.Configurations.Add(new FrequencyMasterMap());
            modelBuilder.Configurations.Add(new CriteriaMap());
            modelBuilder.Configurations.Add(new NominationMap());
            modelBuilder.Configurations.Add(new ReviewerCommentMap());
            modelBuilder.Configurations.Add(new ManagerCommentMap());
            modelBuilder.Configurations.Add(new ReviewerMap());
            modelBuilder.Configurations.Add(new AwardCriteriaMap());
            modelBuilder.Configurations.Add(new ShortlistMap());
            modelBuilder.Configurations.Add(new ConfigurationMap());
            modelBuilder.Configurations.Add(new EmailTemplateMap());
        }
    }
}
