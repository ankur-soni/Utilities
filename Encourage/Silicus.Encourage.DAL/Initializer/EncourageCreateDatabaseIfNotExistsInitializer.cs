using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.Encourage.DAL.Initializer
{
    public class EncourageCreateDatabaseIfNotExistsInitializer : CreateDatabaseIfNotExists<EncourageDatabaseContext>
    {
        protected override void Seed(EncourageDatabaseContext context)
        {
            new BaseDatabaseInitializer().Seed(context);
        }
    }
}
