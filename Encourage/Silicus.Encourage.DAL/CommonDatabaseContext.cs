﻿using Silicus.Encourage.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL
{
    public class CommonDatabaseContext : DataContextBase, ICommonDatabaseContext
    {
        public CommonDatabaseContext(string connectionString)
            : base(connectionString)
        {
            
        }
        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

    }
}
