using System;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using Silicus.Reusable.DAL.EntityConfigurations;

namespace Silicus.Reusable.DAL
{
    public class DataContextBase: DbContext
    {
        protected DataContextBase(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //  Disable the default PluralizingTableNameConvention 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new FrameworxMap());
            modelBuilder.Configurations.Add(new CategoryMap());
        }
    }
}
