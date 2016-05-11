using Silicus.Encourage.DAL.EntityConfigurations;
using Silicus.Encourage.Models.DataObjects;
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
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  Disable the default PluralizingTableNameConvention 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new AwardMap());
            modelBuilder.Configurations.Add(new FrequencyMasterMap());          
        }
    }
}
