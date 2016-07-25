using Silicus.FrameworxProject.DAL.Interfaces;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL
{
    public class DataContextFactory : IDataContextFactory
    {
        //public object ConfigurationManager { get; private set; }

        public ICommonDatabaseContext CreateCommonDbContext()
        {

            ICommonDatabaseContext dataContext = null;

            dataContext = new CommonDatabaseContext("DefaultConnection");

            return dataContext;

        }

        public IFrameworxProjectDatabaseContext CreateFrameworxProjectDbContext()
        {
            //IReusableDatabaseContext dataContext = null;

            // dataContext = new ReusableDatabaseContext(@"Data Source=SILICUS273\SQLEXPRESS;Integrated Security=True;Initial Catalog=StudentModel");

            //return dataContext;
            //return new ReusableDatabaseContext(@"Data Source=SILICUS512\SQLEXPRESS;Integrated Security=True;Initial Catalog=ReusableDB");
            return new FrameworxProjectDatabaseContext(ConfigurationManager.ConnectionStrings["FrameworxProjectDataBaseConnection"].ConnectionString);
        }

    }
}
