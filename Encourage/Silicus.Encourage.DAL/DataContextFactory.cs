﻿using Silicus.Encourage.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL
{
    public class DataContextFactory : IDataContextFactory
    {
        public ICommonDatabaseContext CreateCommonDbContext()
        {
            ICommonDatabaseContext dataContext = new CommonDatabaseContext(
                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            return dataContext;
        }
        public IEncourageDatabaseContext CreateEncourageDbContext()
        {
            IEncourageDatabaseContext dataContext = new EncourageDatabaseContext(
                ConfigurationManager.ConnectionStrings["EncourageDataBaseConnection"].ConnectionString);

            return dataContext;
        }
    }
}
