using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.UtilityContainerr.Entities.DatabaseInitializer
{
    public class BaseDataBaseInitializer : CreateDatabaseIfNotExists<LocalDataBaseContext>
    {
    }
}
